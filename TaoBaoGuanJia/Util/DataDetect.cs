using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace TaoBaoGuanJia.Util
{
	public class DataDetect
	{
		public static bool IsValidUrl(string url, bool needTestConn)
		{
			bool result = false;
			if (string.IsNullOrEmpty(url))
			{
				return result;
			}
			if (url.ToLower().Trim() == "about:blank")
			{
				return true;
			}
			HttpWebResponse httpWebResponse = null;
			try
			{
				string pattern = "^http[s]?://([\\w-]+\\.)*[\\w-]+(:[\\d]*)?(/.*)?$";
				if (Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase))
				{
					if (!needTestConn)
					{
						return true;
					}
					HttpWebRequest httpWebRequest = WebRequestFactory.GetHttpWebRequest(url);
					httpWebRequest.Timeout = 6000;
					httpWebRequest.ReadWriteTimeout = 10000;
					httpWebRequest.KeepAlive = false;
					httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					return true;
				}
				return result;
			}
			catch
			{
				return false;
			}
			finally
			{
				httpWebResponse?.Close();
			}
		}

		public static bool IsInt(object o)
		{
			if (o == null)
			{
				return false;
			}
			return Regex.IsMatch(o.ToString(), "^-*\\d*$");
		}

		public static bool IsNumber(object o)
		{
			try
			{
				if (o != null && o != DBNull.Value)
				{
					double num = 0.0;
					return double.TryParse(o.ToString().Trim(), out num);
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsDate(object o)
		{
			if (o != null)
			{
				if (!Regex.IsMatch(o.ToString(), "\\d{4}-\\d{2}-\\d{2}") && !Regex.IsMatch(o.ToString(), "\\d{4}-\\d{1}-\\d{2}") && !Regex.IsMatch(o.ToString(), "\\d{4}-\\d{1}-\\d{1}") && !Regex.IsMatch(o.ToString(), "\\d{4}-\\d{2}-\\d{1}"))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool IsEmail(object o)
		{
			if (o != null && !o.ToString().EndsWith("@"))
			{
				if (o.ToString().IndexOf('@') >= 1)
				{
					if (o.ToString().Substring(o.ToString().IndexOf('@') + 1).IndexOf('@') >= 0)
					{
						return false;
					}
					if (!Regex.IsMatch(o.ToString(), "\\w+@\\w+[.]{1}[a-zA-Z]+"))
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool IsIdCard(string idCard)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("11", "北京");
			hashtable.Add("12", "天津");
			hashtable.Add("13", "河北");
			hashtable.Add("14", "山西");
			hashtable.Add("15", "内蒙古");
			hashtable.Add("21", "辽宁");
			hashtable.Add("22", "吉林");
			hashtable.Add("23", "黑龙江");
			hashtable.Add("31", "上海");
			hashtable.Add("32", "江苏");
			hashtable.Add("33", "浙江");
			hashtable.Add("34", "安徽");
			hashtable.Add("35", "福建");
			hashtable.Add("36", "江西");
			hashtable.Add("37", "山东");
			hashtable.Add("41", "河南");
			hashtable.Add("42", "湖北");
			hashtable.Add("43", "湖南");
			hashtable.Add("44", "广东");
			hashtable.Add("45", "广西");
			hashtable.Add("46", "海南");
			hashtable.Add("50", "重庆");
			hashtable.Add("51", "四川");
			hashtable.Add("52", "贵州");
			hashtable.Add("53", "云南");
			hashtable.Add("54", "西藏");
			hashtable.Add("61", "陕西");
			hashtable.Add("62", "甘肃");
			hashtable.Add("63", "青海");
			hashtable.Add("64", "宁夏");
			hashtable.Add("65", "新疆");
			hashtable.Add("71", "台湾");
			hashtable.Add("81", "香港");
			hashtable.Add("82", "澳门");
			hashtable.Add("91", "国外");
			if (idCard != null && idCard.Length > 2)
			{
				bool flag = false;
				IEnumerator enumerator = hashtable.Keys.GetEnumerator();
				int num = 0;
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current.ToString() == idCard.Substring(0, 2)))
					{
						num++;
						continue;
					}
					flag = true;
					break;
				}
				if (!flag)
				{
					return false;
				}
				bool flag2 = false;
				switch (idCard.Length)
				{
				case 15:
					flag2 = Chk15IdCard(idCard);
					break;
				case 18:
					flag2 = Chk18IdCard(idCard);
					break;
				}
				if (flag2)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		private static bool Chk15IdCard(string idCard)
		{
			int num = int.Parse(idCard.Substring(6, 2)) + 1900;
			string text = "";
			if (num % 4 != 0 && (num % 100 != 0 || num % 4 != 0))
			{
				text = "^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$";
				return Regex.IsMatch(idCard, text);
			}
			text = "^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$";
			return Regex.IsMatch(idCard, text);
		}

		private static bool Chk18IdCard(string idCard)
		{
			int num = int.Parse(idCard.Substring(6, 2)) + 1900;
			string text = "";
			text = ((num % 4 != 0 && (num % 100 != 0 || num % 4 != 0)) ? "^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$" : "^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$");
			if (Regex.IsMatch(idCard, text))
			{
				int num2 = (int.Parse(idCard.Substring(0, 1)) + int.Parse(idCard.Substring(10, 1))) * 7 + (int.Parse(idCard.Substring(1, 1)) + int.Parse(idCard.Substring(11, 1))) * 9 + (int.Parse(idCard.Substring(2, 1)) + int.Parse(idCard.Substring(12, 1))) * 10 + (int.Parse(idCard.Substring(3, 1)) + int.Parse(idCard.Substring(13, 1))) * 5 + (int.Parse(idCard.Substring(4, 1)) + int.Parse(idCard.Substring(14, 1))) * 8 + (int.Parse(idCard.Substring(5, 1)) + int.Parse(idCard.Substring(15, 1))) * 4 + (int.Parse(idCard.Substring(6, 1)) + int.Parse(idCard.Substring(16, 1))) * 2 + int.Parse(idCard.Substring(7, 1)) + int.Parse(idCard.Substring(8, 1)) * 6 + int.Parse(idCard.Substring(9, 1)) * 3;
				int startIndex = Convert.ToInt32(num2 % 11);
				string text2 = "F";
				string text3 = "10X98765432";
				text2 = text3.Substring(startIndex, 1);
				if (text2 == idCard.Substring(17, 1))
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool IsExceedLength(object o, int length)
		{
			if (o != null)
			{
				if (o.ToString().Length > length)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool IsIncludeStr(string strValue, string token)
		{
			if (strValue != null)
			{
				if (strValue.IndexOf(token) > 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool IsExistFile(string fileName)
		{
			if (File.Exists(fileName))
			{
				return true;
			}
			return false;
		}

		public static bool IsValidDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				return false;
			}
			return true;
		}

		public static bool IsMobile(string strValue)
		{
			if (strValue != null)
			{
				Regex regex = new Regex("^(1)\\d{10}$");
				return regex.IsMatch(strValue);
			}
			return false;
		}
	}
}
