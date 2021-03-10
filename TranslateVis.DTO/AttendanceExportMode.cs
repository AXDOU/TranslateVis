using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.DTO
{
    public class AttendanceExportMode
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
        /// 考勤日期
        /// </summary>
        public DateTime AttendanceDate { get; set; }

        /// <summary>
        /// 考勤类型 0 上班   1 请假  
        /// </summary>
        public int AttendancedType { get; set; }

        /// <summary>
        /// 考勤总分
        /// </summary>
        public decimal AttendancedScore { get; set; }

        /// <summary>
        /// 纪律总分
        /// </summary>
        public decimal AScoreSum { get; set; }


        /// <summary>
        /// 日志进度总分
        /// </summary>
        public decimal BScoreSum { get; set; }


        /// <summary>
        /// 工作态度总分
        /// </summary>
        public decimal CScoreSum { get; set; }


        /// <summary>
        /// 工作任务总分
        /// </summary>
        public decimal DScoreSum { get; set; }


        /// <summary>
        /// 考勤备注
        /// </summary>
        public string AttendanceRemark { get; set; }

        /// <summary>
        /// 考勤统计天数(取最大值)
        /// </summary>
        public int AttndanceDays { get; set; }

    }
}
