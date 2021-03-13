using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranslateVis.DAL.DataBase;
using TranslateVis.DAL.DataEntities;
using TranslateVis.DTO;

namespace TranslateVis.Service
{
    public class DepartmentService : Repository<Department>
    {

        /// <summary>
        /// 获取子级部门
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<FieldOutput> GetDepartments(int parentId)
        {
            List<FieldOutput> departments = base.GetList(x => x.PartentId == parentId).Select(x => new FieldOutput { Value = x.Id, Text = x.DepartmentName }).DefaultIfEmpty().ToList();
            return departments;
        }
    }
}
