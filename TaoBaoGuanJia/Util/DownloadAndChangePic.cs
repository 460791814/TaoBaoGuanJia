
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Util
{
	internal static class DownloadAndChangePic
	{
		private static ImageOperator _imageOperator;

		static DownloadAndChangePic()
		{
			_imageOperator = new ImageOperator();
		}

		private static string GetNewFileName()
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
			return text + Guid.NewGuid().ToString() + ".jpg";
		}

		public static byte[] DownloadPicToBytes(string url)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			if (url.IndexOf("file:///", StringComparison.OrdinalIgnoreCase) >= 0 || url.IndexOf("file:\\\\", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				try
				{
					Uri uri = new Uri(url);
					text = uri.LocalPath;
					text2 = uri.Fragment;
					flag = true;
				}
				catch (Exception ex)
				{
					Log.WriteLog(ex);
				}
			}
			else
			{
				text = _imageOperator.DownLoadPicture(url, null, 3000, 5000);
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (!File.Exists(text))
			{
				if (!flag)
				{
					return null;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text += text2;
					if (!File.Exists(text))
					{
						return null;
					}
				}
			}
			byte[] imageToByteArray = GetImageToByteArray(text);
			try
			{
				if (!flag)
				{
					File.Delete(text);
					return imageToByteArray;
				}
				return imageToByteArray;
			}
			catch
			{
				return imageToByteArray;
			}
		}

		private static byte[] GetImageToByteArray(string fileName)
		{
			return LocalFileOperater.GetImageToByteArray(fileName);
		}

		private static string ConvertToJpg(string fileInPath)
		{
			string newFileName = GetNewFileName();
			Bitmap bitmap = new Bitmap(fileInPath);
			bitmap.Save(newFileName, ImageFormat.Jpeg);
			bitmap.Dispose();
			return newFileName;
		}

		public static byte[] DownloadOrLocalPicToBytes(string url)
		{
			string text = url;
			bool flag = false;
			if (url.IndexOf("http", StringComparison.OrdinalIgnoreCase) == 0)
			{
				text = _imageOperator.DownLoadPicture(url, null, 3000, 5000);
			}
			else
			{
				flag = true;
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (!File.Exists(text))
			{
				return null;
			}
			byte[] imageToByteArray = GetImageToByteArray(text);
			try
			{
				if (!flag)
				{
					File.Delete(text);
					return imageToByteArray;
				}
				return imageToByteArray;
			}
			catch
			{
				return imageToByteArray;
			}
		}
	}
}
