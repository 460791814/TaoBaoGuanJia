using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;
using Top.Api.Domain;
using Top.Api.Response;

namespace TaoBaoGuanJia.Core.TaoBao
{
   public class DownloadDetailTaobaoService
    {
        private bool IsSnatchSellPoint
        {
            get
            {
                return false;
                //  return DataConvert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", base.ToolCode.ToUpper(), "SnatchSellPoint", "false"));
            }
        }
        private void Download(ItemGetResponse itemGetResponse, DownloadItemInfoViewEntity viewEntity, string onlineKey)
        {
            string empty = string.Empty;
            bool isChkTBShield = false;// DataConvert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", base.ToolCode.ToUpper().ToString(), "CItemPicShieldCheck", "false"));
            ItemDetailTaobaoViewEntity itemDetailTaobaoViewEntity = new ItemDetailTaobaoViewEntity();
            int sysid = viewEntity.SourceShop.Sysid;
            int id = viewEntity.SourceShop.Id;
            IList<Sys_shopSort> sysShopSortList = null;
            Dictionary<string, ItemImg> dicTempPictureNameAndItemImg = null;
            Sp_item prdBaseInfo = this.GetPrdBaseInfo(viewEntity, itemGetResponse.Item, sysid, id, out sysShopSortList, out dicTempPictureNameAndItemImg);
            Sys_sysSort sortBySysIdAndKeys = DataHelper.GetSortBySysIdAndKeys(viewEntity.SourceShop.Sysid, itemGetResponse.Item.Cid.ToString());
            int sortId = 0;
            if (sortBySysIdAndKeys != null)
            {
                sortId = sortBySysIdAndKeys.Id;
            }
            string value = string.Empty;
            string text = string.Concat(new string[]
            {
        DataHelper.GetSysConfig("AppConfig", "Taobao", "3cSort1", ""),
        DataHelper.GetSysConfig("AppConfig", "Taobao", "3cSort2", ""),
        DataHelper.GetSysConfig("AppConfig", "Taobao", "3cSort3", ""),
        DataHelper.GetSysConfig("AppConfig", "Taobao", "3cSort4", ""),
        DataHelper.GetSysConfig("AppConfig", "Taobao", "3cSort5", ""),
        DataHelper.GetSysConfig("AppConfig", "Taobao", "3cSort6", ""),
        DataHelper.GetSysConfig("AppConfig", "Taobao", "3cSort7", "")
            });
            string[] array = text.Split(new char[]
            {
        ','
            });
            if (array != null && array.Length > 0)
            {
                value = (string.IsNullOrEmpty(sortBySysIdAndKeys.Path) ? sortBySysIdAndKeys.Name : (sortBySysIdAndKeys.Path + ">>" + sortBySysIdAndKeys.Name));
            }
            string text2 = string.Empty;
            bool flag = false;
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text3 = array2[i];
                string[] array3 = text3.TrimEnd(new char[]
                {
            '!'
                }).Split(new char[]
                {
            '|'
                });
                if (array3 != null && array3.Length > 0 && array3[0].Equals(value))
                {
                    flag = true;
                    break;
                }
            }
            if (this.IsSnatchSellPoint || flag)
            {
                GatherTaobaoUseWebService gatherTaobaoUseWebService = new GatherTaobaoUseWebService();
                text2 = gatherTaobaoUseWebService.GetItemResponseContent(prdBaseInfo.Showurl);
                if (string.IsNullOrEmpty(text2))
                {
                    text2 = gatherTaobaoUseWebService.GetItemResponseContent(prdBaseInfo.Showurl);
                }
                if (this.IsSnatchSellPoint)
                {
                    string text4 = gatherTaobaoUseWebService.GetSellPoint(text2);
                    if (!string.IsNullOrEmpty(text4))
                    {
                        text4 = text4.Trim();
                        prdBaseInfo.SellPoint = ((text4.Length > 150) ? text4.Substring(0, 150) : text4);
                    }
                }
                if (flag)
                {
                    string number3c = this.GetNumber3c(text2);
                    prdBaseInfo.Operatetypes = number3c.Trim();
                }
            }
            IList<Sys_sysProperty> propertyBySortId = DataHelper.GetPropertyBySortId(sortId);
            DataTable propertyValueDtBySortId = DataHelper.GetPropertyValueDtBySortId(sortId);
            Dictionary<string, SellProInfo> dictParam = new Dictionary<string, SellProInfo>();
            Dictionary<string, SellNewProInfo> sourceProValueAndNewProValue = new Dictionary<string, SellNewProInfo>();
            itemDetailTaobaoViewEntity.SpItem = prdBaseInfo;
            itemDetailTaobaoViewEntity.SpFodSecurity = this.GetSpFoodSecurity(itemGetResponse.Item);
            bool flag2 = sortBySysIdAndKeys != null && sortBySysIdAndKeys.IsNewSellPro;
            Dictionary<string, string> dicSetOldToNew = new Dictionary<string, string>();
            IList<Sp_property> spPropertyList;
            if (flag2)
            {
                spPropertyList = this.SetNewspPropertyList(itemGetResponse.Item, propertyBySortId, propertyValueDtBySortId, sortId, out dictParam, out empty, out dicSetOldToNew, out sourceProValueAndNewProValue);
            }
            else
            {
                spPropertyList = this.SetspPropertyList(itemGetResponse.Item, propertyBySortId, propertyValueDtBySortId, sortId, out dictParam, out empty, out sourceProValueAndNewProValue);
            }
            itemDetailTaobaoViewEntity.SpPropertyList = this.GetSpPropertyList(spPropertyList, id, sysid);
            IList<Sys_sysProperty> sellPropertyBySortId = DataHelper.GetSellPropertyBySortId(sortId);
            decimal price = prdBaseInfo.Price;
            GetItemPromoPrice.TaobaoPromoPriceEntity entity = null;
            Dictionary<string, string> dictionary = null;
            if (sellPropertyBySortId != null && sellPropertyBySortId.Count > 0)
            {
                int nums = 0;
                if (dictionary != null)
                {
                    itemDetailTaobaoViewEntity.SpSellPropertyList = this.GetSpSellPropertyList(itemGetResponse.Item, viewEntity.SourceShop.Sysid, viewEntity.SourceShop.Id, dictParam, dictionary, ref price, dicSetOldToNew, out nums);
                }
                else
                {
                    itemDetailTaobaoViewEntity.SpSellPropertyList = this.GetSpSellPropertyList(itemGetResponse.Item, viewEntity.SourceShop.Sysid, viewEntity.SourceShop.Id, dictParam, entity, ref price, sourceProValueAndNewProValue, dicSetOldToNew, out nums);
                }
                if (itemDetailTaobaoViewEntity.SpSellPropertyList != null && itemDetailTaobaoViewEntity.SpSellPropertyList.Count > 0)
                {
                    itemDetailTaobaoViewEntity.SpItem.Nums = nums;
                }
            }
            itemDetailTaobaoViewEntity.SpPicturesList = this.TransAndSavePicture(dicTempPictureNameAndItemImg, base.ToolCode, onlineKey, isChkTBShield, itemGetResponse.Item.Cid.ToString());
            if (itemDetailTaobaoViewEntity.SpPicturesList != null && itemDetailTaobaoViewEntity.SpPicturesList.Count > 0)
            {
                itemDetailTaobaoViewEntity.SpItem.Photo = itemDetailTaobaoViewEntity.SpPicturesList[0].Localpath;
            }
            itemDetailTaobaoViewEntity.SpItemContent = (viewEntity.DownloadAllDetail ? this.GetSpItemContent(itemGetResponse.Item.Desc, itemGetResponse.Item.WirelessDesc, isChkTBShield, itemGetResponse.Item.Cid.ToString()) : null);
            itemDetailTaobaoViewEntity.SpShopSortList = this.GetSpShopSortList(sysShopSortList, id);
            itemDetailTaobaoViewEntity.SpSysSort = this.GetSpSysSort(sortBySysIdAndKeys, id, sysid, sortId);
            if (this._inExc)
            {
                this._downloadDetailTaobaoDao.SaveItemDetail(itemDetailTaobaoViewEntity);
            }
        }
        private string GetNumber3c(string downloadResponseContent)
        {
            string text = string.Empty;
            if (string.IsNullOrEmpty(downloadResponseContent))
            {
                return text;
            }
            string pattern = "<li\\s*title=\"?(?<number3c>[^>]*?)\"?\\s*>.*?证书编号.*?[^<]*?</li>";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(downloadResponseContent);
            if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["number3c"].Value))
            {
                text = match.Groups["number3c"].Value;
            }
            if (string.IsNullOrEmpty(text))
            {
                string pattern2 = "baike.taobao.com/view.htm?[^\"]*?wd=(?<number3c>\\d*)[^\"]*?\"";
                regex = new Regex(pattern2);
                match = regex.Match(downloadResponseContent);
                if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["number3c"].Value))
                {
                    text = match.Groups["number3c"].Value;
                }
            }
            return text;
        }
        private Sp_foodSecurity GetSpFoodSecurity(Item item)
        {
            if (item == null || item.FoodSecurity == null)
            {
                return null;
            }
            FoodSecurity foodSecurity = item.FoodSecurity;
            Sp_foodSecurity sp_foodSecurity = new Sp_foodSecurity();
            sp_foodSecurity.Contact = foodSecurity.Contact;
            sp_foodSecurity.Designcode = foodSecurity.DesignCode;
            sp_foodSecurity.Factory = foodSecurity.Factory;
            sp_foodSecurity.Factorysite = foodSecurity.FactorySite;
            sp_foodSecurity.Foodadditive = foodSecurity.FoodAdditive;
            sp_foodSecurity.Mix = foodSecurity.Mix;
            sp_foodSecurity.Period = foodSecurity.Period;
            sp_foodSecurity.Planstorage = foodSecurity.PlanStorage;
            if (!string.IsNullOrEmpty(foodSecurity.PrdLicenseNo))
            {
                sp_foodSecurity.Prdlicenseno = foodSecurity.PrdLicenseNo.Replace(" ", "");
            }
            sp_foodSecurity.HealthProductNo = foodSecurity.HealthProductNo;
            if (!string.IsNullOrEmpty(foodSecurity.ProductDateEnd))
            {
                sp_foodSecurity.Productdateend = DateTime.Parse(DataConvert.ToDateTime(foodSecurity.ProductDateEnd).ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(foodSecurity.ProductDateStart))
            {
                sp_foodSecurity.Productdatestart = DateTime.Parse(DataConvert.ToDateTime(foodSecurity.ProductDateStart).ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(foodSecurity.StockDateEnd))
            {
                sp_foodSecurity.Stockdateend = DateTime.Parse(DataConvert.ToDateTime(foodSecurity.StockDateEnd).ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(foodSecurity.StockDateStart))
            {
                sp_foodSecurity.Stockdatestart = DateTime.Parse(DataConvert.ToDateTime(foodSecurity.StockDateStart).ToString("yyyy-MM-dd"));
            }
            if (sp_foodSecurity.Productdatestart == sp_foodSecurity.Stockdatestart)
            {
                sp_foodSecurity.Stockdatestart = sp_foodSecurity.Productdatestart.AddDays(1.0);
            }
            if (sp_foodSecurity.Stockdateend < sp_foodSecurity.Stockdatestart)
            {
                sp_foodSecurity.Stockdateend = sp_foodSecurity.Stockdatestart;
            }
            if (!string.IsNullOrEmpty(foodSecurity.Supplier))
            {
                sp_foodSecurity.Supplier = foodSecurity.Supplier.Trim(new char[]
                {
            '"'
                });
            }
            return sp_foodSecurity;
        }
        // Sszg.Tool.ComModule.Download.Service.DownloadDetailTaobaoService
        private Sp_item GetPrdBaseInfo(DownloadItemInfoViewEntity viewEntity, Item item, int sysId, int shopId, out IList<Sys_shopSort> sysShopSortList, out Dictionary<string, ItemImg> dicPictureAndImg)
        {
            sysShopSortList = new List<Sys_shopSort>();
            dicPictureAndImg = new Dictionary<string, ItemImg>();
            List<ItemImg> list = null;
            if (item.ItemImgs != null)
            {
                if (viewEntity.DownloadAllDetail)
                {
                    list = new List<ItemImg>();
                    foreach (ItemImg current in item.ItemImgs)
                    {
                        list.Add(current);
                    }
                }
                if (viewEntity.OnlyDownloadMajorImg)
                {
                    list = new List<ItemImg>();
                    if (item.ItemImgs.Count > 0)
                    {
                        list.Add(item.ItemImgs[0]);
                    }
                }
            }
            dicPictureAndImg = this.GetDicTempPictureNameAndItemImg(list);
            Sp_item sp_item = new Sp_item();
            sp_item.Name = item.Title;
            if (!string.IsNullOrEmpty(sp_item.Name))
            {
                sp_item.Name = HttpUtility.HtmlDecode(sp_item.Name).Replace(">", "＞").Replace("<", "＜").Replace("&", "-");
            }
            sp_item.Sysid = sysId;
            sp_item.Shopid = shopId;
            sp_item.Barcode = item.Barcode;
            sp_item.Neworold = this.GetItzemStuffStatus(item.StuffStatus);
            this.GetProAndCity(item.Location, sysId, ref sp_item);
            this.GetTypeAndName(item.Type, ref sp_item);
            sp_item.Price = DataConvert.ToDecimal(item.Price);
            sp_item.Pricerise = DataConvert.ToDecimal(item.Increment);
            sp_item.Nums = DataConvert.ToInt(item.Num);
            sp_item.Validdate = DataConvert.ToString(item.ValidThru);
            this.GetShipWay(item.FreightPayer, item.PostageId, shopId, ref sp_item);
            sp_item.Shipslow = DataConvert.ToDecimal(item.PostFee);
            sp_item.Shipfast = DataConvert.ToDecimal(item.ExpressFee);
            sp_item.Shipems = DataConvert.ToDecimal(item.EmsFee);
            this.GetState(item.ApproveStatus, item.Num, item.Violation, item.IsTiming, item.ListTime, ref sp_item);
            sp_item.Isrmd = DataConvert.ToString(item.HasShowcase);
            sp_item.Isreturn = DataConvert.ToString(item.SellPromise);
            sp_item.Discount = DataConvert.ToString(item.HasDiscount);
            sp_item.Isticket = (item.HasInvoice ? "1" : "0");
            sp_item.Ticketname = (item.HasInvoice ? "有" : "无");
            sp_item.Isrepair = (item.HasWarranty ? "1" : "0");
            sp_item.Repairname = (item.HasWarranty ? "有" : "无");
            sp_item.Sszgusername = ""; 
            string text = item.PropsName;
            sp_item.Kcitemname = "";
            if (!string.IsNullOrEmpty(text) && text.IndexOf("货号") >= 0)
            {
                int num = text.IndexOf("货号");
                if (num + 3 < text.Length)
                {
                    text = text.Substring(num + 3);
                }
                num = text.IndexOf(';');
                if (num > 0)
                {
                    text = text.Substring(0, text.IndexOf(';'));
                }
                if (text.Length > 20)
                {
                    text = text.Substring(0, 20);
                }
                sp_item.Code = text;
                sp_item.Kcitemname = text;
            }
            else
            {
                sp_item.Code = item.OuterId;
            }
            sp_item.Onlinekey = DataConvert.ToString(item.NumIid);
            sp_item.Listtime = DataConvert.ToDateTime(item.ListTime);
            sp_item.Delisttime = DataConvert.ToDateTime(item.DelistTime);
            sp_item.Detailurl = "http://upload.taobao.com/auction/publish/edit.htm?item_num_id=" + item.NumIid + "&auto=false";
            sp_item.Showurl = item.DetailUrl;
            sp_item.Del = 0;
            this.GetShopSortIdAndName(item.SellerCids, ref sp_item, ref sysShopSortList, shopId);
            sp_item.Synchdetail = 1;
            sp_item.Synchstate = 1;
            sp_item.Aftersaleid = DataConvert.ToInt(item.AfterSaleId);
            if (!string.IsNullOrEmpty(item.ItemSize))
            {
                if (item.ItemSize.StartsWith("bulk:"))
                {
                    sp_item.Size = item.ItemSize;
                }
                else
                {
                    sp_item.Size = "bulk:" + item.ItemSize;
                }
            }
            sp_item.Weight = DataConvert.ToDecimal(item.ItemWeight);
            sp_item.Substock = DataConvert.ToInt(item.SubStock);
            return sp_item;
        }
        private Dictionary<string, ItemImg> GetDicTempPictureNameAndItemImg(List<ItemImg> lstItemImg)
        {
            Dictionary<string, ItemImg> dictionary = new Dictionary<string, ItemImg>();
            if (lstItemImg == null || lstItemImg.Count <= 0)
            {
                return dictionary;
            }
            string text = string.Empty;
            string arg_1F_0 = string.Empty;
            ImageOperator imageOperator = new ImageOperator();
            for (int i = 0; i < lstItemImg.Count; i++)
            {
                ItemImg itemImg = lstItemImg[i];
                if (!string.IsNullOrEmpty(itemImg.Url))
                {
                    text = imageOperator.DownLoadPicture(itemImg.Url, null, 60000, 60000);
                    if (!string.IsNullOrEmpty(text) && !dictionary.ContainsKey(text))
                    {
                        dictionary.Add(text, itemImg);
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        Log.WriteLog("下载淘宝商品主图" + itemImg.Url + "失败");
                    }
                }
            }
            return dictionary;
        }
        private string GetItzemStuffStatus(string stuffStatus)
        {
            if (stuffStatus != null)
            {
                if (stuffStatus == "new")
                {
                    return "5";
                }
                if (stuffStatus == "unused")
                {
                    return "8";
                }
                if (stuffStatus == "second")
                {
                    return "6";
                }
            }
            return string.Empty;
        }
        private void GetProAndCity(Location location, int sysId, ref Sp_item spItem)
        {
            if (location != null)
            {
                if (location.State != null)
                {
                    Sys_sysAddress addressBySysIdAndCode = DataHelper.GetAddressBySysIdAndCode(sysId, location.State);
                    spItem.Province = ((addressBySysIdAndCode != null) ? addressBySysIdAndCode.Addrcode : string.Empty);
                    spItem.Provincename = location.State;
                }
                if (location.City != null)
                {
                    Sys_sysAddress addressBySysIdAndCode2 = DataHelper.GetAddressBySysIdAndCode(sysId, location.City);
                    spItem.City = ((addressBySysIdAndCode2 != null) ? addressBySysIdAndCode2.Addrcode : string.Empty);
                    spItem.Cityname = location.City;
                }
            }
        }
        private void GetTypeAndName(string itemType, ref Sp_item spItem)
        {
            if (itemType != null)
            {
                if (itemType == "fixed")
                {
                    spItem.Selltype = "b";
                    spItem.Selltypename = "一口价";
                    return;
                }
                if (!(itemType == "auction"))
                {
                    return;
                }
                spItem.Selltype = "a";
                spItem.Selltypename = "拍卖";
            }
        }
        private void GetShipWay(string freightPayer, long postageId, int shopId, ref Sp_item spItem)
        {
            if (freightPayer != null)
            {
                if (freightPayer == "seller")
                {
                    spItem.Shipway = "2";
                    spItem.Shipwayname = "卖家承担";
                    spItem.Useshiptpl = "false";
                    return;
                }
                if (!(freightPayer == "buyer"))
                {
                    return;
                }
                spItem.Shipway = "1";
                if (postageId == 0L)
                {
                    spItem.Shipwayname = "买家承担";
                    spItem.Useshiptpl = "false";
                    return;
                }
                spItem.Shipwayname = "运费模板";
                spItem.Useshiptpl = "true";
                if (postageId > 0L)
                {
                    IList<Sys_shopShip> shopShipsByShopId = DataHelper.GetShopShipsByShopId(shopId);
                    foreach (Sys_shopShip current in shopShipsByShopId)
                    {
                        if (current.Keys.Equals(DataConvert.ToString(postageId)))
                        {
                            spItem.Shiptplid = current.Id;
                            break;
                        }
                    }
                }
            }
        }
        private void GetState(string approveStatus, long num, bool violation, bool isTiming, string listTime, ref Sp_item spItem)
        {
            if (approveStatus != null)
            {
                if (approveStatus == "onsale")
                {
                    spItem.State = 1;
                    spItem.Onsell = "0";
                    return;
                }
                if (!(approveStatus == "instock"))
                {
                    return;
                }
                if (num == 0L)
                {
                    spItem.State = 9;
                }
                else
                {
                    if (violation)
                    {
                        spItem.State = -1;
                    }
                    else
                    {
                        spItem.State = 0;
                    }
                }
                if (isTiming)
                {
                    spItem.Onsell = "1";
                    DateTime dateTime = DataConvert.ToDateTime(listTime);
                    spItem.Onselldate = dateTime.Date;
                    spItem.Onsellhour = DataConvert.ToString(dateTime.Hour);
                    spItem.Onsellmin = DataConvert.ToString(dateTime.Minute);
                    return;
                }
                spItem.Onsell = "2";
            }
        }
        private void GetShopSortIdAndName(string sellerCids, ref Sp_item spItem, ref IList<Sys_shopSort> list, int shopId)
        {
            if (string.IsNullOrEmpty(sellerCids))
            {
                spItem.Shopsortids = string.Empty;
                spItem.Shopsortnames = string.Empty;
                return;
            }
            if (string.IsNullOrEmpty(sellerCids))
            {
                return;
            }
            string[] array = sellerCids.Split(new string[]
            {
        ","
            }, StringSplitOptions.RemoveEmptyEntries);
            IList<Sys_shopSort> shopSortsByShopId = DataHelper.GetShopSortsByShopId(shopId);
            foreach (Sys_shopSort current in shopSortsByShopId)
            {
                if (current.Isdelete == 0)
                {
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string value = array2[i];
                        if (current.Keyinshop.Equals(value))
                        {
                            list.Add(current);
                        }
                    }
                }
            }
            if (list != null && list.Count > 0)
            {
                string text = string.Empty;
                string text2 = string.Empty;
                foreach (Sys_shopSort current2 in list)
                {
                    text = text + DataConvert.ToString(current2.Id) + ",";
                    text2 = text2 + DataConvert.ToString(current2.Sortname) + ",";
                }
                spItem.Shopsortids = "," + text;
                spItem.Shopsortnames = text2.Substring(0, text2.Length - 1);
            }
        }

        private IList<Sp_property> SetspPropertyList(Item item, IList<Sys_sysProperty> sysSysPropertyList, DataTable dtPropertyValue, int sortId, out Dictionary<string, SellProInfo> dicProValueAndSellProInfos, out string code, out Dictionary<string, SellNewProInfo> sourceProValueAndNewProValue)
        {
            bool flag = DataConvert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", base.ToolCode.ToUpper().ToString(), "CItemPicShieldCheck", "false"));
            sourceProValueAndNewProValue = new Dictionary<string, SellNewProInfo>();
            bool flag2 = Convert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", "PicSet", "SellProperyPic", "false"));
            code = string.Empty;
            IList<Sp_property> list = new List<Sp_property>();
            IList<Sys_sysProperty> propertyBySortId = DataHelper.GetPropertyBySortId(sortId);
            Dictionary<string, Sys_sysProperty> dictionary = new Dictionary<string, Sys_sysProperty>();
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            List<string> list2 = new List<string>();
            if (sysSysPropertyList != null)
            {
                foreach (Sys_sysProperty current in sysSysPropertyList)
                {
                    if (current.Valuetype == 2 && current.Parentid == 0)
                    {
                        dictionary[current.Keys] = current;
                    }
                    if (current.Parentid == 0)
                    {
                        dictionary2[current.Keys] = DataConvert.ToString(current.Id);
                    }
                    if (current.Issellpro)
                    {
                        list2.Add(DataConvert.ToString(current.Id));
                    }
                }
            }
            Dictionary<string, DataRow> dictionary3 = new Dictionary<string, DataRow>();
            foreach (DataRow dataRow in dtPropertyValue.Rows)
            {
                dictionary3[DataConvert.ToString(dataRow["Value"])] = dataRow;
            }
            dicProValueAndSellProInfos = new Dictionary<string, SellProInfo>();
            if (item == null)
            {
                return list;
            }
            Dictionary<string, PropertyAliasViewEntity> dictionary4 = new Dictionary<string, PropertyAliasViewEntity>();
            PropertyAliasViewEntity propertyAliasViewEntity = new PropertyAliasViewEntity();
            string text = string.Empty;
            string str = string.Empty;
            if (item.PropImgs != null && item.PropImgs.Count > 0)
            {
                foreach (PropImg current2 in item.PropImgs)
                {
                    propertyAliasViewEntity = new PropertyAliasViewEntity();
                    text = current2.Properties;
                    propertyAliasViewEntity.Value = text;
                    propertyAliasViewEntity.ImageUrl = current2.Url;
                    dictionary4[text] = propertyAliasViewEntity;
                }
            }
            if (!string.IsNullOrEmpty(item.PropertyAlias))
            {
                Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
                int num = 0;
                string[] array = item.PropertyAlias.Split(new string[]
                {
            ";"
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    string text2 = array[i];
                    int num2 = text2.LastIndexOf(":");
                    if (num2 > 0)
                    {
                        text = text2.Substring(0, num2);
                        str = text2.Substring(num2 + 1);
                        str = this.SubStringToMaxByteLength(str, 30);
                        if (dictionary4.ContainsKey(text))
                        {
                            propertyAliasViewEntity = dictionary4[text];
                        }
                        else
                        {
                            propertyAliasViewEntity = new PropertyAliasViewEntity();
                        }
                        propertyAliasViewEntity.Value = text2.Substring(0, num2);
                        propertyAliasViewEntity.Alias = HttpUtility.UrlDecode(text2.Substring(num2 + 1));
                        propertyAliasViewEntity.Alias = this.SubStringToMaxByteLength(propertyAliasViewEntity.Alias, 30);
                        if (!string.IsNullOrEmpty(propertyAliasViewEntity.Alias))
                        {
                            if (dictionary5.ContainsKey(propertyAliasViewEntity.Alias))
                            {
                                num++;
                                string text3 = propertyAliasViewEntity.Alias.Substring(0, propertyAliasViewEntity.Alias.Length - 1) + num;
                                propertyAliasViewEntity.Alias = text3;
                                dictionary4[propertyAliasViewEntity.Value] = propertyAliasViewEntity;
                                dictionary5[text3] = propertyAliasViewEntity.Value;
                            }
                            else
                            {
                                dictionary4[propertyAliasViewEntity.Value] = propertyAliasViewEntity;
                                dictionary5[propertyAliasViewEntity.Alias] = propertyAliasViewEntity.Value;
                            }
                        }
                        else
                        {
                            dictionary4[propertyAliasViewEntity.Value] = propertyAliasViewEntity;
                        }
                    }
                }
            }
            string arg_3E1_0 = string.Empty;
            string text4 = string.Empty;
            string proKey = string.Empty;
            string text5 = string.Empty;
            string name = string.Empty;
            Dictionary<string, string> inputPidAndStr = this.GetInputPidAndStr(item);
            ImageOperator imageOperator = new ImageOperator();
            List<PropsViewEntity> lstPropsDTO = this.GetLstPropsDTO(item.PropsName);
            Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
            Dictionary<string, string> dictionary7 = new Dictionary<string, string>();
            foreach (PropsViewEntity current3 in lstPropsDTO)
            {
                dictionary6[current3.PropertyName] = current3.PropertyValueName;
                dictionary7[current3.PropertyValue] = current3.PropertyValueName;
            }
            foreach (KeyValuePair<string, PropertyAliasViewEntity> current4 in dictionary4)
            {
                if (current4.Value != null && string.IsNullOrEmpty(current4.Value.Alias) && dictionary7.ContainsKey(current4.Key))
                {
                    current4.Value.Alias = dictionary7[current4.Key];
                    byte[] bytes = Encoding.Default.GetBytes(current4.Value.Alias);
                    int num3 = bytes.Length;
                    if (num3 > 30)
                    {
                        string text6 = Encoding.Default.GetString(bytes, 0, 30);
                        if (text6.EndsWith("?"))
                        {
                            text6 = text6.TrimEnd(new char[]
                            {
                        '?'
                            });
                        }
                        current4.Value.Alias = text6;
                    }
                }
            }
            IList<PropsViewEntity> list3 = new List<PropsViewEntity>();
            Dictionary<string, string> dictionary8 = new Dictionary<string, string>();
            List<PropsViewEntity> list4 = new List<PropsViewEntity>();
            for (int j = 0; j < lstPropsDTO.Count; j++)
            {
                string propertyValueName = lstPropsDTO[j].PropertyValueName;
                string propertyName = lstPropsDTO[j].PropertyName;
                for (int k = j + 1; k < lstPropsDTO.Count; k++)
                {
                    string propertyValueName2 = lstPropsDTO[k].PropertyValueName;
                    string propertyName2 = lstPropsDTO[k].PropertyName;
                    if (propertyValueName == propertyValueName2 && propertyName == propertyName2 && propertyName.Contains("颜色"))
                    {
                        list4.Add(lstPropsDTO[k]);
                    }
                }
            }
            foreach (PropsViewEntity current5 in list4)
            {
                lstPropsDTO.Remove(current5);
            }
            new Dictionary<string, string>();
            for (int l = 0; l < lstPropsDTO.Count; l++)
            {
                byte[] bytes2 = Encoding.Default.GetBytes(lstPropsDTO[l].PropertyValueName);
                int num4 = bytes2.Length;
                if (num4 > 30)
                {
                    string text7 = Encoding.Default.GetString(bytes2, 0, 30);
                    if (text7.EndsWith("?"))
                    {
                        text7 = text7.TrimEnd(new char[]
                        {
                    '?'
                        });
                    }
                    if (inputPidAndStr.ContainsKey(text7))
                    {
                        text7 += l;
                    }
                    else
                    {
                        inputPidAndStr[text7] = text7;
                    }
                    lstPropsDTO[l].PropertyValueName = text7;
                }
            }
            Dictionary<string, int> dictionary9 = new Dictionary<string, int>();
            foreach (PropsViewEntity current6 in lstPropsDTO)
            {
                DataRow dataRow2 = null;
                Sp_property sp_property = new Sp_property();
                int num2 = current6.PropertyValue.IndexOf(":");
                if (num2 != 0)
                {
                    proKey = current6.PropertyValue.Remove(num2);
                    if (dictionary.ContainsKey(proKey))
                    {
                        text4 = DataConvert.ToString(dictionary[proKey].Id);
                        Sys_sysProperty sys_sysProperty = dictionary[proKey];
                        if (sys_sysProperty != null && sys_sysProperty.Name != null && (sys_sysProperty.Name.EndsWith("货号") || sys_sysProperty.Name.EndsWith("款号")))
                        {
                            code = current6.PropertyValueName;
                        }
                        sp_property.Propertyid = DataConvert.ToInt(text4);
                        sp_property.Issellpro = (list2.Contains(text4) ? 1 : 0);
                        if (dictionary[proKey].Valuetype == 2 && dictionary[proKey].Issellpro)
                        {
                            SellNewProInfo sellNewProInfo = new SellNewProInfo();
                            string value = string.Empty;
                            if (dictionary9 == null || dictionary9.Count == 0)
                            {
                                value = proKey + ":-1001";
                                dictionary9.Add(proKey, -1001);
                                sp_property.Name = current6.PropertyValueName;
                                sp_property.Value = value;
                            }
                            else
                            {
                                if (dictionary9.ContainsKey(proKey))
                                {
                                    Dictionary<string, int> dictionary10;
                                    string proKey2;
                                    (dictionary10 = dictionary9)[proKey2 = proKey] = dictionary10[proKey2] + -1;
                                    value = proKey + ":" + dictionary9[proKey];
                                    sp_property.Name = current6.PropertyValueName;
                                    sp_property.Value = value;
                                }
                                else
                                {
                                    value = proKey + ":-1001";
                                    dictionary9.Add(proKey, -1001);
                                    sp_property.Name = current6.PropertyValueName;
                                    sp_property.Value = value;
                                }
                            }
                            sellNewProInfo.Value = value;
                            sellNewProInfo.Name = current6.PropertyValueName;
                            sourceProValueAndNewProValue[current6.PropertyValue] = sellNewProInfo;
                        }
                        else
                        {
                            sp_property.Value = current6.PropertyValueName;
                        }
                        list.Add(sp_property);
                        dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                    }
                    else
                    {
                        text5 = current6.PropertyValue;
                        if (dictionary3.ContainsKey(text5))
                        {
                            dataRow2 = dictionary3[text5];
                        }
                        else
                        {
                            if (current6.PropertyValueName.IndexOf("'") >= 0)
                            {
                                current6.PropertyValueName = current6.PropertyValueName.Replace("'", "''");
                            }
                            DataRow[] array2 = dtPropertyValue.Select(string.Concat(new string[]
                            {
                        "name='",
                        current6.PropertyValueName,
                        "' and value like '%",
                        proKey,
                        "%'"
                            }));
                            if (array2 != null && array2.Length > 0)
                            {
                                dataRow2 = array2[0];
                            }
                            if (dataRow2 == null)
                            {
                                text5 = proKey + ":-1";
                                if (dictionary3.ContainsKey(text5))
                                {
                                    dataRow2 = dictionary3[text5];
                                }
                            }
                        }
                        if (dataRow2 != null)
                        {
                            text4 = DataConvert.ToString(dataRow2["Propertyid"]);
                            sp_property.Propertyid = DataConvert.ToInt(text4);
                            sp_property.Value = DataConvert.ToString(dataRow2["Value"]);
                            sp_property.Name = DataConvert.ToString(dataRow2["Name"]);
                            sp_property.Issellpro = (list2.Contains(text4) ? 1 : 0);
                            string propertyValueName3 = current6.PropertyValueName;
                            list.Add(sp_property);
                            dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                            if (dictionary4.ContainsKey(text5))
                            {
                                propertyAliasViewEntity = dictionary4[text5];
                                name = propertyAliasViewEntity.Alias;
                                using (IEnumerator<Sys_sysProperty> enumerator = propertyBySortId.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        Sys_sysProperty current7 = enumerator.Current;
                                        if (current7.Parentprovalueid == DataConvert.ToInt(dataRow2["Id"]))
                                        {
                                            sp_property = new Sp_property();
                                            int valuetype = current7.Valuetype;
                                            sp_property = new Sp_property();
                                            text4 = DataConvert.ToString(current7.Id);
                                            sp_property.Propertyid = current7.Id;
                                            sp_property.Issellpro = (list2.Contains(text4) ? 1 : 0);
                                            if (valuetype == 2)
                                            {
                                                sp_property.Name = propertyAliasViewEntity.Alias;
                                                sp_property.Value = propertyAliasViewEntity.Alias;
                                                sp_property.Issellpro = 1;
                                                list.Add(sp_property);
                                            }
                                            else
                                            {
                                                if (valuetype == 3 && !string.IsNullOrEmpty(propertyAliasViewEntity.ImageUrl))
                                                {
                                                    string text8 = imageOperator.DownLoadPicture(propertyAliasViewEntity.ImageUrl, null, 60000, 60000);
                                                    text8 = imageOperator.TransPicture(text8, base.ToolCode, item.NumIid.ToString(), TransPictureType.MovePicture);
                                                    if (flag2 && !string.IsNullOrEmpty(text8))
                                                    {
                                                        text8 = string.Empty;
                                                    }
                                                    if (!string.IsNullOrEmpty(text8))
                                                    {
                                                        sp_property.Name = text8;
                                                        sp_property.Value = text8;
                                                        sp_property.Issellpro = 1;
                                                        if (base.ToolCode == "UPLOADTOYF")
                                                        {
                                                            sp_property.PicUrl = propertyAliasViewEntity.ImageUrl;
                                                        }
                                                        if (flag)
                                                        {
                                                            sp_property.Url = propertyAliasViewEntity.ImageUrl;
                                                        }
                                                        list.Add(sp_property);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    goto IL_E0D;
                                }
                                goto IL_D08;
                            }
                            goto IL_D08;
                            IL_E0D:
                            SellProInfo sellProInfo = new SellProInfo();
                            sellProInfo.Value = DataConvert.ToString(dataRow2["Propertyid"]) + ":" + DataConvert.ToString(dataRow2["Id"]);
                            sellProInfo.Name = name;
                            dicProValueAndSellProInfos[text5] = sellProInfo;
                            this.Haschildproperty(DataConvert.ToBoolean(dataRow2["Haschildproperty"]), sysSysPropertyList, text5, dictionary6, list2, ref list);
                            continue;
                            IL_D08:
                            if (sp_property.Issellpro == 1 && !propertyValueName3.Trim().Equals(sp_property.Name.Trim()))
                            {
                                name = propertyValueName3;
                                using (IEnumerator<Sys_sysProperty> enumerator = propertyBySortId.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        Sys_sysProperty current8 = enumerator.Current;
                                        if (current8.Parentprovalueid == DataConvert.ToInt(dataRow2["Id"]))
                                        {
                                            sp_property = new Sp_property();
                                            int valuetype2 = current8.Valuetype;
                                            sp_property = new Sp_property();
                                            text4 = DataConvert.ToString(current8.Id);
                                            sp_property.Propertyid = current8.Id;
                                            sp_property.Issellpro = (list2.Contains(text4) ? 1 : 0);
                                            if (valuetype2 == 2)
                                            {
                                                sp_property.Name = propertyValueName3;
                                                sp_property.Value = propertyValueName3;
                                                sp_property.Issellpro = 1;
                                                list.Add(sp_property);
                                            }
                                        }
                                    }
                                    goto IL_E0D;
                                }
                            }
                            name = DataConvert.ToString(dataRow2["Name"]).Trim();
                            goto IL_E0D;
                        }
                        list3.Add(current6);
                    }
                }
            }
            foreach (PropsViewEntity current9 in list3)
            {
                DataRow dataRow3 = null;
                Sp_property sp_property = new Sp_property();
                int num2 = current9.PropertyValue.IndexOf(":");
                if (num2 != 0)
                {
                    proKey = current9.PropertyValue.Remove(num2);
                    text5 = current9.PropertyValue;
                    DataRow[] array3 = dtPropertyValue.Select("value like '%" + proKey + "%'");
                    if (array3 != null && array3.Length > 0)
                    {
                        int num5 = DataConvert.ToInt(array3[0]["Propertyid"]);
                        Sys_sysProperty propertyById = ToolServer.ProductData.GetPropertyById(num5);
                        if (propertyById != null)
                        {
                            bool flag3 = false;
                            DataRow[] array4 = array3;
                            for (int i = 0; i < array4.Length; i++)
                            {
                                DataRow dataRow4 = array4[i];
                                if (!dictionary8.ContainsKey(num5.ToString() + DataConvert.ToString(dataRow4["Value"])))
                                {
                                    string value2 = DataConvert.ToString(dataRow4["Name"]);
                                    if (!string.IsNullOrEmpty(value2) && current9.PropertyValueName.IndexOf(value2, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        dataRow3 = dataRow4;
                                        flag3 = true;
                                        break;
                                    }
                                }
                            }
                            if (!flag3 && propertyById.Issellpro)
                            {
                                array4 = array3;
                                for (int i = 0; i < array4.Length; i++)
                                {
                                    DataRow dataRow5 = array4[i];
                                    if (!dictionary8.ContainsKey(num5.ToString() + DataConvert.ToString(dataRow5["Value"])))
                                    {
                                        dataRow3 = dataRow5;
                                        break;
                                    }
                                }
                                if (array3 != null && array3.Length > 0 && dataRow3 != null && !DataConvert.ToBoolean(dataRow3["Haschildproperty"]))
                                {
                                    SellNewProInfo sellNewProInfo2 = new SellNewProInfo();
                                    sp_property.Propertyid = num5;
                                    sp_property.Issellpro = (list2.Contains(DataConvert.ToString(num5)) ? 1 : 0);
                                    string value3 = string.Empty;
                                    if (dictionary9 == null || dictionary9.Count == 0)
                                    {
                                        value3 = proKey + ":-1001";
                                        dictionary9.Add(proKey, -1001);
                                        sp_property.Name = current9.PropertyValueName;
                                        sp_property.Value = value3;
                                    }
                                    else
                                    {
                                        if (dictionary9.ContainsKey(proKey))
                                        {
                                            Dictionary<string, int> dictionary10;
                                            string proKey2;
                                            (dictionary10 = dictionary9)[proKey2 = proKey] = dictionary10[proKey2] + -1;
                                            value3 = proKey + ":" + dictionary9[proKey];
                                            sp_property.Name = current9.PropertyValueName;
                                            sp_property.Value = value3;
                                        }
                                    }
                                    sellNewProInfo2.Value = value3;
                                    sellNewProInfo2.Name = current9.PropertyValueName;
                                    sourceProValueAndNewProValue[current9.PropertyValue] = sellNewProInfo2;
                                    list.Add(sp_property);
                                    dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                                    continue;
                                }
                            }
                        }
                    }
                    if (dataRow3 != null)
                    {
                        text4 = DataConvert.ToString(dataRow3["Propertyid"]);
                        sp_property.Propertyid = DataConvert.ToInt(text4);
                        sp_property.Value = DataConvert.ToString(dataRow3["Value"]);
                        sp_property.Name = DataConvert.ToString(dataRow3["Name"]);
                        sp_property.Issellpro = (list2.Contains(text4) ? 1 : 0);
                        string propertyValueName4 = current9.PropertyValueName;
                        list.Add(sp_property);
                        dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                        if (dictionary4.ContainsKey(text5))
                        {
                            propertyAliasViewEntity = dictionary4[text5];
                            name = propertyAliasViewEntity.Alias;
                            using (IEnumerator<Sys_sysProperty> enumerator = propertyBySortId.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    Sys_sysProperty current10 = enumerator.Current;
                                    if (current10.Parentprovalueid == DataConvert.ToInt(dataRow3["Id"]))
                                    {
                                        sp_property = new Sp_property();
                                        int valuetype3 = current10.Valuetype;
                                        sp_property = new Sp_property();
                                        text4 = DataConvert.ToString(current10.Id);
                                        sp_property.Propertyid = current10.Id;
                                        sp_property.Issellpro = (list2.Contains(text4) ? 1 : 0);
                                        if (valuetype3 == 2)
                                        {
                                            sp_property.Name = propertyAliasViewEntity.Alias;
                                            sp_property.Value = propertyAliasViewEntity.Alias;
                                            sp_property.Issellpro = 1;
                                            list.Add(sp_property);
                                        }
                                        else
                                        {
                                            if (valuetype3 == 3 && !string.IsNullOrEmpty(propertyAliasViewEntity.ImageUrl))
                                            {
                                                string text9 = imageOperator.DownLoadPicture(propertyAliasViewEntity.ImageUrl, null, 60000, 60000);
                                                text9 = imageOperator.TransPicture(text9, base.ToolCode, item.NumIid.ToString(), TransPictureType.MovePicture);
                                                if (flag2 && !string.IsNullOrEmpty(text9))
                                                {
                                                    text9 = string.Empty;
                                                }
                                                if (!string.IsNullOrEmpty(text9))
                                                {
                                                    sp_property.Name = text9;
                                                    sp_property.Value = text9;
                                                    sp_property.Issellpro = 1;
                                                    if (base.ToolCode == "UPLOADTOYF")
                                                    {
                                                        sp_property.PicUrl = propertyAliasViewEntity.ImageUrl;
                                                    }
                                                    if (flag)
                                                    {
                                                        sp_property.Url = propertyAliasViewEntity.ImageUrl;
                                                    }
                                                    list.Add(sp_property);
                                                }
                                            }
                                        }
                                    }
                                }
                                goto IL_1695;
                            }
                            goto IL_1590;
                        }
                        goto IL_1590;
                        IL_1695:
                        SellProInfo sellProInfo = new SellProInfo();
                        sellProInfo.Value = DataConvert.ToString(dataRow3["Propertyid"]) + ":" + DataConvert.ToString(dataRow3["Id"]);
                        sellProInfo.Name = name;
                        dicProValueAndSellProInfos[text5] = sellProInfo;
                        this.Haschildproperty(DataConvert.ToBoolean(dataRow3["Haschildproperty"]), sysSysPropertyList, text5, dictionary6, list2, ref list);
                        continue;
                        IL_1590:
                        if (sp_property.Issellpro == 1 && !propertyValueName4.Trim().Equals(sp_property.Name.Trim()))
                        {
                            name = propertyValueName4;
                            using (IEnumerator<Sys_sysProperty> enumerator = propertyBySortId.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    Sys_sysProperty current11 = enumerator.Current;
                                    if (current11.Parentprovalueid == DataConvert.ToInt(dataRow3["Id"]))
                                    {
                                        sp_property = new Sp_property();
                                        int valuetype4 = current11.Valuetype;
                                        sp_property = new Sp_property();
                                        text4 = DataConvert.ToString(current11.Id);
                                        sp_property.Propertyid = current11.Id;
                                        sp_property.Issellpro = (list2.Contains(text4) ? 1 : 0);
                                        if (valuetype4 == 2)
                                        {
                                            sp_property.Name = propertyValueName4;
                                            sp_property.Value = propertyValueName4;
                                            sp_property.Issellpro = 1;
                                            list.Add(sp_property);
                                        }
                                    }
                                }
                                goto IL_1695;
                            }
                        }
                        name = DataConvert.ToString(dataRow3["Name"]).Trim();
                        goto IL_1695;
                    }
                    Sys_sysProperty sys_sysProperty2 = (propertyBySortId as List<Sys_sysProperty>).Find((Sys_sysProperty x) => x.Keys == proKey);
                    if ((array3 == null || array3.Length <= 0) && sys_sysProperty2 != null)
                    {
                        SellNewProInfo sellNewProInfo3 = new SellNewProInfo();
                        sp_property.Propertyid = sys_sysProperty2.Id;
                        sp_property.Issellpro = (list2.Contains(DataConvert.ToString(sys_sysProperty2.Id)) ? 1 : 0);
                        string value4 = string.Empty;
                        if (dictionary9 == null || dictionary9.Count == 0)
                        {
                            value4 = proKey + ":-1001";
                            dictionary9.Add(proKey, -1001);
                            sp_property.Name = current9.PropertyValueName;
                            sp_property.Value = value4;
                        }
                        else
                        {
                            if (dictionary9.ContainsKey(proKey))
                            {
                                Dictionary<string, int> dictionary10;
                                string proKey2;
                                (dictionary10 = dictionary9)[proKey2 = proKey] = dictionary10[proKey2] + -1;
                                value4 = proKey + ":" + dictionary9[proKey];
                                sp_property.Name = current9.PropertyValueName;
                                sp_property.Value = value4;
                            }
                        }
                        sellNewProInfo3.Value = value4;
                        sellNewProInfo3.Name = current9.PropertyValueName;
                        sourceProValueAndNewProValue[current9.PropertyValue] = sellNewProInfo3;
                        list.Add(sp_property);
                        dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                    }
                }
            }
            return list;
        }
    }
}
