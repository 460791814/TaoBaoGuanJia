using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TaoBaoGuanJia.Util;
using Top.Api.Domain;

namespace TaoBaoGuanJia.Model
{
    public static class GetItemPromoPrice
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
            public Dictionary<string, GetItemPromoPrice.AvailSKU> availSKUs
            {
                get;
                set;
            }
            public List<GetItemPromoPrice.SkuProp> skuProps
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
            public List<GetItemPromoPrice.value> values
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
        public static GetItemPromoPrice.TaobaoPromoPriceEntity GetTaobaoItemPromoPrice(string onlineKey)
        {
            GetItemPromoPrice.TaobaoPromoPriceEntity result = null;
            string url = "http://a.m.taobao.com/ajax/sku.do?item_id=" + onlineKey;
            string responseContent = GetItemPromoPrice.GetResponseContent(url, Encoding.GetEncoding("Utf-8"));
            try
            {
                if (!string.IsNullOrEmpty(responseContent))
                {
                    result = JsonConvert.DeserializeObject<GetItemPromoPrice.TaobaoPromoPriceEntity>(responseContent);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex);
            }
            return result;
        }
        public static decimal TaobaoPromoMobilePrice(string onlineKey, List<Sku> skuList)
        {
            string.Format("http://item.taobao.com/item.htm?id={0}", new object[]
            {
                onlineKey
            });
            string text = string.Format("http://hws.m.taobao.com/cache/wdetail/5.0/?id={0}", new object[]
            {
                onlineKey
            });
            string text2 = GetItemPromoPrice.Invoke(text, text, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(text2.Trim()))
            {
                return GetItemPromoPrice.GetDiscountPrice(text2, skuList);
            }
            return 0m;
        }
        private static decimal GetDiscountPrice(string rsContent, List<Sku> skuList)
        {
            string pattern = "\"apiStack\":.*?installmentEnable";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(rsContent);
            if (match != null && match.Success)
            {
                rsContent = match.Value;
            }
            decimal num = 0m;
            if (DataConvert.ToDecimal(num) <= 0m && skuList != null && skuList.Count > 0)
            {
                foreach (Sku current in skuList)
                {
                    if (num == 0m)
                    {
                        num = DataConvert.ToDecimal(current.Price);
                    }
                    else
                    {
                        if (DataConvert.ToDecimal(num) > DataConvert.ToDecimal(current.Price))
                        {
                            num = DataConvert.ToDecimal(current.Price);
                        }
                    }
                }
            }
            return num;
        }
        public static GetItemPromoPrice.TaobaoPromoPriceEntity GetTaobaoItemPromoPrice2(string onlineKey)
        {
            GetItemPromoPrice.TaobaoPromoPriceEntity taobaoPromoPriceEntity = new GetItemPromoPrice.TaobaoPromoPriceEntity();
            string format = "http://detailskip.taobao.com/json/sib.htm?itemId={0}&u=1&p=1&chnl=pc";
            string format2 = "http://item.taobao.com/item.htm?id={0}";
            string strUrl = string.Format(format, onlineKey);
            string refreUrl = string.Format(format2, onlineKey);
            string text = GetItemPromoPrice.Invoke(strUrl, refreUrl, Encoding.GetEncoding("gb2312"));
            if (!string.IsNullOrEmpty(text.Trim()))
            {
                Regex regex = new Regex("(?is)g_config.PromoData=(.*?}\\s*?);");
                Match match = regex.Match(text.Trim());
                string value = string.Empty;
                if (match.Success)
                {
                    value = match.Groups[1].Value;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        Dictionary<string, List<GetItemPromoPrice.PromoEntity>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<GetItemPromoPrice.PromoEntity>>>(value);
                        if (dictionary != null && dictionary.Count >= 0)
                        {
                            foreach (KeyValuePair<string, List<GetItemPromoPrice.PromoEntity>> current in dictionary)
                            {
                                if (current.Value != null && current.Value.Count > 0)
                                {
                                    string key = current.Key.Trim(new char[]
                                    {
                                        ';'
                                    });
                                    decimal num = 0m;
                                    foreach (GetItemPromoPrice.PromoEntity current2 in current.Value)
                                    {
                                        if (!string.IsNullOrEmpty(current2.Price))
                                        {
                                            if (num == 0m)
                                            {
                                                num = DataConvert.ToDecimal(current2.Price);
                                            }
                                            else
                                            {
                                                if (num > DataConvert.ToDecimal(current2.Price))
                                                {
                                                    num = DataConvert.ToDecimal(current2.Price);
                                                }
                                            }
                                        }
                                    }
                                    if (num != 0m)
                                    {
                                        if (taobaoPromoPriceEntity.availSKUs == null)
                                        {
                                            taobaoPromoPriceEntity.availSKUs = new Dictionary<string, GetItemPromoPrice.AvailSKU>();
                                        }
                                        taobaoPromoPriceEntity.availSKUs[key] = new GetItemPromoPrice.AvailSKU();
                                        taobaoPromoPriceEntity.availSKUs[key].promoPrice = DataConvert.ToString(num);
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
            }
            return taobaoPromoPriceEntity;
        }
        private static string Invoke(string strUrl, string refreUrl, Encoding encoding)
        {
            string empty = string.Empty;
            CommonApiClient commonApiClient = new CommonApiClient();
            return commonApiClient.Invoke(strUrl, encoding, "GET", false, refreUrl);
        }
        public static Dictionary<string, string> GetPaipaiItemPromoPrice(string onlineKey)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string text = string.Empty;
            string url = "http://auction1.paipai.com/" + onlineKey;
            string responseContent = GetItemPromoPrice.GetResponseContent(url, Encoding.GetEncoding("gb2312"));
            if (!string.IsNullOrEmpty(responseContent))
            {
                Regex regex = new Regex("(?is)<input[^>]*?id=\"stockStringNew\"\\s*?value=\"(?<value>.*?)\"", RegexOptions.IgnoreCase);
                Match match = regex.Match(responseContent);
                if (match != null && match.Success)
                {
                    text = match.Groups["value"].Value;
                }
            }
            if (!string.IsNullOrEmpty(text))
            {
                string[] array = text.Split(new char[]
                {
                    ';'
                }, StringSplitOptions.RemoveEmptyEntries);
                if (array != null && array.Length > 0)
                {
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text2 = array2[i];
                        string[] array3 = text2.Split(new char[]
                        {
                            ','
                        });
                        if (array3 != null && array3.Length == 5)
                        {
                            string key = array3[4];
                            string value = array3[1];
                            dictionary[key] = value;
                        }
                    }
                }
            }
            return dictionary;
        }
        private static string GetResponseContent(string url, Encoding encoding)
        {
            string empty = string.Empty;
            CommonApiClient commonApiClient = new CommonApiClient();
            return commonApiClient.Invoke(url, encoding, "GET", false);
        }
    }
}
