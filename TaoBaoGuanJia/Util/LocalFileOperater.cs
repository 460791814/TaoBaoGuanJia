
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Web;

namespace TaoBaoGuanJia.Util
{
	internal static class LocalFileOperater
	{
		private static object _lockObj = new object();

		public static List<string> CompressJpeg(string sourcePicFileName, string newPicFileName, int maxWidth, int maxHeight, long maxLength, out int byteSize, int totalByteSize)
		{
			lock (_lockObj)
			{
				List<string> list = new List<string>();
				string text = Path.GetExtension(newPicFileName);
				if (string.IsNullOrEmpty(text) || (text.ToLower() != ".jpeg" && text.ToLower() != ".bmp" && text.ToLower() != ".png" && text.ToLower() != ".gif" && text.ToLower() != ".jpg"))
				{
					text = ".jpg";
				}
				ImageCodecInfo encoder = (!(text.ToLower() == ".gif")) ? GetEncoderInfo("image/jpeg") : GetEncoderInfo("image/gif");
				if (text.ToLower() == ".png")
				{
					newPicFileName = newPicFileName.Replace(".png", ".jpg");
				}
				Encoder quality = Encoder.Quality;
				EncoderParameters encoderParameters = new EncoderParameters(1);
				double num = 100.0;
				byte[] array = null;
				byteSize = 0;
				try
				{
					if (!File.Exists(sourcePicFileName))
					{
						string text2 = HttpUtility.UrlDecode(sourcePicFileName);
						if (!File.Exists(text2))
						{
							return null;
						}
						sourcePicFileName = text2;
					}
					Image image = Image.FromFile(sourcePicFileName);
					int num2 = image.Height * maxWidth / image.Width;
					int num3 = (int)Math.Ceiling((double)num2 / (double)maxHeight);
					for (int i = 0; i < num3; i++)
					{
						int num4 = (num3 != 1) ? (num2 / num3) : maxHeight;
						if (i == num3 - 1)
						{
							num4 = num2 - num4 * i;
						}
						Bitmap bitmap = new Bitmap(maxWidth, num4);
						Graphics graphics = Graphics.FromImage(bitmap);
						if (num3 == 1)
						{
							graphics.DrawImage(image, 0, maxHeight * i, maxWidth, num4);
						}
						else
						{
							float num5 = (float)image.Height / (float)num3;
							float num6 = (i != num3 - 1) ? num5 : ((float)image.Height - num5 * (float)i);
							Rectangle destRect = new Rectangle(0, 0, maxWidth, num4);
							graphics.DrawImage(image, destRect, 0, (int)(num5 * (float)i), image.Width, (int)num6, GraphicsUnit.Pixel);
						}
						bool flag = true;
						do
						{
							EncoderParameter encoderParameter = new EncoderParameter(quality, (long)num);
							encoderParameters.Param[0] = encoderParameter;
							using (MemoryStream memoryStream = new MemoryStream())
							{
								try
								{
									bitmap.Save(memoryStream, encoder, encoderParameters);
									array = memoryStream.ToArray();
								}
								catch (Exception ex)
								{
									Log.WriteLog(ex);
								}
								memoryStream.Close();
							}
							if (num < 8.0)
							{
								flag = false;
								break;
							}
							num *= 0.9;
						}
						while (array.Length > maxLength);
						graphics.Dispose();
						if (flag && array.Length > DataConvert.ToInt((object)1843.2))
						{
							string text3 = newPicFileName.Insert(newPicFileName.LastIndexOf('.'), i.ToString());
							bitmap.Save(text3, encoder, encoderParameters);
							list.Add(text3);
							byteSize += array.Length;
							if (totalByteSize + byteSize > 2621440)
							{
								break;
							}
						}
						bitmap.Dispose();
					}
					image.Dispose();
				}
				catch (Exception ex2)
				{
					Log.WriteLog(ex2);
				}
				return list;
			}
		}

		private static bool SaveBmpWithImageCodecInfo(Image img, string photoPath, int temp)
		{
			lock (_lockObj)
			{
				try
				{
					string text = Path.GetExtension(photoPath);
					if (string.IsNullOrEmpty(text) || (text.ToLower() != ".jpeg" && text.ToLower() != ".bmp" && text.ToLower() != ".png" && text.ToLower() != ".gif" && text.ToLower() != ".jpg"))
					{
						text = ".jpg";
					}
					ImageCodecInfo encoder = (!(text.ToLower() == ".gif")) ? ((!(text.ToLower() == ".png")) ? GetEncoderInfo("image/jpeg") : GetEncoderInfo("image/png")) : GetEncoderInfo("image/gif");
					Encoder quality = Encoder.Quality;
					EncoderParameters encoderParameters = new EncoderParameters(1);
					EncoderParameter encoderParameter = new EncoderParameter(quality, 100L);
					encoderParameters.Param[0] = encoderParameter;
					if (img == null)
					{
						return false;
					}
					try
					{
						using (Bitmap bitmap = new Bitmap(img))
						{
							bitmap.Save(photoPath, encoder, encoderParameters);
						}
						return true;
					}
					catch (Exception ex)
					{
						Log.WriteLog("ImageOperator类中SaveBmpWithImageCodecInfo出错", ex);
						return false;
					}
				}
				catch (Exception ex2)
				{
					if (ex2.Message.IndexOf("进程无法访问") >= 0 && temp < 1)
					{
						Thread.Sleep(2000);
						temp++;
						return SaveBmpWithImageCodecInfo(img, photoPath, temp);
					}
					Log.WriteLog(ex2);
					return false;
				}
			}
		}

		public static bool SaveBmpWithImageCodecInfo(Image img, string photoPath)
		{
			return SaveBmpWithImageCodecInfo(img, photoPath, 0);
		}

		private static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			for (int i = 0; i < imageEncoders.Length; i++)
			{
				if (imageEncoders[i].MimeType == mimeType)
				{
					return imageEncoders[i];
				}
			}
			return null;
		}

		private static byte[] GetImageToByteArray(string fileName, int temp)
		{
			lock (_lockObj)
			{
				FileStream fileStream = null;
				byte[] array = null;
				try
				{
					SetFileAttribute(fileName);
					fileStream = new FileStream(fileName, FileMode.Open);
					int num = (int)fileStream.Length;
					array = new byte[num];
					fileStream.Read(array, 0, num);
				}
				catch (Exception ex)
				{
					if (ex.Message.IndexOf("进程无法访问") >= 0 && temp < 1)
					{
						try
						{
							if (fileStream != null)
							{
								fileStream.Close();
								fileStream.Dispose();
								fileStream = null;
							}
						}
						catch
						{
						}
						Thread.Sleep(2000);
						temp++;
						return GetImageToByteArray(fileName, temp);
					}
					Log.WriteLog(ex);
					return null;
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
						fileStream.Dispose();
					}
				}
				return array;
			}
		}

		public static byte[] GetImageToByteArray(string fileName)
		{
			return GetImageToByteArray(fileName, 0);
		}

		private static void SetFileAttribute(string fullFileName)
		{
			if (File.Exists(fullFileName))
			{
				FileInfo fileInfo = new FileInfo(fullFileName);
				if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") > -1)
				{
					fileInfo.Attributes = FileAttributes.Normal;
				}
			}
		}
	}
}
