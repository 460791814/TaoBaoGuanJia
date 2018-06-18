using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        public void CollectToDB(string productId)
        {
            string onlineKey = productId.ToString();
            ItemGetResponse itemGetResponse = new TaoBaoWebService().GetItemGetMobileResponseByOnlinekeyNew(onlineKey, 0);
            DownloadDetailTaobaoService taoBaoService = new DownloadDetailTaobaoService();
            DownloadItemInfoViewEntity viewEntity = new DownloadItemInfoViewEntity();
            viewEntity.SourceShop = new Sys_shop() { Sysid=1 };
         
            taoBaoService.Download(itemGetResponse, viewEntity, onlineKey);


        }
        /// <summary>
        /// 批量导出淘宝助理文件
        /// </summary>
        public void ExportToCsv(List<string> itemList)
        {
            string timeStamp = Utils.GetTimeStamp();
            //csv保存路径
            string csvPath = Path.Combine(ConfigHelper.GetCsvPath() , timeStamp + ".csv");
            //图片保存路径
            string contentPath = Path.Combine(ConfigHelper.GetCsvPath(), timeStamp);
            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }
            TaoBaoExport export = new TaoBaoExport();
            List<string[]> _outputList = new List<string[]>();
            _outputList.Add(new string[] { "version 1.00" });
            _outputList.Add(ConfigHelper.TaoBaoHeaderFieldRow);
            _outputList.Add(ConfigHelper.TaoBaoHeaderRow);
            List<ProductItem> list = DataHelper.GetProductItemList(string.Join(",",itemList.ToArray()));
            foreach (var item in list)
            {
                _outputList.Add(export.ConvertProductToDic(item, contentPath));
            }
           
    
            export.WriteDicToFile(csvPath, _outputList);
        }

    }
}
