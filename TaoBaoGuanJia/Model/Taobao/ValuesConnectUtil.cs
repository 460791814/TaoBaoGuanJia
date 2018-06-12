using System.Collections.Generic;
using System.Data;

namespace TaoBaoGuanJia.Model
{
	public class ValuesConnectUtil
	{
		public static string ConnectValuesWithComma(IList<int> intList, WrapCharType wrapCharType)
		{
			if (intList == null)
			{
				return ConnectValuesWithChar(null, ',', wrapCharType);
			}
			int[] array = new int[intList.Count];
			for (int i = 0; i < intList.Count; i++)
			{
				array[i] = intList[i];
			}
			return ConnectValuesWithChar(array, ',', wrapCharType);
		}

		public static string ConnectValuesWithComma(IList<string> strList, WrapCharType wrapCharType, bool trim)
		{
			if (strList == null)
			{
				return ConnectValuesWithChar(null, ',', wrapCharType, trim);
			}
			string[] array = new string[strList.Count];
			for (int i = 0; i < strList.Count; i++)
			{
				array[i] = strList[i];
			}
			return ConnectValuesWithChar(array, ',', wrapCharType, trim);
		}

		public static string ConnectValuesWithComma(int[] arr, WrapCharType wrapCharType)
		{
			return ConnectValuesWithChar(arr, ',', wrapCharType);
		}

		public static string ConnectValuesWithComma(string[] arr, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(arr, ',', wrapCharType, trim);
		}

		public static string ConnectValuesWithComma(object[] arr, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(arr, ',', wrapCharType, trim);
		}

		public static string ConnectValuesWithComma(IList<EntityCustom> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		private static string OperateInputStr(string inputStr, WrapCharType wrapCharType, bool trim)
		{
			if (inputStr == null)
			{
				inputStr = "";
			}
			if (trim)
			{
				inputStr = inputStr.Trim();
			}
			switch (wrapCharType)
			{
			case WrapCharType.DoubleQuotationMarks:
				return "\"" + inputStr + "\"";
			case WrapCharType.InvertedComma:
				return "'" + inputStr + "'";
			default:
				return inputStr;
			}
		}

		private static string ConnectValues(string oldValues, string addValues, char connectChar)
		{
			oldValues = ((oldValues != null) ? (oldValues + connectChar + addValues) : addValues);
			return oldValues;
		}

		public static string ConnectValuesWithChar(int[] arr, char connectChar, WrapCharType wrapCharType)
		{
			string text = null;
			if (arr != null && arr.Length > 0)
			{
				for (int i = 0; i < arr.Length; i++)
				{
					text = ConnectValues(text, OperateInputStr(arr[i].ToString(), wrapCharType, false), connectChar);
				}
				return text;
			}
			return text;
		}

		public static string ConnectValuesWithChar(string[] arr, char connectChar, WrapCharType wrapCharType, bool trim)
		{
			string text = null;
			if (arr != null && arr.Length > 0)
			{
				string text2 = null;
				for (int i = 0; i < arr.Length; i++)
				{
					if (arr[i] != null)
					{
						text2 = arr[i];
						if (trim)
						{
							text2 = arr[i].Trim();
						}
						if (!string.IsNullOrEmpty(text2))
						{
							text = ConnectValues(text, OperateInputStr(text2, wrapCharType, trim), connectChar);
						}
					}
				}
				return text;
			}
			return text;
		}

		public static string ConnectValuesWithChar(object[] arr, char connectChar, WrapCharType wrapCharType, bool trim)
		{
			string text = null;
			if (arr != null && arr.Length > 0)
			{
				string text2 = null;
				for (int i = 0; i < arr.Length; i++)
				{
					if (arr[i] != null)
					{
						text2 = arr[i].ToString();
						if (trim)
						{
							text2 = arr[i].ToString().Trim();
						}
						if (!string.IsNullOrEmpty(text2))
						{
							text = ConnectValues(text, OperateInputStr(text2, wrapCharType, trim), connectChar);
						}
					}
				}
				return text;
			}
			return text;
		}

		public static string ConnectValuesWithChar(IList<EntityCustom> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
		{
			if (list != null && list.Count > 0)
			{
				string[] array = new string[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = list[i].GetStringNull(fieldName);
				}
				return ConnectValuesWithChar(array, connectChar, wrapCharType, trim);
			}
			return null;
		}

		public static IList<EntityCustom> TransDataTableToEntityCustomList(DataTable dt, string name)
		{
			IList<EntityCustom> list = new List<EntityCustom>();
			EntityCustom entityCustom = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				entityCustom = new EntityCustom(name);
				foreach (DataColumn column in dt.Columns)
				{
					entityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(entityCustom);
			}
			return list;
		}

		public static IList<EntityCustom> TransDataTableToEntityCustomList(DataTable dt)
		{
			return TransDataTableToEntityCustomList(dt, null);
		}
	}
}
