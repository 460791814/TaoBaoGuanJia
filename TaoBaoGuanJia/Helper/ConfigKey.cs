using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoGuanJia.Helper
{
    /// <summary>
    /// 用户设置key
    /// </summary>
   public class ConfigKey
    {
        /// <summary>
        /// 是否需要下载手机详情
        /// </summary>
        public static readonly string IsExportMobileDesc = "isexportmobiledesc";
        /// <summary>
        /// 是否采用促销价格
        /// </summary>
        public static readonly string taobao_csv_saleprice = "taobao_csv_saleprice";
    }
}
