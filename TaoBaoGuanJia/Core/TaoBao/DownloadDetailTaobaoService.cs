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
        public string ToolCode="SNATCH_4PLUS";
        private bool IsSnatchPromoPrice => ConfigHelper.TaoBaoSnatchPromoPrice;// DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", base.ToolCode.ToUpper(), "SnatchPromoPrice", "false"));

        private bool IsSnatchSellPoint => ConfigHelper.TaoBaoSnatchSellPoint;// DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", base.ToolCode.ToUpper(), "SnatchSellPoint", "false"));
   
        public void Download(ItemGetResponse itemGetResponse, DownloadItemInfoViewEntity viewEntity, string onlineKey)
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
            itemDetailTaobaoViewEntity.SpPicturesList = this.TransAndSavePicture(dicTempPictureNameAndItemImg, ToolCode, onlineKey, isChkTBShield, itemGetResponse.Item.Cid.ToString());
            if (itemDetailTaobaoViewEntity.SpPicturesList != null && itemDetailTaobaoViewEntity.SpPicturesList.Count > 0)
            {
                itemDetailTaobaoViewEntity.SpItem.Photo = itemDetailTaobaoViewEntity.SpPicturesList[0].Localpath;
            }
            itemDetailTaobaoViewEntity.SpItemContent = (viewEntity.DownloadAllDetail ? this.GetSpItemContent(itemGetResponse.Item.Desc, itemGetResponse.Item.WirelessDesc, isChkTBShield, itemGetResponse.Item.Cid.ToString()) : null);
            itemDetailTaobaoViewEntity.SpShopSortList = this.GetSpShopSortList(sysShopSortList, id);
            itemDetailTaobaoViewEntity.SpSysSort = this.GetSpSysSort(sortBySysIdAndKeys, id, sysid, sortId);
      
            SaveItemDetail(itemDetailTaobaoViewEntity);
            
        }

        private void SaveItemDetail(ItemDetailTaobaoViewEntity viewEntity)
        {
            int num = 0;
            num = DataHelper.InsertSpItem(viewEntity.SpItem);
            if (viewEntity.SpItemContent != null) {
                viewEntity.SpItemContent.Itemid = num;
            }
            if (viewEntity.SpSysSort != null)
            {
                viewEntity.SpSysSort.Itemid = num;
            }
            if (viewEntity.SpFodSecurity != null)
            {
                viewEntity.SpFodSecurity.Itemid = num;
            }

            DataHelper.InsertSpPictures(num, viewEntity.SpPicturesList);
            DataHelper.InsertSpItemContent(viewEntity.SpItemContent);
            DataHelper.InsertSpSysSort(viewEntity.SpSysSort);
            DataHelper.InsertSpPropertyList(num, viewEntity.SpPropertyList);
            DataHelper.InsertSpSellPropertyList(num, viewEntity.SpSellPropertyList);
            DataHelper.InsertFoodSecurity(viewEntity.SpFodSecurity);
        }

        private Sp_sysSort GetSpSysSort(Sys_sysSort sysSysSort, int shopId, int sysId, int sortId)
        {
            if (sysSysSort == null)
            {
                return null;
            }
            return new Sp_sysSort
            {
                Shopid = shopId,
                Sysid = sysId,
                Syssortid = sortId,
                Syssortname = sysSysSort.Name,
                Syssortpath = sysSysSort.Path,
                Modifytime = DateTime.Now
            };
        }
        private IList<Sp_shopSort> GetSpShopSortList(IList<Sys_shopSort> sysShopSortList, int shopId)
        {
            IList<Sp_shopSort> list = new List<Sp_shopSort>();
            foreach (Sys_shopSort current in sysShopSortList)
            {
                list.Add(new Sp_shopSort
                {
                    Shopid = shopId,
                    Shopsortid = current.Id,
                    Shopsort = current.Sortname,
                    Modifytime = DateTime.Now
                });
            }
            return list;
        }

        private Sp_itemContent GetSpItemContent(string desc, string wirelessdesc, bool isChkTBShield, string cId)
        {
            return new Sp_itemContent
            {
                Content = HttpUtility.HtmlDecode(desc),
                Modifydate = DateTime.Now,
                Wirelessdesc = wirelessdesc
            };
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
        private IList<Sp_property> SetNewspPropertyList(Item item, IList<Sys_sysProperty> sysSysPropertyList, DataTable dtPropertyValue, int sortId, out Dictionary<string, SellProInfo> dicProValueAndSellProInfos, out string code, out Dictionary<string, string> dicSetOldToNew, out Dictionary<string, SellNewProInfo> sourceProValueAndNewProValue)
        {
            bool flag = ConfigHelper.TaoBaoCItemPicShieldCheck;// DataConvert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", base.ToolCode.ToUpper().ToString(), "CItemPicShieldCheck", "false"));
            sourceProValueAndNewProValue = new Dictionary<string, SellNewProInfo>();
            bool flag2 = ConfigHelper.TaoBaoSellProperyPic;// Convert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", "PicSet", "SellProperyPic", "false"));
            code = string.Empty;
            dicSetOldToNew = new Dictionary<string, string>();
            IList<Sp_property> list = new List<Sp_property>();
            IList<Sys_sysProperty> propertyBySortId = DataHelper.GetPropertyBySortId(sortId);
            Dictionary<string, Sys_sysProperty> dictionary = new Dictionary<string, Sys_sysProperty>();
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            List<string> list2 = new List<string>();
            IList<Sys_sysPropertyValue> list3 = new List<Sys_sysPropertyValue>();
            int num = 0;
            string text = string.Empty;
            Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
            Sys_sysSort sortById = DataHelper.GetSortById(sortId);
            string sortAllName = string.Empty;
            if (sortById != null && !string.IsNullOrEmpty(sortById.Name))
            {
                sortAllName = sortById.Path + ">>" + sortById.Name;
            }
            Dictionary<string, string> dicCustomProperty = this.GetDicCustomProperty();
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
                    if (current.Issellpro && current.Parentid == 0 && current.Name.Contains("尺"))
                    {
                        num = current.Id;
                        list3 = DataHelper.GetPropertyValuesByPropertyId(num);
                        if (list3 != null && list3.Count > 0 && sortById != null && sortById.IsNewSellPro && !string.IsNullOrEmpty(sortById.SizeGroupType))
                        {
                            DataTable sizeDetailTableByTypeCode = DataHelper.GetSizeDetailTableByTypeCode(sortById.SizeGroupType, 1);
                            if (sizeDetailTableByTypeCode != null)
                            {
                                DataRow[] array = sizeDetailTableByTypeCode.Select("SizeValue like '" + this.GetLikeStrForDtSelect(current.Keys) + ":%'");
                                if (array != null && array.Length > 0)
                                {
                                    DataRow[] array2 = array;
                                    for (int i = 0; i < array2.Length; i++)
                                    {
                                        DataRow dataRow = array2[i];
                                        string value = dataRow["GroupOnlineId"].ToString();
                                        string text2 = dataRow["SizeValue"].ToString();
                                        foreach (Sys_sysPropertyValue current2 in list3)
                                        {
                                            if (!dictionary3.ContainsKey(text2) && current2.Value == text2)
                                            {
                                                dictionary3[text2] = value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (current.Issellpro)
                    {
                        list2.Add(DataConvert.ToString(current.Id));
                    }
                }
            }
            string text3 = string.Empty;
            DataRow[] array3 = null;
            if (string.IsNullOrEmpty(sortById.SizeGroupType))
            {
                IList<Sys_sysProperty> sellPropertyBySortId = DataHelper.GetSellPropertyBySortId(sortById.Id);
                if (dtPropertyValue != null && dtPropertyValue.Rows.Count > 0 && sellPropertyBySortId != null && sellPropertyBySortId.Count > 0)
                {
                    if (sellPropertyBySortId != null && sellPropertyBySortId.Count > 0)
                    {
                        Sys_sysProperty sys_sysProperty = (sellPropertyBySortId as List<Sys_sysProperty>).Find((Sys_sysProperty a) => a.Name.Contains("尺") || a.Name.Contains("鞋码"));
                        if (sys_sysProperty != null && sys_sysProperty.Id > 0)
                        {
                            text3 = sys_sysProperty.Keys;
                        }
                    }
                    if (!string.IsNullOrEmpty(text3))
                    {
                        array3 = dtPropertyValue.Select("value like '" + this.GetLikeStrForDtSelect(text3) + ":%'");
                    }
                }
            }
            Dictionary<string, DataRow> dictionary4 = new Dictionary<string, DataRow>();
            bool flag3 = false;
            foreach (DataRow dataRow2 in dtPropertyValue.Rows)
            {
                dictionary4[DataConvert.ToString(dataRow2["Value"])] = dataRow2;
                if (!flag3 && DataConvert.ToString(dataRow2["Name"]) == "透明")
                {
                    flag3 = true;
                }
            }
            dicProValueAndSellProInfos = new Dictionary<string, SellProInfo>();
            if (item == null)
            {
                return list;
            }
            Dictionary<string, PropertyAliasViewEntity> dictionary5 = new Dictionary<string, PropertyAliasViewEntity>();
            PropertyAliasViewEntity propertyAliasViewEntity = new PropertyAliasViewEntity();
            string text4 = string.Empty;
            string str = string.Empty;
            if (item.PropImgs != null && item.PropImgs.Count > 0)
            {
                foreach (PropImg current3 in item.PropImgs)
                {
                    propertyAliasViewEntity = new PropertyAliasViewEntity();
                    text4 = current3.Properties;
                    propertyAliasViewEntity.Value = text4;
                    propertyAliasViewEntity.ImageUrl = current3.Url;
                    dictionary5[text4] = propertyAliasViewEntity;
                }
            }
            if (!string.IsNullOrEmpty(item.PropertyAlias))
            {
                string[] array4 = item.PropertyAlias.Split(new string[]
                {
            ";"
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array4.Length; i++)
                {
                    string text5 = array4[i];
                    int num2 = text5.LastIndexOf(":");
                    if (num2 > 0)
                    {
                        text4 = text5.Substring(0, num2);
                        str = text5.Substring(num2 + 1);
                        str = this.SubStringToMaxByteLength(str, 16);
                        if (dictionary5.ContainsKey(text4))
                        {
                            propertyAliasViewEntity = dictionary5[text4];
                        }
                        else
                        {
                            propertyAliasViewEntity = new PropertyAliasViewEntity();
                        }
                        propertyAliasViewEntity.Value = text5.Substring(0, num2);
                        propertyAliasViewEntity.Alias = HttpUtility.UrlDecode(text5.Substring(num2 + 1));
                        propertyAliasViewEntity.Alias = this.SubStringToMaxByteLength(propertyAliasViewEntity.Alias, 16);
                        dictionary5[propertyAliasViewEntity.Value] = propertyAliasViewEntity;
                    }
                }
            }
            string arg_5F8_0 = string.Empty;
            string text6 = string.Empty;
            string text7 = string.Empty;
            string text8 = string.Empty;
            string name = string.Empty;
            Sp_property sp_property = null;
            Dictionary<string, string> inputPidAndStr = this.GetInputPidAndStr(item);
            ImageOperator imageOperator = new ImageOperator();
            List<PropsViewEntity> lstPropsDTO = this.GetLstPropsDTO(item.PropsName);
            Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
            Dictionary<string, string> dictionary7 = new Dictionary<string, string>();
            for (int j = 0; j < lstPropsDTO.Count; j++)
            {
                if (dictionary2.ContainsKey(lstPropsDTO[j].ProKey))
                {
                    string item2 = dictionary2[lstPropsDTO[j].ProKey];
                    if (list2.Contains(item2))
                    {
                        byte[] bytes = Encoding.Default.GetBytes(lstPropsDTO[j].PropertyValueName);
                        int num3 = bytes.Length;
                        if (num3 > 30)
                        {
                            string text9 = Encoding.Default.GetString(bytes, 0, 30);
                            if (text9.EndsWith("?"))
                            {
                                text9 = text9.TrimEnd(new char[]
                                {
                            '?'
                                });
                            }
                            text9 = text9.Trim();
                            if (inputPidAndStr.ContainsKey(text9))
                            {
                                text9 += j;
                            }
                            else
                            {
                                inputPidAndStr[text9] = text9;
                            }
                            lstPropsDTO[j].PropertyValueName = text9;
                        }
                    }
                }
            }
            int num4 = 0;
            foreach (PropsViewEntity current4 in lstPropsDTO)
            {
                dictionary6[current4.PropertyName] = current4.PropertyValueName;
                dictionary7[current4.PropertyValue] = current4.PropertyValueName;
                if (current4.PropertyName.IndexOf("颜色") > -1)
                {
                    num4++;
                }
            }
            foreach (KeyValuePair<string, PropertyAliasViewEntity> current5 in dictionary5)
            {
                if (current5.Value != null && string.IsNullOrEmpty(current5.Value.Alias) && dictionary7.ContainsKey(current5.Key))
                {
                    current5.Value.Alias = dictionary7[current5.Key];
                    if (current5.Value.Alias == "肤色" && flag3)
                    {
                        current5.Value.Alias = "透明";
                    }
                }
            }
            int num5 = 0;
            Dictionary<string, int> dictionary8 = new Dictionary<string, int>();
            string text10 = string.Empty;
            foreach (string current6 in list2)
            {
                text10 = text10 + current6 + ",";
            }
            if (!string.IsNullOrEmpty(text10))
            {
                text10 = text10.TrimEnd(new char[]
                {
            ','
                });
            }
            new Dictionary<string, string>();
            foreach (PropsViewEntity current7 in lstPropsDTO)
            {
                bool flag4 = false;
                bool flag5 = false;
                DataRow drPropertyValue = null;
                sp_property = new Sp_property();
                string text11 = current7.PropertyValueName;
                int num2 = current7.PropertyValue.IndexOf(":");
                if (num2 != 0)
                {
                    text7 = current7.PropertyValue.Remove(num2);
                    if (dictionary.ContainsKey(text7))
                    {
                        text6 = DataConvert.ToString(dictionary[text7].Id);
                        Sys_sysProperty sys_sysProperty2 = dictionary[text7];
                        if (sys_sysProperty2 != null && sys_sysProperty2.Name != null && (sys_sysProperty2.Name.EndsWith("货号") || sys_sysProperty2.Name.EndsWith("款号")))
                        {
                            code = current7.PropertyValueName;
                        }
                        sp_property.Propertyid = DataConvert.ToInt(text6);
                        sp_property.Issellpro = (list2.Contains(text6) ? 1 : 0);
                        if (dictionary[text7].Valuetype == 2 && dictionary[text7].Issellpro)
                        {
                            SellNewProInfo sellNewProInfo = new SellNewProInfo();
                            string value2 = string.Empty;
                            string value3 = string.Empty;
                            if (dictionary8 == null || dictionary8.Count == 0)
                            {
                                value3 = text6 + ":-1001";
                                value2 = text7 + ":-1001";
                                dictionary8.Add(text7, -1001);
                                sp_property.Name = current7.PropertyValueName;
                                sp_property.Value = value2;
                            }
                            else
                            {
                                if (dictionary8.ContainsKey(text7))
                                {
                                    Dictionary<string, int> dictionary9;
                                    string key;
                                    (dictionary9 = dictionary8)[key = text7] = dictionary9[key] + -1;
                                    value3 = text6 + ":" + dictionary8[text7];
                                    value2 = text7 + ":" + dictionary8[text7];
                                    sp_property.Name = current7.PropertyValueName;
                                    sp_property.Value = value2;
                                }
                                else
                                {
                                    value3 = text6 + ":-1001";
                                    value2 = text7 + ":-1001";
                                    dictionary8.Add(text7, -1001);
                                    sp_property.Name = current7.PropertyValueName;
                                    sp_property.Value = value2;
                                }
                            }
                            sellNewProInfo.Value = value3;
                            sellNewProInfo.Name = current7.PropertyValueName;
                            sourceProValueAndNewProValue[current7.PropertyValue] = sellNewProInfo;
                        }
                        else
                        {
                            sp_property.Value = current7.PropertyValueName;
                        }
                        list.Add(sp_property);
                    }
                    else
                    {
                        text8 = current7.PropertyValue;
                        string text12 = text8;
                        string value4 = string.Empty;
                        string text13 = string.Empty;
                        string value5 = string.Empty;
                        string text14 = string.Empty;
                        int num6 = 0;
                        string text15 = string.Empty;
                        if (dictionary2.ContainsKey(current7.ProKey))
                        {
                            text15 = dictionary2[current7.ProKey];
                        }
                        if ((sortId == 154246 || sortId == 154247) && list2.Contains(text12) && current7.PropertyName.IndexOf("颜色") > -1)
                        {
                            string propertyValueName = current7.PropertyValueName;
                            string empty8 = string.Empty;
                            switch (propertyValueName)
                            {
                                case "白":
                                    empty8 = "白(新)";
                                    break;
                                case "蓝":
                                    empty8 = "蓝(新)";
                                    break;
                                case "黑":
                                    empty8 = "黑(新)";
                                    break;
                                case "红":
                                    empty8 = "红(新)";
                                    break;
                                case "粉":
                                    empty8 = "粉(新)";
                                    break;
                                case "灰":
                                    empty8 = "灰(新)";
                                    break;
                                case "米黄色":
                                    empty8 = "米黄色(新)";
                                    break;
                                case "绿":
                                    empty8 = "绿(新)";
                                    break;
                                default:
                                    empty8 = propertyValueName;
                                    break;
                            }
                            if (empty8 != propertyValueName)
                            {
                                current7.PropertyValueName = empty8;
                                text8 = empty8;
                            }
                        }
                        DataRow[] array5 = dtPropertyValue.Select("value='" + DbUtil.OerateSpecialChar(text8) + "'");
                        if (array5 != null && array5.Length > 0)
                        {
                            num6 = DataConvert.ToInt(array5[0]["propertyId"]);
                        }
                        if (!string.IsNullOrEmpty(text10) && !string.IsNullOrEmpty(text15) && text10.Contains(text15))
                        {
                            string[] array6 = text10.Split(new char[]
                            {
                        ','
                            });
                            DataRow[] array7 = null;
                            string[] array4 = array6;
                            for (int i = 0; i < array4.Length; i++)
                            {
                                string text17 = array4[i];
                                array7 = dtPropertyValue.Select(string.Concat(new string[]
                                {
                            "propertyId = ",
                            text17,
                            " and name='",
                            DbUtil.OerateSpecialChar(current7.PropertyValueName),
                            "'"
                                }));
                                if (array7.Length > 0)
                                {
                                    break;
                                }
                            }
                            if (array5 != null && array5.Length > 0 && array7 != null && array7.Length > 0)
                            {
                                if (DataConvert.ToString(array5[0]["name"]) != DataConvert.ToString(array7[0]["name"]) && DataConvert.ToString(array5[0]["value"]) != DataConvert.ToString(array7[0]["value"]) && array7[0]["value"].ToString().Contains(text7 + ":"))
                                {
                                    current7.PropertyValue = DataConvert.ToString(array7[0]["value"]);
                                    current7.PropertyValueAndName = current7.PropertyValue + ":" + DataConvert.ToString(array7[0]["name"]);
                                    text8 = current7.PropertyValue;
                                    dicSetOldToNew[text12] = text8;
                                    if (dictionary5.ContainsKey(text12))
                                    {
                                        dictionary5[text8] = dictionary5[text12];
                                        dictionary5.Remove(text12);
                                    }
                                }
                            }
                            else
                            {
                                if ((array5 == null || array5.Length == 0) && array7 != null && array7.Length > 0 && array7[0]["value"].ToString().Contains(text7 + ":"))
                                {
                                    current7.PropertyValue = DataConvert.ToString(array7[0]["value"]);
                                    current7.PropertyValueAndName = current7.PropertyValue + ":" + DataConvert.ToString(array7[0]["name"]);
                                    text8 = current7.PropertyValue;
                                    dicSetOldToNew[text12] = text8;
                                    if (dictionary5.ContainsKey(text12))
                                    {
                                        dictionary5[text8] = dictionary5[text12];
                                        dictionary5.Remove(text12);
                                    }
                                }
                            }
                        }
                        if (text12 != text8)
                        {
                            array5 = dtPropertyValue.Select("value='" + DbUtil.OerateSpecialChar(text8) + "'");
                            if (array5 != null && array5.Length > 0)
                            {
                                num6 = DataConvert.ToInt(array5[0]["propertyId"]);
                            }
                        }
                        bool flag6 = true;
                        if (array5 != null && array5.Length > 0 && this.IsOldSizeAndNewTaobao(dicCustomProperty, sortAllName, current7.PropertyName) && array5[0]["name"].ToString() != current7.PropertyValueName)
                        {
                            flag6 = ((current7.PropertyName == "参考身高" || current7.PropertyName == "尺码") && (array5[0]["name"] + "cm").ToString() == current7.PropertyValueName);
                        }
                        if (flag6 && dictionary4.ContainsKey(text8) && (dictionary3.ContainsKey(text8) || num6 != num))
                        {
                            drPropertyValue = dictionary4[text8];
                            if (dictionary5.ContainsKey(current7.PropertyValue))
                            {
                                PropertyAliasViewEntity propertyAliasViewEntity2 = dictionary5[current7.PropertyValue];
                                if (propertyAliasViewEntity2 != null && !string.IsNullOrEmpty(propertyAliasViewEntity2.Alias) && propertyAliasViewEntity2.Alias != DataConvert.ToString(drPropertyValue["Name"]) && sortById != null && sortById.IsNewSellPro)
                                {
                                    if (num == DataConvert.ToInt(drPropertyValue["Propertyid"]) && string.IsNullOrEmpty(sortById.SizeGroupType))
                                    {
                                        text13 = propertyAliasViewEntity2.Alias;
                                        flag5 = true;
                                    }
                                    else
                                    {
                                        string likeStrForDtSelect = this.GetLikeStrForDtSelect(text7);
                                        if (num4 > 24)
                                        {
                                            string text18 = propertyAliasViewEntity2.Alias;
                                            if (text18.Contains("色"))
                                            {
                                                text18 = text18.Substring(0, text18.IndexOf("色") + 1);
                                            }
                                            else
                                            {
                                                if (text18.Contains("西瓜红"))
                                                {
                                                    text18 = "西瓜红";
                                                }
                                                else
                                                {
                                                    if (text18.Contains("柠檬黄"))
                                                    {
                                                        text18 = "柠檬黄";
                                                    }
                                                    else
                                                    {
                                                        if (text18.Contains("荧光黄"))
                                                        {
                                                            text18 = "荧光黄";
                                                        }
                                                        else
                                                        {
                                                            if (text18.Contains("荧光绿"))
                                                            {
                                                                text18 = "荧光绿";
                                                            }
                                                            else
                                                            {
                                                                if (text18.Contains("孔雀蓝"))
                                                                {
                                                                    text18 = "孔雀蓝";
                                                                }
                                                                else
                                                                {
                                                                    if (text18.Contains("紫罗兰"))
                                                                    {
                                                                        text18 = "紫罗兰";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            DataRow[] array8 = dtPropertyValue.Select(string.Concat(new string[]
                                            {
                                        "name='",
                                        DbUtil.OerateSpecialChar(text18),
                                        "' and value like '%",
                                        likeStrForDtSelect,
                                        "%'"
                                            }));
                                            if (array8 != null && array8.Length > 0)
                                            {
                                                drPropertyValue = array8[0];
                                            }
                                            else
                                            {
                                                bool flag7 = false;
                                                if (!string.IsNullOrEmpty(text18))
                                                {
                                                    string[] array9 = text18.Split(new char[]
                                                    {
                                                ' '
                                                    });
                                                    if (array9 != null && array9.Length >= 2)
                                                    {
                                                        DataRow[] array10 = dtPropertyValue.Select(string.Concat(new string[]
                                                        {
                                                    "name='",
                                                    DbUtil.OerateSpecialChar(array9[0]),
                                                    "' and value like '%",
                                                    likeStrForDtSelect,
                                                    "%'"
                                                        }));
                                                        if (array10 != null && array10.Length > 0)
                                                        {
                                                            drPropertyValue = array10[0];
                                                        }
                                                        else
                                                        {
                                                            flag7 = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        flag7 = true;
                                                    }
                                                }
                                                if (flag7)
                                                {
                                                    flag4 = true;
                                                    if (dictionary8.ContainsKey(text7))
                                                    {
                                                        Dictionary<string, int> dictionary9;
                                                        string key;
                                                        (dictionary9 = dictionary8)[key = text7] = dictionary9[key] + -1;
                                                    }
                                                    else
                                                    {
                                                        dictionary8.Add(text7, -1001);
                                                    }
                                                    value4 = text7 + ":" + dictionary8[text7];
                                                    if (drPropertyValue != null)
                                                    {
                                                        value5 = drPropertyValue["Propertyid"] + ":" + dictionary8[text7];
                                                    }
                                                    text14 = propertyAliasViewEntity2.Alias;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DataRow[] array11 = dtPropertyValue.Select(string.Concat(new string[]
                                            {
                                        "name='",
                                        DbUtil.OerateSpecialChar(propertyAliasViewEntity2.Alias),
                                        "' and value like '%",
                                        likeStrForDtSelect,
                                        "%'"
                                            }));
                                            if (array11 != null && array11.Length > 0)
                                            {
                                                drPropertyValue = array11[0];
                                            }
                                            else
                                            {
                                                bool flag8 = false;
                                                if (!string.IsNullOrEmpty(propertyAliasViewEntity2.Alias))
                                                {
                                                    string[] array12 = propertyAliasViewEntity2.Alias.Split(new char[]
                                                    {
                                                ' '
                                                    });
                                                    if (array12 != null && array12.Length >= 2)
                                                    {
                                                        DataRow[] array13 = dtPropertyValue.Select(string.Concat(new string[]
                                                        {
                                                    "name='",
                                                    DbUtil.OerateSpecialChar(array12[0]),
                                                    "' and value='",
                                                    current7.PropertyValue,
                                                    "'"
                                                        }));
                                                        if (array13 != null && array13.Length > 0)
                                                        {
                                                            drPropertyValue = array13[0];
                                                        }
                                                        else
                                                        {
                                                            flag8 = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        flag8 = true;
                                                    }
                                                }
                                                if (flag8)
                                                {
                                                    if (dictionary8.ContainsKey(text7))
                                                    {
                                                        Dictionary<string, int> dictionary9;
                                                        string key;
                                                        (dictionary9 = dictionary8)[key = text7] = dictionary9[key] + -1;
                                                    }
                                                    else
                                                    {
                                                        dictionary8.Add(text7, -1001);
                                                    }
                                                    flag4 = true;
                                                    value4 = text7 + ":" + dictionary8[text7];
                                                    if (drPropertyValue != null)
                                                    {
                                                        value5 = drPropertyValue["Propertyid"] + ":" + dictionary8[text7];
                                                    }
                                                    text14 = propertyAliasViewEntity2.Alias;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(text3) && text8.StartsWith(text3) && array3 != null && array3.Length > 0 && num5 < array3.Length)
                            {
                                bool flag9 = false;
                                bool flag10 = false;
                                int k = 0;
                                while (k < array3.Length)
                                {
                                    if (DataConvert.ToString(array3[k]["Value"]) == text8)
                                    {
                                        if (!(current7.PropertyValueName != DataConvert.ToString(array3[k]["name"])) || !this.IsOldSizeAndNewTaobao(dicCustomProperty, sortAllName, current7.PropertyName))
                                        {
                                            drPropertyValue = array3[k];
                                            flag9 = true;
                                            break;
                                        }
                                        if ((!(current7.PropertyName == "参考身高") && !(current7.PropertyName == "尺码")) || !current7.PropertyValueName.Equals(DataConvert.ToString(array3[k]["name"]) + "cm"))
                                        {
                                            flag10 = true;
                                            break;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        if (this.IsOldSizeAndNewTaobao(dicCustomProperty, sortAllName, current7.PropertyName))
                                        {
                                            flag10 = true;
                                        }
                                        k++;
                                    }
                                }
                                if (!flag9)
                                {
                                    if (flag10)
                                    {
                                        drPropertyValue = null;
                                        text13 = current7.PropertyValueName;
                                    }
                                    else
                                    {
                                        drPropertyValue = array3[num5];
                                        if (drPropertyValue != null && current7.PropertyName == "尺码" && list != null && list.Count > 0)
                                        {
                                            Sp_property sp_property2 = (list as List<Sp_property>).Find((Sp_property a) => a.Name == drPropertyValue["Name"].ToString() && a.Value == drPropertyValue["Value"].ToString() && a.Issellpro == 1);
                                            if (sp_property2 == null || sp_property2.Propertyid <= 0)
                                            {
                                                num5++;
                                            }
                                            else
                                            {
                                                bool flag11 = false;
                                                do
                                                {
                                                    drPropertyValue = array3[num5];
                                                    sp_property2 = (list as List<Sp_property>).Find((Sp_property a) => a.Name == drPropertyValue["Name"].ToString() && a.Value == drPropertyValue["Value"].ToString() && a.Issellpro == 1);
                                                    if (sp_property2 == null || sp_property2.Propertyid <= 0)
                                                    {
                                                        num5++;
                                                        flag11 = true;
                                                    }
                                                    else
                                                    {
                                                        num5++;
                                                        if (num5 == array3.Length)
                                                        {
                                                            flag11 = true;
                                                        }
                                                    }
                                                }
                                                while (!flag11);
                                            }
                                        }
                                        else
                                        {
                                            num5++;
                                        }
                                        text13 = current7.PropertyValueName;
                                        flag5 = true;
                                    }
                                }
                            }
                            if (drPropertyValue == null)
                            {
                                if (dictionary2.ContainsKey(text7))
                                {
                                    text6 = dictionary2[text7];
                                }
                                if (string.IsNullOrEmpty(text6) || DataConvert.ToInt(text6) == 0)
                                {
                                    continue;
                                }
                                string sysConfig = DataHelper.GetSysConfig("AppConfig", "Taobao", "TransToStandardSize", "");
                                if (!string.IsNullOrEmpty(sysConfig) && sysConfig.Contains(sortId + ",") && sysConfig.Contains(current7.PropertyValueName + ":"))
                                {
                                    Regex regex = new Regex(string.Concat(new object[]
                                    {
                                sortId,
                                ",[^;]*?",
                                current7.PropertyValueName,
                                ":(?<newSize>.*?)(\\||;)"
                                    }));
                                    Match match = regex.Match(sysConfig);
                                    if (match != null && match.Groups["newSize"] != null && !string.IsNullOrEmpty(match.Groups["newSize"].Value))
                                    {
                                        string value6 = match.Groups["newSize"].Value;
                                        DataRow[] array14 = dtPropertyValue.Select("name='" + value6 + "'");
                                        if (array14 != null && array14.Length > 0)
                                        {
                                            text8 = DataConvert.ToString(array14[0]["value"]);
                                            text11 = value6;
                                            current7.PropertyValueName = value6;
                                            dicSetOldToNew[current7.PropertyValue] = text8;
                                            current7.PropertyValue = text8;
                                            current7.PropertyValueAndName = string.Concat(new string[]
                                            {
                                        current7.PropertyValue,
                                        ":",
                                        current7.PropertyName,
                                        ":",
                                        current7.PropertyValueName
                                            });
                                        }
                                    }
                                }
                                if (dictionary3.ContainsKey(current7.PropertyValue))
                                {
                                    if (DataConvert.ToInt(text6) == num)
                                    {
                                        if (dictionary5.ContainsKey(current7.PropertyValue))
                                        {
                                            PropertyAliasViewEntity propertyAliasViewEntity3 = dictionary5[current7.PropertyValue];
                                            if (propertyAliasViewEntity3.Alias.IndexOf("'") > 0)
                                            {
                                                propertyAliasViewEntity3.Alias = propertyAliasViewEntity3.Alias.Replace("'", "''");
                                            }
                                            text14 = propertyAliasViewEntity3.Alias;
                                            string likeStrForDtSelect2 = this.GetLikeStrForDtSelect(propertyAliasViewEntity3.Alias);
                                            string likeStrForDtSelect3 = this.GetLikeStrForDtSelect(text7);
                                            DataRow[] array15 = dtPropertyValue.Select(string.Concat(new string[]
                                            {
                                        "name like '%",
                                        likeStrForDtSelect2,
                                        "' and value like '%",
                                        likeStrForDtSelect3,
                                        "%'"
                                            }));
                                            if (array15 != null && array15.Length > 0)
                                            {
                                                drPropertyValue = array15[0];
                                                if (string.IsNullOrEmpty(text))
                                                {
                                                    if (dictionary3.ContainsKey(drPropertyValue["value"].ToString()))
                                                    {
                                                        text = dictionary3[drPropertyValue["value"].ToString()];
                                                    }
                                                }
                                                else
                                                {
                                                    if (dictionary3.ContainsKey(drPropertyValue["value"].ToString()) && !text.Equals(dictionary3[drPropertyValue["value"].ToString()]))
                                                    {
                                                        drPropertyValue = null;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (current7.PropertyValueName.IndexOf("'") > 0)
                                            {
                                                current7.PropertyValueName = current7.PropertyValueName.Replace("'", "''");
                                            }
                                            text14 = current7.PropertyValueName;
                                            string likeStrForDtSelect4 = this.GetLikeStrForDtSelect(current7.PropertyValueName);
                                            string likeStrForDtSelect5 = this.GetLikeStrForDtSelect(text7);
                                            DataRow[] array16 = dtPropertyValue.Select(string.Concat(new string[]
                                            {
                                        "name like '%",
                                        likeStrForDtSelect4,
                                        "' and value like '%",
                                        likeStrForDtSelect5,
                                        "%'"
                                            }));
                                            if (array16 != null && array16.Length > 0)
                                            {
                                                drPropertyValue = array16[0];
                                                if (string.IsNullOrEmpty(text))
                                                {
                                                    if (dictionary3.ContainsKey(drPropertyValue["value"].ToString()))
                                                    {
                                                        text = dictionary3[drPropertyValue["value"].ToString()];
                                                    }
                                                }
                                                else
                                                {
                                                    if (dictionary3.ContainsKey(drPropertyValue["value"].ToString()) && !text.Equals(dictionary3[drPropertyValue["value"].ToString()]))
                                                    {
                                                        drPropertyValue = null;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (dictionary5.ContainsKey(current7.PropertyValue))
                                        {
                                            PropertyAliasViewEntity propertyAliasViewEntity4 = dictionary5[current7.PropertyValue];
                                            if (propertyAliasViewEntity4.Alias.IndexOf("'") > 0)
                                            {
                                                propertyAliasViewEntity4.Alias = propertyAliasViewEntity4.Alias.Replace("'", "''");
                                            }
                                            text14 = propertyAliasViewEntity4.Alias;
                                            string likeStrForDtSelect6 = this.GetLikeStrForDtSelect(text7);
                                            DataRow[] array17 = dtPropertyValue.Select(string.Concat(new string[]
                                            {
                                        "name='",
                                        DbUtil.OerateSpecialChar(propertyAliasViewEntity4.Alias),
                                        "' and value like '%",
                                        likeStrForDtSelect6,
                                        "%'"
                                            }));
                                            if (array17 != null && array17.Length > 0)
                                            {
                                                drPropertyValue = array17[0];
                                            }
                                        }
                                        else
                                        {
                                            if (current7.PropertyValueName.IndexOf("'") > 0)
                                            {
                                                current7.PropertyValueName = current7.PropertyValueName.Replace("'", "''");
                                            }
                                            text14 = current7.PropertyValueName;
                                            string likeStrForDtSelect7 = this.GetLikeStrForDtSelect(text7);
                                            DataRow[] array18 = dtPropertyValue.Select(string.Concat(new string[]
                                            {
                                        "name='",
                                        DbUtil.OerateSpecialChar(current7.PropertyValueName),
                                        "' and value like '%",
                                        likeStrForDtSelect7,
                                        "%'"
                                            }));
                                            if (array18 != null && array18.Length > 0)
                                            {
                                                drPropertyValue = array18[0];
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (drPropertyValue == null)
                                    {
                                        if (list2.Contains(text6))
                                        {
                                            if (DataConvert.ToInt(text6) == num)
                                            {
                                                text14 = current7.PropertyValueName;
                                                Regex regex2 = new Regex("^\\d+(\\.\\d+)?$");
                                                if (regex2.Match(text14).Success)
                                                {
                                                    text14 += "码";
                                                }
                                            }
                                            if (dictionary8.ContainsKey(text7))
                                            {
                                                Dictionary<string, int> dictionary9;
                                                string key;
                                                (dictionary9 = dictionary8)[key = text7] = dictionary9[key] + -1;
                                            }
                                            else
                                            {
                                                dictionary8.Add(text7, -1001);
                                            }
                                            if (this.IsOldSizeAndNewTaobao(dicCustomProperty, sortAllName, current7.PropertyName) && item != null && item.Skus != null && item.Skus.Count > 0)
                                            {
                                                dicSetOldToNew[current7.PropertyValue] = text7 + ":" + dictionary8[text7];
                                                current7.PropertyValue = text7 + ":" + dictionary8[text7];
                                                current7.PropertyValueAndName = current7.PropertyValue + ":" + current7.PropertyValueName;
                                            }
                                            flag4 = true;
                                            value4 = text7 + ":" + dictionary8[text7];
                                            value5 = text6 + ":" + dictionary8[text7];
                                            drPropertyValue = dtPropertyValue.NewRow();
                                            drPropertyValue["Propertyid"] = text6;
                                            drPropertyValue["Value"] = value4;
                                            drPropertyValue["Name"] = text14;
                                            drPropertyValue["Haschildproperty"] = false;
                                        }
                                        else
                                        {
                                            text8 = text7 + ":-1";
                                            if (dictionary4.ContainsKey(text8))
                                            {
                                                drPropertyValue = dictionary4[text8];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (drPropertyValue != null)
                        {
                            text6 = DataConvert.ToString(drPropertyValue["Propertyid"]);
                            sp_property.Propertyid = DataConvert.ToInt(text6);
                            if (!string.IsNullOrEmpty(value4))
                            {
                                sp_property.Value = value4;
                                if (!string.IsNullOrEmpty(text14))
                                {
                                    sp_property.Name = text14;
                                }
                                else
                                {
                                    sp_property.Name = current7.PropertyValueName;
                                }
                            }
                            else
                            {
                                sp_property.Value = DataConvert.ToString(drPropertyValue["Value"]);
                                sp_property.Name = DataConvert.ToString(drPropertyValue["Name"]);
                            }
                            sp_property.Issellpro = (list2.Contains(text6) ? 1 : 0);
                            if (dictionary5.ContainsKey(current7.PropertyValue))
                            {
                                PropertyAliasViewEntity propertyAliasViewEntity5 = dictionary5[current7.PropertyValue];
                                if (propertyAliasViewEntity5 != null && !string.IsNullOrEmpty(propertyAliasViewEntity5.ImageUrl))
                                {
                                    string text19 = imageOperator.DownLoadPicture(propertyAliasViewEntity5.ImageUrl, null, 60000, 60000);
                                    if (flag2 && !string.IsNullOrEmpty(text19))
                                    {
                                        text19 = string.Empty;
                                    }
                                    else
                                    {
                                        text19 = imageOperator.TransPicture(text19, ToolCode, item.NumIid.ToString(), TransPictureType.MovePicture);
                                    }
                                    if (!string.IsNullOrEmpty(text19))
                                    {
                                        sp_property.PicUrl = text19;
                                        if (ToolCode == "UPLOADTOYF")
                                        {
                                            sp_property.PicUrl = propertyAliasViewEntity5.ImageUrl;
                                        }
                                        if (flag)
                                        {
                                            sp_property.Url = propertyAliasViewEntity5.ImageUrl;
                                        }
                                    }
                                }
                            }
                            name = DataConvert.ToString(drPropertyValue["Name"]).Trim();
                            string str2 = DataConvert.ToString(drPropertyValue["Id"]);
                            if (num4 > 24 && flag4)
                            {
                                if (sp_property.Issellpro == 1 && sp_property.Name != current7.PropertyValueName && text6 != DataConvert.ToString(num))
                                {
                                    sp_property.Aliasname = current7.PropertyValueName;
                                    sp_property.Aliasname = sp_property.Aliasname.Replace(sp_property.Name, "").Trim();
                                    if (dictionary8.ContainsKey(text7))
                                    {
                                        str2 = DataConvert.ToString(dictionary8[text7]);
                                    }
                                }
                            }
                            else
                            {
                                if (sp_property.Issellpro == 1 && sp_property.Name != current7.PropertyValueName && text6 != DataConvert.ToString(num) && (current7.PropertyName.Contains("尺") || current7.PropertyName.Contains("颜")))
                                {
                                    if (!string.IsNullOrEmpty(current7.PropertyValueName) && current7.PropertyValueName.IndexOf(" ") > 0)
                                    {
                                        string[] array19 = current7.PropertyValueName.Split(new char[]
                                        {
                                    ' '
                                        });
                                        if (array19 != null && array19.Length >= 2)
                                        {
                                            DataRow[] array20 = dtPropertyValue.Select(string.Concat(new string[]
                                            {
                                        "name='",
                                        DbUtil.OerateSpecialChar(array19[0]),
                                        "' and value='",
                                        current7.PropertyValue,
                                        "'"
                                            }));
                                            if (array20 != null && array20.Length > 0)
                                            {
                                                sp_property.Aliasname = current7.PropertyValueName;
                                                sp_property.Aliasname = sp_property.Aliasname.Substring(sp_property.Aliasname.LastIndexOf(' ') + 1);
                                                byte[] bytes2 = Encoding.Default.GetBytes(sp_property.Aliasname);
                                                int num8 = bytes2.Length;
                                                if (num8 > 16)
                                                {
                                                    sp_property.Aliasname = sp_property.Aliasname.Replace("（", "(").Replace("）", ")");
                                                    bytes2 = Encoding.Default.GetBytes(sp_property.Aliasname);
                                                    num8 = bytes2.Length;
                                                    if (num8 > 16)
                                                    {
                                                        sp_property.Aliasname = this.SubStringToMaxByteLength(sp_property.Aliasname, 16);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                this.SellProConvertCustom(current7.PropertyValueName, text7, sp_property, dictionary8);
                                                name = current7.PropertyValueName;
                                                if (dictionary8.ContainsKey(text7))
                                                {
                                                    str2 = DataConvert.ToString(dictionary8[text7]);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            this.SellProConvertCustom(current7.PropertyValueName, text7, sp_property, dictionary8);
                                            name = current7.PropertyValueName;
                                            if (dictionary8.ContainsKey(text7))
                                            {
                                                str2 = DataConvert.ToString(dictionary8[text7]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.SellProConvertCustom(current7.PropertyValueName, text7, sp_property, dictionary8);
                                        name = current7.PropertyValueName;
                                        if (dictionary8.ContainsKey(text7))
                                        {
                                            str2 = DataConvert.ToString(dictionary8[text7]);
                                        }
                                    }
                                }
                                if (sp_property.Issellpro == 1 && sp_property.Name != current7.PropertyValueName && !string.IsNullOrEmpty(current7.PropertyValueName) && current7.PropertyName.Contains("尺码") && text6 == DataConvert.ToString(num))
                                {
                                    if (current7.PropertyValueName.LastIndexOf(' ') > -1 && !string.IsNullOrEmpty(sortById.SizeGroupType))
                                    {
                                        sp_property.Aliasname = current7.PropertyValueName;
                                        sp_property.Aliasname = sp_property.Aliasname.Substring(sp_property.Aliasname.LastIndexOf(' ') + 1);
                                        byte[] bytes3 = Encoding.Default.GetBytes(sp_property.Aliasname);
                                        int num9 = bytes3.Length;
                                        if (num9 > 16)
                                        {
                                            sp_property.Aliasname = sp_property.Aliasname.Replace("（", "(").Replace("）", ")");
                                            bytes3 = Encoding.Default.GetBytes(sp_property.Aliasname);
                                            num9 = bytes3.Length;
                                            if (num9 > 16)
                                            {
                                                sp_property.Aliasname = this.SubStringToMaxByteLength(sp_property.Aliasname, 16);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(sortById.SizeGroupType) && !sp_property.Name.Contains(current7.PropertyValueName))
                                        {
                                            this.SellProConvertCustom(current7.PropertyValueName, text7, sp_property, dictionary8);
                                            name = current7.PropertyValueName;
                                            if (dictionary8.ContainsKey(text7))
                                            {
                                                str2 = DataConvert.ToString(dictionary8[text7]);
                                            }
                                        }
                                    }
                                }
                            }
                            bool flag12 = false;
                            foreach (Sp_property current8 in list)
                            {
                                if (current8.Itemid == sp_property.Itemid && current8.Sysid == sp_property.Sysid && current8.Shopid == sp_property.Shopid && current8.Propertyid == sp_property.Propertyid && current8.Name == sp_property.Name && current8.Value == sp_property.Value && sp_property.Issellpro != 1)
                                {
                                    flag12 = true;
                                    break;
                                }
                            }
                            if (!flag12)
                            {
                                list.Add(sp_property);
                                if (!flag5)
                                {
                                    goto IL_2894;
                                }
                                if (sp_property.Issellpro == 1 && !text13.Trim().Equals(sp_property.Name.Trim()))
                                {
                                    name = text13;
                                    using (IEnumerator<Sys_sysProperty> enumerator = propertyBySortId.GetEnumerator())
                                    {
                                        while (enumerator.MoveNext())
                                        {
                                            Sys_sysProperty current9 = enumerator.Current;
                                            if (current9.Parentprovalueid == DataConvert.ToInt(drPropertyValue["Id"]))
                                            {
                                                sp_property = new Sp_property();
                                                int valuetype = current9.Valuetype;
                                                sp_property = new Sp_property();
                                                text6 = DataConvert.ToString(current9.Id);
                                                sp_property.Propertyid = current9.Id;
                                                sp_property.Issellpro = (list2.Contains(text6) ? 1 : 0);
                                                if (valuetype == 2)
                                                {
                                                    sp_property.Name = text13;
                                                    sp_property.Value = text13;
                                                    sp_property.Issellpro = 1;
                                                    list.Add(sp_property);
                                                }
                                            }
                                        }
                                        goto IL_2A96;
                                    }
                                    goto IL_2894;
                                }
                                IL_2A96:
                                SellProInfo sellProInfo = new SellProInfo();
                                if (!string.IsNullOrEmpty(value5))
                                {
                                    sellProInfo.Value = value5;
                                    if (!string.IsNullOrEmpty(text14))
                                    {
                                        sellProInfo.Name = text14;
                                    }
                                    else
                                    {
                                        sellProInfo.Name = current7.PropertyValueName;
                                    }
                                    dicProValueAndSellProInfos[current7.PropertyValue] = sellProInfo;
                                }
                                else
                                {
                                    sellProInfo.Value = DataConvert.ToString(drPropertyValue["Propertyid"]) + ":" + str2;
                                    sellProInfo.Name = name;
                                    dicProValueAndSellProInfos[text8] = sellProInfo;
                                }
                                this.Haschildproperty(DataConvert.ToBoolean(drPropertyValue["Haschildproperty"]), sysSysPropertyList, text8, dictionary6, list2, ref list);
                                continue;
                                IL_2894:
                                if (sp_property.Issellpro == 1 && !text11.Trim().Equals(sp_property.Name.Trim()) && num4 <= 24 && string.IsNullOrEmpty(sp_property.Aliasname) && (!(current7.PropertyName == "尺码") || (!("US" + text11.Trim().ToUpper()).Equals(sp_property.Name.Trim().ToUpper()) && !("UK" + text11.Trim().ToUpper()).Equals(sp_property.Name.Trim().ToUpper()) && !("EUR" + text11.Trim().ToUpper()).Equals(sp_property.Name.Trim().ToUpper()))) && ((!(current7.PropertyName == "参考身高") && !(current7.PropertyName == "尺码")) || !text11.Equals(sp_property.Name + "cm")))
                                {
                                    name = text11;
                                    foreach (Sys_sysProperty current10 in propertyBySortId)
                                    {
                                        if (current10.Parentprovalueid == DataConvert.ToInt(drPropertyValue["Id"]) && current10.Parentprovalueid > 0)
                                        {
                                            sp_property = new Sp_property();
                                            int valuetype2 = current10.Valuetype;
                                            sp_property = new Sp_property();
                                            text6 = DataConvert.ToString(current10.Id);
                                            sp_property.Propertyid = current10.Id;
                                            sp_property.Issellpro = (list2.Contains(text6) ? 1 : 0);
                                            if (valuetype2 == 2)
                                            {
                                                sp_property.Name = text11;
                                                sp_property.Value = text11;
                                                sp_property.Issellpro = 1;
                                                list.Add(sp_property);
                                            }
                                        }
                                    }
                                    goto IL_2A96;
                                }
                                goto IL_2A96;
                            }
                        }
                    }
                }
            }
            return list;
        }
        private IList<Sp_property> SetspPropertyList(Item item, IList<Sys_sysProperty> sysSysPropertyList, DataTable dtPropertyValue, int sortId, out Dictionary<string, SellProInfo> dicProValueAndSellProInfos, out string code, out Dictionary<string, SellNewProInfo> sourceProValueAndNewProValue)
        {
            bool flag = ConfigHelper.TaoBaoCItemPicShieldCheck;// DataConvert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", base.ToolCode.ToUpper().ToString(), "CItemPicShieldCheck", "false"));
            sourceProValueAndNewProValue = new Dictionary<string, SellNewProInfo>();
            bool flag2 = ConfigHelper.TaoBaoSellProperyPic;//  Convert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", "PicSet", "SellProperyPic", "false"));
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
                                                    text8 = imageOperator.TransPicture(text8, ToolCode, item.NumIid.ToString(), TransPictureType.MovePicture);
                                                    if (flag2 && !string.IsNullOrEmpty(text8))
                                                    {
                                                        text8 = string.Empty;
                                                    }
                                                    if (!string.IsNullOrEmpty(text8))
                                                    {
                                                        sp_property.Name = text8;
                                                        sp_property.Value = text8;
                                                        sp_property.Issellpro = 1;
                                                        if (ToolCode == "UPLOADTOYF")
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
                        Sys_sysProperty propertyById = DataHelper.GetPropertyById(num5);
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
                                                text9 = imageOperator.TransPicture(text9, ToolCode, item.NumIid.ToString(), TransPictureType.MovePicture);
                                                if (flag2 && !string.IsNullOrEmpty(text9))
                                                {
                                                    text9 = string.Empty;
                                                }
                                                if (!string.IsNullOrEmpty(text9))
                                                {
                                                    sp_property.Name = text9;
                                                    sp_property.Value = text9;
                                                    sp_property.Issellpro = 1;
                                                    if (ToolCode == "UPLOADTOYF")
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
        private void Haschildproperty(bool haschildproperty, IList<Sys_sysProperty> sysSysPropertyList, string newProValue, Dictionary<string, string> dicProNameAndValue, List<string> lstSellProId, ref IList<Sp_property> lstEntSpProperty)
        {
            if (!haschildproperty)
            {
                return;
            }
            foreach (Sys_sysProperty current in sysSysPropertyList)
            {
                if (current.Parentpropertyvalue.Equals(newProValue) && current.Valuetype == 2 && dicProNameAndValue.ContainsKey(current.Name))
                {
                    Sp_property sp_property = new Sp_property();
                    string item = DataConvert.ToString(current.Id);
                    sp_property.Propertyid = current.Id;
                    sp_property.Value = dicProNameAndValue[current.Name];
                    sp_property.Issellpro = (lstSellProId.Contains(item) ? 1 : 0);
                    sp_property.Name = current.Name;
                    lstEntSpProperty.Add(sp_property);
                }
            }
        }
        public string SubStringToMaxByteLength(string str, int maxByteLength)
        {
            if (str.Equals(string.Empty))
            {
                return string.Empty;
            }
            if (str.Length * 2 <= maxByteLength)
            {
                return str;
            }
            string text = string.Empty;
            string text2 = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                text2 = text + str[i];
                int stringByteLength = this.GetStringByteLength(text2);
                if (stringByteLength > maxByteLength)
                {
                    return text;
                }
                text = text2;
            }
            return text;
        }
        private int GetStringByteLength(string str)
        {
            if (str.Equals(string.Empty))
            {
                return 0;
            }
            int num = 0;
            ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
            byte[] bytes = aSCIIEncoding.GetBytes(str);
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63)
                {
                    num++;
                }
                num++;
            }
            return num;
        }
        private Dictionary<string, string> GetInputPidAndStr(Item item)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (item != null && item.InputPids != null && item.InputStr != null)
            {
                string[] array = item.InputPids.Split(new string[]
                {
            ","
                }, StringSplitOptions.RemoveEmptyEntries);
                string[] array2 = item.InputStr.Split(new string[]
                {
            ","
                }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length > 0 && array.Length == array2.Length)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        dictionary[array[i]] = array2[i];
                    }
                }
            }
            return dictionary;
        }
        private List<PropsViewEntity> GetLstPropsDTO(string propsName)
        {
            List<PropsViewEntity> list = new List<PropsViewEntity>();
            if (string.IsNullOrEmpty(propsName))
            {
                return list;
            }
            string pattern = "(?<PropertyValueAndName>(?<PropertyValue>(?<ProKey>\\d*):-?\\d*):(?<PropertyName>.*?):(?<PropertyValueName>.*?));";
            MatchCollection matchCollection = Regex.Matches(propsName + ";", pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matchCollection)
            {
                list.Add(new PropsViewEntity
                {
                    PropertyValueAndName = match.Groups["PropertyValueAndName"].Value,
                    PropertyValue = match.Groups["PropertyValue"].Value,
                    ProKey = match.Groups["ProKey"].Value,
                    PropertyName = match.Groups["PropertyName"].Value,
                    PropertyValueName = match.Groups["PropertyValueName"].Value.Replace("#scln#", ";")
                });
            }
            return list;
        }
        private Dictionary<string, string> GetDicCustomProperty()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string sysConfig = DataHelper.GetSysConfig("AppConfig", "Taobao", "CustomSellProperty", "");
            string sysConfig2 = DataHelper.GetSysConfig("AppConfig", "Taobao", "CustomSellProperty2", "");
            string text = sysConfig + sysConfig2;
            if (!string.IsNullOrEmpty(text))
            {
                text = text.TrimEnd(new char[]
                {
            '|'
                });
                string[] array = text.Split(new char[]
                {
            '|'
                });
                if (array != null && array.Length > 0)
                {
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text2 = array2[i];
                        if (!string.IsNullOrEmpty(text2))
                        {
                            string[] array3 = text2.Split(new char[]
                            {
                        ','
                            });
                            if (array3 != null && array3.Length == 2)
                            {
                                dictionary[array3[0]] = array3[1];
                            }
                        }
                    }
                }
            }
            return dictionary;
        }
        private string GetLikeStrForDtSelect(string likeOrdinaryStr)
        {
            string text = DbUtil.OerateSpecialChar(likeOrdinaryStr);
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }
            return text.Replace("[", "【").Replace("]", "】").Replace("%", "[%]").Replace("_", "[_]").Replace("'", "，").Replace("*", "[*]");
        }
        private bool IsOldSizeAndNewTaobao(Dictionary<string, string> dicCustomProperty, string sortAllName, string propertyName)
        {
            bool result = false;
            if (dicCustomProperty != null && dicCustomProperty.Count > 0 && !string.IsNullOrEmpty(sortAllName) && !string.IsNullOrEmpty(propertyName) && dicCustomProperty.ContainsKey(sortAllName) && dicCustomProperty[sortAllName].Contains(propertyName))
            {
                result = true;
            }
            return result;
        }
        private void SellProConvertCustom(string proName, string proKey, Sp_property property, Dictionary<string, int> dicPidAndOtherValue)
        {
            if (dicPidAndOtherValue == null || dicPidAndOtherValue.Count == 0)
            {
                string value = proKey + ":-1001";
                dicPidAndOtherValue.Add(proKey, -1001);
                property.Name = proName;
                property.Value = value;
            }
            else if (dicPidAndOtherValue.ContainsKey(proKey))
            {
                Dictionary<string, int> dictionary;
                string key;
                (dictionary = dicPidAndOtherValue)[key = proKey] = dictionary[key] + -1;
                string value2 = proKey + ":" + dicPidAndOtherValue[proKey];
                property.Name = proName;
                property.Value = value2;
            }
            else
            {
                string value3 = proKey + ":-1001";
                dicPidAndOtherValue.Add(proKey, -1001);
                property.Name = proName;
                property.Value = value3;
            }
        }
        private IList<Sp_property> GetSpPropertyList(IList<Sp_property> spPropertyList, int shopId, int sysId)
        {
            bool flag = ConfigHelper.TaoBaoSellProperyPic;// Convert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", "PicSet", "SellProperyPic", "false"));
            IList<Sp_property> list = new List<Sp_property>();
            if (spPropertyList == null)
            {
                return list;
            }
            foreach (Sp_property current in spPropertyList)
            {
                current.Shopid = shopId;
                current.Sysid = sysId;
                current.Modifytime = DateTime.Now;
                if (flag && !string.IsNullOrEmpty(current.PicUrl))
                {
                    current.PicUrl = string.Empty;
                }
                list.Add(current);
            }
            return list;
        }
        private IList<Sp_sellProperty> GetSpSellPropertyList(Item item, int sysId, int shopId, Dictionary<string, SellProInfo> dictParam, Dictionary<string, string> dicPromoPrice, ref decimal minPrice, Dictionary<string, string> dicSetOldToNew, out int totalNum)
        {
            IList<Sp_sellProperty> list = new List<Sp_sellProperty>();
            totalNum = 0;
            Dictionary<string, Sp_sellProperty> dictionary = new Dictionary<string, Sp_sellProperty>();
            string key = string.Empty;
            string text = string.Empty;
            StringBuilder stringBuilder = null;
            StringBuilder stringBuilder2 = null;
            string pattern = "(?<proValue>(-)?\\d*:(-)?\\d*):(?<parentProName>.*?):.*?;";
            new Regex(pattern, RegexOptions.IgnoreCase);
            decimal num = 0m;
            if (dicPromoPrice != null && dicPromoPrice.Count > 0)
            {
                foreach (KeyValuePair<string, string> current in dicPromoPrice)
                {
                    if (num == 0m)
                    {
                        num = DataConvert.ToDecimal(current.Value);
                    }
                    else
                    {
                        if (num < DataConvert.ToDecimal(current.Value))
                        {
                            num = DataConvert.ToDecimal(current.Value);
                        }
                    }
                }
            }
            if (item != null && item.Skus != null)
            {
                foreach (Sku current2 in item.Skus)
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder2 = new StringBuilder();
                    MatchCollection matchCollection = Regex.Matches(current2.PropertiesName + ";", pattern, RegexOptions.IgnoreCase);
                    if (matchCollection != null && matchCollection.Count > 0)
                    {
                        foreach (Match match in matchCollection)
                        {
                            if (match.Success && match.Groups["proValue"].Success && match.Success && match.Groups["parentProName"].Success)
                            {
                                key = match.Groups["proValue"].Value;
                                if (dicSetOldToNew != null && dicSetOldToNew.Count > 0 && dicSetOldToNew.ContainsKey(key))
                                {
                                    key = dicSetOldToNew[key];
                                }
                                if (dictParam.ContainsKey(key))
                                {
                                    text = match.Groups["parentProName"].Value + ":" + dictParam[key].Name;
                                    stringBuilder.Append(dictParam[key].Value + ":" + text.Replace('|', '*') + "|");
                                    stringBuilder2.Append(text + ";");
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(stringBuilder.ToString()))
                        {
                            Sp_sellProperty sp_sellProperty = new Sp_sellProperty();
                            sp_sellProperty.Shopid = shopId;
                            sp_sellProperty.Sysid = sysId;
                            sp_sellProperty.Barcode = current2.Barcode;
                            sp_sellProperty.Sellproinfos = stringBuilder.ToString().Trim(new char[]
                            {
                        '|'
                            });
                            if (this.IsSnatchPromoPrice && dicPromoPrice != null && dicPromoPrice.Count > 0)
                            {
                                if (dicPromoPrice.ContainsKey(DataConvert.ToString(current2.SkuId)))
                                {
                                    sp_sellProperty.Price = DataConvert.ToDecimal(dicPromoPrice[DataConvert.ToString(current2.SkuId)]);
                                    if (minPrice > DataConvert.ToDecimal(dicPromoPrice[DataConvert.ToString(current2.SkuId)]))
                                    {
                                        minPrice = DataConvert.ToDecimal(dicPromoPrice[DataConvert.ToString(current2.SkuId)]);
                                    }
                                }
                                else
                                {
                                    sp_sellProperty.Price = ((num == 0m) ? DataConvert.ToDecimal(current2.Price) : num);
                                }
                            }
                            else
                            {
                                sp_sellProperty.Price = DataConvert.ToDecimal(current2.Price);
                            }
                            sp_sellProperty.Nums = DataConvert.ToInt(current2.Quantity);
                            sp_sellProperty.Code = current2.OuterId;
                            sp_sellProperty.Name = stringBuilder2.ToString().Trim(new char[]
                            {
                        ';'
                            });
                            sp_sellProperty.Modifytime = DateTime.Now;
                            if (dictionary.ContainsKey(sp_sellProperty.Name))
                            {
                                dictionary[sp_sellProperty.Name].Nums += sp_sellProperty.Nums;
                            }
                            else
                            {
                                dictionary[sp_sellProperty.Name] = sp_sellProperty;
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, Sp_sellProperty> current3 in dictionary)
                {
                    current3.Value.Nums = ((current3.Value.Nums > 99999) ? 99999 : current3.Value.Nums);
                    list.Add(current3.Value);
                    totalNum += current3.Value.Nums;
                }
            }
            return list;
        }
        private IList<Sp_sellProperty> GetSpSellPropertyList(Item item, int sysId, int shopId, Dictionary<string, SellProInfo> dictParam, GetItemPromoPrice.TaobaoPromoPriceEntity entity, ref decimal minPrice, Dictionary<string, SellNewProInfo> sourceProValueAndNewProValue, Dictionary<string, string> dicSetOldToNew, out int totalNum)
        {
            IList<Sp_sellProperty> list = new List<Sp_sellProperty>();
            totalNum = 0;
            Dictionary<string, Sp_sellProperty> dictionary = new Dictionary<string, Sp_sellProperty>();
            string key = string.Empty;
            string text = string.Empty;
            StringBuilder stringBuilder = null;
            StringBuilder stringBuilder2 = null;
            string pattern = "(?<proValue>(-)?\\d*:(-)?\\d*):(?<parentProName>.*?):.*?;";
            new Regex(pattern, RegexOptions.IgnoreCase);
            decimal num = 0m;
            if (entity != null && entity.availSKUs != null)
            {
                foreach (KeyValuePair<string, GetItemPromoPrice.AvailSKU> current in entity.availSKUs)
                {
                    if (entity.availSKUs.Count <= 1 || !(current.Key == "def"))
                    {
                        if (num == 0m)
                        {
                            if (DataConvert.ToDecimal(current.Value.promoPrice) != 0m)
                            {
                                num = DataConvert.ToDecimal(current.Value.promoPrice);
                            }
                            else
                            {
                                if (DataConvert.ToDecimal(current.Value.price) != 0m)
                                {
                                    num = DataConvert.ToDecimal(current.Value.price);
                                }
                            }
                        }
                        else
                        {
                            if (DataConvert.ToDecimal(current.Value.promoPrice) != 0m)
                            {
                                if (num < DataConvert.ToDecimal(current.Value.promoPrice))
                                {
                                    num = DataConvert.ToDecimal(current.Value.promoPrice);
                                }
                            }
                            else
                            {
                                if (DataConvert.ToDecimal(current.Value.price) != 0m && num < DataConvert.ToDecimal(current.Value.price))
                                {
                                    num = DataConvert.ToDecimal(current.Value.price);
                                }
                            }
                        }
                    }
                }
            }
            if (item != null && item.Skus != null)
            {
                foreach (Sku current2 in item.Skus)
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder2 = new StringBuilder();
                    MatchCollection matchCollection = Regex.Matches(current2.PropertiesName + ";", pattern, RegexOptions.IgnoreCase);
                    if (matchCollection != null && matchCollection.Count > 0)
                    {
                        foreach (Match match in matchCollection)
                        {
                            if (match.Success && match.Groups["proValue"].Success && match.Success && match.Groups["parentProName"].Success)
                            {
                                key = match.Groups["proValue"].Value;
                                if (dicSetOldToNew != null && dicSetOldToNew.Count > 0 && dicSetOldToNew.ContainsKey(key))
                                {
                                    key = dicSetOldToNew[key];
                                }
                                if (dictParam.ContainsKey(key))
                                {
                                    text = match.Groups["parentProName"].Value + ":" + dictParam[key].Name;
                                    stringBuilder.Append(dictParam[key].Value + ":" + text.Replace('|', '*') + "|");
                                    stringBuilder2.Append(text + ";");
                                }
                                else
                                {
                                    if (sourceProValueAndNewProValue.ContainsKey(key))
                                    {
                                        text = match.Groups["parentProName"].Value + ":" + sourceProValueAndNewProValue[key].Name;
                                        stringBuilder.Append(sourceProValueAndNewProValue[key].Value + ":" + text.Replace('|', '*') + "|");
                                        stringBuilder2.Append(text + ";");
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(stringBuilder.ToString()))
                        {
                            Sp_sellProperty sp_sellProperty = new Sp_sellProperty();
                            sp_sellProperty.Shopid = shopId;
                            sp_sellProperty.Sysid = sysId;
                            sp_sellProperty.Barcode = current2.Barcode;
                            sp_sellProperty.Sellproinfos = stringBuilder.ToString().Trim(new char[]
                            {
                        '|'
                            });
                            if (this.IsSnatchPromoPrice && entity != null && entity.availSKUs != null)
                            {
                                GetItemPromoPrice.AvailSKU availSKU = this.isContains(entity.availSKUs, current2.Properties);
                                if (availSKU != null)
                                {
                                    if (DataConvert.ToDecimal(availSKU.promoPrice) != 0m)
                                    {
                                        sp_sellProperty.Price = DataConvert.ToDecimal(availSKU.promoPrice);
                                        if (minPrice > DataConvert.ToDecimal(availSKU.promoPrice))
                                        {
                                            minPrice = DataConvert.ToDecimal(availSKU.promoPrice);
                                        }
                                    }
                                    else
                                    {
                                        if (DataConvert.ToDecimal(availSKU.price) != 0m)
                                        {
                                            sp_sellProperty.Price = DataConvert.ToDecimal(availSKU.price);
                                            if (minPrice > DataConvert.ToDecimal(availSKU.price))
                                            {
                                                minPrice = DataConvert.ToDecimal(availSKU.price);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    sp_sellProperty.Price = ((num == 0m) ? DataConvert.ToDecimal(current2.Price) : num);
                                }
                            }
                            else
                            {
                                sp_sellProperty.Price = DataConvert.ToDecimal(current2.Price);
                            }
                            sp_sellProperty.Nums = DataConvert.ToInt(current2.Quantity);
                            sp_sellProperty.Code = current2.OuterId;
                            sp_sellProperty.Name = stringBuilder2.ToString().Trim(new char[]
                            {
                        ';'
                            });
                            sp_sellProperty.Modifytime = DateTime.Now;
                            if (dictionary.ContainsKey(sp_sellProperty.Name))
                            {
                                dictionary[sp_sellProperty.Name].Nums += sp_sellProperty.Nums;
                            }
                            else
                            {
                                dictionary[sp_sellProperty.Name] = sp_sellProperty;
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, Sp_sellProperty> current3 in dictionary)
                {
                    current3.Value.Nums = ((current3.Value.Nums > 99999) ? 99999 : current3.Value.Nums);
                    list.Add(current3.Value);
                    totalNum += current3.Value.Nums;
                }
            }
            if (num != 0m && num < minPrice)
            {
                minPrice = num;
            }
            return list;
        }
        private GetItemPromoPrice.AvailSKU isContains(Dictionary<string, GetItemPromoPrice.AvailSKU> entity, string properties)
        {
            string[] t = properties.Split(';');
            List<string[]> permutation = PermutationAndCombination<string>.GetPermutation(t);
            foreach (string[] item in permutation)
            {
                string key = string.Join(";", item);
                if (entity.ContainsKey(key))
                {
                    return entity[key];
                }
            }
            return null;
        }
        private IList<Sp_pictures> TransAndSavePicture(Dictionary<string, ItemImg> dicTempPictureNameAndItemImg, string toolCode, string onlineKey, bool isChkTBShield, string cId)
        {
            bool flag = ConfigHelper.TaoBaoFirstMianPic;// Convert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", "PicSet", "FirstMianPic", "false"));
            if (dicTempPictureNameAndItemImg == null || dicTempPictureNameAndItemImg.Count <= 0)
            {
                return null;
            }
            IList<Sp_pictures> list = new List<Sp_pictures>();
            ImageOperator imageOperator = new ImageOperator();
            string localpath = string.Empty;
            Sp_pictures sp_pictures = null;
            foreach (KeyValuePair<string, ItemImg> current in dicTempPictureNameAndItemImg)
            {
                if (!string.IsNullOrEmpty(current.Key))
                {
                    Sp_pictures sp_pictures2 = new Sp_pictures();
                    localpath = imageOperator.TransPicture(current.Key, toolCode, onlineKey, TransPictureType.MovePicture);
                    sp_pictures2.Localpath = localpath;
                    sp_pictures2.Url = current.Value.Url;
                    sp_pictures2.Keys = DataConvert.ToString(current.Value.Id);
                    sp_pictures2.Picindex = DataConvert.ToInt(current.Value.Position);
                    if (sp_pictures == null)
                    {
                        sp_pictures = sp_pictures2;
                    }
                    if (flag)
                    {
                        if (!string.IsNullOrEmpty(sp_pictures2.Url) && sp_pictures2.Url.Contains("0-item_pic"))
                        {
                            list.Add(sp_pictures2);
                            break;
                        }
                    }
                    else
                    {
                        list.Add(sp_pictures2);
                    }
                }
            }
            if (flag && list.Count <= 0 && sp_pictures != null)
            {
                list.Add(sp_pictures);
            }
            return list;
        }

    }
}
