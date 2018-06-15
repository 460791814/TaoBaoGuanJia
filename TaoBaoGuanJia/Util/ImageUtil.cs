using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace TaoBaoGuanJia.Util
{
    public static class ImageUtil
    {
        public static byte[] ConvertImgToBytes(Image img)
        {
            if (img == null)
            {
                return null;
            }
            byte[] result = null;
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                img.Save(memoryStream, img.RawFormat);
                result = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteLog("将图片转换为字节数组时出现异常", ex);
            }
            finally
            {
                memoryStream.Close();
                memoryStream.Dispose();
            }
            return result;
        }
        public static Image ConvertBytesToImg(byte[] datas)
        {
            if (datas == null || datas.Length <= 0)
            {
                return null;
            }
            Image result = null;
            MemoryStream memoryStream = new MemoryStream(datas);
            try
            {
                result = Image.FromStream(memoryStream, true);
            }
            catch (Exception ex)
            {
                Log.WriteLog("将字节数组转换为图片时出现异常", ex);
            }
            finally
            {
                memoryStream.Close();
                memoryStream.Dispose();
            }
            return result;
        }
    }
}
