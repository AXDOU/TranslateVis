using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslateVis.DAL.DataBase;
using TranslateVis.DAL.DataEntities;
using TranslateVis.DTO;

namespace TranslateVis.Service
{
    public class UserService : Repository<UserInfo>
    {
        public List<UserInfo> GetUsers(string keyword, int department)
        {
            if (department == 0) return base.GetList(x => x.LinkMan.Contains(keyword));
            return base.GetList(x => x.LinkMan.Contains(keyword) && x.DepartmentId == department);
        }

        public  async Task<OutPut<bool>> CreateAsync(UserInfo userInfo,string password)
        {
            OutPut<bool> outPut = new OutPut<bool>();
            var userList = base.GetList(x => x.LoginName == userInfo.LoginName);
            if(userList != null && userList.Count > 0)
            {
                outPut.Succeeded = false;
                outPut.Errors = new List<ErrorModel>
                {
                    new ErrorModel{ Description = "用户名不可重复！"}
                };
                return outPut;
            }
            outPut.Succeeded = await base.InsertAsync(userInfo);
            return outPut;
        }

        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="isRememberMe"></param>
        /// <returns></returns>
        public async Task<bool> PasswordSignInAsync(string loginName,string password,bool isRememberMe)
        {
            var userInfo = base.GetSingle(x => x.LoginName == loginName);
            if (userInfo == null) return false;
            else
            {
               return userInfo.Password == password;
            }
        }

    }
}
