﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using TaoBaoGuanJia.Core.TaoBao;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;
using Top.Api.Domain;
using Top.Api.Response;

namespace TaoBaoGuanJia.Service
{
    public class TaoBaoService
    {
        private bool IsSnatchSellPoint
        {
            get
            {
                return false;
              //  return DataConvert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", base.ToolCode.ToUpper(), "SnatchSellPoint", "false"));
            }
        }

        /// <summary>
        /// 采集入库
        /// </summary>
        /// <param name="productId"></param>
        public void CollectToDB(int productId)
        {
            string onlineKey = productId.ToString();
            ItemGetResponse itemGetResponse = new TaoBaoWebService().GetItemGetMobileResponseByOnlinekeyNew(onlineKey, 0);
           
        }
        /// <summary>
        /// 批量导出淘宝助理文件
        /// </summary>
        public void ExportToCsv(List<int> itemList)
        {

        }

    }
}
