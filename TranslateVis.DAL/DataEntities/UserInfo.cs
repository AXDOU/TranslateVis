using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.DAL.DataEntities
{
    [SugarTable("UserInfo")]
    public class UserInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        ///<summary>
        ///
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string LinkMan { get; set; }

        ///<summary>
        ///
        ///</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否在职 0 否   1 是
        /// </summary>
        public bool IsNotIncumbency { get; set; }


        ///<summary>
        /// 部门ID
        ///</summary>
        public int DepartmentId { get; set; }
    }
}
