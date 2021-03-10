using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranslateVis.DAL.DataBase;
using TranslateVis.DAL.DataEntities;

namespace TranslateVis.Service
{
    public class DepartmentService : Repository<Department>
    {

        /// <summary>
        /// 获取子级部门
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<string> GetDepartments(int parentId)
        {
            List<string> departments = base.GetList(x => x.PartentId == parentId).Select(x => x.DepartmentName).DefaultIfEmpty().ToList() ;
            return departments;
        }
    }
}
