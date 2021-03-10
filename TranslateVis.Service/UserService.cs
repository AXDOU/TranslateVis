using System;
using System.Collections.Generic;
using TranslateVis.DAL.DataBase;
using TranslateVis.DAL.DataEntities;

namespace TranslateVis.Service
{
    public class UserService : Repository<UserInfo>
    {
        public List<UserInfo> GetUsers(string keyword)
        {
            return base.GetList(x => x.LinkMan.Contains(keyword));
        }
    }
}
