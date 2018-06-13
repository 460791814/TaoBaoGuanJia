
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_sysConfig : BaseEntity
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

		public string Sectiongroup
		{
			get
			{
				return DataConvert.ToString(entityCus["sectiongroup"]);
			}
			set
			{
				entityCus["sectiongroup"] = value;
			}
		}

		public string Sectionname
		{
			get
			{
				return DataConvert.ToString(entityCus["sectionname"]);
			}
			set
			{
				entityCus["sectionname"] = value;
			}
		}

		public string Configkey
		{
			get
			{
				return DataConvert.ToString(entityCus["configkey"]);
			}
			set
			{
				entityCus["configkey"] = value;
			}
		}

		public string Configvalue
		{
			get
			{
				return DataConvert.ToString(entityCus["configvalue"]);
			}
			set
			{
				entityCus["configvalue"] = value;
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

		public DateTime Createdtime
		{
			get
			{
				return DataConvert.ToDateTime(entityCus["createdtime"]);
			}
			set
			{
				entityCus["createdtime"] = value;
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

		public Sys_sysConfig()
		{
			entityCus = new EntityCustom("sys_sysConfig");
		}

		public Sys_sysConfig Clone()
		{
			Sys_sysConfig sys_sysConfig = new Sys_sysConfig();
			sys_sysConfig.entityCus = entityCus.Clone();
			return sys_sysConfig;
		}

		public static string ConnectValuesWithChar(IList<Sys_sysConfig> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_sysConfig> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_sysConfig> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_sysConfig> list = new List<Sys_sysConfig>();
			Sys_sysConfig sys_sysConfig = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_sysConfig = new Sys_sysConfig();
				foreach (DataColumn column in dt.Columns)
				{
					sys_sysConfig.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_sysConfig);
			}
			return list;
		}
	}
}
