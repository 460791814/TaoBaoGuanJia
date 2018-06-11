
using System.Collections.Generic;
using System.Data;

namespace TaoBaoGuanJia.Util
{
	internal class DbUtil
	{
		private static string OerateOneSpecialChar(string str, string specialChar, string left, string right)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return str.Replace(specialChar, left + specialChar + right);
		}

		public static string OerateSpecialChar(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			str = str.Replace("'", "''");
			return str;
		}

		private static string ConnectValues(string oldValues, string addValues, char connectChar)
		{
			oldValues = ((oldValues != null) ? (oldValues + connectChar + addValues) : addValues);
			return oldValues;
		}
        
	}
}
