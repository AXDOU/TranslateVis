using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using TranslateVis.DAL.DataEntities;
using TranslateVis.DTO;
using TranslateVis.Service;
using TranslateVis.Tools;

namespace TranslateVis.Controllers
{
    public class ScoreController : Controller
    {
        private UserService userService = new UserService();
        private DepartmentService departmentService = new DepartmentService();
        private ScoreService scoreService = new ScoreService();
        private static int parentId = 1;//技术事业部Id
        // GET: ScoreController
        public ActionResult Index(string startTime = "", string endTime = "", string department = "", string keyword = "")
        {

            List<AttendanceStatisticsOutput> attendances = new List<AttendanceStatisticsOutput>();
            keyword = keyword ?? string.Empty;
            var userList = userService.GetUsers(keyword);
            if (!string.IsNullOrWhiteSpace(department))
            {
                userList = userList.Where(x => x.DepartmentName.Equals(department)).ToList();
            }
            DateTime startDate = DateTime.Now.Date.AddDays(-10), endDate = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                DateTime.TryParse(startTime, out startDate);
            }

            if (!string.IsNullOrWhiteSpace(endTime))
            {
                DateTime.TryParse(endTime, out endDate);
            }

            foreach (var userItem in userList)
            {
                var scoreList = scoreService.GetList(x => x.UserId == userItem.Id && x.AttendanceDate >= startDate && x.AttendanceDate <= endDate);
                AttendanceStatisticsOutput attendance = new AttendanceStatisticsOutput
                {
                    Department = userItem.DepartmentName,
                    FullName = userItem.LinkMan,
                    MissedCount = scoreList.Where(x => x.AttendanceType == 1).Count(),
                    OnDutyDays = scoreList.Count(),
                    ActualWorkingDays = scoreList.Where(x => x.AttendanceType == 0).Count(),
                    Score = scoreList.Select(x => x.Score).Sum(),
                    UserId = userItem.Id,
                    IsNotIncumbency = userItem.IsNotIncumbency
                };
                foreach (var scoreItem in scoreList)
                {
                    WorkingStatistics workingStatistic = new WorkingStatistics
                    {
                        AttendancedId = scoreItem.Id,
                        AttendanceTag = GetAttendanceTag(scoreItem),
                        AttendancedScore = scoreItem.AScore + scoreItem.BScore + scoreItem.CScore + scoreItem.DScore,
                        AttendancedType = scoreItem.AttendanceType,
                        AttendanceDate = scoreItem.AttendanceDate,
                        AttendanceRemark = scoreItem.Remark ?? string.Empty,
                        UserId = scoreItem.UserId,
                        ActualType = GetAttendanceType(scoreItem.AttendanceType),
                        MissedDay = scoreItem.AttendanceType == 1 ? 1 : 0,
                        WorkingDay = scoreItem.AttendanceType == 0 ? 1 : 0,
                    };
                    attendance.Statistics.Add(workingStatistic);
                }
                attendances.Add(attendance);
            }
            ViewBag.startdate = startDate.ToString("yyyy-MM-dd");
            ViewBag.enddate = endDate.ToString("yyyy-MM-dd");
            ViewBag.department = department;
            ViewBag.keyword = keyword;
            ViewBag.titles = GetAttendancTitles(startDate, endDate.AddDays(-1));
            ViewBag.groups = departmentService.GetDepartments(parentId);
            return View(attendances);
        }

        public ActionResult Edit(int userId, DateTime attendanceddate, int id = 0)
        {
            ViewBag.id = id;
            AttemdanceDetailOutput userScore = new AttemdanceDetailOutput();
            if (id > 0)
            {
                userScore = scoreService.GetDetail(id);
            }
            else
            {
                var userInfo = userService.GetById(userId);
                userScore = new AttemdanceDetailOutput
                {
                    AttendanceDate = attendanceddate,
                    LinkMan = userInfo != null ? userInfo.LinkMan : "",
                    AScore = 0.0m,
                    BScore = 0.0m,
                    CScore = 0.0m,
                    DScore = 0.0m
                };
            }
            return View(userScore);
        }


        // GET: ScoreController/Details/5
        public ActionResult Details(int attendancedId)
        {
            var userScore = scoreService.GetDetail(attendancedId);
            ViewBag.Score = (userScore.AScore + userScore.BScore + userScore.CScore + userScore.DScore);
            return View(userScore);
        }


        // POST: ScoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(int id, IFormCollection collection)
        {
            try
            {
                AttemdanceDetailOutput attemdance = collection as AttemdanceDetailOutput;

                decimal aScore = Convert.ToDecimal(collection["AScore"].ToString());
                decimal bScore = Convert.ToDecimal(collection["BScore"].ToString());
                decimal cScore = Convert.ToDecimal(collection["CScore"].ToString());
                decimal dScore = Convert.ToDecimal(collection["DScore"].ToString());
                DateTime attendanceDate = Convert.ToDateTime(collection["AttendanceDate"].ToString());
                string remark = collection["Remark"].ToString();
                int userId = Convert.ToInt32(collection["UserId"].ToString());
                UserScore userScore = new UserScore
                {
                    Id = id,
                    Score = aScore + bScore + cScore + dScore,
                    AScore = aScore,
                    BScore = bScore,
                    CScore = cScore,
                    DScore = dScore,
                    AttendanceDate = attendanceDate,
                    AttendanceType = 0,
                    Remark = remark ?? string.Empty,
                    UserId = userId
                };
                if (userScore.Score > 10)
                {
                    return Json(new { code = 0, msg = "分值超出界限值，请验证后重试！" });
                }
                bool result = id > 0 ? scoreService.Update(userScore) : scoreService.Insert(userScore);
                return Json(new { code = 200, msg = "保存成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, msg = "网络错误，请重试！描述：" + ex.Message });
            }
        }


        public FileResult ExportExcel(string startTime = "", string endTime = "", string department = "", string keyword = "")
        {
            List<AttendanceExportMode> attendances = new List<AttendanceExportMode>();
            keyword = keyword ?? string.Empty;
            var userList = userService.GetUsers(keyword);
            if (!string.IsNullOrWhiteSpace(department))
            {
                userList = userList.Where(x => x.DepartmentName.Equals(department)).ToList();
            }
            DateTime startDate = DateTime.Now.Date.AddDays(-10), endDate = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                DateTime.TryParse(startTime, out startDate);
            }

            if (!string.IsNullOrWhiteSpace(endTime))
            {
                DateTime.TryParse(endTime, out endDate);
            }

            foreach (var userItem in userList)
            {
                var scoreList = scoreService.GetList(x => x.UserId == userItem.Id && x.AttendanceDate >= startDate && x.AttendanceDate <= endDate);
                AttendanceExportMode attendance = new AttendanceExportMode
                {
                    Department = userItem.DepartmentName,
                    FullName = userItem.LinkMan,
                    UserId = userItem.Id,
                    IsNotIncumbency = userItem.IsNotIncumbency,
                    AScoreSum = scoreList.Sum(x => x.AScore),
                    BScoreSum = scoreList.Sum(x => x.BScore),
                    CScoreSum = scoreList.Sum(x => x.CScore),
                    DScoreSum = scoreList.Sum(x => x.DScore),
                    AttendancedScore = scoreList.Sum(x => x.Score),
                    AttendanceRemark = ConvertStr(scoreList.Select(x => x.Remark).ToList()),
                    AttndanceDays = scoreList.Count()
                };
                attendances.Add(attendance);
            }
            //excel
            string dateOfWeek = $"{startDate.ToString("yyyy.MM.dd")}-{endDate.ToString("yyyy.MM.dd")}";
            return ToCustomExcel($"{dateOfWeek}周排期表", dateOfWeek, attendances);
        }

        public FileStreamResult ToCustomExcel(string dataTitle, string dateOfWeek, List<AttendanceExportMode> attendances)
        {
            DataTable dt = new DataTable();
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet(dataTitle);
            for (var i = 0; i < 7; i++)
            {
                sheet.SetColumnWidth(i, 13 * 256);
            }
            IFont font = workbook.CreateFont();
            font.FontHeight = 20 * 20;
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.WrapText = true;

            NPOI.SS.UserModel.IRow rowheader = sheet.CreateRow(0);
            rowheader.HeightInPoints = 20;

            int attendanceCount = attendances.Select(x => x.AttndanceDays).Max();
            List<string> titles = new List<string> { "姓名", $"纪律（{1 * attendanceCount}）", $"日志及进度（{1 * attendanceCount}）", $"工作态度（{3 * attendanceCount}）", $"工作任务（{ 5 * attendanceCount}）", $"总分（{10 * attendanceCount}）", "备注" };

            NPOI.SS.UserModel.IRow rowheader2 = sheet.CreateRow(1);

            //添加列名
            for (int i = 0; i < 7; i++)
            {
                if (i < 6)
                    rowheader.CreateCell(i).SetCellValue("周积分制度评分情况");
                else
                    rowheader.CreateCell(i).SetCellValue(dateOfWeek);
                rowheader2.CreateCell(i).SetCellValue(titles[i]);
            }

            int rowstart = 2;
            int rowsCount = attendances.Count, columnCount = 7;
            for (int i = 0; i < rowsCount; i++)
            {
                IRow row = sheet.CreateRow(i + rowstart);
                for (int j = 0; j < columnCount; j++)
                {
                    var rowAttendance = attendances[i];
                    string cellText = string.Empty;
                    switch (j)
                    {
                        case 0:
                            cellText = rowAttendance.FullName;
                            break;
                        case 1:
                            cellText = rowAttendance.AScoreSum.ToString();
                            break;
                        case 2:
                            cellText = rowAttendance.BScoreSum.ToString();
                            break;
                        case 3:
                            cellText = rowAttendance.CScoreSum.ToString();
                            break;
                        case 4:
                            cellText = rowAttendance.DScoreSum.ToString();
                            break;
                        case 5:
                            cellText = rowAttendance.AttendancedScore.ToString();
                            break;
                        case 6:
                            cellText = rowAttendance.AttendanceRemark;
                            break;
                        default:
                            break;
                    }
                    row.CreateCell(j).SetCellValue(cellText);
                }
            }
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 5));
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            string strdate = dataTitle + DateTime.Now.ToString("yyyyMMddHHmmss");//获取当前时间 
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }


        /// <summary>
        /// 获取日期格式表头标题
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<AttendanceTitle> GetAttendancTitles(DateTime startDate, DateTime endDate)
        {
            List<AttendanceTitle> titles = new List<AttendanceTitle>();
            for (var i = 0; i <= (endDate - startDate).Days; i++)
            {
                titles.Add(new AttendanceTitle
                {
                    AttendanceDate = startDate.AddDays(i),
                    Date = startDate.AddDays(i).ToString("dd"),
                    WeekDay = startDate.AddDays(i).ToWeekOfDay()
                });
            }
            return titles;
        }

        private string GetAttendanceType(int attendanceType)
        {
            switch (attendanceType)
            {
                case 0:
                    return "上班";
                case 1:
                    return "请假";
                default:
                    return "其他";
            }
        }

        /// <summary>
        /// 获取考勤分值
        /// </summary>
        /// <param name="userScore"></param>
        /// <returns></returns>
        private string GetAttendanceTag(UserScore userScore)
        {
            return $"纪律分 { userScore.AScore} 日志及进度 {userScore.BScore} 态度 {userScore.CScore} 工作任务{userScore.DScore} 备注 {userScore.Remark ?? string.Empty}";
        }

        public string ConvertStr(List<string> list)
        {
            if (list != null && list.Count > 0)
            {
                string str = string.Join(' ', list).ToString();
                return str;
            }
            return string.Empty;
        }

    }
}
