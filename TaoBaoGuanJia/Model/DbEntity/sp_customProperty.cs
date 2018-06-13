
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sp_customProperty : BaseEntity
	{
		private EntityCustom entityCus;
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
		public int AllowPic
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["allowPic"]);
			}
			set
			{
				this.entityCus["allowPic"] = value;
			}
		}
		public string Custompropertyname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["custompropertyname"]);
			}
			set
			{
				this.entityCus["custompropertyname"] = value;
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
		public Sp_customProperty()
		{
			this.entityCus = new EntityCustom("sp_customProperty");
		}
		public Sp_customProperty Clone()
		{
			return new Sp_customProperty
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_customProperty> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_customProperty> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_customProperty.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_customProperty> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_customProperty> list = new List<Sp_customProperty>();
			Sp_customProperty sp_customProperty = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_customProperty = new Sp_customProperty();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_customProperty.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_customProperty);
			}
			return list;
		}
	}
}
