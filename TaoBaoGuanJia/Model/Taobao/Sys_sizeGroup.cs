
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_sizeGroup : BaseEntity
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

		public int SysId
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

		public string GroupType
		{
			get
			{
				return DataConvert.ToString(entityCus["grouptype"]);
			}
			set
			{
				entityCus["grouptype"] = value;
			}
		}

		public string GroupName
		{
			get
			{
				return DataConvert.ToString(entityCus["groupname"]);
			}
			set
			{
				entityCus["groupname"] = value;
			}
		}

		public string GroupOnlineID
		{
			get
			{
				return DataConvert.ToString(entityCus["grouponlineid"]);
			}
			set
			{
				entityCus["grouponlineid"] = value;
			}
		}

		public string GroupKey
		{
			get
			{
				return DataConvert.ToString(entityCus["groupkey"]);
			}
			set
			{
				entityCus["groupkey"] = value;
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

		public DateTime ModifyTime
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

		public Sys_sizeGroup()
		{
			entityCus = new EntityCustom("sys_sizegroup");
		}

		public Sys_sizeGroup Clone()
		{
			Sys_sizeGroup sys_sizeGroup = new Sys_sizeGroup();
			sys_sizeGroup.entityCus = entityCus.Clone();
			return sys_sizeGroup;
		}

		public static string ConnectValuesWithChar(IList<Sys_sizeGroup> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_sizeGroup> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_sizeGroup> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_sizeGroup> list = new List<Sys_sizeGroup>();
			Sys_sizeGroup sys_sizeGroup = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_sizeGroup = new Sys_sizeGroup();
				foreach (DataColumn column in dt.Columns)
				{
					sys_sizeGroup.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_sizeGroup);
			}
			return list;
		}
	}
}
