using System;
using System.Text;

namespace TaoBaoGuanJia.Util
{
	public class Base64
	{
		public static string FromBase64(string str)
		{
			return FromBase64(str, Encoding.UTF8);
		}

		public static string FromBase64(string str, Encoding encoder)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			if (encoder == null)
			{
				encoder = Encoding.UTF8;
			}
			byte[] bytes = Convert.FromBase64String(str);
			return encoder.GetString(bytes);
		}

		public static string ToBase64(string str)
		{
			return ToBase64(str, Encoding.UTF8);
		}

		public static string ToBase64(string str, Encoding encoder)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			if (encoder == null)
			{
				encoder = Encoding.UTF8;
			}
			byte[] bytes = encoder.GetBytes(str);
			return Convert.ToBase64String(bytes);
		}
	}
}
