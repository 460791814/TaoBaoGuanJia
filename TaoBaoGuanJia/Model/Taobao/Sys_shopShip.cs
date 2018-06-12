
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_shopShip : BaseEntity
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

		public int Shopid
		{
			get
			{
				return DataConvert.ToInt(entityCus["shopid"]);
			}
			set
			{
				entityCus["shopid"] = value;
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

		public DateTime Crtdate
		{
			get
			{
				return DataConvert.ToDateTime(entityCus["crtdate"]);
			}
			set
			{
				entityCus["crtdate"] = value;
			}
		}

		public DateTime Modifydate
		{
			get
			{
				return DataConvert.ToDateTime(entityCus["modifydate"]);
			}
			set
			{
				entityCus["modifydate"] = value;
			}
		}

		public int Synchstate
		{
			get
			{
				return DataConvert.ToInt(entityCus["synchstate"]);
			}
			set
			{
				entityCus["synchstate"] = value;
			}
		}

		public Sys_shopShip()
		{
			entityCus = new EntityCustom("sys_shopShip");
		}

		public Sys_shopShip Clone()
		{
			Sys_shopShip sys_shopShip = new Sys_shopShip();
			sys_shopShip.entityCus = entityCus.Clone();
			return sys_shopShip;
		}

		public static string ConnectValuesWithChar(IList<Sys_shopShip> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_shopShip> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_shopShip> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_shopShip> list = new List<Sys_shopShip>();
			Sys_shopShip sys_shopShip = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_shopShip = new Sys_shopShip();
				foreach (DataColumn column in dt.Columns)
				{
					sys_shopShip.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_shopShip);
			}
			return list;
		}
	}
}
