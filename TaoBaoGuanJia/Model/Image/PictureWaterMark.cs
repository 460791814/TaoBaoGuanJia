using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
    public class PictureWaterMark
    {
        private string _text = string.Empty;
        private string _waterImgPath = string.Empty;
        private int _markX;
        private int _markY;
        private float _transparence = 1f;
        private string _fontFamilyStr = "宋体";
        private int _fontSize = 24;
        private System.Drawing.Color _textColor = System.Drawing.Color.Black;
        private bool _bold;
        private int[] sizes = new int[]
        {
            64,
            48,
            32,
            16,
            8,
            4
        };
        private System.Drawing.Image _sourceImage;
        private System.Drawing.Image _waterImage;
        private System.Drawing.Image _markedImage;
        private MarkLocation _markLocation = MarkLocation.LeftUp;
        private MarkType _markType;
        private System.Drawing.Font _textFont;
        private CompressSize _compressSize;
        private static object _lockObj = new object();
        public MarkLocation MarkLocation
        {
            get
            {
                return this._markLocation;
            }
            set
            {
                this._markLocation = value;
            }
        }
        public CompressSize CompressSize
        {
            get
            {
                return this._compressSize;
            }
            set
            {
                this._compressSize = value;
            }
        }
        public System.Drawing.Font TextFont
        {
            get
            {
                return this._textFont;
            }
            set
            {
                this._textFont = value;
            }
        }
        public MarkType MarkType
        {
            get
            {
                return this._markType;
            }
            set
            {
                this._markType = value;
            }
        }
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
        public string WaterImgPath
        {
            get
            {
                return this._waterImgPath;
            }
            set
            {
                this._waterImgPath = value;
            }
        }
        public System.Drawing.Image WaterImage
        {
            get
            {
                return this._waterImage;
            }
            set
            {
                this._waterImage = value;
            }
        }
        public int FontSize
        {
            get
            {
                return this._fontSize;
            }
            set
            {
                this._fontSize = value;
            }
        }
        public int MarkX
        {
            get
            {
                return this._markX;
            }
            set
            {
                this._markX = value;
            }
        }
        public int MarkY
        {
            get
            {
                return this._markY;
            }
            set
            {
                this._markY = value;
            }
        }
        public float Transparence
        {
            get
            {
                if (this._transparence > 1f)
                {
                    this._transparence = 1f;
                }
                return this._transparence;
            }
            set
            {
                this._transparence = value;
            }
        }
        public System.Drawing.Color TextColor
        {
            get
            {
                return this._textColor;
            }
            set
            {
                this._textColor = value;
            }
        }
        public string FontFamilyStr
        {
            get
            {
                return this._fontFamilyStr;
            }
            set
            {
                this._fontFamilyStr = value;
            }
        }
        public System.Drawing.FontFamily FontFamily
        {
            get
            {
                return new System.Drawing.FontFamily(this.FontFamilyStr);
            }
        }
        public bool Bold
        {
            get
            {
                return this._bold;
            }
            set
            {
                this._bold = value;
            }
        }
        public System.Drawing.Image SourceImage
        {
            get
            {
                return this._sourceImage;
            }
            set
            {
                this._sourceImage = value;
            }
        }
        public System.Drawing.Image MarkedImage
        {
            get
            {
                return this._markedImage;
            }
        }
        public PictureWaterMark()
        {
        }
        public PictureWaterMark(System.Drawing.Image sourceImg, string text, string fontFamily, int fontSize, bool bold, System.Drawing.Color color, float tranparence, int markX, int markY)
        {
            this._sourceImage = sourceImg;
            this._transparence = tranparence;
            this._markType = MarkType.Text;
            this._text = text;
            this._fontFamilyStr = fontFamily;
            this._fontSize = fontSize;
            this._bold = bold;
            this._textColor = color;
            this._markX = markX;
            this._markY = markY;
        }
        public PictureWaterMark(System.Drawing.Image sourceImg, string text, System.Drawing.Font textFont, System.Drawing.Color color, float tranparence, MarkLocation markLocation, CompressSize compressSize)
        {
            this._textFont = textFont;
            this._sourceImage = sourceImg;
            this._transparence = tranparence;
            this._markType = MarkType.Text;
            this._text = text;
            this._textColor = color;
            this._markLocation = markLocation;
            if (compressSize != CompressSize.None)
            {
                this._compressSize = compressSize;
            }
        }
        public PictureWaterMark(System.Drawing.Image sourceImg, System.Drawing.Image waterImg, float tranparence, int markX, int markY)
        {
            this._sourceImage = sourceImg;
            this._markType = MarkType.Image;
            this._waterImage = waterImg;
            this._markX = markX;
            this._markY = markY;
            this._transparence = tranparence;
        }
        public PictureWaterMark(System.Drawing.Image sourceImg, System.Drawing.Image waterImg, float tranparence, MarkLocation markLocation, CompressSize compressSize)
        {
            this._sourceImage = sourceImg;
            this._markType = MarkType.Image;
            this._waterImage = waterImg;
            this._transparence = tranparence;
            this._markLocation = markLocation;
            if (compressSize != CompressSize.None)
            {
                this._compressSize = compressSize;
            }
        }
        private System.Drawing.Image ConvertBmpToNewImg(System.Drawing.Bitmap newBitmap, System.Drawing.Imaging.ImageCodecInfo myImageCodecInfo, System.Drawing.Imaging.EncoderParameters myEncoderParameters)
        {
            if (newBitmap == null)
            {
                return null;
            }
            byte[] array = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    newBitmap.Save(memoryStream, myImageCodecInfo, myEncoderParameters);
                    array = memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    Log.WriteLog("加水印时，读取加水印后的图片字节出错", ex);
                }
                memoryStream.Close();
            }
            if (array == null)
            {
                return null;
            }
            return ImageUtil.ConvertBytesToImg(array);
        }
        public void SaveBmpWithImageCodecInfo(System.Drawing.Image img, string photoPath)
        {
            System.Drawing.Imaging.ImageCodecInfo encoderInfo = PictureWaterMark.GetEncoderInfo("image/jpeg");
            System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
            System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(quality, 100L);
            encoderParameters.Param[0] = encoderParameter;
            if (img == null)
            {
                return;
            }
            System.Drawing.Bitmap bitmap = null;
            try
            {
                bitmap = new System.Drawing.Bitmap(img);
                bitmap.Save(photoPath, encoderInfo, encoderParameters);
            }
            catch (Exception ex)
            {
                Log.WriteLog("SaveBmpWithImageCodecInfo出错", ex);
            }
            finally
            {
                bitmap.Dispose();
            }
        }
        public System.Drawing.Image Mark(System.Drawing.Image img, MarkType markType, string text, System.Drawing.Image waterImg, int markx, int marky, bool bold, System.Drawing.Color textColor, float transparence, System.Drawing.FontFamily fontFamily, int fontSize, CompressSize compressSize)
        {
            Guid[] frameDimensionsList = img.FrameDimensionsList;
            System.Drawing.Image result;
            for (int i = 0; i < frameDimensionsList.Length; i++)
            {
                Guid guid = frameDimensionsList[i];
                System.Drawing.Imaging.FrameDimension dimension = new System.Drawing.Imaging.FrameDimension(guid);
                if (img.GetFrameCount(dimension) > 1)
                {
                    result = img;
                    return result;
                }
            }
            try
            {
                System.Drawing.Imaging.ImageCodecInfo encoderInfo = PictureWaterMark.GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
                System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(quality, 100L);
                encoderParameters.Param[0] = encoderParameter;
                if (markType == MarkType.Text)
                {
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(img, new System.Drawing.Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, System.Drawing.GraphicsUnit.Pixel);
                    System.Drawing.Font textFont = this.TextFont;
                    System.Drawing.SizeF sizeF = default(System.Drawing.SizeF);
                    float num = this.TextFont.Size - 2f;
                    if ((double)num < 0.0001)
                    {
                    }
                    sizeF = graphics.MeasureString(this.Text, textFont);
                    while ((ushort)sizeF.Width > (ushort)img.Width)
                    {
                        num = this.TextFont.Size - 2f;
                        if ((double)num < 0.0001)
                        {
                            break;
                        }
                        this.TextFont = new System.Drawing.Font(this.TextFont.FontFamily, num, this.TextFont.Style);
                        sizeF = graphics.MeasureString(this.Text, this.TextFont);
                        if ((ushort)sizeF.Width < (ushort)this.SourceImage.Width)
                        {
                            break;
                        }
                    }
                    int alpha = Convert.ToInt32(255f * transparence);
                    System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(alpha, textColor));
                    graphics.DrawString(text, textFont, brush, (float)markx, (float)marky);
                    graphics.Dispose();
                    img = this.ConvertBmpToNewImg(bitmap, encoderInfo, encoderParameters);
                    bitmap.Dispose();
                    result = img;
                }
                else
                {
                    if (markType == MarkType.Image)
                    {
                        float[][] array = new float[5][];
                        float[][] arg_236_0 = array;
                        int arg_236_1 = 0;
                        float[] array2 = new float[5];
                        array2[0] = 1f;
                        arg_236_0[arg_236_1] = array2;
                        float[][] arg_24D_0 = array;
                        int arg_24D_1 = 1;
                        float[] array3 = new float[5];
                        array3[1] = 1f;
                        arg_24D_0[arg_24D_1] = array3;
                        float[][] arg_264_0 = array;
                        int arg_264_1 = 2;
                        float[] array4 = new float[5];
                        array4[2] = 1f;
                        arg_264_0[arg_264_1] = array4;
                        float[][] arg_278_0 = array;
                        int arg_278_1 = 3;
                        float[] array5 = new float[5];
                        array5[3] = transparence;
                        arg_278_0[arg_278_1] = array5;
                        array[4] = new float[]
                        {
                            0f,
                            0f,
                            0f,
                            0f,
                            1f
                        };
                        float[][] newColorMatrix = array;
                        System.Drawing.Imaging.ColorMatrix newColorMatrix2 = new System.Drawing.Imaging.ColorMatrix(newColorMatrix);
                        System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
                        imageAttributes.SetColorMatrix(newColorMatrix2, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Default);
                        System.Drawing.Bitmap bitmap2 = new System.Drawing.Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        bitmap2.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                        System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage(bitmap2);
                        graphics2.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics2.DrawImage(img, new System.Drawing.Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, System.Drawing.GraphicsUnit.Pixel);
                        if (waterImg.Width > img.Width || waterImg.Height > img.Height)
                        {
                            System.Drawing.Image.GetThumbnailImageAbort callback = null;
                            System.Drawing.Image image = null;
                            if (this.CompressSize == CompressSize.None)
                            {
                                image = this.WaterImage.GetThumbnailImage(this.SourceImage.Width, this.SourceImage.Height, callback, new IntPtr(0));
                            }
                            else
                            {
                                if (this.CompressSize == CompressSize.Compress4)
                                {
                                    image = this.WaterImage.GetThumbnailImage(this.SourceImage.Width / 2, this.SourceImage.Height / 2, callback, new IntPtr(0));
                                }
                                else
                                {
                                    if (this.CompressSize == CompressSize.Compress9)
                                    {
                                        image = this.WaterImage.GetThumbnailImage(this.SourceImage.Width / 3, this.SourceImage.Height / 3, callback, new IntPtr(0));
                                    }
                                    else
                                    {
                                        if (this.CompressSize == CompressSize.Compress16)
                                        {
                                            image = this.WaterImage.GetThumbnailImage(this.SourceImage.Width / 4, this.SourceImage.Height / 4, callback, new IntPtr(0));
                                        }
                                    }
                                }
                            }
                            if (this.CompressSize == CompressSize.Compress25)
                            {
                                image = this.WaterImage.GetThumbnailImage(this.SourceImage.Width / 5, this.SourceImage.Height / 5, callback, new IntPtr(0));
                            }
                            if (this.CompressSize == CompressSize.Compress36)
                            {
                                image = this.WaterImage.GetThumbnailImage(this.SourceImage.Width / 6, this.SourceImage.Height / 6, callback, new IntPtr(0));
                            }
                            graphics2.DrawImage(image, new System.Drawing.Rectangle(markx, marky, image.Width, image.Height), 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel, imageAttributes);
                            image.Dispose();
                            graphics2.Dispose();
                            img = this.ConvertBmpToNewImg(bitmap2, encoderInfo, encoderParameters);
                            bitmap2.Dispose();
                            result = img;
                        }
                        else
                        {
                            graphics2.DrawImage(waterImg, new System.Drawing.Rectangle(markx, marky, waterImg.Width, waterImg.Height), 0, 0, waterImg.Width, waterImg.Height, System.Drawing.GraphicsUnit.Pixel, imageAttributes);
                            graphics2.Dispose();
                            img = this.ConvertBmpToNewImg(bitmap2, encoderInfo, encoderParameters);
                            bitmap2.Dispose();
                            result = img;
                        }
                    }
                    else
                    {
                        result = img;
                    }
                }
            }
            catch
            {
                result = img;
            }
            return result;
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
        public System.Drawing.Image MarkByXy()
        {
            return this._markedImage = this.Mark(this.SourceImage, this.MarkType, this.Text, this.WaterImage, this.MarkX, this.MarkY, this.Bold, this.TextColor, this.Transparence, this.FontFamily, this.FontSize, this.CompressSize);
        }
        public System.Drawing.Image MarkByLocation()
        {
            System.Drawing.Image result;
            lock (PictureWaterMark._lockObj)
            {
                result = this.MarkByLocationInternal();
            }
            return result;
        }
        private System.Drawing.Image MarkByLocationInternal()
        {
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(this.SourceImage);
            if (this.MarkType == MarkType.Text)
            {
                System.Drawing.Font textFont = this.TextFont;
                System.Drawing.SizeF sizeF = default(System.Drawing.SizeF);
                float num = this.TextFont.Size - 2f;
                if ((double)num < 0.0001)
                {
                }
                sizeF = graphics.MeasureString(this.Text, textFont);
                while ((ushort)sizeF.Width > (ushort)this.SourceImage.Width)
                {
                    num = this.TextFont.Size - 2f;
                    if ((double)num < 0.0001)
                    {
                        break;
                    }
                    this.TextFont = new System.Drawing.Font(this.TextFont.FontFamily, num, this.TextFont.Style);
                    sizeF = graphics.MeasureString(this.Text, this.TextFont);
                    if ((ushort)sizeF.Width < (ushort)this.SourceImage.Width)
                    {
                        break;
                    }
                }
                graphics.Dispose();
                switch (this.MarkLocation)
                {
                    case MarkLocation.LeftUp:
                        this._markX = 0;
                        this._markY = 0;
                        break;
                    case MarkLocation.LeftDown:
                        this._markX = 0;
                        this._markY = this.SourceImage.Height - Convert.ToInt32(sizeF.Height);
                        break;
                    case MarkLocation.RightUp:
                        this._markX = this.SourceImage.Width - Convert.ToInt32(sizeF.Width);
                        this._markY = 0;
                        break;
                    case MarkLocation.RightDown:
                        this._markX = this.SourceImage.Width - Convert.ToInt32(sizeF.Width);
                        this._markY = this.SourceImage.Height - Convert.ToInt32(sizeF.Height);
                        break;
                    case MarkLocation.Middle:
                        this._markX = this.SourceImage.Width / 2 - Convert.ToInt32(sizeF.Width / 2f);
                        this._markY = this.SourceImage.Height / 2 - Convert.ToInt32(sizeF.Height / 2f);
                        break;
                }
            }
            else
            {
                if (this.MarkType == MarkType.Image)
                {
                    System.Drawing.Image image = null;
                    if (this.SourceImage.Width <= 4 || this.SourceImage.Height <= 4)
                    {
                        if (image != null)
                        {
                            image.Dispose();
                        }
                        if (graphics != null)
                        {
                            graphics.Dispose();
                        }
                        return this.SourceImage;
                    }
                    double num2 = (double)this.WaterImage.Width;
                    double num3 = (double)this.WaterImage.Height;
                    this.GetNewSize(this.SourceImage, this.WaterImage, this.CompressSize, out num2, out num3);
                    image = this.GetNewImage(this.WaterImage, DataConvert.ToInt(num2), DataConvert.ToInt(num3));
                    this._waterImage = image;
                    if (this._waterImage != null)
                    {
                        switch (this.MarkLocation)
                        {
                            case MarkLocation.LeftUp:
                                this._markX = 0;
                                this._markY = 0;
                                break;
                            case MarkLocation.LeftDown:
                                this._markX = 0;
                                this._markY = this.SourceImage.Height - Convert.ToInt32(this._waterImage.Height);
                                break;
                            case MarkLocation.RightUp:
                                this._markX = this.SourceImage.Width - Convert.ToInt32(this._waterImage.Width);
                                this._markY = 0;
                                break;
                            case MarkLocation.RightDown:
                                this._markX = this.SourceImage.Width - Convert.ToInt32(this._waterImage.Width);
                                this._markY = this.SourceImage.Height - Convert.ToInt32(this._waterImage.Height);
                                break;
                            case MarkLocation.Middle:
                                this._markX = this.SourceImage.Width / 2 - Convert.ToInt32(this._waterImage.Width / 2);
                                this._markY = this.SourceImage.Height / 2 - Convert.ToInt32(this._waterImage.Height / 2);
                                break;
                        }
                    }
                    if (this._markX < 0)
                    {
                        this._markX = 0;
                    }
                    if (this._markY < 0)
                    {
                        this._markY = 0;
                    }
                }
            }
            System.Drawing.Image result = this.MarkByXy();
            if (graphics != null)
            {
                graphics.Dispose();
            }
            return result;
        }
        private System.Drawing.Image GetNewImage(System.Drawing.Image iSource, int dWidth, int dHeight)
        {
            System.Drawing.Imaging.ImageFormat arg_06_0 = iSource.RawFormat;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(dWidth, dHeight);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.Clear(System.Drawing.Color.Transparent);
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(iSource, new System.Drawing.Rectangle(0, 0, dWidth, dHeight), 0, 0, iSource.Width, iSource.Height, System.Drawing.GraphicsUnit.Pixel);
            graphics.Dispose();
            return bitmap;
        }
        private void GetNewSize(System.Drawing.Image sourceImg, System.Drawing.Image waterImg, CompressSize compressSize, out double width, out double height)
        {
            double num = (double)(sourceImg.Height * sourceImg.Width);
            double num2 = DataConvert.ToDouble(waterImg.Height) / DataConvert.ToDouble(waterImg.Width);
            int num3;
            if (compressSize <= CompressSize.Compress9)
            {
                if (compressSize == CompressSize.None)
                {
                    num3 = 1;
                    goto IL_78;
                }
                if (compressSize == CompressSize.Compress4)
                {
                    num3 = 4;
                    goto IL_78;
                }
                if (compressSize == CompressSize.Compress9)
                {
                    num3 = 9;
                    goto IL_78;
                }
            }
            else
            {
                if (compressSize == CompressSize.Compress16)
                {
                    num3 = 16;
                    goto IL_78;
                }
                if (compressSize == CompressSize.Compress25)
                {
                    num3 = 25;
                    goto IL_78;
                }
                if (compressSize == CompressSize.Compress36)
                {
                    num3 = 36;
                    goto IL_78;
                }
            }
            num3 = 1;
            IL_78:
            if (compressSize != CompressSize.None)
            {
                width = Math.Sqrt(num / (double)num3 / num2);
                height = width * num2;
            }
            else
            {
                width = (double)waterImg.Width;
                height = (double)waterImg.Height;
            }
            if (width > (double)sourceImg.Width && height > (double)sourceImg.Height)
            {
                width = (double)sourceImg.Width;
                height = (double)sourceImg.Height;
                return;
            }
            if (width > (double)sourceImg.Width)
            {
                width = (double)sourceImg.Width;
                height = width * num2;
                return;
            }
            if (height > (double)sourceImg.Height)
            {
                height = (double)sourceImg.Height;
                width = height / num2;
            }
        }
    }
}
