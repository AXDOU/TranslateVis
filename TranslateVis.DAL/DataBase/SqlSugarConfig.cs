using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.DAL.DataBase
{
    /// <summary>
    /// SqlSugar 数据库连接配置
    /// </summary>
    public static class SqlSugarConfig
    {
        public static string SQLConnString = "Server=.;database=scoredb;user=sa;password=sa123";
        public static SqlSugarClient SugarClient { get { return SetSugarClient(); } }

        private static SqlSugarClient SetSugarClient()
        {
            //创建数据库对象
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = SQLConnString,//连接符字串
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute//从特性读取主键自增信息
            });
            return db;
        }
    }
}
