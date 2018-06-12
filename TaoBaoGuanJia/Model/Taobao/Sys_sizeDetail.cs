
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_sizeDetail : BaseEntity
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

		public string SizeName
		{
			get
			{
				return DataConvert.ToString(entityCus["sizename"]);
			}
			set
			{
				entityCus["sizename"] = value;
			}
		}

		public string SizeValue
		{
			get
			{
				return DataConvert.ToString(entityCus["sizevalue"]);
			}
			set
			{
				entityCus["sizevalue"] = value;
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

		public Sys_sizeDetail()
		{
			entityCus = new EntityCustom("sys_sizedetail");
		}

		public Sys_sizeDetail Clone()
		{
			Sys_sizeDetail sys_sizeDetail = new Sys_sizeDetail();
			sys_sizeDetail.entityCus = entityCus.Clone();
			return sys_sizeDetail;
		}

		public static string ConnectValuesWithChar(IList<Sys_sizeDetail> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_sizeDetail> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_sizeDetail> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_sizeDetail> list = new List<Sys_sizeDetail>();
			Sys_sizeDetail sys_sizeDetail = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_sizeDetail = new Sys_sizeDetail();
				foreach (DataColumn column in dt.Columns)
				{
					sys_sizeDetail.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_sizeDetail);
			}
			return list;
		}
	}
}
