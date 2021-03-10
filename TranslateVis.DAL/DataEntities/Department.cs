using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.DAL.DataEntities
{
    [SugarTable("Department")]
    public class Department
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        ///<summary>
        ///
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string DepartmentName { get; set; }

        ///<summary>
        ///
        ///</summary>
        public int PartentId { get; set; }
    }
}
