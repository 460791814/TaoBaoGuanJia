using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;
using Top.Api.Domain;
using Top.Api.Response;

namespace TaoBaoGuanJia.Core.TaoBao
{
    public class TaoBaoWebService
    {
        internal static readonly string STARTSTR = "needsplit:";

        private static readonly string CONNSTR1 = "!-!";

        private static readonly string CONNSTR2 = "|-|";
       
        public ItemGetResponse GetItemGetMobileResponseByOnlinekeyNew(string onlineKey, int fromType, bool snatchPromotionPrice = false)
        {

            ItemGetResponse val = null;
            try
            {
                string stringToEscape = "{\"itemNumId\":\"" + onlineKey + "\"}";
                stringToEscape = Uri.EscapeDataString(stringToEscape);
                string detailUrl = $"http://acs.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data="+stringToEscape;

                //String detailUrl = "http://acs.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data=%7B%22itemNumId%22%3A%22566940414408%22%7D";
                var response = Utils.SendWebRequest(detailUrl);
                ItemTaobaoEntity itemTaobaoEntity = null;
                ItemJsonEntity60 itemJsonEntity = null;
                itemJsonEntity = JsonConvert.DeserializeObject<ItemJsonEntity60>(response);
                string text = "onsale";
                text = GetOnline(response);
                string text2 = string.Empty;
                long num = 0L;
                if (itemJsonEntity != null)
                {
                    val = new ItemGetResponse();
                    itemTaobaoEntity = new ItemTaobaoEntity();
                    itemTaobaoEntity.Data = new Data();
                    itemTaobaoEntity.Data.ItemInfoModel = new ItemInfoModel();
                    itemTaobaoEntity.Data.ItemInfoModel.ItemId = itemJsonEntity.data.item.itemId;
                    itemTaobaoEntity.Data.ItemInfoModel.Title = itemJsonEntity.data.item.title;
                    itemTaobaoEntity.Data.ItemInfoModel.CategoryId = DataConvert.ToInt((object)itemJsonEntity.data.item.categoryId);
                    itemTaobaoEntity.Data.Seller = new Seller();
                    itemTaobaoEntity.Data.Seller.Nick = itemJsonEntity.data.seller.sellerNick;
                    itemTaobaoEntity.Data.Seller.ShopId = itemJsonEntity.data.seller.shopId;
                    if (itemJsonEntity.data != null && itemJsonEntity.data.apiStack != null && itemJsonEntity.data.apiStack.Count > 0)
                    {
                        itemTaobaoEntity.Data.ItemInfoModel.Location = GetLocationNew(itemJsonEntity.data.apiStack[0].value);
                    }
                    if (itemJsonEntity.data != null && itemJsonEntity.data.props != null && itemJsonEntity.data.props.groupProps != null && itemJsonEntity.data.props.groupProps.Count > 0)
                    {
                        List<Prop> list = new List<Prop>();
                        if (itemJsonEntity.data.props.groupProps[0].BaseInfo != null && itemJsonEntity.data.props.groupProps[0].BaseInfo.Count > 0)
                        {
                            foreach (Dictionary<string, string> item in itemJsonEntity.data.props.groupProps[0].BaseInfo)
                            {
                                foreach (string key in item.Keys)
                                {
                                    Prop prop = new Prop();
                                    prop.Name = key;
                                    prop.Value = item[key];
                                    list.Add(prop);
                                }
                            }
                        }
                        itemTaobaoEntity.Data.Props = list;
                    }
                    if (itemJsonEntity.data != null && itemJsonEntity.data.item != null && itemJsonEntity.data.item.images != null)
                    {
                        List<string> list2 = new List<string>();
                        foreach (string image in itemJsonEntity.data.item.images)
                        {
                            list2.Add(image);
                        }
                        itemTaobaoEntity.Data.ItemInfoModel.picsPath = list2;
                    }
                    if (itemJsonEntity.data != null && itemJsonEntity.data.item != null)
                    {
                        itemTaobaoEntity.Data.DescInfo = new DescInfo();
                        itemTaobaoEntity.Data.DescInfo.PcDescUrl = "http://hws.m.taobao.com/cache/wdesc/5.0?id=" + onlineKey;
                        itemTaobaoEntity.Data.DescInfo.H5DescUrl = "http://hws.m.taobao.com/cache/mdesc/5.0?id=" + onlineKey;
                    }
                    itemTaobaoEntity.Data.SkuModel = new SkuModel();
                    itemTaobaoEntity.Data.SkuModel.SkuProps = new List<SkuPropMobile>();
                    if (itemJsonEntity.data != null && itemJsonEntity.data.item != null && itemJsonEntity.data.skuBase != null && itemJsonEntity.data.skuBase.props != null)
                    {
                        foreach (PropsItem prop2 in itemJsonEntity.data.skuBase.props)
                        {
                            SkuPropMobile skuPropMobile = new SkuPropMobile();
                            skuPropMobile.PropName = prop2.name;
                            skuPropMobile.PropId = prop2.pid;
                            skuPropMobile.Values = new List<Value>();
                            foreach (ValuesItem value2 in prop2.values)
                            {
                                Value value = new Value();
                                value.Name = value2.name;
                                value.ValueId = value2.vid;
                                value.ImgUrl = value2.image;
                                skuPropMobile.Values.Add(value);
                            }
                            itemTaobaoEntity.Data.SkuModel.SkuProps.Add(skuPropMobile);
                        }
                    }
                    bool isTaobao = false;
                    if (itemJsonEntity.data != null && itemJsonEntity.data.item != null && itemJsonEntity.data.item.taobaoPcDescUrl.IndexOf("sellerType=C") >= 0)
                    {
                        isTaobao = true;
                    }
                    itemTaobaoEntity.SkuList = GetMobilePromoPriceSkuList(response, isTaobao, itemJsonEntity, ref text2, ref num, snatchPromotionPrice, itemJsonEntity.data);
                    List<Sku>.Enumerator enumerator6;
                    if (!snatchPromotionPrice)
                    {
                        string emptyPrice = string.Empty;
                        List<Sku> mobilePromoPriceSkuList = GetMobilePromoPriceSkuList(response, isTaobao, itemJsonEntity, ref emptyPrice, ref num, true, itemJsonEntity.data);
                        if (mobilePromoPriceSkuList != null && mobilePromoPriceSkuList.Count > 0)
                        {
                            enumerator6 = itemTaobaoEntity.SkuList.GetEnumerator();
                            try
                            {
                                while (enumerator6.MoveNext())
                                {
                                    Sku tmpSku = enumerator6.Current;
                                    List<Sku> list3 = mobilePromoPriceSkuList;
                                    Predicate<Sku> match = (Sku p) => p.SkuId == tmpSku.SkuId;
                                    Sku val2 = list3.Find(match);
                                    if (val2 != null)
                                    {
                                        tmpSku.Quantity = val2.Quantity;
                                    }
                                }
                            }
                            finally
                            {
                                ((IDisposable)enumerator6).Dispose();
                            }
                        }
                    }
                    text2 = GetMobilePrice(itemTaobaoEntity.SkuList, response, snatchPromotionPrice, text2, itemTaobaoEntity.Data.ItemInfoModel.ItemId);
                    num = GetMobileNums(itemTaobaoEntity.SkuList, response);
                    bool flag = false;
                    bool flag2 = true;// DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", _toolCode.ToUpper(), "SnacthFilter", "false"));
                    if (text != "onsale" && flag2)
                    {
                        val.ErrCode = "19";
                        val.ErrMsg = "未抓取到商品，该商品已删除或已下架";
                        return val;
                    }
                    if (text != "onsale")
                    {
                        if (itemTaobaoEntity != null && itemTaobaoEntity.SkuList != null && itemTaobaoEntity.SkuList.Count > 0)
                        {
                            long num2 = 0L;
                            enumerator6 = itemTaobaoEntity.SkuList.GetEnumerator();
                            try
                            {
                                while (enumerator6.MoveNext())
                                {
                                    Sku current6 = enumerator6.Current;
                                    current6.Quantity = 100L;
                                    num2 += current6.Quantity;
                                }
                            }
                            finally
                            {
                                ((IDisposable)enumerator6).Dispose();
                            }
                            num = num2;
                        }
                        else
                        {
                            num = 100L;
                        }
                    }
                    //旺旺过滤
                    //if (DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", _toolCode.ToUpper(), "WangWangFilter", "false")))
                    //{
                    //    IList<UserSetData> userSetDataList = UserSetDataService.GetUserSetDataList(1, 1);
                    //    if (userSetDataList != null && userSetDataList.Count > 0 && itemTaobaoEntity != null && itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.Seller != null)
                    //    {
                    //        string nick = itemTaobaoEntity.Data.Seller.Nick;
                    //        if (!string.IsNullOrEmpty(nick))
                    //        {
                    //            for (int i = 0; i < userSetDataList.Count; i++)
                    //            {
                    //                if (nick == userSetDataList[i].Name)
                    //                {
                    //                    return val;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    if (flag)
                    {
                        if (text != "onsale")
                        {
                            val.ErrCode = "19";
                            val.ErrMsg = "未抓取到商品，该商品已删除或已下架";
                        }
                        else
                        {
                            val.ErrCode = "18";
                            val.ErrMsg = "未抓取到商品，该商品有可能已删除或下架";
                        }
                        return val;
                    }
                    string title = string.Empty;
                    string text3 = string.Empty;
                    string text4 = string.Empty;
                    string text5 = string.Empty;
                    string wirelessDesc = string.Empty;
                    string text6 = string.Empty;
                    string empty = string.Empty;
                    string empty9 = string.Empty;
                    string empty10 = string.Empty;
                    string loctionStr = string.Empty;
                    string nick2 = string.Empty;
                    string empty11 = string.Empty;
                    string empty2 = string.Empty;
                    string empty12 = string.Empty;
                    string stuffStatus = string.Empty;
                    string freightPayer = string.Empty;
                    string postFee = string.Empty;
                    string expressFee = string.Empty;
                    string emsFee = string.Empty;
                    List<ItemImg> itemImgs = new List<ItemImg>();
                    List<PropImg> propImgs = new List<PropImg>();
                    IList<SellProInfo> sellProInfoList = new List<SellProInfo>();
                    List<Sku> list4 = null;
                    Dictionary<string, string> dicProNameAndProValue = null;
                    if (itemTaobaoEntity != null && itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.ItemInfoModel != null)
                    {
                        title = itemTaobaoEntity.Data.ItemInfoModel.Title;
                        text3 = DataConvert.ToString((object)itemTaobaoEntity.Data.ItemInfoModel.CategoryId);
                        loctionStr = itemTaobaoEntity.Data.ItemInfoModel.Location;
                        text4 = itemTaobaoEntity.Data.ItemInfoModel.ItemId;
                        freightPayer = itemTaobaoEntity.Data.ItemInfoModel.FreightPayer;
                        postFee = itemTaobaoEntity.Data.ItemInfoModel.PostFee;
                        expressFee = itemTaobaoEntity.Data.ItemInfoModel.ExpressFee;
                        emsFee = itemTaobaoEntity.Data.ItemInfoModel.EmsFee;
                        bool flag3 = itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "二手";
                        stuffStatus = ((!(itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "二手")) ? ((!(itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "个人闲置")) ? "new" : "unused") : "second");
                        if (itemTaobaoEntity.Data.Seller != null)
                        {
                            Seller seller = itemTaobaoEntity.Data.Seller;
                            string shopId = seller.ShopId;
                            nick2 = seller.Nick;
                            bool flag4 = itemTaobaoEntity.Data.ItemInfoModel.ItemTypeName == "tmall";
                        }
                        if (itemTaobaoEntity.Data.ItemInfoModel.picsPath != null && itemTaobaoEntity.Data.ItemInfoModel.picsPath.Count > 0)
                        {
                            itemImgs = GetItemImgList(itemTaobaoEntity.Data.ItemInfoModel.picsPath);
                        }
                        if (itemTaobaoEntity.Data.DescInfo != null && !string.IsNullOrEmpty(itemTaobaoEntity.Data.DescInfo.PcDescUrl))
                        {
                            empty = itemTaobaoEntity.Data.DescInfo.PcDescUrl;
                            string fullDescUrl = itemTaobaoEntity.Data.DescInfo.FullDescUrl;
                            text6 = itemTaobaoEntity.Data.DescInfo.H5DescUrl;
                            string briefDescUrl = itemTaobaoEntity.Data.DescInfo.BriefDescUrl;
                            text5 = GetpcDescContent(empty);
                            wirelessDesc = GetWirelessDescContent(text6, text5);
                        }
                        if (itemTaobaoEntity.Data.SkuModel != null && itemTaobaoEntity.Data.SkuModel.SkuProps != null && itemTaobaoEntity.Data.SkuModel.SkuProps.Count > 0)
                        {
                            List<SkuPropMobile> skuProps = itemTaobaoEntity.Data.SkuModel.SkuProps;
                            propImgs = GetMobileProImgList(skuProps, sellProInfoList);
                        }
                        if (itemTaobaoEntity.Data.Props != null && itemTaobaoEntity.Data.Props.Count > 0)
                        {
                            dicProNameAndProValue = GetMobileItemProperty(itemTaobaoEntity.Data.Props, ref empty2);
                        }
                        if (itemTaobaoEntity.SkuList != null && itemTaobaoEntity.SkuList.Count > 0)
                        {
                            list4 = itemTaobaoEntity.SkuList;
                        }
                    }
                    Item val3 = new Item();
                    val3.ApproveStatus = text;
                    val3.Cid = DataConvert.ToLong((object)text3);
                    val3.Title = title;
                    val3.Desc = text5;
                    val3.Nick = nick2;
                    val3.FreightPayer = freightPayer;
                    val3.ExpressFee = expressFee;
                    val3.EmsFee = emsFee;
                    val3.PostFee = postFee;
                    val3.DetailUrl = detailUrl;
                    val3.WirelessDesc = wirelessDesc;
                    val3.WapDetailUrl = text6;
                    val3.HasDiscount = false;
                    val3.HasInvoice = false;
                    val3.HasShowcase = true;
                    val3.HasWarranty = true;
                    val3.IsTiming = false;
                    val3.ItemImgs = itemImgs;
                    val3.NumIid = DataConvert.ToLong((object)text4);
                    val3.PostageId = 0L;
                    val3.PropImgs = propImgs;
                    string empty3 = string.Empty;
                    string empty4 = string.Empty;
                    string empty5 = string.Empty;
                    string empty6 = string.Empty;
                    if (fromType == 0)
                    {
                         GetPropertyStrForLocalSnatch(dicProNameAndProValue, sellProInfoList, text3, out empty3, out empty4, out empty5, out empty6);
                    }
                    else
                    {
                        GetPropertyStrForCloudSnatch(dicProNameAndProValue, sellProInfoList, out empty3, out empty4);
                    }
                    FoodSecurity foodSecurity = GetFoodSecurity(dicProNameAndProValue);
                    if (foodSecurity != null && (foodSecurity.PrdLicenseNo != null || foodSecurity.DesignCode != null || foodSecurity.Factory != null || foodSecurity.FactorySite != null || foodSecurity.Contact != null || foodSecurity.Mix != null || foodSecurity.PlanStorage != null || foodSecurity.Period != null || foodSecurity.FoodAdditive != "无" || foodSecurity.HealthProductNo != null || foodSecurity.Supplier != null))
                    {
                        string empty7 = string.Empty;
                        string empty8 = string.Empty;
                        FoodSecurityDate(onlineKey, ref empty7, ref empty8);
                        foodSecurity.ProductDateStart = empty7;
                        foodSecurity.ProductDateEnd = empty8;
                        FoodSecurity obj = foodSecurity;
                        DateTime dateTime = DateTime.Now;
                        dateTime = dateTime.Date;
                        obj.StockDateStart = dateTime.ToString("yyyy-MM-dd");
                        FoodSecurity obj2 = foodSecurity;
                        dateTime = DateTime.Now;
                        dateTime = dateTime.Date;
                        obj2.StockDateEnd = dateTime.ToString("yyyy-MM-dd");
                    }
                    val3.Props = empty3;
                    val3.PropsName = empty4;
                    val3.InputPids = empty5;
                    val3.InputStr = empty6;
                    val3.FoodSecurity = foodSecurity;
                    val3.SellPromise = true;
                    val3.StuffStatus = "new";
                    val3.Type = "fixed";
                    val3.ValidThru = 7L;
                    val3.Violation = false;
                    val3.Barcode = empty2;
                    val3.Skus = list4;
                    val3.Location = GetLoction(loctionStr);
                    val3.Num = num;
                    //if (DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", _toolCode.ToUpper(), "SnacthCPrice", "false")))
                    //{
                    //    val3.Price=GetGoodsCPrice(list4);
                    //    if (DataConvert.ToDecimal((object)val3.Price) <= 0m)
                    //    {
                    //        val3.Price=text2;
                    //    }
                    //}
                    //else
                    //{
                    //    val3.Price=text2;
                    //}
                    val3.Price = text2;

                    val3.StuffStatus = stuffStatus;
                    val.Item = val3;
                    return val;
                }
                return null;
            }
            catch (Exception ex)
            {
                //Log.WriteLog("下载淘宝商品信息出现异常", ex);
                return val;
            }
        }
        private void FoodSecurityDate(string onlineKey, ref string productStartDatetime, ref string productEndDateTime)
        {
            //IL_0060: Unknown result type (might be due to invalid IL or missing references)
            productStartDatetime = DateTime.Now.Date.AddDays(-1.0).ToString("yyyy-MM-dd");
            productEndDateTime = DateTime.Now.Date.AddDays(-1.0).ToString("yyyy-MM-dd");
            new ItemGetResponse();
            string text = $"http://item.taobao.com/item.htm?id={new object[1] { onlineKey }}";
            try
            {
                int num = 1;
                string responseResultBody = GetResponseResultBody(text, Encoding.GetEncoding("gb2312"), "GET", true, text, ref num);
                if (!string.IsNullOrEmpty(responseResultBody))
                {
                    string pattern = "(?is)生产日期:(?<date>.*?)<";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(responseResultBody);
                    if (match != null && match.Success)
                    {
                        string value = match.Value;
                        value = value.Replace("年", "-").Replace("月", "-").Replace("日", "-");
                        value = value.Replace("/", "-");
                        string pattern2 = "\\d{4}\\-\\d{1,2}\\-\\d{1,2}";
                        Regex regex2 = new Regex(pattern2);
                        MatchCollection matchCollection = regex2.Matches(value);
                        if (matchCollection != null && matchCollection.Count == 2)
                        {
                            productStartDatetime = matchCollection[0].Value;
                            productEndDateTime = matchCollection[1].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //	Log.WriteLog(ex);
            }
        }
        private FoodSecurity GetFoodSecurity(Dictionary<string, string> dicProNameAndProValue)
        {
            //IL_0014: Unknown result type (might be due to invalid IL or missing references)
            //IL_0019: Expected O, but got Unknown
            FoodSecurity val = null;
            if (dicProNameAndProValue != null && dicProNameAndProValue.Count > 0)
            {
                val = new FoodSecurity();
                foreach (KeyValuePair<string, string> item in dicProNameAndProValue)
                {
                    if (item.Key.Contains("生产许可证编号"))
                    {
                        val.PrdLicenseNo = item.Value;
                    }
                    if (item.Key.Contains("产品标准号"))
                    {
                        val.DesignCode = item.Value;
                    }
                    if (item.Key.Contains("厂名"))
                    {
                        val.Factory = item.Value;
                    }
                    if (item.Key.Contains("厂址"))
                    {
                        val.FactorySite = item.Value;
                    }
                    if (item.Key.Contains("厂家联系方式"))
                    {
                        val.Contact = item.Value;
                    }
                    if (item.Key.Contains("配料表"))
                    {
                        val.Mix = item.Value;
                    }
                    if (item.Key.Contains("储藏方法"))
                    {
                        val.PlanStorage = item.Value;
                    }
                    if (item.Key.Contains("保质期"))
                    {
                        val.Period = item.Value;
                    }
                    if (item.Key.Contains("食品添加剂"))
                    {
                        val.FoodAdditive = item.Value;
                    }
                    if (item.Key.Contains("健字号"))
                    {
                        val.HealthProductNo = item.Value;
                    }
                    if (item.Key.Contains("供应商") || item.Key.Contains("厂名"))
                    {
                        val.Supplier = item.Value;
                    }
                }
                if (string.IsNullOrEmpty(val.FoodAdditive))
                {
                    val.FoodAdditive = "无";
                }
            }
            return val;
        }

        internal static void GetPropertyStrForCloudSnatch(Dictionary<string, string> dicProNameAndProValue, IList<SellProInfo> sellProInfoList, out string propsStr, out string propsNameStr)
        {
            propsStr = string.Empty;
            propsNameStr = string.Empty;
            if (dicProNameAndProValue != null && dicProNameAndProValue.Count > 0)
            {
                propsStr = STARTSTR;
                foreach (KeyValuePair<string, string> item in dicProNameAndProValue)
                {
                    if (propsStr == STARTSTR)
                    {
                        propsStr = propsStr + Base64.ToBase64(item.Key) + CONNSTR2 + Base64.ToBase64(item.Value);
                    }
                    else
                    {
                        string text = propsStr;
                        propsStr = text + CONNSTR1 + Base64.ToBase64(item.Key) + CONNSTR2 + Base64.ToBase64(item.Value);
                    }
                }
            }
            if (sellProInfoList != null && sellProInfoList.Count > 0)
            {
                propsNameStr = STARTSTR;
                for (int i = 0; i < sellProInfoList.Count; i++)
                {
                    if (propsNameStr == STARTSTR)
                    {
                        propsNameStr = propsNameStr + Base64.ToBase64(sellProInfoList[i].Name) + CONNSTR2 + Base64.ToBase64(sellProInfoList[i].Value);
                    }
                    else
                    {
                        string text2 = propsNameStr;
                        propsNameStr = text2 + CONNSTR1 + Base64.ToBase64(sellProInfoList[i].Name) + CONNSTR2 + Base64.ToBase64(sellProInfoList[i].Value);
                    }
                }
            }
        }

        internal static void GetPropertyStrForLocalSnatch(Dictionary<string, string> dicProNameAndProValue, IList<SellProInfo> sellProInfoList, string cid, out string propsStr, out string propsNameStr, out string inputStr, out string inputStrName)
        {

            int num = 100001;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            propsStr = string.Empty;
            propsNameStr = string.Empty;
            inputStr = string.Empty;
            inputStrName = string.Empty;
            if (dicProNameAndProValue != null && dicProNameAndProValue.Count > 0)
            {
                int num2 = 0;
                 Sys_sysSort val = DataHelper.GetSysSort(cid);// ToolServer.get_ProductData().GetSortBySysIdAndKeys(1, cid);
            
                if (val != null)
                {
                    num2 = val.Id;
                }
                if (num2 > 0)
                {
                    Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                    DataTable propertyDtBySortId = DataHelper.GetPropertyDtBySortId(num2);
                    DataTable propertyValueDtBySortId =  DataHelper.GetPropertyValueDtBySortId(num2);// ToolServer.get_ProductData().GetPropertyValueDtBySortId(num2);
                    foreach (KeyValuePair<string, string> item in dicProNameAndProValue)
                    {
                        try
                        {
                            if (!dictionary.ContainsKey(item.Value + item.Key))
                            {
                                string likeStrForDtSelect = GetLikeStrForDtSelect(item.Key);
                                DataRow[] array = propertyDtBySortId.Select("name='" + DbUtil.OerateSpecialChar(likeStrForDtSelect) + "'");
                                if (item.Key == "品牌" && item.Key == "品牌")
                                {
                                    bool flag = false;
                                    DataRow dataRow = null;
                                    DataRow[] array2 = array;
                                    foreach (DataRow dataRow2 in array2)
                                    {
                                        DataRow[] array3 = propertyValueDtBySortId.Select("propertyid='" + dataRow2["id"].ToString() + "' and name = '" + DbUtil.OerateSpecialChar(item.Value) + "'");
                                        if (array3 != null && array3.Length > 0)
                                        {
                                            flag = true;
                                            dataRow = dataRow2;
                                            break;
                                        }
                                    }
                                    if (flag && dataRow != null)
                                    {
                                        array[0] = dataRow;
                                    }
                                    else
                                    {
                                        DataRow dataRow3 = null;
                                        DataRow[] array4 = array;
                                        foreach (DataRow dataRow4 in array4)
                                        {
                                            if (dataRow4["parentId"].ToString() == "0")
                                            {
                                                dataRow3 = dataRow4;
                                                break;
                                            }
                                        }
                                        if (dataRow3 != null)
                                        {
                                            array[0] = dataRow3;
                                        }
                                    }
                                }
                                if (array != null && array.Length > 0 && DataConvert.ToInt(array[0]["isSellPro"]) != 1 && DataConvert.ToInt(array[0]["valueType"]) == 0)
                                {
                                    string proId = array[0]["id"].ToString();
                                    GetChildPropertyAndSetProps(propertyDtBySortId, propertyValueDtBySortId, dictionary, dicProNameAndProValue, likeStrForDtSelect, ref propsStr, ref propsNameStr, ref inputStr, ref inputStrName, proId, item.Value, ref num);
                                }
                                else if (array != null && array.Length > 0 && DataConvert.ToInt(array[0]["isSellPro"]) != 1 && DataConvert.ToInt(array[0]["valueType"]) == 1)
                                {
                                    string text = array[0]["id"].ToString();
                                    string[] array5 = item.Value.Trim().Split(new char[2]
                                    {
                                        ' ',
                                        ','
                                    }, StringSplitOptions.RemoveEmptyEntries);
                                    if (array5 != null && array5.Length > 0)
                                    {
                                        DataRow[] array6 = null;
                                        string[] array7 = array5;
                                        foreach (string text2 in array7)
                                        {
                                            array6 = propertyValueDtBySortId.Select("propertyid='" + text + "' and name = '" + DbUtil.OerateSpecialChar(text2) + "'");
                                            if (array6 != null && array6.Length > 0)
                                            {
                                                propsStr = propsStr + array6[0]["value"].ToString().Trim() + ";";
                                                string text3 = propsNameStr;
                                                propsNameStr = text3 + array6[0]["value"].ToString().Trim() + ":" + likeStrForDtSelect + ":" + array6[0]["name"].ToString().Trim() + ";";
                                            }
                                        }
                                    }
                                }
                                else if (array != null && array.Length > 0 && DataConvert.ToInt(array[0]["isSellPro"]) != 1 && DataConvert.ToInt(array[0]["valueType"]) == 2)
                                {
                                    if (!dictionary2.ContainsKey(DbUtil.OerateSpecialChar(likeStrForDtSelect)))
                                    {
                                        DataRow[] array8 = propertyDtBySortId.Select("name='" + DbUtil.OerateSpecialChar(likeStrForDtSelect) + "' and levels = 1 and propertyType = 1");
                                        if (array8 == null || array8.Length <= 0)
                                        {
                                            array8 = propertyDtBySortId.Select("name='" + DbUtil.OerateSpecialChar(likeStrForDtSelect) + "' and levels = 1");
                                        }
                                        if (array8 != null && array8.Length > 0)
                                        {
                                            string text4 = array8[0]["keys"] + ":11111";
                                            propsStr = propsStr + text4 + ";";
                                            object obj = propsNameStr;
                                            propsNameStr = obj + text4 + ":" + array8[0]["name"] + ":" + item.Value + ";";
                                            dictionary2[DbUtil.OerateSpecialChar(likeStrForDtSelect)] = array8[0]["keys"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        string text5 = dictionary2[DbUtil.OerateSpecialChar(likeStrForDtSelect)];
                                        DataRow[] array9 = propertyDtBySortId.Select("name='" + DbUtil.OerateSpecialChar(likeStrForDtSelect) + "' and keys = '" + text5 + "'");
                                        if (array9 != null && array9.Length > 0)
                                        {
                                            int num3 = DataConvert.ToInt(array9[0]["propertyType"]);
                                            DataRow[] array10 = propertyDtBySortId.Select("name='" + DbUtil.OerateSpecialChar(likeStrForDtSelect) + "' and levels = " + num3 + 1);
                                            if (array10 != null && array10.Length > 0)
                                            {
                                                string text6 = array10[0]["keys"] + ":11111";
                                                propsStr = propsStr + text6 + ";";
                                                object obj2 = propsNameStr;
                                                propsNameStr = obj2 + text6 + ":" + array10[0]["name"] + ":" + item.Value + ";";
                                                dictionary2[DbUtil.OerateSpecialChar(likeStrForDtSelect)] = array10[0]["keys"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLog(ex);
                        }
                    }
                    if (sellProInfoList != null && sellProInfoList.Count > 0)
                    {
                        foreach (SellProInfo sellProInfo in sellProInfoList)
                        {
                            propsStr = propsStr + sellProInfo.Value.Trim() + ";";
                            if (sellProInfo.Value.IndexOf(":") > 0)
                            {
                                string text7 = sellProInfo.Value.Substring(0, sellProInfo.Value.IndexOf(":"));
                                DataRow[] array11 = propertyDtBySortId.Select("keys='" + DbUtil.OerateSpecialChar(text7) + "'");
                                if (array11 != null && array11.Length > 0)
                                {
                                    object obj3 = propsNameStr;
                                    propsNameStr = obj3 + sellProInfo.Value.Trim() + ":" + array11[0]["name"] + ":" + sellProInfo.Name.Trim().Trim() + ";";
                                }
                            }
                        }
                    }
                }
            }
        }
        private static string GetLikeStrForDtSelect(string likeOrdinaryStr)
        {
            string text = DbUtil.OerateSpecialChar(likeOrdinaryStr);
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }
            return text.Replace("[", "【").Replace("]", "】").Replace("%", "[%]")
                .Replace("_", "[_]")
                .Replace("'", "，")
                .Replace("*", "[*]")
                .Trim();
        }
        private static void GetChildPropertyAndSetProps(DataTable dtProperty, DataTable dtPropertyValue, Dictionary<string, string> dicDistributionProValueAndProName, Dictionary<string, string> dicProNameAndProValue, string key, ref string propsStr, ref string propsNameStr, ref string inputStr, ref string inputStrName, string proId, string value, ref int keyIndex)
        {
            if (dtPropertyValue != null && dtPropertyValue.Rows != null && dtPropertyValue.Rows.Count > 0)
            {
                DataRow[] array = null;
                array = dtPropertyValue.Select("propertyid='" + proId + "' and name = '" + DbUtil.OerateSpecialChar(value) + "'");
                if (array == null || array.Length <= 0)
                {
                    DataRow[] array2 = dtPropertyValue.Select("propertyid='" + proId + "'");
                    int num = 0;
                    while (num < array2.Length)
                    {
                        if (!value.Contains(array2[num]["name"].ToString()))
                        {
                            num++;
                            continue;
                        }
                        value = array2[num]["name"].ToString();
                        break;
                    }
                    array = dtPropertyValue.Select("propertyid='" + proId + "' and name = '" + DbUtil.OerateSpecialChar(value) + "'");
                }
                if (array != null && array.Length > 0)
                {
                    propsStr = propsStr + array[0]["value"].ToString().Trim() + ";";
                    string text = propsNameStr;
                    propsNameStr = text + array[0]["value"].ToString().Trim() + ":" + key + ":" + array[0]["name"].ToString().Trim() + ";";
                    if (DataConvert.ToBoolean(array[0]["hasChildProperty"]))
                    {
                        string text2 = array[0]["value"].ToString();
                        DataRow[] array3 = dtProperty.Select("parentPropertyValue='" + DbUtil.OerateSpecialChar(text2) + "'");
                        if (array3 != null && array3.Length > 0)
                        {
                            key = array3[0]["name"].ToString();
                            if (dicProNameAndProValue.ContainsKey(key) && !dicDistributionProValueAndProName.ContainsKey(dicProNameAndProValue[key] + key))
                            {
                                proId = array3[0]["id"].ToString();
                                value = dicProNameAndProValue[key];
                                dicDistributionProValueAndProName[value + key] = key;
                                GetChildPropertyAndSetProps(dtProperty, dtPropertyValue, dicDistributionProValueAndProName, dicProNameAndProValue, key, ref propsStr, ref propsNameStr, ref inputStr, ref inputStrName, proId, value, ref keyIndex);
                            }
                        }
                    }
                }
                if (array == null || array.Length <= 0)
                {
                    array = dtPropertyValue.Select("propertyid='" + proId + "' and name = '" + DbUtil.OerateSpecialChar("其他") + "'");
                    if (array != null && array.Length > 0)
                    {
                        string text3 = array[0]["value"].ToString().Substring(0, array[0]["value"].ToString().IndexOf(':'));
                        string text4 = text3 + ":" + keyIndex;
                        propsStr = propsStr + text4 + ";";
                        string text5 = propsNameStr;
                        propsNameStr = text5 + text4 + ":" + key + ":" + value + ";";
                        inputStr = inputStr + text3 + ",";
                        inputStrName = inputStrName + value + ",";
                        keyIndex++;
                        if (DataConvert.ToBoolean(array[0]["hasChildProperty"]))
                        {
                            string text6 = array[0]["value"].ToString();
                            DataRow[] array4 = dtProperty.Select("parentPropertyValue='" + DbUtil.OerateSpecialChar(text6) + "'");
                            if (array4 != null && array4.Length > 0)
                            {
                                DataRow[] array5 = array4;
                                foreach (DataRow dataRow in array5)
                                {
                                    key = dataRow["name"].ToString();
                                    if (dicProNameAndProValue.ContainsKey(key) && !dicDistributionProValueAndProName.ContainsKey(dicProNameAndProValue[key] + key))
                                    {
                                        proId = dataRow["id"].ToString();
                                        value = dicProNameAndProValue[key];
                                        dicDistributionProValueAndProName[value + key] = key;
                                        GetChildPropertyAndSetProps(dtProperty, dtPropertyValue, dicDistributionProValueAndProName, dicProNameAndProValue, key, ref propsStr, ref propsNameStr, ref inputStr, ref inputStrName, proId, value, ref keyIndex);
                                    }
                                }
                            }
                        }
                    }
                }
                if (dtProperty != null && dtProperty.Rows.Count > 0)
                {
                    if (array != null && array.Length > 0)
                    {
                        return;
                    }
                    DataRow[] array6 = dtProperty.Select("id='" + proId + "'");
                    if (array6 != null && array6.Length > 0)
                    {
                        propsStr = propsStr + array6[0][17].ToString().Trim() + ";";
                        string text7 = propsNameStr;
                        propsNameStr = text7 + array6[0][17].ToString().Trim() + ":" + key + ":" + value + ";";
                    }
                }
            }
        }

        private string GetGoodsCPrice(List<Sku> skuList)
        {
            string result = string.Empty;
            List<decimal> list = new List<decimal>();
            if (skuList != null && skuList.Count > 0)
            {
                foreach (Sku sku in skuList)
                {
                    list.Add(DataConvert.ToDecimal((object)sku.Price));
                }
            }
            list.Sort();
            if (list.Count > 0)
            {
                result = list[list.Count - 1].ToString();
            }
            return result;
        }
        private IList<string> GetProvinceList()
        {
            IList<string> list = new List<string>();
            list.Add("河北省");
            list.Add("河北");
            list.Add("山西省");
            list.Add("山西");
            list.Add("内蒙古自治区");
            list.Add("内蒙古");
            list.Add("辽宁省");
            list.Add("辽宁");
            list.Add("吉林省");
            list.Add("吉林");
            list.Add("黑龙江省");
            list.Add("黑龙江");
            list.Add("江苏省");
            list.Add("江苏");
            list.Add("浙江省");
            list.Add("浙江");
            list.Add("安徽省");
            list.Add("安徽");
            list.Add("福建省");
            list.Add("福建");
            list.Add("江西省");
            list.Add("江西");
            list.Add("山东省");
            list.Add("山东");
            list.Add("河南省");
            list.Add("河南");
            list.Add("湖北省");
            list.Add("湖北");
            list.Add("湖南省");
            list.Add("湖南");
            list.Add("广东省");
            list.Add("广东");
            list.Add("广西壮族自治区");
            list.Add("广西");
            list.Add("海南省");
            list.Add("海南");
            list.Add("四川省");
            list.Add("四川");
            list.Add("贵州省");
            list.Add("贵州");
            list.Add("云南省");
            list.Add("云南");
            list.Add("西藏自治区");
            list.Add("西藏");
            list.Add("陕西省");
            list.Add("陕西");
            list.Add("甘肃省");
            list.Add("甘肃");
            list.Add("青海省");
            list.Add("青海");
            list.Add("宁夏回族自治区");
            list.Add("宁夏");
            list.Add("新疆维吾尔自治区");
            list.Add("新疆");
            list.Add("台湾省");
            return list;
        }
        private Location GetLoction(string loctionStr)
        {
            //IL_0060: Unknown result type (might be due to invalid IL or missing references)
            //IL_0065: Expected O, but got Unknown
            string text = string.Empty;
            string text2 = string.Empty;
            IList<string> provinceList = GetProvinceList();
            for (int i = 0; i < provinceList.Count; i++)
            {
                text = provinceList[i];
                if (!string.IsNullOrEmpty(loctionStr) && loctionStr.Contains(text))
                {
                    text2 = loctionStr.Replace(text, string.Empty);
                    break;
                }
            }
            if (!string.IsNullOrEmpty(loctionStr) && string.IsNullOrEmpty(text2))
            {
                text = loctionStr;
                text2 = loctionStr;
            }
            Location val = new Location();
            val.City = text2;
            val.State = text;
            return val;
        }
        private Dictionary<string, string> GetMobileItemProperty(List<Prop> proList, ref string barcode)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (proList != null && proList.Count > 0)
            {
                for (int i = 0; i < proList.Count; i++)
                {
                    dictionary[proList[i].Name] = proList[i].Value;
                    if (proList[i].Name.Contains("条形码"))
                    {
                        barcode = proList[i].Value;
                    }
                }
            }
            return dictionary;
        }
        private List<PropImg> GetMobileProImgList(List<SkuPropMobile> skuModelList, IList<SellProInfo> sellProInfoList)
        {
            //IL_0084: Unknown result type (might be due to invalid IL or missing references)
            //IL_0089: Expected O, but got Unknown
            List<PropImg> list = new List<PropImg>();
            PropImg val = null;
            SellProInfo sellProInfo = null;
            foreach (SkuPropMobile skuModel in skuModelList)
            {
                if (!string.IsNullOrEmpty(skuModel.PropId) && skuModel != null && skuModel.Values != null && skuModel.Values.Count > 0)
                {
                    List<Value> values = skuModel.Values;
                    foreach (Value item in values)
                    {
                        sellProInfo = new SellProInfo();
                        if (!string.IsNullOrEmpty(item.ImgUrl))
                        {
                            val = new PropImg();
                            val.Id = 0L;
                            val.Position = 0L;
                            val.Properties = skuModel.PropId + ":" + item.ValueId;
                            val.Url = item.ImgUrl;
                            list.Add(val);
                        }
                        sellProInfo.Name = item.Name;
                        sellProInfo.Value = skuModel.PropId + ":" + item.ValueId;
                        sellProInfoList.Add(sellProInfo);
                    }
                }
            }
            return list;
        }
        private string GetWirelessDescContent(string h5DescUrl, string pcContentDesc)
        {
            int num = 0;
            if (!h5DescUrl.StartsWith("http://") && !h5DescUrl.StartsWith("https://"))
            {
                h5DescUrl = "http://" + h5DescUrl;
            }
            string result = string.Empty;
            try
            {
                string responseResultBody = GetResponseResultBody(h5DescUrl, Encoding.GetEncoding("gb2312"), "GET", true, h5DescUrl, ref num);
                string text = string.Empty;
                string text2 = string.Empty;
                if (!string.IsNullOrEmpty(responseResultBody))
                {
                    string text3 = string.Empty;
                    string pattern = "(?is)\"pages\":\\[(?<data>.*?)\\],";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(responseResultBody);
                    if (match != null && match.Groups["data"].Success)
                    {
                        text3 = match.Groups["data"].Value.Replace("\"", string.Empty).Replace(",", string.Empty);
                        text3 = text3.Replace("＜", "<").Replace("＞", ">");
                        text3 = text3.Replace("\\u00A0", " ");
                    }
                    bool flag = true;
                    if (!string.IsNullOrEmpty(text3))
                    {
                        regex = new Regex("(?is)<txt>(?<txtValue>.*?)</txt>");
                        MatchCollection matchCollection = regex.Matches(text3);
                        {
                            IEnumerator enumerator = matchCollection.GetEnumerator();
                            try
                            {
                                if (enumerator.MoveNext())
                                {
                                    Match match2 = (Match)enumerator.Current;
                                    if (matchCollection.Count == 1 && match2.Groups["txtValue"].Value.Contains("\\u00A0") && match2.Groups["txtValue"].Value != "\\u00A0")
                                    {
                                        flag = false;
                                    }
                                }
                            }
                            finally
                            {
                                IDisposable disposable = enumerator as IDisposable;
                                if (disposable != null)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(text3) && flag)
                    {
                        regex = new Regex("(?is)((size=(?<maxSize>.*?)x[^<>].*?)|(img))>(?<contentInfo>.*?)<");
                        MatchCollection matchCollection2 = regex.Matches(text3);
                        if (matchCollection2 != null && matchCollection2.Count > 0)
                        {
                            foreach (Match item in matchCollection2)
                            {
                                if (!(item.Groups["maxSize"].Value != "") || DataConvert.ToInt((object)item.Groups["maxSize"].Value) >= 450)
                                {
                                    string value = item.Groups["contentInfo"].Value;
                                    if (!value.Contains(".gif") && value.Contains("//"))
                                    {
                                        text = text + "<img>" + value.Replace("//", "http://") + "</img>";
                                    }
                                }
                            }
                        }
                        regex = new Regex("(?is)<txt>(?<txtInfo>.*?)</txt>");
                        MatchCollection matchCollection3 = regex.Matches(text3);
                        if (matchCollection3 != null && matchCollection3.Count > 0)
                        {
                            foreach (Match item2 in matchCollection3)
                            {
                                if (!item2.Groups["txtInfo"].Value.Trim().Equals("\\u00A0") && !item2.Groups["txtInfo"].Value.Trim().Equals("&nbsp;"))
                                {
                                    text2 += item2;
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(pcContentDesc))
                    {
                        regex = new Regex("(?is)src=\"(?<MPicUrl>.*?)\"");
                        MatchCollection matchCollection4 = regex.Matches(pcContentDesc);
                        if (matchCollection4 != null && matchCollection4.Count > 0)
                        {
                            foreach (Match item3 in matchCollection4)
                            {
                                string value2 = item3.Groups["MPicUrl"].Value;
                                if (!value2.Contains(".gif") && value2.Contains("http://"))
                                {
                                    text = text + "<img>" + value2 + "</img>";
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
                    {
                        result = string.Empty;
                        return result;
                    }
                    result = "<wapDesc>" + text2 + text + "</wapDesc>";
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                // Log.WriteLog("下载商品手机详情出现异常", ex);
                return result;
            }
        }
        private string GetpcDescContent(string pcDescUrl)
        {
            return GetpcDescContent(pcDescUrl, 1);
        }

        private string GetpcDescContent(string pcDescUrl, int tryNum)
        {
            int num = 0;
            if (!pcDescUrl.StartsWith("http://") && !pcDescUrl.StartsWith("https://"))
            {
                pcDescUrl = "http://" + pcDescUrl;
            }
            string text = GetResponseResultBody(pcDescUrl, Encoding.GetEncoding("gb2312"), "GET", true, pcDescUrl, ref num);
            if (!string.IsNullOrEmpty(text))
            {
                string pattern = "var\\s*wdescData\\s*=\\s*{\\s*tfsContent\\s*:\\s*'(?<itemContent>.*?)',";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = regex.Match(text);
                if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["itemContent"].Value))
                {
                    text = match.Groups["itemContent"].Value;
                    pattern = "src\\s*=\\s*\"\\s*//";
                    regex = new Regex(pattern);
                    text = regex.Replace(text, "src=\"http://");
                    pattern = "background=\"\\s*//";
                    regex = new Regex(pattern);
                    text = regex.Replace(text, "background=\"http://");
                    pattern = "background\\s*:\\s*url\\s*\\(\"\\s*//";
                    regex = new Regex(pattern);
                    text = regex.Replace(text, "background:url(\"http://");
                    pattern = "background\\s*:\\s*url\\s*\\(\\s*//";
                    regex = new Regex(pattern);
                    text = regex.Replace(text, "background:url(http://");
                    pattern = "\\.jpg_\\.webp";
                    regex = new Regex(pattern);
                    text = regex.Replace(text, ".jpg");
                    pattern = "(?is)<embed[^>]*?>";
                    regex = new Regex(pattern);
                    text = regex.Replace(text, string.Empty);
                    pattern = "(?is)<img[^>]*?size=\"(?<width>\\d+)x\\d+\"[^>]*?(?<img>src=\"[^\"]*?\")[^>]*?>|(?is)<img[^>]*?(?<img>src=\"[^\"]*?\")[^>]*?size=\"(?<width>\\d+)x\\d+\"[^>]*?>";
                    regex = new Regex(pattern);
                    MatchCollection matchCollection = regex.Matches(text);
                    if (matchCollection != null && matchCollection.Count > 0)
                    {
                        foreach (Match item in matchCollection)
                        {
                            if (item != null && !string.IsNullOrEmpty(item.Groups["img"].Value) && !string.IsNullOrEmpty(item.Groups["width"].Value) && DataConvert.ToInt((object)item.Groups["width"].Value) > 750)
                            {
                                string value = item.Groups["img"].Value;
                                string newValue = item.Groups["img"].Value + " width=\"750px\" ";
                                text = text.Replace(value, newValue);
                                text = text.Replace("width=\"790\"", "width=\"750\"").Replace("width: 790px", "width: 750px");
                            }
                        }
                    }
                }
            }
            string pattern2 = "(?is)alt=(?<isImg>[^<>\\s]*?)[\\s<>]";
            Regex regex2 = new Regex(pattern2, RegexOptions.IgnoreCase);
            MatchCollection matchCollection2 = regex2.Matches(text);
            if (matchCollection2.Count > 0)
            {
                foreach (Match item2 in matchCollection2)
                {
                    string text2 = item2.Groups["isImg"].Value.ToLower();
                    if (text2.Contains("http://") || (text2.Contains("https://") && (text2.Contains(".jpg") || text2.Contains(".jpeg") || text2.Contains(".png") || text2.Contains(".gif"))))
                    {
                        text = text.Replace(item2.Value, "src=" + item2.Groups["isImg"].Value);
                    }
                }
            }
            string text3 = text;
            if (string.IsNullOrEmpty(text))
            {
                text = text3;
            }
            if (string.IsNullOrEmpty(text) && tryNum < 3)
            {
                tryNum++;
                Thread.Sleep(1000);
                return GetpcDescContent(pcDescUrl, tryNum);
            }
            if (!string.IsNullOrEmpty(text) && !text.Contains("https:"))
            {
                text = text.Replace("http", "https");
            }
            return text;
        }

        private List<ItemImg> GetItemImgList(List<string> picsPath)
        {
            //IL_0018: Unknown result type (might be due to invalid IL or missing references)
            //IL_001d: Expected O, but got Unknown
            List<ItemImg> list = new List<ItemImg>();
            ItemImg val = null;
            if (picsPath != null && picsPath.Count > 0)
            {
                for (int i = 0; i < picsPath.Count; i++)
                {
                    val = new ItemImg();
                    val.Position = (long)i;
                    if (picsPath[i].Contains("img01.taobaocdn"))
                    {
                        picsPath[i] = picsPath[i].Replace("img01.taobaocdn", "gd1.alicdn");
                    }
                    val.Url = picsPath[i];
                    list.Add(val);
                }
            }
            return list;
        }

        private string GetResponseResultBody(string url, Encoding encoding, string rquestMethod, bool isIE, string referer, ref int times)
        {
            string empty = string.Empty;
            CommonApiClient commonApiClient = new CommonApiClient();
            CookieCollection cookieCollection = new CookieCollection();
            if (url.Contains(".taobao.com"))
            {
                Cookie cookie = new Cookie("tracknick", "zbs18938040399");
                cookie.Domain = ".taobao.com";
                Cookie cookie2 = cookie;
                cookieCollection.Add(cookie2);
            }
            else
            {
                Cookie cookie3 = new Cookie("tracknick", "zbs18938040399");
                cookie3.Domain = ".tmall.com";
                Cookie cookie4 = cookie3;
                cookieCollection.Add(cookie4);
            }
            empty = commonApiClient.Invoke(url, encoding, rquestMethod, isIE, referer, cookieCollection);
            int num = 0;
            while (string.IsNullOrEmpty(empty) && num < 3)
            {
                num++;
                empty = commonApiClient.Invoke(url, encoding, rquestMethod, isIE, referer, cookieCollection);
                Thread.Sleep(1000);
            }
            if (string.IsNullOrEmpty(empty))
            {
                // Log.WriteLog("请求失败:" + url);
            }
            if (!string.IsNullOrEmpty(empty) && times < 3)
            {
                if (empty.Contains("访问受限了"))
                {
                    // Log.WriteLog("访问受限了:" + empty);
                    return string.Empty;
                }
                if (empty.Contains("ERRCODE_QUERY_DETAIL_FAIL"))
                {
                    times++;
                    Thread.Sleep(times * 1000);
                    return GetResponseResultBody(url, encoding, rquestMethod, isIE, referer, ref times);
                }
            }
            return empty;
        }
        private long GetMobileNums(List<Sku> skuList, string rsContent)
        {
            long num = 0L;
            if (skuList != null && skuList.Count > 0)
            {
                foreach (Sku sku in skuList)
                {
                    num += sku.Quantity;
                }
            }
            if (num <= 0)
            {
                string pattern = "\\\\\"quantity\\\\\":\\\\\"?(?<quantity>.*?)\\\\\"?";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(rsContent);
                if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["quantity"].Value))
                {
                    num = DataConvert.ToLong((object)match.Groups["quantity"].Value);
                }
            }
            return num;
        }

        private string GetMobilePrice(List<Sku> skuList, string rsContent, bool isSnatchpromoPrice, string cxPrice, string itemId)
        {
            string pattern = "\"apiStack\":.*?installmentEnable";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(rsContent);
            if (match != null && match.Success)
            {
                rsContent = match.Value;
            }
            string text = string.Empty;
            decimal num = 0m;
            if (skuList != null && skuList.Count > 0)
            {
                foreach (Sku sku in skuList)
                {
                    if (string.IsNullOrEmpty(text) && sku.Quantity > 0)
                    {
                        text = sku.Price;
                    }
                    else if (DataConvert.ToDecimal((object)text) > DataConvert.ToDecimal((object)sku.Price) && sku.Quantity > 0)
                    {
                        text = sku.Price;
                    }
                }
            }
            else
            {
                text = cxPrice;
            }
            if (DataConvert.ToDecimal((object)text) <= 0m)
            {
                string pattern2 = "\\\\\"price\\\\\":\\\\\"(?<price>.*?)\\\\\"";
                Regex regex2 = new Regex(pattern2);
                MatchCollection matchCollection = regex2.Matches(rsContent);
                if (matchCollection != null && matchCollection.Count > 0)
                {
                    foreach (Match item in matchCollection)
                    {
                        if (!isSnatchpromoPrice)
                        {
                            if (num == 0m)
                            {
                                num = DataConvert.ToDecimal((object)item.Groups["price"].Value);
                            }
                            else if (DataConvert.ToDecimal((object)num) < DataConvert.ToDecimal((object)DataConvert.ToDecimal((object)item.Groups["price"].Value)))
                            {
                                num = DataConvert.ToDecimal((object)item.Groups["price"].Value);
                            }
                        }
                        else if (num == 0m)
                        {
                            num = DataConvert.ToDecimal((object)item.Groups["price"].Value);
                        }
                        else if (DataConvert.ToDecimal((object)num) > DataConvert.ToDecimal((object)DataConvert.ToDecimal((object)item.Groups["price"].Value)))
                        {
                            num = DataConvert.ToDecimal((object)item.Groups["price"].Value);
                        }
                    }
                }
                text = DataConvert.ToString((object)num);
                if (text == "0")
                {
                    int num2 = 1;
                    string stringToEscape = "{\"itemNumId\":\"" + itemId + "\"}";
                    stringToEscape = Uri.EscapeDataString(stringToEscape);
                    string text2 = "http://acs.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data=" + stringToEscape;
                    string input = GetResponseResultBody(text2, Encoding.GetEncoding("utf-8"), "GET", true, text2, ref num2);
                    if (isSnatchpromoPrice)
                    {
                        pattern2 = "(?is)\\\\\"skuCore\\\\\":.*?\"item\"";
                        regex2 = new Regex(pattern2);
                        Match match3 = regex2.Match(input);
                        if (match3 != null && match3.Success)
                        {
                            input = match3.Value;
                        }
                    }
                    pattern2 = "\\\\\"priceText\\\\\":\\\\\"(?<priceText>.*?)\\\\\"";
                    regex2 = new Regex(pattern2);
                    matchCollection = regex2.Matches(input);
                    if (matchCollection != null && matchCollection.Count > 0)
                    {
                        foreach (Match item2 in matchCollection)
                        {
                            if (!isSnatchpromoPrice)
                            {
                                if (num == 0m)
                                {
                                    num = DataConvert.ToDecimal((object)item2.Groups["priceText"].Value);
                                }
                                else if (DataConvert.ToDecimal((object)num) < DataConvert.ToDecimal((object)DataConvert.ToDecimal((object)item2.Groups["priceText"].Value)))
                                {
                                    num = DataConvert.ToDecimal((object)item2.Groups["priceText"].Value);
                                }
                            }
                            else if (num == 0m)
                            {
                                num = DataConvert.ToDecimal((object)item2.Groups["priceText"].Value);
                            }
                            else if (DataConvert.ToDecimal((object)num) > DataConvert.ToDecimal((object)DataConvert.ToDecimal((object)item2.Groups["priceText"].Value)))
                            {
                                num = DataConvert.ToDecimal((object)item2.Groups["priceText"].Value);
                            }
                        }
                    }
                    text = DataConvert.ToString((object)num);
                }
            }
            return text;
        }

        public List<Sku> GetMobilePromoPriceSkuList(string newResContent, bool isTaobao, ItemJsonEntity60 itemJsonEntity60, ref string price, ref long num, bool snatchPromotionPrice, Data60 itemData)
        {
            //IL_0425: Unknown result type (might be due to invalid IL or missing references)
            //IL_042a: Expected O, but got Unknown
            price = "";
            num = 0L;
            long num2 = 0L;
            Dictionary<string, Sku2Info> dictionary = new Dictionary<string, Sku2Info>();
            Dictionary<string, Sku2Info_T> dictionary2 = new Dictionary<string, Sku2Info_T>();
            if (!string.IsNullOrEmpty(newResContent))
            {
                Regex regex = null;
                string pattern = "\\\"value\\\".*?\\\\\"skuCore\\\\\":(?<skuCore>.*?),\\\\\"skuItem\\\\\"";
                if (isTaobao)
                {
                    if (!snatchPromotionPrice && itemData != null && !string.IsNullOrEmpty(itemData.mockData))
                    {
                        pattern = "\\\"skuCore\\\":(?<skuCore>.*?),\\\"skuItem\\\"";
                        newResContent = itemData.mockData;
                    }
                    regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match = regex.Match(newResContent);
                    if (match != null && match.Groups["skuCore"].Success && !string.IsNullOrEmpty(match.Groups["skuCore"].Value))
                    {
                        string text = match.Groups["skuCore"].Value.Replace("\\\"", "\"") + "}";
                        ItemTaobaoSkuCore itemTaobaoSkuCore = JsonConvert.DeserializeObject<ItemTaobaoSkuCore>(text);
                        dictionary = itemTaobaoSkuCore.sku2Info;
                    }
                }
                else
                {
                    pattern = "(?is)\\\"sku2info\\\\\":(?<skuInfo>.*?)},\\\\\"skuVertical\\\\\"";
                    if (!snatchPromotionPrice && itemData != null && !string.IsNullOrEmpty(itemData.mockData))
                    {
                        pattern = "(?is)\"sku2info\":(?<skuInfo>.*?)},\"skuItem\"";
                        newResContent = itemData.mockData;
                    }
                    regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match2 = regex.Match(newResContent);
                    if (match2 != null && match2.Success && !string.IsNullOrEmpty(match2.Groups["skuInfo"].Value))
                    {
                        string text2 = match2.Groups["skuInfo"].Value.Replace("\\\"", "\"");
                        if (!string.IsNullOrEmpty(text2))
                        {
                            if (text2.Contains("services"))
                            {
                                regex = new Regex("(?is),\"services\":.*?}][^,]", RegexOptions.IgnoreCase);
                                text2 = regex.Replace(text2, string.Empty);
                            }
                            pattern = "(?is)\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity)[^}]*?}),?|\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity).*?}]})}?,?";
                            regex = new Regex(pattern, RegexOptions.IgnoreCase);
                            MatchCollection matchCollection = regex.Matches(text2);
                            if (matchCollection != null && matchCollection.Count > 0)
                            {
                                foreach (Match item in matchCollection)
                                {
                                    if (item != null && item.Success && !string.IsNullOrEmpty(item.Groups["pathId"].Value))
                                    {
                                        string text3 = DataConvert.ToString((object)item.Groups["priceUnits"].Value);
                                        if (!string.IsNullOrEmpty(text3))
                                        {
                                            Regex regex2 = new Regex("(?is)\"subPrice\":.*?\"priceText\":\"(?<priceText>.*?)\".*?\"quantity\":\"(?<quantity>.*?)\",?", RegexOptions.IgnoreCase);
                                            Match match4 = regex2.Match(text3);
                                            if (match4 != null && match4.Success)
                                            {
                                                Sku2Info_T sku2Info_T = new Sku2Info_T();
                                                sku2Info_T.price = new Price();
                                                sku2Info_T.price.priceText = DataConvert.ToString((object)match4.Groups["priceText"].Value);
                                                sku2Info_T.quantity = DataConvert.ToString((object)match4.Groups["quantity"].Value);
                                                dictionary2[DataConvert.ToString((object)item.Groups["pathId"].Value)] = sku2Info_T;
                                            }
                                            else
                                            {
                                                regex2 = new Regex("(?is)\"priceText\":\"(?<priceText>.*?)\".*?\"quantity\":\"?(?<quantity>.*?)\"?(,|})", RegexOptions.IgnoreCase);
                                                match4 = regex2.Match(text3);
                                                if (match4 != null && match4.Success)
                                                {
                                                    Sku2Info_T sku2Info_T2 = new Sku2Info_T();
                                                    sku2Info_T2.price = new Price();
                                                    sku2Info_T2.price.priceText = DataConvert.ToString((object)match4.Groups["priceText"].Value);
                                                    sku2Info_T2.quantity = DataConvert.ToString((object)match4.Groups["quantity"].Value);
                                                    dictionary2[DataConvert.ToString((object)item.Groups["pathId"].Value)] = sku2Info_T2;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            List<Sku> list = new List<Sku>();
            if (itemJsonEntity60.data.skuBase.skus != null)
            {
                foreach (SkusItem sku in itemJsonEntity60.data.skuBase.skus)
                {
                    Sku val = new Sku();
                    val.Properties = sku.propPath;
                    val.SkuId = DataConvert.ToLong((object)sku.skuId);
                    string text4 = "";
                    string[] array = sku.propPath.Split(';');
                    string[] array2 = array;
                    foreach (string text5 in array2)
                    {
                        if (!string.IsNullOrEmpty(text5) && text5.Split(':').Length == 2)
                        {
                            string text6 = text5.Split(':')[0];
                            string text7 = text5.Split(':')[1];
                            foreach (PropsItem prop in itemJsonEntity60.data.skuBase.props)
                            {
                                if (prop.pid == text6)
                                {
                                    string name = prop.name;
                                    bool flag = false;
                                    foreach (ValuesItem value in prop.values)
                                    {
                                        if (value.vid == text7)
                                        {
                                            string name2 = value.name;
                                            string text8 = text4;
                                            text4 = text8 + text6 + ":" + text7 + ":" + name + ":" + name2 + ";";
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (flag)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    val.PropertiesName = text4.Trim(';');
                    list.Add(val);
                }
            }
            if (list != null && list.Count > 0)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (isTaobao)
                    {
                        if (dictionary != null && dictionary.Count > 0 && dictionary.ContainsKey(list[j].SkuId.ToString()))
                        {
                            Sku2Info sku2Info = dictionary[list[j].SkuId.ToString()];
                            decimal num3 = DataConvert.ToDecimal((object)sku2Info.price.priceText);
                            DataConvert.ToDecimal((object)list[j].Price);
                            if (num3 > 0m)
                            {
                                list[j].Price = Math.Round(num3, 2).ToString();
                                list[j].Quantity = DataConvert.ToLong((object)sku2Info.quantity);
                                if (string.IsNullOrEmpty(price) && list[j].Quantity > 0)
                                {
                                    price = list[j].Price;
                                }
                                else if (DataConvert.ToDecimal((object)price) > DataConvert.ToDecimal((object)list[j].Price) && list[j].Quantity > 0)
                                {
                                    price = list[j].Price;
                                }
                            }
                        }
                    }
                    else if (dictionary2 != null && dictionary2.Count > 0 && dictionary2.ContainsKey(list[j].SkuId.ToString()))
                    {
                        Sku2Info_T sku2Info_T3 = dictionary2[list[j].SkuId.ToString()];
                        decimal num4 = DataConvert.ToDecimal((object)sku2Info_T3.price.priceText);
                        decimal num5 = DataConvert.ToDecimal((object)list[j].Price);
                        if (num4 < num5 || num5 == 0m)
                        {
                            list[j].Price = Math.Round(num4, 2).ToString();
                            list[j].Quantity = DataConvert.ToLong((object)sku2Info_T3.quantity);
                            if (string.IsNullOrEmpty(price) && list[j].Quantity > 0)
                            {
                                price = sku2Info_T3.price.priceText;
                            }
                            if (DataConvert.ToDecimal((object)price) > DataConvert.ToDecimal((object)num4) && list[j].Quantity > 0)
                            {
                                price = list[j].Price;
                            }
                        }
                    }
                    num2 += list[j].Quantity;
                }
            }
            else if (dictionary.ContainsKey("0"))
            {
                Sku2Info sku2Info2 = dictionary["0"];
                price = sku2Info2.price.priceText;
                num2 = DataConvert.ToLong((object)sku2Info2.quantity);
            }
            if (list != null && list.Count > 0)
            {
                foreach (Sku item2 in list)
                {
                    if (DataConvert.ToDecimal((object)item2.Price) <= 0m && DataConvert.ToLong((object)item2.Quantity) == 0)
                    {
                        item2.Price = price;
                    }
                }
            }
            num = num2;
            return list;
        }

        private string GetLocationNew(string apiStack)
        {
            string result = string.Empty;
            string pattern = "(?is)\"from\":\"(?<location>.*?)\"";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(apiStack);
            if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["location"].Value))
            {
                result = match.Groups["location"].Value;
            }
            return result;
        }

        private string GetOnline(string itemContent)
        {
            string pattern = "ERRCODE_QUERY_DETAIL_FAIL::宝贝不存在|\"errorMessage\\\\\":\\\\\"(已下架|当前区域卖光了)\\\\\"|\\\\\"hintBanner\\\\\":{\\\\\"text\\\\\":\\\\\"(已下架|商品已经下架啦~)\\\\\"";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(itemContent);
            string a = string.Empty;
            if (match != null && match.Success)
            {
                a = match.Groups[1].Value.ToString();
            }
            bool flag = true;
            if (a == "当前区域卖光了")
            {
                pattern = "(?is)soldQuantityText.*?\"quantity\":\\\\\"(?<quantity>.*?)\\\\\"";
                regex = new Regex(pattern);
                Match match2 = regex.Match(itemContent);
                if (match2 != null && match2.Success)
                {
                    int num = DataConvert.ToInt((object)match2.Groups["quantity"].Value.ToString());
                    if (num <= 0)
                    {
                        flag = false;
                    }
                }
            }
            if (match != null && match.Success)
            {
                if (a == "当前区域卖光了" && flag)
                {
                    return "onsale";
                }
                return "instock";
            }
            return "onsale";
        }

    }
}
