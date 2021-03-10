using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Reflection;
using System.Data;
using System.Web;
using System.IO;

namespace TranslateVis.Tools
{
    public class HttpExtension
    {
        public CookieContainer cookies = new CookieContainer();
        
        public string Get(string url, string referer = "", int timeout = 2000, Encoding encode = null)
        {
            HttpWebResponse res = null;
            HttpWebRequest req = null;
            try
            {
                req = (HttpWebRequest)WebRequest.CreateHttp(url);
                req.CookieContainer = cookies;
                req.AllowAutoRedirect = false;
                req.Timeout = timeout;
                req.Referer = referer;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0;%20WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
                res = (HttpWebResponse)req.GetResponse();
                string data = new StreamReader(res.GetResponseStream(), encode ?? Encoding.UTF8).ReadToEnd();
                res.Close();
                req.Abort();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string Post(string url, string postdata, CookieContainer cookie = null, string referer = "", int timeout = 2000, Encoding encode = null)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            encode = encode ?? Encoding.UTF8;
            string html = string.Empty;
            try
            {
                byte[] byteArray = encode.GetBytes(postdata);
                req = (HttpWebRequest)WebRequest.CreateHttp(new Uri(url));
                if (cookie == null) cookie = new CookieContainer();
                req.CookieContainer = cookie;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; InfoPath.1)";
                req.Method = "POST";
                req.Referer = referer;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = byteArray.Length;
                req.Timeout = timeout;
                Stream newStream = req.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                res = (HttpWebResponse)req.GetResponse();
                cookie.Add(res.Cookies);
                StreamReader str = new StreamReader(res.GetResponseStream(), encode);
                html = str.ReadToEnd();
                return html;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
