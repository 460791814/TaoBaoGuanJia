using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace TaoBaoGuanJia.Util
{
	public class CommonApiClient
	{
		public class BaseField
		{
			public string Key
			{
				get;
				set;
			}

			public string Value
			{
				get;
				set;
			}

			public BaseField()
			{
			}

			public BaseField(string key, string value)
			{
				Key = key;
				Value = value;
			}
		}

		public class StringField : BaseField
		{
			public StringField(string key, string value)
				: base(key, value)
			{
			}
		}

		private const string SEPARATOR = "&";

		private Dictionary<string, BaseField> dictParm;

		public CommonApiClient()
		{
			dictParm = new Dictionary<string, BaseField>();
		}

		private string Invoke(string strUrl, Encoding encoding, string method, bool isIE, string referer, CookieCollection cookieCollection, int tryNum)
		{
			bool flag = false;
			string result = string.Empty;
			HttpWebResponse httpWebResponse = null;
			StreamReader streamReader = null;
			Stream stream = null;
			HttpWebRequest httpWebRequest = null;
			try
			{
				if (string.IsNullOrEmpty(strUrl))
				{
					return string.Empty;
				}
				if (strUrl.StartsWith("//"))
				{
					strUrl = "http:" + strUrl;
				}
				httpWebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
				httpWebRequest.Method = method.ToString();
				httpWebRequest.ServicePoint.Expect100Continue = false;
				if (isIE)
				{
					httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; Chrome)";
				}
				else
				{
					httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
				}
				httpWebRequest.KeepAlive = false;
				if (!string.IsNullOrEmpty(referer))
				{
					httpWebRequest.Referer = referer;
				}
				if (cookieCollection != null)
				{
					httpWebRequest.CookieContainer = new CookieContainer();
					httpWebRequest.CookieContainer.Add(cookieCollection);
				}
				if (method.ToUpper().Equals("POST"))
				{
					byte[] array = null;
					RetriveParamInURL(strUrl);
					array = GetMultipartData(dictParm, "&");
					httpWebRequest.ContentType = "multipart/form-data; boundary=&";
					httpWebRequest.ContentLength = array.Length;
					stream = httpWebRequest.GetRequestStream();
					stream.Write(array, 0, array.Length);
					stream.Close();
					stream.Dispose();
				}
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				stream = httpWebResponse.GetResponseStream();
				streamReader = new StreamReader(stream, encoding);
				result = streamReader.ReadToEnd();
			}
			catch (Exception ex)
			{
				//Log.WriteLog("访问接口失败(" + strUrl + ")，", ex);
				if (ex.Message.IndexOf("SSL/TLS", StringComparison.OrdinalIgnoreCase) < 0 && ex.Message.IndexOf("超时", StringComparison.OrdinalIgnoreCase) < 0 && ex.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) < 0 && ex.Message.IndexOf("连接已经关闭", StringComparison.OrdinalIgnoreCase) < 0)
				{
					goto end_IL_0153;
				}
				if (ex.InnerException != null && ex.InnerException.Message != null && ex.InnerException.Message.IndexOf(" EOF ", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
				}
				if (tryNum < 3)
				{
					tryNum++;
					flag = true;
				}
				end_IL_0153:;
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
					streamReader.Dispose();
				}
				if (stream != null)
				{
					stream.Close();
					stream.Dispose();
				}
				httpWebResponse?.Close();
				httpWebRequest?.Abort();
			}
			if (flag)
			{
				Thread.Sleep(1000);
				return Invoke(strUrl, encoding, method, isIE, referer, cookieCollection, tryNum);
			}
			return result;
		}

		public string Invoke(string strUrl, Encoding encoding, string method, bool isIE, string referer, CookieCollection cookieCollection)
		{
			return Invoke(strUrl, encoding, method, isIE, referer, cookieCollection, 0);
		}

		public string Invoke(string strUrl, Encoding encoding, string method, bool isIE, string referer)
		{
			return Invoke(strUrl, encoding, method, isIE, referer, null);
		}

		public string Invoke(string strUrl, Encoding encoding, string method, bool isIE)
		{
			return Invoke(strUrl, encoding, method, isIE, string.Empty);
		}

		public string Invoke(string strUrl, Encoding encoding, string method)
		{
			return Invoke(strUrl, encoding, method, true);
		}

		public string Invoke(string strUrl, Encoding encoding)
		{
			return Invoke(strUrl, encoding, "POST");
		}

		public string Invoke(string strUrl)
		{
			return Invoke(strUrl, Encoding.GetEncoding("UTF-8"));
		}

		private string GetHostName(string strUrl)
		{
			try
			{
				Uri uri = new Uri(strUrl);
				return uri.Host.ToLower();
			}
			catch (Exception ex)
			{
				//Log.WriteLog("获取域名出现异常", ex);
			}
			return string.Empty;
		}

		private void RetriveParamInURL(string strUrl)
		{
			string empty = string.Empty;
			if (strUrl.IndexOf('?') > 0)
			{
				empty = strUrl.Substring(strUrl.IndexOf('?'));
				empty = empty.Substring(1);
				char[] separator = new char[1]
				{
					'&'
				};
				string[] array = empty.Split(separator, 2147483647);
				string[] array2 = array;
				foreach (string text in array2)
				{
					string empty2 = string.Empty;
					string empty3 = string.Empty;
					if (text.IndexOf('=') >= 0)
					{
						empty2 = text.Substring(0, text.IndexOf('='));
						empty3 = text.Substring(text.IndexOf('='));
						if (0 < empty3.Length)
						{
							empty3 = empty3.Substring(1);
						}
						AddStringParm(empty2, empty3);
					}
				}
			}
		}

		public void AddStringParm(string key, string value)
		{
			string text = string.IsNullOrEmpty(key) ? string.Empty : key.Trim();
			string value2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
			if (!string.IsNullOrEmpty(text))
			{
				if (dictParm.ContainsKey(text) && dictParm[text] is StringField)
				{
					if (string.IsNullOrEmpty(value2))
					{
						dictParm.Remove(text);
					}
					else
					{
						(dictParm[key] as StringField).Value = value2;
					}
				}
				else if (!string.IsNullOrEmpty(value2))
				{
					dictParm.Add(key, new StringField(text, value2));
				}
			}
		}

		private byte[] GetMultipartData(Dictionary<string, BaseField> dictField, string boundary)
		{
			string text = "\r\n";
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			string str = "utf-8";
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, BaseField> item in dictField)
			{
				if (item.Value is StringField)
				{
					StringField stringField = item.Value as StringField;
					if (string.IsNullOrEmpty(stringField.Key))
					{
						break;
					}
					if (string.IsNullOrEmpty(stringField.Value))
					{
						break;
					}
					stringBuilder.Append("--" + boundary);
					stringBuilder.Append(text);
					stringBuilder.Append("Content-Disposition: form-data; name=\"" + stringField.Key + "\"");
					stringBuilder.Append(text);
					stringBuilder.Append("Content-Type: text/plain; charset=" + str);
					stringBuilder.Append(text + text);
					stringBuilder.Append(stringField.Value);
					stringBuilder.Append(text);
				}
			}
			stringBuilder.Append("--" + boundary + "--" + text);
			return uTF8Encoding.GetBytes(stringBuilder.ToString());
		}
	}
}
