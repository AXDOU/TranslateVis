using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.DTO
{
    /// <summary>
    /// 用户考核分值详情
    /// </summary>
    public class AttemdanceDetailOutput
    {
        ///<summary>
        ///
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        ///纪律分 1分
        ///</summary>
        public decimal AScore { get; set; }

        ///<summary>
        ///日志及进度更新 1分
        ///</summary>
        public decimal BScore { get; set; }

        ///<summary>
        ///工作态度 3分
        ///</summary>
        public decimal CScore { get; set; }


        ///<summary>
        ///工作任务 5分
        ///</summary>
        public decimal DScore { get; set; }

        ///<summary>
        ///考勤得分
        ///</summary>
        public decimal Score { get; set; }


        /// <summary>
        /// 出勤类型 0 上班  1 请假 
        /// </summary>
        public int AttendanceType { get; set; }


        /// <summary>
        /// 出勤日期
        /// </summary>
        public DateTime AttendanceDate { get; set; }

        ///<summary>
        ///
        ///</summary>
        public int UserId { get; set; }


        ///<summary>
        ///用户名称
        ///</summary>
        public string LinkMan { get; set; }

        ///<summary>
        ///说明
        ///</summary>
        public string Remark { get; set; }

    }
}
