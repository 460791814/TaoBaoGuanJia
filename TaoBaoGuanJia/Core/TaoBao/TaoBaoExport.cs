using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Core.TaoBao
{

    public class TaoBaoExport
    {
        public string[] TaobaoPrepareCSVData(ProductItem productItem)
        {
            string[] array = new string[64];
            if (Encoding.Default.GetByteCount(productItem.Name) > 60)
            {
                string[] array2 = productItem.Name.Split(new string[]
                {
                    " "
                }, StringSplitOptions.RemoveEmptyEntries);
                string text = string.Empty;
                if (array2 != null && array2.Length > 0)
                {
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text2 = array2[i];
                        string text3 = text2;
                        if (text2.StartsWith("【") && text2.IndexOf("】") > 0)
                        {
                            text3 = text2.Substring(text2.IndexOf("】") + 1);
                        }
                        if (Encoding.Default.GetByteCount(text + text3) < 60)
                        {
                            text = text + " " + text3;
                        }
                        else
                        {
                            if (Encoding.Default.GetByteCount(text + text3) == 60)
                            {
                                text += text3;
                            }
                            else
                            {
                                text += text3;
                                //string userConfig = ToolServer.get_ConfigData().GetUserConfig("AppConfig", "DefaultConfig", "ThisToolName", "");
                                //if (!(userConfig == "SNATCHJD"))
                                //{
                                //    text = Encoding.Default.GetString(Encoding.Default.GetBytes(text), 0, 60);
                                //    break;
                                //}
                                //if (DataConvert.ToBoolean(ToolServer.get_ConfigData().GetUserConfig("AppConfig", "SNATCHJD", "SourceItemName", "false")))
                                //{
                                //    text = Encoding.Default.GetString(Encoding.Default.GetBytes(text), 0, 60);
                                //    break;
                                //}
                            }
                        }
                    }
                    productItem.Name = text.TrimEnd(new char[]
                    {
                        '?'
                    });
                }
            }
            array[0] = this.ConvertToSaveCellCommon(productItem.Name);
            array[1] = productItem.ProductSortKeys;
            array[2] = "\"" + string.Empty + "\"";
            array[3] = (string.IsNullOrEmpty(productItem.ActualNewOrOld) ? string.Empty : productItem.ActualNewOrOld.Trim());
            array[4] = (string.IsNullOrEmpty(productItem.Province) ? ("\"" + string.Empty + "\"") : ("\"" + productItem.Province.Trim() + "\""));
            array[5] = (string.IsNullOrEmpty(productItem.City) ? ("\"" + string.Empty + "\"") : ("\"" + productItem.City.Trim() + "\""));
            string text4 = string.IsNullOrEmpty(productItem.SellType) ? string.Empty : productItem.SellType.Trim();
            if (text4.Equals("b"))
            {
                array[6] = "1";
            }
            else
            {
                if (text4.Equals("a"))
                {
                    array[6] = "2";
                }
            }
            array[7] = DataConvert.ToString(productItem.Price);
            array[8] = DataConvert.ToString(productItem.PriceRise);
            array[9] = DataConvert.ToString(productItem.Nums);
            array[10] = (string.IsNullOrEmpty(productItem.validDate) ? string.Empty : productItem.validDate.Trim());
            array[11] = (string.IsNullOrEmpty(productItem.ShipWay) ? string.Empty : productItem.ShipWay.Trim());
            array[12] = productItem.ActualShipSlow;
            array[13] = productItem.ActualShipEMS;
            array[14] = productItem.ActualShipFast;
            array[15] = (string.IsNullOrEmpty(productItem.IsTicket) ? string.Empty : productItem.IsTicket.Trim());
            array[16] = (string.IsNullOrEmpty(productItem.IsRepair) ? string.Empty : productItem.IsRepair.Trim());
            array[17] = productItem.OnSell;
            if (string.IsNullOrEmpty(productItem.IsRmd))
            {
                array[18] = "0";
            }
            else
            {
                array[18] = DataConvert.ToString(DataConvert.ToInt(DataConvert.ToBoolean(productItem.IsRmd.ToLower())));
            }
            array[19] = this.ConvertToSaveCellCommon(productItem.ActualOnSellDate);
            productItem.Content = this.ClearVedio(productItem.Content);
            array[20] = this.ConvertToSaveCell(productItem.Content);
            array[21] = this.ConvertToSaveCellCommon(productItem.PropertyValue);
            array[22] = productItem.ActualShipTpl;
            string discount = productItem.Discount;
            array[23] = (DataConvert.ToBoolean(discount) ? "1" : "0");
            array[24] = "\"\"";
            array[25] = "200";
            array[26] = "";
            array[27] = "0";
            array[28] = this.ConvertToSaveCellCommon(productItem.Photo);
            array[29] = "\"\"";
            array[30] = this.ConvertToSaveCellCommon(productItem.SellProperty);
            array[31] = this.ConvertToSaveCellCommon(productItem.UserInputPropIDs);
            array[32] = this.ConvertToSaveCellCommon(productItem.UserInputPropValues);
            array[33] = this.ConvertToSaveCellCommon(productItem.Code);
            array[34] = this.ConvertToSaveCellCommon(productItem.CustomProperty);
            array[35] = "0";
            array[36] = productItem.OnlineKey;
            array[37] = "0";
            array[38] = "";
            array[39] = DataConvert.ToString(productItem.SszgUserName);
            array[40] = "0";
            array[41] = "0";
            array[42] = "0";
            array[43] = this.ConvertToSaveCell(productItem.FoodParame);
            array[44] = productItem.SubStock;
            array[45] = productItem.ItemSize;
            array[46] = productItem.ItemWeight;
            array[47] = "0";
            array[48] = "-1";
            array[49] = string.Empty;
            array[50] = this.ConvertToSaveCell(productItem.Wirelessdesc);
            array[51] = productItem.Barcode;
            array[52] = this.ConvertToSaveCell(productItem.SellPoint);
            array[53] = productItem.SkuBarcode;
            array[54] = this.ConvertToSaveCellCommon(productItem.CpvMemo);
            array[55] = this.ConvertToSaveCellCommon(productItem.InputCustomCpv);
            array[56] = productItem.Features;
            array[61] = productItem.OperateTypes;
            array[62] = "0";
            return array;
        }

        public string[] ConvertProductToDic(ProductItem productItem, string photoPath)
        {
            string text = DataConvert.ToString(productItem.Id);
            string value = DataConvert.ToString(productItem.SysSortId);
            string newOrOld = DataConvert.ToString(productItem.NewOrOld);
            DataConvert.ToString(productItem.Province);
            DataConvert.ToString(productItem.City);
            DataConvert.ToString(productItem.SellType);
            DataConvert.ToString(productItem.validDate);
            DataConvert.ToString(productItem.ShipWay);
            DataConvert.ToString(productItem.IsTicket);
            DataConvert.ToString(productItem.IsRepair);
            DataConvert.ToString(productItem.IsRmd);
            DataConvert.ToString(productItem.UseShipTpl);
            DataConvert.ToString(productItem.ShipTplId);
            string onSell = DataConvert.ToString(productItem.OnSell).Trim();
            string onSellDate = DataConvert.ToString(productItem.OnSellDate).Trim();
            string onSellHour = DataConvert.ToString(productItem.OnSellHour).Trim();
            string onSellMin = DataConvert.ToString(productItem.OnSellMin).Trim();
            productItem.ActualNewOrOld = this.GetNewOrOld(newOrOld);
            int sysId = 1;
            if (!string.IsNullOrEmpty(value))
            {
                productItem.ProductSortKeys = DataHelper.GetSortKeyBySortId(productItem.SysSortId);
                productItem.SortKey = productItem.ProductSortKeys;
            }
            this.HandleOnSell(onSell, onSellDate, onSellHour, onSellMin, productItem);
            this.HandleShipWay(productItem);
            string empty = string.Empty;
            productItem.SellProperty = this.HandleSellProperty(productItem.Id, out empty, productItem);
            productItem.SkuBarcode = empty;
            this.HandleProperty(text, productItem);
            productItem.Photo = this.HandlePhoto(text, photoPath, productItem.SortKey);
            string empty2 = string.Empty;
            Sys_sysSort sortBySysIdAndKeys = DataHelper.GetSortBySysIdAndKeys(1, productItem.SortKey);
            if (sortBySysIdAndKeys == null || !sortBySysIdAndKeys.IsNewSellPro)
            {
                string inputCustomCpv = "";
                productItem.CustomProperty = this.HandleCustomPropertyName(text, out empty2, out inputCustomCpv);
                productItem.InputCustomCpv = inputCustomCpv;
            }
            else
            {
                string cpvMemo = "";
                string inputCustomCpv2 = "";
                productItem.CustomProperty = this.NewHandleCustomPropertyName(text, out cpvMemo, out inputCustomCpv2);
                productItem.InputCustomCpv = inputCustomCpv2;
                productItem.CpvMemo = cpvMemo;
            }
            productItem.FoodParame = this.HandleFoodSecurity(text);
            bool flag = ConfigHelper.IsExportMobileDesc;// DataConvert.ToBoolean(ToolServer.ConfigData.GetUserConfig("AppConfig", "IsExportMobileDesc", this._exportPackageDao.ToolCode, "false"));
            if (flag)
            {
                string mobilePhotoPath = Path.Combine(photoPath, Utils.GetFileNameWithOutInvalid(productItem.Name) + productItem.Id + "\\m");
                productItem.Wirelessdesc = this.HandleMobileDesc(productItem.Wirelessdesc, mobilePhotoPath);
            }
            else
            {
                productItem.Wirelessdesc = string.Empty;
            }
            //下载PC内容图片
            DownloadItemContentPic(productItem, photoPath);
            if (!string.IsNullOrEmpty(productItem.OperateTypes))
            {
                string stringToEscape = "{\"bathrobe_field_tag_image_1\" : \"\", \"bathrobe_field_tag_image_2\" : \"\",\"var_item_board_inspection_report\" : \"\",\"var_item_glove_inspection_report_1\" : \"\",\"var_item_light_inspection_report_2\" : \"\",\"var_org_auth_tri_c_code\" : \"" + productItem.OperateTypes + "\",\"var_tri_c_cer_image\" : \"\", \"var_tri_c_cer_image_2\" : \"\" }";
                productItem.OperateTypes = Uri.EscapeDataString(stringToEscape);
            }
            return this.TaobaoPrepareCSVData(productItem);
        }
        private void DownloadItemContentPic(ProductItem productItem, string photoPath)
        {

            try
            {

                if (productItem != null && !string.IsNullOrEmpty(productItem.Content))
                {
                    string path = Path.Combine(photoPath, Utils.GetFileNameWithOutInvalid(productItem.Name) + productItem.Id + "\\pc");
                    Dictionary<string, string> _picMovedUrls = null;
                    bool _uccorError = false;
                    productItem.Content = DownloadContentPic.DownloadItemContentPic(productItem.Id, productItem.Content, path, productItem.IsTaobao5, ref _picMovedUrls, ref _uccorError);

                }
            }
            catch (Exception ex)
            {
                try
                {
                    Log.WriteLog("下载商品描述图片失败", ex);
                }
                catch
                {
                }
            }
            finally
            {

            }
        }
        private string HandlePhoto(string itemId, string photoPath, string sortKey)
        {
            string text = "";
            IList<Sp_pictures> photos = DataHelper.GetPhotos(itemId);
            foreach (Sp_pictures current in photos)
            {
                string localpath = current.Localpath;
                int num = localpath.LastIndexOf("\\");
                if (num != -1)
                {
                    string text2 = localpath.Substring(num + 1);
                    if (text2.LastIndexOf(".") != -1)
                    {
                        text2 = text2.Substring(0, text2.LastIndexOf("."));
                    }
                    Regex regex = new Regex("(?is)[\\u4E00-\\u9FA5]");
                    if (regex.Match(text2).Success)
                    {
                        text2 = Guid.NewGuid().ToString().Replace(" ", ".").Replace("-", ".");
                    }
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        text2,
                        ":1:",
                        current.Picindex,
                        ":|;"
                    });
                    string targetFileFullName = Path.Combine(photoPath, text2 + ".tbi");
                    string targetFilePath = Path.Combine(ConfigHelper.GetCurrentDomainDirectory(), localpath);
                    if (File.Exists(targetFilePath))
                    {
                        ImageCompress.ChangeImage(targetFilePath, 512000L, targetFileFullName);
                    }
                }
            }
            IList<Sp_property> customColorList = this.GetCustomColorList(DataConvert.ToInt(itemId), sortKey);
            string text3 = "";
            Sys_sysSort sortBySysIdAndKeys = DataHelper.GetSortBySysIdAndKeys(1, sortKey);
            for (int i = 0; i < customColorList.Count; i++)
            {
                string text4;
                string text5;
                if (sortBySysIdAndKeys != null && sortBySysIdAndKeys.IsNewSellPro)
                {
                    DataConvert.ToString(customColorList[i].Id);
                    text4 = ((customColorList[i].PicUrl == null) ? "" : customColorList[i].PicUrl);
                    text5 = customColorList[i].Value;
                }
                else
                {
                    DataConvert.ToString(customColorList[i].Id);
                    text4 = ((customColorList[i].Value == null) ? "" : customColorList[i].Value);
                    text5 = ((customColorList[i].SysProperty.Keys == null) ? "" : customColorList[i].SysProperty.Keys);
                    string text6 = (customColorList[i].SysProperty.Parentpropertyvalue == null) ? "" : customColorList[i].SysProperty.Parentpropertyvalue;
                    if (text5.IndexOf(":") == -1 && !string.IsNullOrEmpty(text6))
                    {
                        text5 = text6;
                    }
                }
                int num2 = text4.LastIndexOf("\\");
                if (num2 != -1)
                {
                    string text7 = text4.Substring(num2 + 1);
                    if (text7.LastIndexOf(".") != -1)
                    {
                        text7 = text7.Substring(0, text7.LastIndexOf("."));
                    }
                    string text8 = text3;
                    text3 = string.Concat(new string[]
                    {
                        text8,
                        text7,
                        ":2:0:",
                        text5,
                        "|;"
                    });
                    string targetFileFullName2 = (photoPath.EndsWith("\\") ? photoPath : (photoPath + "\\")) + text7 + ".tbi";
                    if (File.Exists(ConfigHelper.GetCurrentDomainDirectory() + text4))
                    {
                        ImageCompress.ChangeImage(ConfigHelper.GetCurrentDomainDirectory() + text4, 512000L, targetFileFullName2);
                    }
                }
            }
            return text + text3;
        }
        private IList<Sp_property> GetCustomColorList(int itemId, string sortKey)
        {
            IList<Sp_property> propertyListByItemId = DataHelper.GetPropertyListByItemId(DataConvert.ToInt(itemId));
            IList<Sp_property> list = new List<Sp_property>();
            Sys_sysSort sortBySysIdAndKeys = DataHelper.GetSortBySysIdAndKeys(1, sortKey);
            if (sortBySysIdAndKeys != null && sortBySysIdAndKeys.IsNewSellPro)
            {
                for (int i = 0; i < propertyListByItemId.Count; i++)
                {
                    if (!string.IsNullOrEmpty(propertyListByItemId[i].PicUrl))
                    {
                        list.Add(propertyListByItemId[i]);
                    }
                }
            }
            else
            {
                for (int j = 0; j < propertyListByItemId.Count; j++)
                {
                    if (propertyListByItemId[j].SysProperty != null && propertyListByItemId[j].SysProperty.Parentid > 0 && propertyListByItemId[j].SysProperty.Valuetype == 3)
                    {
                        list.Add(propertyListByItemId[j]);
                    }
                }
            }
            return list;
        }

        private string HandleMobileDesc(string wirelessdesc, string mobilePhotoPath)
        {

  
            bool isExportMibleDesc2500k = false;
            string a = string.Empty;
            a = "ExpMobDesc1500K";// ToolServer.ConfigData.GetUserConfig("AppConfig", "ExpMobDescMode", this._exportPackageDao.ToolCode, "");
            double num = 0.0;
            if (a == "ExpMobDesc2500K")
            {
                num = 2621440.0;
                isExportMibleDesc2500k = true;
            }
            else
            {
                if (a == "ExpMobDesc1500K")
                {
                    num = 1572864.0;
                }
                else
                {
                    if (a == "ExpMobDescWeb")
                    {
                        return wirelessdesc;
                    }
                }
            }
            if (string.IsNullOrEmpty(wirelessdesc))
            {
                return wirelessdesc;
            }
            wirelessdesc = wirelessdesc.Replace("https://", "http://");
            if (!Directory.Exists(mobilePhotoPath))
            {
                Directory.CreateDirectory(mobilePhotoPath);
            }
            string text = wirelessdesc;
            if (string.IsNullOrEmpty(wirelessdesc))
            {
                return text;
            }
            Regex regex = new Regex("(?is)<img>([^<>]*?)</img>");
            MatchCollection matchCollection = regex.Matches(text);
            if (matchCollection != null && matchCollection.Count > 0)
            {
                try
                {
                    int num2 = 0;
                    //ToolServer.ConfigData.SetUserConfig("AppConfig", "DefaultConfig", "MobileDescDowning", "true");
                    string arg = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + new Random().Next(10);
                    int num3 = 0;
                    long num4 = 0L;
                    foreach (Match match in matchCollection)
                    {
                        if (match != null && !string.IsNullOrEmpty(match.Groups[1].Value))
                        {
                            string value = match.Groups[1].Value;
                            if (value.IndexOf("http://", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                num3++;
                                bool flag = false;
                                string fileName = arg + "-" + num3;
                                string empty = string.Empty;
                                try
                                {
                                    string empty2 = string.Empty;
                                    bool flag2 = DownloadContentPic.DownLoadPicture(HttpUtility.HtmlDecode(value), mobilePhotoPath, fileName, out empty, ref flag, out empty2);
                                    if (flag2)
                                    {
                                        if (a == "ExpMobDesc2500K" || a == "ExpMobDesc1500K")
                                        {
                                            Image image = Image.FromFile(empty);
                                            if (image.Width < 480 || DataConvert.ToDecimal(image.Width) / DataConvert.ToDecimal(image.Height) > 8m)
                                            {
                                                string oldValue = "<img>" + value + "</img>";
                                                text = text.Replace(oldValue, "");
                                            }
                                            else
                                            {
                                                List<string> list = this.HandleMobilePic(empty, 620, ref num2, isExportMibleDesc2500k);
                                                string text2 = string.Empty;
                                                if (list != null && list.Count > 0)
                                                {
                                                    foreach (string current in list)
                                                    {
                                                        num4 += this.GetMobileDescAllImageSize(current);
                                                        text2 = text2 + "<img>" + current + "</img>";
                                                    }
                                                }
                                                if ((double)num4 >= num)
                                                {
                                                    string oldValue2 = "<img>" + value + "</img>";
                                                    text = text.Replace(oldValue2, "");
                                                }
                                                else
                                                {
                                                    if (this.FilterImage(1843.2f, 0.125, empty))
                                                    {
                                                        string oldValue3 = "<img>" + value + "</img>";
                                                        text = text.Replace(oldValue3, "");
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(text2))
                                                        {
                                                            string oldValue4 = "<img>" + value + "</img>";
                                                            text = text.Replace(oldValue4, text2);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string newValue = "<img>" + empty + "</img>";
                                            string oldValue5 = "<img>" + value + "</img>";
                                            text = text.Replace(oldValue5, newValue);
                                        }
                                        continue;
                                    }
                                    string oldValue6 = "<img>" + value + "</img>";
                                    text = text.Replace(oldValue6, "");
                                    continue;
                                }
                                catch (Exception)
                                {
                                    string oldValue7 = "<img>" + value + "</img>";
                                    text = text.Replace(oldValue7, "");
                                    continue;
                                }
                            }
                            if (File.Exists(value))
                            {
                                try
                                {
                                    if (a == "ExpMobDesc2500K" || a == "ExpMobDesc1500K")
                                    {
                                        string text3 = string.Empty;
                                        List<string> list2 = new List<string>();
                                        list2 = this.HandleMobilePic(value, 480, ref num2, true);
                                        foreach (string current2 in list2)
                                        {
                                            text3 = current2;
                                        }
                                        if (this.GetMobileDescAllImageSize(value) < this.GetMobileDescAllImageSize(text3))
                                        {
                                            text3 = value;
                                        }
                                        if (this.FilterImage(1843.2f, 0.125, text3))
                                        {
                                            File.Delete(value);
                                            string oldValue8 = "<img>" + value + "</img>";
                                            text = text.Replace(oldValue8, "");
                                        }
                                        else
                                        {
                                            num4 += this.GetMobileDescAllImageSize(text3);
                                            if ((double)num4 >= num)
                                            {
                                                string oldValue9 = "<img>" + value + "</img>";
                                                text = text.Replace(oldValue9, "");
                                            }
                                            else
                                            {
                                                string text4 = Path.Combine(mobilePhotoPath, text3.Substring(text3.LastIndexOf("\\") + 1));
                                                if (File.Exists(text4))
                                                {
                                                    File.SetAttributes(text4, FileAttributes.Normal);
                                                    File.Copy(text3, text4, true);
                                                }
                                                else
                                                {
                                                    File.Copy(text3, text4, false);
                                                }
                                                text = text.Replace(value, text4);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string text5 = Path.Combine(mobilePhotoPath, value.Substring(value.LastIndexOf("\\") + 1));
                                        if (File.Exists(text5))
                                        {
                                            File.SetAttributes(text5, FileAttributes.Normal);
                                            File.Copy(value, text5, true);
                                        }
                                        else
                                        {
                                            File.Copy(value, text5, false);
                                        }
                                        text = text.Replace(value, text5);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteLog("复制手机本地详情图片失败：", ex);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex2)
                {
                    Log.WriteLog("导出数据包，处理手机详情时出错：", ex2);
                }
                finally
                {
                    // ToolServer.ConfigData.SetUserConfig("AppConfig", "DefaultConfig", "MobileDescDowning", "false");
                }
            }
            return text;
        }
        private bool FilterImage(float lenght, double lengthWidthRatio, string imagePath)
        {
            bool flag = false;
            if (File.Exists(imagePath))
            {
                try
                {
                    Image image = Image.FromFile(imagePath);
                    if (image != null)
                    {
                        double num = (double)image.Width / (double)image.Height;
                        double num2 = (double)image.Height / (double)image.Width;
                        if (num <= lengthWidthRatio || num2 <= lengthWidthRatio)
                        {
                            flag = true;
                        }
                        if (!flag)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                string text = string.Empty;
                                if (!string.IsNullOrEmpty(imagePath) && imagePath.LastIndexOf(".") > 0)
                                {
                                    text = imagePath.Substring(imagePath.LastIndexOf("."));
                                }
                                if (string.IsNullOrEmpty(text))
                                {
                                    text = ".jpeg";
                                }
                                ImageFormat formatByExtension = GetFormatByExtension(text);
                                Bitmap bitmap = new Bitmap(image);
                                bitmap.Save(memoryStream, formatByExtension);
                                byte[] buffer = memoryStream.GetBuffer();
                                if ((float)buffer.Length <= lenght)
                                {
                                    flag = true;
                                }
                                bitmap.Dispose();
                            }
                        }
                        image.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex);
                }
            }
            return flag;
        }
        private static ImageFormat GetFormatByExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return ImageFormat.Jpeg;
            }
            extension = extension.ToLower().Trim();
            string a;
            ImageFormat result;
            if ((a = extension) != null)
            {
                if (a == ".jpg")
                {
                    result = ImageFormat.Jpeg;
                    return result;
                }
                if (a == ".jpeg")
                {
                    result = ImageFormat.Jpeg;
                    return result;
                }
                if (a == ".gif")
                {
                    result = ImageFormat.Gif;
                    return result;
                }
                if (a == ".png")
                {
                    result = ImageFormat.Png;
                    return result;
                }
                if (a == ".bmp")
                {
                    result = ImageFormat.Bmp;
                    return result;
                }
            }
            result = ImageFormat.Jpeg;
            return result;
        }
        private List<string> HandleMobilePic(string sourceFileName, int width, ref int totalByteSize, bool isExportMibleDesc2500k)
        {
            int num = totalByteSize;
            int num2 = 0;
            string newPicFileName = sourceFileName.Insert(sourceFileName.LastIndexOf('.'), "_dmjbs");
            Image image = Image.FromFile(sourceFileName);
            int num3 = image.Width;
            if (num3 > width)
            {
                num3 = width;
            }
            List<string> list = DownloadContentPic.CompressDescJpeg(sourceFileName, newPicFileName, num3, 960, 102400L, out num2, totalByteSize);
            List<string> list2 = new List<string>();
            if (!isExportMibleDesc2500k)
            {
                num2 = 0;
                totalByteSize = num;
                foreach (string current in list)
                {
                    newPicFileName = current.Insert(current.LastIndexOf('.'), "_dmjbs");
                    List<string> list3 = DownloadContentPic.CompressDescJpeg(current, newPicFileName, 480, 960, 102400L, out num2, totalByteSize);
                    totalByteSize += num2;
                    string item = string.Empty;
                    foreach (string current2 in list3)
                    {
                        item = current2;
                        if (this.GetMobileDescAllImageSize(current) < this.GetMobileDescAllImageSize(current2))
                        {
                            item = current;
                        }
                        list2.Add(item);
                    }
                }
                num2 = 0;
                isExportMibleDesc2500k = false;
                list = list2;
            }
            totalByteSize += num2;
            return list;
        }
        private long GetMobileDescAllImageSize(string imagePaht)
        {
            long result = 0L;
            if (!string.IsNullOrEmpty(imagePaht) && File.Exists(imagePaht))
            {
                byte[] array = File.ReadAllBytes(imagePaht);
                result = (long)array.Length;
            }
            return result;
        }
        private string GetChineseStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            Regex regex = new Regex("[\\u4E00-\\u9FA5]+");
            MatchCollection matchCollection = regex.Matches(str, 0);
            if (matchCollection.Count <= 0)
            {
                return string.Empty;
            }
            string text = string.Empty;
            for (int i = 0; i < matchCollection.Count; i++)
            {
                text += matchCollection[i].Value;
            }
            return text;
        }
        private string HandleFoodSecurity(string id)
        {
            string text = string.Empty;
            Sp_foodSecurity foodSecurityByItemId = DataHelper.GetFoodSecurityByItemId(id);
            if (foodSecurityByItemId != null)
            {
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Contact))
                {
                    text = text + "contact:" + foodSecurityByItemId.Contact + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Designcode))
                {
                    text = text + "design_code:" + foodSecurityByItemId.Designcode + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Factory))
                {
                    text = text + "factory:" + foodSecurityByItemId.Factory + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Factorysite))
                {
                    text = text + "factory_site:" + foodSecurityByItemId.Factorysite + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Foodadditive))
                {
                    text = text + "food_additive:" + foodSecurityByItemId.Foodadditive + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Mix))
                {
                    text = text + "mix:" + foodSecurityByItemId.Mix + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Period))
                {
                    text = text + "period:" + foodSecurityByItemId.Period + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Planstorage))
                {
                    text = text + "plan_storage:" + foodSecurityByItemId.Planstorage + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Prdlicenseno))
                {
                    text = text + "prd_license_no:" + foodSecurityByItemId.Prdlicenseno + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.HealthProductNo))
                {
                    text = text + "health_product_no:" + foodSecurityByItemId.HealthProductNo + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString(foodSecurityByItemId.Productdateend)))
                {
                    text = text + "product_date_end:" + foodSecurityByItemId.Productdateend.ToString("yyyy-MM-dd") + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString(foodSecurityByItemId.Productdatestart)))
                {
                    text = text + "product_date_start:" + foodSecurityByItemId.Productdatestart.ToString("yyyy-MM-dd") + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString(foodSecurityByItemId.Stockdateend)))
                {
                    text = text + "stock_date_end:" + foodSecurityByItemId.Stockdateend.ToString("yyyy-MM-dd") + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString(foodSecurityByItemId.Stockdatestart)))
                {
                    text = text + "stock_date_start:" + foodSecurityByItemId.Stockdatestart.ToString("yyyy-MM-dd") + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Supplier))
                {
                    text = text + "supplier:" + foodSecurityByItemId.Supplier + ";";
                }
                if (!string.IsNullOrEmpty(text))
                {
                    text = text.TrimEnd(new char[]
                    {
                ';'
                    });
                }
                return text;
            }
            return string.Empty;
        }
        private IList<Sp_property> NewGetCustomPropertyList(int itemId, ref List<Sp_property> AliasnameList)
        {
            IList<Sp_property> propertyListByItemId = DataHelper.GetPropertyListByItemId(DataConvert.ToInt(itemId));
            IList<Sp_property> list = new List<Sp_property>();
            for (int i = 0; i < propertyListByItemId.Count; i++)
            {
                if (propertyListByItemId[i].Issellpro == 1)
                {
                    if (propertyListByItemId[i].SysProperty != null && propertyListByItemId[i].SysProperty.Parentid > 0 && propertyListByItemId[i].SysProperty.Valuetype == 2)
                    {
                        list.Add(propertyListByItemId[i]);
                    }
                    if (propertyListByItemId[i].Value.Contains(":-10"))
                    {
                        list.Add(propertyListByItemId[i]);
                    }
                    if (!string.IsNullOrEmpty(propertyListByItemId[i].Aliasname))
                    {
                        AliasnameList.Add(propertyListByItemId[i]);
                    }
                }
            }
            return list;
        }
        private string NewHandleCustomPropertyName(string itemId, out string cpvMemo, out string inputCustomCpv)
        {
            string text = string.Empty;
            inputCustomCpv = string.Empty;
            cpvMemo = string.Empty;
            List<Sp_property> list = new List<Sp_property>();
            IList<Sp_property> list2 = this.NewGetCustomPropertyList(DataConvert.ToInt(itemId), ref list);
            for (int i = 0; i < list2.Count; i++)
            {
                DataConvert.ToString(list2[i].Id);
                string text2 = (list2[i].Name == null) ? "" : list2[i].Name;
                if (list2[i].SysProperty.Keys != null || list2[i].SysProperty.Parentpropertyvalue != null)
                {
                    if (list2[i].Value.Contains(":-10"))
                    {
                        string text3 = inputCustomCpv;
                        inputCustomCpv = string.Concat(new string[]
                        {
                    text3,
                    list2[i].Value,
                    ":",
                    text2,
                    ";"
                        });
                    }
                    else
                    {
                        string text4 = (list2[i].SysProperty.Parentpropertyvalue == null) ? "" : list2[i].SysProperty.Parentpropertyvalue;
                        string text5 = list2[i].SysProperty.Keys;
                        if (text5.IndexOf(":") == -1 && !string.IsNullOrEmpty(text4))
                        {
                            text5 = text4;
                        }
                        string text6 = text;
                        text = string.Concat(new string[]
                        {
                    text6,
                    text5,
                    ":",
                    text2,
                    ";"
                        });
                    }
                }
            }
            if (list != null && list.Count > 0)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    DataConvert.ToString(list[j].Id);
                    string text7 = (list[j].Aliasname == null) ? "" : list[j].Aliasname;
                    if (list[j].SysProperty.Keys != null || list[j].SysProperty.Parentpropertyvalue != null)
                    {
                        string text8 = cpvMemo;
                        cpvMemo = string.Concat(new string[]
                        {
                    text8,
                    list[j].Value,
                    ":",
                    text7,
                    ";"
                        });
                    }
                }
            }
            text = text.Trim(new char[]
            {
        ';'
            });
            inputCustomCpv = inputCustomCpv.Trim(new char[]
            {
        ';'
            });
            cpvMemo = cpvMemo.Trim(new char[]
            {
        ';'
            });
            return text;
        }
        private IList<Sp_property> GetCustomPropertyList(int itemId, ref List<Sp_property> customColorList)
        {
            IList<Sp_property> propertyListByItemId = DataHelper.GetPropertyListByItemId(DataConvert.ToInt(itemId));
            IList<Sp_property> list = new List<Sp_property>();
            for (int i = 0; i < propertyListByItemId.Count; i++)
            {
                if (propertyListByItemId[i].SysProperty != null && propertyListByItemId[i].SysProperty.Parentid > 0 && propertyListByItemId[i].SysProperty.Valuetype == 2)
                {
                    if (propertyListByItemId[i].SysProperty.Parentname.Contains("颜色"))
                    {
                        customColorList.Add(propertyListByItemId[i]);
                    }
                    list.Add(propertyListByItemId[i]);
                }
                if (propertyListByItemId[i].Value.Contains(":-10"))
                {
                    list.Add(propertyListByItemId[i]);
                }
            }
            return list;
        }
        private string HandleCustomPropertyName(string itemId, out string cpvmenoStr, out string inputCustomCpv)
        {
            inputCustomCpv = string.Empty;
            cpvmenoStr = string.Empty;
            List<Sp_property> list = new List<Sp_property>();
            IList<Sp_property> customPropertyList = this.GetCustomPropertyList(DataConvert.ToInt(itemId), ref list);
            string text = "";
            for (int i = 0; i < customPropertyList.Count; i++)
            {
                DataConvert.ToString(customPropertyList[i].Id);
                string text2 = (customPropertyList[i].Name == null) ? "" : customPropertyList[i].Name;
                if (customPropertyList[i].SysProperty.Keys != null || customPropertyList[i].SysProperty.Parentpropertyvalue != null)
                {
                    if (customPropertyList[i].Value.Contains(":-10"))
                    {
                        string text3 = inputCustomCpv;
                        inputCustomCpv = string.Concat(new string[]
                        {
                    text3,
                    customPropertyList[i].Value,
                    ":",
                    text2,
                    ";"
                        });
                    }
                    else
                    {
                        string text4 = (customPropertyList[i].SysProperty.Parentpropertyvalue == null) ? "" : customPropertyList[i].SysProperty.Parentpropertyvalue;
                        string text5 = customPropertyList[i].SysProperty.Keys;
                        if (text5.IndexOf(":") == -1 && !string.IsNullOrEmpty(text4))
                        {
                            text5 = text4;
                        }
                        string text6 = text;
                        text = string.Concat(new string[]
                        {
                    text6,
                    text5,
                    ":",
                    text2,
                    ";"
                        });
                    }
                }
            }
            if (list != null && list.Count > 0)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    DataConvert.ToString(list[j].Id);
                    string text7 = (list[j].Name == null) ? "" : list[j].Name;
                    if (list[j].SysProperty.Keys != null || list[j].SysProperty.Parentpropertyvalue != null)
                    {
                        string text8 = (list[j].SysProperty.Parentpropertyvalue == null) ? "" : list[j].SysProperty.Parentpropertyvalue;
                        string text9 = list[j].SysProperty.Keys;
                        if (text9.IndexOf(":") == -1 && !string.IsNullOrEmpty(text8))
                        {
                            text9 = text8;
                        }
                        string text10 = cpvmenoStr;
                        cpvmenoStr = string.Concat(new string[]
                        {
                    text10,
                    text9,
                    ":",
                    text7,
                    ";"
                        });
                    }
                }
            }
            text = text.Trim(new char[]
            {
        ';'
            });
            cpvmenoStr = cpvmenoStr.Trim(new char[]
            {
        ';'
            });
            inputCustomCpv = inputCustomCpv.Trim(new char[]
            {
        ';'
            });
            return text;
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
        private bool IsOldSizeAndNewTaobao(Dictionary<string, string> dicCustomProperty, string sortAllName, string propertyName)
        {
            bool result = false;
            if (dicCustomProperty != null && dicCustomProperty.Count > 0 && !string.IsNullOrEmpty(sortAllName) && !string.IsNullOrEmpty(propertyName) && dicCustomProperty.ContainsKey(sortAllName) && dicCustomProperty[sortAllName].Contains(propertyName))
            {
                result = true;
            }
            return result;
        }
        private void HandleProperty(string itemId, ProductItem productItem)
        {
            string text = "";
            string text2 = string.Empty;
            string text3 = "";
            string features = "";
            IList<Sp_property> propertyListByItemId = DataHelper.GetPropertyListByItemId(DataConvert.ToInt(itemId));
            foreach (Sp_property prop_item in propertyListByItemId)
            {
                prop_item.SysProperty = DataHelper.GetPropertyById(prop_item.Propertyid);
            }
            Dictionary<string, Sp_property> dictionary = new Dictionary<string, Sp_property>();
            IList<Sp_property> list = new List<Sp_property>();
            if (propertyListByItemId != null && propertyListByItemId.Count > 0)
            {
                for (int i = 0; i < propertyListByItemId.Count; i++)
                {
                    if (propertyListByItemId[i].SysProperty != null && i < propertyListByItemId.Count - 1 && propertyListByItemId[i].SysProperty.Parentname == propertyListByItemId[i + 1].Name && propertyListByItemId[i + 1].Name == "品牌")
                    {
                        Sp_property value = propertyListByItemId[i];
                        propertyListByItemId[i] = propertyListByItemId[i + 1];
                        propertyListByItemId[i + 1] = value;
                        break;
                    }
                }
            }
            Dictionary<string, string> dicCustomProperty = this.GetDicCustomProperty();
            Sys_sysSort sortBySysIdAndKeys = DataHelper.GetSortBySysIdAndKeys(1, productItem.SortKey);
            string sortAllName = string.Empty;
            if (sortBySysIdAndKeys != null && !string.IsNullOrEmpty(sortBySysIdAndKeys.Name))
            {
                sortAllName = sortBySysIdAndKeys.Path + ">>" + sortBySysIdAndKeys.Name;
            }
            foreach (Sp_property current in propertyListByItemId)
            {
                if (current.SysProperty != null && current.SysProperty.Keys != null)
                {
                    string text4 = DataConvert.ToString(current.SysProperty.Id);
                    string a = DataConvert.ToString(current.SysProperty.Valuetype);
                    string text5 = current.SysProperty.Keys;
                    string a2 = DataConvert.ToString(current.SysProperty.Parentid);
                    string o = DataConvert.ToString(current.SysProperty.Levels);
                    string text6 = (current.SysProperty.Parentpropertyvalue == null) ? "" : current.SysProperty.Parentpropertyvalue;
                    if ((a != "2" && a != "3") || (a == "2" && current.Issellpro == 1 && this.IsOldSizeAndNewTaobao(dicCustomProperty, sortAllName, current.SysProperty.Name)))
                    {
                        text = text + current.Value + ";";
                    }
                    else
                    {
                        if (current.Issellpro == 1)
                        {
                            continue;
                        }
                        string text7 = current.Value;
                        if (DataConvert.ToInt(o) >= 2)
                        {
                            string text8 = "";
                            if (a2 != "0" && text5.IndexOf(":") == -1 && !string.IsNullOrEmpty(text6))
                            {
                                text5 = text6;
                            }
                            IList<Sys_sysProperty> propertyAllTopLevelProperty = DataHelper.GetPropertyAllTopLevelProperty(text4);
                            for (int j = propertyAllTopLevelProperty.Count - 1; j >= 0; j--)
                            {
                                Sys_sysProperty sys_sysProperty = propertyAllTopLevelProperty[j];
                                string key = DataConvert.ToString(sys_sysProperty.Id);
                                if (dictionary.ContainsKey(key))
                                {
                                    Sys_sysProperty sysProperty = dictionary[key].SysProperty;
                                    Sp_property sp_property = dictionary[key];
                                    if (sysProperty.Valuetype == 0)
                                    {
                                        if (sysProperty.Parentid == 0)
                                        {
                                            text5 = sysProperty.Keys;
                                            if (DataConvert.ToInt(o) > 2 || (!(sp_property.Name != "其他") && !(sp_property.Name != "其它")))
                                            {
                                                text8 = text8 + sp_property.Name + ";";
                                            }
                                        }
                                        else
                                        {
                                            string name = sp_property.Name;
                                            if (name == "其他" || name == "其它")
                                            {
                                                text8 = text8 + sysProperty.Name + ";";
                                            }
                                            else
                                            {
                                                string text9 = text8;
                                                text8 = string.Concat(new string[]
                                                {
                                            text9,
                                            sysProperty.Name,
                                            ";",
                                            name,
                                            ";"
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                            if (current.SysProperty.Parentpropertyvalue == "20000:-1" && current.SysProperty.Keys != "20000")
                            {
                                text7 = current.SysProperty.Name + ";" + text7;
                            }
                            text7 = text8 + text7;
                            list.Add(current);
                            if (list.Count >= 2 && list[list.Count - 1].SysProperty.Parentid == list[list.Count - 2].SysProperty.Parentid)
                            {
                                text3 = text3.TrimEnd(new char[]
                                {
                            ','
                                });
                                text3 = text3 + ";" + text7 + ",";
                            }
                            else
                            {
                                string[] array = text5.Split(new char[]
                                {
                            ':'
                                }, StringSplitOptions.RemoveEmptyEntries);
                                if (array.Length > 1)
                                {
                                    text5 = array[0];
                                }
                                if (!string.IsNullOrEmpty(text5) && !text2.Contains(text5 + ","))
                                {
                                    text2 = text2 + text5 + ",";
                                    text3 = text3 + text7 + ",";
                                }
                            }
                        }
                        else
                        {
                            string[] array2 = text5.Split(new char[]
                            {
                        ':'
                            }, StringSplitOptions.RemoveEmptyEntries);
                            if (array2.Length > 1)
                            {
                                text5 = array2[0];
                            }
                            if (!string.IsNullOrEmpty(text5))
                            {
                                text2 = text2 + text5 + ",";
                            }
                            text3 = text3 + text7 + ",";
                        }
                    }
                    if (!dictionary.ContainsKey(text4))
                    {
                        dictionary.Add(text4, current);
                    }
                }
            }
            text = text.Trim(new char[]
            {
        ','
            });
            text2 = text2.TrimEnd(new char[]
            {
        ','
            });
            text3 = text3.TrimEnd(new char[]
            {
        ','
            });
            Sys_sysSort sys_sysSort = DataHelper.GetSortBySysIdAndKeys(1, productItem.SortKey);
            if (sys_sysSort == null && !string.IsNullOrEmpty(productItem.SortKey))
            {
                sys_sysSort = DataHelper.GetSortById(sys_sysSort.Id);
            }
            if (sys_sysSort != null && sys_sysSort.IsNewSellPro)
            {
                Sys_sizeDetail sys_sizeDetail = new Sys_sizeDetail();
                foreach (Sp_property current2 in propertyListByItemId)
                {
                    sys_sizeDetail = DataHelper.GetSizeDetailBySizeValue(current2.Value, sys_sysSort.SizeGroupType, productItem.SysId);
                    if (sys_sizeDetail != null && sys_sizeDetail.SizeName != "均码")
                    {
                        break;
                    }
                }
                if (sys_sizeDetail != null)
                {
                    Sys_sizeGroup sizeGroupByGroupOnlineID = DataHelper.GetSizeGroupByGroupOnlineID(sys_sysSort.SizeGroupType, sys_sizeDetail.GroupOnlineID, productItem.SysId);
                    if (sizeGroupByGroupOnlineID != null)
                    {
                        if (sizeGroupByGroupOnlineID.GroupName == "其它")
                        {
                            features = string.Concat(new string[]
                            {
                        "mysize_tp:-1;sizeGroupId:",
                        sys_sizeDetail.GroupOnlineID,
                        ";sizeGroupName:",
                        sizeGroupByGroupOnlineID.GroupName,
                        ";sizeGroupType:no_group1"
                            });
                        }
                        else
                        {
                            features = string.Concat(new string[]
                            {
                        "mysize_tp:-1;sizeGroupId:",
                        sys_sizeDetail.GroupOnlineID,
                        ";sizeGroupName:",
                        sizeGroupByGroupOnlineID.GroupName,
                        ";sizeGroupType:",
                        sys_sysSort.SizeGroupType
                            });
                        }
                    }
                }
                productItem.Features = features;
            }
            productItem.PropertyValue = text;
            productItem.UserInputPropIDs = text2;
            productItem.UserInputPropValues = text3;
        }
        private List<string> GetOrderProp(Dictionary<int, IList<Sp_property>> dicProp, int itemId)
        {
            Sp_sysSort spSysSort = DataHelper.GetSpSysSort(itemId);
            DataTable dataTable = new DataTable();
            if (spSysSort != null)
            {
                dataTable = DataHelper.GetPropertyValueDtBySortId(spSysSort.Syssortid);
            }
            List<List<string>> list = new List<List<string>>();
            if (dicProp != null && dicProp.Count > 0)
            {
                foreach (KeyValuePair<int, IList<Sp_property>> current in dicProp)
                {
                    List<string> list2 = new List<string>();
                    foreach (Sp_property current2 in current.Value)
                    {
                        if (!string.IsNullOrEmpty(current2.Value) && current2.Value.Split(new char[]
                        {
                    ':'
                        }).Length >= 2)
                        {
                            if (current2.Value.Split(new char[]
                            {
                        ':'
                            })[1].Contains("-"))
                            {
                                list2.Add(current2.Propertyid + ":" + current2.Value.Split(new char[]
                                {
                            ':'
                                })[1]);
                            }
                            else
                            {
                                if (dataTable != null && dataTable.Rows.Count > 0)
                                {
                                    DataRow[] array = dataTable.Select("value='" + current2.Value + "'");
                                    if (array != null && array.Length > 0)
                                    {
                                        list2.Add(current2.Propertyid + ":" + DataConvert.ToString(array[0]["id"]));
                                    }
                                }
                            }
                        }
                    }
                    list.Add(list2);
                }
            }
            return this.Exchange(list);
        }
        private IList<Sp_sellProperty> OrderProperty(IList<Sp_sellProperty> sellPropertyList, int itemId)
        {
            IList<Sp_sellProperty> list = new List<Sp_sellProperty>();
            if (sellPropertyList != null && sellPropertyList.Count > 0)
            {
                IList<Sp_property> propertyListByItemId = DataHelper.GetPropertyListByItemId(itemId);
                Dictionary<int, IList<Sp_property>> dictionary = new Dictionary<int, IList<Sp_property>>();
                if (propertyListByItemId != null && propertyListByItemId.Count > 0)
                {
                    foreach (Sp_property current in propertyListByItemId)
                    {
                        if (current.Issellpro == 1 && current.SysProperty != null && current.SysProperty.Levels == 1)
                        {
                            if (dictionary.ContainsKey(current.Propertyid))
                            {
                                dictionary[current.Propertyid].Add(current);
                            }
                            else
                            {
                                dictionary[current.Propertyid] = new List<Sp_property>();
                                dictionary[current.Propertyid].Add(current);
                            }
                        }
                    }
                }
                List<string> list2 = new List<string>();
                if (dictionary != null && dictionary.Count > 0 && itemId > 0)
                {
                    list2 = this.GetOrderProp(dictionary, itemId);
                }
                foreach (string current2 in list2)
                {
                    foreach (Sp_sellProperty current3 in sellPropertyList)
                    {
                        string[] array = current2.Split(new char[]
                        {
                    '|'
                        });
                        bool flag = false;
                        if (array != null && array.Length > 0)
                        {
                            string[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                string str = array2[i];
                                if (!current3.Sellproinfos.Contains(str + ":"))
                                {
                                    flag = false;
                                    break;
                                }
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            list.Add(current3);
                        }
                    }
                }
                if (list == null || list.Count <= 0)
                {
                    list = sellPropertyList;
                }
            }
            return list;
        }
        private string HandleSellProperty(int id, out string skuBarcode, ProductItem productItem)
        {
            skuBarcode = string.Empty;
            int sysId = 1;
            Sys_sysSort sys_sysSort = new Sys_sysSort();
            if (!string.IsNullOrEmpty(productItem.SortKey))
            {
                sys_sysSort = DataHelper.GetSortBySysIdAndKeys(1, productItem.SortKey);
            }
            if (sys_sysSort == null)
            {
                sys_sysSort = DataHelper.GetSortById(productItem.SysSortId);
            }
            if (sys_sysSort == null)
            {
                Log.WriteLog("导出商品时，获取类目信息失败，" + Environment.StackTrace);
                return string.Empty;
            }
            IList<Sp_sellProperty> list = DataHelper.GetSellProperty(id, sysId);
            if (list == null || list.Count == 0)
            {
                return string.Empty;
            }
            List<string> list2 = new List<string>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list2.Contains(list[i].Sellproinfos))
                {
                    list.Remove(list[i]);
                }
                else
                {
                    list2.Add(list[i].Sellproinfos);
                }
            }
            string text = "";
            skuBarcode = string.Empty;
            list = this.OrderProperty(list, id);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (Sp_sellProperty current in list)
            {
                string text2 = (current.Sellproinfos == null) ? "" : current.Sellproinfos;
                string text3 = (current.Price == 0m) ? "" : DataConvert.ToString(current.Price);
                string text4 = (current.Nums == 0) ? "" : DataConvert.ToString(current.Nums);
                string text5 = (current.Code == null) ? "" : DataConvert.ToString(current.Code).Replace(" ", "").Replace("\u3000", "").Replace("/", "").Replace("\\", "").Replace("&", "");
                string text6 = text;
                text = string.Concat(new string[]
                {
            text6,
            text3,
            ":",
            text4,
            ":",
            text5,
            ":"
                });
                if (!string.IsNullOrEmpty(text2))
                {
                    string[] array = text2.Split(new char[]
                    {
                '|'
                    });
                    for (int j = 0; j < array.Length; j++)
                    {
                        string[] array2 = array[j].Split(new char[]
                        {
                    ':'
                        });
                        if (array2 != null && array2.Length > 1)
                        {
                            if (sys_sysSort != null && sys_sysSort.IsNewSellPro)
                            {
                                Sys_sysPropertyValue propertyValueById = DataHelper.GetPropertyValueById(Convert.ToInt32(array2[1]));
                                if (propertyValueById != null)
                                {
                                    text = text + propertyValueById.Value + ";";
                                }
                                else
                                {
                                    IList<Sys_sysPropertyValue> propertyValuesByPropertyId = DataHelper.GetPropertyValuesByPropertyId(Convert.ToInt32(array2[0]));
                                    if (propertyValuesByPropertyId != null && propertyValuesByPropertyId.Count > 0)
                                    {
                                        string[] array3 = propertyValuesByPropertyId[0].Value.Split(new char[]
                                        {
                                    ':'
                                        });
                                        string text7 = text;
                                        text = string.Concat(new string[]
                                        {
                                    text7,
                                    array3[0],
                                    ":",
                                    array2[1],
                                    ";"
                                        });
                                    }
                                    else
                                    {
                                        if (array2.Length >= 2)
                                        {
                                            if (dictionary != null && !dictionary.ContainsKey(array2[0]))
                                            {
                                                IList<Sys_sysProperty> propertyAllTopLevelProperty = DataHelper.GetPropertyAllTopLevelProperty(array2[0]);
                                                if (propertyAllTopLevelProperty != null && propertyAllTopLevelProperty.Count == 1)
                                                {
                                                    dictionary[array2[0]] = propertyAllTopLevelProperty[0].Keys;
                                                }
                                            }
                                            string str = string.Empty;
                                            if (dictionary != null && dictionary.Count > 0 && dictionary.ContainsKey(array2[0]) && !string.IsNullOrEmpty(dictionary[array2[0]]))
                                            {
                                                str = dictionary[array2[0]] + ":" + array2[1] + ";";
                                            }
                                            else
                                            {
                                                str = array2[0] + ":" + array2[1] + ";";
                                            }
                                            text += str;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Sys_sysPropertyValue propertyValueById2 = DataHelper.GetPropertyValueById(DataConvert.ToInt(array2[1]));
                                string text8 = (propertyValueById2 != null) ? propertyValueById2.Value : string.Empty;
                                if (string.IsNullOrEmpty(text8) && array2.Length >= 2)
                                {
                                    text8 = array2[0] + ":" + array2[1];
                                }
                                text = text + text8 + ";";
                            }
                        }
                    }
                }
                skuBarcode = skuBarcode + current.Barcode + ";";
            }
            return text;
        }
        private void HandleShipWay(ProductItem productItem)
        {
            string actualShipSlow = "0";
            string actualShipFast = "0";
            string actualShipEMS = "0";
            string actualShipTpl = "0";
            string text = DataConvert.ToString(productItem.ShipSlow);
            string text2 = DataConvert.ToString(productItem.ShipFast);
            string text3 = DataConvert.ToString(productItem.ShipEMS);
            string text4 = productItem.ShipWay;
            string useShipTpl = productItem.UseShipTpl;
            string text5 = DataConvert.ToString(productItem.ShipTplId);
            if (string.IsNullOrEmpty(text4))
            {
                return;
            }
            string a;
            if ((a = text4.Trim()) != null)
            {
                if (!(a == "1"))
                {
                    if (a == "2")
                    {
                        text4 = "1";
                    }
                }
                else
                {
                    if (!DataConvert.ToBoolean(useShipTpl))
                    {
                        actualShipSlow = text;
                        actualShipFast = text2;
                        actualShipEMS = text3;
                    }
                    else
                    {
                        Sys_shopShip shopShipById = DataHelper.GetShopShipById(DataConvert.ToInt(text5));
                        text5 = ((shopShipById != null) ? shopShipById.Keys : string.Empty);
                        actualShipTpl = text5;
                    }
                    text4 = "2";
                }
            }
            productItem.ActualShipSlow = actualShipSlow;
            productItem.ActualShipFast = actualShipFast;
            productItem.ActualShipEMS = actualShipEMS;
            productItem.ActualShipTpl = actualShipTpl;
            productItem.ShipWay = text4;
        }
        private void HandleOnSell(string onSell, string onSellDate, string onSellHour, string onSellMin, ProductItem productItem)
        {
            if (onSell == "0")
            {
                onSell = "1";
            }
            else
            {
                if (onSell == "1")
                {
                    onSell = "2";
                    string text = string.Concat(new string[]
                    {
                DataConvert.ToDateTime(onSellDate).ToString("yyyy-MM-dd"),
                " ",
                onSellHour,
                ":",
                onSellMin,
                ":00"
                    });
                    text = DataConvert.ToDateTime(text).ToString("yyyy-MM-dd HH:mm:ss").Replace("/", "-");
                    productItem.ActualOnSellDate = text;
                }
                else
                {
                    if (onSell == "2")
                    {
                        onSell = "2";
                    }
                }
            }
            productItem.OnSell = onSell;
        }
        private string GetNewOrOld(string newOrOld)
        {
            string result = "1";
            if (!string.IsNullOrEmpty(newOrOld))
            {
                string text = string.Empty;
                string enumCode = "xjcd";
                IList<Sys_sysEnumItem> enumItemByCode = DataHelper.GetEnumItemByCode(new Sys_sysEnumItem() { Enumtypecode = enumCode, Sysid = 1 });
                foreach (Sys_sysEnumItem current in enumItemByCode)
                {
                    if (current.Value.Equals(newOrOld.Trim()))
                    {
                        text = current.Enumitemname;
                        string a;
                        if ((a = text) != null)
                        {
                            if (a == "全新")
                            {
                                result = "1";
                                continue;
                            }
                            if (a == "二手")
                            {
                                result = "3";
                                continue;
                            }
                            if (a == "个人闲置")
                            {
                                result = "2";
                                continue;
                            }
                        }
                        result = "1";
                    }
                }
            }
            return result;
        }
        public string ConvertToSaveCellCommon(string cell)
        {
            if (string.IsNullOrEmpty(cell))
            {
                return "\"" + string.Empty + "\"";
            }
            cell = cell.Replace("\"", "");
            return "\"" + cell + "\"";
        }
        protected string ConvertToSaveCell(string cell)
        {
            if (string.IsNullOrEmpty(cell))
            {
                return "\"" + string.Empty + "\"";
            }
            cell = cell.Replace("\"", "\"\"");
            cell = cell.Replace("\u00a0", "&nbsp;");
            cell = cell.Replace("\r\n", "");
            return "\"" + cell + "\"";
        }
        private string ClearVedio(string content)
        {
            Regex regex = new Regex("(?is)<EMBED[^<>]*video.taobao[^<>]*>\\s*?</EMBED>|(?is)<EMBED[^<>]*video.taobao[^<>]*>");
            return regex.Replace(content, string.Empty);
        }
        protected string ConvertCell(string cellContent)
        {
            if (!string.IsNullOrEmpty(cellContent))
            {
                cellContent = cellContent.Trim();
                cellContent = cellContent.Replace("\"", "\"\"");
                return "\"" + cellContent + "\"";
            }
            return string.Empty;
        }
        public void WriteDicToFile(string fileName, List<string[]> dicList)
        {
            int num = 1;
            StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.Unicode);
            int num2 = 0;
            foreach (string[] current in dicList)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    if (num2 != 0 && i != 0 && num != 1 && i != 2)
                    {
                        current[i] = this.ConvertCell(current[i]);
                    }
                }
                string value = string.Empty;
                value = string.Join("\t", current);
                streamWriter.WriteLine(value);
                num2++;
            }
            streamWriter.Flush();
            streamWriter.Close();
        }
        private List<string> Exchange(List<List<string>> strList)
        {
            if (strList != null && strList.Count > 0)
            {
                int count = strList.Count;
                if (count >= 2)
                {
                    int count2 = strList[0].Count;
                    int count3 = strList[1].Count;
                    List<string> list = new List<string>();
                    int num = 0;
                    for (int i = 0; i < count2; i++)
                    {
                        for (int j = 0; j < count3; j++)
                        {
                            list.Add(strList[0][i] + "|" + strList[1][j]);
                            num++;
                        }
                    }
                    List<List<string>> list2 = new List<List<string>>();
                    list2.Add(list);
                    for (int k = 2; k < strList.Count; k++)
                    {
                        list2.Add(strList[k]);
                    }
                    return Exchange(list2);
                }
                return strList[0];
            }
            return null;
        }

    }
}
