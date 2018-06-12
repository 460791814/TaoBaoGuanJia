
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_sysSort : BaseEntity
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

		public string Remark
		{
			get
			{
				return DataConvert.ToString(entityCus["remark"]);
			}
			set
			{
				entityCus["remark"] = value;
			}
		}

		public string Parentkey
		{
			get
			{
				return DataConvert.ToString(entityCus["parentkey"]);
			}
			set
			{
				entityCus["parentkey"] = value;
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

		public string Path
		{
			get
			{
				return DataConvert.ToString(entityCus["path"]);
			}
			set
			{
				entityCus["path"] = value;
			}
		}

		public int Hassellinfo
		{
			get
			{
				return DataConvert.ToInt(entityCus["hassellinfo"]);
			}
			set
			{
				entityCus["hassellinfo"] = value;
			}
		}

		public bool IsNewSellPro
		{
			get
			{
				return DataConvert.ToBoolean(entityCus["isnewsellpro"]);
			}
			set
			{
				entityCus["isnewsellpro"] = value;
			}
		}

		public string SizeGroupType
		{
			get
			{
				return DataConvert.ToString(entityCus["sizegrouptype"]);
			}
			set
			{
				entityCus["sizegrouptype"] = value;
			}
		}

		public Sys_sysSort()
		{
			entityCus = new EntityCustom("sys_sysSort");
		}

		public Sys_sysSort Clone()
		{
			Sys_sysSort sys_sysSort = new Sys_sysSort();
			sys_sysSort.entityCus = entityCus.Clone();
			return sys_sysSort;
		}

		public static string ConnectValuesWithChar(IList<Sys_sysSort> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_sysSort> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_sysSort> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_sysSort> list = new List<Sys_sysSort>();
			Sys_sysSort sys_sysSort = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_sysSort = new Sys_sysSort();
				foreach (DataColumn column in dt.Columns)
				{
					sys_sysSort.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_sysSort);
			}
			return list;
		}
	}
}
