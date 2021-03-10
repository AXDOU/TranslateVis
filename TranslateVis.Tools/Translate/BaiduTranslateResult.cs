using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.Tools.Translate
{
    /// <summary>
    /// 百度通用翻译返回信息
    /// </summary>
    public class BaiduTranslateJson
    {
        /// <summary>
        /// 源语言
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 目标语言
        /// </summary>
        public string to { get; set; }

        /// <summary>
        /// 翻译结果
        /// </summary>
        public List<BaiduTranslateResult> trans_result = new List<BaiduTranslateResult>();
    }

    /// <summary>
    /// 百度翻译结果json
    /// </summary>
    public class BaiduTranslateResult
    {
        /// <summary>
        /// 来源
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 翻译结果
        /// </summary>
        public string dst { get; set; }
    }
}
