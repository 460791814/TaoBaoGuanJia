using System;
using System.Text;

namespace TaoBaoGuanJia.Util
{
	public class MD5Crypt
	{
		private const int BITS_TO_A_BYTE = 8;

		private const int BYTES_TO_A_WORD = 4;

		private const int BITS_TO_A_WORD = 32;

		private static long[] m_lOnBits = new long[31];

		private static long[] m_l2Power = new long[31];

		private static long LShift(long lValue, long iShiftBits)
		{
			long num = 0L;
			if (iShiftBits == 0)
			{
				return lValue;
			}
			if (iShiftBits == 31)
			{
				if (Convert.ToBoolean(lValue & 1))
				{
					return 2147483648L;
				}
				return 0L;
			}
			if (iShiftBits < 0)
			{
				;
			}
			if (Convert.ToBoolean(lValue & m_l2Power[31 - iShiftBits]))
			{
				return (lValue & m_lOnBits[31 - (iShiftBits + 1)]) * m_l2Power[iShiftBits] | 2147483648u;
			}
			return (lValue & m_lOnBits[31 - iShiftBits]) * m_l2Power[iShiftBits];
		}

		private static long RShift(long lValue, long iShiftBits)
		{
			long num = 0L;
			if (iShiftBits == 0)
			{
				return lValue;
			}
			if (iShiftBits == 31)
			{
				if (Convert.ToBoolean(lValue & 2147483648u))
				{
					return 1L;
				}
				return 0L;
			}
			if (iShiftBits < 0)
			{
				;
			}
			num = (lValue & 0x7FFFFFFE) / m_l2Power[iShiftBits];
			if (Convert.ToBoolean(lValue & 2147483648u))
			{
				num |= 1073741824 / m_l2Power[iShiftBits - 1];
			}
			return num;
		}

		private static long RotateLeft(long lValue, long iShiftBits)
		{
			long num = 0L;
			return LShift(lValue, iShiftBits) | RShift(lValue, 32 - iShiftBits);
		}

		private static long AddUnsigned(long lX, long lY)
		{
			long num = 0L;
			long num2 = 0L;
			long num3 = 0L;
			long num4 = 0L;
			long num5 = 0L;
			long num6 = 0L;
			num4 = (lX & 2147483648u);
			num5 = (lY & 2147483648u);
			num2 = (lX & 0x40000000);
			num3 = (lY & 0x40000000);
			num6 = (lX & 0x3FFFFFFF) + (lY & 0x3FFFFFFF);
			if (Convert.ToBoolean(num2 & num3))
			{
				return num6 ^ 2147483648u ^ num4 ^ num5;
			}
			if (Convert.ToBoolean(num2 | num3))
			{
				if (Convert.ToBoolean(num6 & 0x40000000))
				{
					return num6 ^ 3221225472u ^ num4 ^ num5;
				}
				return num6 ^ 0x40000000 ^ num4 ^ num5;
			}
			return num6 ^ num4 ^ num5;
		}

		private static long md5_F(long x, long y, long z)
		{
			long num = 0L;
			return (x & y) | (~x & z);
		}

		private static long md5_G(long x, long y, long z)
		{
			long num = 0L;
			return (x & z) | (y & ~z);
		}

		private static long md5_H(long x, long y, long z)
		{
			long num = 0L;
			return x ^ y ^ z;
		}

		private static long md5_I(long x, long y, long z)
		{
			long num = 0L;
			return y ^ (x | ~z);
		}

		private static void md5_FF(ref long a, long b, long c, long d, long x, long s, long ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_F(b, c, d), x), ac));
			a = RotateLeft(a, s);
			a = AddUnsigned(a, b);
		}

		private static void md5_GG(ref long a, long b, long c, long d, long x, long s, long ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_G(b, c, d), x), ac));
			a = RotateLeft(a, s);
			a = AddUnsigned(a, b);
		}

		private static void md5_HH(ref long a, long b, long c, long d, long x, long s, long ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_H(b, c, d), x), ac));
			a = RotateLeft(a, s);
			a = AddUnsigned(a, b);
		}

		private static void md5_II(ref long a, long b, long c, long d, long x, long s, long ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(md5_I(b, c, d), x), ac));
			a = RotateLeft(a, s);
			a = AddUnsigned(a, b);
		}

		private static long[] ConvertToWordArray(byte[] bytes)
		{
			long[] array = null;
			int num = 0;
			int num2 = 0;
			long[] array2 = null;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i <= 30; i++)
			{
				m_lOnBits[i] = Convert.ToInt64(Math.Pow(2.0, (double)(i + 1)) - 1.0);
				m_l2Power[i] = Convert.ToInt64(Math.Pow(2.0, (double)i));
			}
			num = bytes.Length;
			num2 = ((num + 8) / 64 + 1) * 16;
			array2 = new long[num2];
			num3 = 0;
			for (num4 = 0; num4 < num; num4++)
			{
				num5 = num4 / 4;
				num3 = num4 % 4 * 8;
				array2[num5] |= LShift(bytes[num4], num3);
			}
			num5 = num4 / 4;
			num3 = num4 % 4 * 8;
			array2[num5] |= LShift(128L, num3);
			array2[num2 - 2] = LShift(num, 3L);
			array2[num2 - 1] = RShift(num, 29L);
			return array2;
		}

		private static string WordToHex(long lValue)
		{
			string text = "";
			long num = 0L;
			int num2 = 0;
			string text2 = "";
			for (num2 = 0; num2 <= 3; num2++)
			{
				num = (RShift(lValue, num2 * 8) & m_lOnBits[7]);
				text2 = ((num != 0) ? ("0" + ToHex(num)) : "00");
				text += text2.Substring(text2.Length - 2);
			}
			return text;
		}

		private static string ToHex(long dec)
		{
			string text = "";
			while (dec > 0)
			{
				text = tohex(dec % 16) + text;
				dec /= 16;
			}
			return text;
		}

		private static string tohex(long hex)
		{
			string text = "";
			long num = hex;
			if (num <= 15 && num >= 10)
			{
				switch (num - 10)
				{
				case 0L:
					return "a";
				case 1L:
					return "b";
				case 2L:
					return "c";
				case 3L:
					return "d";
				case 4L:
					return "e";
				case 5L:
					return "f";
				}
			}
			return hex.ToString();
		}

		public static string Encrypt(string sMessage)
		{
			return Encrypt(sMessage, BitType.Bit32);
		}

		public static string Encrypt(string sMessage, BitType bitType)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(sMessage);
			return Encrypt(bytes, bitType);
		}

		public static string Encrypt(byte[] bytes)
		{
			return Encrypt(bytes, BitType.Bit32);
		}

		public static string Encrypt(byte[] bytes, BitType bitType)
		{
			return Encrypt(ConvertToWordArray(bytes), (int)bitType);
		}

		private static string Encrypt(long[] x, int stype)
		{
			if (stype != 16 && stype != 32)
			{
				throw new Exception("参数stype的值只能取值16或32");
			}
			string text = "";
			int num = 0;
			long num2 = 0L;
			long num3 = 0L;
			long num4 = 0L;
			long num5 = 0L;
			long num6 = 0L;
			long num7 = 0L;
			long num8 = 0L;
			long num9 = 0L;
			num6 = 1732584193L;
			num7 = 4023233417L;
			num8 = 2562383102L;
			num9 = 271733878L;
			for (num = 0; num < x.Length; num += 16)
			{
				num2 = num6;
				num3 = num7;
				num4 = num8;
				num5 = num9;
				md5_FF(ref num6, num7, num8, num9, x[num], 7L, 3614090360L);
				md5_FF(ref num9, num6, num7, num8, x[num + 1], 12L, 3905402710L);
				md5_FF(ref num8, num9, num6, num7, x[num + 2], 17L, 606105819L);
				md5_FF(ref num7, num8, num9, num6, x[num + 3], 22L, 3250441966L);
				md5_FF(ref num6, num7, num8, num9, x[num + 4], 7L, 4118548399L);
				md5_FF(ref num9, num6, num7, num8, x[num + 5], 12L, 1200080426L);
				md5_FF(ref num8, num9, num6, num7, x[num + 6], 17L, 2821735955L);
				md5_FF(ref num7, num8, num9, num6, x[num + 7], 22L, 4249261313L);
				md5_FF(ref num6, num7, num8, num9, x[num + 8], 7L, 1770035416L);
				md5_FF(ref num9, num6, num7, num8, x[num + 9], 12L, 2336552879L);
				md5_FF(ref num8, num9, num6, num7, x[num + 10], 17L, 4294925233L);
				md5_FF(ref num7, num8, num9, num6, x[num + 11], 22L, 2304563134L);
				md5_FF(ref num6, num7, num8, num9, x[num + 12], 7L, 1804603682L);
				md5_FF(ref num9, num6, num7, num8, x[num + 13], 12L, 4254626195L);
				md5_FF(ref num8, num9, num6, num7, x[num + 14], 17L, 2792965006L);
				md5_FF(ref num7, num8, num9, num6, x[num + 15], 22L, 1236535329L);
				md5_GG(ref num6, num7, num8, num9, x[num + 1], 5L, 4129170786L);
				md5_GG(ref num9, num6, num7, num8, x[num + 6], 9L, 3225465664L);
				md5_GG(ref num8, num9, num6, num7, x[num + 11], 14L, 643717713L);
				md5_GG(ref num7, num8, num9, num6, x[num], 20L, 3921069994L);
				md5_GG(ref num6, num7, num8, num9, x[num + 5], 5L, 3593408605L);
				md5_GG(ref num9, num6, num7, num8, x[num + 10], 9L, 38016083L);
				md5_GG(ref num8, num9, num6, num7, x[num + 15], 14L, 3634488961L);
				md5_GG(ref num7, num8, num9, num6, x[num + 4], 20L, 3889429448L);
				md5_GG(ref num6, num7, num8, num9, x[num + 9], 5L, 568446438L);
				md5_GG(ref num9, num6, num7, num8, x[num + 14], 9L, 3275163606L);
				md5_GG(ref num8, num9, num6, num7, x[num + 3], 14L, 4107603335L);
				md5_GG(ref num7, num8, num9, num6, x[num + 8], 20L, 1163531501L);
				md5_GG(ref num6, num7, num8, num9, x[num + 13], 5L, 2850285829L);
				md5_GG(ref num9, num6, num7, num8, x[num + 2], 9L, 4243563512L);
				md5_GG(ref num8, num9, num6, num7, x[num + 7], 14L, 1735328473L);
				md5_GG(ref num7, num8, num9, num6, x[num + 12], 20L, 2368359562L);
				md5_HH(ref num6, num7, num8, num9, x[num + 5], 4L, 4294588738L);
				md5_HH(ref num9, num6, num7, num8, x[num + 8], 11L, 2272392833L);
				md5_HH(ref num8, num9, num6, num7, x[num + 11], 16L, 1839030562L);
				md5_HH(ref num7, num8, num9, num6, x[num + 14], 23L, 4259657740L);
				md5_HH(ref num6, num7, num8, num9, x[num + 1], 4L, 2763975236L);
				md5_HH(ref num9, num6, num7, num8, x[num + 4], 11L, 1272893353L);
				md5_HH(ref num8, num9, num6, num7, x[num + 7], 16L, 4139469664L);
				md5_HH(ref num7, num8, num9, num6, x[num + 10], 23L, 3200236656L);
				md5_HH(ref num6, num7, num8, num9, x[num + 13], 4L, 681279174L);
				md5_HH(ref num9, num6, num7, num8, x[num], 11L, 3936430074L);
				md5_HH(ref num8, num9, num6, num7, x[num + 3], 16L, 3572445317L);
				md5_HH(ref num7, num8, num9, num6, x[num + 6], 23L, 76029189L);
				md5_HH(ref num6, num7, num8, num9, x[num + 9], 4L, 3654602809L);
				md5_HH(ref num9, num6, num7, num8, x[num + 12], 11L, 3873151461L);
				md5_HH(ref num8, num9, num6, num7, x[num + 15], 16L, 530742520L);
				md5_HH(ref num7, num8, num9, num6, x[num + 2], 23L, 3299628645L);
				md5_II(ref num6, num7, num8, num9, x[num], 6L, 4096336452L);
				md5_II(ref num9, num6, num7, num8, x[num + 7], 10L, 1126891415L);
				md5_II(ref num8, num9, num6, num7, x[num + 14], 15L, 2878612391L);
				md5_II(ref num7, num8, num9, num6, x[num + 5], 21L, 4237533241L);
				md5_II(ref num6, num7, num8, num9, x[num + 12], 6L, 1700485571L);
				md5_II(ref num9, num6, num7, num8, x[num + 3], 10L, 2399980690L);
				md5_II(ref num8, num9, num6, num7, x[num + 10], 15L, 4293915773L);
				md5_II(ref num7, num8, num9, num6, x[num + 1], 21L, 2240044497L);
				md5_II(ref num6, num7, num8, num9, x[num + 8], 6L, 1873313359L);
				md5_II(ref num9, num6, num7, num8, x[num + 15], 10L, 4264355552L);
				md5_II(ref num8, num9, num6, num7, x[num + 6], 15L, 2734768916L);
				md5_II(ref num7, num8, num9, num6, x[num + 13], 21L, 1309151649L);
				md5_II(ref num6, num7, num8, num9, x[num + 4], 6L, 4149444226L);
				md5_II(ref num9, num6, num7, num8, x[num + 11], 10L, 3174756917L);
				md5_II(ref num8, num9, num6, num7, x[num + 2], 15L, 718787259L);
				md5_II(ref num7, num8, num9, num6, x[num + 9], 21L, 3951481745L);
				num6 = AddUnsigned(num6, num2);
				num7 = AddUnsigned(num7, num3);
				num8 = AddUnsigned(num8, num4);
				num9 = AddUnsigned(num9, num5);
			}
			text = ((stype != 32) ? (WordToHex(num7) + WordToHex(num8)) : (WordToHex(num6) + WordToHex(num7) + WordToHex(num8) + WordToHex(num9)));
			return text.ToUpper();
		}
	}
}
