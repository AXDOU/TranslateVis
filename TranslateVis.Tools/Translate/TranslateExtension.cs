using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranslateVis.Tools.Translate;

namespace TranslateVis.Tools
{

    //eg: http://api.fanyi.baidu.com/api/trans/vip/translate?q=apple&from=en&to=zh&appid=2015063000000001&salt=1435660288&sign=f89f9594663708c1605f3d736d01d2d4
    /// <summary>
    /// 通用翻译API
    /// </summary>
    public static class TranslateExtension
    {
        #region[   百度通用翻译API  ]
        //private static string appId = "20210207000692830";//应用Id
        //private static string secretkey = "4KxbllaoslURKvfqMS68";//密钥
        private static string baiduAPI = "http://api.fanyi.baidu.com/api/trans/vip/translate";//http


        private static string appId = "20210207000692830";//应用Id
        private static string secretkey = "4KxbllaoslURKvfqMS68";//密钥
        //private static string baiduAPIHttps = "https://fanyi-api.baidu.com/api/trans/vip/translate";   //https
        #endregion

        #region [  签名sign   ]
        /****
         * 生成签名sign：
         * Step1. 拼接字符串1：
         * 拼接appid=2015063000000001+q=apple+salt=1435660288+密钥=12345678得到字符串1：“2015063000000001apple143566028812345678”
         * Step2. 计算签名：（对字符串1做md5加密）
         * sign=md5(2015063000000001apple143566028812345678)，得到sign=f89f9594663708c1605f3d736d01d2d4
         */

        /// <summary>
        /// 拼接生成sign
        /// </summary>
        /// <param name="query"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string getSign(string query, string salt)
        {
            string sign = MD5Encryption.Encryption($"{appId}{query}{salt}{secretkey}");
            return sign;
        }
        #endregion


        #region[ 英汉互译 ]

        /// <summary>
        /// 英译汉
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static string ConvertChinese(string q)
        {
            return TranslateHttpGet(q, "en", "zh");
        }

        /// <summary>
        /// 汉译英
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static string ConvertEnglish(string q)
        {
            return TranslateHttpGet(q, "zh", "en");
        }

        #endregion

        //get请求
        /// <summary>
        /// 字符串翻译
        /// </summary>
        /// <param name="q">翻译的字符串</param>
        /// <param name="from">源语言</param>
        /// <param name="to">目标语言</param>
        /// <returns></returns>
        public static string TranslateHttpGet(string q, string from, string to)
        {
            string salt = GetRandom();
            string uri = $"{baiduAPI}?q={q}&from={from}&to={to}&appid={appId}&salt={salt}&sign={getSign(q, salt)}";

            var resultJson = JsonConvert.DeserializeObject<BaiduTranslateJson>(new HttpExtension().Get(uri));

            string unicode = string.Empty;
            if (resultJson != null && resultJson.trans_result != null && resultJson.trans_result.Count > 0)
            {
                BaiduTranslateResult json = resultJson.trans_result.FirstOrDefault(x => x.src == q);
                unicode = Unicode2String(json?.dst);
            }
            return unicode;
        }


        public static async Task<string> PostWebAsync(string q, string from, string to)
        {
            var request = (HttpWebRequest)WebRequest.Create(baiduAPI);
            string salt = GetRandom();
            string idata = $"q={q}&from={from}&to={to}&appid={appId}&salt={salt}&sign={getSign(q, salt)}";
            var data = Encoding.ASCII.GetBytes(idata);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = await request.GetRequestStreamAsync())
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            var response = (HttpWebResponse)await request.GetResponseAsync();

            var r = new StreamReader(response.GetResponseStream());
            //    System.Windows.MessageBox.Show(await r.ReadToEndAsync());
            string res =  await r.ReadToEndAsync();
            var resultJson = JsonConvert.DeserializeObject<BaiduTranslateJson>(res);
            string unicode = string.Empty;
            if (resultJson != null && resultJson.trans_result != null && resultJson.trans_result.Count > 0)
            {
                BaiduTranslateResult json = resultJson.trans_result.FirstOrDefault(x => x.src == q);
                unicode = Unicode2String(json?.dst);
            }
            return unicode;
        }

        //post请求
        /// <summary>
        /// 字符串翻译
        /// </summary>
        /// <param name="q">翻译的字符串</param>
        /// <param name="from">源语言</param>
        /// <param name="to">目标语言</param>
        /// <returns></returns>
        public static string TranslateHttpPost(string q, string from, string to)
        {
            string salt = GetRandom();
            string uri = $"q={q}&from={from}&to={to}&appid={appId}&salt={salt}&sign={getSign(q, salt)}";
            var resultJson = JsonConvert.DeserializeObject<BaiduTranslateJson>(new HttpExtension().Post(baiduAPI, uri));

            string unicode = string.Empty;
            if (resultJson != null && resultJson.trans_result != null && resultJson.trans_result.Count > 0)
            {
                BaiduTranslateResult json = resultJson.trans_result.FirstOrDefault(x => x.src == q);
                unicode = Unicode2String(json?.dst);
            }
            return unicode;
        }


        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns></returns>
        private static string GetRandom()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }

        

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string _sourceStr)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                _sourceStr, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
    }
}
