
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_sysProperty : BaseEntity
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

		public int Parentid
		{
			get
			{
				return DataConvert.ToInt(entityCus["parentid"]);
			}
			set
			{
				entityCus["parentid"] = value;
			}
		}

		public string Parentname
		{
			get
			{
				return DataConvert.ToString(entityCus["parentname"]);
			}
			set
			{
				entityCus["parentname"] = value;
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

		public bool Allownull
		{
			get
			{
				return DataConvert.ToBoolean(entityCus["allownull"]);
			}
			set
			{
				entityCus["allownull"] = value;
			}
		}

		public int Valuetype
		{
			get
			{
				return DataConvert.ToInt(entityCus["valuetype"]);
			}
			set
			{
				entityCus["valuetype"] = value;
			}
		}

		public string Cssselection
		{
			get
			{
				return DataConvert.ToString(entityCus["cssselection"]);
			}
			set
			{
				entityCus["cssselection"] = value;
			}
		}

		public bool Issellpro
		{
			get
			{
				return DataConvert.ToBoolean(entityCus["issellpro"]);
			}
			set
			{
				entityCus["issellpro"] = value;
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

		public int Parentprovalueid
		{
			get
			{
				return DataConvert.ToInt(entityCus["parentprovalueid"]);
			}
			set
			{
				entityCus["parentprovalueid"] = value;
			}
		}

		public int Propertytype
		{
			get
			{
				return DataConvert.ToInt(entityCus["propertytype"]);
			}
			set
			{
				entityCus["propertytype"] = value;
			}
		}

		public int Parentlevel
		{
			get
			{
				return DataConvert.ToInt(entityCus["parentlevel"]);
			}
			set
			{
				entityCus["parentlevel"] = value;
			}
		}

		public int Levels
		{
			get
			{
				return DataConvert.ToInt(entityCus["levels"]);
			}
			set
			{
				entityCus["levels"] = value;
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

		public string Parentpropertyvalue
		{
			get
			{
				return DataConvert.ToString(entityCus["parentpropertyvalue"]);
			}
			set
			{
				entityCus["parentpropertyvalue"] = value;
			}
		}

		public string Keys
		{
			get
			{
				return DataConvert.ToString(entityCus["keys"]);
			}
			set
			{
				entityCus["keys"] = value;
			}
		}

		public int Maxlength
		{
			get
			{
				return DataConvert.ToInt(entityCus["maxlength"]);
			}
			set
			{
				entityCus["maxlength"] = value;
			}
		}

		public int Maxnum
		{
			get
			{
				return DataConvert.ToInt(entityCus["maxnum"]);
			}
			set
			{
				entityCus["maxnum"] = value;
			}
		}

		public string Hint
		{
			get
			{
				return DataConvert.ToString(entityCus["hint"]);
			}
			set
			{
				entityCus["hint"] = value;
			}
		}

		public string Pattern
		{
			get
			{
				return DataConvert.ToString(entityCus["pattern"]);
			}
			set
			{
				entityCus["pattern"] = value;
			}
		}

		public Sys_sysProperty()
		{
			entityCus = new EntityCustom("sys_sysProperty");
		}

		public Sys_sysProperty Clone()
		{
			Sys_sysProperty sys_sysProperty = new Sys_sysProperty();
			sys_sysProperty.entityCus = entityCus.Clone();
			return sys_sysProperty;
		}

		public static string ConnectValuesWithChar(IList<Sys_sysProperty> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_sysProperty> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_sysProperty> TransDataRowsToEntityList(DataTable dt, DataRow[] rows)
		{
			IList<Sys_sysProperty> list = new List<Sys_sysProperty>();
			if (rows != null && rows.Length > 0)
			{
				Sys_sysProperty sys_sysProperty = null;
				for (int i = 0; i < rows.Length; i++)
				{
					sys_sysProperty = new Sys_sysProperty();
					foreach (DataColumn column in dt.Columns)
					{
						sys_sysProperty.EntityCustom.SetValue(column.ColumnName, rows[i][column]);
					}
					list.Add(sys_sysProperty);
				}
				return list;
			}
			return list;
		}

		public static IList<Sys_sysProperty> TransDataRowToEntityList(DataTable dt, DataRow row)
		{
			IList<Sys_sysProperty> list = new List<Sys_sysProperty>();
			Sys_sysProperty sys_sysProperty = new Sys_sysProperty();
			foreach (DataColumn column in dt.Columns)
			{
				sys_sysProperty.EntityCustom.SetValue(column.ColumnName, row[column]);
			}
			list.Add(sys_sysProperty);
			return list;
		}

		public static IList<Sys_sysProperty> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_sysProperty> list = new List<Sys_sysProperty>();
			Sys_sysProperty sys_sysProperty = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_sysProperty = new Sys_sysProperty();
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
