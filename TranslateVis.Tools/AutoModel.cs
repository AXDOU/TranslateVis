using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using System.Linq;

namespace TranslateVis.Tools
{
    public static class AutoEntityExtension
    {

        /// <summary>
        /// 自动生成实体
        /// </summary>
        /// <param name="zhlist"></param>
        /// <returns></returns>
        public static string AutoToEntity(List<string> zhlist)
        {
            List<string> enlist = new List<string>();
            foreach (var str in zhlist)
            {
                var res = TranslateExtension.ConvertEnglish(str).ToHumpNaming();
                enlist.Add(res);
            }
            string builder = string.Empty;
            string space = "  ";
            builder += "///<sumary>" + Environment.NewLine;
            builder += "///" + Environment.NewLine;
            builder += "///</sumary>" + Environment.NewLine;
            builder += "public class Common" + Environment.NewLine;
            builder += "{" + Environment.NewLine;
            foreach (var str in zhlist)
            {
                builder += Environment.NewLine;
                builder += space + "/// <summary>" + Environment.NewLine;
                builder += space + "/// " + str + Environment.NewLine;
                builder += space + "/// </summary" + Environment.NewLine;
                builder += space + "public string " + enlist[zhlist.IndexOf(str)] + " {get;set;}" + Environment.NewLine;
            }
            builder += "}" + Environment.NewLine;
            return builder;
        }

        /// <summary>
        /// 格式化驼峰命名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ToHumpNaming(this string str)
        {
            List<string> strList = str.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            string humpStr = string.Empty;
            foreach(var strKey in strList)
            {
                humpStr += strKey.Substring(0, 1).ToUpper() + strKey.Substring(1,strKey.Length - 1);
            }
            return humpStr;
        }
    }
}
