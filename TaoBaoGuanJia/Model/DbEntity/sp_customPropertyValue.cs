using TaoBaoGuanJia.Util;
using System;
using System.Collections.Generic;
using System.Data;
namespace TaoBaoGuanJia.Model
{
	public class Sp_customPropertyValue : BaseEntity
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
		public int Custompropertyid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["custompropertyid"]);
			}
			set
			{
				this.entityCus["custompropertyid"] = value;
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
		public string Custompropertyvalue
		{
			get
			{
				return DataConvert.ToString(this.entityCus["custompropertyvalue"]);
			}
			set
			{
				this.entityCus["custompropertyvalue"] = value;
			}
		}
		public string PicFile
		{
			get
			{
				return DataConvert.ToString(this.entityCus["picFile"]);
			}
			set
			{
				this.entityCus["picFile"] = value;
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
		public string Custompropertyvalueid
		{
			get
			{
				return DataConvert.ToString(this.entityCus["custompropertyvalueid"]);
			}
			set
			{
				this.entityCus["custompropertyvalueid"] = value;
			}
		}
		public string Custompropertyaliasname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["custompropertyaliasname"]);
			}
			set
			{
				this.entityCus["custompropertyaliasname"] = value;
			}
		}
		public string Custompropertyvaluename
		{
			get
			{
				return DataConvert.ToString(this.entityCus["Custompropertyvaluename"]);
			}
			set
			{
				this.entityCus["Custompropertyvaluename"] = value;
			}
		}
		public Sp_customPropertyValue()
		{
			this.entityCus = new EntityCustom("sp_customPropertyValue");
		}
		public Sp_customPropertyValue Clone()
		{
			return new Sp_customPropertyValue
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_customPropertyValue> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_customPropertyValue> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_customPropertyValue.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_customPropertyValue> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_customPropertyValue> list = new List<Sp_customPropertyValue>();
			Sp_customPropertyValue sp_customPropertyValue = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_customPropertyValue = new Sp_customPropertyValue();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_customPropertyValue.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_customPropertyValue);
			}
			return list;
		}
	}
}
