using System;

namespace TaoBaoGuanJia.Util
{
	public class DataConvert
	{
		public static short ToShort(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				try
				{
					return Convert.ToInt16(o);
				}
				catch
				{
					return 0;
				}
			}
			return 0;
		}

		public static long ToLong(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				try
				{
					return Convert.ToInt64(o);
				}
				catch
				{
					return 0L;
				}
			}
			return 0L;
		}

		public static int ToInt(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				try
				{
					return Convert.ToInt32(o);
				}
				catch
				{
					return 0;
				}
			}
			return 0;
		}

		public static bool ToBoolean(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				string text = ToString(o).ToLower();
				if (!(text == "true") && !(text == "1"))
				{
					try
					{
						bool result = false;
						bool.TryParse(text, out result);
						return result;
					}
					catch
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static double ToDouble(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				try
				{
					return Convert.ToDouble(o);
				}
				catch
				{
					return 0.0;
				}
			}
			return 0.0;
		}

		public static decimal ToDecimal(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				try
				{
					return Convert.ToDecimal(o);
				}
				catch
				{
					return 0m;
				}
			}
			return 0m;
		}

		public static string ToString(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				try
				{
					return Convert.ToString(o);
				}
				catch
				{
					return string.Empty;
				}
			}
			return string.Empty;
		}

		public static DateTime ToDateTime(object o)
		{
			if (o != null && o != DBNull.Value)
			{
				try
				{
					return Convert.ToDateTime(o);
				}
				catch
				{
					return DateTime.MinValue;
				}
			}
			return DateTime.MinValue;
		}

		public static int[] StrToInts(string strValue, char token)
		{
			int[] array = null;
			if (strValue != null)
			{
				string[] array2 = strValue.Split(token);
				array = new int[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array[i] = ToInt(array2[i]);
				}
			}
			return array;
		}

		public static string[] StrToStrs(string strValue, char token)
		{
			string[] array = null;
			if (strValue != null)
			{
				string[] array2 = strValue.Split(token);
				array = new string[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array[i] = array2[i];
				}
			}
			return array;
		}

		public static long[] StrToLongs(string strValue, char token)
		{
			long[] array = null;
			if (strValue != null)
			{
				string[] array2 = strValue.Split(token);
				array = new long[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array[i] = ToLong(array2[i]);
				}
			}
			return array;
		}

		private static string GetString(string str)
		{
			string text = "";
			switch (str)
			{
			case "1":
				return "01";
			case "2":
				return "02";
			case "3":
				return "03";
			case "4":
				return "04";
			case "5":
				return "05";
			case "6":
				return "06";
			case "7":
				return "07";
			case "8":
				return "08";
			case "9":
				return "09";
			default:
				return str;
			}
		}

		public static string FirstCharUpper(string strValue)
		{
			if (strValue != null)
			{
				if (strValue.Length >= 2)
				{
					return strValue.Substring(0, 1).ToUpper() + strValue.Substring(1, strValue.Length - 1);
				}
				return strValue;
			}
			return "";
		}

		public static string FirstCharLower(string strValue)
		{
			if (strValue != null)
			{
				if (strValue.Length >= 2)
				{
					return strValue.Substring(0, 1).ToLower() + strValue.Substring(1, strValue.Length - 1);
				}
				return strValue;
			}
			return "";
		}

		public static bool StrToBool(string str)
		{
			if (str != null && str.Trim().Length > 0)
			{
				str = str.ToLower().Trim();
				if (!(str == "true") && !(str == "1"))
				{
					try
					{
						bool result = false;
						bool.TryParse(str, out result);
						return result;
					}
					catch
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool StrToInt(string str, out int result)
		{
			result = 0;
			if (str != null && str.Trim().Length > 0)
			{
				if (str.ToLower().Trim() == "true")
				{
					result = 1;
					return true;
				}
				if (str.ToLower().Trim() == "false")
				{
					result = 0;
					return true;
				}
				try
				{
					if (int.TryParse(str, out result))
					{
						return true;
					}
					result = 0;
					return false;
				}
				catch
				{
					result = 0;
					return false;
				}
			}
			return true;
		}

		public static bool StrToLong(string str, out long result)
		{
			result = 0L;
			if (str != null && str.Trim().Length > 0)
			{
				if (str.ToLower().Trim() == "true")
				{
					result = 1L;
					return true;
				}
				if (str.ToLower().Trim() == "false")
				{
					result = 0L;
					return true;
				}
				try
				{
					if (long.TryParse(str, out result))
					{
						return true;
					}
					result = 0L;
					return false;
				}
				catch
				{
					result = 0L;
					return false;
				}
			}
			return true;
		}

		public static bool StrToShort(string str, out short result)
		{
			result = 0;
			if (str != null && str.Trim().Length > 0)
			{
				if (str.ToLower().Trim() == "true")
				{
					result = 1;
					return true;
				}
				if (str.ToLower().Trim() == "false")
				{
					result = 0;
					return true;
				}
				try
				{
					if (short.TryParse(str, out result))
					{
						return true;
					}
					result = 0;
					return false;
				}
				catch
				{
					result = 0;
					return false;
				}
			}
			return true;
		}
	}
}
