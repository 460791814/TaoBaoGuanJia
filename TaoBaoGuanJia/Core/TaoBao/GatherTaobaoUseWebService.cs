using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
    public class GatherTaobaoUseWebService : BaseSnatcher
    {
        public class PromoEntity
        {
            public string Type
            {
                get;
                set;
            }
            public string Wenan
            {
                get;
                set;
            }
            public string LimitTime
            {
                get;
                set;
            }
            public string Channelkey
            {
                get;
                set;
            }
            public string Add
            {
                get;
                set;
            }
            public string Gift
            {
                get;
                set;
            }
            public string Cart
            {
                get;
                set;
            }
            public string AmountRestriction
            {
                get;
                set;
            }
            public string IsStart
            {
                get;
                set;
            }
            public string Price
            {
                get;
                set;
            }
        }
        public class TaobaoPromoPriceEntity
        {
            public System.Collections.Generic.Dictionary<string, GatherTaobaoUseWebService.AvailSKU> availSKUs
            {
                get;
                set;
            }
            public System.Collections.Generic.List<GatherTaobaoUseWebService.SkuProp> skuProps
            {
                get;
                set;
            }
        }
        public class SkuProp
        {
            public string name
            {
                get;
                set;
            }
            public System.Collections.Generic.List<GatherTaobaoUseWebService.value> values
            {
                get;
                set;
            }
        }
        public class value
        {
            public string id
            {
                get;
                set;
            }
            public string img
            {
                get;
                set;
            }
            public string txt
            {
                get;
                set;
            }
        }
        public class AvailSKU
        {
            public string price
            {
                get;
                set;
            }
            public string promoPrice
            {
                get;
                set;
            }
            public string quantity
            {
                get;
                set;
            }
            public string skuId
            {
                get;
                set;
            }
            public string tmall
            {
                get;
                set;
            }
        }
        public string _toolCode = string.Empty;
        private string _sysName = "Taobao";
        private static object _lockObj = new object();
        private static System.DateTime _lastDownTime = System.DateTime.MinValue;
        //private ITaobaoService _taobaoService;
        //private IUserSetDataService _userSetDataService;
        internal static readonly string STARTSTR = "needsplit:";
        private static readonly string CONNSTR1 = "!-!";
        private static readonly string CONNSTR2 = "|-|";
        private static object obj = new object();
        //private ITaobaoService TaobaoService
        //{
        //    get
        //    {
        //        if (this._taobaoService == null)
        //        {
        //            lock (GatherTaobaoUseWebService._lockObj)
        //            {
        //                if (this._taobaoService == null)
        //                {
        //                    this._taobaoService = ObjectGetter.GetService<ITaobaoService>();
        //                }
        //            }
        //        }
        //        return this._taobaoService;
        //    }
        //}
        //private IUserSetDataService UserSetDataService
        //{
        //    get
        //    {
        //        if (this._userSetDataService == null)
        //        {
        //            this._userSetDataService = ObjectGetter.GetService<IUserSetDataService>();
        //            this._userSetDataService.ToolCode = this._toolCode;
        //        }
        //        return this._userSetDataService;
        //    }
        //}
        public override int SysId
        {
            get
            {
                return 1;
            }
        }
        public ItemGetResponse GetItemResponse(string onlineKey, bool isSnatchPromoPrice)
        {
            ItemGetResponse itemGetResponse = null;
            try
            {
                itemGetResponse = new ItemGetResponse();
                itemGetResponse.ErrCode = "998";
            }
            catch (System.Exception ex)
            {
                Log.WriteLog(ex);
            }
            finally
            {
                GatherTaobaoUseWebService._lastDownTime = System.DateTime.Now;
            }
            return itemGetResponse;
        }
        internal static void DecodePropertyStr(string propsStr, string propsNameStr, out System.Collections.Generic.Dictionary<string, string> dicProNameAndProValue, out System.Collections.Generic.IList<SellProInfo> sellProInfoList)
        {
            dicProNameAndProValue = new System.Collections.Generic.Dictionary<string, string>();
            sellProInfoList = new System.Collections.Generic.List<SellProInfo>();
            if (!string.IsNullOrEmpty(propsStr) && propsStr.StartsWith(GatherTaobaoUseWebService.STARTSTR))
            {
                propsStr = propsStr.Substring(GatherTaobaoUseWebService.STARTSTR.Length);
                if (!string.IsNullOrEmpty(propsStr))
                {
                    string[] array = propsStr.Split(new string[]
                    {
                        GatherTaobaoUseWebService.CONNSTR1
                    }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (array != null && array.Length > 0)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(array[i]))
                            {
                                string[] array2 = array[i].Split(new string[]
                                {
                                    GatherTaobaoUseWebService.CONNSTR2
                                }, System.StringSplitOptions.RemoveEmptyEntries);
                                if (array2 != null && array2.Length == 2)
                                {
                                    string key = string.Empty;
                                    if (!string.IsNullOrEmpty(array2[0]))
                                    {
                                        key = Base64.FromBase64(array2[0]);
                                    }
                                    string value = string.Empty;
                                    if (!string.IsNullOrEmpty(array2[1]))
                                    {
                                        value = Base64.FromBase64(array2[1]);
                                    }
                                    dicProNameAndProValue[key] = value;
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(propsNameStr) && propsNameStr.StartsWith(GatherTaobaoUseWebService.STARTSTR))
            {
                propsNameStr = propsNameStr.Substring(GatherTaobaoUseWebService.STARTSTR.Length);
                if (!string.IsNullOrEmpty(propsNameStr))
                {
                    string[] array3 = propsNameStr.Split(new string[]
                    {
                        GatherTaobaoUseWebService.CONNSTR1
                    }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (array3 != null && array3.Length > 0)
                    {
                        for (int j = 0; j < array3.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(array3[j]))
                            {
                                string[] array4 = array3[j].Split(new string[]
                                {
                                    GatherTaobaoUseWebService.CONNSTR2
                                }, System.StringSplitOptions.RemoveEmptyEntries);
                                if (array4 != null && array4.Length == 2)
                                {
                                    string name = string.Empty;
                                    if (!string.IsNullOrEmpty(array4[0]))
                                    {
                                        name = Base64.FromBase64(array4[0]);
                                    }
                                    string value2 = string.Empty;
                                    if (!string.IsNullOrEmpty(array4[1]))
                                    {
                                        value2 = Base64.FromBase64(array4[1]);
                                    }
                                    sellProInfoList.Add(new SellProInfo
                                    {
                                        Name = name,
                                        Value = value2
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
        internal static void GetPropertyStrForCloudSnatch(System.Collections.Generic.Dictionary<string, string> dicProNameAndProValue, System.Collections.Generic.IList<SellProInfo> sellProInfoList, out string propsStr, out string propsNameStr)
        {
            propsStr = string.Empty;
            propsNameStr = string.Empty;
            if (dicProNameAndProValue != null && dicProNameAndProValue.Count > 0)
            {
                propsStr = GatherTaobaoUseWebService.STARTSTR;
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicProNameAndProValue)
                {
                    if (propsStr == GatherTaobaoUseWebService.STARTSTR)
                    {
                        propsStr = propsStr + Base64.ToBase64(current.Key) + GatherTaobaoUseWebService.CONNSTR2 + Base64.ToBase64(current.Value);
                    }
                    else
                    {
                        string text = propsStr;
                        propsStr = string.Concat(new string[]
                        {
                            text,
                            GatherTaobaoUseWebService.CONNSTR1,
                            Base64.ToBase64(current.Key),
                            GatherTaobaoUseWebService.CONNSTR2,
                            Base64.ToBase64(current.Value)
                        });
                    }
                }
            }
            if (sellProInfoList != null && sellProInfoList.Count > 0)
            {
                propsNameStr = GatherTaobaoUseWebService.STARTSTR;
                for (int i = 0; i < sellProInfoList.Count; i++)
                {
                    if (propsNameStr == GatherTaobaoUseWebService.STARTSTR)
                    {
                        propsNameStr = propsNameStr + Base64.ToBase64(sellProInfoList[i].Name) + GatherTaobaoUseWebService.CONNSTR2 + Base64.ToBase64(sellProInfoList[i].Value);
                    }
                    else
                    {
                        string text2 = propsNameStr;
                        propsNameStr = string.Concat(new string[]
                        {
                            text2,
                            GatherTaobaoUseWebService.CONNSTR1,
                            Base64.ToBase64(sellProInfoList[i].Name),
                            GatherTaobaoUseWebService.CONNSTR2,
                            Base64.ToBase64(sellProInfoList[i].Value)
                        });
                    }
                }
            }
        }
        internal static void GetPropertyStrForLocalSnatch(System.Collections.Generic.Dictionary<string, string> dicProNameAndProValue, System.Collections.Generic.IList<SellProInfo> sellProInfoList, string cid, out string propsStr, out string propsNameStr, out string inputStr, out string inputStrName)
        {
            int num = 100001;
            System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
            propsStr = string.Empty;
            propsNameStr = string.Empty;
            inputStr = string.Empty;
            inputStrName = string.Empty;
            if (dicProNameAndProValue != null && dicProNameAndProValue.Count > 0)
            {
                int num2 = 0;
                Sys_sysSort sortBySysIdAndKeys = DataHelper.GetSortBySysIdAndKeys(1, cid);
                if (sortBySysIdAndKeys != null)
                {
                    num2 = sortBySysIdAndKeys.Id;
                }
                if (num2 > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
                    DataTable propertyDtBySortId = DataHelper.GetPropertyDtBySortId(num2);
                    DataTable propertyValueDtBySortId = DataHelper.GetPropertyValueDtBySortId(num2);
                    foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicProNameAndProValue)
                    {
                        try
                        {
                            if (!dictionary.ContainsKey(current.Value + current.Key))
                            {
                                string likeStrForDtSelect = GatherTaobaoUseWebService.GetLikeStrForDtSelect(current.Key);
                                DataRow[] array = propertyDtBySortId.Select("name='" + DbUtil.OerateSpecialChar(likeStrForDtSelect) + "'");
                                if (current.Key == "品牌" && current.Key == "品牌")
                                {
                                    bool flag = false;
                                    DataRow dataRow = null;
                                    DataRow[] array2 = array;
                                    for (int i = 0; i < array2.Length; i++)
                                    {
                                        DataRow dataRow2 = array2[i];
                                        DataRow[] array3 = propertyValueDtBySortId.Select(string.Concat(new string[]
                                        {
                                            "propertyid='",
                                            dataRow2["id"].ToString(),
                                            "' and name = '",
                                            DbUtil.OerateSpecialChar(current.Value),
                                            "'"
                                        }));
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
                                        for (int j = 0; j < array4.Length; j++)
                                        {
                                            DataRow dataRow4 = array4[j];
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
                                    GatherTaobaoUseWebService.GetChildPropertyAndSetProps(propertyDtBySortId, propertyValueDtBySortId, dictionary, dicProNameAndProValue, likeStrForDtSelect, ref propsStr, ref propsNameStr, ref inputStr, ref inputStrName, proId, current.Value, ref num);
                                }
                                else
                                {
                                    if (array != null && array.Length > 0 && DataConvert.ToInt(array[0]["isSellPro"]) != 1 && DataConvert.ToInt(array[0]["valueType"]) == 1)
                                    {
                                        string text = array[0]["id"].ToString();
                                        string[] array5 = current.Value.Trim().Split(new char[]
                                        {
                                            ' ',
                                            ','
                                        }, System.StringSplitOptions.RemoveEmptyEntries);
                                        if (array5 != null && array5.Length > 0)
                                        {
                                            string[] array6 = array5;
                                            for (int k = 0; k < array6.Length; k++)
                                            {
                                                string str = array6[k];
                                                DataRow[] array7 = propertyValueDtBySortId.Select(string.Concat(new string[]
                                                {
                                                    "propertyid='",
                                                    text,
                                                    "' and name = '",
                                                    DbUtil.OerateSpecialChar(str),
                                                    "'"
                                                }));
                                                if (array7 != null && array7.Length > 0)
                                                {
                                                    propsStr = propsStr + array7[0]["value"].ToString().Trim() + ";";
                                                    string text2 = propsNameStr;
                                                    propsNameStr = string.Concat(new string[]
                                                    {
                                                        text2,
                                                        array7[0]["value"].ToString().Trim(),
                                                        ":",
                                                        likeStrForDtSelect,
                                                        ":",
                                                        array7[0]["name"].ToString().Trim(),
                                                        ";"
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (array != null && array.Length > 0 && DataConvert.ToInt(array[0]["isSellPro"]) != 1 && DataConvert.ToInt(array[0]["valueType"]) == 2)
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
                                                    string text3 = array8[0]["keys"] + ":11111";
                                                    propsStr = propsStr + text3 + ";";
                                                    object obj = propsNameStr;
                                                    propsNameStr = string.Concat(new object[]
                                                    {
                                                        obj,
                                                        text3,
                                                        ":",
                                                        array8[0]["name"],
                                                        ":",
                                                        current.Value,
                                                        ";"
                                                    });
                                                    dictionary2[DbUtil.OerateSpecialChar(likeStrForDtSelect)] = array8[0]["keys"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                string text4 = dictionary2[DbUtil.OerateSpecialChar(likeStrForDtSelect)];
                                                DataRow[] array9 = propertyDtBySortId.Select(string.Concat(new string[]
                                                {
                                                    "name='",
                                                    DbUtil.OerateSpecialChar(likeStrForDtSelect),
                                                    "' and keys = '",
                                                    text4,
                                                    "'"
                                                }));
                                                if (array9 != null && array9.Length > 0)
                                                {
                                                    int num3 = DataConvert.ToInt(array9[0]["propertyType"]);
                                                    DataRow[] array10 = propertyDtBySortId.Select(string.Concat(new object[]
                                                    {
                                                        "name='",
                                                        DbUtil.OerateSpecialChar(likeStrForDtSelect),
                                                        "' and levels = ",
                                                        num3,
                                                        1
                                                    }));
                                                    if (array10 != null && array10.Length > 0)
                                                    {
                                                        string text5 = array10[0]["keys"] + ":11111";
                                                        propsStr = propsStr + text5 + ";";
                                                        object obj2 = propsNameStr;
                                                        propsNameStr = string.Concat(new object[]
                                                        {
                                                            obj2,
                                                            text5,
                                                            ":",
                                                            array10[0]["name"],
                                                            ":",
                                                            current.Value,
                                                            ";"
                                                        });
                                                        dictionary2[DbUtil.OerateSpecialChar(likeStrForDtSelect)] = array10[0]["keys"].ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Log.WriteLog(ex);
                        }
                    }
                    if (sellProInfoList != null && sellProInfoList.Count > 0)
                    {
                        foreach (SellProInfo current2 in sellProInfoList)
                        {
                            propsStr = propsStr + current2.Value.Trim() + ";";
                            if (current2.Value.IndexOf(":") > 0)
                            {
                                string str2 = current2.Value.Substring(0, current2.Value.IndexOf(":"));
                                DataRow[] array11 = propertyDtBySortId.Select("keys='" + DbUtil.OerateSpecialChar(str2) + "'");
                                if (array11 != null && array11.Length > 0)
                                {
                                    object obj3 = propsNameStr;
                                    propsNameStr = string.Concat(new object[]
                                    {
                                        obj3,
                                        current2.Value.Trim(),
                                        ":",
                                        array11[0]["name"],
                                        ":",
                                        current2.Name.Trim().Trim(),
                                        ";"
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void GetChildPropertyAndSetProps(DataTable dtProperty, DataTable dtPropertyValue, System.Collections.Generic.Dictionary<string, string> dicDistributionProValueAndProName, System.Collections.Generic.Dictionary<string, string> dicProNameAndProValue, string key, ref string propsStr, ref string propsNameStr, ref string inputStr, ref string inputStrName, string proId, string value, ref int keyIndex)
        {
            if (dtPropertyValue != null && dtPropertyValue.Rows != null && dtPropertyValue.Rows.Count > 0)
            {
                DataRow[] array = dtPropertyValue.Select(string.Concat(new string[]
                {
                    "propertyid='",
                    proId,
                    "' and name = '",
                    DbUtil.OerateSpecialChar(value),
                    "'"
                }));
                if (array == null || array.Length <= 0)
                {
                    DataRow[] array2 = dtPropertyValue.Select("propertyid='" + proId + "'");
                    for (int i = 0; i < array2.Length; i++)
                    {
                        if (value.Contains(array2[i]["name"].ToString()))
                        {
                            value = array2[i]["name"].ToString();
                            break;
                        }
                    }
                    array = dtPropertyValue.Select(string.Concat(new string[]
                    {
                        "propertyid='",
                        proId,
                        "' and name = '",
                        DbUtil.OerateSpecialChar(value),
                        "'"
                    }));
                }
                if (array != null && array.Length > 0)
                {
                    propsStr = propsStr + array[0]["value"].ToString().Trim() + ";";
                    string text = propsNameStr;
                    propsNameStr = string.Concat(new string[]
                    {
                        text,
                        array[0]["value"].ToString().Trim(),
                        ":",
                        key,
                        ":",
                        array[0]["name"].ToString().Trim(),
                        ";"
                    });
                    if (DataConvert.ToBoolean(array[0]["hasChildProperty"]))
                    {
                        string str = array[0]["value"].ToString();
                        DataRow[] array3 = dtProperty.Select("parentPropertyValue='" + DbUtil.OerateSpecialChar(str) + "'");
                        if (array3 != null && array3.Length > 0)
                        {
                            key = array3[0]["name"].ToString();
                            if (dicProNameAndProValue.ContainsKey(key) && !dicDistributionProValueAndProName.ContainsKey(dicProNameAndProValue[key] + key))
                            {
                                proId = array3[0]["id"].ToString();
                                value = dicProNameAndProValue[key];
                                dicDistributionProValueAndProName[value + key] = key;
                                GatherTaobaoUseWebService.GetChildPropertyAndSetProps(dtProperty, dtPropertyValue, dicDistributionProValueAndProName, dicProNameAndProValue, key, ref propsStr, ref propsNameStr, ref inputStr, ref inputStrName, proId, value, ref keyIndex);
                            }
                        }
                    }
                }
                if (array == null || array.Length <= 0)
                {
                    array = dtPropertyValue.Select(string.Concat(new string[]
                    {
                        "propertyid='",
                        proId,
                        "' and name = '",
                        DbUtil.OerateSpecialChar("其他"),
                        "'"
                    }));
                    if (array != null && array.Length > 0)
                    {
                        string text2 = array[0]["value"].ToString().Substring(0, array[0]["value"].ToString().IndexOf(':'));
                        string text3 = text2 + ":" + keyIndex;
                        propsStr = propsStr + text3 + ";";
                        string text4 = propsNameStr;
                        propsNameStr = string.Concat(new string[]
                        {
                            text4,
                            text3,
                            ":",
                            key,
                            ":",
                            value,
                            ";"
                        });
                        inputStr = inputStr + text2 + ",";
                        inputStrName = inputStrName + value + ",";
                        keyIndex++;
                        if (DataConvert.ToBoolean(array[0]["hasChildProperty"]))
                        {
                            string str2 = array[0]["value"].ToString();
                            DataRow[] array4 = dtProperty.Select("parentPropertyValue='" + DbUtil.OerateSpecialChar(str2) + "'");
                            if (array4 != null && array4.Length > 0)
                            {
                                DataRow[] array5 = array4;
                                for (int j = 0; j < array5.Length; j++)
                                {
                                    DataRow dataRow = array5[j];
                                    key = dataRow["name"].ToString();
                                    if (dicProNameAndProValue.ContainsKey(key) && !dicDistributionProValueAndProName.ContainsKey(dicProNameAndProValue[key] + key))
                                    {
                                        proId = dataRow["id"].ToString();
                                        value = dicProNameAndProValue[key];
                                        dicDistributionProValueAndProName[value + key] = key;
                                        GatherTaobaoUseWebService.GetChildPropertyAndSetProps(dtProperty, dtPropertyValue, dicDistributionProValueAndProName, dicProNameAndProValue, key, ref propsStr, ref propsNameStr, ref inputStr, ref inputStrName, proId, value, ref keyIndex);
                                    }
                                }
                            }
                        }
                    }
                }
                if (dtProperty != null && dtProperty.Rows.Count > 0 && (array == null || array.Length <= 0))
                {
                    DataRow[] array6 = dtProperty.Select("id='" + proId + "'");
                    if (array6 != null && array6.Length > 0)
                    {
                        propsStr = propsStr + array6[0][17].ToString().Trim() + ";";
                        string text5 = propsNameStr;
                        propsNameStr = string.Concat(new string[]
                        {
                            text5,
                            array6[0][17].ToString().Trim(),
                            ":",
                            key,
                            ":",
                            value,
                            ";"
                        });
                    }
                }
            }
        }
        private ItemGetResponse GetItemGetResponseByOnlinekey(string onlineKey, int fromType)
        {
            ItemGetResponse itemGetResponse = new ItemGetResponse();
            string text = string.Format("http://item.taobao.com/item.htm?id={0}", new object[]
            {
                onlineKey
            });
            try
            {
                int num = 1;
                string responseResultBody = this.GetResponseResultBody(text, System.Text.Encoding.GetEncoding("gb2312"), "GET", true, text, ref num);
                if (!string.IsNullOrEmpty(responseResultBody))
                {
                    string itemName = this.GetItemName(responseResultBody);
                    string itemCid = this.GetItemCid(responseResultBody);
                    string itemContent = this.GetItemContent(responseResultBody, text);
                    Item item = new Item();
                    item.ApproveStatus = "onsale";
                    item.Cid = DataConvert.ToLong(itemCid);
                    item.Desc = itemContent;
                    item.DetailUrl = text;
                    item.EmsFee = "0.00";
                    item.ExpressFee = "0.00";
                    item.FreightPayer = "buyer";
                    item.HasDiscount = false;
                    item.HasInvoice = false;
                    item.HasShowcase = true;
                    item.HasWarranty = true;
                    item.IsTiming = false;
                    System.Collections.Generic.List<ItemImg> itemImgList = this.GetItemImgList(responseResultBody);
                    item.ItemImgs = itemImgList;
                    item.NumIid = this.GetItemNumIid(responseResultBody);
                    item.PostFee = "0.00";
                    item.PostageId = 0L;
                    System.Collections.Generic.IList<SellProInfo> list = new System.Collections.Generic.List<SellProInfo>();
                    System.Collections.Generic.List<PropImg> proImgList = this.GetProImgList(responseResultBody, list);
                    this.SetSizeGroupList(responseResultBody, list);
                    item.PropImgs = proImgList;
                    System.Collections.Generic.Dictionary<string, string> itemProperty = this.GetItemProperty(responseResultBody);
                    string empty = string.Empty;
                    string empty2 = string.Empty;
                    string empty3 = string.Empty;
                    string empty4 = string.Empty;
                    if (fromType == 0)
                    {
                        GatherTaobaoUseWebService.GetPropertyStrForLocalSnatch(itemProperty, list, itemCid, out empty, out empty2, out empty3, out empty4);
                    }
                    else
                    {
                        GatherTaobaoUseWebService.GetPropertyStrForCloudSnatch(itemProperty, list, out empty, out empty2);
                    }
                    FoodSecurity foodSecurity = this.GetFoodSecurity(itemProperty);
                    item.FoodSecurity = foodSecurity;
                    item.Props = empty;
                    item.PropsName = empty2;
                    item.InputPids = empty3;
                    item.InputStr = empty4;
                    item.SellPoint = this.GetItemSellPoint(responseResultBody);
                    item.SellPromise = true;
                    item.StuffStatus = "new";
                    item.Title = itemName;
                    item.Type = "fixed";
                    item.ValidThru = 7L;
                    item.Violation = false;
                    long num2 = 0L;
                    string text2 = string.Empty;
                    System.Collections.Generic.List<Sku> skuList = this.GetSkuList(onlineKey, out num2, out text2);
                    if (skuList == null || skuList.Count <= 0)
                    {
                        text2 = this.GetItemPrice(responseResultBody);
                        if (string.IsNullOrEmpty(text2))
                        {
                            string pattern = "(?is)<em class=\"tb-rmb-num\">(?<price>.*?)</em>";
                            Regex regex = new Regex(pattern);
                            Match match = regex.Match(responseResultBody);
                            if (match != null && match.Groups["price"].Success)
                            {
                                text2 = match.Groups["price"].Value;
                            }
                        }
                        try
                        {
                            num2 = DataConvert.ToLong(this.GetItemNum(responseResultBody));
                            if (num2 == 0L)
                            {
                                num2 = 9999L;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Log.WriteLog(string.Concat(new object[]
                            {
                                "商品名称：",
                                itemName,
                                "获取商品数量是出错",
                                ex
                            }));
                        }
                    }
                    item.Skus = skuList;
                    item.Location = null;
                    item.Num = num2;
                    item.Price = text2;
                    itemGetResponse.Item = item;
                }
                else
                {
                    itemGetResponse.ErrCode = "9999";
                    itemGetResponse.ErrMsg = "无法通过" + text + "获取商品信息。";
                }
            }
            catch (System.Exception ex2)
            {
                Log.WriteLog("下载淘宝商品信息出现异常", ex2);
                itemGetResponse.ErrCode = "9998";
                itemGetResponse.ErrMsg = "下载淘宝商品" + text + "失败，" + ex2.Message;
            }
            return itemGetResponse;
        }
        //private ItemGetResponse GetItemGetMobileResponseByOnlinekey(string onlineKey, int fromType, bool snatchPromotionPrice)
        //{
        //    ItemGetResponse itemGetResponse = new ItemGetResponse();
        //    try
        //    {
        //        string detailUrl = string.Format("http://item.taobao.com/item.htm?id={0}", new object[]
        //        {
        //            onlineKey
        //        });
        //        string text = string.Format("http://hws.m.taobao.com/cache/wdetail/5.0/?id={0}&ttid=2013@taobao_h5_1.0.0", new object[]
        //        {
        //            onlineKey
        //        });
        //        int num = 1;
        //        string responseResultBody = this.GetResponseResultBody(text, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text, ref num);
        //        string text2 = "onsale";
        //        text2 = this.GetOnline(responseResultBody);
        //        string empty = string.Empty;
        //        long num2 = 0L;
        //        bool flag = true;
        //        if (!string.IsNullOrEmpty(responseResultBody))
        //        {
        //            flag = false;
        //        }
        //        bool flag2 = false;
        //        ItemTaobaoEntity itemTaobaoEntity = this.GetItemTaobaoEntity(responseResultBody, ref empty, ref num2, snatchPromotionPrice, onlineKey, ref flag2);
        //        if (itemTaobaoEntity == null && flag2)
        //        {
        //            itemGetResponse.ErrCode = "998";
        //            ItemGetResponse result = itemGetResponse;
        //            return result;
        //        }
        //        if (itemTaobaoEntity == null)
        //        {
        //            num = 1;
        //            responseResultBody = this.GetResponseResultBody(text, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text, ref num);
        //            if (!string.IsNullOrEmpty(responseResultBody))
        //            {
        //                flag = false;
        //            }
        //            itemTaobaoEntity = this.GetItemTaobaoEntity(responseResultBody, ref empty, ref num2, snatchPromotionPrice, onlineKey, ref flag2);
        //            if (itemTaobaoEntity == null)
        //            {
        //                num = 1;
        //                responseResultBody = this.GetResponseResultBody(text, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text, ref num);
        //                if (!string.IsNullOrEmpty(responseResultBody))
        //                {
        //                    flag = false;
        //                }
        //                itemTaobaoEntity = this.GetItemTaobaoEntity(responseResultBody, ref empty, ref num2, snatchPromotionPrice, onlineKey, ref flag2);
        //            }
        //        }
        //        bool flag3 = false;
        //        if (!flag)
        //        {
        //            if (itemTaobaoEntity == null)
        //            {
        //                flag = true;
        //                flag3 = true;
        //            }
        //            else
        //            {
        //                if (itemTaobaoEntity.Data == null || itemTaobaoEntity.Data.ItemInfoModel == null || string.IsNullOrEmpty(itemTaobaoEntity.Data.ItemInfoModel.Title))
        //                {
        //                    flag = true;
        //                    flag3 = true;
        //                }
        //            }
        //        }
        //        if (flag && (text2 != "onsale" || fromType == 1))
        //        {
        //            flag = false;
        //        }
        //        bool flag4 = DataConvert.ToBoolean(DataHelper.GetUserConfig("AppConfig", this._toolCode.ToUpper(), "SnacthFilter", "false"));
        //        if (text2 != "onsale" && flag4)
        //        {
        //            itemGetResponse.ErrCode = "19";
        //            itemGetResponse.ErrMsg = "未抓取到商品，该商品已删除或已下架";
        //            ItemGetResponse result = itemGetResponse;
        //            return result;
        //        }
        //        if (text2 != "onsale")
        //        {
        //            if (itemTaobaoEntity != null && itemTaobaoEntity.SkuList != null && itemTaobaoEntity.SkuList.Count > 0)
        //            {
        //                long num3 = 0L;
        //                foreach (Sku current in itemTaobaoEntity.SkuList)
        //                {
        //                    current.Quantity = 100L;
        //                    num3 += current.Quantity;
        //                }
        //                num2 = num3;
        //            }
        //            else
        //            {
        //                num2 = 100L;
        //            }
        //        }
        //        bool flag5 = DataConvert.ToBoolean(DataHelper.GetUserConfig("AppConfig", this._toolCode.ToUpper(), "WangWangFilter", "false"));
        //        if (flag5)
        //        {
        //            System.Collections.Generic.IList<UserSetData> userSetDataList = this.UserSetDataService.GetUserSetDataList(1, 1);
        //            if (userSetDataList != null && userSetDataList.Count > 0 && itemTaobaoEntity != null && itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.Seller != null)
        //            {
        //                string nick = itemTaobaoEntity.Data.Seller.Nick;
        //                if (!string.IsNullOrEmpty(nick))
        //                {
        //                    for (int i = 0; i < userSetDataList.Count; i++)
        //                    {
        //                        if (nick == userSetDataList[i].Name)
        //                        {
        //                            ItemGetResponse result = itemGetResponse;
        //                            return result;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (!flag)
        //        {
        //            if (flag3)
        //            {
        //                if (text2 != "onsale")
        //                {
        //                    itemGetResponse.ErrCode = "19";
        //                    itemGetResponse.ErrMsg = "未抓取到商品，该商品已删除或已下架";
        //                }
        //                else
        //                {
        //                    itemGetResponse.ErrCode = "18";
        //                    itemGetResponse.ErrMsg = "未抓取到商品，该商品有可能已删除或下架";
        //                }
        //                ItemGetResponse result = itemGetResponse;
        //                return result;
        //            }
        //            string title = string.Empty;
        //            string text3 = string.Empty;
        //            string o = string.Empty;
        //            string text4 = string.Empty;
        //            string wirelessDesc = string.Empty;
        //            string text5 = string.Empty;
        //            string pcDescUrl = string.Empty;
        //            string arg_3B3_0 = string.Empty;
        //            string arg_3B9_0 = string.Empty;
        //            string loctionStr = string.Empty;
        //            string nick2 = string.Empty;
        //            string arg_3CD_0 = string.Empty;
        //            string empty2 = string.Empty;
        //            string arg_3DA_0 = string.Empty;
        //            string stuffStatus = string.Empty;
        //            string freightPayer = string.Empty;
        //            string postFee = string.Empty;
        //            string expressFee = string.Empty;
        //            string emsFee = string.Empty;
        //            System.Collections.Generic.List<ItemImg> itemImgs = new System.Collections.Generic.List<ItemImg>();
        //            System.Collections.Generic.List<PropImg> propImgs = new System.Collections.Generic.List<PropImg>();
        //            System.Collections.Generic.IList<SellProInfo> sellProInfoList = new System.Collections.Generic.List<SellProInfo>();
        //            System.Collections.Generic.List<Sku> list = null;
        //            System.Collections.Generic.Dictionary<string, string> dicProNameAndProValue = null;
        //            if (itemTaobaoEntity != null && itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.ItemInfoModel != null)
        //            {
        //                title = itemTaobaoEntity.Data.ItemInfoModel.Title;
        //                text3 = DataConvert.ToString(itemTaobaoEntity.Data.ItemInfoModel.CategoryId);
        //                loctionStr = itemTaobaoEntity.Data.ItemInfoModel.Location;
        //                o = itemTaobaoEntity.Data.ItemInfoModel.ItemId;
        //                freightPayer = itemTaobaoEntity.Data.ItemInfoModel.FreightPayer;
        //                postFee = itemTaobaoEntity.Data.ItemInfoModel.PostFee;
        //                expressFee = itemTaobaoEntity.Data.ItemInfoModel.ExpressFee;
        //                emsFee = itemTaobaoEntity.Data.ItemInfoModel.EmsFee;
        //                itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "二手";
        //                if (itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "二手")
        //                {
        //                    stuffStatus = "second";
        //                }
        //                else
        //                {
        //                    if (itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "个人闲置")
        //                    {
        //                        stuffStatus = "unused";
        //                    }
        //                    else
        //                    {
        //                        stuffStatus = "new";
        //                    }
        //                }
        //                if (itemTaobaoEntity.Data.Seller != null)
        //                {
        //                    Seller seller = itemTaobaoEntity.Data.Seller;
        //                    string arg_571_0 = seller.ShopId;
        //                    nick2 = seller.Nick;
        //                    itemTaobaoEntity.Data.ItemInfoModel.ItemTypeName == "tmall";
        //                }
        //                if (itemTaobaoEntity.Data.ItemInfoModel.picsPath != null && itemTaobaoEntity.Data.ItemInfoModel.picsPath.Count > 0)
        //                {
        //                    itemImgs = this.GetItemImgList(itemTaobaoEntity.Data.ItemInfoModel.picsPath);
        //                }
        //                if (itemTaobaoEntity.Data.DescInfo != null && !string.IsNullOrEmpty(itemTaobaoEntity.Data.DescInfo.PcDescUrl))
        //                {
        //                    pcDescUrl = itemTaobaoEntity.Data.DescInfo.PcDescUrl;
        //                    string arg_626_0 = itemTaobaoEntity.Data.DescInfo.FullDescUrl;
        //                    text5 = itemTaobaoEntity.Data.DescInfo.H5DescUrl;
        //                    string arg_64B_0 = itemTaobaoEntity.Data.DescInfo.BriefDescUrl;
        //                    text4 = this.GetpcDescContent(pcDescUrl);
        //                    wirelessDesc = this.GetWirelessDescContent(text5, text4);
        //                }
        //                if (itemTaobaoEntity.Data.SkuModel != null && itemTaobaoEntity.Data.SkuModel.SkuProps != null && itemTaobaoEntity.Data.SkuModel.SkuProps.Count > 0)
        //                {
        //                    System.Collections.Generic.List<SkuPropMobile> skuProps = itemTaobaoEntity.Data.SkuModel.SkuProps;
        //                    propImgs = this.GetMobileProImgList(skuProps, sellProInfoList);
        //                }
        //                if (itemTaobaoEntity.Data.Props != null && itemTaobaoEntity.Data.Props.Count > 0)
        //                {
        //                    dicProNameAndProValue = this.GetMobileItemProperty(itemTaobaoEntity.Data.Props, ref empty2);
        //                }
        //                if (itemTaobaoEntity.SkuList != null && itemTaobaoEntity.SkuList.Count > 0)
        //                {
        //                    list = itemTaobaoEntity.SkuList;
        //                }
        //            }
        //            Item item = new Item();
        //            item.ApproveStatus = text2;
        //            item.Cid = DataConvert.ToLong(text3);
        //            item.Title = title;
        //            item.Desc = text4;
        //            item.Nick = nick2;
        //            item.FreightPayer = freightPayer;
        //            item.ExpressFee = expressFee;
        //            item.EmsFee = emsFee;
        //            item.PostFee = postFee;
        //            item.DetailUrl = detailUrl;
        //            item.WirelessDesc = wirelessDesc;
        //            item.WapDetailUrl = text5;
        //            item.HasDiscount = false;
        //            item.HasInvoice = false;
        //            item.HasShowcase = true;
        //            item.HasWarranty = true;
        //            item.IsTiming = false;
        //            item.ItemImgs = itemImgs;
        //            item.NumIid = DataConvert.ToLong(o);
        //            item.PostageId = 0L;
        //            item.PropImgs = propImgs;
        //            string empty3 = string.Empty;
        //            string empty4 = string.Empty;
        //            string empty5 = string.Empty;
        //            string empty6 = string.Empty;
        //            if (fromType == 0)
        //            {
        //                GatherTaobaoUseWebService.GetPropertyStrForLocalSnatch(dicProNameAndProValue, sellProInfoList, text3, out empty3, out empty4, out empty5, out empty6);
        //            }
        //            else
        //            {
        //                GatherTaobaoUseWebService.GetPropertyStrForCloudSnatch(dicProNameAndProValue, sellProInfoList, out empty3, out empty4);
        //            }
        //            FoodSecurity foodSecurity = this.GetFoodSecurity(dicProNameAndProValue);
        //            if (foodSecurity != null && (foodSecurity.PrdLicenseNo != null || foodSecurity.DesignCode != null || foodSecurity.Factory != null || foodSecurity.FactorySite != null || foodSecurity.Contact != null || foodSecurity.Mix != null || foodSecurity.PlanStorage != null || foodSecurity.Period != null || foodSecurity.FoodAdditive != "无" || foodSecurity.HealthProductNo != null || foodSecurity.Supplier != null))
        //            {
        //                string empty7 = string.Empty;
        //                string empty8 = string.Empty;
        //                this.FoodSecurityDate(onlineKey, ref empty7, ref empty8);
        //                foodSecurity.ProductDateStart = empty7;
        //                foodSecurity.ProductDateEnd = empty8;
        //                foodSecurity.StockDateStart = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        //                foodSecurity.StockDateEnd = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        //            }
        //            item.Props = empty3;
        //            item.PropsName = empty4;
        //            item.InputPids = empty5;
        //            item.InputStr = empty6;
        //            item.FoodSecurity = foodSecurity;
        //            item.SellPromise = true;
        //            item.StuffStatus = "new";
        //            item.Type = "fixed";
        //            item.ValidThru = 7L;
        //            item.Violation = false;
        //            item.Barcode = empty2;
        //            item.Skus = list;
        //            item.Location = this.GetLoction(loctionStr);
        //            item.Num = num2;
        //            bool flag6 = DataConvert.ToBoolean(DataHelper.GetUserConfig("AppConfig", this._toolCode.ToUpper(), "SnacthCPrice", "false"));
        //            if (flag6)
        //            {
        //                item.Price = this.GetGoodsCPrice(list);
        //                if (DataConvert.ToDecimal(item.Price) <= 0m)
        //                {
        //                    item.Price = empty;
        //                }
        //            }
        //            else
        //            {
        //                item.Price = empty;
        //            }
        //            item.StuffStatus = stuffStatus;
        //            itemGetResponse.Item = item;
        //        }
        //        else
        //        {
        //            itemGetResponse.ErrCode = "9999";
        //            itemGetResponse.ErrMsg = "无法通过" + text + "获取商品信息。";
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Log.WriteLog("下载淘宝商品信息出现异常", ex);
        //    }
        //    return itemGetResponse;
        //}
        private string GetGoodsCPrice(System.Collections.Generic.List<Sku> skuList)
        {
            string result = string.Empty;
            System.Collections.Generic.List<decimal> list = new System.Collections.Generic.List<decimal>();
            if (skuList != null && skuList.Count > 0)
            {
                foreach (Sku current in skuList)
                {
                    list.Add(DataConvert.ToDecimal(current.Price));
                }
            }
            list.Sort();
            if (list.Count > 0)
            {
                result = list[list.Count - 1].ToString();
            }
            return result;
        }
        private long GetMobileNums(System.Collections.Generic.List<Sku> skuList, string rsContent)
        {
            long num = 0L;
            if (skuList != null && skuList.Count > 0)
            {
                foreach (Sku current in skuList)
                {
                    num += current.Quantity;
                }
            }
            if (num <= 0L)
            {
                string pattern = "\\\\\"quantity\\\\\":\\\\\"?(?<quantity>.*?)\\\\\"?";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(rsContent);
                if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["quantity"].Value))
                {
                    num = DataConvert.ToLong(match.Groups["quantity"].Value);
                }
            }
            return num;
        }
        private string GetMobilePrice(System.Collections.Generic.List<Sku> skuList, string rsContent, bool isSnatchpromoPrice, string cxPrice, string itemId)
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
                using (System.Collections.Generic.List<Sku>.Enumerator enumerator = skuList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Sku current = enumerator.Current;
                        if (string.IsNullOrEmpty(text) && current.Quantity > 0L)
                        {
                            text = current.Price;
                        }
                        else
                        {
                            if (DataConvert.ToDecimal(text) > DataConvert.ToDecimal(current.Price) && current.Quantity > 0L)
                            {
                                text = current.Price;
                            }
                        }
                    }
                    goto IL_BA;
                }
            }
            text = cxPrice;
            IL_BA:
            if (DataConvert.ToDecimal(text) <= 0m)
            {
                string pattern2 = "\\\\\"price\\\\\":\\\\\"(?<price>.*?)\\\\\"";
                Regex regex2 = new Regex(pattern2);
                MatchCollection matchCollection = regex2.Matches(rsContent);
                if (matchCollection != null && matchCollection.Count > 0)
                {
                    foreach (Match match2 in matchCollection)
                    {
                        if (!isSnatchpromoPrice)
                        {
                            if (num == 0m)
                            {
                                num = DataConvert.ToDecimal(match2.Groups["price"].Value);
                            }
                            else
                            {
                                if (DataConvert.ToDecimal(num) < DataConvert.ToDecimal(DataConvert.ToDecimal(match2.Groups["price"].Value)))
                                {
                                    num = DataConvert.ToDecimal(match2.Groups["price"].Value);
                                }
                            }
                        }
                        else
                        {
                            if (num == 0m)
                            {
                                num = DataConvert.ToDecimal(match2.Groups["price"].Value);
                            }
                            else
                            {
                                if (DataConvert.ToDecimal(num) > DataConvert.ToDecimal(DataConvert.ToDecimal(match2.Groups["price"].Value)))
                                {
                                    num = DataConvert.ToDecimal(match2.Groups["price"].Value);
                                }
                            }
                        }
                    }
                }
                text = DataConvert.ToString(num);
                if (text == "0")
                {
                    int num2 = 1;
                    string text2 = "{\"itemNumId\":\"" + itemId + "\"}";
                    text2 = Uri.EscapeDataString(text2);
                    string text3 = "http://acs.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data=" + text2;
                    string input = this.GetResponseResultBody(text3, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text3, ref num2);
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
                        foreach (Match match4 in matchCollection)
                        {
                            if (!isSnatchpromoPrice)
                            {
                                if (num == 0m)
                                {
                                    num = DataConvert.ToDecimal(match4.Groups["priceText"].Value);
                                }
                                else
                                {
                                    if (DataConvert.ToDecimal(num) < DataConvert.ToDecimal(DataConvert.ToDecimal(match4.Groups["priceText"].Value)))
                                    {
                                        num = DataConvert.ToDecimal(match4.Groups["priceText"].Value);
                                    }
                                }
                            }
                            else
                            {
                                if (num == 0m)
                                {
                                    num = DataConvert.ToDecimal(match4.Groups["priceText"].Value);
                                }
                                else
                                {
                                    if (DataConvert.ToDecimal(num) > DataConvert.ToDecimal(DataConvert.ToDecimal(match4.Groups["priceText"].Value)))
                                    {
                                        num = DataConvert.ToDecimal(match4.Groups["priceText"].Value);
                                    }
                                }
                            }
                        }
                    }
                    text = DataConvert.ToString(num);
                }
            }
            return text;
        }
        private System.Collections.Generic.IList<string> GetProvinceList()
        {
            return new System.Collections.Generic.List<string>
            {
                "河北省",
                "河北",
                "山西省",
                "山西",
                "内蒙古自治区",
                "内蒙古",
                "辽宁省",
                "辽宁",
                "吉林省",
                "吉林",
                "黑龙江省",
                "黑龙江",
                "江苏省",
                "江苏",
                "浙江省",
                "浙江",
                "安徽省",
                "安徽",
                "福建省",
                "福建",
                "江西省",
                "江西",
                "山东省",
                "山东",
                "河南省",
                "河南",
                "湖北省",
                "湖北",
                "湖南省",
                "湖南",
                "广东省",
                "广东",
                "广西壮族自治区",
                "广西",
                "海南省",
                "海南",
                "四川省",
                "四川",
                "贵州省",
                "贵州",
                "云南省",
                "云南",
                "西藏自治区",
                "西藏",
                "陕西省",
                "陕西",
                "甘肃省",
                "甘肃",
                "青海省",
                "青海",
                "宁夏回族自治区",
                "宁夏",
                "新疆维吾尔自治区",
                "新疆",
                "台湾省"
            };
        }
        private Location GetLoction(string loctionStr)
        {
            string text = string.Empty;
            string text2 = string.Empty;
            System.Collections.Generic.IList<string> provinceList = this.GetProvinceList();
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
            return new Location
            {
                City = text2,
                State = text
            };
        }
        private System.Collections.Generic.Dictionary<string, string> GetMobileItemProperty(System.Collections.Generic.List<Prop> proList, ref string barcode)
        {
            System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
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
        private System.Collections.Generic.List<PropImg> GetMobileProImgList(System.Collections.Generic.List<SkuPropMobile> skuModelList, System.Collections.Generic.IList<SellProInfo> sellProInfoList)
        {
            System.Collections.Generic.List<PropImg> list = new System.Collections.Generic.List<PropImg>();
            foreach (SkuPropMobile current in skuModelList)
            {
                if (!string.IsNullOrEmpty(current.PropId) && current != null && current.Values != null && current.Values.Count > 0)
                {
                    System.Collections.Generic.List<Value> values = current.Values;
                    foreach (Value current2 in values)
                    {
                        SellProInfo sellProInfo = new SellProInfo();
                        if (!string.IsNullOrEmpty(current2.ImgUrl))
                        {
                            list.Add(new PropImg
                            {
                                Id = 0L,
                                Position = 0L,
                                Properties = current.PropId + ":" + current2.ValueId,
                                Url = current2.ImgUrl
                            });
                        }
                        sellProInfo.Name = current2.Name;
                        sellProInfo.Value = current.PropId + ":" + current2.ValueId;
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
                string responseResultBody = this.GetResponseResultBody(h5DescUrl, System.Text.Encoding.GetEncoding("gb2312"), "GET", true, h5DescUrl, ref num);
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
                        System.Collections.IEnumerator enumerator = matchCollection.GetEnumerator();
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
                            System.IDisposable disposable = enumerator as System.IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(text3) && flag)
                    {
                        regex = new Regex("(?is)((size=(?<maxSize>.*?)x[^<>].*?)|(img))>(?<contentInfo>.*?)<");
                        MatchCollection matchCollection2 = regex.Matches(text3);
                        if (matchCollection2 != null && matchCollection2.Count > 0)
                        {
                            foreach (Match match3 in matchCollection2)
                            {
                                if (!(match3.Groups["maxSize"].Value != "") || DataConvert.ToInt(match3.Groups["maxSize"].Value) >= 450)
                                {
                                    string value = match3.Groups["contentInfo"].Value;
                                    if (!value.Contains(".gif") && value.Contains("//"))
                                    {
                                        text = text + "<img>" + value.Replace("//", "http://") + "</img>";
                                    }
                                }
                            }
                        }
                        regex = new Regex("(?is)<txt>(?<txtInfo>.*?)</txt>");
                        MatchCollection matchCollection3 = regex.Matches(text3);
                        if (matchCollection3 == null || matchCollection3.Count <= 0)
                        {
                            goto IL_44B;
                        }
                        System.Collections.IEnumerator enumerator3 = matchCollection3.GetEnumerator();
                        try
                        {
                            while (enumerator3.MoveNext())
                            {
                                Match match4 = (Match)enumerator3.Current;
                                if (!match4.Groups["txtInfo"].Value.Trim().Equals("\\u00A0") && !match4.Groups["txtInfo"].Value.Trim().Equals("&nbsp;"))
                                {
                                    text2 += match4;
                                }
                            }
                            goto IL_44B;
                        }
                        finally
                        {
                            System.IDisposable disposable3 = enumerator3 as System.IDisposable;
                            if (disposable3 != null)
                            {
                                disposable3.Dispose();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(pcContentDesc))
                    {
                        regex = new Regex("(?is)src=\"(?<MPicUrl>.*?)\"");
                        MatchCollection matchCollection4 = regex.Matches(pcContentDesc);
                        if (matchCollection4 != null && matchCollection4.Count > 0)
                        {
                            foreach (Match match5 in matchCollection4)
                            {
                                string value2 = match5.Groups["MPicUrl"].Value;
                                if (!value2.Contains(".gif") && value2.Contains("http://"))
                                {
                                    text = text + "<img>" + value2 + "</img>";
                                }
                            }
                        }
                    }
                    IL_44B:
                    if (!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(text2))
                    {
                        result = "<wapDesc>" + text2 + text + "</wapDesc>";
                    }
                    else
                    {
                        result = string.Empty;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLog("下载商品手机详情出现异常", ex);
            }
            return result;
        }
        private string GetpcDescContent(string pcDescUrl)
        {
            return this.GetpcDescContent(pcDescUrl, 1);
        }
        private string GetpcDescContent(string pcDescUrl, int tryNum)
        {
            int num = 0;
            if (!pcDescUrl.StartsWith("http://") && !pcDescUrl.StartsWith("https://"))
            {
                pcDescUrl = "http://" + pcDescUrl;
            }
            string text = this.GetResponseResultBody(pcDescUrl, System.Text.Encoding.GetEncoding("gb2312"), "GET", true, pcDescUrl, ref num);
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
                        foreach (Match match2 in matchCollection)
                        {
                            if (match2 != null && !string.IsNullOrEmpty(match2.Groups["img"].Value) && !string.IsNullOrEmpty(match2.Groups["width"].Value) && DataConvert.ToInt(match2.Groups["width"].Value) > 750)
                            {
                                string value = match2.Groups["img"].Value;
                                string newValue = match2.Groups["img"].Value + " width=\"750px\" ";
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
                foreach (Match match3 in matchCollection2)
                {
                    string text2 = match3.Groups["isImg"].Value.ToLower();
                    if (text2.Contains("http://") || (text2.Contains("https://") && (text2.Contains(".jpg") || text2.Contains(".jpeg") || text2.Contains(".png") || text2.Contains(".gif"))))
                    {
                        text = text.Replace(match3.Value, "src=" + match3.Groups["isImg"].Value);
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
                System.Threading.Thread.Sleep(1000);
                return this.GetpcDescContent(pcDescUrl, tryNum);
            }
            if (!string.IsNullOrEmpty(text) && !text.Contains("https:"))
            {
                text = text.Replace("http", "https");
            }
            return text;
        }
        private string GetNewPcDescContent(string fullDescUrl)
        {
            int num = 0;
            if (!fullDescUrl.StartsWith("http://") && !fullDescUrl.StartsWith("https://"))
            {
                fullDescUrl = "http://" + fullDescUrl;
            }
            string text = this.GetResponseResultBody(fullDescUrl, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, fullDescUrl, ref num);
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    string text2 = string.Empty;
                    JObject jObject = (JObject)JsonConvert.DeserializeObject(text);
                    JToken jToken = jObject["data"];
                    JValue jValue = (JValue)jToken["desc"];
                    text2 = jValue.Value.ToString();
                    if (!string.IsNullOrEmpty(text2))
                    {
                        string pattern = "(?is)<body[^>]*?>(?<itemContent>.*?)</body>";
                        Regex regex = new Regex(pattern);
                        Match match = regex.Match(text2);
                        if (match != null && match.Groups["itemContent"].Success)
                        {
                            text = match.Groups["itemContent"].Value;
                            pattern = "(?is)<script[^>]*?>.*?</script>";
                            regex = new Regex(pattern);
                            text = regex.Replace(text, string.Empty);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Log.WriteLog("下载淘宝商品描述出现异常", ex);
                }
            }
            return text;
        }
        private System.Collections.Generic.List<ItemImg> GetItemImgList(System.Collections.Generic.List<string> picsPath)
        {
            System.Collections.Generic.List<ItemImg> list = new System.Collections.Generic.List<ItemImg>();
            if (picsPath != null && picsPath.Count > 0)
            {
                for (int i = 0; i < picsPath.Count; i++)
                {
                    ItemImg itemImg = new ItemImg();
                    itemImg.Position = (long)i;
                    if (picsPath[i].Contains("img01.taobaocdn"))
                    {
                        picsPath[i] = picsPath[i].Replace("img01.taobaocdn", "gd1.alicdn");
                    }
                    itemImg.Url = picsPath[i];
                    list.Add(itemImg);
                }
            }
            return list;
        }
        private ItemTaobaoEntity GetItemTaobaoEntity(string rsContent, ref string price, ref long num, bool isSnatchpromoPrice, string onlineKey, ref bool isLimit)
        {
            if (string.IsNullOrEmpty(rsContent))
            {
                return null;
            }
            ItemTaobaoEntity itemTaobaoEntity = null;
            string defDyn = string.Empty;
            try
            {
                itemTaobaoEntity = JsonConvert.DeserializeObject<ItemTaobaoEntity>(rsContent);
                if (itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.ItemInfoModel != null)
                {
                    try
                    {
                        this.SetFreightMessage(rsContent, itemTaobaoEntity.Data.ItemInfoModel);
                    }
                    catch (System.Exception ex)
                    {
                        Log.WriteLog("处理运费信息失败", ex);
                    }
                }
                System.Collections.Generic.List<Sku> skuList = null;
                if (itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.SkuModel != null)
                {
                    if (itemTaobaoEntity.Data.ApiStack != null && itemTaobaoEntity.Data.ApiStack.Count > 0 && !string.IsNullOrEmpty(itemTaobaoEntity.Data.ApiStack[0].Value))
                    {
                        defDyn = itemTaobaoEntity.Data.ApiStack[0].Value;
                    }
                    else
                    {
                        if (itemTaobaoEntity.Data.Extras != null)
                        {
                            defDyn = itemTaobaoEntity.Data.Extras.DefDyn;
                        }
                    }
                    bool isTaobao = false;
                    if (itemTaobaoEntity.Data.ItemInfoModel != null && !string.IsNullOrEmpty(itemTaobaoEntity.Data.ItemInfoModel.ItemUrl) && itemTaobaoEntity.Data.ItemInfoModel.ItemUrl.Contains("taobao"))
                    {
                        isTaobao = true;
                    }
                    skuList = this.GetMobileSkuList(rsContent, defDyn, itemTaobaoEntity.Data.SkuModel, ref price, ref num, isSnatchpromoPrice);
                    if (itemTaobaoEntity.Data.ItemInfoModel != null && !string.IsNullOrEmpty(itemTaobaoEntity.Data.ItemInfoModel.ItemId) && isSnatchpromoPrice)
                    {
                        skuList = this.GetMobilePromoPriceSkuList(itemTaobaoEntity.Data.ItemInfoModel.ItemId, skuList, ref price, ref num, isSnatchpromoPrice, isTaobao);
                    }
                    itemTaobaoEntity.SkuList = skuList;
                }
                price = this.GetMobilePrice(skuList, rsContent, isSnatchpromoPrice, price, itemTaobaoEntity.Data.ItemInfoModel.ItemId);
                num = this.GetMobileNums(skuList, rsContent);
            }
            catch (System.Exception)
            {
                ItemTaobaoEntity result;
                if (rsContent.Contains("亲，小二正忙，滑动一下马上回来") || rsContent.Contains("https://www.taobao.com/markets"))
                {
                    isLimit = true;
                    result = null;
                    return result;
                }
                rsContent = this.GetHtmlCode(onlineKey);
                itemTaobaoEntity = JsonConvert.DeserializeObject<ItemTaobaoEntity>(rsContent);
                if (itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.ItemInfoModel != null)
                {
                    try
                    {
                        this.SetFreightMessage(rsContent, itemTaobaoEntity.Data.ItemInfoModel);
                    }
                    catch (System.Exception ex2)
                    {
                        Log.WriteLog("处理运费信息失败", ex2);
                    }
                }
                System.Collections.Generic.List<Sku> skuList2 = null;
                if (itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.SkuModel != null)
                {
                    if (itemTaobaoEntity.Data.ApiStack != null && itemTaobaoEntity.Data.ApiStack.Count > 0)
                    {
                        defDyn = itemTaobaoEntity.Data.ApiStack[0].Value;
                    }
                    else
                    {
                        if (itemTaobaoEntity.Data.Extras != null)
                        {
                            defDyn = itemTaobaoEntity.Data.Extras.DefDyn;
                        }
                    }
                    bool isTaobao2 = false;
                    if (itemTaobaoEntity.Data.ItemInfoModel != null && !string.IsNullOrEmpty(itemTaobaoEntity.Data.ItemInfoModel.ItemUrl) && itemTaobaoEntity.Data.ItemInfoModel.ItemUrl.Contains("taobao"))
                    {
                        isTaobao2 = true;
                    }
                    skuList2 = this.GetMobileSkuList(rsContent, defDyn, itemTaobaoEntity.Data.SkuModel, ref price, ref num, isSnatchpromoPrice);
                    if (itemTaobaoEntity.Data.ItemInfoModel != null && !string.IsNullOrEmpty(itemTaobaoEntity.Data.ItemInfoModel.ItemId) && isSnatchpromoPrice)
                    {
                        skuList2 = this.GetMobilePromoPriceSkuList(itemTaobaoEntity.Data.ItemInfoModel.ItemId, skuList2, ref price, ref num, isSnatchpromoPrice, isTaobao2);
                    }
                    itemTaobaoEntity.SkuList = skuList2;
                }
                price = this.GetMobilePrice(skuList2, rsContent, isSnatchpromoPrice, price, itemTaobaoEntity.Data.ItemInfoModel.ItemId);
                num = this.GetMobileNums(skuList2, rsContent);
                result = itemTaobaoEntity;
                return result;
            }
            return itemTaobaoEntity;
        }
        private void SetFreightMessage(string rsContent, ItemInfoModel itemInfoModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(rsContent))
                {
                    string text = string.Empty;
                    string pattern = "(?is)\\\\\"deliveryFees\\\\\":\\[(?<deliveryFees>.*?)\\]";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(rsContent);
                    if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["deliveryFees"].Value))
                    {
                        text = match.Groups["deliveryFees"].Value.Replace("\\", string.Empty).Trim();
                        text = "[" + text + "]";
                        if (!string.IsNullOrEmpty(text))
                        {
                            System.Collections.Generic.List<string> list = JsonConvert.DeserializeObject<System.Collections.Generic.List<string>>(text);
                            if (list != null && list.Count > 0)
                            {
                                string freightPayer = "buyer";
                                foreach (string current in list)
                                {
                                    if (current.Contains("平邮"))
                                    {
                                        itemInfoModel.PostFee = current.Replace("平邮", string.Empty).Trim();
                                    }
                                    else
                                    {
                                        if (current.Contains("快递"))
                                        {
                                            itemInfoModel.ExpressFee = current.Replace("快递", string.Empty).Trim();
                                        }
                                        else
                                        {
                                            if (current.Contains("EMS"))
                                            {
                                                itemInfoModel.EmsFee = current.Replace("EMS", string.Empty).Trim();
                                            }
                                            else
                                            {
                                                if (current.Contains("卖家包邮"))
                                                {
                                                    freightPayer = "seller";
                                                }
                                            }
                                        }
                                    }
                                }
                                itemInfoModel.FreightPayer = freightPayer;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLog(ex);
            }
        }
        private System.Collections.Generic.List<Sku> GetMobileSkuList(string rsContent, string defDyn, SkuModel skuModel, ref string price, ref long num, bool isSnatchpromoPrice)
        {
            System.Collections.Generic.List<Sku> list = new System.Collections.Generic.List<Sku>();
            try
            {
                System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
                bool flag = false;
                string str = string.Empty;
                if (skuModel != null && skuModel.SkuProps != null && skuModel.SkuProps.Count > 0)
                {
                    string key = string.Empty;
                    string value = string.Empty;
                    foreach (SkuPropMobile current in skuModel.SkuProps)
                    {
                        if (current.Values != null && current.Values.Count > 0 && !string.IsNullOrEmpty(current.PropId) && !string.IsNullOrEmpty(current.PropName))
                        {
                            if (current.PropName == "颜色" && current.PropId.IndexOf('-') > -1)
                            {
                                str = current.PropId;
                                current.PropId = "1627207";
                                flag = true;
                            }
                            foreach (Value current2 in current.Values)
                            {
                                if (!string.IsNullOrEmpty(current2.ValueId) && DataConvert.ToLong(current2.ValueId) != 0L && !string.IsNullOrEmpty(current2.Name))
                                {
                                    key = current.PropId + ":" + current2.ValueId;
                                    value = string.Concat(new string[]
                                    {
                                        current.PropId,
                                        ":",
                                        current2.ValueId,
                                        ":",
                                        current.PropName,
                                        ":",
                                        current2.Name
                                    });
                                    dictionary2[key] = value;
                                }
                            }
                        }
                    }
                }
                string pattern = "\"ppathIdmap\":\\s*{(?<pathIdmap>.*?)}},";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(rsContent);
                if (match != null && match.Success && !string.IsNullOrEmpty(match.Groups["pathIdmap"].Value))
                {
                    pattern = "(?is)\"(?<skuValue>.*?)\":\"(?<pathId>.*?)\"";
                    regex = new Regex(pattern);
                    MatchCollection matchCollection = regex.Matches(match.Groups["pathIdmap"].Value);
                    if (matchCollection != null && matchCollection.Count > 0)
                    {
                        foreach (Match match2 in matchCollection)
                        {
                            if (match2 != null && match2.Success && !string.IsNullOrEmpty(match2.Groups["skuValue"].Value) && !string.IsNullOrEmpty(match2.Groups["pathId"].Value))
                            {
                                string value2 = match2.Groups["pathId"].Value;
                                string text = match2.Groups["skuValue"].Value;
                                if (flag)
                                {
                                    text = text.Replace(str + ":", "1627207:");
                                }
                                dictionary[value2] = text;
                            }
                        }
                    }
                }
                new System.Collections.Generic.List<Sku>();
                pattern = "(?is)\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity)[^}]*?}),?|\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity).*?}]})}?,?";
                regex = new Regex(pattern);
                MatchCollection matchCollection2 = regex.Matches(defDyn);
                if (matchCollection2 != null && matchCollection2.Count > 0)
                {
                    string text2 = string.Empty;
                    string value3 = string.Empty;
                    string text3 = string.Empty;
                    string text4 = string.Empty;
                    Sku sku = null;
                    decimal num2 = 0m;
                    foreach (Match match3 in matchCollection2)
                    {
                        if (match3 != null && match3.Success && !string.IsNullOrEmpty(match3.Groups["pathId"].Value) && !string.IsNullOrEmpty(match3.Groups["priceUnits"].Value))
                        {
                            text2 = match3.Groups["pathId"].Value;
                            value3 = match3.Groups["priceUnits"].Value;
                            PriceUnitsEntity priceUnitsEntity = JsonConvert.DeserializeObject<PriceUnitsEntity>(value3);
                            if (priceUnitsEntity != null && !string.IsNullOrEmpty(text2))
                            {
                                sku = new Sku();
                                if (dictionary.ContainsKey(text2))
                                {
                                    text3 = dictionary[text2];
                                    sku.Properties = text3;
                                    text4 = text3;
                                    if (!string.IsNullOrEmpty(text3))
                                    {
                                        string[] array = text3.Split(new char[]
                                        {
                                            ';'
                                        });
                                        if (array != null && array.Length > 0)
                                        {
                                            for (int i = 0; i < array.Length; i++)
                                            {
                                                if (dictionary2.ContainsKey(array[i]))
                                                {
                                                    text4 = text4.Replace(array[i], dictionary2[array[i]]);
                                                }
                                            }
                                        }
                                        sku.PropertiesName = text4;
                                    }
                                    if (priceUnitsEntity.PriceUnits != null && priceUnitsEntity.PriceUnits.Count > 0)
                                    {
                                        decimal num3 = 0m;
                                        if (isSnatchpromoPrice)
                                        {
                                            num3 = System.Convert.ToDecimal(priceUnitsEntity.PriceUnits[0].Price);
                                            foreach (PriceUnits current3 in priceUnitsEntity.PriceUnits)
                                            {
                                                if (System.Convert.ToDecimal(current3.Price) < num3)
                                                {
                                                    num3 = System.Convert.ToDecimal(current3.Price);
                                                }
                                            }
                                            if (num3 < 0m)
                                            {
                                                if (num2 <= 0m)
                                                {
                                                    foreach (Match match4 in matchCollection2)
                                                    {
                                                        PriceUnitsEntity priceUnitsEntity2 = JsonConvert.DeserializeObject<PriceUnitsEntity>(match4.Groups["priceUnits"].Value);
                                                        decimal num4 = 0m;
                                                        if (priceUnitsEntity2.PriceUnits != null && priceUnitsEntity2.PriceUnits.Count > 0)
                                                        {
                                                            num4 = System.Convert.ToDecimal(priceUnitsEntity2.PriceUnits[0].Price);
                                                            foreach (PriceUnits current4 in priceUnitsEntity2.PriceUnits)
                                                            {
                                                                if (System.Convert.ToDecimal(current4.Price) < num4 || num4 < 0m)
                                                                {
                                                                    num4 = System.Convert.ToDecimal(current4.Price);
                                                                }
                                                            }
                                                        }
                                                        if (num2 <= 0m)
                                                        {
                                                            num2 = num4;
                                                        }
                                                        else
                                                        {
                                                            if (num2 > num4)
                                                            {
                                                                num2 = num4;
                                                            }
                                                        }
                                                    }
                                                }
                                                num3 = num2;
                                            }
                                        }
                                        else
                                        {
                                            num3 = System.Convert.ToDecimal(priceUnitsEntity.PriceUnits[0].Price);
                                            foreach (PriceUnits current5 in priceUnitsEntity.PriceUnits)
                                            {
                                                if (System.Convert.ToDecimal(current5.Price) > num3)
                                                {
                                                    num3 = System.Convert.ToDecimal(current5.Price);
                                                }
                                            }
                                        }
                                        sku.Price = System.Convert.ToString(num3);
                                        sku.Created = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        sku.Modified = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        sku.SkuId = System.Convert.ToInt64(text2);
                                        sku.Quantity = System.Convert.ToInt64(priceUnitsEntity.Quantity);
                                        num += sku.Quantity;
                                        if (string.IsNullOrEmpty(price))
                                        {
                                            price = sku.Price;
                                        }
                                        else
                                        {
                                            if (System.Convert.ToDecimal(price) > System.Convert.ToDecimal(sku.Price))
                                            {
                                                price = sku.Price;
                                            }
                                        }
                                        list.Add(sku);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLog(ex);
            }
            return list;
        }
        private FoodSecurity GetFoodSecurity(System.Collections.Generic.Dictionary<string, string> dicProNameAndProValue)
        {
            FoodSecurity foodSecurity = null;
            if (dicProNameAndProValue != null && dicProNameAndProValue.Count > 0)
            {
                foodSecurity = new FoodSecurity();
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicProNameAndProValue)
                {
                    if (current.Key.Contains("生产许可证编号"))
                    {
                        foodSecurity.PrdLicenseNo = current.Value;
                    }
                    if (current.Key.Contains("产品标准号"))
                    {
                        foodSecurity.DesignCode = current.Value;
                    }
                    if (current.Key.Contains("厂名"))
                    {
                        foodSecurity.Factory = current.Value;
                    }
                    if (current.Key.Contains("厂址"))
                    {
                        foodSecurity.FactorySite = current.Value;
                    }
                    if (current.Key.Contains("厂家联系方式"))
                    {
                        foodSecurity.Contact = current.Value;
                    }
                    if (current.Key.Contains("配料表"))
                    {
                        foodSecurity.Mix = current.Value;
                    }
                    if (current.Key.Contains("储藏方法"))
                    {
                        foodSecurity.PlanStorage = current.Value;
                    }
                    if (current.Key.Contains("保质期"))
                    {
                        foodSecurity.Period = current.Value;
                    }
                    if (current.Key.Contains("食品添加剂"))
                    {
                        foodSecurity.FoodAdditive = current.Value;
                    }
                    if (current.Key.Contains("健字号"))
                    {
                        foodSecurity.HealthProductNo = current.Value;
                    }
                    if (current.Key.Contains("供应商") || current.Key.Contains("厂名"))
                    {
                        foodSecurity.Supplier = current.Value;
                    }
                }
                if (string.IsNullOrEmpty(foodSecurity.FoodAdditive))
                {
                    foodSecurity.FoodAdditive = "无";
                }
            }
            return foodSecurity;
        }
        private System.Collections.Generic.List<Sku> GetSkuList(string onlineKey, out long num, out string price)
        {
            System.Collections.Generic.List<Sku> list = null;
            num = 0L;
            price = string.Empty;
            GatherTaobaoUseWebService.TaobaoPromoPriceEntity taobaoItemPromoPrice = this.GetTaobaoItemPromoPrice(onlineKey);
            if (taobaoItemPromoPrice != null)
            {
                list = new System.Collections.Generic.List<Sku>();
                System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
                if (taobaoItemPromoPrice.skuProps != null && taobaoItemPromoPrice.skuProps.Count > 0)
                {
                    foreach (GatherTaobaoUseWebService.SkuProp current in taobaoItemPromoPrice.skuProps)
                    {
                        if (current != null && current.values != null && current.values.Count > 0)
                        {
                            foreach (GatherTaobaoUseWebService.value current2 in current.values)
                            {
                                dictionary[current2.id] = current.name + ":" + current2.txt;
                            }
                        }
                    }
                }
                if (dictionary.Count > 0 && taobaoItemPromoPrice.availSKUs != null && taobaoItemPromoPrice.availSKUs.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, GatherTaobaoUseWebService.AvailSKU> current3 in taobaoItemPromoPrice.availSKUs)
                    {
                        string key = current3.Key;
                        string text = string.Empty;
                        GatherTaobaoUseWebService.AvailSKU value = current3.Value;
                        string[] array = key.Split(new char[]
                        {
                            ';'
                        }, System.StringSplitOptions.RemoveEmptyEntries);
                        if (array != null && array.Length > 0)
                        {
                            string[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                string text2 = array2[i];
                                if (dictionary.ContainsKey(text2))
                                {
                                    string text3 = text;
                                    text = string.Concat(new string[]
                                    {
                                        text3,
                                        text2,
                                        ":",
                                        dictionary[text2],
                                        ";"
                                    });
                                }
                            }
                        }
                        text = text.TrimEnd(new char[]
                        {
                            ';'
                        });
                        Sku sku = new Sku();
                        sku.Properties = key;
                        sku.PropertiesName = text;
                        sku.Price = value.price;
                        sku.Created = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        sku.Modified = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        sku.SkuId = DataConvert.ToLong(value.skuId);
                        sku.Quantity = DataConvert.ToLong(value.quantity);
                        num += sku.Quantity;
                        if (string.IsNullOrEmpty(price))
                        {
                            price = sku.Price;
                        }
                        else
                        {
                            if (DataConvert.ToDecimal(price) > DataConvert.ToDecimal(sku.Price))
                            {
                                price = sku.Price;
                            }
                        }
                        list.Add(sku);
                    }
                }
            }
            return list;
        }
        private GatherTaobaoUseWebService.TaobaoPromoPriceEntity GetTaobaoItemPromoPrice(string onlineKey)
        {
            GatherTaobaoUseWebService.TaobaoPromoPriceEntity result = null;
            string url = "http://a.m.taobao.com/ajax/sku.do?item_id=" + onlineKey;
            string referer = string.Format("http://item.taobao.com/item.htm?id={0}", new object[]
            {
                onlineKey
            });
            int num = 1;
            string responseResultBody = this.GetResponseResultBody(url, System.Text.Encoding.GetEncoding("Utf-8"), "GET", false, referer, ref num);
            try
            {
                if (!string.IsNullOrEmpty(responseResultBody))
                {
                    result = JsonConvert.DeserializeObject<GatherTaobaoUseWebService.TaobaoPromoPriceEntity>(responseResultBody);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLog(ex);
            }
            return result;
        }
        private string GetItemSellPoint(string responseContent)
        {
            string result = string.Empty;
            string sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemSellPoint", "");
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
        private static string GetLikeStrForDtSelect(string likeOrdinaryStr)
        {
            string text = DbUtil.OerateSpecialChar(likeOrdinaryStr);
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }
            return text.Replace("[", "【").Replace("]", "】").Replace("%", "[%]").Replace("_", "[_]").Replace("'", "，").Replace("*", "[*]").Trim();
        }
        private System.Collections.Generic.Dictionary<string, string> GetItemProperty(string itemContent)
        {
            System.Collections.Generic.Dictionary<string, string> dictionary = null;
            dictionary = new System.Collections.Generic.Dictionary<string, string>();
            string text = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemProperty", "");
            if (!string.IsNullOrEmpty(text))
            {
                string[] array = text.Split(new string[]
                {
                    "分隔"
                }, System.StringSplitOptions.RemoveEmptyEntries);
                if (array != null && array.Length > 0)
                {
                    if (array.Length == 2 && array[0].StartsWith("Match:"))
                    {
                        text = array[0].Substring(6);
                        Regex regex = new Regex(text);
                        Match match = regex.Match(itemContent);
                        if (match == null || match.Groups == null || !match.Groups["ItemProperty"].Success)
                        {
                            string pattern = "(?is)\\{\"attrs\":\\[.*?]";
                            Regex regex2 = new Regex(pattern);
                            MatchCollection matchCollection = regex2.Matches(itemContent);
                            if (matchCollection != null)
                            {
                                foreach (Match match2 in matchCollection)
                                {
                                    string pattern2 = "(?is)\"name\":(?<name>.*?),|(?is)\"value\":(?<value>.*?)\\}";
                                    Regex regex3 = new Regex(pattern2);
                                    MatchCollection matchCollection2 = regex3.Matches(match2.Value);
                                    if (matchCollection2 != null && matchCollection2.Count > 0)
                                    {
                                        string key = "";
                                        foreach (Match match3 in matchCollection2)
                                        {
                                            if (match3.Groups["name"].Success && !string.IsNullOrEmpty(match3.Groups["name"].Value))
                                            {
                                                string text2 = match3.Groups["name"].Value.Replace("\"", "");
                                                dictionary[text2] = "";
                                                key = text2;
                                            }
                                            if (match3.Groups["value"].Success && !string.IsNullOrEmpty(match3.Groups["value"].Value))
                                            {
                                                string value = match3.Groups["value"].Value.Replace("\"", "");
                                                dictionary[key] = value;
                                            }
                                        }
                                    }
                                }
                            }
                            return dictionary;
                        }
                        itemContent = match.Groups["ItemProperty"].Value;
                    }
                    text = array[array.Length - 1];
                    if (text.StartsWith("Matchs:"))
                    {
                        text = text.Substring(7);
                        Regex regex = new Regex(text);
                        MatchCollection matchCollection3 = regex.Matches(itemContent);
                        if (matchCollection3 != null && matchCollection3.Count > 0)
                        {
                            foreach (Match match4 in matchCollection3)
                            {
                                if (match4.Groups["PropertyName"].Success && !string.IsNullOrEmpty(match4.Groups["PropertyName"].Value) && match4.Groups["PropertyValue"].Success && !string.IsNullOrEmpty(match4.Groups["PropertyValue"].Value))
                                {
                                    string key2 = this.ConvertDBC(match4.Groups["PropertyName"].Value).Replace("&nbsp", "").Replace(";", "");
                                    if (match4.Groups["PropertyValue"].Value.ToUpper().Contains("<A"))
                                    {
                                        if (match4.Groups["PropertyValue"].Value.ToLower().Contains("class") && match4.Groups["PropertyValue"].Value.ToLower().Contains("c_green"))
                                        {
                                            string pattern3 = "(?is)<a[^>]*?>|</a>";
                                            Regex regex4 = new Regex(pattern3);
                                            string value2 = regex4.Replace(match4.Groups["PropertyValue"].Value.ToString(), "");
                                            dictionary[key2] = value2;
                                        }
                                        else
                                        {
                                            string pattern4 = "(?is)<a[^>]*?>(?<value>[^<>]*?)</a>";
                                            Regex regex5 = new Regex(pattern4);
                                            MatchCollection matchCollection4 = regex5.Matches(match4.Groups["PropertyValue"].Value);
                                            if (matchCollection4 != null && matchCollection4.Count > 0)
                                            {
                                                string text3 = string.Empty;
                                                foreach (Match match5 in matchCollection4)
                                                {
                                                    if (match5.Groups["value"].Success && !string.IsNullOrEmpty(match5.Groups["value"].Value))
                                                    {
                                                        text3 = text3 + match5.Groups["value"].Value + ",";
                                                    }
                                                }
                                                dictionary[key2] = text3;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dictionary[key2] = HttpUtility.HtmlDecode(match4.Groups["PropertyValue"].Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dictionary;
        }
        private string ToDBC(string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '\u3000')
                {
                    array[i] = ' ';
                }
                else
                {
                    if (array[i] > '＀' && array[i] < '｟')
                    {
                        array[i] -= 'ﻠ';
                    }
                }
            }
            return new string(array);
        }
        private string ConvertDBC(string str)
        {
            string result = string.Empty;
            string pattern = "[＀-￿]";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(str);
            if (match.Success)
            {
                result = this.ToDBC(str);
            }
            else
            {
                result = str;
            }
            return result;
        }
        private void SetSizeGroupList(string responseContent, System.Collections.Generic.IList<SellProInfo> sellProinfoList)
        {
            if (sellProinfoList == null)
            {
                sellProinfoList = new System.Collections.Generic.List<SellProInfo>();
            }
            string text = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetSizeGroup", "");
            if (!string.IsNullOrEmpty(text))
            {
                string text2 = string.Empty;
                string[] array = text.Split(new string[]
                {
                    "分隔"
                }, System.StringSplitOptions.RemoveEmptyEntries);
                if (array != null && array.Length == 2 && array[0].StartsWith("Match:"))
                {
                    text = array[0].Substring(6);
                    Regex regex = new Regex(text);
                    MatchCollection matchCollection = regex.Matches(responseContent);
                    if (matchCollection != null && matchCollection.Count > 0)
                    {
                        foreach (Match match in matchCollection)
                        {
                            text2 = match.Groups["SizeGroup"].Value;
                            if (!string.IsNullOrEmpty(text2) && array[1].StartsWith("Matchs:"))
                            {
                                text = array[1].Substring(7);
                                Regex regex2 = new Regex(text);
                                MatchCollection matchCollection2 = regex2.Matches(text2);
                                if (matchCollection2 != null && matchCollection2.Count > 0)
                                {
                                    foreach (Match match2 in matchCollection2)
                                    {
                                        if (match2.Groups["SizeName"].Success && !string.IsNullOrEmpty(match2.Groups["SizeName"].Value) && match2.Groups["SizeValue"].Success && !string.IsNullOrEmpty(match2.Groups["SizeValue"].Value))
                                        {
                                            sellProinfoList.Add(new SellProInfo
                                            {
                                                Name = match2.Groups["SizeName"].Value,
                                                Value = match2.Groups["SizeValue"].Value
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private long GetItemNumIid(string responseContent)
        {
            string o = string.Empty;
            string sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemNumIid", "");
            if (!string.IsNullOrEmpty(sysConfig))
            {
                Regex regex = new Regex(sysConfig);
                Match match = regex.Match(responseContent);
                if (match != null && match.Groups["ItemNumIid"].Success)
                {
                    o = match.Groups["ItemNumIid"].Value;
                }
            }
            return DataConvert.ToLong(o);
        }
        private System.Collections.Generic.List<PropImg> GetProImgList(string responseContent, System.Collections.Generic.IList<SellProInfo> sellProinfoList)
        {
            System.Collections.Generic.List<PropImg> list = null;
            string text = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetProImgs", "");
            if (!string.IsNullOrEmpty(text))
            {
                string[] array = text.Split(new string[]
                {
                    "分隔"
                }, System.StringSplitOptions.RemoveEmptyEntries);
                if (array != null && array.Length == 2)
                {
                    if (array[0].StartsWith("Match:"))
                    {
                        text = array[0].Substring(6);
                        Regex regex = new Regex(text);
                        MatchCollection matchCollection = regex.Matches(responseContent);
                        if (matchCollection != null && matchCollection.Count > 0)
                        {
                            list = new System.Collections.Generic.List<PropImg>();
                            foreach (Match match in matchCollection)
                            {
                                string arg_C9_0 = string.Empty;
                                if (match.Groups["PropertyValue"].Success && !string.IsNullOrEmpty(match.Groups["PropertyValue"].Value) && match.Groups["ProertyValueImg"].Success && !string.IsNullOrEmpty(match.Groups["ProertyValueImg"].Value) && match.Groups["PropertyName"].Success && !string.IsNullOrEmpty(match.Groups["PropertyName"].Value))
                                {
                                    PropImg propImg = new PropImg();
                                    SellProInfo sellProInfo = new SellProInfo();
                                    propImg.Id = 0L;
                                    propImg.Position = 0L;
                                    propImg.Properties = match.Groups["PropertyValue"].Value;
                                    propImg.Url = match.Groups["ProertyValueImg"].Value;
                                    list.Add(propImg);
                                    sellProInfo.Name = match.Groups["PropertyName"].Value.Replace("amp;", "");
                                    sellProInfo.Value = match.Groups["PropertyValue"].Value;
                                    sellProinfoList.Add(sellProInfo);
                                }
                            }
                        }
                    }
                    if (array[1].StartsWith("Matchs:"))
                    {
                        text = array[1].Substring(7);
                        Regex regex2 = new Regex(text);
                        MatchCollection matchCollection2 = regex2.Matches(responseContent);
                        if (matchCollection2 != null && matchCollection2.Count > 0)
                        {
                            foreach (Match match2 in matchCollection2)
                            {
                                if (match2.Groups["PropertyValue"].Success && !string.IsNullOrEmpty(match2.Groups["PropertyValue"].Value) && match2.Groups["PropertyName"].Success && !string.IsNullOrEmpty(match2.Groups["PropertyName"].Value))
                                {
                                    sellProinfoList.Add(new SellProInfo
                                    {
                                        Name = match2.Groups["PropertyName"].Value.Replace("amp;", ""),
                                        Value = match2.Groups["PropertyValue"].Value
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
        private System.Collections.Generic.List<ItemImg> GetItemImgList(string responseContent)
        {
            System.Collections.Generic.List<ItemImg> list = null;
            string text = string.Empty;
            string text2 = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemImgs", "");
            if (!string.IsNullOrEmpty(text2))
            {
                Regex regex = new Regex(text2);
                Match match = regex.Match(responseContent);
                if (match != null && match.Groups["ItemImgs"].Success)
                {
                    text = match.Groups["ItemImgs"].Value;
                }
                if (string.IsNullOrEmpty(text))
                {
                    string text3 = string.Empty;
                    text2 = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemImgsTwo", "");
                    if (text2 != null)
                    {
                        string[] array = text2.Split(new string[]
                        {
                            "分隔"
                        }, System.StringSplitOptions.RemoveEmptyEntries);
                        if (array.Length == 2 && array[0].StartsWith("Match:"))
                        {
                            text2 = array[0].Substring(6);
                            regex = new Regex(text2);
                            match = regex.Match(responseContent);
                            if (match != null && match.Groups != null && match.Groups["ItemImgs"].Success)
                            {
                                text3 = match.Groups["ItemImgs"].Value;
                            }
                            if (array[1].StartsWith("Matchs:") && !string.IsNullOrEmpty(text3))
                            {
                                text2 = array[1].Substring(7);
                                regex = new Regex(text2);
                                MatchCollection matchCollection = regex.Matches(text3);
                                if (matchCollection != null && matchCollection.Count > 0)
                                {
                                    text = "[";
                                    foreach (Match match2 in matchCollection)
                                    {
                                        if (match2.Groups["ImageUrl"].Success && !string.IsNullOrEmpty(match2.Groups["ImageUrl"].Value))
                                        {
                                            string str = match2.Groups["ImageUrl"].Value.Replace(".jpg_60x60q90", "");
                                            text = text + "\"" + str + "\",";
                                        }
                                    }
                                    text = text.TrimEnd(new char[]
                                    {
                                        ','
                                    }) + "]";
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(text))
                {
                    System.Collections.Generic.List<string> list2 = JsonConvert.DeserializeObject<System.Collections.Generic.List<string>>(text);
                    if (list2 != null && list2.Count > 0)
                    {
                        list = new System.Collections.Generic.List<ItemImg>();
                        for (int i = 0; i < list2.Count; i++)
                        {
                            list.Add(new ItemImg
                            {
                                Position = (long)i,
                                Url = list2[i]
                            });
                        }
                    }
                }
            }
            return list;
        }
        private string GetItemContent(string itemContent, string itemUrl)
        {
            string text = string.Empty;
            string result = string.Empty;
            if (!string.IsNullOrEmpty(this._sysName))
            {
                string sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemContentUrl", "");
                if (!string.IsNullOrEmpty(sysConfig))
                {
                    Regex regex = new Regex(sysConfig);
                    Match match = regex.Match(itemContent);
                    if (match != null && match.Groups["ItemContentUrl"].Success)
                    {
                        text = match.Groups["ItemContentUrl"].Value;
                    }
                }
                if (!string.IsNullOrEmpty(text) && !text.StartsWith("http://") && !text.StartsWith("https://"))
                {
                    if (text.StartsWith("//"))
                    {
                        text = "http:" + text;
                    }
                    else
                    {
                        text = "http://" + text;
                    }
                }
                if (!string.IsNullOrEmpty(text))
                {
                    int num = 1;
                    string responseResultBody = this.GetResponseResultBody(text, System.Text.Encoding.GetEncoding("gb2312"), "GET", true, itemUrl, ref num);
                    if (!string.IsNullOrEmpty(responseResultBody))
                    {
                        sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemContent", "");
                        if (!string.IsNullOrEmpty(sysConfig))
                        {
                            Regex regex2 = new Regex(sysConfig);
                            Match match2 = regex2.Match(responseResultBody);
                            if (match2 != null && match2.Groups["ItemContent"].Success)
                            {
                                result = match2.Groups["ItemContent"].Value;
                            }
                        }
                    }
                }
            }
            return result;
        }
        private string GetItemCid(string responseContent)
        {
            string result = string.Empty;
            string sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemCid", "");
            if (!string.IsNullOrEmpty(sysConfig))
            {
                Regex regex = new Regex(sysConfig);
                Match match = regex.Match(responseContent);
                if (match != null && match.Groups["ItemCid"].Success)
                {
                    result = match.Groups["ItemCid"].Value;
                }
            }
            return result;
        }
        private string GetItemPrice(string responseContent)
        {
            string result = string.Empty;
            string sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemPrice", "");
            if (!string.IsNullOrEmpty(sysConfig))
            {
                Regex regex = new Regex(sysConfig);
                Match match = regex.Match(responseContent);
                if (match != null && match.Groups["ItemPrice"].Success)
                {
                    result = match.Groups["ItemPrice"].Value;
                }
            }
            return result;
        }
        private string GetItemNum(string responseContent)
        {
            string result = string.Empty;
            string sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemNum", "");
            if (!string.IsNullOrEmpty(sysConfig))
            {
                Regex regex = new Regex(sysConfig);
                Match match = regex.Match(responseContent);
                if (match != null && match.Groups["ItemNum"].Success)
                {
                    result = match.Groups["ItemNum"].Value;
                }
            }
            return result;
        }
        private string GetItemName(string itemContent)
        {
            string text = string.Empty;
            if (!string.IsNullOrEmpty(this._sysName))
            {
                bool flag = DataConvert.ToBoolean(DataHelper.GetSysConfig("AppConfig", this._sysName, "IsNewGetItemName", "false"));
                if (flag)
                {
                    DataHelper.GetSysConfig("AppConfig", this._sysName, "NewGetItemNameUrl", "");
                }
                else
                {
                    string text2 = "data-value='\\{.*?\\}'";
                    string sysConfig = DataHelper.GetSysConfig("AppConfig", this._sysName, "GetItemName", "");
                    if (!string.IsNullOrEmpty(text2))
                    {
                        Regex regex = new Regex(text2);
                        MatchCollection matchCollection = regex.Matches(itemContent);
                        if (matchCollection != null)
                        {
                            foreach (Match match in matchCollection)
                            {
                                Regex regex2 = new Regex(sysConfig);
                                Match match2 = regex2.Match(match.Value);
                                if (match2 != null && match2.Groups["ItemName"].Success)
                                {
                                    text = match2.Groups["ItemName"].Value;
                                    break;
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        Regex regex3 = new Regex(sysConfig);
                        Match match3 = regex3.Match(itemContent);
                        if (match3 != null && match3.Groups["ItemName"].Success)
                        {
                            text = match3.Groups["ItemName"].Value;
                        }
                    }
                }
            }
            return text;
        }
        private string GetResponseResultBody(string url, System.Text.Encoding encoding, string rquestMethod, bool isIE, string referer, ref int times)
        {
            string text = string.Empty;
            CommonApiClient commonApiClient = new CommonApiClient();
            CookieCollection cookieCollection = new CookieCollection();
            if (url.Contains(".taobao.com"))
            {
                Cookie cookie = new Cookie("tracknick", "zbs18938040399")
                {
                    Domain = ".taobao.com"
                };
                cookieCollection.Add(cookie);
            }
            else
            {
                Cookie cookie2 = new Cookie("tracknick", "zbs18938040399")
                {
                    Domain = ".tmall.com"
                };
                cookieCollection.Add(cookie2);
            }
            text = commonApiClient.Invoke(url, encoding, rquestMethod, isIE, referer, cookieCollection);
            int num = 0;
            while (string.IsNullOrEmpty(text) && num < 3)
            {
                num++;
                text = commonApiClient.Invoke(url, encoding, rquestMethod, isIE, referer, cookieCollection);
                System.Threading.Thread.Sleep(1000);
            }
            if (string.IsNullOrEmpty(text))
            {
                Log.WriteLog("请求失败:" + url);
            }
            if (!string.IsNullOrEmpty(text) && times < 3)
            {
                if (text.Contains("访问受限了"))
                {
                    Log.WriteLog("访问受限了:" + text);
                    return string.Empty;
                }
                if (text.Contains("ERRCODE_QUERY_DETAIL_FAIL"))
                {
                    times++;
                    System.Threading.Thread.Sleep(times * 1000);
                    return this.GetResponseResultBody(url, encoding, rquestMethod, isIE, referer, ref times);
                }
            }
            return text;
        }
        public override ItemGetResponse SnatchItem(string itemOnlineKey, bool snatchPromotionPrice)
        {
            return null;
            //  return this.GetItemGetMobileResponseByOnlinekey(itemOnlineKey, 1, snatchPromotionPrice);
        }
        //public override System.Collections.Generic.IList<ProductInfo> SnatchItemsNew(string shopNick, bool isTmall, long categoryId, int pageNum, string shopId)
        //{
        //    System.Collections.Generic.IList<ProductInfo> result;
        //    try
        //    {
        //        System.Collections.Generic.IList<ProductInfo> list = new System.Collections.Generic.List<ProductInfo>();
        //        System.Collections.Generic.IList<ProductInfo> itemsNew;
        //        if (categoryId > 0L)
        //        {
        //            itemsNew = this.TaobaoService.GetItemsNew(shopNick, isTmall, categoryId, pageNum, shopId);
        //        }
        //        else
        //        {
        //            itemsNew = this.TaobaoService.GetItemsNew(shopNick, isTmall, pageNum, shopId);
        //        }
        //        if (itemsNew != null && itemsNew.Count > 0)
        //        {
        //            for (int i = 0; i < itemsNew.Count; i++)
        //            {
        //                list.Add(new ProductInfo
        //                {
        //                    ImageUrl = itemsNew[i].ImageUrl,
        //                    ItemCode = itemsNew[i].ItemCode,
        //                    Name = itemsNew[i].Name,
        //                    SysId = itemsNew[i].SysId,
        //                    Url = itemsNew[i].Url
        //                });
        //            }
        //        }
        //        result = list;
        //    }
        //    catch (System.Exception)
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
        //public override System.Collections.Generic.IList<ProductInfo> SnatchItems(string shopNick, bool isTmall, long categoryId)
        //{
        //    System.Collections.Generic.IList<ProductInfo> result;
        //    try
        //    {
        //        System.Collections.Generic.IList<ProductInfo> list = new System.Collections.Generic.List<ProductInfo>();
        //        System.Collections.Generic.IList<ProductInfo> items;
        //        if (categoryId > 0L)
        //        {
        //            items = this.TaobaoService.GetItems(shopNick, isTmall, new System.Collections.Generic.List<long>
        //            {
        //                categoryId
        //            });
        //        }
        //        else
        //        {
        //            items = this.TaobaoService.GetItems(shopNick, isTmall);
        //        }
        //        if (items != null && items.Count > 0)
        //        {
        //            for (int i = 0; i < items.Count; i++)
        //            {
        //                list.Add(new ProductInfo
        //                {
        //                    ImageUrl = items[i].ImageUrl,
        //                    ItemCode = items[i].ItemCode,
        //                    Name = items[i].Name,
        //                    SysId = items[i].SysId,
        //                    Url = items[i].Url
        //                });
        //            }
        //        }
        //        result = list;
        //    }
        //    catch (System.Exception)
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
        //public override int GetTotalPage(string shopNick, bool isTmall, long categoryId, out string shopId)
        //{
        //    return this.TaobaoService.GetTotalPage(shopNick, isTmall, categoryId, out shopId);
        //}
        public override string InvokeWebRequest(string url)
        {
            return this.InvokeWebRequestNew(url, "gb2312");
        }
        public override string InvokeWebRequestNew(string url, string encode)
        {
            bool flag = false;
            if (encode == "0")
            {
                flag = true;
            }
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(encode) || encode == "0")
            {
                encode = "gb2312";
            }
            encode = encode.Trim();
            string referer = string.Empty;
            if (url.IndexOf(".taobao.", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                referer = "http://www.taobao.com";
            }
            else
            {
                if (url.IndexOf(".1688.", System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    referer = "http://www.1688.com";
                }
                else
                {
                    if (url.IndexOf(".tmall.", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        referer = "http://www.tmall.com";
                    }
                }
            }
            if (url.IndexOf("getcategory.1688") > -1)
            {
                string text = url.Replace("getcategory.1688.", "");
                if (!string.IsNullOrEmpty(text))
                {
                    string aliCatIdByOffer = this.GetAliCatIdByOffer(text);
                    if (string.IsNullOrEmpty(aliCatIdByOffer))
                    {
                        System.Threading.Thread.Sleep(2000);
                        aliCatIdByOffer = this.GetAliCatIdByOffer(text);
                    }
                    return aliCatIdByOffer;
                }
            }
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("gb2312");
            try
            {
                if (!string.IsNullOrEmpty(encode) && encode.ToLower() != "gb2312")
                {
                    encoding = System.Text.Encoding.GetEncoding(encode);
                }
            }
            catch
            {
            }
            CommonApiClient commonApiClient = new CommonApiClient();
            string text2 = commonApiClient.Invoke(url, encoding, "GET", true, referer);
            if (!flag)
            {
                return text2;
            }
            string userNickFromResponseResult = GetUserNickFromResponseResult(text2);
            if (string.IsNullOrEmpty(userNickFromResponseResult))
            {
                return string.Empty;
            }
            return "shopnick:" + userNickFromResponseResult;
        }



        public string GetUserNickFromResponseResult(string responseContent)
        {
            if (string.IsNullOrEmpty(responseContent))
            {
                return string.Empty;
            }
            string text = string.Empty;
            string pattern = "<span class=\"J_WangWang\" data-nick=\"(?<userNick>.*?)\"";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(responseContent);
            if (match.Success && match.Groups["userNick"].Success)
            {
                text = match.Groups["userNick"].Value;
                text = HttpUtility.UrlDecode(text);
            }
            if (string.IsNullOrEmpty(text))
            {
                pattern = "<span class=\"J_WangWang wangwang\"[\\s]*data-nick=\"(?<userNick>.*?)\"";
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                match = regex.Match(responseContent);
                if (match.Success && match.Groups["userNick"].Success)
                {
                    text = match.Groups["userNick"].Value;
                    text = HttpUtility.UrlDecode(text);
                }
            }
            if (string.IsNullOrEmpty(text))
            {
                pattern = "<h3 class=\"shop-title\">\\s*<a.*href=\".*\">(?<userNick>.*?)</a>\\s*</h3>";
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                match = regex.Match(responseContent);
                if (match.Success && match.Groups["userNick"].Success)
                {
                    text = match.Groups["userNick"].Value;
                    text = HttpUtility.UrlDecode(text);
                }
            }
            if (string.IsNullOrEmpty(text))
            {
                pattern = "<span class=\"J_WangWang wangwang\" data-nick=\"(?<userNick>.*?)\"";
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                match = regex.Match(responseContent);
                if (match.Success && match.Groups["userNick"].Success)
                {
                    text = match.Groups["userNick"].Value;
                    text = HttpUtility.UrlDecode(text);
                }
            }
            if (string.IsNullOrEmpty(text))
            {
                pattern = "(?is)<a[^>]*?class=\"slogo-shopname\"[^>]*?>[^<>]*?<strong>(?<userNick>.*?)</strong>[^<>]*?</a>";
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                match = regex.Match(responseContent);
                if (match.Success && match.Groups["userNick"].Success)
                {
                    text = match.Groups["userNick"].Value;
                    text = HttpUtility.UrlDecode(text);
                }
            }
            if (string.IsNullOrEmpty(text))
            {
                pattern = "(?is)sellerNick\\s*:\\s*'(?<userNick>.*?)',";
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                match = regex.Match(responseContent);
                if (match.Success && match.Groups["userNick"].Success)
                {
                    text = match.Groups["userNick"].Value;
                    text = HttpUtility.UrlDecode(text);
                }
            }
            return text;
        }


        private void FoodSecurityDate(string onlineKey, ref string productStartDatetime, ref string productEndDateTime)
        {
            productStartDatetime = System.DateTime.Now.Date.AddDays(-1.0).ToString("yyyy-MM-dd");
            productEndDateTime = System.DateTime.Now.Date.AddDays(-1.0).ToString("yyyy-MM-dd");
            new ItemGetResponse();
            string text = string.Format("http://item.taobao.com/item.htm?id={0}", new object[]
            {
                onlineKey
            });
            try
            {
                int num = 1;
                string responseResultBody = this.GetResponseResultBody(text, System.Text.Encoding.GetEncoding("gb2312"), "GET", true, text, ref num);
                if (!string.IsNullOrEmpty(responseResultBody))
                {
                    string pattern = "(?is)生产日期:(?<date>.*?)<";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(responseResultBody);
                    if (match != null && match.Success)
                    {
                        string text2 = match.Value;
                        text2 = text2.Replace("年", "-").Replace("月", "-").Replace("日", "-");
                        text2 = text2.Replace("/", "-");
                        string pattern2 = "\\d{4}\\-\\d{1,2}\\-\\d{1,2}";
                        Regex regex2 = new Regex(pattern2);
                        MatchCollection matchCollection = regex2.Matches(text2);
                        if (matchCollection != null && matchCollection.Count == 2)
                        {
                            productStartDatetime = matchCollection[0].Value;
                            productEndDateTime = matchCollection[1].Value;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLog(ex);
            }
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
        private int GetStringLengthWithChinlish(string source)
        {
            Regex regex = new Regex("[\\u4E00-\\u9fa5]");
            int num = 0;
            char[] array = source.ToCharArray();
            char[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                char c = array2[i];
                System.Console.Write(c.ToString());
                if (regex.IsMatch(c.ToString()))
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
            }
            return num;
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
                    int num = DataConvert.ToInt(match2.Groups["quantity"].Value.ToString());
                    if (num <= 0)
                    {
                        flag = false;
                    }
                }
            }
            if (match == null || !match.Success)
            {
                return "onsale";
            }
            if (a == "当前区域卖光了" && flag)
            {
                return "onsale";
            }
            return "instock";
        }
        public string GetSellPoint(string responseContent)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(responseContent))
            {
                return result;
            }
            try
            {
                result = this.GetItemSellPoint(responseContent);
            }
            catch (System.Exception ex)
            {
                Log.WriteLog("下载商品卖点出现异常", ex);
            }
            return result;
        }
        public string GetItemResponseContent(string itemUrl)
        {
            string empty = string.Empty;
            CommonApiClient commonApiClient = new CommonApiClient();
            return commonApiClient.Invoke(itemUrl, System.Text.Encoding.GetEncoding("gb2312"), "GET", false, itemUrl);
        }
        public System.Collections.Generic.List<Sku> GetMobilePromoPriceSkuList(string itemId, System.Collections.Generic.List<Sku> skuList, ref string price, ref long num, bool isSnatchpromoPrice, bool isTaobao)
        {
            price = string.Empty;
            int num2 = 1;
            long num3 = 0L;
            if (string.IsNullOrEmpty(itemId))
            {
                return skuList;
            }
            string text = "{\"itemNumId\":\"" + itemId + "\"}";
            text = Uri.EscapeDataString(text);
            string text2 = "http://acs.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data=" + text;
            string responseResultBody = this.GetResponseResultBody(text2, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text2, ref num2);
            if (string.IsNullOrEmpty(responseResultBody))
            {
                int num4 = 0;
                do
                {
                    if (string.IsNullOrEmpty(responseResultBody))
                    {
                        num4++;
                        num2 = 1;
                        responseResultBody = this.GetResponseResultBody(text2, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text2, ref num2);
                    }
                    else
                    {
                        num4 = 3;
                    }
                }
                while (num4 < 3);
            }
            lock (GatherTaobaoUseWebService.obj)
            {
                ItemJsonEntity60 itemJsonEntity = null;
                try
                {
                    itemJsonEntity = JsonConvert.DeserializeObject<ItemJsonEntity60>(responseResultBody);
                }
                catch (System.Exception)
                {
                }
                int num5 = 0;
                while (itemJsonEntity != null && itemJsonEntity.data.item == null && responseResultBody.Contains("FAIL_SYS_USER_VALIDATE") && num5 < 50)
                {
                    num5++;
                    System.Threading.Thread.Sleep(3000);
                    num2 = 1;
                    responseResultBody = this.GetResponseResultBody(text2, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text2, ref num2);
                    try
                    {
                        itemJsonEntity = JsonConvert.DeserializeObject<ItemJsonEntity60>(responseResultBody);
                    }
                    catch (System.Exception)
                    {
                    }
                }
                if (itemJsonEntity != null && itemJsonEntity.data.item == null && responseResultBody.Contains("FAIL_SYS_USER_VALIDATE"))
                {
                    Log.WriteLog("6.0接口访问失败" + responseResultBody);
                }
            }
            System.Collections.Generic.Dictionary<string, Sku2Info> dictionary = new System.Collections.Generic.Dictionary<string, Sku2Info>();
            System.Collections.Generic.Dictionary<string, Sku2Info_T> dictionary2 = new System.Collections.Generic.Dictionary<string, Sku2Info_T>();
            if (!string.IsNullOrEmpty(responseResultBody))
            {
                string pattern = "\\\"value\\\".*?\\\\\"skuCore\\\\\":(?<skuCore>.*?),\\\\\"skuItem\\\\\"";
                if (isTaobao)
                {
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(responseResultBody);
                    if (match != null && match.Groups["skuCore"].Success && !string.IsNullOrEmpty(match.Groups["skuCore"].Value))
                    {
                        string value = match.Groups["skuCore"].Value.Replace("\\\"", "\"") + "}";
                        ItemTaobaoSkuCore itemTaobaoSkuCore = JsonConvert.DeserializeObject<ItemTaobaoSkuCore>(value);
                        dictionary = itemTaobaoSkuCore.sku2Info;
                    }
                }
                else
                {
                    pattern = "(?is)\\\"sku2info\\\\\":(?<skuInfo>.*?)},\\\\\"skuVertical\\\\\"";
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match2 = regex.Match(responseResultBody);
                    if (match2 != null && match2.Success && !string.IsNullOrEmpty(match2.Groups["skuInfo"].Value))
                    {
                        string text3 = match2.Groups["skuInfo"].Value.Replace("\\\"", "\"");
                        if (!string.IsNullOrEmpty(text3))
                        {
                            if (text3.Contains("services"))
                            {
                                regex = new Regex("(?is),\"services\":.*?}][^,]", RegexOptions.IgnoreCase);
                                text3 = regex.Replace(text3, string.Empty);
                            }
                            pattern = "(?is)\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity)[^}]*?}),?|\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity).*?}]})}?,?";
                            regex = new Regex(pattern, RegexOptions.IgnoreCase);
                            MatchCollection matchCollection = regex.Matches(text3);
                            if (matchCollection != null && matchCollection.Count > 0)
                            {
                                foreach (Match match3 in matchCollection)
                                {
                                    if (match3 != null && match3.Success && !string.IsNullOrEmpty(match3.Groups["pathId"].Value))
                                    {
                                        string text4 = DataConvert.ToString(match3.Groups["priceUnits"].Value);
                                        if (!string.IsNullOrEmpty(text4))
                                        {
                                            Regex regex2 = new Regex("(?is)\"subPrice\":.*?\"priceText\":\"(?<priceText>.*?)\".*?\"quantity\":\"(?<quantity>.*?)\",?", RegexOptions.IgnoreCase);
                                            Match match4 = regex2.Match(text4);
                                            if (match4 != null && match4.Success)
                                            {
                                                Sku2Info_T sku2Info_T = new Sku2Info_T();
                                                sku2Info_T.price = new Price();
                                                sku2Info_T.price.priceText = DataConvert.ToString(match4.Groups["priceText"].Value);
                                                sku2Info_T.quantity = DataConvert.ToString(match4.Groups["quantity"].Value);
                                                dictionary2[DataConvert.ToString(match3.Groups["pathId"].Value)] = sku2Info_T;
                                            }
                                            else
                                            {
                                                regex2 = new Regex("(?is)\"priceText\":\"(?<priceText>.*?)\".*?\"quantity\":\"(?<quantity>.*?)\",?", RegexOptions.IgnoreCase);
                                                match4 = regex2.Match(text4);
                                                if (match4 != null && match4.Success)
                                                {
                                                    Sku2Info_T sku2Info_T2 = new Sku2Info_T();
                                                    sku2Info_T2.price = new Price();
                                                    sku2Info_T2.price.priceText = DataConvert.ToString(match4.Groups["priceText"].Value);
                                                    sku2Info_T2.quantity = DataConvert.ToString(match4.Groups["quantity"].Value);
                                                    dictionary2[DataConvert.ToString(match3.Groups["pathId"].Value)] = sku2Info_T2;
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
            if (skuList != null && skuList.Count > 0)
            {
                for (int i = 0; i < skuList.Count; i++)
                {
                    if (isTaobao)
                    {
                        if (dictionary != null && dictionary.Count > 0 && dictionary.ContainsKey(skuList[i].SkuId.ToString()))
                        {
                            Sku2Info sku2Info = dictionary[skuList[i].SkuId.ToString()];
                            decimal num6 = DataConvert.ToDecimal(sku2Info.price.priceText);
                            DataConvert.ToDecimal(skuList[i].Price);
                            if (num6 > 0m)
                            {
                                skuList[i].Price = System.Math.Round(num6, 2).ToString();
                                skuList[i].Quantity = DataConvert.ToLong(sku2Info.quantity);
                                if (string.IsNullOrEmpty(price) && skuList[i].Quantity > 0L)
                                {
                                    price = skuList[i].Price;
                                }
                                else
                                {
                                    if (System.Convert.ToDecimal(price) > System.Convert.ToDecimal(skuList[i].Price) && skuList[i].Quantity > 0L)
                                    {
                                        price = skuList[i].Price;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dictionary2 != null && dictionary2.Count > 0 && dictionary2.ContainsKey(skuList[i].SkuId.ToString()))
                        {
                            Sku2Info_T sku2Info_T3 = dictionary2[skuList[i].SkuId.ToString()];
                            decimal num7 = DataConvert.ToDecimal(sku2Info_T3.price.priceText);
                            decimal d = DataConvert.ToDecimal(skuList[i].Price);
                            if (num7 < d)
                            {
                                skuList[i].Price = System.Math.Round(num7, 2).ToString();
                                skuList[i].Quantity = DataConvert.ToLong(sku2Info_T3.quantity);
                                if (string.IsNullOrEmpty(price) && skuList[i].Quantity > 0L)
                                {
                                    price = sku2Info_T3.price.priceText;
                                }
                                else
                                {
                                    if (System.Convert.ToDecimal(price) > System.Convert.ToDecimal(num7) && skuList[i].Quantity > 0L)
                                    {
                                        price = skuList[i].Price;
                                    }
                                }
                            }
                        }
                    }
                    num3 += skuList[i].Quantity;
                }
            }
            else
            {
                if (dictionary.ContainsKey("0"))
                {
                    Sku2Info sku2Info2 = dictionary["0"];
                    price = sku2Info2.price.priceText;
                    num3 = DataConvert.ToLong(sku2Info2.quantity);
                }
            }
            num = num3;
            return skuList;
        }
        private string GetHtmlCode(string onlineKey)
        {
            string requestUriString = string.Format("http://hws.m.taobao.com/cache/wdetail/5.0/?id={0}&ttid=2013@taobao_h5_1.0.0", new object[]
            {
                onlineKey
            });
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Timeout = 30000;
            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Mozilla/4.0";
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result;
            if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
            {
                using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    using (GZipStream gZipStream = new GZipStream(responseStream, CompressionMode.Decompress))
                    {
                        using (System.IO.StreamReader streamReader = new System.IO.StreamReader(gZipStream, System.Text.Encoding.UTF8))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                    return result;
                }
            }
            using (System.IO.Stream responseStream2 = httpWebResponse.GetResponseStream())
            {
                using (System.IO.StreamReader streamReader2 = new System.IO.StreamReader(responseStream2, System.Text.Encoding.Default))
                {
                    result = streamReader2.ReadToEnd();
                }
            }
            return result;
        }
        private string GetAliCatIdByOffer(string onlineKey)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(onlineKey))
            {
                return result;
            }
            CommonApiClient commonApiClient = new CommonApiClient();
            string text = "https://detail.1688.com/pic/" + onlineKey + ".html";
            string text2 = commonApiClient.Invoke(text, System.Text.Encoding.GetEncoding("gbk"), "get", true, text);
            if (!string.IsNullOrEmpty(text2))
            {
                Regex regex = new Regex("<meta\\s*name=\"?description\"?.*?我们还为您精选了(?<catName>.*?)公司黄页.*?/>");
                Match match = regex.Match(text2);
                if (match != null && !string.IsNullOrEmpty(match.Groups["catName"].Value))
                {
                    string value = match.Groups["catName"].Value;
                    DataTable sortDtBySysId = null;// ToolServer.ProductData.GetSortDtBySysId(7, "keys,name,del");
                    if (sortDtBySysId != null && sortDtBySysId.Rows.Count > 0)
                    {
                        DataRow[] array = sortDtBySysId.Select("name='" + value + "' and del=0");
                        if (array != null && array.Length > 0)
                        {
                            result = DataConvert.ToString(array[0]["keys"]);
                        }
                    }
                }
            }
            return result;
        }
        private ItemGetResponse GetItemGetMobileResponseByOnlinekeyNew(string response, string onlineKey, int fromType, bool snatchPromotionPrice)
        {
            ItemGetResponse itemGetResponse = null;
            try
            {
                string detailUrl = string.Format("http://item.taobao.com/item.htm?id={0}", new object[]
                {
                    onlineKey
                });
                string text = "{\"itemNumId\":\"" + onlineKey + "\"}";
                text = Uri.EscapeDataString(text);
                string.Format("http://acs.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data={0}", new object[]
                {
                    text
                });
                ItemTaobaoEntity itemTaobaoEntity = null;
                ItemJsonEntity60 itemJsonEntity = null;
                itemJsonEntity = JsonConvert.DeserializeObject<ItemJsonEntity60>(response);
                string text2 = "onsale";
                text2 = this.GetOnline(response);
                string text3 = string.Empty;
                long num = 0L;
                if (itemJsonEntity == null)
                {
                    ItemGetResponse result = null;
                    return result;
                }
                itemGetResponse = new ItemGetResponse();
                itemTaobaoEntity = new ItemTaobaoEntity();
                itemTaobaoEntity.Data = new Data();
                itemTaobaoEntity.Data.ItemInfoModel = new ItemInfoModel();
                itemTaobaoEntity.Data.ItemInfoModel.ItemId = itemJsonEntity.data.item.itemId;
                itemTaobaoEntity.Data.ItemInfoModel.Title = itemJsonEntity.data.item.title;
                itemTaobaoEntity.Data.ItemInfoModel.CategoryId = DataConvert.ToInt(itemJsonEntity.data.item.categoryId);
                itemTaobaoEntity.Data.Seller = new Seller();
                itemTaobaoEntity.Data.Seller.Nick = itemJsonEntity.data.seller.sellerNick;
                itemTaobaoEntity.Data.Seller.ShopId = itemJsonEntity.data.seller.shopId;
                if (itemJsonEntity.data != null && itemJsonEntity.data.apiStack != null && itemJsonEntity.data.apiStack.Count > 0)
                {
                    itemTaobaoEntity.Data.ItemInfoModel.Location = this.GetLocationNew(itemJsonEntity.data.apiStack[0].value);
                }
                if (itemJsonEntity.data != null && itemJsonEntity.data.props != null && itemJsonEntity.data.props.groupProps != null && itemJsonEntity.data.props.groupProps.Count > 0)
                {
                    System.Collections.Generic.List<Prop> list = new System.Collections.Generic.List<Prop>();
                    if (itemJsonEntity.data.props.groupProps[0].BaseInfo != null && itemJsonEntity.data.props.groupProps[0].BaseInfo.Count > 0)
                    {
                        foreach (System.Collections.Generic.Dictionary<string, string> current in itemJsonEntity.data.props.groupProps[0].BaseInfo)
                        {
                            foreach (string current2 in current.Keys)
                            {
                                list.Add(new Prop
                                {
                                    Name = current2,
                                    Value = current[current2]
                                });
                            }
                        }
                    }
                    itemTaobaoEntity.Data.Props = list;
                }
                if (itemJsonEntity.data != null && itemJsonEntity.data.item != null && itemJsonEntity.data.item.images != null)
                {
                    System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
                    foreach (string current3 in itemJsonEntity.data.item.images)
                    {
                        list2.Add(current3);
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
                itemTaobaoEntity.Data.SkuModel.SkuProps = new System.Collections.Generic.List<SkuPropMobile>();
                if (itemJsonEntity.data != null && itemJsonEntity.data.item != null && itemJsonEntity.data.skuBase != null && itemJsonEntity.data.skuBase.props != null)
                {
                    foreach (PropsItem current4 in itemJsonEntity.data.skuBase.props)
                    {
                        SkuPropMobile skuPropMobile = new SkuPropMobile();
                        skuPropMobile.PropName = current4.name;
                        skuPropMobile.PropId = current4.pid;
                        skuPropMobile.Values = new System.Collections.Generic.List<Value>();
                        foreach (ValuesItem current5 in current4.values)
                        {
                            Value value = new Value();
                            value.Name = current5.name;
                            value.ValueId = current5.vid;
                            value.ImgUrl = current5.image;
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
                itemTaobaoEntity.SkuList = this.GetMobilePromoPriceSkuList(response, isTaobao, itemJsonEntity, ref text3, ref num, snatchPromotionPrice, itemJsonEntity.data);
                if (!snatchPromotionPrice)
                {
                    System.Collections.Generic.List<Sku> mobilePromoPriceSkuList = this.GetMobilePromoPriceSkuList(response, isTaobao, itemJsonEntity, ref text3, ref num, true, itemJsonEntity.data);
                    if (mobilePromoPriceSkuList != null && mobilePromoPriceSkuList.Count > 0)
                    {
                        using (System.Collections.Generic.List<Sku>.Enumerator enumerator6 = itemTaobaoEntity.SkuList.GetEnumerator())
                        {
                            while (enumerator6.MoveNext())
                            {
                                Sku tmpSku = enumerator6.Current;
                                Sku sku = mobilePromoPriceSkuList.Find((Sku p) => p.SkuId == tmpSku.SkuId);
                                if (sku != null)
                                {
                                    tmpSku.Quantity = sku.Quantity;
                                }
                            }
                        }
                    }
                }
                text3 = this.GetMobilePrice(itemTaobaoEntity.SkuList, response, snatchPromotionPrice, text3, itemTaobaoEntity.Data.ItemInfoModel.ItemId);
                num = this.GetMobileNums(itemTaobaoEntity.SkuList, response);
                bool flag = false;
                bool flag2 = false;// DataConvert.ToBoolean(DataHelper.GetUserConfig("AppConfig", this._toolCode.ToUpper(), "SnacthFilter", "false"));
                if (text2 != "onsale" && flag2)
                {
                    itemGetResponse.ErrCode = "19";
                    itemGetResponse.ErrMsg = "未抓取到商品，该商品已删除或已下架";
                    ItemGetResponse result = itemGetResponse;
                    return result;
                }
                if (text2 != "onsale")
                {
                    if (itemTaobaoEntity != null && itemTaobaoEntity.SkuList != null && itemTaobaoEntity.SkuList.Count > 0)
                    {
                        long num2 = 0L;
                        foreach (Sku current6 in itemTaobaoEntity.SkuList)
                        {
                            current6.Quantity = 100L;
                            num2 += current6.Quantity;
                        }
                        num = num2;
                    }
                    else
                    {
                        num = 100L;
                    }
                }
                //bool flag3 = DataConvert.ToBoolean(DataHelper.GetUserConfig("AppConfig", this._toolCode.ToUpper(), "WangWangFilter", "false"));
                //if (flag3)
                //{
                //    System.Collections.Generic.IList<UserSetData> userSetDataList = this.UserSetDataService.GetUserSetDataList(1, 1);
                //    if (userSetDataList != null && userSetDataList.Count > 0 && itemTaobaoEntity != null && itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.Seller != null)
                //    {
                //        string nick = itemTaobaoEntity.Data.Seller.Nick;
                //        if (!string.IsNullOrEmpty(nick))
                //        {
                //            for (int i = 0; i < userSetDataList.Count; i++)
                //            {
                //                if (nick == userSetDataList[i].Name)
                //                {
                //                    ItemGetResponse result = itemGetResponse;
                //                    return result;
                //                }
                //            }
                //        }
                //    }
                //}
                if (flag)
                {
                    if (text2 != "onsale")
                    {
                        itemGetResponse.ErrCode = "19";
                        itemGetResponse.ErrMsg = "未抓取到商品，该商品已删除或已下架";
                    }
                    else
                    {
                        itemGetResponse.ErrCode = "18";
                        itemGetResponse.ErrMsg = "未抓取到商品，该商品有可能已删除或下架";
                    }
                    ItemGetResponse result = itemGetResponse;
                    return result;
                }
                string title = string.Empty;
                string text4 = string.Empty;
                string o = string.Empty;
                string text5 = string.Empty;
                string wirelessDesc = string.Empty;
                string text6 = string.Empty;
                string pcDescUrl = string.Empty;
                string arg_892_0 = string.Empty;
                string arg_898_0 = string.Empty;
                string loctionStr = string.Empty;
                string nick2 = string.Empty;
                string arg_8AC_0 = string.Empty;
                string empty = string.Empty;
                string arg_8B9_0 = string.Empty;
                string stuffStatus = string.Empty;
                string freightPayer = string.Empty;
                string postFee = string.Empty;
                string expressFee = string.Empty;
                string emsFee = string.Empty;
                System.Collections.Generic.List<ItemImg> itemImgs = new System.Collections.Generic.List<ItemImg>();
                System.Collections.Generic.List<PropImg> propImgs = new System.Collections.Generic.List<PropImg>();
                System.Collections.Generic.IList<SellProInfo> sellProInfoList = new System.Collections.Generic.List<SellProInfo>();
                System.Collections.Generic.List<Sku> list3 = null;
                System.Collections.Generic.Dictionary<string, string> dicProNameAndProValue = null;
                if (itemTaobaoEntity != null && itemTaobaoEntity.Data != null && itemTaobaoEntity.Data.ItemInfoModel != null)
                {
                    title = itemTaobaoEntity.Data.ItemInfoModel.Title;
                    text4 = DataConvert.ToString(itemTaobaoEntity.Data.ItemInfoModel.CategoryId);
                    loctionStr = itemTaobaoEntity.Data.ItemInfoModel.Location;
                    o = itemTaobaoEntity.Data.ItemInfoModel.ItemId;
                    freightPayer = itemTaobaoEntity.Data.ItemInfoModel.FreightPayer;
                    postFee = itemTaobaoEntity.Data.ItemInfoModel.PostFee;
                    expressFee = itemTaobaoEntity.Data.ItemInfoModel.ExpressFee;
                    emsFee = itemTaobaoEntity.Data.ItemInfoModel.EmsFee;
                    // itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "二手";
                    if (itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "二手")
                    {
                        stuffStatus = "second";
                    }
                    else
                    {
                        if (itemTaobaoEntity.Data.ItemInfoModel.StuffStatus == "个人闲置")
                        {
                            stuffStatus = "unused";
                        }
                        else
                        {
                            stuffStatus = "new";
                        }
                    }
                    if (itemTaobaoEntity.Data.Seller != null)
                    {
                        Seller seller = itemTaobaoEntity.Data.Seller;
                        string arg_A40_0 = seller.ShopId;
                        nick2 = seller.Nick;
                        // itemTaobaoEntity.Data.ItemInfoModel.ItemTypeName == "tmall";
                    }
                    if (itemTaobaoEntity.Data.ItemInfoModel.picsPath != null && itemTaobaoEntity.Data.ItemInfoModel.picsPath.Count > 0)
                    {
                        itemImgs = this.GetItemImgList(itemTaobaoEntity.Data.ItemInfoModel.picsPath);
                    }
                    if (itemTaobaoEntity.Data.DescInfo != null && !string.IsNullOrEmpty(itemTaobaoEntity.Data.DescInfo.PcDescUrl))
                    {
                        pcDescUrl = itemTaobaoEntity.Data.DescInfo.PcDescUrl;
                        string arg_AED_0 = itemTaobaoEntity.Data.DescInfo.FullDescUrl;
                        text6 = itemTaobaoEntity.Data.DescInfo.H5DescUrl;
                        string arg_B10_0 = itemTaobaoEntity.Data.DescInfo.BriefDescUrl;
                        text5 = this.GetpcDescContent(pcDescUrl);
                        wirelessDesc = this.GetWirelessDescContent(text6, text5);
                    }
                    if (itemTaobaoEntity.Data.SkuModel != null && itemTaobaoEntity.Data.SkuModel.SkuProps != null && itemTaobaoEntity.Data.SkuModel.SkuProps.Count > 0)
                    {
                        System.Collections.Generic.List<SkuPropMobile> skuProps = itemTaobaoEntity.Data.SkuModel.SkuProps;
                        propImgs = this.GetMobileProImgList(skuProps, sellProInfoList);
                    }
                    if (itemTaobaoEntity.Data.Props != null && itemTaobaoEntity.Data.Props.Count > 0)
                    {
                        dicProNameAndProValue = this.GetMobileItemProperty(itemTaobaoEntity.Data.Props, ref empty);
                    }
                    if (itemTaobaoEntity.SkuList != null && itemTaobaoEntity.SkuList.Count > 0)
                    {
                        list3 = itemTaobaoEntity.SkuList;
                    }
                }
                Item item = new Item();
                item.ApproveStatus = text2;
                item.Cid = DataConvert.ToLong(text4);
                item.Title = title;
                item.Desc = text5;
                item.Nick = nick2;
                item.FreightPayer = freightPayer;
                item.ExpressFee = expressFee;
                item.EmsFee = emsFee;
                item.PostFee = postFee;
                item.DetailUrl = detailUrl;
                item.WirelessDesc = wirelessDesc;
                item.WapDetailUrl = text6;
                item.HasDiscount = false;
                item.HasInvoice = false;
                item.HasShowcase = true;
                item.HasWarranty = true;
                item.IsTiming = false;
                item.ItemImgs = itemImgs;
                item.NumIid = DataConvert.ToLong(o);
                item.PostageId = 0L;
                item.PropImgs = propImgs;
                string empty2 = string.Empty;
                string empty3 = string.Empty;
                string empty4 = string.Empty;
                string empty5 = string.Empty;
                if (fromType == 0)
                {
                    GatherTaobaoUseWebService.GetPropertyStrForLocalSnatch(dicProNameAndProValue, sellProInfoList, text4, out empty2, out empty3, out empty4, out empty5);
                }
                else
                {
                    GatherTaobaoUseWebService.GetPropertyStrForCloudSnatch(dicProNameAndProValue, sellProInfoList, out empty2, out empty3);
                }
                FoodSecurity foodSecurity = this.GetFoodSecurity(dicProNameAndProValue);
                if (foodSecurity != null && (foodSecurity.PrdLicenseNo != null || foodSecurity.DesignCode != null || foodSecurity.Factory != null || foodSecurity.FactorySite != null || foodSecurity.Contact != null || foodSecurity.Mix != null || foodSecurity.PlanStorage != null || foodSecurity.Period != null || foodSecurity.FoodAdditive != "无" || foodSecurity.HealthProductNo != null || foodSecurity.Supplier != null))
                {
                    string empty6 = string.Empty;
                    string empty7 = string.Empty;
                    this.FoodSecurityDate(onlineKey, ref empty6, ref empty7);
                    foodSecurity.ProductDateStart = empty6;
                    foodSecurity.ProductDateEnd = empty7;
                    foodSecurity.StockDateStart = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
                    foodSecurity.StockDateEnd = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
                item.Props = empty2;
                item.PropsName = empty3;
                item.InputPids = empty4;
                item.InputStr = empty5;
                item.FoodSecurity = foodSecurity;
                item.SellPromise = true;
                item.StuffStatus = "new";
                item.Type = "fixed";
                item.ValidThru = 7L;
                item.Violation = false;
                item.Barcode = empty;
                item.Skus = list3;
                item.Location = this.GetLoction(loctionStr);
                item.Num = num;
                bool flag4 = false;// DataConvert.ToBoolean(DataHelper.GetUserConfig("AppConfig", this._toolCode.ToUpper(), "SnacthCPrice", "false"));
                if (flag4)
                {
                    item.Price = this.GetGoodsCPrice(list3);
                    if (DataConvert.ToDecimal(item.Price) <= 0m)
                    {
                        item.Price = text3;
                    }
                }
                else
                {
                    item.Price = text3;
                }
                item.StuffStatus = stuffStatus;
                itemGetResponse.Item = item;
            }
            catch (System.Exception ex)
            {
                Log.WriteLog("下载淘宝商品信息出现异常", ex);
            }
            return itemGetResponse;
        }
        public System.Collections.Generic.List<Sku> GetMobilePromoPriceSkuList(string newResContent, bool isTaobao, ItemJsonEntity60 itemJsonEntity60, ref string price, ref long num, bool snatchPromotionPrice, Data60 itemData)
        {
            price = "";
            num = 0L;
            long num2 = 0L;
            System.Collections.Generic.Dictionary<string, Sku2Info> dictionary = new System.Collections.Generic.Dictionary<string, Sku2Info>();
            System.Collections.Generic.Dictionary<string, Sku2Info_T> dictionary2 = new System.Collections.Generic.Dictionary<string, Sku2Info_T>();
            if (!string.IsNullOrEmpty(newResContent))
            {
                string pattern = "\\\"value\\\".*?\\\\\"skuCore\\\\\":(?<skuCore>.*?),\\\\\"skuItem\\\\\"";
                if (isTaobao)
                {
                    if (!snatchPromotionPrice && itemData != null && !string.IsNullOrEmpty(itemData.mockData))
                    {
                        pattern = "\\\"skuCore\\\":(?<skuCore>.*?),\\\"skuItem\\\"";
                        newResContent = itemData.mockData;
                    }
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match = regex.Match(newResContent);
                    if (match != null && match.Groups["skuCore"].Success && !string.IsNullOrEmpty(match.Groups["skuCore"].Value))
                    {
                        string value = match.Groups["skuCore"].Value.Replace("\\\"", "\"") + "}";
                        ItemTaobaoSkuCore itemTaobaoSkuCore = JsonConvert.DeserializeObject<ItemTaobaoSkuCore>(value);
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
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match2 = regex.Match(newResContent);
                    if (match2 != null && match2.Success && !string.IsNullOrEmpty(match2.Groups["skuInfo"].Value))
                    {
                        string text = match2.Groups["skuInfo"].Value.Replace("\\\"", "\"");
                        if (!string.IsNullOrEmpty(text))
                        {
                            if (text.Contains("services"))
                            {
                                regex = new Regex("(?is),\"services\":.*?}][^,]", RegexOptions.IgnoreCase);
                                text = regex.Replace(text, string.Empty);
                            }
                            pattern = "(?is)\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity)[^}]*?}),?|\"(?<pathId>\\d+)\":(?<priceUnits>.*?(quantityText|quantity).*?}]})}?,?";
                            regex = new Regex(pattern, RegexOptions.IgnoreCase);
                            MatchCollection matchCollection = regex.Matches(text);
                            if (matchCollection != null && matchCollection.Count > 0)
                            {
                                foreach (Match match3 in matchCollection)
                                {
                                    if (match3 != null && match3.Success && !string.IsNullOrEmpty(match3.Groups["pathId"].Value))
                                    {
                                        string text2 = DataConvert.ToString(match3.Groups["priceUnits"].Value);
                                        if (!string.IsNullOrEmpty(text2))
                                        {
                                            Regex regex2 = new Regex("(?is)\"subPrice\":.*?\"priceText\":\"(?<priceText>.*?)\".*?\"quantity\":\"(?<quantity>.*?)\",?", RegexOptions.IgnoreCase);
                                            Match match4 = regex2.Match(text2);
                                            if (match4 != null && match4.Success)
                                            {
                                                Sku2Info_T sku2Info_T = new Sku2Info_T();
                                                sku2Info_T.price = new Price();
                                                sku2Info_T.price.priceText = DataConvert.ToString(match4.Groups["priceText"].Value);
                                                sku2Info_T.quantity = DataConvert.ToString(match4.Groups["quantity"].Value);
                                                dictionary2[DataConvert.ToString(match3.Groups["pathId"].Value)] = sku2Info_T;
                                            }
                                            else
                                            {
                                                regex2 = new Regex("(?is)\"priceText\":\"(?<priceText>.*?)\".*?\"quantity\":\"?(?<quantity>.*?)\"?(,|})", RegexOptions.IgnoreCase);
                                                match4 = regex2.Match(text2);
                                                if (match4 != null && match4.Success)
                                                {
                                                    Sku2Info_T sku2Info_T2 = new Sku2Info_T();
                                                    sku2Info_T2.price = new Price();
                                                    sku2Info_T2.price.priceText = DataConvert.ToString(match4.Groups["priceText"].Value);
                                                    sku2Info_T2.quantity = DataConvert.ToString(match4.Groups["quantity"].Value);
                                                    dictionary2[DataConvert.ToString(match3.Groups["pathId"].Value)] = sku2Info_T2;
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
            System.Collections.Generic.List<Sku> list = new System.Collections.Generic.List<Sku>();
            if (itemJsonEntity60.data.skuBase.skus != null)
            {
                foreach (SkusItem current in itemJsonEntity60.data.skuBase.skus)
                {
                    Sku sku = new Sku();
                    sku.Properties = current.propPath;
                    sku.SkuId = DataConvert.ToLong(current.skuId);
                    string text3 = "";
                    string[] array = current.propPath.Split(new char[]
                    {
                        ';'
                    });
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text4 = array2[i];
                        if (!string.IsNullOrEmpty(text4) && text4.Split(new char[]
                        {
                            ':'
                        }).Length == 2)
                        {
                            string text5 = text4.Split(new char[]
                            {
                                ':'
                            })[0];
                            string text6 = text4.Split(new char[]
                            {
                                ':'
                            })[1];
                            foreach (PropsItem current2 in itemJsonEntity60.data.skuBase.props)
                            {
                                if (current2.pid == text5)
                                {
                                    string name = current2.name;
                                    bool flag = false;
                                    foreach (ValuesItem current3 in current2.values)
                                    {
                                        if (current3.vid == text6)
                                        {
                                            string name2 = current3.name;
                                            string text7 = text3;
                                            text3 = string.Concat(new string[]
                                            {
                                                text7,
                                                text5,
                                                ":",
                                                text6,
                                                ":",
                                                name,
                                                ":",
                                                name2,
                                                ";"
                                            });
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
                    sku.PropertiesName = text3.Trim(new char[]
                    {
                        ';'
                    });
                    list.Add(sku);
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
                            decimal num3 = DataConvert.ToDecimal(sku2Info.price.priceText);
                            DataConvert.ToDecimal(list[j].Price);
                            if (num3 > 0m)
                            {
                                list[j].Price = System.Math.Round(num3, 2).ToString();
                                list[j].Quantity = DataConvert.ToLong(sku2Info.quantity);
                                if (string.IsNullOrEmpty(price) && list[j].Quantity > 0L)
                                {
                                    price = list[j].Price;
                                }
                                else
                                {
                                    if (DataConvert.ToDecimal(price) > DataConvert.ToDecimal(list[j].Price) && list[j].Quantity > 0L)
                                    {
                                        price = list[j].Price;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dictionary2 != null && dictionary2.Count > 0 && dictionary2.ContainsKey(list[j].SkuId.ToString()))
                        {
                            Sku2Info_T sku2Info_T3 = dictionary2[list[j].SkuId.ToString()];
                            decimal num4 = DataConvert.ToDecimal(sku2Info_T3.price.priceText);
                            decimal num5 = DataConvert.ToDecimal(list[j].Price);
                            if (num4 < num5 || num5 == 0m)
                            {
                                list[j].Price = System.Math.Round(num4, 2).ToString();
                                list[j].Quantity = DataConvert.ToLong(sku2Info_T3.quantity);
                                if (string.IsNullOrEmpty(price) && list[j].Quantity > 0L)
                                {
                                    price = sku2Info_T3.price.priceText;
                                }
                                if (DataConvert.ToDecimal(price) > DataConvert.ToDecimal(num4) && list[j].Quantity > 0L)
                                {
                                    price = list[j].Price;
                                }
                            }
                        }
                    }
                    num2 += list[j].Quantity;
                }
            }
            else
            {
                if (dictionary.ContainsKey("0"))
                {
                    Sku2Info sku2Info2 = dictionary["0"];
                    price = sku2Info2.price.priceText;
                    num2 = DataConvert.ToLong(sku2Info2.quantity);
                }
            }
            if (list != null && list.Count > 0)
            {
                foreach (Sku current4 in list)
                {
                    if (DataConvert.ToDecimal(current4.Price) <= 0m && DataConvert.ToLong(current4.Quantity) == 0L)
                    {
                        current4.Price = price;
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
        public ItemGetResponse TbResponseHandle(string response, string onlineKey, bool isSnatchPromoPrice)
        {
            return this.GetItemGetMobileResponseByOnlinekeyNew(response, onlineKey, 0, isSnatchPromoPrice);
        }
        public bool GetResponseFromTaobao(string onlineKey, bool isSnatchPromoPrice, out string response)
        {
            return this.GetMobileResponse(onlineKey, 0, isSnatchPromoPrice, out response);
        }
        private bool GetMobileResponse(string onlineKey, int fromType, bool snatchPromotionPrice, out string response)
        {
            response = null;
            try
            {
                string.Format("http://item.taobao.com/item.htm?id={0}", new object[]
                {
                    onlineKey
                });
                string text = "{\"itemNumId\":\"" + onlineKey + "\"}";
                text = Uri.EscapeDataString(text);
                string text2 = string.Format("http://acs.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?data={0}", new object[]
                {
                    text
                });
                int num = 1;
                response = this.GetResponseResultBody(text2, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text2, ref num);
                if (string.IsNullOrEmpty(response) || !response.Contains("{\"apiStack\":[{\"name\":\"esi\",\"value\":\"{"))
                {
                    int num2 = 0;
                    do
                    {
                        if (string.IsNullOrEmpty(response) || !response.Contains("{\"apiStack\":[{\"name\":\"esi\",\"value\":\"{"))
                        {
                            num2++;
                            num = 1;
                            System.Threading.Thread.Sleep(2000);
                            response = this.GetResponseResultBody(text2, System.Text.Encoding.GetEncoding("utf-8"), "GET", true, text2, ref num);
                        }
                        else
                        {
                            num2 = 10;
                        }
                    }
                    while (num2 < 10);
                }
                try
                {
                    ItemJsonEntity60 itemJsonEntity = JsonConvert.DeserializeObject<ItemJsonEntity60>(response);
                    if (itemJsonEntity != null && itemJsonEntity.data.item == null && response.Contains("FAIL_SYS_USER_VALIDATE"))
                    {
                        bool result = false;
                        return result;
                    }
                }
                catch (System.Exception ex)
                {
                    Log.WriteLog("反序列化6.0失败，内容:" + response);
                    Log.WriteLog(ex.Message);
                    bool result = false;
                    return result;
                }
            }
            catch (System.Exception ex2)
            {
                Log.WriteLog("下载淘宝商品信息出现异常", ex2);
                bool result = false;
                return result;
            }
            return true;
        }

        public override IList<ProductInfo> SnatchItems(string shopNick, bool isTmall, long categoryId)
        {
            throw new NotImplementedException();
        }

        public override IList<ProductInfo> SnatchItemsNew(string shopNick, bool isTmall, long categoryId, int pageNum, string shopId)
        {
            throw new NotImplementedException();
        }

        public override int GetTotalPage(string shopNick, bool isTmall, long categoryId, out string shopId)
        {
            throw new NotImplementedException();
        }
    }
}
