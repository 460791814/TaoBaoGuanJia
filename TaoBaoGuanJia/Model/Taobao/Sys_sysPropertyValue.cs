
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_sysPropertyValue : BaseEntity
	{
		private EntityCustom entityCus;

		public override EntityCustom EntityCustom => entityCus;

		public int Id
		{
			get
			{
				return DataConvert.ToInt(entityCus["id"]);
			}
			set
			{
				entityCus["id"] = value;
			}
		}

		public int Sysid
		{
			get
			{
				return DataConvert.ToInt(entityCus["sysid"]);
			}
			set
			{
				entityCus["sysid"] = value;
			}
		}

		public int Sortid
		{
			get
			{
				return DataConvert.ToInt(entityCus["sortid"]);
			}
			set
			{
				entityCus["sortid"] = value;
			}
		}

		public int Propertyid
		{
			get
			{
				return DataConvert.ToInt(entityCus["propertyid"]);
			}
			set
			{
				entityCus["propertyid"] = value;
			}
		}

		public string Name
		{
			get
			{
				return DataConvert.ToString(entityCus["name"]);
			}
			set
			{
				entityCus["name"] = value;
			}
		}

		public string Value
		{
			get
			{
				return DataConvert.ToString(entityCus["value"]);
			}
			set
			{
				entityCus["value"] = value;
			}
		}

		public DateTime Modifytime
		{
			get
			{
				return DataConvert.ToDateTime(entityCus["modifytime"]);
			}
			set
			{
				entityCus["modifytime"] = value;
			}
		}

		public bool Isconfirm
		{
			get
			{
				return DataConvert.ToBoolean(entityCus["isconfirm"]);
			}
			set
			{
				entityCus["isconfirm"] = value;
			}
		}

		public int Del
		{
			get
			{
				return DataConvert.ToInt(entityCus["del"]);
			}
			set
			{
				entityCus["del"] = value;
			}
		}

		public int Isstandardproduct
		{
			get
			{
				return DataConvert.ToInt(entityCus["isstandardproduct"]);
			}
			set
			{
				entityCus["isstandardproduct"] = value;
			}
		}

		public bool Haschildproperty
		{
			get
			{
				return DataConvert.ToBoolean(entityCus["haschildproperty"]);
			}
			set
			{
				entityCus["haschildproperty"] = value;
			}
		}

		public Sys_sysPropertyValue()
		{
			entityCus = new EntityCustom("sys_sysPropertyValue");
		}

		public Sys_sysPropertyValue Clone()
		{
			Sys_sysPropertyValue sys_sysPropertyValue = new Sys_sysPropertyValue();
			sys_sysPropertyValue.entityCus = entityCus.Clone();
			return sys_sysPropertyValue;
		}

		public static string ConnectValuesWithChar(IList<Sys_sysPropertyValue> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
		{
			if (list != null && list.Count > 0)
			{
				IList<EntityCustom> list2 = new List<EntityCustom>();
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(list[i].EntityCustom);
				}
				return ValuesConnectUtil.ConnectValuesWithChar(list2, fieldName, connectChar, wrapCharType, trim);
			}
			return null;
		}

		public static string ConnectValuesWithChar(IList<Sys_sysPropertyValue> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_sysPropertyValue> TransDataRowToEntityList(DataTable dt, DataRow row)
		{
			IList<Sys_sysPropertyValue> list = new List<Sys_sysPropertyValue>();
			Sys_sysPropertyValue sys_sysPropertyValue = new Sys_sysPropertyValue();
			foreach (DataColumn column in dt.Columns)
			{
				sys_sysPropertyValue.EntityCustom.SetValue(column.ColumnName, row[column]);
			}
			list.Add(sys_sysPropertyValue);
			return list;
		}

		public static IList<Sys_sysPropertyValue> TransDataRowsToEntityList(DataTable dt, DataRow[] rows)
		{
			IList<Sys_sysPropertyValue> list = new List<Sys_sysPropertyValue>();
			if (rows != null && rows.Length > 0)
			{
				Sys_sysPropertyValue sys_sysPropertyValue = null;
				for (int i = 0; i < rows.Length; i++)
				{
					sys_sysPropertyValue = new Sys_sysPropertyValue();
					foreach (DataColumn column in dt.Columns)
					{
						sys_sysPropertyValue.EntityCustom.SetValue(column.ColumnName, rows[i][column]);
					}
					list.Add(sys_sysPropertyValue);
				}
				return list;
			}
			return list;
		}

		public static IList<Sys_sysPropertyValue> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_sysPropertyValue> list = new List<Sys_sysPropertyValue>();
			Sys_sysPropertyValue sys_sysPropertyValue = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_sysPropertyValue = new Sys_sysPropertyValue();
				foreach (DataColumn column in dt.Columns)
				{
					sys_sysPropertyValue.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_sysPropertyValue);
			}
			return list;
		}
	}
}
