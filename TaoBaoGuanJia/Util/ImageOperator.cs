 
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class ImageOperator
	{
		private static int _maxRandValue = 0;

		private static object _lockObj = new object();

		private static string GetNexRandValue()
		{
			lock (_lockObj)
			{
				_maxRandValue++;
				return _maxRandValue.ToString("0000") + Guid.NewGuid().ToString().Substring(0, 6);
			}
		}

		public string DownLoadPicture(string url, string fileFullName, int timeOut, int readWriteTimeout)
		{
			return DownLoadPicture(url, fileFullName, timeOut, readWriteTimeout, 0, 1);
		}

		private string RemoveEndStrForUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			string value = "_.webp";
			if (url.EndsWith(value))
			{
				url = url.Substring(0, url.LastIndexOf(value));
			}
			return url;
		}

		private HttpWebRequest GetWebRequest(string url)
		{
			Uri uri = null;
			if (string.IsNullOrEmpty(url))
			{
				return null;
			}
			if (url.IndexOf("//") == 0)
			{
				url = "http:" + url;
			}
			uri = (url.EndsWith(".") ? UrlUtil.MakeSpecialUri(url) : new Uri(url));
			return (HttpWebRequest)WebRequest.Create(uri);
		}

		private string DownLoadPicture(string url, string fileFullName, int timeOut, int readWriteTimeout, int temp, int nums)
		{
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			WebResponse webResponse = null;
			Stream stream = null;
			Stream stream2 = null;
			HttpWebRequest httpWebRequest = null;
			string text = string.Empty;
			try
			{
				text = GetRredirectUrl(url);
				if (string.IsNullOrEmpty(text))
				{
					text = url;
				}
				text = RemoveEndStrForUrl(text);
				try
				{
					if (text.IndexOf("paipai", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						timeOut = 40000 + temp * 10000;
						readWriteTimeout = 40000 + temp * 10000;
					}
					else
					{
						timeOut = 20000;
						readWriteTimeout = 20000;
					}
					httpWebRequest = GetWebRequest(text);
					httpWebRequest.Timeout = timeOut;
					httpWebRequest.ReadWriteTimeout = readWriteTimeout;
					httpWebRequest.Method = "GET";
					httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; Chrome)";
					if (url.Contains(".baobeitu.com") || url.Contains(".wal8.com"))
					{
						httpWebRequest.Referer = url;
					}
					if (url.Contains("qpic.cn"))
					{
						httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
						httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
						((NameValueCollection)httpWebRequest.Headers)["Accept-Encoding"] = "gzip,deflate,sdch";
						((NameValueCollection)httpWebRequest.Headers)["Accept-Language"] = "zh-CN,zh;q=0.8";
						((NameValueCollection)httpWebRequest.Headers)["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
					}
					if (url.Contains("yzcdn.cn") || url.Contains("geilicdn.com") || url.Contains("alicdn.com"))
					{
						ServicePointManager.ServerCertificateValidationCallback = ((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
					}
					webResponse = httpWebRequest.GetResponse();
				}
				catch (Exception ex)
				{
					if (temp < nums && !string.IsNullOrEmpty(text) && (text.IndexOf("taobaocdn", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("paipai", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("alicdn", StringComparison.OrdinalIgnoreCase) >= 0) && ex != null && !string.IsNullOrEmpty(ex.Message) && (ex.Message.IndexOf("操作超时") >= 0 || ex.Message.IndexOf("The operation has timed out", StringComparison.OrdinalIgnoreCase) >= 0))
					{
						if (webResponse != null)
						{
							webResponse.Close();
							webResponse = null;
						}
						httpWebRequest?.Abort();
						temp++;
						Thread.Sleep(1000);
						return DownLoadPicture(url, fileFullName, timeOut, readWriteTimeout, temp, nums);
					}
					Log.WriteLog("下载图片失败1，地址:" + url + ex.Message);
				}
				if (webResponse != null && (url.Contains("ef168.com") || !webResponse.ContentType.ToLower().StartsWith("text/")))
				{
					if (string.IsNullOrEmpty(fileFullName))
					{
						fileFullName = GetNewFileName(url);
					}
					if (File.Exists(fileFullName))
					{
						fileFullName = GetNewFileName(url);
					}
					string extension = Path.GetExtension(fileFullName);
                    bool flag = false;// DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", "PicSet", "KeepHighQuality", "false"));
                    bool flag2 = false;// DataConvert.ToBoolean((object)ToolServer.get_ConfigData().GetUserConfig("AppConfig", "DefaultConfig", "MobileDescDowning", "false"));
					if (extension.ToLower() == ".gif" || !flag || flag2)
					{
						stream2 = File.Create(fileFullName);
						byte[] array = new byte[1024];
						stream = webResponse.GetResponseStream();
						int num = -1;
						int num2 = 1;
						bool flag3 = true;
						do
						{
							num = stream.Read(array, 0, array.Length);
							if (num > 0)
							{
								stream2.Write(array, 0, num);
							}
							if (num == 0 || num == 1024)
							{
								num2 = 1;
							}
							else if (num < 1024)
							{
								num2++;
							}
							if (num2 >= 10)
							{
								flag3 = false;
								break;
							}
						}
						while (num > 0);
						if (!flag3)
						{
							throw new Exception("下载图片太慢");
						}
					}
					else
					{
						Image image = new Bitmap(webResponse.GetResponseStream());
						if (!SaveBmpWithImageCodecInfo(image, fileFullName))
						{
							fileFullName = string.Empty;
						}
						image.Dispose();
					}
				}
			}
			catch (Exception ex2)
			{
				if (ex2.Message.IndexOf("System.Net.FileWebRequest", StringComparison.OrdinalIgnoreCase) >= 0 && ex2.Message.IndexOf("System.Net.HttpWebRequest", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					fileFullName = DownLoadLocalPicture(url, fileFullName, temp, nums);
				}
				else if (ex2.Message.IndexOf("进程无法访问") >= 0)
				{
					Thread.Sleep(2000);
					if (temp < nums)
					{
						temp++;
						return DownLoadPicture(url, fileFullName, timeOut, readWriteTimeout, temp, nums);
					}
					Log.WriteLog("下载图片失败2，地址:" + url + ex2.Message);
					fileFullName = "";
				}
				else if (ex2.Message.IndexOf("下载图片太慢") >= 0)
				{
					if (temp < nums)
					{
						temp++;
						return DownLoadPicture(url, fileFullName, timeOut, readWriteTimeout, temp, nums);
					}
					Log.WriteLog("图片重试下载" + nums + "次后，仍然太慢，放弃下载，图片地址:" + url);
					fileFullName = "";
				}
				else
				{
					Log.WriteLog("下载图片失败2，地址:" + url + ex2.Message);
					fileFullName = "";
				}
			}
			finally
			{
				try
				{
					if (stream != null)
					{
						stream.Close();
						stream.Dispose();
					}
					if (stream2 != null)
					{
						stream2.Close();
						stream2.Dispose();
					}
					if (webResponse != null)
					{
						webResponse.Close();
						webResponse = null;
					}
					httpWebRequest?.Abort();
				}
				catch (Exception ex3)
				{
					Log.WriteLog("清除下载图片时的资源时出现异常", ex3);
				}
			}
			bool flag4 = false;
			if (!string.IsNullOrEmpty(fileFullName) && File.Exists(fileFullName))
			{
				flag4 = true;
			}
			if (!string.IsNullOrEmpty(text) && text.IndexOf("qpic.cn", StringComparison.OrdinalIgnoreCase) >= 0 && flag4)
			{
				byte[] imageToByteArray = GetImageToByteArray(fileFullName);
				if (imageToByteArray != null && (imageToByteArray.Length == 3691 || imageToByteArray.Length == 2121) && temp <= nums)
				{
					Thread.Sleep(5000);
					try
					{
						File.Delete(fileFullName);
					}
					catch
					{
					}
					temp++;
					return DownLoadPicture(text, fileFullName, timeOut, readWriteTimeout, temp, nums);
				}
				if (imageToByteArray != null && (imageToByteArray.Length == 3691 || imageToByteArray.Length == 2121) && temp > nums)
				{
					Log.WriteLog("下载图片失败3，地址:" + text);
					fileFullName = "";
					flag4 = false;
				}
			}
			if (!flag4)
			{
				fileFullName = "";
				temp++;
				if (!url.Contains("https://") && url.Contains("http://"))
				{
					url = url.Replace("http://", "https://");
					return DownLoadPicture(url, fileFullName, timeOut, readWriteTimeout, temp, nums);
				}
			}
			return fileFullName;
		}

		private byte[] GetImageToByteArray(string fileName)
		{
			return LocalFileOperater.GetImageToByteArray(fileName);
		}

		public bool SaveBmpWithImageCodecInfo(Image img, string photoPath)
		{
			return LocalFileOperater.SaveBmpWithImageCodecInfo(img, photoPath);
		}

		private string GetRredirectUrl(string url)
		{
			if (!url.Contains("paipaiimg") && !url.Contains("qpic.cn"))
			{
				HttpWebRequest httpWebRequest = null;
				try
				{
					httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
					httpWebRequest.Referer = HttpUtility.UrlEncode(url);
					httpWebRequest.AllowAutoRedirect = false;
					using (WebResponse webResponse = httpWebRequest.GetResponse())
					{
						return ((NameValueCollection)webResponse.Headers)["Location"];
					}
				}
				catch (Exception)
				{
					return url;
				}
				finally
				{
					httpWebRequest?.Abort();
				}
			}
			return url;
		}

		private string DownLoadLocalPicture(string url, string fileFullName, int temp, int nums)
		{
			WebResponse webResponse = null;
			try
			{
				FileWebRequest fileWebRequest = (FileWebRequest)WebRequest.Create(url);
				try
				{
					temp++;
					fileWebRequest.Timeout = 1000;
					webResponse = fileWebRequest.GetResponse();
				}
				catch
				{
					if (temp > nums)
					{
						goto end_IL_0027;
					}
					Thread.Sleep(1000);
					return DownLoadLocalPicture(url, fileFullName, temp, nums);
					end_IL_0027:;
				}
				if (string.IsNullOrEmpty(fileFullName))
				{
					fileFullName = GetNewFileName(url);
				}
				if (File.Exists(fileFullName))
				{
					fileFullName = GetNewFileName(url);
				}
				Image image = new Bitmap(webResponse.GetResponseStream());
				if (!SaveBmpWithImageCodecInfo(image, fileFullName))
				{
					fileFullName = string.Empty;
				}
				image.Dispose();
				return fileFullName;
			}
			catch (Exception ex)
			{
				Log.WriteLog("下载本地图片失败3，地址:" + url + ex.Message);
				fileFullName = "";
				return fileFullName;
			}
			finally
			{
				if (webResponse != null)
				{
					webResponse.Close();
					webResponse = null;
				}
			}
		}

		public string TransPicture(string fileFullPath, string toolCode, string onlineKey, TransPictureType transPictureType, bool replaceOld)
		{
			if (string.IsNullOrEmpty(fileFullPath))
			{
				return null;
			}
			if (!File.Exists(fileFullPath))
			{
				return null;
			}
			string newFilePath = GetNewFilePath(fileFullPath, toolCode, onlineKey, replaceOld);
			if (newFilePath.ToLower() != fileFullPath.ToLower())
			{
				try
				{
					if (File.Exists(newFilePath))
					{
						File.Delete(newFilePath);
					}
					switch (transPictureType)
					{
					case TransPictureType.CopyPicture:
						File.Copy(fileFullPath, newFilePath, true);
						break;
					case TransPictureType.MovePicture:
						File.Move(fileFullPath, newFilePath);
						break;
					}
				}
				catch (Exception ex)
				{
					Log.WriteLog(ex);
				}
			}
			return newFilePath.Substring(ConfigHelper.GetCurrentDomainDirectory().Length);
		}

		public string TransPicture(string fileFullPath, string toolCode, string onlineKey, TransPictureType transPictureType)
		{
			return TransPicture(fileFullPath, toolCode, onlineKey, transPictureType, false);
		}

		public string GetNewFilePath(string fileFullPath, string toolCode, string onlineKey, bool replaceOld)
		{
			if (string.IsNullOrEmpty(fileFullPath))
			{
				return string.Empty;
			}
			FileInfo fileInfo = new FileInfo(fileFullPath);
			string text = fileInfo.Name.ToLower().Trim();
			if (!text.EndsWith(".jpg") && !text.EndsWith(".jpeg") && !text.EndsWith(".gif") && !text.EndsWith(".png") && !text.EndsWith(".bmp"))
			{
				text += ".jpg";
			}
			string text2 = ConfigHelper.GetCurrentDomainDirectory() + "pic\\" + toolCode + "\\";
			if (!string.IsNullOrEmpty(onlineKey))
			{
				text2 = text2 + onlineKey + "\\";
			}
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string text3 = text2 + text;
			if (text3.ToLower() == fileFullPath.ToLower())
			{
				return text3;
			}
			if (!replaceOld)
			{
				int startIndex = text3.LastIndexOf('.');
				int num = 1;
				while (File.Exists(text3))
				{
					text3 = (text2 + text).Insert(startIndex, num.ToString());
					num++;
				}
			}
			return text3;
		}

		public string TransPicture(string fileFullPath, string toolCode, TransPictureType transPictureType)
		{
			if (string.IsNullOrEmpty(fileFullPath))
			{
				return null;
			}
			if (!File.Exists(fileFullPath))
			{
				return null;
			}
			string newFilePath = GetNewFilePath(fileFullPath, toolCode);
			if (newFilePath.ToLower() != fileFullPath.ToLower())
			{
				if (File.Exists(newFilePath))
				{
					File.Delete(newFilePath);
				}
				switch (transPictureType)
				{
				case TransPictureType.CopyPicture:
					File.Copy(fileFullPath, newFilePath, true);
					break;
				case TransPictureType.MovePicture:
					File.Move(fileFullPath, newFilePath);
					break;
				}
			}
			return newFilePath.Substring(ConfigHelper.GetCurrentDomainDirectory().Length);
		}

		public string GetNewFilePath(string fileFullPath, string toolCode)
		{
			if (string.IsNullOrEmpty(fileFullPath))
			{
				return string.Empty;
			}
			FileInfo fileInfo = new FileInfo(fileFullPath);
			string text = fileInfo.Name.ToLower().Trim();
			if (!text.EndsWith(".jpg") && !text.EndsWith(".jpeg") && !text.EndsWith(".gif") && !text.EndsWith(".png") && !text.EndsWith(".bmp"))
			{
				text += ".jpg";
			}
			string text2 = ConfigHelper.GetCurrentDomainDirectory() + "pic\\" + toolCode + "\\";
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string text3 = text2 + text;
			if (text3.ToLower() == fileFullPath.ToLower())
			{
				return text3;
			}
			int num = num = text3.LastIndexOf('.');
			int num2 = 1;
			while (File.Exists(text3))
			{
				text3 = (text2 + text).Insert(num, num2.ToString());
				num2++;
			}
			return text3;
		}

		private string GetNewFileName(string src)
		{
			string text = ConfigHelper.GetCurrentDomainDirectory() + "temp\\";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (!text.EndsWith("\\"))
			{
				text += "\\";
			}
			string text2 = Path.GetFileNameWithoutExtension(src);
			if (text2.Length > 30)
			{
				string nexRandValue = GetNexRandValue();
				text2 = nexRandValue + text2.Substring(0, 6) + text2.Substring(text2.Length - 14);
				text2 = text2.Replace("!", "_");
			}
			string text3 = Path.GetExtension(src);
			if (string.IsNullOrEmpty(text3) || (text3.ToLower() != ".jpeg" && text3.ToLower() != ".bmp" && text3.ToLower() != ".png" && text3.ToLower() != ".gif" && text3.ToLower() != ".jpg"))
			{
				text3 = ".jpg";
			}
			string empty = string.Empty;
			empty = ((text2.IndexOfAny(Path.GetInvalidFileNameChars()) < 0) ? (text + text2 + text3) : GetNewFileName(text, text3));
			while (File.Exists(empty))
			{
				empty = GetNewFileName(text, text3);
			}
			return empty;
		}

		private string GetNewFileName(string path, string fileExtension)
		{
			string str = Guid.NewGuid().ToString();
			return path + str + fileExtension;
		}
	}
}
