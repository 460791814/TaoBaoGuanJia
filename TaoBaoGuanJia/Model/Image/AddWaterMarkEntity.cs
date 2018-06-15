using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoGuanJia.Model
{
    public class AddWaterMarkEntity
    {
        public string aexeType
        {
            get;
            set;
        }
        public string awaterText
        {
            get;
            set;
        }
        public System.Drawing.Font atextFont
        {
            get;
            set;
        }
        public System.Drawing.Color atextColor
        {
            get;
            set;
        }
        public float atransparence
        {
            get;
            set;
        }
        public System.Drawing.Image awaterImg
        {
            get;
            set;
        }
        public MarkLocation amarkLocation
        {
            get;
            set;
        }
        public CompressSize compressSize
        {
            get;
            set;
        }
        public bool isNotAddWaterToo
        {
            get;
            set;
        }
    }
    public enum MarkLocation
    {
        LeftUp = 1,
        LeftDown,
        RightUp,
        RightDown,
        Middle
    }
    public enum CompressSize
    {
        None,
        Compress4 = 4,
        Compress9 = 9,
        Compress16 = 16,
        Compress25 = 25,
        Compress36 = 36
    }
}
