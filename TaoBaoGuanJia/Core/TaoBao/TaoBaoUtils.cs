
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;
using Top.Api.Domain;


namespace TaoBaoGuanJia.Core.TaoBao
{
   public class TaoBaoUtils
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

        internal string GetOperateTypes(string detailUrl)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(detailUrl))
            {
                string content = Utils.SendWebRequest(detailUrl);
                string operateTypes = GetNumber3c(content);
                string stringToEscape = "{\"bathrobe_field_tag_image_1\" : \"\", \"bathrobe_field_tag_image_2\" : \"\",\"var_item_board_inspection_report\" : \"\",\"var_item_glove_inspection_report_1\" : \"\",\"var_item_light_inspection_report_2\" : \"\",\"var_org_auth_tri_c_code\" : \"" + operateTypes + "\",\"var_tri_c_cer_image\" : \"\", \"var_tri_c_cer_image_2\" : \"\" }";
                result = Uri.EscapeDataString(stringToEscape);
            }
            return result;
        }

        internal string GetFoodParame(Item item)
        {
           // StringBuilder sb = new StringBuilder();
            string text = string.Empty;
            if (item != null && item.FoodSecurity != null)
            {
                FoodSecurity foodSecurityByItemId = item.FoodSecurity;
               
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Contact))
                {
                    text = text + "contact:" + foodSecurityByItemId.Contact + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.DesignCode))
                {
                    text = text + "design_code:" + foodSecurityByItemId.DesignCode + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Factory))
                {
                    text = text + "factory:" + foodSecurityByItemId.Factory + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.FactorySite))
                {
                    text = text + "factory_site:" + foodSecurityByItemId.FactorySite + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.FoodAdditive))
                {
                    text = text + "food_additive:" + foodSecurityByItemId.FoodAdditive + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Mix))
                {
                    text = text + "mix:" + foodSecurityByItemId.Mix + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Period))
                {
                    text = text + "period:" + foodSecurityByItemId.Period + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.PlanStorage))
                {
                    text = text + "plan_storage:" + foodSecurityByItemId.PlanStorage + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.PrdLicenseNo))
                {
                    text = text + "prd_license_no:" + foodSecurityByItemId.PrdLicenseNo + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.HealthProductNo))
                {
                    text = text + "health_product_no:" + foodSecurityByItemId.HealthProductNo + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString((object)foodSecurityByItemId.ProductDateEnd)))
                {
                    text = text + "product_date_end:" + foodSecurityByItemId.ProductDateEnd + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString((object)foodSecurityByItemId.ProductDateStart)))
                {
                    text = text + "product_date_start:" + foodSecurityByItemId.ProductDateStart + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString((object)foodSecurityByItemId.StockDateEnd)))
                {
                    text = text + "stock_date_end:" + foodSecurityByItemId.StockDateEnd + ";";
                }
                if (!string.IsNullOrEmpty(DataConvert.ToString((object)foodSecurityByItemId.StockDateStart)))
                {
                    text = text + "stock_date_start:" + foodSecurityByItemId.StockDateStart + ";";
                }
                if (!string.IsNullOrEmpty(foodSecurityByItemId.Supplier))
                {
                    text = text + "supplier:" + foodSecurityByItemId.Supplier + ";";
                }
                if (!string.IsNullOrEmpty(text))
                {
                    text = text.TrimEnd(';');
                }
      
            }
            return text;
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

        public string GetItzemStuffStatus(string stuffStatus)
        {
            switch (stuffStatus)
            {
                case "new":
                    return "5";
                case "unused":
                    return "8";
                case "second":
                    return "6";
                default:
                    return string.Empty;
            }
        }

        public string GetCode(string propsName,string outerId) {
           
           string text = propsName;
          
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
                 
            }
            else
            {
                text = outerId;
            }
            return text;
        }
        public string GetNumber3c(string downloadResponseContent)
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

        public string GetItemSellPoint(string responseContent)
        {
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            string result = string.Empty;
            string sysConfig = "(?is)<p\\s*?class=\"tb - subtitle\">\\s*?(?<ItemSellPoint>.*?)</p>|<div[^>]*?class=\"tb - detail - hd\"[^>]*?>[^<>]*?<h1[^>]*?>[^>]*?</h1>[^<>]*?<p>(?<ItemSellPoint>.*?)</p>[^<>]*?</div>";
            if (!string.IsNullOrEmpty(sysConfig))
            {
                Regex regex = new Regex(sysConfig);
                Match match = regex.Match(responseContent);
                if (match != null && match.Groups["ItemSellPoint"].Success)
                {
                    result = match.Groups["ItemSellPoint"].Value;
                }
            }
            return result;
        }

        public  IList<Sp_property> SetspPropertyList(Item item,  int sortId, out Dictionary<string, SellProInfo> dicProValueAndSellProInfos, out string code)
        {
            IList<Sys_sysProperty> sysSysPropertyList= DataHelper.GetPropertyBySortId(sortId);
            DataTable dtPropertyValue= DataHelper.GetPropertyValueDtBySortId(sortId);
            bool flag = false;// DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", base.ToolCode.ToUpper().ToString(), "CItemPicShieldCheck", "false"));
            Dictionary<string, SellNewProInfo> sourceProValueAndNewProValue = new Dictionary<string, SellNewProInfo>();
            bool flag2 = false;// Convert.ToBoolean(ToolServer.get_ConfigData().GetUserConfig("AppConfig", "PicSet", "SellProperyPic", "false"));
            code = string.Empty;
            IList<Sp_property> list = new List<Sp_property>();
            IList<Sys_sysProperty> propertyBySortId = DataHelper.GetPropertyBySortId(sortId);
            Dictionary<string, Sys_sysProperty> dictionary = new Dictionary<string, Sys_sysProperty>();
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            List<string> list2 = new List<string>();
            if (sysSysPropertyList != null)
            {
                foreach (Sys_sysProperty sysSysProperty in sysSysPropertyList)
                {
                    if (sysSysProperty.Valuetype == 2 && sysSysProperty.Parentid == 0)
                    {
                        dictionary[sysSysProperty.Keys] = sysSysProperty;
                    }
                    if (sysSysProperty.Parentid == 0)
                    {
                        dictionary2[sysSysProperty.Keys] = DataConvert.ToString((object)sysSysProperty.Id);
                    }
                    if (sysSysProperty.Issellpro)
                    {
                        list2.Add(DataConvert.ToString((object)sysSysProperty.Id));
                    }
                }
            }
            Dictionary<string, DataRow> dictionary3 = new Dictionary<string, DataRow>();
            foreach (DataRow row in dtPropertyValue.Rows)
            {
                dictionary3[DataConvert.ToString(row["Value"])] = row;
            }
            dicProValueAndSellProInfos = new Dictionary<string, SellProInfo>();
            SellProInfo sellProInfo = null;
            int num = 0;
            if (item == null)
            {
                return list;
            }
            Dictionary<string, PropertyAliasViewEntity> dictionary4 = new Dictionary<string, PropertyAliasViewEntity>();
            PropertyAliasViewEntity propertyAliasViewEntity = new PropertyAliasViewEntity();
            string empty = string.Empty;
            string empty2 = string.Empty;
            if (item.PropImgs != null && item.PropImgs.Count > 0)
            {
                foreach (PropImg propImg in item.PropImgs)
                {
                    propertyAliasViewEntity = new PropertyAliasViewEntity();
                    empty = (propertyAliasViewEntity.Value = propImg.Properties);
                    propertyAliasViewEntity.ImageUrl = propImg.Url;
                    dictionary4[empty] = propertyAliasViewEntity;
                }
            }
            if (!string.IsNullOrEmpty(item.PropertyAlias))
            {
                Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
                int num2 = 0;
                string[] array = item.PropertyAlias.Split(new string[1]
                {
                    ";"
                }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string text in array)
                {
                    num = text.LastIndexOf(":");
                    if (num > 0)
                    {
                        empty = text.Substring(0, num);
                        empty2 = text.Substring(num + 1);
                        empty2 = SubStringToMaxByteLength(empty2, 30);
                        propertyAliasViewEntity = ((!dictionary4.ContainsKey(empty)) ? new PropertyAliasViewEntity() : dictionary4[empty]);
                        propertyAliasViewEntity.Value = text.Substring(0, num);
                        propertyAliasViewEntity.Alias = HttpUtility.UrlDecode(text.Substring(num + 1));
                        propertyAliasViewEntity.Alias = SubStringToMaxByteLength(propertyAliasViewEntity.Alias, 30);
                        if (!string.IsNullOrEmpty(propertyAliasViewEntity.Alias))
                        {
                            if (dictionary5.ContainsKey(propertyAliasViewEntity.Alias))
                            {
                                num2++;
                                string key = propertyAliasViewEntity.Alias = propertyAliasViewEntity.Alias.Substring(0, propertyAliasViewEntity.Alias.Length - 1) + num2;
                                dictionary4[propertyAliasViewEntity.Value] = propertyAliasViewEntity;
                                dictionary5[key] = propertyAliasViewEntity.Value;
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
            string empty7 = string.Empty;
            string empty3 = string.Empty;
            string proKey = string.Empty;
            string empty4 = string.Empty;
            string empty5 = string.Empty;
            Sp_property sp_property = null;
            Dictionary<string, string> inputPidAndStr = GetInputPidAndStr(item);
            ImageOperator imageOperator = new ImageOperator();
            List<PropsViewEntity> lstPropsDTO = GetLstPropsDTO(item.PropsName);
            Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
            Dictionary<string, string> dictionary7 = new Dictionary<string, string>();
            List<PropsViewEntity>.Enumerator enumerator4 = lstPropsDTO.GetEnumerator();
            try
            {
                while (enumerator4.MoveNext())
                {
                    PropsViewEntity current3 = enumerator4.Current;
                    dictionary6[current3.PropertyName] = current3.PropertyValueName;
                    dictionary7[current3.PropertyValue] = current3.PropertyValueName;
                }
            }
            finally
            {
                ((IDisposable)enumerator4).Dispose();
            }
            foreach (KeyValuePair<string, PropertyAliasViewEntity> item2 in dictionary4)
            {
                if (item2.Value != null && string.IsNullOrEmpty(item2.Value.Alias) && dictionary7.ContainsKey(item2.Key))
                {
                    item2.Value.Alias = dictionary7[item2.Key];
                    byte[] bytes = Encoding.Default.GetBytes(item2.Value.Alias);
                    int num3 = bytes.Length;
                    if (num3 > 30)
                    {
                        string text3 = Encoding.Default.GetString(bytes, 0, 30);
                        if (text3.EndsWith("?"))
                        {
                            text3 = text3.TrimEnd('?');
                        }
                        item2.Value.Alias = text3;
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
            enumerator4 = list4.GetEnumerator();
            try
            {
                while (enumerator4.MoveNext())
                {
                    PropsViewEntity current5 = enumerator4.Current;
                    lstPropsDTO.Remove(current5);
                }
            }
            finally
            {
                ((IDisposable)enumerator4).Dispose();
            }
            new Dictionary<string, string>();
            for (int l = 0; l < lstPropsDTO.Count; l++)
            {
                byte[] bytes2 = Encoding.Default.GetBytes(lstPropsDTO[l].PropertyValueName);
                int num4 = bytes2.Length;
                if (num4 > 30)
                {
                    string text4 = Encoding.Default.GetString(bytes2, 0, 30);
                    if (text4.EndsWith("?"))
                    {
                        text4 = text4.TrimEnd('?');
                    }
                    if (inputPidAndStr.ContainsKey(text4))
                    {
                        text4 += l;
                    }
                    else
                    {
                        inputPidAndStr[text4] = text4;
                    }
                    lstPropsDTO[l].PropertyValueName = text4;
                }
            }
            Dictionary<string, int> dictionary9 = new Dictionary<string, int>();
            enumerator4 = lstPropsDTO.GetEnumerator();
            long numIid;
            try
            {
                while (enumerator4.MoveNext())
                {
                    PropsViewEntity current6 = enumerator4.Current;
                    DataRow dataRow2 = null;
                    sp_property = new Sp_property();
                    num = current6.PropertyValue.IndexOf(":");
                    if (num != 0)
                    {
                        proKey = current6.PropertyValue.Remove(num);
                        if (dictionary.ContainsKey(proKey))
                        {
                            SellNewProInfo sellNewProInfo = null;
                            empty3 = DataConvert.ToString((object)dictionary[proKey].Id);
                            Sys_sysProperty val = dictionary[proKey];
                            if (val != null && val.Name != null && (val.Name.EndsWith("货号") || val.Name.EndsWith("款号")))
                            {
                                code = current6.PropertyValueName;
                            }
                            sp_property.Propertyid = DataConvert.ToInt((object)empty3);
                            sp_property.Issellpro = (list2.Contains(empty3) ? 1 : 0);
                            if (dictionary[proKey].Valuetype == 2 && dictionary[proKey].Issellpro)
                            {
                                sellNewProInfo = new SellNewProInfo();
                                string empty6 = string.Empty;
                                if (dictionary9 == null || dictionary9.Count == 0)
                                {
                                    empty6 = proKey + ":-1001";
                                    dictionary9.Add(proKey, -1001);
                                    sp_property.Name = current6.PropertyValueName;
                                    sp_property.Value = empty6;
                                }
                                else if (dictionary9.ContainsKey(proKey))
                                {
                                    Dictionary<string, int> dictionary10;
                                    string key2;
                                    (dictionary10 = dictionary9)[key2 = proKey] = dictionary10[key2] + -1;
                                    empty6 = proKey + ":" + dictionary9[proKey];
                                    sp_property.Name = current6.PropertyValueName;
                                    sp_property.Value = empty6;
                                }
                                else
                                {
                                    empty6 = proKey + ":-1001";
                                    dictionary9.Add(proKey, -1001);
                                    sp_property.Name = current6.PropertyValueName;
                                    sp_property.Value = empty6;
                                }
                                sellNewProInfo.Value = empty6;
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
                            empty4 = current6.PropertyValue;
                            if (dictionary3.ContainsKey(empty4))
                            {
                                dataRow2 = dictionary3[empty4];
                            }
                            else
                            {
                                if (current6.PropertyValueName.IndexOf("'") >= 0)
                                {
                                    current6.PropertyValueName = current6.PropertyValueName.Replace("'", "''");
                                }
                                DataRow[] array2 = dtPropertyValue.Select("name='" + current6.PropertyValueName + "' and value like '%" + proKey + "%'");
                                if (array2 != null && array2.Length > 0)
                                {
                                    dataRow2 = array2[0];
                                }
                                if (dataRow2 == null)
                                {
                                    empty4 = proKey + ":-1";
                                    if (dictionary3.ContainsKey(empty4))
                                    {
                                        dataRow2 = dictionary3[empty4];
                                    }
                                }
                            }
                            if (dataRow2 == null)
                            {
                                list3.Add(current6);
                            }
                            else
                            {
                                empty3 = DataConvert.ToString(dataRow2["Propertyid"]);
                                sp_property.Propertyid = DataConvert.ToInt((object)empty3);
                                sp_property.Value = DataConvert.ToString(dataRow2["Value"]);
                                sp_property.Name = DataConvert.ToString(dataRow2["Name"]);
                                sp_property.Issellpro = (list2.Contains(empty3) ? 1 : 0);
                                string propertyValueName3 = current6.PropertyValueName;
                                list.Add(sp_property);
                                dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                                if (dictionary4.ContainsKey(empty4))
                                {
                                    propertyAliasViewEntity = dictionary4[empty4];
                                    empty5 = propertyAliasViewEntity.Alias;
                                    foreach (Sys_sysProperty item3 in propertyBySortId)
                                    {
                                        if (item3.Parentprovalueid == DataConvert.ToInt(dataRow2["Id"]))
                                        {
                                            sp_property = new Sp_property();
                                            int valuetype = item3.Valuetype;
                                            sp_property = new Sp_property();
                                            empty3 = DataConvert.ToString((object)item3.Id);
                                            sp_property.Propertyid = item3.Id;
                                            sp_property.Issellpro = (list2.Contains(empty3) ? 1 : 0);
                                            switch (valuetype)
                                            {
                                                case 2:
                                                    sp_property.Name = propertyAliasViewEntity.Alias;
                                                    sp_property.Value = propertyAliasViewEntity.Alias;
                                                    sp_property.Issellpro = 1;
                                                    list.Add(sp_property);
                                                    break;
                                                case 3:
                                                    if (!string.IsNullOrEmpty(propertyAliasViewEntity.ImageUrl))
                                                    {
                                                        string text5 = imageOperator.DownLoadPicture(propertyAliasViewEntity.ImageUrl, null, 60000, 60000);
                                                        ImageOperator imageOperator2 = imageOperator;
                                                        string fileFullPath = text5;
                                                        string toolCode = null;// base.ToolCode;
                                                        numIid = item.NumIid;
                                                        text5 = imageOperator2.TransPicture(fileFullPath, toolCode, numIid.ToString(), TransPictureType.MovePicture);
                                                        if (flag2 && !string.IsNullOrEmpty(text5))
                                                        {
                                                            text5 = string.Empty;
                                                        }
                                                        if (!string.IsNullOrEmpty(text5))
                                                        {
                                                            sp_property.Name = text5;
                                                            sp_property.Value = text5;
                                                            sp_property.Issellpro = 1;
                                                            if (toolCode == "UPLOADTOYF")
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
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else if (sp_property.Issellpro == 1 && !propertyValueName3.Trim().Equals(sp_property.Name.Trim()))
                                {
                                    empty5 = propertyValueName3;
                                    foreach (Sys_sysProperty item4 in propertyBySortId)
                                    {
                                        if (item4.Parentprovalueid == DataConvert.ToInt(dataRow2["Id"]))
                                        {
                                            sp_property = new Sp_property();
                                            int valuetype2 = item4.Valuetype;
                                            sp_property = new Sp_property();
                                            empty3 = DataConvert.ToString((object)item4.Id);
                                            sp_property.Propertyid = item4.Id;
                                            sp_property.Issellpro = (list2.Contains(empty3) ? 1 : 0);
                                            if (valuetype2 == 2)
                                            {
                                                sp_property.Name = propertyValueName3;
                                                sp_property.Value = propertyValueName3;
                                                sp_property.Issellpro = 1;
                                                list.Add(sp_property);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    empty5 = DataConvert.ToString(dataRow2["Name"]).Trim();
                                }
                                sellProInfo = new SellProInfo();
                                sellProInfo.Value = DataConvert.ToString(dataRow2["Propertyid"]) + ":" + DataConvert.ToString(dataRow2["Id"]);
                                sellProInfo.Name = empty5;
                                dicProValueAndSellProInfos[empty4] = sellProInfo;
                                Haschildproperty(DataConvert.ToBoolean(dataRow2["Haschildproperty"]), sysSysPropertyList, empty4, dictionary6, list2, ref list);
                            }
                        }
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator4).Dispose();
            }
            foreach (PropsViewEntity item5 in list3)
            {
                DataRow dataRow3 = null;
                sp_property = new Sp_property();
                num = item5.PropertyValue.IndexOf(":");
                if (num != 0)
                {
                    proKey = item5.PropertyValue.Remove(num);
                    empty4 = item5.PropertyValue;
                    DataRow[] array3 = dtPropertyValue.Select("value like '%" + proKey + "%'");
                    if (array3 != null && array3.Length > 0)
                    {
                        int num5 = DataConvert.ToInt(array3[0]["Propertyid"]);
                        Sys_sysProperty val2 = DataHelper.GetPropertyById(num5);
                        if (val2 != null)
                        {
                            bool flag3 = false;
                            DataRow[] array4 = array3;
                            foreach (DataRow dataRow4 in array4)
                            {
                                if (!dictionary8.ContainsKey(num5.ToString() + DataConvert.ToString(dataRow4["Value"])))
                                {
                                    string value = DataConvert.ToString(dataRow4["Name"]);
                                    if (!string.IsNullOrEmpty(value) && item5.PropertyValueName.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        dataRow3 = dataRow4;
                                        flag3 = true;
                                        break;
                                    }
                                }
                            }
                            if (!flag3 && val2.Issellpro)
                            {
                                array4 = array3;
                                foreach (DataRow dataRow5 in array4)
                                {
                                    if (!dictionary8.ContainsKey(num5.ToString() + DataConvert.ToString(dataRow5["Value"])))
                                    {
                                        dataRow3 = dataRow5;
                                        flag3 = true;
                                        break;
                                    }
                                }
                                if (array3 != null && array3.Length > 0 && dataRow3 != null && !DataConvert.ToBoolean(dataRow3["Haschildproperty"]))
                                {
                                    SellNewProInfo sellNewProInfo2 = sellNewProInfo2 = new SellNewProInfo();
                                    sp_property.Propertyid = num5;
                                    sp_property.Issellpro = (list2.Contains(DataConvert.ToString((object)num5)) ? 1 : 0);
                                    string value2 = string.Empty;
                                    if (dictionary9 == null || dictionary9.Count == 0)
                                    {
                                        value2 = proKey + ":-1001";
                                        dictionary9.Add(proKey, -1001);
                                        sp_property.Name = item5.PropertyValueName;
                                        sp_property.Value = value2;
                                    }
                                    else if (dictionary9.ContainsKey(proKey))
                                    {
                                        Dictionary<string, int> dictionary10;
                                        string key2;
                                        (dictionary10 = dictionary9)[key2 = proKey] = dictionary10[key2] + -1;
                                        value2 = proKey + ":" + dictionary9[proKey];
                                        sp_property.Name = item5.PropertyValueName;
                                        sp_property.Value = value2;
                                    }
                                    sellNewProInfo2.Value = value2;
                                    sellNewProInfo2.Name = item5.PropertyValueName;
                                    sourceProValueAndNewProValue[item5.PropertyValue] = sellNewProInfo2;
                                    list.Add(sp_property);
                                    dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                                    continue;
                                }
                            }
                        }
                    }
                    if (dataRow3 == null)
                    {
                        Sys_sysProperty val3 = (propertyBySortId as List<Sys_sysProperty>).Find((Sys_sysProperty x) => x.Keys == proKey);
                        if ((array3 == null || array3.Length <= 0) && val3 != null)
                        {
                            SellNewProInfo sellNewProInfo3 = sellNewProInfo3 = new SellNewProInfo();
                            sp_property.Propertyid = val3.Id;
                            sp_property.Issellpro = (list2.Contains(DataConvert.ToString((object)val3.Id)) ? 1 : 0);
                            string value3 = string.Empty;
                            if (dictionary9 == null || dictionary9.Count == 0)
                            {
                                value3 = proKey + ":-1001";
                                dictionary9.Add(proKey, -1001);
                                sp_property.Name = item5.PropertyValueName;
                                sp_property.Value = value3;
                            }
                            else if (dictionary9.ContainsKey(proKey))
                            {
                                Dictionary<string, int> dictionary10;
                                string key2;
                                (dictionary10 = dictionary9)[key2 = proKey] = dictionary10[key2] + -1;
                                value3 = proKey + ":" + dictionary9[proKey];
                                sp_property.Name = item5.PropertyValueName;
                                sp_property.Value = value3;
                            }
                            sellNewProInfo3.Value = value3;
                            sellNewProInfo3.Name = item5.PropertyValueName;
                            sourceProValueAndNewProValue[item5.PropertyValue] = sellNewProInfo3;
                            list.Add(sp_property);
                            dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                        }
                    }
                    else
                    {
                        empty3 = DataConvert.ToString(dataRow3["Propertyid"]);
                        sp_property.Propertyid = DataConvert.ToInt((object)empty3);
                        sp_property.Value = DataConvert.ToString(dataRow3["Value"]);
                        sp_property.Name = DataConvert.ToString(dataRow3["Name"]);
                        sp_property.Issellpro = (list2.Contains(empty3) ? 1 : 0);
                        string propertyValueName4 = item5.PropertyValueName;
                        list.Add(sp_property);
                        dictionary8[sp_property.Propertyid + sp_property.Value] = sp_property.Value;
                        if (dictionary4.ContainsKey(empty4))
                        {
                            propertyAliasViewEntity = dictionary4[empty4];
                            empty5 = propertyAliasViewEntity.Alias;
                            foreach (Sys_sysProperty item6 in propertyBySortId)
                            {
                                if (item6.Parentprovalueid == DataConvert.ToInt(dataRow3["Id"]))
                                {
                                    sp_property = new Sp_property();
                                    int valuetype3 = item6.Valuetype;
                                    sp_property = new Sp_property();
                                    empty3 = DataConvert.ToString((object)item6.Id);
                                    sp_property.Propertyid = item6.Id;
                                    sp_property.Issellpro = (list2.Contains(empty3) ? 1 : 0);
                                    switch (valuetype3)
                                    {
                                        case 2:
                                            sp_property.Name = propertyAliasViewEntity.Alias;
                                            sp_property.Value = propertyAliasViewEntity.Alias;
                                            sp_property.Issellpro = 1;
                                            list.Add(sp_property);
                                            break;
                                        case 3:
                                            if (!string.IsNullOrEmpty(propertyAliasViewEntity.ImageUrl))
                                            {
                                                string text6 = imageOperator.DownLoadPicture(propertyAliasViewEntity.ImageUrl, null, 60000, 60000);
                                                ImageOperator imageOperator3 = imageOperator;
                                                string fileFullPath2 = text6;
                                                string toolCode2 = null;// base.ToolCode;
                                                numIid = item.NumIid;
                                                text6 = imageOperator3.TransPicture(fileFullPath2, toolCode2, numIid.ToString(), TransPictureType.MovePicture);
                                                if (flag2 && !string.IsNullOrEmpty(text6))
                                                {
                                                    text6 = string.Empty;
                                                }
                                                if (!string.IsNullOrEmpty(text6))
                                                {
                                                    sp_property.Name = text6;
                                                    sp_property.Value = text6;
                                                    sp_property.Issellpro = 1;
                                                    if (toolCode2 == "UPLOADTOYF")
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
                                            break;
                                    }
                                }
                            }
                        }
                        else if (sp_property.Issellpro == 1 && !propertyValueName4.Trim().Equals(sp_property.Name.Trim()))
                        {
                            empty5 = propertyValueName4;
                            foreach (Sys_sysProperty item7 in propertyBySortId)
                            {
                                if (item7.Parentprovalueid == DataConvert.ToInt(dataRow3["Id"]))
                                {
                                    sp_property = new Sp_property();
                                    int valuetype4 = item7.Valuetype;
                                    sp_property = new Sp_property();
                                    empty3 = DataConvert.ToString((object)item7.Id);
                                    sp_property.Propertyid = item7.Id;
                                    sp_property.Issellpro = (list2.Contains(empty3) ? 1 : 0);
                                    if (valuetype4 == 2)
                                    {
                                        sp_property.Name = propertyValueName4;
                                        sp_property.Value = propertyValueName4;
                                        sp_property.Issellpro = 1;
                                        list.Add(sp_property);
                                    }
                                }
                            }
                        }
                        else
                        {
                            empty5 = DataConvert.ToString(dataRow3["Name"]).Trim();
                        }
                        sellProInfo = new SellProInfo();
                        sellProInfo.Value = DataConvert.ToString(dataRow3["Propertyid"]) + ":" + DataConvert.ToString(dataRow3["Id"]);
                        sellProInfo.Name = empty5;
                        dicProValueAndSellProInfos[empty4] = sellProInfo;
                        Haschildproperty(DataConvert.ToBoolean(dataRow3["Haschildproperty"]), sysSysPropertyList, empty4, dictionary6, list2, ref list);
                    }
                }
            }
            return list;
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
            string empty = string.Empty;
            int num = 0;
            for (int i = 0; i < str.Length; i++)
            {
                empty = text + str[i];
                num = GetStringByteLength(empty);
                if (num > maxByteLength)
                {
                    return text;
                }
                text = empty;
            }
            return text;
        }
        private Dictionary<string, string> GetInputPidAndStr(Item item)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (item != null && item.InputPids != null && item.InputStr != null)
            {
                string[] array = item.InputPids.Split(new string[1]
                {
                    ","
                }, StringSplitOptions.RemoveEmptyEntries);
                string[] array2 = item.InputStr.Split(new string[1]
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
        private List<PropsViewEntity> GetLstPropsDTO(string propsName)
        {
            List<PropsViewEntity> list = new List<PropsViewEntity>();
            if (string.IsNullOrEmpty(propsName))
            {
                return list;
            }
            PropsViewEntity propsViewEntity = null;
            string pattern = "(?<PropertyValueAndName>(?<PropertyValue>(?<ProKey>\\d*):-?\\d*):(?<PropertyName>.*?):(?<PropertyValueName>.*?));";
            MatchCollection matchCollection = Regex.Matches(propsName + ";", pattern, RegexOptions.IgnoreCase);
            foreach (Match item in matchCollection)
            {
                propsViewEntity = new PropsViewEntity();
                propsViewEntity.PropertyValueAndName = item.Groups["PropertyValueAndName"].Value;
                propsViewEntity.PropertyValue = item.Groups["PropertyValue"].Value;
                propsViewEntity.ProKey = item.Groups["ProKey"].Value;
                propsViewEntity.PropertyName = item.Groups["PropertyName"].Value;
                propsViewEntity.PropertyValueName = item.Groups["PropertyValueName"].Value.Replace("#scln#", ";");
                list.Add(propsViewEntity);
            }
            return list;
        }
        private void Haschildproperty(bool haschildproperty, IList<Sys_sysProperty> sysSysPropertyList, string newProValue, Dictionary<string, string> dicProNameAndValue, List<string> lstSellProId, ref IList<Sp_property> lstEntSpProperty)
        {
            if (haschildproperty)
            {
                foreach (Sys_sysProperty sysSysProperty in sysSysPropertyList)
                {
                    if (sysSysProperty.Parentpropertyvalue.Equals(newProValue) && sysSysProperty.Valuetype == 2 && dicProNameAndValue.ContainsKey(sysSysProperty.Name   ))
                    {
                        Sp_property sp_property = new Sp_property();
                        string item = DataConvert.ToString((object)sysSysProperty.Id);
                        sp_property.Propertyid = sysSysProperty.Id;
                        sp_property.Value = dicProNameAndValue[sysSysProperty.Name];
                        sp_property.Issellpro = (lstSellProId.Contains(item) ? 1 : 0);
                        sp_property.Name = sysSysProperty.Name;
                        lstEntSpProperty.Add(sp_property);
                    }
                }
            }
        }
        public void HandleProperty( ProductItem productItem, IList<Sp_property> propertyList)
        {

            string text = "";
            string text2 = string.Empty;
            string text3 = "";
            string features = "";
            IList<Sp_property> propertyListByItemId = propertyList;
            Dictionary<string, Sp_property> dictionary = new Dictionary<string, Sp_property>();
            IList<Sp_property> list = new List<Sp_property>();
            if (propertyListByItemId != null && propertyListByItemId.Count > 0)
            {
                int num = 0;
                while (num < propertyListByItemId.Count)
                {
                    if ((object)propertyListByItemId[num].SysProperty == null || num >= propertyListByItemId.Count - 1 || !(propertyListByItemId[num].SysProperty.Parentname == propertyListByItemId[num + 1].Name) || !(propertyListByItemId[num + 1].Name == "品牌"))
                    {
                        num++;
                        continue;
                    }
                    Sp_property value = propertyListByItemId[num];
                    propertyListByItemId[num] = propertyListByItemId[num + 1];
                    propertyListByItemId[num + 1] = value;
                    break;
                }
            }
            Dictionary<string, string> dicCustomProperty = GetDicCustomProperty();
            Sys_sysSort val = DataHelper.GetSysSort(productItem.ProductSortKeys);// ToolServer.get_ProductData().GetSortBySysIdAndKeys(1, productItem.SortKey);
            string sortAllName = string.Empty;
            if (val != null && !string.IsNullOrEmpty(val.Name))
            {
                sortAllName = val.Path + ">>" + val.Name;
            }
            foreach (Sp_property item in propertyListByItemId)
            {
                string text4;
                if ((object)item.SysProperty != null && item.SysProperty.Keys != null)
                {
                    text4 = DataConvert.ToString((object)item.SysProperty.Id);
                    string a = DataConvert.ToString((object)item.SysProperty.Valuetype);
                    string text5 = item.SysProperty.Keys;
                    string a2 = DataConvert.ToString((object)item.SysProperty.Parentid);
                    string text6 = DataConvert.ToString((object)item.SysProperty.Levels);
                    string text7 = (item.SysProperty.Parentpropertyvalue == null) ? "" : item.SysProperty.Parentpropertyvalue;
                    if (a != "2" && a != "3")
                    {
                        goto IL_025c;
                    }
                    if (a == "2" && item.Issellpro == 1 && IsOldSizeAndNewTaobao(dicCustomProperty, sortAllName, item.SysProperty.Name))
                    {
                        goto IL_025c;
                    }
                    if (item.Issellpro != 1)
                    {
                        string text8 = item.Value;
                        if (DataConvert.ToInt((object)text6) >= 2)
                        {
                            string text9 = "";
                            if (a2 != "0" && text5.IndexOf(":") == -1 && !string.IsNullOrEmpty(text7))
                            {
                                text5 = text7;
                            }
                            IList<Sys_sysProperty> propertyAllTopLevelProperty = DataHelper.GetPropertyAllTopLevelProperty(text4);
                            Sys_sysProperty val2 = null;
                            for (int num2 = propertyAllTopLevelProperty.Count - 1; num2 >= 0; num2--)
                            {
                                val2 = propertyAllTopLevelProperty[num2];
                                string key = DataConvert.ToString((object)val2.Id);
                                if (dictionary.ContainsKey(key))
                                {
                                    Sys_sysProperty sysProperty = dictionary[key].SysProperty;
                                    Sp_property sp_property = dictionary[key];
                                    if (sysProperty.Valuetype == 0)
                                    {
                                        if (sysProperty.Parentid == 0)
                                        {
                                            text5 = sysProperty.Keys;
                                            if (DataConvert.ToInt((object)text6) > 2 || (!(sp_property.Name != "其他") && !(sp_property.Name != "其它")))
                                            {
                                                text9 = text9 + sp_property.Name + ";";
                                            }
                                        }
                                        else
                                        {
                                            string name = sp_property.Name;
                                            if (name == "其他" || name == "其它")
                                            {
                                                text9 = text9 + sysProperty.Name + ";";
                                            }
                                            else
                                            {
                                                string text10 = text9;
                                                text9 = text10 + sysProperty.Name + ";" + name + ";";
                                            }
                                        }
                                    }
                                }
                            }
                            if (item.SysProperty.Parentpropertyvalue == "20000:-1" && item.SysProperty.Keys != "20000")
                            {
                                text8 = item.SysProperty.Name + ";" + text8;
                            }
                            text8 = text9 + text8;
                            list.Add(item);
                            if (list.Count >= 2 && list[list.Count - 1].SysProperty.Parentid == list[list.Count - 2].SysProperty.Parentid)
                            {
                                text3 = text3.TrimEnd(',');
                                text3 = text3 + ";" + text8 + ",";
                            }
                            else
                            {
                                string[] array = text5.Split(new char[1]
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
                                    text3 = text3 + text8 + ",";
                                }
                            }
                        }
                        else
                        {
                            string[] array2 = text5.Split(new char[1]
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
                            text3 = text3 + text8 + ",";
                        }
                        goto IL_059f;
                    }
                }
                continue;
                IL_059f:
                if (!dictionary.ContainsKey(text4))
                {
                    dictionary.Add(text4, item);
                }
                continue;
                IL_025c:
                text = text + item.Value + ";";
                goto IL_059f;
            }
            text = text.Trim(',');
            text2 = text2.TrimEnd(',');
            text3 = text3.TrimEnd(',');
            Sys_sysSort val3 = DataHelper.GetSysSort(productItem.SortKey);// ToolServer.get_ProductData().GetSortBySysIdAndKeys(1, productItem.SortKey);
            if (val3 == null && !string.IsNullOrEmpty(productItem.SortKey))
            {
                val3 = DataHelper.GetSysSort(val3.Id.ToString());//  ToolServer.get_ProductData().GetSortById(val3.Id);
            }
            if (val3 != null && val3.IsNewSellPro)
            {
                Sys_sizeDetail val4 = new Sys_sizeDetail();
                foreach (Sp_property item2 in propertyListByItemId)
                {
                    val4 = DataHelper.GetSizeDetailBySizeValue(item2.Value, val3.SizeGroupType, 1);
                    if (val4 != null && val4.SizeName != "均码")
                    {
                        break;
                    }
                }
                if (val4 != null)
                {
                    Sys_sizeGroup val5 = DataHelper.GetSizeGroupByGroupOnlineID(val3.SizeGroupType, val4.GroupOnlineID, 1);
                    if (val5 != null)
                    {
                        features = ((!(val5.GroupName == "其它")) ? ("mysize_tp:-1;sizeGroupId:" + val4.GroupOnlineID + ";sizeGroupName:" + val5.GroupName + ";sizeGroupType:" + val3.SizeGroupType) : ("mysize_tp:-1;sizeGroupId:" + val4.GroupOnlineID + ";sizeGroupName:" + val5.GroupName + ";sizeGroupType:no_group1"));
                    }
                }
                productItem.Features = features;
            }
            productItem.PropertyValue = text;
            productItem.UserInputPropIDs = text2;
            productItem.UserInputPropValues = text3;
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
        private Dictionary<string, string> GetDicCustomProperty()
        {
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            //IL_0025: Unknown result type (might be due to invalid IL or missing references)
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string sysConfig = ConfigHelper.TaoBaoCustomSellProperty;// ToolServer.get_ConfigData().GetSysConfig("AppConfig", "Taobao", "CustomSellProperty", "");
            string sysConfig2 = ConfigHelper.TaoBaoCustomSellProperty2;// ToolServer.get_ConfigData().GetSysConfig("AppConfig", "Taobao", "CustomSellProperty2", "");
            string text = sysConfig + sysConfig2;
            if (!string.IsNullOrEmpty(text))
            {
                text = text.TrimEnd('|');
                string[] array = text.Split('|');
                if (array != null && array.Length > 0)
                {
                    string[] array2 = array;
                    foreach (string text2 in array2)
                    {
                        if (!string.IsNullOrEmpty(text2))
                        {
                            string[] array3 = text2.Split(',');
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
        public void HandleShipWay(ProductItem productItem)
        {
 
            string actualShipSlow = "0";
            string actualShipFast = "0";
            string actualShipEMS = "0";
            string actualShipTpl = "0";
            string text = DataConvert.ToString((object)productItem.ShipSlow);
            string text2 = DataConvert.ToString((object)productItem.ShipFast);
            string text3 = DataConvert.ToString((object)productItem.ShipEMS);
            string text4 = productItem.ShipWay;
            string useShipTpl = productItem.UseShipTpl;
            string text5 = DataConvert.ToString((object)productItem.ShipTplId);
            if (!string.IsNullOrEmpty(text4))
            {
                switch (text4.Trim())
                {
                    case "1":
                        if (!DataConvert.ToBoolean((object)useShipTpl))
                        {
                            actualShipSlow = text;
                            actualShipFast = text2;
                            actualShipEMS = text3;
                        }
                        else
                        {
                            Sys_shopShip val = DataHelper.GetShopShipById(DataConvert.ToInt((object)text5));
                            text5 = ((val != null) ? val.Keys : string.Empty);
                            actualShipTpl = text5;
                        }
                        text4 = "2";
                        break;
                    case "2":
                        text4 = "1";
                        break;
                }
                productItem.ActualShipSlow = actualShipSlow;
                productItem.ActualShipFast = actualShipFast;
                productItem.ActualShipEMS = actualShipEMS;
                productItem.ActualShipTpl = actualShipTpl;
                productItem.ShipWay = text4;
            }
        }

        public string HandleSellProperty(int cid, out string skuBarcode, ProductItem productItem, IList<Sp_property> propertyList, IList<Sp_sellProperty> SpSellPropertyList)
        {

            skuBarcode = string.Empty;
            int sysId = 1;
            Sys_sysSort val = DataHelper.GetSysSort(DataConvert.ToString(productItem.ProductSortKeys));

            if (val == null)
            {
                // Log.WriteLog("导出商品时，获取类目信息失败，" + Environment.StackTrace);
                return string.Empty;
            }
            IList<Sp_sellProperty> sellProperty = SpSellPropertyList;// DataHelper.GetSellProperty(id, sysId);
            if (sellProperty != null && sellProperty.Count != 0)
            {
                List<string> list = new List<string>();
                for (int num = sellProperty.Count - 1; num >= 0; num--)
                {
                    if (list.Contains(sellProperty[num].Sellproinfos))
                    {
                        sellProperty.Remove(sellProperty[num]);
                    }
                    else
                    {
                        list.Add(sellProperty[num].Sellproinfos);
                    }
                }
                string text = "";
                skuBarcode = string.Empty;
                sellProperty = OrderProperty(sellProperty, cid, propertyList);
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                {
                    foreach (Sp_sellProperty item in sellProperty)
                    {
                        string text2 = (item.Sellproinfos == null) ? "" : item.Sellproinfos;
                        string text3 = (item.Price == 0m) ? "" : DataConvert.ToString((object)item.Price);
                        string text4 = (item.Nums == 0) ? "" : DataConvert.ToString((object)item.Nums);
                        string text5 = (item.Code == null) ? "" : DataConvert.ToString((object)item.Code).Replace(" ", "").Replace("\u3000", "")
                            .Replace("/", "")
                            .Replace("\\", "")
                            .Replace("&", "");
                        string text6 = text;
                        text = text6 + text3 + ":" + text4 + ":" + text5 + ":";
                        if (!string.IsNullOrEmpty(text2))
                        {
                            string[] array = text2.Split('|');
                            for (int i = 0; i < array.Length; i++)
                            {
                                string[] array2 = array[i].Split(':');
                                if (array2 != null && array2.Length > 1)
                                {
                                    if (val != null && val.IsNewSellPro)
                                    {
                                        Sys_sysPropertyValue val2 = DataHelper.GetPropertyValueById(Convert.ToInt32(array2[1]));
                                        if (val2 != null)
                                        {
                                            text = text + val2.Value + ";";
                                        }
                                        else
                                        {
                                            IList<Sys_sysPropertyValue> propertyValuesByPropertyId = DataHelper.GetPropertyValuesByPropertyId(Convert.ToInt32(array2[0]));
                                            if (propertyValuesByPropertyId != null && propertyValuesByPropertyId.Count > 0)
                                            {
                                                string[] array3 = propertyValuesByPropertyId[0].Value.Split(':');
                                                string text7 = text;
                                                text = text7 + array3[0] + ":" + array2[1] + ";";
                                            }
                                            else if (array2.Length >= 2)
                                            {
                                                if (dictionary != null && !dictionary.ContainsKey(array2[0]))
                                                {
                                                    IList<Sys_sysProperty> propertyAllTopLevelProperty = DataHelper.GetPropertyAllTopLevelProperty(array2[0]);
                                                    if (propertyAllTopLevelProperty != null && propertyAllTopLevelProperty.Count == 1)
                                                    {
                                                        dictionary[array2[0]] = propertyAllTopLevelProperty[0].Keys;
                                                    }
                                                }
                                                string empty = string.Empty;
                                                empty = ((dictionary == null || dictionary.Count <= 0 || !dictionary.ContainsKey(array2[0]) || string.IsNullOrEmpty(dictionary[array2[0]])) ? (array2[0] + ":" + array2[1] + ";") : (dictionary[array2[0]] + ":" + array2[1] + ";"));
                                                text += empty;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Sys_sysPropertyValue val3 = DataHelper.GetPropertyValueById(DataConvert.ToInt((object)array2[1]));
                                        string text8 = (val3 != null) ? val3.Value : string.Empty;
                                        if (string.IsNullOrEmpty(text8) && array2.Length >= 2)
                                        {
                                            text8 = array2[0] + ":" + array2[1];
                                        }
                                        text = text + text8 + ";";
                                    }
                                }
                            }
                        }
                        skuBarcode = skuBarcode + item.Barcode + ";";
                    }
                    return text;
                }
            }
            return string.Empty;
        }
        private IList<Sp_sellProperty> OrderProperty(IList<Sp_sellProperty> sellPropertyList, int itemId, IList<Sp_property> propertyList)
        {
            IList<Sp_sellProperty> list = new List<Sp_sellProperty>();
            if (sellPropertyList != null && sellPropertyList.Count > 0)
            {
                IList<Sp_property> propertyListByItemId = propertyList;// DataHelper.GetPropertyListByItemId(itemId);
                Dictionary<int, IList<Sp_property>> dictionary = new Dictionary<int, IList<Sp_property>>();
                if (propertyListByItemId != null && propertyListByItemId.Count > 0)
                {
                    foreach (Sp_property item in propertyListByItemId)
                    {
                        if (item.Issellpro == 1 && (object)item.SysProperty != null && item.SysProperty.Levels == 1)
                        {
                            if (dictionary.ContainsKey(item.Propertyid))
                            {
                                dictionary[item.Propertyid].Add(item);
                            }
                            else
                            {
                                dictionary[item.Propertyid] = new List<Sp_property>();
                                dictionary[item.Propertyid].Add(item);
                            }
                        }
                    }
                }
                List<string> list2 = new List<string>();
                if (dictionary != null && dictionary.Count > 0 && itemId > 0)
                {
                    list2 = GetOrderProp(dictionary, itemId);
                }
                foreach (string item2 in list2)
                {
                    foreach (Sp_sellProperty sellProperty in sellPropertyList)
                    {
                        string[] array = item2.Split('|');
                        bool flag = false;
                        if (array != null && array.Length > 0)
                        {
                            string[] array2 = array;
                            int num = 0;
                            while (num < array2.Length)
                            {
                                string str = array2[num];
                                if (sellProperty.Sellproinfos.Contains(str + ":"))
                                {
                                    flag = true;
                                    num++;
                                    continue;
                                }
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            list.Add(sellProperty);
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
        private List<string> GetOrderProp(Dictionary<int, IList<Sp_property>> dicProp, int itemId)
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            Sys_sysSort spSysSort = DataHelper.GetSysSort(itemId.ToString());
            DataTable dataTable = new DataTable();
            if (spSysSort != null)
            {
                dataTable = DataHelper.GetPropertyValueDtBySortId(spSysSort.Id);
            }
            List<List<string>> list = new List<List<string>>();
            if (dicProp != null && dicProp.Count > 0)
            {
                foreach (KeyValuePair<int, IList<Sp_property>> item in dicProp)
                {
                    List<string> list2 = new List<string>();
                    foreach (Sp_property item2 in item.Value)
                    {
                        if (!string.IsNullOrEmpty(item2.Value) && item2.Value.Split(':').Length >= 2)
                        {
                            if (item2.Value.Split(':')[1].Contains("-"))
                            {
                                list2.Add(item2.Propertyid + ":" + item2.Value.Split(':')[1]);
                            }
                            else if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                DataRow[] array = dataTable.Select("value='" + item2.Value + "'");
                                if (array != null && array.Length > 0)
                                {
                                    list2.Add(item2.Propertyid + ":" + DataConvert.ToString(array[0]["id"]));
                                }
                            }
                        }
                    }
                    list.Add(list2);
                }
            }
            return Exchange(list);
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
