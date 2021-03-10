using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.DAL.DataEntities
{
    [SugarTable("UserOfDepartment")]
    public class UserOfDepartment
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        ///<summary>
        ///
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        ///
        ///</summary>
        public int DepartmentId { get; set; }

        ///<summary>
        ///
        ///</summary>
        public int UserId { get; set; }
    }
}
