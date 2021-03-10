using System;
using System.Collections.Generic;

namespace TranslateVis.DTO
{
    public class AttendanceStatisticsOutput
    {
        /// <summary>
        /// 所属用户
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 是否离职
        /// </summary>
        public bool IsNotIncumbency { get; set; }


        /// <summary>
        /// 应出勤天数
        /// </summary>
        public decimal OnDutyDays { get; set; }

        /// <summary>
        /// 实际出勤天数
        /// </summary>
        public decimal ActualWorkingDays { get; set; }


        /// <summary>
        /// 实际出勤天数
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 请假次数
        /// </summary>
        public decimal MissedCount { get; set; }

        /// <summary>
        /// 出勤详情
        /// </summary>
        public List<WorkingStatistics> Statistics = new List<WorkingStatistics>();
    }

    /// <summary>
    /// 考勤统计(子类)
    /// </summary>
    public class WorkingStatistics
    {

        /// <summary>
        /// 评分信息Id
        /// </summary>
        public int AttendancedId { get; set; }

        /// <summary>
        /// 考勤日期
        /// </summary>
        public DateTime AttendanceDate { get; set; }

        /// <summary>
        /// 考勤类型 0 上班   1 请假  
        /// </summary>
        public int AttendancedType { get; set; }

        /// <summary>
        /// 考勤得分
        /// </summary>
        public decimal AttendancedScore { get; set; }


        /// <summary>
        /// 评分类型  
        /// </summary>
        public string ActualType { get; set; }

        /// <summary>
        /// 评估工时(天)
        /// </summary>
        public int WorkingDay { get; set; }

        /// <summary>
        /// 扣分项
        /// </summary>
        public int MissedDay { get; set; }

        /// <summary>
        /// 开始打卡时间
        /// </summary>
        //public DateTime StartPunchTime { get; set; }

        /// <summary>
        /// 结束打卡时间
        /// </summary>
        //public DateTime EndPunchTime { get; set; }

        /// <summary>
        /// 考勤信息
        /// </summary>
        public string AttendanceTag { get; set; }

        /// <summary>
        /// 考勤备注
        /// </summary>
        public string AttendanceRemark { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// 考勤标题
    /// </summary>
    public class AttendanceTitle
    {

        /// <summary>
        /// 考勤日期
        /// </summary>
        public DateTime AttendanceDate { get; set; }

        /// <summary>
        /// 周几
        /// </summary>
        public string WeekDay { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
    }
}
