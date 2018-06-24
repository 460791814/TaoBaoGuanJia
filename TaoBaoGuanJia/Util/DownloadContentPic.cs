
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TaoBaoGuanJia.Extension;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Model;


namespace TaoBaoGuanJia.Util
{
	public class DownloadContentPic
	{
		public static readonly string _baohuSrcUrl = "http://t.cn/zQJCtiW";

		public static string _baohuLocalPath = string.Empty;

		public static readonly string _patternLocalFile = "(?<url>file:///[^\\u0022]+\\.(?:jpg|gif|bmp|png|jpeg))[\\s\\u0022]?";

		private static bool _initial = false;

		private static object _lockOjb = new object();

		private static bool _samePicDownloadOneTime = false;

		private static bool _firtItemPicInDownload = false;

		public static readonly string _imgPatern = "<\\s*img\\s+[^>]*(src)+[^>]*|http[s]?://[^<>\\s&\\u0022]+\\.(?:jpg|gif|bmp|png|jpeg)[^<>\"\\s\\(\\)]*|(?<=background=[\"]?)(http[s]?://)+[^>\\s\"]*|(?<=background=[\"]?)file://[^>\"]*\\.(?:jpg|gif|bmp|png|jpeg)|(?<=background:url[(]?)file://[^>\"]*\\.(?:jpg|gif|bmp|png|jpeg)";

		public static readonly string _patternUrl = "alt=(?([\"\\'])[\"\\']([^\"\\']+?)[\"\\']|\\s*[^\\s?]*)|src=(?([\"\\'])[\"\\']([^\"\\']+?)[\"\\']|\\s*[^\\s>]*)|url\\((?([\"\\'])[\"\\']([^\"\\']+?)[\"\\']|\\s*[^\\s?]*)\\)";

		public static readonly string _protectImg = "<\\s*img\\s+[^>]*(src)+[^>]*>";

		public static string _strRight = "<img\\b[^<>]*?\\bsrc[\\s\\t\\r\\n]*=[\\s\\t\\r\\n]*[\"']?[\\s\\t\\r\\n]*(?<imgUrl>[^\\s\\t\\r\\n\"'<>]*)[^<>]*?/?[\\s\\t\\r\\n]*>";

		private static Bitmap _colorBitmap;

		private static MatchCollection GetMatchPicPath(string content)
		{
			return Regex.Matches(content, _imgPatern, RegexOptions.IgnoreCase);
		}

		private static Bitmap GetColorBitmap()
		{
			Random random = new Random();
			Bitmap bitmap = new Bitmap(500, 500);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.High;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.Clear(Color.Transparent);
				graphics.DrawImage(bitmap, new Rectangle(0, 0, 500, 500), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
				for (int i = 0; i < bitmap.Width; i++)
				{
					for (int j = 0; j < bitmap.Height; j++)
					{
						bitmap.GetPixel(i, j);
						Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
						bitmap.SetPixel(i, j, color);
					}
				}
				return bitmap;
			}
		}

		private static string FilterChar(string txt, string fileterCharText)
		{
			if (!string.IsNullOrEmpty(txt) && !string.IsNullOrEmpty(fileterCharText))
			{
				fileterCharText = fileterCharText.Replace('，', ',');
				if (fileterCharText.IndexOf(',') > -1)
				{
					string[] array = fileterCharText.Split(',');
					if (array != null && array.Length > 0)
					{
						for (int i = 0; i < array.Length; i++)
						{
							txt = txt.Replace(array[i], "");
						}
					}
				}
				return txt;
			}
			return txt;
		}

		public static string GetMobileDesc(int itemId, string content, string filePath, Dictionary<string, string> dicPicFileNameAndSize, bool isCreatePicAndWord, bool isFilterChar, string filterCharText)
		{
			try
			{
				if (!Directory.Exists(filePath))
				{
					Directory.CreateDirectory(filePath);
				}
			}
			catch (Exception ex)
			{
				Log.WriteLog("这都出错，要死啊", ex);
			}
			int num = 0;
			string text = string.Empty;
			MatchCollection matchPicPath = GetMatchPicPath(content);
			string text2 = content;
			if (matchPicPath.Count > 0)
			{
				for (int i = 0; i < matchPicPath.Count; i++)
				{
					string value = matchPicPath[i].Groups[0].Value;
					if (text2.IndexOf(value) != text2.LastIndexOf(value))
					{
						int startIndex = text2.IndexOf(value);
						text2 = text2.Replace(value, "");
						text2 = text2.Insert(startIndex, value);
					}
				}
			}
			MatchCollection matchPicPath2 = GetMatchPicPath(text2);
			if (matchPicPath2.Count > 0)
			{
				Dictionary<string, string> dictionary = null;
				for (int j = 0; j < matchPicPath2.Count; j++)
				{
					dictionary = new Dictionary<string, string>();
					string value2 = matchPicPath2[j].Groups[0].Value;
					string text3 = string.Empty;
					string str = string.Empty;
					if (isCreatePicAndWord)
					{
						string oldstrImg = string.Empty;
						if (j > 0)
						{
							oldstrImg = matchPicPath2[j - 1].Groups[0].Value;
						}
						if (string.IsNullOrEmpty(text3))
						{
							text3 = GetNewTxt(text2, oldstrImg, value2);
						}
						if (matchPicPath2.Count == 1)
						{
							str = GetNewTxt(text2, oldstrImg, value2, matchPicPath2.Count);
						}
						if (!string.IsNullOrEmpty(text3) && isCreatePicAndWord && isFilterChar)
						{
							text3 = FilterChar(text3, filterCharText);
						}
						if (!string.IsNullOrEmpty(text3))
						{
							text += text3;
						}
					}
					if (value2.StartsWith("file:///"))
					{
						dictionary["src"] = value2;
					}
					else
					{
						string pattern = "(src)=(?([\"\\'])[\"\\']([^\"\\']+?)[\"\\']|(\\s*[^\\s]*))";
						MatchCollection matchCollection = Regex.Matches(value2, pattern, RegexOptions.IgnoreCase);
						if (matchCollection != null)
						{
							foreach (Match item in matchCollection)
							{
								if (!string.IsNullOrEmpty(item.Groups[1].Value))
								{
									if (!string.IsNullOrEmpty(item.Groups[2].Value))
									{
										dictionary[item.Groups[1].Value.ToLower()] = item.Groups[2].Value;
									}
									else if (!string.IsNullOrEmpty(item.Groups[3].Value))
									{
										dictionary[item.Groups[1].Value.ToLower()] = item.Groups[3].Value;
									}
								}
							}
						}
					}
					if (dictionary.ContainsKey("src") && dicPicFileNameAndSize.ContainsKey(dictionary["src"]))
					{
						string text4 = dictionary["src"];
						string text5 = dicPicFileNameAndSize[text4];
						int num2 = DataConvert.ToInt((object)text5.Split(',')[0]);
						int num3 = DataConvert.ToInt((object)text5.Split(',')[1]);
						if (num2 >= 480 && !(DataConvert.ToDecimal((object)num2) / DataConvert.ToDecimal((object)num3) > 8m))
						{
							int num4 = 0;
							string text6 = text4.Replace("file:///", string.Empty);
							if (text6.LastIndexOf('/') != -1)
							{
								string text7 = filePath + text6.Substring(text6.LastIndexOf('/') + 1);
								string newPicFileName = text7.Insert(text7.LastIndexOf('.'), "_dmjbs");
								int maxWidth = num2;
								if (num2 > 620)
								{
									maxWidth = 620;
								}
								List<string> list = CompressJpeg(text6, newPicFileName, maxWidth, 960, 102400L, out num4, num);
								if (list != null && list.Count > 0)
								{
									num += num4;
									for (int k = 0; k < list.Count; k++)
									{
										if (list[k].IndexOf(".gif", StringComparison.OrdinalIgnoreCase) > 0)
										{
											try
											{
												string text8 = list[k].Substring(0, list[k].LastIndexOf('.'));
												Image image = Image.FromFile(list[k]);
												FrameDimension dimension = new FrameDimension(image.FrameDimensionsList[0]);
												int frameCount = image.GetFrameCount(dimension);
												for (int l = 0; l < frameCount; l++)
												{
													image.SelectActiveFrame(dimension, l);
													image.Save(text8 + ".png", ImageFormat.Png);
													text = text + "<img>" + text8 + ".png</img>";
												}
											}
											catch (Exception arg)
											{
												Log.WriteLog("转换gif格式图片出错" + arg);
											}
										}
										else
										{
											text = text + "<img>" + list[k] + "</img>";
										}
									}
								}
								goto IL_050d;
							}
						}
						continue;
					}
					goto IL_050d;
					IL_050d:
					if (isCreatePicAndWord && j == matchPicPath2.Count - 1)
					{
						string oldstrImg2 = string.Empty;
						if (j > 0)
						{
							oldstrImg2 = matchPicPath2[j].Groups[0].Value;
						}
						text3 = GetTxt(text2, oldstrImg2, string.Empty);
						if (string.IsNullOrEmpty(text3))
						{
							text3 = GetNewTxt(text2, oldstrImg2, value2);
						}
						if (string.IsNullOrEmpty(text3))
						{
							text3 = GetItemComtentTxt(text2, oldstrImg2, value2);
						}
						if (!string.IsNullOrEmpty(text3) && isCreatePicAndWord && isFilterChar)
						{
							text3 = FilterChar(text3, filterCharText);
						}
						if (!string.IsNullOrEmpty(text3))
						{
							text += text3;
						}
					}
					if (matchPicPath2.Count == 1)
					{
						text += str;
					}
				}
			}
			else if (!string.IsNullOrEmpty(text2) && isCreatePicAndWord)
			{
				text = GetTxt(text2, isCreatePicAndWord, isFilterChar, filterCharText);
			}
			return text;
		}

		private static string GetTxt(string content, bool isCreatePicAndWord, bool isFilterChar, string filterCharText)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(content))
			{
				HtmlDocument htmlDocument = new HtmlDocument();
				content = content.Replace("&nbsp;", " ");
				htmlDocument.LoadHtml(content);
				if (!string.IsNullOrEmpty(htmlDocument.DocumentNode.InnerText))
				{
					text = htmlDocument.DocumentNode.InnerText.Replace("<", "").Replace(">", "");
					if (isCreatePicAndWord && isFilterChar)
					{
						text = FilterChar(text, filterCharText);
					}
				}
			}
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				while (text.Length > 500)
				{
					string oldValue = text.Substring(0, 500);
					text2 = text2 + "<txt>" + text.Substring(0, 500) + "</txt>";
					text = text.Replace(oldValue, "");
				}
				text2 = text2 + "<txt>" + text.Substring(0, text.Length) + "</txt>";
			}
			return text2;
		}

		private static string GetTxt(string content, string oldstrImg, string strImg)
		{
			string text = string.Empty;
			string empty = string.Empty;
			int num = 0;
			int num2 = 0;
			if (string.IsNullOrEmpty(oldstrImg))
			{
				num = content.IndexOf(strImg, 0);
				empty = content.Substring(0, num);
			}
			else
			{
				num = content.IndexOf(oldstrImg, 0) + oldstrImg.Length;
				if (!string.IsNullOrEmpty(strImg))
				{
					num2 = content.IndexOf(strImg, num);
				}
				empty = ((num2 <= num) ? content.Substring(num) : content.Substring(num, num2 - num));
				if (empty.IndexOf(oldstrImg) >= 0)
				{
					num = empty.IndexOf(oldstrImg, 0) + oldstrImg.Length;
					if (empty.Length > num)
					{
						empty = empty.Substring(num);
					}
				}
			}
			if (!string.IsNullOrEmpty(empty) && empty.IndexOf("table") < 0)
			{
				HtmlDocument htmlDocument = new HtmlDocument();
				Regex regex = new Regex("(?is)<([^>]+>)+");
				Match match = regex.Match(empty);
				if (match != null && match.Success)
				{
					empty = match.Value;
				}
				if (empty.IndexOf("<") == -1 && empty.EndsWith(">"))
				{
					text = string.Empty;
				}
				else
				{
					empty = empty.Replace("&nbsp;", " ");
					htmlDocument.LoadHtml(empty);
					if (!string.IsNullOrEmpty(htmlDocument.DocumentNode.InnerText.Trim()))
					{
						text = htmlDocument.DocumentNode.InnerText.Replace("<", "").Replace(">", "").Trim();
					}
				}
			}
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				while (text.Length > 500)
				{
					string oldValue = text.Substring(0, 500).Trim();
					text2 = text2 + "<txt>" + text.Substring(0, 500).Trim() + "</txt>";
					text = text.Replace(oldValue, "");
				}
				if (!string.IsNullOrEmpty(text))
				{
					text2 = text2 + "<txt>" + text.Substring(0, text.Length).Trim() + "</txt>";
				}
			}
			if (!string.IsNullOrEmpty(text2) && text2.IndexOf(" ") > 0)
			{
				string text3 = string.Empty;
				string[] array = text2.Split(new string[3]
				{
					" ",
					" ",
					"\u00a0"
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						text3 += array[i].Trim();
					}
					return text3;
				}
				return text2;
			}
			return text2;
		}

		private static List<string> CompressJpeg(string sourcePicFileName, string newPicFileName, int maxWidth, int maxHeight, long maxLength, out int byteSize, int totalByteSize)
		{
			return LocalFileOperater.CompressJpeg(sourcePicFileName, newPicFileName, maxWidth, 960, 102400L, out byteSize, totalByteSize);
		}

		public static List<string> CompressDescJpeg(string sourcePicFileName, string newPicFileName, int maxWidth, int maxHeight, long maxLength, out int byteSize, int totalByteSize)
		{
			return LocalFileOperater.CompressJpeg(sourcePicFileName, newPicFileName, maxWidth, 960, 102400L, out byteSize, totalByteSize);
		}

		public static List<string> CompressDescJpegBig(string sourcePicFileName, string newPicFileName, int maxWidth, int maxHeight, long maxLength, out int byteSize, int totalByteSize)
		{
			return LocalFileOperater.CompressJpeg(sourcePicFileName, newPicFileName, maxWidth, maxHeight, maxLength, out byteSize, totalByteSize);
		}

		public static List<string> GetAllPicUrlByItemContent(string content)
		{
			List<string> list = new List<string>();
			MatchCollection matchPicPath = GetMatchPicPath(content);
			if (matchPicPath.Count > 0)
			{
				for (int i = 0; i < matchPicPath.Count; i++)
				{
					string value = matchPicPath[i].Groups[0].Value;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary = GetSourceUrlDic(value);
					if (dictionary != null && dictionary.Count > 0)
					{
						foreach (KeyValuePair<string, string> item in dictionary)
						{
							if (!list.Contains(item.Key))
							{
								list.Add(item.Key);
							}
						}
					}
				}
			}
			return list;
		}

		private static string DownloadItemContentPicFact(int itemId, string content, string filePath, bool IsTaobao5, ref Dictionary<string, string> moveUrlDic, ref bool uccorError)
		{
			MatchCollection matchPicPath = GetMatchPicPath(content);
			if (matchPicPath.Count > 0)
			{
				string empty = string.Empty;
				empty = ((itemId <= 0) ? (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Guid.NewGuid().ToString().Substring(0, 6)) : (DataConvert.ToString((object)itemId) + Guid.NewGuid().ToString().Substring(0, 6)));
				for (int i = 0; i < matchPicPath.Count; i++)
				{
					if (uccorError)
					{
						break;
					}
					string value = matchPicPath[i].Groups[0].Value;
					try
					{
                       
						DownloadPicture(ref content, value, filePath, empty + "-" + (i + 1), true, IsTaobao5, ref moveUrlDic, ref uccorError);
					}
					catch (Exception ex)
					{
						try
						{
							Log.WriteLog("下载描述图片" + value + "失败", ex);
						}
						catch
						{
						}
					}
				}
			}
			content = RemoveUselessPictures(content);
			return content;
		}

		public static string DownloadItemContentPic(int itemId, string content, string filePath, bool IsTaobao5, ref Dictionary<string, string> moveUrlDic, ref bool uccorError)
		{
			bool flag = false;
			lock (_lockOjb)
			{
				if (!_initial)
				{
					_initial = true;
					flag = true;
					_firtItemPicInDownload = true;
                    _samePicDownloadOneTime = ConfigHelper.SamePicDownloadOneTime;//  DataConvert.ToBoolean((object)AppConfig.GetConfig("AppConfig", "Basic", "SamePicDownloadOneTime"));
					if (moveUrlDic == null)
					{
						moveUrlDic = new Dictionary<string, string>();
					}
				}
			}
			Dictionary<string, string> dictionary = null;
			dictionary = ((!_samePicDownloadOneTime) ? new Dictionary<string, string>() : moveUrlDic);
			try
			{
				if (_samePicDownloadOneTime && _firtItemPicInDownload)
				{
					lock (_lockOjb)
					{
						content = DownloadItemContentPicFact(itemId, content, filePath, IsTaobao5, ref dictionary, ref uccorError);
					}
				}
				else
				{
					content = DownloadItemContentPicFact(itemId, content, filePath, IsTaobao5, ref dictionary, ref uccorError);
				}
			}
			catch (Exception ex)
			{
				Log.WriteLog(ex);
			}
			if (flag)
			{
				_firtItemPicInDownload = false;
			}
			return content;
		}

		private static string GetBaohuSrc()
		{
            string text = ConfigHelper.GetCurrentDomainDirectory() + "Tool\\COMMODULE\\OdPNb.gif";
			if (File.Exists(text))
			{
				text = text.Replace('\\', '/');
				return "file:///" + text;
			}
			ImageOperator imageOperator = new ImageOperator();
			text = imageOperator.DownLoadPicture(_baohuSrcUrl, text, 3000, 5000);
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Replace('\\', '/');
				text = "file:///" + text;
			}
			return text;
		}

		public static string ProtectPic(string itemContent, Dictionary<string, string> dicPicFileNameAndSize)
		{
			if (string.IsNullOrEmpty(itemContent))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(_baohuLocalPath) || !File.Exists(_baohuLocalPath.Replace("file:///", string.Empty)))
			{
				_baohuLocalPath = GetBaohuSrc();
			}
			if (string.IsNullOrEmpty(_baohuLocalPath))
			{
				return itemContent;
			}
			MatchCollection matchCollection = Regex.Matches(itemContent, _protectImg, RegexOptions.IgnoreCase);
			if (matchCollection.Count > 0)
			{
				Dictionary<string, string> dictionary = null;
				for (int i = 0; i < matchCollection.Count; i++)
				{
					dictionary = new Dictionary<string, string>();
					string value = matchCollection[i].Groups[0].Value;
					string pattern = "(alt)=(?(\")\"([^\"]+?)\"|\\s*[^\\s]*)|(src)=(?(\")\"([^\"]+?)\"|\\s*[^\\s]*)";
					MatchCollection matchCollection2 = Regex.Matches(value, pattern, RegexOptions.IgnoreCase);
					if (matchCollection2 != null)
					{
						foreach (Match item in matchCollection2)
						{
							if (!string.IsNullOrEmpty(item.Groups[1].Value))
							{
								dictionary[item.Groups[1].Value.ToLower()] = item.Groups[2].Value;
							}
							else
							{
								dictionary[item.Groups[3].Value.ToLower()] = item.Groups[4].Value;
							}
						}
					}
					if ((dictionary.Count != 2 || !dictionary.ContainsKey("src") || !Path.GetExtension(dictionary["src"]).ToLower().Equals("gif")) && dictionary.ContainsKey("src") && dicPicFileNameAndSize.ContainsKey(dictionary["src"]))
					{
						string text = dictionary["src"];
						if (!(text == _baohuSrcUrl))
						{
							string picSize = dicPicFileNameAndSize[text];
							string protectImgStr = GetProtectImgStr(text, picSize);
							if (!string.IsNullOrEmpty(protectImgStr))
							{
								itemContent = itemContent.Replace(value, protectImgStr);
							}
						}
					}
				}
			}
			return itemContent;
		}

		private static string GetProtectImgStr(string srcPic, string picSize)
		{
			string[] array = picSize.Split(',');
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (array.Length == 0)
			{
				return string.Empty;
			}
			empty = array[0];
			empty2 = array[0];
			if (DataConvert.ToInt((object)empty) >= 400 && DataConvert.ToInt((object)empty2) >= 300)
			{
				if (array.Length > 1)
				{
					empty2 = array[1];
				}
				string value = "<IMG src=\"" + _baohuLocalPath + "\" width=\"" + empty + "\" height=\"" + empty2 + "\">";
				string value2 = "<TD style=\"BACKGROUND-REPEAT: no-repeat\" background=\"" + srcPic + "\">";
				string value3 = "<TABLE align=\"center\" width=\"" + empty + "\" height=\"" + empty2 + "\">";
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(value3);
				stringBuilder.Append("<TBODY>");
				stringBuilder.Append("<TR>");
				stringBuilder.Append(value2);
				stringBuilder.Append(value);
				stringBuilder.Append("</TD></TR></TBODY></TABLE>");
				return stringBuilder.ToString();
			}
			return string.Empty;
		}

		private static string DownloadItemContentPicFact(int itemId, string content, string photoPath, bool isFangBaohu, bool isPicAddWater, ref Dictionary<string, string> moveUrlDic, ref bool uccorError, AddWaterMarkEntity addWaterMarkEntity, Dictionary<string, string> dicPicFileNameAndSize)
		{
			MatchCollection matchPicPath = GetMatchPicPath(content);
			if (matchPicPath.Count > 0)
			{
				string empty = string.Empty;
				empty = ((itemId <= 0) ? (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + new Random().Next(10, 99)) : (DataConvert.ToString((object)itemId) + "-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()));
				for (int i = 0; i < matchPicPath.Count; i++)
				{
					if (uccorError)
					{
						break;
					}
					string value = matchPicPath[i].Groups[0].Value;
					try
					{
						DownloadPicture(ref content, value, photoPath, empty + "-" + (i + 1), true, false, isFangBaohu, isPicAddWater, addWaterMarkEntity, ref moveUrlDic, ref uccorError, dicPicFileNameAndSize);
					}
					catch (Exception ex)
					{
						try
						{
							Log.WriteLog("下载描述图片" + value + "失败", ex);
						}
						catch
						{
						}
					}
				}
			}
			content = RemoveUselessPictures(content);
			return content;
		}

		public static string DownloadItemContentPic(int itemId, string content, string photoPath, bool isFangBaohu, bool isPicAddWater, ref Dictionary<string, string> moveUrlDic, ref bool uccorError, AddWaterMarkEntity addWaterMarkEntity, Dictionary<string, string> dicPicFileNameAndSize)
		{
			bool flag = false;
			lock (_lockOjb)
			{
				if (!_initial)
				{
					_initial = true;
					flag = true;
					_firtItemPicInDownload = true;
                    _samePicDownloadOneTime = ConfigHelper.SamePicDownloadOneTime;// DataConvert.ToBoolean((object)AppConfig.GetConfig("AppConfig", "Basic", "SamePicDownloadOneTime"));
					if (moveUrlDic == null)
					{
						moveUrlDic = new Dictionary<string, string>();
					}
				}
			}
			Dictionary<string, string> dictionary = null;
			dictionary = ((!_samePicDownloadOneTime) ? new Dictionary<string, string>() : moveUrlDic);
			try
			{
				if (_samePicDownloadOneTime && _firtItemPicInDownload)
				{
					lock (_lockOjb)
					{
						content = DownloadItemContentPicFact(itemId, content, photoPath, isFangBaohu, isPicAddWater, ref dictionary, ref uccorError, addWaterMarkEntity, dicPicFileNameAndSize);
					}
				}
				else
				{
					content = DownloadItemContentPicFact(itemId, content, photoPath, isFangBaohu, isPicAddWater, ref dictionary, ref uccorError, addWaterMarkEntity, dicPicFileNameAndSize);
				}
			}
			catch (Exception ex)
			{
				Log.WriteLog(ex);
			}
			if (flag)
			{
				_firtItemPicInDownload = false;
			}
			return content;
		}

		private static bool DownloadPicture(ref string content, string strImg, string photoPath, string fileName, bool isReplacePicture, bool isTaobao5, bool isFangBaohu, bool isPicAddWater, AddWaterMarkEntity addWaterMarkEntity, ref Dictionary<string, string> moveUrlDic, ref bool uccorError, Dictionary<string, string> dicPicFileNameAndSize)
		{
			PictureWaterMark pictureWaterMark = null;
			bool flag = false;
			if (string.IsNullOrEmpty(strImg))
			{
				return flag;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = GetSourceUrlDic(strImg);
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				bool flag2 = true;
				string key = item.Key;
				string value = item.Value;
				if (!(value == _baohuSrcUrl))
				{
					try
					{
						if (string.IsNullOrEmpty(key))
						{
							content = content.Replace(strImg, string.Empty);
							return flag;
						}
						string text = string.Empty;
						if (moveUrlDic.ContainsKey(key))
						{
							text = moveUrlDic[key];
							string empty = string.Empty;
							empty = strImg.Replace(value, text);
							content = content.Replace(strImg, empty);
							strImg = empty;
							string text2 = string.Empty;
							if (text.IndexOf("file:///", 0) > -1)
							{
								text2 = HttpUtility.UrlDecode(text);
								text2 = text2.Replace("file:///", string.Empty);
								if (File.Exists(text2))
								{
									text2 = text2.Replace("/", "\\");
								}
							}
							string value2 = string.Empty;
							if (File.Exists(text2))
							{
								Image image = Image.FromFile(text2);
								try
								{
									if (image.Size.Height > 0 && image.Size.Width > 400)
									{
										value2 = image.Size.Width + "," + image.Size.Height;
									}
								}
								catch (Exception ex)
								{
									Log.WriteLog(ex);
								}
								finally
								{
									image?.Dispose();
								}
							}
							if (!string.IsNullOrEmpty(value2))
							{
								dicPicFileNameAndSize[text] = value2;
							}
							goto end_IL_0056;
						}
						if (key.IndexOf("file:///", StringComparison.OrdinalIgnoreCase) >= 0 || key.IndexOf("file:\\\\", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							try
							{
								new Uri(key);
							}
							catch (Exception ex2)
							{
								Log.WriteLog(ex2);
								flag2 = false;
							}
						}
						else
						{
							flag2 = DataDetect.IsValidUrl(key, false);
						}
						string empty2 = string.Empty;
						int maxDescPicSize = GetMaxDescPicSize();
                        ControlsUtils.OperationLog("下载图片:" + key);
                        flag = DownLoadPicture(HttpUtility.HtmlDecode(key), photoPath, fileName, maxDescPicSize, out text, ref uccorError, out empty2);
						if (!flag && uccorError)
						{
							return false;
						}
						ImageFormat formatByExtension;
						string value4;
						Image image4;
						Image image5;
						if (flag)
						{
							if (!string.IsNullOrEmpty(empty2) && moveUrlDic.ContainsKey(empty2))
							{
								text = moveUrlDic[empty2];
								string empty3 = string.Empty;
								empty3 = strImg.Replace(value, text);
								content = content.Replace(strImg, empty3);
								strImg = empty3;
								string text3 = string.Empty;
								if (text.IndexOf("file:///", 0) > -1)
								{
									text3 = HttpUtility.UrlDecode(text);
									text3 = text3.Replace("file:///", string.Empty);
									if (File.Exists(text3))
									{
										text3 = text3.Replace("/", "\\");
									}
								}
								string value3 = string.Empty;
								if (File.Exists(text3))
								{
									Image image2 = Image.FromFile(text3);
									try
									{
										if (image2.Size.Height > 0 && image2.Size.Width > 400)
										{
											value3 = image2.Size.Width + "," + image2.Size.Height;
										}
									}
									catch (Exception ex3)
									{
										Log.WriteLog(ex3);
									}
									finally
									{
										image2?.Dispose();
									}
								}
								if (!string.IsNullOrEmpty(value3))
								{
									dicPicFileNameAndSize[text] = value3;
								}
								goto IL_085f;
							}
							string text4 = string.Empty;
							if (!string.IsNullOrEmpty(text) && text.LastIndexOf(".") > 0)
							{
								text4 = text.Substring(text.LastIndexOf("."));
							}
							if (string.IsNullOrEmpty(text4))
							{
								text4 = ".jpeg";
							}
							formatByExtension = GetFormatByExtension(text4);
							value4 = string.Empty;
							if (File.Exists(text))
							{
								Image image3 = Image.FromFile(text);
								try
								{
									if (image3.Size.Height > 0 && image3.Size.Width > 400)
									{
										value4 = image3.Size.Width + "," + image3.Size.Height;
									}
								}
								catch (Exception ex4)
								{
									Log.WriteLog(ex4);
								}
								finally
								{
									image3?.Dispose();
								}
							}
							if (!isPicAddWater && !isFangBaohu)
							{
								goto IL_07b0;
							}
							image4 = null;
							if (File.Exists(text))
							{
								image5 = null;
								image5 = Image.FromFile(text);
								if (image5 != null && image5.HorizontalResolution == 1f)
								{
									goto IL_04b3;
								}
								if (image5.VerticalResolution == 1f)
								{
									goto IL_04b3;
								}
								goto IL_0560;
							}
							goto IL_06c4;
						}
						if (key.IndexOf(".gif", StringComparison.OrdinalIgnoreCase) > 0)
						{
                            string text5 = ConfigHelper.GetCurrentDomainDirectory() + "Tool\\COMMODULE\\OdPNb.gif";
							text5 = text5.Replace('\\', '/');
							text5 = "file:///" + text5;
							moveUrlDic[key] = text5;
							flag = true;
						}
						goto IL_085f;
						IL_07b0:
						text = text.Replace('\\', '/');
						text = "file:///" + text;
						text = text.Replace(" ", "%20");
						moveUrlDic[key] = text;
						if (!string.IsNullOrEmpty(empty2))
						{
							moveUrlDic[empty2] = text;
						}
						if (!string.IsNullOrEmpty(value4))
						{
							dicPicFileNameAndSize[text] = value4;
						}
						goto IL_085f;
						IL_06c4:
						if (isFangBaohu && !string.IsNullOrEmpty(text) && File.Exists(text) && formatByExtension != ImageFormat.Gif && !text.Contains("_H."))
						{
							Image image6 = null;
							Image image7 = null;
							Image image8 = null;
							try
							{
								image7 = Image.FromFile(text);
								if (image7 != null)
								{
									if (_colorBitmap == null)
									{
										_colorBitmap = GetColorBitmap();
									}
									image8 = new Bitmap(_colorBitmap, image7.Width, image7.Height);
									pictureWaterMark = new PictureWaterMark(image7, image8, 0.01f, MarkLocation.LeftUp, CompressSize.None);
									image6 = pictureWaterMark.MarkByLocation();
									if (image6 != null)
									{
										text = text.Insert(text.LastIndexOf('.'), "_H");
										pictureWaterMark.SaveBmpWithImageCodecInfo(image6, text);
									}
								}
							}
							catch (Exception ex5)
							{
								Log.WriteLog(ex5);
							}
							finally
							{
								image6?.Dispose();
								image7?.Dispose();
								image8?.Dispose();
							}
						}
						goto IL_07b0;
						IL_0560:
						if (isPicAddWater && addWaterMarkEntity != null && image5 != null && File.Exists(text) && formatByExtension != ImageFormat.Gif)
						{
							bool flag3 = true;
							try
							{
								if (image5.Size.Height > 240 && image5.Size.Width > 240)
								{
									if (text.Contains("_wdajtser"))
									{
										if (addWaterMarkEntity.isNotAddWaterToo)
										{
											flag3 = false;
										}
									}
									else
									{
										text = text.Insert(text.LastIndexOf('.'), "_wdajtser");
									}
									if (flag3)
									{
										if (addWaterMarkEntity.aexeType.Equals("txt"))
										{
											pictureWaterMark = new PictureWaterMark(image5, addWaterMarkEntity.awaterText, addWaterMarkEntity.atextFont, addWaterMarkEntity.atextColor, addWaterMarkEntity.atransparence, addWaterMarkEntity.amarkLocation, addWaterMarkEntity.compressSize);
											image4 = pictureWaterMark.MarkByLocation();
										}
										if (addWaterMarkEntity.aexeType.Equals("pic"))
										{
											pictureWaterMark = new PictureWaterMark(image5, addWaterMarkEntity.awaterImg, addWaterMarkEntity.atransparence, addWaterMarkEntity.amarkLocation, addWaterMarkEntity.compressSize);
											image4 = pictureWaterMark.MarkByLocation();
										}
									}
								}
							}
							catch (Exception ex6)
							{
								Log.WriteLog(ex6);
							}
							image5?.Dispose();
							if (image4 != null && !string.IsNullOrEmpty(text))
							{
								pictureWaterMark.SaveBmpWithImageCodecInfo(image4, text);
							}
							image4?.Dispose();
						}
						goto IL_06c4;
						IL_085f:
						if (isReplacePicture)
						{
							if (flag)
							{
								string empty4 = string.Empty;
								empty4 = strImg.Replace(value, text);
								content = content.Replace(strImg, empty4);
								strImg = empty4;
							}
							else if (dictionary.Count == 1)
							{
								if (strImg.StartsWith("<img ", StringComparison.OrdinalIgnoreCase) && content.IndexOf(strImg + ">", StringComparison.OrdinalIgnoreCase) >= 0)
								{
									strImg += ">";
								}
								content = content.Replace(strImg, string.Empty);
							}
							else if (flag2)
							{
								string empty5 = string.Empty;
								empty5 = strImg.Replace(value, string.Empty);
								content = content.Replace(strImg, empty5);
								strImg = empty5;
							}
						}
						goto end_IL_0056;
						IL_04b3:
						Bitmap bitmap = new Bitmap(image5.Width, image5.Height, PixelFormat.Format24bppRgb);
						bitmap.SetResolution(96f, 96f);
						Graphics graphics = Graphics.FromImage(bitmap);
						graphics.DrawImage(image5, new Rectangle(0, 0, image5.Width, image5.Height), 0, 0, image5.Width, image5.Height, GraphicsUnit.Pixel);
						graphics.Dispose();
						string filename = ConfigHelper.GetCurrentDomainDirectory() + "pic\\SNATCH\\" + Guid.NewGuid().ToString() + ".jpg";
						bitmap.Save(filename);
						bitmap.Dispose();
						image5 = Image.FromFile(filename);
						goto IL_0560;
						end_IL_0056:;
					}
					catch (Exception ex7)
					{
						string empty6 = string.Empty;
						empty6 = strImg.Replace(value, string.Empty);
						content = content.Replace(strImg, empty6);
						strImg = empty6;
						Log.WriteLog(ex7);
					}
				}
			}
			return flag;
		}

		private static int GetMaxDescPicSize()
		{
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            string userConfig = ConfigHelper.ExportDescSize;// ToolServer.get_ConfigData().GetUserConfig("AppConfig", "DescPicSize", "ExportDescSize", "");
			switch (userConfig)
			{
			case "200K":
				return 204800;
			case "500K":
				return 524288;
			case "1M":
				return 1048576;
			case "2M":
				return 2097152;
			default:
				return 3145728;
			}
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

		public static void DecodeLocalImgPath(ref string desc)
		{
			if (!string.IsNullOrEmpty(desc))
			{
				string pattern = "(\\<img[^\\>]+\\>)|(<img(.|\\s)>)|(<img>)|(<img/>)";
				MatchCollection matchCollection = Regex.Matches(desc, pattern, RegexOptions.IgnoreCase);
				if (matchCollection.Count > 0)
				{
					string text = null;
					foreach (Match item in matchCollection)
					{
						text = item.Groups[0].Value;
						desc = DecodePicturePath(desc, text);
					}
				}
			}
		}

		private static string DecodePicturePath(string content, string strImg)
		{
			MatchCollection matchCollection = Regex.Matches(strImg, _strRight, RegexOptions.IgnoreCase);
			if (matchCollection.Count > 0)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary = GetSourceUrlDic(strImg);
				if (dictionary.Count > 0)
				{
					{
						foreach (KeyValuePair<string, string> item in dictionary)
						{
							string key = item.Key;
							string empty = string.Empty;
							if (IsLoalPic(key, out empty))
							{
								content = content.Replace(strImg, HttpUtility.UrlDecode(strImg));
							}
						}
						return content;
					}
				}
			}
			return content;
		}

		public static void RemoveUselessPictures(ref string desc, ref int removeCount)
		{
			removeCount = 0;
			if (!string.IsNullOrEmpty(desc))
			{
				string pattern = "(\\<img[^\\>]+\\>)|(<img(.|\\s)>)|(<img>)|(<img/>)";
				MatchCollection matchCollection = Regex.Matches(desc, pattern, RegexOptions.IgnoreCase);
				if (matchCollection.Count > 0)
				{
					string text = null;
					bool flag = false;
					foreach (Match item in matchCollection)
					{
						text = item.Groups[0].Value;
						flag = false;
						desc = CheckPicture(desc, text, out flag);
						if (flag)
						{
							removeCount++;
						}
					}
				}
			}
		}

		private static string CheckPicture(string content, string strImg, out bool isRemove)
		{
			isRemove = false;
			MatchCollection matchCollection = Regex.Matches(strImg, _strRight, RegexOptions.IgnoreCase);
			if (matchCollection.Count > 0)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary = GetSourceUrlDic(strImg);
				if (dictionary.Count == 0)
				{
					content = content.Replace(strImg, "&nbsp;");
					isRemove = true;
					goto IL_0175;
				}
				HttpWebResponse httpWebResponse = null;
				{
					foreach (KeyValuePair<string, string> item in dictionary)
					{
						string key = item.Key;
						if (!(key == _baohuSrcUrl))
						{
							string empty = string.Empty;
							if (IsLoalPic(key, out empty))
							{
								if (string.IsNullOrEmpty(empty) || !File.Exists(empty))
								{
									content = content.Replace(strImg, "&nbsp;");
									isRemove = true;
								}
							}
							else
							{
								try
								{
									HttpWebRequest httpWebRequest = UrlUtil.GetHttpWebRequest(key);
									httpWebRequest.Headers.Set("Pragma", "no-cache");
									ServicePointManager.Expect100Continue = false;
									httpWebRequest.Timeout = 5000;
									httpWebRequest.ServicePoint.ConnectionLimit = 200;
									httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
									if (!httpWebResponse.ResponseUri.IsAbsoluteUri)
									{
										content = content.Replace(strImg, "&nbsp;");
										isRemove = true;
									}
								}
								catch
								{
									content = content.Replace(strImg, "&nbsp;");
									isRemove = true;
								}
								finally
								{
									httpWebResponse?.Close();
								}
							}
						}
					}
					return content;
				}
			}
			content = content.Replace(strImg, "&nbsp;");
			isRemove = true;
			goto IL_0175;
			IL_0175:
			return content;
		}

		public static string RemoveUselessPictures(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return content;
			}
			string pattern = "(\\<img[^\\>]+\\>)|(<img(.|\\s)>)|(<img>)|(<img/>)";
			MatchCollection matchCollection = Regex.Matches(content, pattern, RegexOptions.IgnoreCase);
			for (int i = 0; i < matchCollection.Count; i++)
			{
				string value = matchCollection[i].Groups[0].Value;
				content = CheckAndRemovePicture(content, value);
			}
			return content;
		}

		private static string CheckAndRemovePicture(string content, string strImg)
		{
			MatchCollection matchCollection = Regex.Matches(strImg, _strRight, RegexOptions.IgnoreCase);
			if (matchCollection.Count > 0)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary = GetSourceUrlDic(strImg);
				if (dictionary.Count == 0)
				{
					if (strImg.IndexOf("src=\"#\"", StringComparison.OrdinalIgnoreCase) < 0)
					{
						content = content.Replace(strImg, "&nbsp;");
					}
					goto IL_00f4;
				}
				{
					foreach (KeyValuePair<string, string> item in dictionary)
					{
						string key = item.Key;
						if (!(key == _baohuSrcUrl))
						{
							string empty = string.Empty;
							if (IsLoalPic(key, out empty) && (string.IsNullOrEmpty(empty) || !File.Exists(empty)) && strImg.IndexOf("src=\"#\"", StringComparison.OrdinalIgnoreCase) < 0)
							{
								content = content.Replace(strImg, "&nbsp;");
							}
						}
					}
					return content;
				}
			}
			if (strImg.IndexOf("src=\"#\"", StringComparison.OrdinalIgnoreCase) < 0)
			{
				content = content.Replace(strImg, "&nbsp;");
			}
			goto IL_00f4;
			IL_00f4:
			return content;
		}

		private static bool IsLoalPic(string url, out string file)
		{
			file = string.Empty;
			if (string.IsNullOrEmpty(url))
			{
				return false;
			}
			bool result = false;
			if (url.IndexOf("file:///", StringComparison.OrdinalIgnoreCase) < 0 && url.IndexOf("file:\\\\", StringComparison.OrdinalIgnoreCase) < 0)
			{
				return result;
			}
			try
			{
				Uri uri = new Uri(url);
				file = uri.LocalPath + uri.Fragment;
				result = true;
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static Dictionary<string, string> GetSourceUrlDic(string mValue)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(mValue))
			{
				return dictionary;
			}
			string text = string.Empty;
			string text2 = string.Empty;
			if (!mValue.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !mValue.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				string text3 = string.Empty;
				MatchCollection matchCollection = Regex.Matches(mValue, _patternUrl);
				if (matchCollection != null && matchCollection.Count > 0)
				{
					foreach (Match item in matchCollection)
					{
						text3 = item.Groups[0].Value;
						if (!list.Contains(text3))
						{
							list.Add(text3);
						}
					}
					if (!string.IsNullOrEmpty(text3) && Regex.IsMatch(text3, _patternUrl))
					{
						goto IL_0171;
					}
					return dictionary;
				}
				MatchCollection matchCollection2 = Regex.Matches(mValue, _patternLocalFile);
				if (matchCollection2 != null && matchCollection2.Count > 0)
				{
					foreach (Match item2 in matchCollection2)
					{
						text3 = item2.Groups["url"].Value;
						if (!string.IsNullOrEmpty(text3.Trim()))
						{
							list.Add(text3);
						}
					}
				}
				goto IL_0171;
			}
			text2 = (dictionary[mValue] = mValue);
			return dictionary;
			IL_0171:
			list.Sort();
			foreach (string item3 in list)
			{
				if (item3.IndexOf('"') > 0)
				{
					int num = item3.IndexOf('"') + 1;
					int num2 = item3.LastIndexOf('"') - item3.IndexOf('"') - 1;
					if (num >= 0 && num2 > 0)
					{
						text2 = item3.Substring(num, num2).Trim('\'');
						text = HttpUtility.HtmlDecode(text2);
					}
				}
				else if (item3.IndexOf('\'') > 0)
				{
					int num3 = item3.IndexOf('\'') + 1;
					int num4 = item3.LastIndexOf('\'') - item3.IndexOf('\'') - 1;
					if (num3 >= 0 && num4 > 0)
					{
						text2 = item3.Substring(num3, num4).Trim('\'');
						text = HttpUtility.HtmlDecode(text2);
					}
				}
				else if (item3.Substring(item3.Length - 1, 1) == "/")
				{
					int num5 = item3.IndexOf('=') + 1;
					int num6 = item3.Length - item3.IndexOf('=') - 2;
					if (num5 >= 0 && num6 > 0)
					{
						text2 = item3.Substring(num5, num6).Trim('\'');
						text = HttpUtility.HtmlDecode(text2);
					}
				}
				else
				{
					int num7 = item3.IndexOf('=') + 1;
					if (num7 >= 0)
					{
						text2 = item3.Substring(num7).Trim('\'');
						text = HttpUtility.HtmlDecode(text2);
					}
				}
				bool flag = true;
				if (text.IndexOf("file:///", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("file:\\\\", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					try
					{
						new Uri(text);
					}
					catch (Exception)
					{
						flag = false;
					}
				}
				else if (UrlUtil.IsValidUrl(text))
				{
					try
					{
						UrlUtil.GetHttpWebRequest(text);
					}
					catch (Exception)
					{
						try
						{
							if (text.StartsWith("about:"))
							{
								text = text.Replace("about:", "http:");
							}
							if (!text.StartsWith("http:"))
							{
								text = "http:" + text;
							}
							UrlUtil.GetHttpWebRequest(text);
						}
						catch (Exception)
						{
							flag = false;
						}
					}
				}
				else
				{
					Match match3 = Regex.Match(text, "http://[\\S]+?\\.jpg|\\.gif|\\.png|\\.jpeg|\\.bmp", RegexOptions.IgnoreCase);
					flag = (match3.Success && true);
				}
				if (flag && !string.IsNullOrEmpty(text.Trim()))
				{
					dictionary[text.Trim()] = text2.Trim();
				}
			}
			return dictionary;
		}

		private static bool DownloadPicture(ref string content, string strImg, string filePath, string fileName, bool isReplacePicture, bool isTaobao5, ref Dictionary<string, string> moveUrlDic, ref bool uccorError)
		{
			Dictionary<string, string> dicPicFileNameAndSize = new Dictionary<string, string>();
			AddWaterMarkEntity addWaterMarkEntity = null;
			return DownloadPicture(ref content, strImg, filePath, fileName, isReplacePicture, isTaobao5, false, false, addWaterMarkEntity, ref moveUrlDic, ref uccorError, dicPicFileNameAndSize);
		}

		public static bool DownLoadPicture(string url, string filePath, string fileName, out string fileFullName, ref bool uccorError, out string picMD5)
		{
			return DownLoadPicture(url, filePath, fileName, 3145728, out fileFullName, ref uccorError, out picMD5);
		}

		public static bool DownLoadPicture(string url, string filePath, string fileName, int picSize, out string fileFullName, ref bool uccorError, out string picMD5)
		{
			bool result = true;
			Stream stream = null;
			fileFullName = string.Empty;
			picMD5 = string.Empty;
			try
			{
				byte[] array = null;
				fileFullName = GetNewFileName(url, filePath, fileName, out array);
				if (!string.IsNullOrEmpty(fileFullName) && array != null)
				{
					byte[] array2 = CompressImage(array, fileFullName, picSize);
					picMD5 = MD5Crypt.Encrypt(array2);
					stream = File.Create(fileFullName);
					stream.Write(array2, 0, array2.Length);
					return result;
				}
				return false;
			}
			catch (Exception ex)
			{
				Log.WriteLog(ex);
				if (ex.Message == "磁盘空间不足")
				{
					uccorError = true;
					//XtraMessageBox.Show("下载商品描述图片时，" + ex.Message + ",请清理磁盘空间后，再次导出数据");
				}
				return false;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
					stream.Dispose();
				}
			}
		}

		private static byte[] CompressImage(byte[] data, string fileFullName)
		{
			if (data != null && data.Length >= 3145728)
			{
				Image image = null;
				string text = string.Empty;
				if (!string.IsNullOrEmpty(fileFullName) && fileFullName.LastIndexOf(".") > 0)
				{
					text = fileFullName.Substring(fileFullName.LastIndexOf("."));
				}
				if (string.IsNullOrEmpty(text))
				{
					text = ".jpeg";
				}
				FileInfo fileInfo = new FileInfo(fileFullName);
				string text2 = fileInfo.DirectoryName;
				if (!text2.EndsWith("\\"))
				{
					text2 += "\\";
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				string text3 = text2 + "Compress" + DateTime.Now.Ticks + text;
				while (File.Exists(text3))
				{
					text3 = text2 + "Compress" + DateTime.Now.Ticks + text;
				}
				ImageFormat formatByExtension = GetFormatByExtension(text);
				bool flag = false;
				do
				{
					try
					{
						using (MemoryStream stream = new MemoryStream(data))
						{
							image = Image.FromStream(stream);
							if (image != null)
							{
								double towidth;
								double toheight;
								if (!flag)
								{
									towidth = (double)image.Width;
									toheight = (double)image.Height;
								}
								else if (image.Width > 800)
								{
									towidth = 800.0;
									toheight = (double)image.Height * 800.0 / (double)image.Width;
								}
								else
								{
									towidth = (double)image.Width * 0.9;
									toheight = (double)image.Height * 0.9;
								}
								data = MakeThumbnail(image, text3, towidth, toheight, formatByExtension);
								flag = true;
								goto end_IL_0121;
							}
							return data;
							end_IL_0121:;
						}
					}
					catch (Exception ex)
					{
						Log.WriteLog(ex);
						image.Dispose();
						return data;
					}
					finally
					{
						image?.Dispose();
					}
				}
				while (data.Length >= 3145728);
				return data;
			}
			return data;
		}

		private static byte[] CompressImage(byte[] data, ref string fileFullName)
		{
			return CompressImage(data, fileFullName, 1048576);
		}

		private static byte[] CompressImage(byte[] data, string fileFullName, int size)
		{
			if (data != null && data.Length >= size)
			{
				string text = Path.GetExtension(fileFullName);
				if (string.IsNullOrEmpty(text) || (text.ToLower() != ".jpeg" && text.ToLower() != ".bmp" && text.ToLower() != ".png" && text.ToLower() != ".gif" && text.ToLower() != ".jpg"))
				{
					text = ".jpg";
				}
				ImageCodecInfo encoder = (!(text.ToLower() == ".gif")) ? GetEncoderInfo("image/jpeg") : GetEncoderInfo("image/gif");
				if (text.ToLower() == ".png")
				{
					fileFullName = fileFullName.Replace(".png", ".jpg");
				}
				System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
				EncoderParameters encoderParameters = new EncoderParameters(1);
				double num = 100.0;
				byte[] array = null;
				Image image = null;
				using (MemoryStream stream = new MemoryStream(data))
				{
					image = Image.FromStream(stream);
				}
				Bitmap bitmap = new Bitmap(image.Width, image.Height);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.DrawImage(image, 0, 0, image.Width, image.Height);
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
						break;
					}
					num *= 0.9;
				}
				while (array.Length > size);
				graphics?.Dispose();
				bitmap?.Dispose();
				image?.Dispose();
				return array;
			}
			return data;
		}

		private static byte[] MakeThumbnail(Image originalImage, string newPath, double towidth, double toheight, ImageFormat format)
		{
			ImageCodecInfo encoderInfo = GetEncoderInfo("image/jpeg");
			System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
			EncoderParameters encoderParameters = new EncoderParameters(1);
			EncoderParameter encoderParameter = new EncoderParameter(quality, 100L);
			encoderParameters.Param[0] = encoderParameter;
			byte[] result = null;
			Image image = null;
			Graphics graphics = null;
			try
			{
				int x = 0;
				int y = 0;
				int width = originalImage.Width;
				int height = originalImage.Height;
				double num = toheight / Convert.ToDouble(height);
				double num2 = towidth / Convert.ToDouble(width);
				if (toheight > (double)height && towidth > (double)width)
				{
					toheight = (double)height;
					towidth = (double)width;
				}
				else if (num > num2)
				{
					toheight = num2 * (double)originalImage.Height;
				}
				else
				{
					towidth = num * (double)originalImage.Width;
				}
				image = new Bitmap(Convert.ToInt32(towidth), Convert.ToInt32(toheight));
				graphics = Graphics.FromImage(image);
				graphics.Clear(Color.Transparent);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.High;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(originalImage, new Rectangle(0, 0, Convert.ToInt32(towidth), Convert.ToInt32(toheight)), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
				image.Save(newPath, encoderInfo, encoderParameters);
			}
			catch (Exception ex)
			{
				Log.WriteLog(ex);
			}
			finally
			{
				try
				{
					graphics?.Dispose();
					image?.Dispose();
					originalImage?.Dispose();
				}
				catch (Exception ex2)
				{
					Log.WriteLog(ex2);
				}
			}
			if (File.Exists(newPath))
			{
				result = GetImageToByteArray(newPath);
			}
			return result;
		}

		private static byte[] GetImageToByteArray(string url)
		{
			return LocalFileOperater.GetImageToByteArray(url);
		}

		private static ImageFormat GetFormatByExtension(string extension)
		{
			if (string.IsNullOrEmpty(extension))
			{
				return ImageFormat.Jpeg;
			}
			ImageFormat imageFormat = null;
			extension = extension.ToLower().Trim();
			switch (extension)
			{
			case ".jpg":
				return ImageFormat.Jpeg;
			case ".jpeg":
				return ImageFormat.Jpeg;
			case ".gif":
				return ImageFormat.Gif;
			case ".png":
				return ImageFormat.Png;
			case ".bmp":
				return ImageFormat.Bmp;
			default:
				return ImageFormat.Jpeg;
			}
		}

		private static string GetImageExtension(string src, byte[] fileData)
		{
			string text = string.Empty;
			if (fileData != null && fileData.Length >= 2)
			{
				string imageBytes = fileData[0].ToString() + fileData[1].ToString();
				text = GetImageExtension(imageBytes);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = Path.GetExtension(src);
				if (!string.IsNullOrEmpty(text))
				{
					text = ((!text.StartsWith(".jpg", StringComparison.OrdinalIgnoreCase)) ? ((!text.StartsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) ? ((!text.StartsWith(".png", StringComparison.OrdinalIgnoreCase)) ? ((!text.StartsWith(".bmp", StringComparison.OrdinalIgnoreCase)) ? ((!text.StartsWith(".gif", StringComparison.OrdinalIgnoreCase)) ? ".jpg" : ".gif") : ".bmp") : ".png") : ".jpeg") : ".jpg");
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = ".jpg";
			}
			return text;
		}

		private static string GetImageExtension(string imageBytes)
		{
			switch (imageBytes)
			{
			case "255216":
				return ".jpg";
			case "7173":
				return ".gif";
			case "13780":
				return ".png";
			case "6677":
				return ".bmp";
			default:
				return "";
			}
		}

		private static string GetNewFileName(string src, string filePath, string fileName, out byte[] fileData)
		{
			string empty = string.Empty;
			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}
			if (!filePath.EndsWith("\\"))
			{
				filePath += "\\";
			}
			fileData = DownloadAndChangePic.DownloadPicToBytes(src);
			if (fileData == null)
			{
				return empty;
			}
			if (src.IndexOf("file:///", StringComparison.OrdinalIgnoreCase) >= 0 || src.IndexOf("file:\\\\", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				try
				{
					Uri uri = new Uri(src);
					fileName = uri.LocalPath.Substring(uri.LocalPath.LastIndexOf("\\") + 1, uri.LocalPath.LastIndexOf(".") - uri.LocalPath.LastIndexOf("\\") - 1);
				}
				catch (Exception ex)
				{
					Log.WriteLog(ex);
				}
			}
			string imageExtension = GetImageExtension(src, fileData);
			empty = ((fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0) ? (filePath + fileName + imageExtension) : GetNewFileName(filePath, imageExtension));
			if (File.Exists(fileName))
			{
				empty = GetNewFileName(filePath, imageExtension);
			}
			return empty;
		}

		private static string GetNewFileName(string path, string fileExtension)
		{
			string str = Guid.NewGuid().ToString();
			return path + str + fileExtension;
		}

		private static string GetNewTxt(string content, string oldstrImg, string strImg)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = 0;
			int num2 = 0;
			if (string.IsNullOrEmpty(oldstrImg))
			{
				num = content.IndexOf(strImg, 0);
				text2 = content.Substring(0, num);
			}
			else
			{
				num = content.IndexOf(oldstrImg, 0) + oldstrImg.Length;
				if (!string.IsNullOrEmpty(strImg) && num > 0)
				{
					num2 = content.IndexOf(strImg, num);
				}
				if (num > 0)
				{
					text2 = ((num2 <= num || num2 - num <= 0) ? content.Substring(num) : content.Substring(num, num2 - num));
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				string empty = string.Empty;
				string pattern = "(?is)<(?<CSS>[^<>]*?)>(?<content>[^<>]*?[\\u4E00-\\u9FFF\\w]+?[^<>]*)";
				Regex regex = new Regex(pattern);
				MatchCollection matchCollection = regex.Matches(text2);
				if (matchCollection != null && matchCollection.Count > 0)
				{
					string str = "";
					foreach (Match item in matchCollection)
					{
						string text3 = "";
						if (!item.Groups["CSS"].Value.StartsWith("/"))
						{
							string text5 = "<" + item.Groups["CSS"].Value + ">";
							text3 = ((item.Groups["CSS"].Value.IndexOf(" ") < 0) ? item.Groups["CSS"].Value : item.Groups["CSS"].Value.Substring(0, item.Groups["CSS"].Value.IndexOf(" ")));
							string text6 = "</" + text3 + ">";
						}
						string text4 = item.Groups["content"].Value + "  ";
						string str2 = text;
						string str3 = text4;
						text += str3;
						if (text.ToLower().Contains("#ffffff"))
						{
							text = text.Replace("#ffffff", "#66CCFF").Replace("#FFFFFF", "#66CCFF");
						}
						text = text.Replace("&nbsp;", "").Replace("&mdash;", "");
						if (text.Length >= 499)
						{
							str = str + "<txt>" + str2 + "</txt>";
							text = "";
							text += str3;
						}
					}
					if (text.Replace(" ", "") != string.Empty)
					{
						text = str + "<txt>" + text + "</txt>";
					}
				}
			}
			return text;
		}

		private static string GetNewTxt(string content, string oldstrImg, string strImg, int count)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = 0;
			if (string.IsNullOrEmpty(oldstrImg) && count == 1)
			{
				num = content.IndexOf(strImg, 0);
				text2 = content.Substring(num);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				string empty = string.Empty;
				string pattern = "(?is)<(?<CSS>[^<>]*?)>(?<content>[^<>]*?[\\u4E00-\\u9FFF\\w]+?[^<>]*)";
				Regex regex = new Regex(pattern);
				MatchCollection matchCollection = regex.Matches(text2);
				if (matchCollection != null && matchCollection.Count > 0)
				{
					string str = "";
					foreach (Match item in matchCollection)
					{
						string text3 = "";
						if (!item.Groups["CSS"].Value.StartsWith("/"))
						{
							string text5 = "<" + item.Groups["CSS"].Value + ">";
							text3 = ((item.Groups["CSS"].Value.IndexOf(" ") < 0) ? item.Groups["CSS"].Value : item.Groups["CSS"].Value.Substring(0, item.Groups["CSS"].Value.IndexOf(" ")));
							string text6 = "</" + text3 + ">";
						}
						string text4 = item.Groups["content"].Value + "  ";
						string str2 = text;
						string str3 = text4;
						text += str3;
						if (text.ToLower().Contains("#ffffff"))
						{
							text = text.Replace("#ffffff", "#66CCFF").Replace("#FFFFFF", "#66CCFF");
						}
						text = text.Replace("&nbsp;", "").Replace("&mdash;", "");
						if (text.Length >= 499)
						{
							str = str + "<txt>" + str2 + "</txt>";
							text = "";
							text += str3;
						}
					}
					if (text.Replace(" ", "") != string.Empty)
					{
						text = str + "<txt>" + text + "</txt>";
					}
				}
			}
			return text;
		}

		private static string GetItemComtentTxt(string content, string oldstrImg, string strImg)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = 0;
			int num2 = 0;
			if (string.IsNullOrEmpty(oldstrImg))
			{
				num = content.IndexOf(strImg, 0);
				text2 = content.Substring(0, num);
			}
			else
			{
				num = content.IndexOf(oldstrImg, 0) + oldstrImg.Length;
				if (!string.IsNullOrEmpty(strImg) && num > 0)
				{
					num2 = content.IndexOf(strImg, num);
				}
				if (num > 0)
				{
					text2 = ((num2 <= num || num2 - num <= 0) ? content.Substring(num) : content.Substring(num, num2 - num));
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				string empty3 = string.Empty;
				string pattern = "(?<txtContent>[^<>]*?[\\u4E00-\\u9FFF]+?[^<>]*)";
				Regex regex = new Regex(pattern);
				try
				{
					MatchCollection matchCollection = regex.Matches(text2);
					if (matchCollection != null)
					{
						if (matchCollection.Count > 0)
						{
							string empty = string.Empty;
							{
								foreach (Match item in matchCollection)
								{
									if (item.Groups["txtContent"].Success)
									{
										empty = item.Groups["txtContent"].Value.Replace(" ", "");
										int length = GetLength(empty);
										if (length > 500)
										{
											string text3 = string.Empty;
											string empty2 = string.Empty;
											for (int i = 0; i < length / 500; i++)
											{
												if (string.IsNullOrEmpty(text3))
												{
													empty2 = CutStringByte(empty, 0, 499);
													text3 = empty.Replace(empty2, "");
												}
												else
												{
													empty2 = CutStringByte(text3, 0, 499);
													text3 = text3.Replace(empty2, "");
												}
												empty = empty + "<txt>" + empty2 + "</txt>";
											}
											empty = empty + "<txt>" + text3 + "</txt>";
										}
										text = text + "<txt>" + empty + "</txt>";
									}
								}
								return text;
							}
						}
						return text;
					}
					return text;
				}
				catch (Exception arg)
				{
					Log.WriteLog("生成手机详情文字出现异常！" + arg);
					return text;
				}
			}
			return text;
		}

		private static int GetLength(string str)
		{
			if (str.Length == 0)
			{
				return 0;
			}
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			int num = 0;
			byte[] bytes = aSCIIEncoding.GetBytes(str);
			for (int i = 0; i < bytes.Length; i++)
			{
				num = ((bytes[i] != 63) ? (num + 1) : (num + 2));
			}
			return num;
		}

		public static string CutStringByte(string str, int startIndex, int len)
		{
			if (str != null && !(str.Trim() == ""))
			{
				if (Encoding.Default.GetByteCount(str) < startIndex + 1 + len)
				{
					return str;
				}
				int num = 0;
				int num2 = 0;
				string text = str;
				foreach (char c in text)
				{
					num = ((c <= '\u007f') ? (num + 1) : (num + 2));
					if (num > startIndex + len)
					{
						str = str.Substring(startIndex, num2);
						break;
					}
					if (num > startIndex)
					{
						num2++;
					}
				}
				return str;
			}
			return "";
		}
	}
}
