
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sys_shop : BaseEntity
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

		public string Username
		{
			get
			{
				return DataConvert.ToString(entityCus["username"]);
			}
			set
			{
				entityCus["username"] = value;
			}
		}

		public string Password
		{
			get
			{
				return DataConvert.ToString(entityCus["password"]);
			}
			set
			{
				entityCus["password"] = value;
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

		public bool Isdefault
		{
			get
			{
				return DataConvert.ToBoolean(entityCus["isdefault"]);
			}
			set
			{
				entityCus["isdefault"] = value;
			}
		}

		public bool Autologin
		{
			get
			{
				return DataConvert.ToBoolean(entityCus["autologin"]);
			}
			set
			{
				entityCus["autologin"] = value;
			}
		}

		public string Sszgusername
		{
			get
			{
				return DataConvert.ToString(entityCus["sszgusername"]);
			}
			set
			{
				entityCus["sszgusername"] = value;
			}
		}

		public int Istmall
		{
			get
			{
				return DataConvert.ToInt(entityCus["istmall"]);
			}
			set
			{
				entityCus["istmall"] = value;
			}
		}

		public int Assignstate
		{
			get
			{
				return DataConvert.ToInt(entityCus["assignstate"]);
			}
			set
			{
				entityCus["assignstate"] = value;
			}
		}

		public int Processid
		{
			get
			{
				return DataConvert.ToInt(entityCus["processid"]);
			}
			set
			{
				entityCus["processid"] = value;
			}
		}

		public string Processname
		{
			get
			{
				return DataConvert.ToString(entityCus["processname"]);
			}
			set
			{
				entityCus["processname"] = value;
			}
		}

		public string Shopshowurl
		{
			get
			{
				return DataConvert.ToString(entityCus["shopshowurl"]);
			}
			set
			{
				entityCus["shopshowurl"] = value;
			}
		}

		public int Useapi
		{
			get
			{
				return DataConvert.ToInt(entityCus["useapi"]);
			}
			set
			{
				entityCus["useapi"] = value;
			}
		}

		public string Appkey
		{
			get
			{
				return DataConvert.ToString(entityCus["appkey"]);
			}
			set
			{
				entityCus["appkey"] = value;
			}
		}

		public string Appsecret
		{
			get
			{
				return DataConvert.ToString(entityCus["appsecret"]);
			}
			set
			{
				entityCus["appsecret"] = value;
			}
		}

		public int Credit
		{
			get
			{
				return DataConvert.ToInt(entityCus["credit"]);
			}
			set
			{
				entityCus["credit"] = value;
			}
		}

		public int Goodeval
		{
			get
			{
				return DataConvert.ToInt(entityCus["goodeval"]);
			}
			set
			{
				entityCus["goodeval"] = value;
			}
		}

		public int Totaleval
		{
			get
			{
				return DataConvert.ToInt(entityCus["totaleval"]);
			}
			set
			{
				entityCus["totaleval"] = value;
			}
		}

		public int Totalitem
		{
			get
			{
				return DataConvert.ToInt(entityCus["totalitem"]);
			}
			set
			{
				entityCus["totalitem"] = value;
			}
		}

		public string Awssecret
		{
			get
			{
				return DataConvert.ToString(entityCus["awssecret"]);
			}
			set
			{
				entityCus["awssecret"] = value;
			}
		}

		public string Authcode
		{
			get
			{
				return DataConvert.ToString(entityCus["authcode"]);
			}
			set
			{
				entityCus["authcode"] = value;
			}
		}

		public Sys_shop()
		{
			entityCus = new EntityCustom("sys_shop");
		}

		public Sys_shop Clone()
		{
			Sys_shop sys_shop = new Sys_shop();
			sys_shop.entityCus = entityCus.Clone();
			return sys_shop;
		}

		public static string ConnectValuesWithChar(IList<Sys_shop> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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

		public static string ConnectValuesWithChar(IList<Sys_shop> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}

		public static IList<Sys_shop> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sys_shop> list = new List<Sys_shop>();
			Sys_shop sys_shop = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sys_shop = new Sys_shop();
				foreach (DataColumn column in dt.Columns)
				{
					sys_shop.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
				}
				list.Add(sys_shop);
			}
			return list;
		}
	}
}
