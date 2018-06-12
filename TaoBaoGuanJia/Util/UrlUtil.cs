using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Web;

namespace TaoBaoGuanJia.Util
{
	public static class UrlUtil
	{
		public const string PARMNAME = "thenowtime";

		public const string ABOUTBLANK = "about:blank";

		public static string AddParmToUrl(string url)
		{
			return AddParmToUrl(url, "thenowtime", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
		}

		public static string AddParmToUrl(string url, string parmName, string parmValue)
		{
			if (string.IsNullOrEmpty(parmName))
			{
				return url;
			}
			if (string.IsNullOrEmpty(url))
			{
				return url;
			}
			if (url.ToLower() == "about:blank")
			{
				return url;
			}
			url = url.Trim();
			Uri uriFromUrlString = GetUriFromUrlString(url);
			string text = string.Empty;
			if (uriFromUrlString.OriginalString != "about:blank")
			{
				url = uriFromUrlString.OriginalString;
				text = uriFromUrlString.Query;
				if (!string.IsNullOrEmpty(text))
				{
					int num = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
					if (num < 0)
					{
						text = HttpUtility.UrlDecode(text);
						num = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
						if (num < 0)
						{
							text = HttpUtility.UrlEncode(text);
							num = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
						}
					}
				}
			}
			else
			{
				int num2 = url.IndexOf("?");
				if (num2 >= 0)
				{
					text = url.Substring(num2);
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.TrimStart('?');
				string[] array = text.Split(new char[1]
				{
					'&'
				}, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (text2.IndexOf('=') < 0)
					{
						if (string.Compare(text2, parmName, true) == 0)
						{
							url = RemoveParmFromUrl(url, parmName);
						}
					}
					else
					{
						string[] array3 = text2.Split(new char[1]
						{
							'='
						}, StringSplitOptions.RemoveEmptyEntries);
						if (array3.Length > 0 && string.Compare(array3[0], parmName, true) == 0)
						{
							url = RemoveParmFromUrl(url, parmName);
						}
					}
				}
			}
			if (!url.EndsWith("?") && !url.EndsWith("&"))
			{
				url = url.TrimEnd('/');
				url = ((url.IndexOf('?') < 0) ? (url + "?") : (url + "&"));
			}
			url = url + parmName + "=" + parmValue;
			return url;
		}

		private static Uri GetUriFromUrlString(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return new Uri("about:blank");
			}
			try
			{
				if (!url.EndsWith("."))
				{
					return new Uri(url);
				}
				return MakeSpecialUri(url);
			}
			catch
			{
				return new Uri("about:blank");
			}
		}

		public static Uri MakeSpecialUri(string url)
		{
			try
			{
				Uri uri = new Uri(url);
				string value = uri.AbsoluteUri + ".";
				object value2 = typeof(Uri).GetField("m_Info", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uri);
				object value3 = typeof(Uri).Assembly.GetType("System.Uri+UriInfo").GetField("MoreInfo", BindingFlags.Instance | BindingFlags.Public).GetValue(value2);
				typeof(Uri).Assembly.GetType("System.Uri+MoreInfo").GetField("AbsoluteUri", BindingFlags.Instance | BindingFlags.Public).SetValue(value3, value);
				ulong num = (ulong)typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uri);
				typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uri, (ulong)((long)num & -8193L));
				return uri;
			}
			catch (Exception ex)
			{
				//Log.WriteLog("处理链接" + url + "出现异常", ex);
				return new Uri("about:blank");
			}
		}

		public static string GetParmValueFromUrl(string url, string parmName)
		{
			if (string.IsNullOrEmpty(parmName))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			if (!(url.ToLower() == "about:blank") && url.IndexOf(parmName, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				url = url.Trim();
				Uri uriFromUrlString = GetUriFromUrlString(url);
				string text = string.Empty;
				if (uriFromUrlString.OriginalString == "about:blank")
				{
					int num = url.IndexOf("?");
					if (num >= 0)
					{
						text = url.Substring(num);
					}
				}
				else
				{
					text = uriFromUrlString.Query;
					if (!string.IsNullOrEmpty(text))
					{
						int num2 = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
						if (num2 < 0)
						{
							text = HttpUtility.UrlDecode(text);
							num2 = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
							if (num2 < 0)
							{
								text = HttpUtility.UrlEncode(text);
								num2 = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
								if (num2 < 0)
								{
									text = string.Empty;
								}
							}
						}
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					return string.Empty;
				}
				text = text.TrimStart('?');
				if (string.IsNullOrEmpty(text))
				{
					return string.Empty;
				}
				string[] array = text.Split(new char[1]
				{
					'&'
				}, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					int num3 = text2.IndexOf('=');
					if (num3 > 0)
					{
						string strA = text2.Substring(0, num3);
						if (string.Compare(strA, parmName, true) == 0 && num3 != text2.Length - 1)
						{
							return text2.Substring(num3 + 1);
						}
					}
				}
				return string.Empty;
			}
			return string.Empty;
		}

		public static string RemoveParmFromUrl(string url, string parmName)
		{
			if (string.IsNullOrEmpty(parmName))
			{
				return url;
			}
			if (string.IsNullOrEmpty(url))
			{
				return url;
			}
			if (!(url.ToLower() == "about:blank") && url.IndexOf(parmName, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				url = url.Trim();
				Uri uriFromUrlString = GetUriFromUrlString(url);
				string text = string.Empty;
				string text2 = string.Empty;
				if (uriFromUrlString.OriginalString == "about:blank")
				{
					int num = url.IndexOf("?");
					if (num >= 0)
					{
						text = url.Substring(num);
						if (num > 0)
						{
							text2 = url.Substring(0, num);
						}
					}
				}
				else
				{
					text = uriFromUrlString.Query;
					if (!string.IsNullOrEmpty(text))
					{
						int num2 = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
						if (num2 < 0)
						{
							text = HttpUtility.UrlDecode(text);
							num2 = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
							if (num2 < 0)
							{
								text = HttpUtility.UrlEncode(text);
								num2 = uriFromUrlString.OriginalString.IndexOf(text, StringComparison.OrdinalIgnoreCase);
							}
						}
						if (num2 >= 0)
						{
							text2 = uriFromUrlString.OriginalString.Substring(0, num2);
						}
						else
						{
							text = string.Empty;
						}
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					return url;
				}
				text = text.TrimStart('?');
				if (string.IsNullOrEmpty(text))
				{
					return url;
				}
				string[] array = text.Split(new char[1]
				{
					'&'
				}, StringSplitOptions.RemoveEmptyEntries);
				string text3 = null;
				string[] array2 = array;
				foreach (string text4 in array2)
				{
					int num3 = text4.IndexOf('=');
					if (num3 <= 0)
					{
						if (string.Compare(text4, parmName, true) != 0)
						{
							text3 = ((!string.IsNullOrEmpty(text3)) ? (text3 + "&" + text4) : text4);
						}
					}
					else
					{
						string strA = text4.Substring(0, num3);
						if (string.Compare(strA, parmName, true) != 0)
						{
							text3 = ((!string.IsNullOrEmpty(text3)) ? (text3 + "&" + text4) : text4);
						}
					}
				}
				if (!string.IsNullOrEmpty(text3) && !string.IsNullOrEmpty(text2) && !text2.EndsWith("?") && !text2.EndsWith("&"))
				{
					text2 += "?";
				}
				url = text2 + text3;
				while (true)
				{
					if (string.IsNullOrEmpty(url))
					{
						break;
					}
					if (!url.EndsWith("?") && !url.EndsWith("&"))
					{
						break;
					}
					url = url.Substring(0, url.Length - 1);
				}
				return url;
			}
			return url;
		}

		public static string RemoveParmFromUrl(string url)
		{
			return RemoveParmFromUrl(url, "thenowtime");
		}

		public static WebRequest GetWebRequest(string url)
		{
			Uri uri = null;
			uri = (url.EndsWith(".") ? MakeSpecialUri(url) : new Uri(url));
			return WebRequest.Create(uri);
		}

		public static HttpWebRequest GetHttpWebRequest(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)GetWebRequest(url);
			httpWebRequest.KeepAlive = false;
			httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
			httpWebRequest.Timeout = 36000;
			return httpWebRequest;
		}

		public static bool IsValidUrl(string url)
		{
			try
			{
				if (string.IsNullOrEmpty(url))
				{
					return false;
				}
				new Uri(url);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static void OpenUrlUseDefaultBrowser(string url)
		{
			if (IsValidUrl(url))
			{
				try
				{
					string name = "http\\shell\\open\\command";
					RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(name, false);
					if (registryKey != null)
					{
						object value = registryKey.GetValue(null, null);
						if (value != null)
						{
							string[] array = value.ToString().Split('"');
							string empty = string.Empty;
							if (array.Length >= 2)
							{
								empty = array[1];
								Process.Start(empty, url);
								goto end_IL_0009;
							}
						}
					}
					Process.Start(new ProcessStartInfo("IEXPLORE.EXE", url));
					end_IL_0009:;
				}
				catch (Win32Exception)
				{
					try
					{
						Process.Start(url);
					}
					catch (Exception ex)
					{
						//Log.WriteLog(ex);
					}
				}
			}
		}
	}
}
