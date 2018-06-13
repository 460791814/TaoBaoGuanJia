
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_shopSort : BaseEntity
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

		public string Sortname
		{
			get
			{
				return DataConvert.ToString(entityCus["sortname"]);
			}
			set
			{
				entityCus["sortname"] = value;
			}
		}

		public int Sortlevels
		{
			get
			{
				return DataConvert.ToInt(entityCus["sortlevels"]);
			}
			set
			{
				entityCus["sortlevels"] = value;
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

		public string Parent
		{
			get
			{
				return DataConvert.ToString(entityCus["parent"]);
			}
			set
			{
				entityCus["parent"] = value;
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

		public int Isupload
		{
			get
			{
				return DataConvert.ToInt(entityCus["isupload"]);
			}
			set
			{
				entityCus["isupload"] = value;
			}
		}

		public string Keyinshop
		{
			get
			{
				return DataConvert.ToString(entityCus["keyinshop"]);
			}
			set
			{
				entityCus["keyinshop"] = value;
			}
		}

		public int Isdelete
		{
			get
			{
				return DataConvert.ToInt(entityCus["isdelete"]);
			}
			set
			{
				entityCus["isdelete"] = value;
			}
		}

		public string Uploadfailmsg
		{
			get
			{
				return DataConvert.ToString(entityCus["uploadfailmsg"]);
			}
			set
			{
				entityCus["uploadfailmsg"] = value;
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

		public string Apikey
		{
			get
			{
				return DataConvert.ToString(entityCus["apikey"]);
			}
			set
			{
				entityCus["apikey"] = value;
			}
		}

		public Sys_shopSort()
		{
			entityCus = new EntityCustom("sys_shopSort");
		}

		public Sys_shopSort Clone()
		{
			Sys_shopSort sys_shopSort = new Sys_shopSort();
			sys_shopSort.entityCus = entityCus.Clone();
			return sys_shopSort;
		}

		public static string ConnectValuesWithChar(IList<Sys_shopSort> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_shopSort> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_shopSort> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_shopSort> list = new List<Sys_shopSort>();
			Sys_shopSort sys_shopSort = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_shopSort = new Sys_shopSort();
				foreach (DataColumn column in dt.Columns)
				{
					sys_shopSort.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_shopSort);
			}
			return list;
		}
	}
}
