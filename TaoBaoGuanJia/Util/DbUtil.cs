
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Model;

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
        public static IList<T> DataTableToEntityList<T>(DataTable dt) where T : BaseEntity, new()
        {
            IList<T> list = new List<T>();
            T sys_sysProperty = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sys_sysProperty = new T();
                foreach (DataColumn column in dt.Columns)
                {
                    sys_sysProperty.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
                }
                list.Add(sys_sysProperty);
            }
            return list;
        }

    }
}
