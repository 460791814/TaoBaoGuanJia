using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoGuanJia.Core.TaoBao
{
   public class DownloadDetailTaobaoService
    {
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
            sp_item.Sszgusername = CurrentLoginUser.UserName;
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

    }
}
