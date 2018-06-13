using TaoBaoGuanJia.Util;

using System;
using System.Collections.Generic;
using System.Data;
namespace TaoBaoGuanJia.Model
{
	public class Sp_property : BaseEntity
	{
		private List<Sp_property> _childSpPorpertyList = new List<Sp_property>();
		private EntityCustom entityCus;
		public Sys_sysProperty SysProperty
		{
			get;
			set;
		}
		public List<Sp_property> ChildSpPorpertyList
		{
			get
			{
				if (this._childSpPorpertyList == null)
				{
					this._childSpPorpertyList = new List<Sp_property>();
				}
				return this._childSpPorpertyList;
			}
			set
			{
				this._childSpPorpertyList = value;
			}
		}
		public override EntityCustom EntityCustom
		{
			get
			{
				return this.entityCus;
			}
		}
		public int Id
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["id"]);
			}
			set
			{
				this.entityCus["id"] = value;
			}
		}
		public int Itemid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["itemid"]);
			}
			set
			{
				this.entityCus["itemid"] = value;
			}
		}
		public int Shopid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["shopid"]);
			}
			set
			{
				this.entityCus["shopid"] = value;
			}
		}
		public int Sysid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["sysid"]);
			}
			set
			{
				this.entityCus["sysid"] = value;
			}
		}
		public int Propertyid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["propertyid"]);
			}
			set
			{
				this.entityCus["propertyid"] = value;
			}
		}
		public string Name
		{
			get
			{
				return DataConvert.ToString(this.entityCus["name"]);
			}
			set
			{
				this.entityCus["name"] = value;
			}
		}
		public string Value
		{
			get
			{
				return DataConvert.ToString(this.entityCus["value"]);
			}
			set
			{
				this.entityCus["value"] = value;
			}
		}
		public DateTime Modifytime
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["modifytime"]);
			}
			set
			{
				this.entityCus["modifytime"] = value;
			}
		}
		public string Propertykeys
		{
			get
			{
				return DataConvert.ToString(this.entityCus["propertykeys"]);
			}
			set
			{
				this.entityCus["propertykeys"] = value;
			}
		}
		public int Issellpro
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["issellpro"]);
			}
			set
			{
				this.entityCus["issellpro"] = value;
			}
		}
		public string Aliasname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["aliasname"]);
			}
			set
			{
				this.entityCus["aliasname"] = value;
			}
		}
		public string PicUrl
		{
			get
			{
				return DataConvert.ToString(this.entityCus["picurl"]);
			}
			set
			{
				this.entityCus["picurl"] = value;
			}
		}
		public string Url
		{
			get
			{
				return DataConvert.ToString(this.entityCus["url"]);
			}
			set
			{
				this.entityCus["url"] = value;
			}
		}
		public Sp_property()
		{
			this.entityCus = new EntityCustom("sp_property");
		}
		public Sp_property Clone()
		{
			return new Sp_property
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_property> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
		{
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			IList<EntityCustom> list2 = new List<EntityCustom>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(list[i].EntityCustom);
			}
			return ValuesConnectUtil.ConnectValuesWithChar(list2, fieldName, connectChar, wrapCharType, trim);
		}
		public static string ConnectValuesWithChar(IList<Sp_property> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_property.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_property> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_property> list = new List<Sp_property>();
			Sp_property sp_property = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_property = new Sp_property();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_property.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_property);
			}
			return list;
		}
	}
}
