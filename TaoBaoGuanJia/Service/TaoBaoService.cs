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
using System.Threading.Tasks;
using System.Web;
using TaoBaoGuanJia.Core.TaoBao;
using TaoBaoGuanJia.Extension;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;

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
        public void ImportProduct(string url)
        {
            Uri u = new Uri(url);
            string path = u.AbsolutePath;
            ControlsUtils.OperationLog("识别地址...");
            if (path.IndexOf("item.htm") > -1)
            {
                //详情
                string id = Utils.GetId(url);
                if (!string.IsNullOrEmpty(id))
                {
                    new Task(() =>
                    {

                        CollectToDB(id);

                    }).Start();

                }
            }
            else {
                ControlsUtils.Msg("请填写正确的淘宝详情链接！");
            }
            //else if (path.IndexOf("search") > -1)
            //{
            //   string content= Utils.SendWebRequest(url);
            //    //列表
            //    List<string> list = Utils.GetMatchList(content, "detail_url\":\"(.*?)\"");


            //    if (list != null && list.Count > 0)
            //    {
            //        MaxVal = list.Count;
            //        CurVal = 0;
            //        foreach (var item in list)
            //        {
            //            string infoUrl = HttpUtility.HtmlDecode(item);
            //            string id = Utils.GetId(infoUrl);
            //            if (!string.IsNullOrEmpty(id))
            //            {
            //                new Task(() =>
            //                {
            //                    CollectToDB(id);
            //                    CurVal++;
            //                    ControlsUtils.SetProgressBarValue(MaxVal, CurVal);
            //                }).Start();
            //            }
            //        }

            //    }

            //}
            //else if (path.IndexOf(".taobao.com") > -1)
            //{
            //    //列表
            //}
        }

        /// <summary>
        /// 采集入库
        /// </summary>
        /// <param name="productId"></param>
        public void CollectToDB(string productId)
        {
            ControlsUtils.OperationLog("开始采集宝贝(" + productId + ")");
            string onlineKey = productId.ToString();
            ItemGetResponse itemGetResponse = new TaoBaoWebService().GetItemGetMobileResponseByOnlinekeyNew(onlineKey, 0);
            DownloadDetailTaobaoService taoBaoService = new DownloadDetailTaobaoService();
            DownloadItemInfoViewEntity viewEntity = new DownloadItemInfoViewEntity();
            viewEntity.SourceShop = new Sys_shop() { Sysid = 1 };
            if (itemGetResponse.Item != null)
            {
                ControlsUtils.OperationLog("宝贝(" + itemGetResponse.Item.Title + ")信息获取成功");
                taoBaoService.Download(itemGetResponse, viewEntity, onlineKey);
            }
            else
            {
                ControlsUtils.OperationLog("宝贝(" + productId + ")信息获取失败 " + itemGetResponse.ErrMsg);
            }

            ControlsUtils.OperationLog("宝贝(" + productId + ")采集完毕！");
            ControlsUtils.RefreshDataGridViewMaster();

        }
        public static object obj = new object();
        public static int MaxVal = 0;
        public static int CurVal = 0;
        /// <summary>
        /// 批量导出淘宝助理文件
        /// </summary>
        public void ExportToCsv(List<string> itemList)
        {
            ControlsUtils.OperationLog("开始生成淘宝助理CSV格式数据文件");
            string timeStamp = Utils.GetTimeStamp();
            //csv保存路径
            string csvPath = Path.Combine(ConfigHelper.GetCsvPath(), timeStamp + ".csv");
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
            List<ProductItem> list = DataHelper.GetProductItemList(string.Join(",", itemList.ToArray()));
            MaxVal = list.Count;
            CurVal = 0;
            //切割10份开一个线程
            List<E_Range> rangeList = Utils.PrepareData(0, list.Count, 2);
            Task[] tasks = new Task[rangeList.Count];
            for (int i = 0; i < rangeList.Count; i++)
            {
                E_Range item = rangeList[i];
                tasks[i] = Task.Factory.StartNew(() =>
                 {
                     for (int j = item.begin - 1; j < item.end; j++)
                     {
                         ProductItem productItem = list[j];
                         ControlsUtils.OperationLog("处理宝贝主体信息：" + productItem.Name);
                         string[] tempStr = export.ConvertProductToDic(productItem, contentPath);
                         lock (obj)
                         {
                             _outputList.Add(tempStr);
                         }
                         CurVal++;
                         ControlsUtils.OperationLog("宝贝处理完毕：" + productItem.Name);
                         ControlsUtils.SetProgressBarValue(MaxVal, CurVal);

                     }

                 });
            }
            Task.WaitAll(tasks);//等待线程执行完毕
            export.WriteDicToFile(csvPath, _outputList);
            ControlsUtils.SetProgressBarValue(list.Count, list.Count);
            ControlsUtils.OperationLog("淘宝助理CSV格式数据文件全部生成完毕！保存路径：" + csvPath);
            //for (int i = 0; i < list.Count; i++)
            //{

            //    ProductItem item = list[i];
            //    ControlsUtils.OperationLog("处理宝贝主体信息：" + item.Name);
            //    Task<string[]> taskResult = new Task<string[]>(() =>
            //    {
            //        return export.ConvertProductToDic(item, contentPath);
            //    });
            //    taskResult.Start();
            //    taskResult.Wait();
            //    _outputList.Add(taskResult.Result);
            //    ControlsUtils.OperationLog("宝贝处理完毕：" + item.Name);
            //    ControlsUtils.SetProgressBarValue(list.Count, i);
            //}
            //export.WriteDicToFile(csvPath, _outputList);
            //ControlsUtils.SetProgressBarValue(list.Count, list.Count);
        }


    }
}
