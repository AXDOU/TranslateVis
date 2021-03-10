using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TranslateVis.Tools
{
    public static class StringExtension
    {

        public static string ToDateString<T>(this T data, string format = "yyyy-MM-dd")
        {
            if (data.GetType() == typeof(DateTime))
            {
                return Convert.ToDateTime(data.ToString()).ToString(format);
            }
            else if (data.GetType() == typeof(DateTime?))
            {
                if (data == null || string.IsNullOrWhiteSpace(data.ToString())) return string.Empty;
                return Convert.ToDateTime(data).ToString(format);
            }
            return string.Empty;
        }

        /// <summary>
        /// 默认HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatTime<T>(this T date, string format = "HH:mm")
        {
            if (date == null) return string.Empty;
            else if (date.GetType() == typeof(DateTime)) return Convert.ToDateTime(date).ToString(format);
            DateTime.TryParse(date.ToString(), out DateTime outdate);
            return outdate.ToString(format);
        }


        /// <summary>
        /// 判断两个值是否等于的扩展方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsTo(this object obj, object value)
        {
            if (obj == null && value == null) return true;
            else if (obj != null)
            {
                return obj.Equals(value);
            }
            else if (value != null)
            {
                return value.Equals(obj);
            }
            return obj == value;
        }

        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToWeekOfDay<T>(this T data)
        {
            if (data.GetType() == typeof(DateTime))
            {
                DateTime dateTime = Convert.ToDateTime(data.ToString());
                return DayOfWeekToString((int)dateTime.DayOfWeek);
            }
            return string.Empty;
        }


        /// <summary>
        /// 转换数字
        /// </summary>
        public static string DayOfWeekToString(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return "一";
                case 2: return "二";
                case 3: return "三";
                case 4: return "四";
                case 5: return "五";
                case 6: return "六";
                case 0: return "日";
                default: return "";
            }
        }

        /// <summary>
        /// 判断是否为整数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsInteger<T>(this T data)
        {
            if (data.GetType() == typeof(Int32))
                return true;
            try
            {
                string pattern = @"^-?\d+$";
                return Regex.IsMatch(data.ToString(), pattern);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 截取拼接指定长度的字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string SubStr(this string str, int length, string extension = "...")
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            else if (length > str.Length) return str.Substring(0, str.Length);
            else return str.Substring(0, length) + extension;
        }
    }
}
