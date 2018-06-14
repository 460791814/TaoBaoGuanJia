using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TaoBaoGuanJia.Util
{
    public static class ImageCompress
    {
        public static bool ChangeImage(string sourceFileFullName, long maxLength, string targetFileFullName)
        {
            Stream stream = null;
            bool result = true;
            try
            {
                bool flag = false;
                byte[] array = ImageCompress.CompressImage(sourceFileFullName, maxLength, out flag);
                if (flag || !(sourceFileFullName == targetFileFullName))
                {
                    stream = File.Create(targetFileFullName);
                    stream.Write(array, 0, array.Length);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex);
                result = false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            return result;
        }
        public static bool ChangeImageNotChange(string sourceFileFullName, long maxLength, string targetFileFullName)
        {
            Stream stream = null;
            bool result = true;
            try
            {
                bool flag = false;
                byte[] array = ImageCompress.CompressImageNotChange(sourceFileFullName, maxLength, out flag);
                if (flag || !(sourceFileFullName == targetFileFullName))
                {
                    stream = File.Create(targetFileFullName);
                    stream.Write(array, 0, array.Length);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex);
                result = false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            return result;
        }
        public static byte[] CompressImageByData(byte[] data, long maxLength, string sourceUrl)
        {
            System.Drawing.Image image = null;
            string value = string.Empty;
            if (!string.IsNullOrEmpty(sourceUrl) && sourceUrl.LastIndexOf(".") > 0)
            {
                value = sourceUrl.Substring(sourceUrl.LastIndexOf("."));
            }
            if (string.IsNullOrEmpty(value))
            {
                value = ".jpeg";
            }
            System.Drawing.Imaging.ImageFormat formatByExtension = ImageCompress.GetFormatByExtension(ref value);
            bool flag = false;
            do
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(data))
                    {
                        image = System.Drawing.Image.FromStream(memoryStream);
                        if (image == null)
                        {
                            break;
                        }
                        double towidth;
                        double toheight;
                        if (!flag)
                        {
                            towidth = (double)image.Width;
                            toheight = (double)image.Height;
                        }
                        else
                        {
                            if (image.Width > 800)
                            {
                                towidth = 800.0;
                                toheight = (double)image.Height * 800.0 / (double)image.Width;
                            }
                            else
                            {
                                towidth = (double)image.Width * 0.9;
                                toheight = (double)image.Height * 0.9;
                            }
                        }
                        data = ImageCompress.MakeThumbnail(image, string.Empty, towidth, toheight, formatByExtension);
                        flag = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex);
                    image.Dispose();
                    break;
                }
                finally
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }
                }
            }
            while ((long)data.Length >= maxLength);
            return data;
        }
        private static byte[] CompressImageNotChange(byte[] data, long maxLength, ref string fileFullName)
        {
            if (data == null || (long)data.Length < maxLength)
            {
                return data;
            }
            string text = Path.GetExtension(fileFullName);
            if (string.IsNullOrEmpty(text) || (text.ToLower() != ".jpeg" && text.ToLower() != ".bmp" && text.ToLower() != ".png" && text.ToLower() != ".gif" && text.ToLower() != ".jpg"))
            {
                text = ".jpg";
            }
            System.Drawing.Imaging.ImageCodecInfo encoderInfo;
            if (text.ToLower() == ".gif")
            {
                encoderInfo = ImageCompress.GetEncoderInfo("image/gif");
            }
            else
            {
                encoderInfo = ImageCompress.GetEncoderInfo("image/jpeg");
            }
            if (text.ToLower() == ".png")
            {
                fileFullName = fileFullName.Replace(".png", ".jpg");
            }
            System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
            System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            double num = 100.0;
            byte[] array = null;
            System.Drawing.Image image = null;
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                image = System.Drawing.Image.FromStream(memoryStream);
            }
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image.Width, image.Height);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            do
            {
                System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(quality, (long)num);
                encoderParameters.Param[0] = encoderParameter;
                using (MemoryStream memoryStream2 = new MemoryStream())
                {
                    try
                    {
                        bitmap.Save(memoryStream2, encoderInfo, encoderParameters);
                        array = memoryStream2.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog(ex);
                    }
                    memoryStream2.Close();
                }
                if (num < 8.0)
                {
                    break;
                }
                num *= 0.9;
            }
            while ((long)array.Length > maxLength);
            if (graphics != null)
            {
                graphics.Dispose();
            }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            if (image != null)
            {
                image.Dispose();
            }
            return array;
        }
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            System.Drawing.Imaging.ImageCodecInfo[] imageEncoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < imageEncoders.Length; i++)
            {
                if (imageEncoders[i].MimeType == mimeType)
                {
                    return imageEncoders[i];
                }
            }
            return null;
        }
        public static byte[] CompressImage(string sourceFileFullName, long maxLength)
        {
            bool flag = false;
            return ImageCompress.CompressImage(sourceFileFullName, maxLength, out flag);
        }
        public static byte[] CompressImageNotChange(string sourceFileFullName, long maxLength, out bool hasCompress)
        {
            hasCompress = false;
            byte[] imageToByteArrayByFile = ImageCompress.GetImageToByteArrayByFile(sourceFileFullName);
            if (imageToByteArrayByFile == null || (long)imageToByteArrayByFile.Length <= maxLength)
            {
                return imageToByteArrayByFile;
            }
            hasCompress = true;
            return ImageCompress.CompressImageNotChange(imageToByteArrayByFile, maxLength, ref sourceFileFullName);
        }
        public static byte[] CompressImage(string sourceFileFullName, long maxLength, out bool hasCompress)
        {
            hasCompress = false;
            byte[] array = ImageCompress.GetImageToByteArrayByFile(sourceFileFullName);
            if (array == null || (long)array.Length <= maxLength)
            {
                return array;
            }
            System.Drawing.Image image = null;
            string text = string.Empty;
            if (!string.IsNullOrEmpty(sourceFileFullName) && sourceFileFullName.LastIndexOf(".") > 0)
            {
                text = sourceFileFullName.Substring(sourceFileFullName.LastIndexOf("."));
            }
            if (string.IsNullOrEmpty(text))
            {
                text = ".jpeg";
            }
            FileInfo fileInfo = new FileInfo(sourceFileFullName);
            string text2 = fileInfo.DirectoryName;
            if (!text2.EndsWith("\\"))
            {
                text2 += "\\";
            }
            if (!Directory.Exists(text2))
            {
                Directory.CreateDirectory(text2);
            }
            System.Drawing.Imaging.ImageFormat formatByExtension = ImageCompress.GetFormatByExtension(ref text);
            string text3 = string.Concat(new object[]
            {
                text2,
                "Compress",
                DateTime.Now.Ticks,
                text
            });
            while (File.Exists(text3))
            {
                text3 = string.Concat(new object[]
                {
                    text2,
                    "Compress",
                    DateTime.Now.Ticks,
                    text
                });
            }
            bool flag = false;
            do
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(array))
                    {
                        image = System.Drawing.Image.FromStream(memoryStream);
                        if (image == null)
                        {
                            break;
                        }
                        double towidth;
                        double toheight;
                        if (!flag)
                        {
                            towidth = (double)image.Width;
                            toheight = (double)image.Height;
                        }
                        else
                        {
                            if (image.Width > 800)
                            {
                                towidth = 800.0;
                                toheight = (double)image.Height * 800.0 / (double)image.Width;
                            }
                            else
                            {
                                towidth = (double)image.Width * 0.9;
                                toheight = (double)image.Height * 0.9;
                            }
                        }
                        array = ImageCompress.MakeThumbnail(image, text3, towidth, toheight, formatByExtension);
                        flag = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex);
                    image.Dispose();
                    break;
                }
                finally
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }
                }
            }
            while ((long)array.Length >= maxLength);
            hasCompress = true;
            return array;
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
        private static byte[] GetImageToByteArrayByFile(string fileName)
        {
            FileStream fileStream = null;
            byte[] array = null;
            try
            {
                ImageCompress.SetFileAttribute(fileName);
                fileStream = new FileStream(fileName, FileMode.Open);
                int num = (int)fileStream.Length;
                array = new byte[num];
                fileStream.Read(array, 0, num);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex);
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
        private static byte[] MakeThumbnail(System.Drawing.Image originalImage, string newPath, double towidth, double toheight, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] array = null;
            System.Drawing.Image image = null;
            System.Drawing.Graphics graphics = null;
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
                else
                {
                    if (num > num2)
                    {
                        toheight = num2 * (double)originalImage.Height;
                    }
                    else
                    {
                        towidth = num * (double)originalImage.Width;
                    }
                }
                image = new System.Drawing.Bitmap(Convert.ToInt32(towidth), Convert.ToInt32(toheight));
                graphics = System.Drawing.Graphics.FromImage(image);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.Clear(System.Drawing.Color.Transparent);
                graphics.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, Convert.ToInt32(towidth), Convert.ToInt32(toheight)), new System.Drawing.Rectangle(x, y, width, height), System.Drawing.GraphicsUnit.Pixel);
                if (!string.IsNullOrEmpty(newPath))
                {
                    image.Save(newPath, format);
                }
                else
                {
                    MemoryStream memoryStream = new MemoryStream();
                    image.Save(memoryStream, format);
                    array = memoryStream.GetBuffer();
                    memoryStream.Close();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex);
            }
            finally
            {
                try
                {
                    graphics.Dispose();
                    image.Dispose();
                    originalImage.Dispose();
                }
                catch (Exception ex2)
                {
                    Log.WriteLog(ex2);
                }
            }
            byte[] result;
            if (!string.IsNullOrEmpty(newPath) && File.Exists(newPath))
            {
                result = ImageCompress.GetImageToByteArray(newPath);
            }
            else
            {
                result = array;
            }
            return result;
        }
        private static byte[] GetImageToByteArray(string url)
        {
            FileStream fileStream = null;
            byte[] array = null;
            try
            {
                fileStream = new FileStream(url, FileMode.Open);
                int num = (int)fileStream.Length;
                array = new byte[num];
                fileStream.Read(array, 0, num);
            }
            catch (Exception ex)
            {
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
        private static System.Drawing.Imaging.ImageFormat GetFormatByExtension(ref string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            extension = extension.ToLower().Trim();
            string a;
            System.Drawing.Imaging.ImageFormat result;
            if ((a = extension) != null)
            {
                if (a == ".jpg")
                {
                    result = System.Drawing.Imaging.ImageFormat.Jpeg;
                    return result;
                }
                if (a == ".jpeg")
                {
                    result = System.Drawing.Imaging.ImageFormat.Jpeg;
                    return result;
                }
                if (a == ".gif")
                {
                    result = System.Drawing.Imaging.ImageFormat.Gif;
                    return result;
                }
                if (a == ".png")
                {
                    result = System.Drawing.Imaging.ImageFormat.Png;
                    return result;
                }
                if (a == ".bmp")
                {
                    result = System.Drawing.Imaging.ImageFormat.Bmp;
                    return result;
                }
            }
            extension = ".jpg";
            result = System.Drawing.Imaging.ImageFormat.Jpeg;
            return result;
        }
        public static bool RevPic(string imgFile)
        {
            bool result = false;
            if (string.IsNullOrEmpty(imgFile))
            {
                return result;
            }
            if (!File.Exists(imgFile))
            {
                return result;
            }
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(imgFile);
            int width = bitmap.Width;
            int height = bitmap.Height;
            long imageSize = ImageCompress.GetImageSize(imgFile);
            using (System.Drawing.Bitmap bitmap2 = new System.Drawing.Bitmap(width, height))
            {
                try
                {
                    for (int i = height - 1; i >= 0; i--)
                    {
                        int j = width - 1;
                        int num = 0;
                        while (j >= 0)
                        {
                            System.Drawing.Color pixel = bitmap.GetPixel(j, i);
                            bitmap2.SetPixel(num++, i, System.Drawing.Color.FromArgb((int)pixel.R, (int)pixel.G, (int)pixel.B));
                            j--;
                        }
                    }
                    bitmap.Dispose();
                    bitmap2.Save(imgFile);
                    result = ImageCompress.ChangeImageNotChange(imgFile, imageSize, imgFile);
                }
                catch (Exception arg)
                {
                    Log.WriteLog("主图翻转错误，" + arg);
                }
            }
            return result;
        }
        private static long GetImageSize(string imagePath)
        {
            string text = string.Empty;
            if (File.Exists(imagePath))
            {
                text = imagePath;
            }
            long result = 0L;
            if (!string.IsNullOrEmpty(text) && File.Exists(text))
            {
                byte[] array = File.ReadAllBytes(text);
                result = (long)array.Length;
            }
            return result;
        }
    }
}
