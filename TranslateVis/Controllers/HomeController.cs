using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslateVis.Models;
using TranslateVis.Service;
using TranslateVis.Tools;

namespace TranslateVis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserService userService = new UserService();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpExtension httpExtension = new HttpExtension();
            //1.运行示例
            //var res = httpExtension.Get("http://api.fanyi.baidu.com/api/trans/vip/translate?q=apple&from=en&to=zh&appid=2015063000000001&salt=1435660288&sign=f89f9594663708c1605f3d736d01d2d4");


            //2.实例翻译
            //TranslateExtension translate = new TranslateExtension();
            //var res2 = translate.ConvertChinese("hello");
            //var res3 = translate.ConvertChinese("love");

            //3.扩展
            //01 支持字符串转实体
            //post 
            //List<string> zhlist = "Id,名称,年龄".Split(',').ToList();
            //string builder = AutoEntityExtension.AutoToEntity(zhlist);

            //02 支持库表转实体

            //03 支持生成SQL建表语句

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult ConvertEntity(string key)
        {
            string builder = AutoToEntity(key);
            return Json(new { code = 200, result = builder });
        }

        [HttpGet]
        public FileResult ExportEntity(string key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(AutoToEntity(key));

            byte[] fileContents = Encoding.Default.GetBytes(stringBuilder.ToString());
            return File(fileContents, "application/ms-txt", "Entity.txt");
        }


        [HttpGet]
        public FileResult ExportSchedule(DateTime startTime, DateTime endTime, string startName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(AutoToSchedule(startTime, endTime, startName));

            byte[] fileContents = Encoding.Default.GetBytes(stringBuilder.ToString());
            return File(fileContents, "application/ms-txt", "Entity.txt");
        }

        /// <summary>
        /// 自动生成实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string AutoToEntity(string key)
        {
            List<string> list = key.Split(',').ToList();
            string builder = AutoEntityExtension.AutoToEntity(list);
            return builder;
        }


        private string AutoToSchedule(DateTime startTime, DateTime endTime, string startName)
        {
            Dictionary<string, string> linkMethodDic = new Dictionary<string, string>();
            List<ScheduleUser> scheduleUsers = new List<ScheduleUser>
            {
                new ScheduleUser { Sort = 1, LinkMan = "何燕", LinkTel = "18398102490" },
                new ScheduleUser { Sort = 2, LinkMan = "郁鹏凯", LinkTel = "18398102490" },
                new ScheduleUser { Sort = 3, LinkMan = "黄香风", LinkTel = "18398102490" },
                new ScheduleUser { Sort = 4, LinkMan = "姜瑞斌", LinkTel = "18398102490" },
                new ScheduleUser { Sort = 5, LinkMan = "查波得", LinkTel = "18398102490" },
                new ScheduleUser { Sort = 6, LinkMan = "赵刚刚", LinkTel = "18398102490" }
            };
            List<ScheduleUser> sortScheduleUsers = new List<ScheduleUser>();
            var startScheduleUser = scheduleUsers.FirstOrDefault(x => x.LinkMan == startName);
            if (startScheduleUser != null && startScheduleUser.Sort != 1)
            {
                int  reduceSortBy = scheduleUsers.Where(x => x.Sort < startScheduleUser.Sort).Count();  //比当前用户Sort小的增sortBy，比当前用户Sort大的减去sortBy
                int plusSortBy = scheduleUsers.Where(x => x.Sort >= startScheduleUser.Sort).Count();
                foreach (var scheduleUser in scheduleUsers)
                {
                    ScheduleUser schedule = new ScheduleUser
                    {
                        LinkMan = scheduleUser.LinkMan,
                        LinkTel = scheduleUser.LinkTel,
                        Sort = scheduleUser.Sort
                    };
                    if (scheduleUser.Sort < startScheduleUser.Sort)
                    {
                        schedule.Sort += plusSortBy;
                    }
                    else
                    {
                        schedule.Sort -= reduceSortBy;
                    }
                    sortScheduleUsers.Add(schedule);
                };
            }
            else
            {
                sortScheduleUsers.AddRange(scheduleUsers);
            }


            List<DateTime> scheduleDates = GetScheduleDate(startTime, endTime);
            List<OnlineSchedule> schedules = new List<OnlineSchedule>();

            int count = 0;
            for (var i = 1; i <= scheduleDates.Count; i++)
            {
                int sort = (i % 6 == 0 ? 6 : i % 6);
                var scheduleUserInfo = sortScheduleUsers.FirstOrDefault(x => x.Sort == sort);
                OnlineSchedule online = CreateSchedult(count, scheduleUserInfo.LinkMan, scheduleUserInfo.LinkTel, scheduleDates[i - 1]);
                schedules.Add(online);
            }

            string builder = $"产品组{startTime.Year} 年{startTime.ToString("yyyy-MM-dd")} 至 {endTime.ToString("yyyy-MM-dd")}线上值班表{Environment.NewLine}";
            foreach (var schedule in schedules)
            {
                builder += $"{schedule.LinkMan} {schedule.LinkTel} {schedule.OnlineDate.ToString("yyyy-MM-dd")} {Environment.NewLine}";
            }
            return builder;
        }

        private OnlineSchedule CreateSchedult(int sort, string linkMan, string linkTel, DateTime dateTime)
        {
            OnlineSchedule onlineSchedule = new OnlineSchedule
            {
                Sort = 1,
                LinkMan = linkMan,
                LinkTel = linkTel,
                OnlineDate = dateTime
            };
            return onlineSchedule;
        }

        private List<DateTime> GetScheduleDate(DateTime startTime, DateTime endTime)
        {
            int totalDays = (endTime - startTime).Days;
            List<DateTime> scheduleDates = new List<DateTime>();
            for (var i = 0; i < totalDays; i++)
            {
                DateTime thisDate = startTime.AddDays(i);
                if (thisDate.DayOfWeek.Equals(DayOfWeek.Saturday) || thisDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    scheduleDates.Add(thisDate);
                }
            }
            return scheduleDates.OrderBy(x => x).ToList();
        }
    }

    public class ScheduleUser
    {
        public int Sort { get; set; }
        public string LinkMan { get; set; }

        public string LinkTel { get; set; }
    }

    public class OnlineSchedule
    {
        public int Sort { get; set; }

        public string LinkMan { get; set; }

        public string LinkTel { get; set; }

        public DateTime OnlineDate { get; set; }
    }
}
