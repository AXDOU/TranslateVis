using System;
using System.Collections.Generic;
using TranslateVis.DAL.DataBase;
using TranslateVis.DAL.DataEntities;

namespace TranslateVis.Service
{
    public class UserService : Repository<UserInfo>
    {
        public List<UserInfo> GetUsers(string keyword, int department)
        {
            if (department == 0) return base.GetList(x => x.LinkMan.Contains(keyword));
            return base.GetList(x => x.LinkMan.Contains(keyword) && x.DepartmentId == department);
        }
    }
}
