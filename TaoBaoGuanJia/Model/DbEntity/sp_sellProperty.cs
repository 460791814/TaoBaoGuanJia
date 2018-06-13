using TaoBaoGuanJia.Util;
using System;
using System.Collections.Generic;
using System.Data;
namespace TaoBaoGuanJia.Model
{
	public class Sp_sellProperty : BaseEntity
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
		public string Sellproinfos
		{
			get
			{
				return DataConvert.ToString(this.entityCus["sellproinfos"]);
			}
			set
			{
				this.entityCus["sellproinfos"] = value;
			}
		}
		public decimal Price
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["price"]);
			}
			set
			{
				this.entityCus["price"] = value;
			}
		}
		public int Nums
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["nums"]);
			}
			set
			{
				this.entityCus["nums"] = value;
			}
		}
		public string Code
		{
			get
			{
				return DataConvert.ToString(this.entityCus["code"]);
			}
			set
			{
				this.entityCus["code"] = value;
			}
		}
		public string Remark
		{
			get
			{
				return DataConvert.ToString(this.entityCus["remark"]);
			}
			set
			{
				this.entityCus["remark"] = value;
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
		public string Neworold
		{
			get
			{
				return DataConvert.ToString(this.entityCus["neworold"]);
			}
			set
			{
				this.entityCus["neworold"] = value;
			}
		}
		public decimal Saleprice
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["saleprice"]);
			}
			set
			{
				this.entityCus["saleprice"] = value;
			}
		}
		public DateTime Salepricebegindate
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["salepricebegindate"]);
			}
			set
			{
				this.entityCus["salepricebegindate"] = value;
			}
		}
		public DateTime Salepriceenddate
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["salepriceenddate"]);
			}
			set
			{
				this.entityCus["salepriceenddate"] = value;
			}
		}
		public string Standardproductid
		{
			get
			{
				return DataConvert.ToString(this.entityCus["standardproductid"]);
			}
			set
			{
				this.entityCus["standardproductid"] = value;
			}
		}
		public int State
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["state"]);
			}
			set
			{
				this.entityCus["state"] = value;
			}
		}
		public string Productidtype
		{
			get
			{
				return DataConvert.ToString(this.entityCus["productidtype"]);
			}
			set
			{
				this.entityCus["productidtype"] = value;
			}
		}
		public string Saleattr
		{
			get
			{
				return DataConvert.ToString(this.entityCus["saleattr"]);
			}
			set
			{
				this.entityCus["saleattr"] = value;
			}
		}
		public string Skuid
		{
			get
			{
				return DataConvert.ToString(this.entityCus["skuid"]);
			}
			set
			{
				this.entityCus["skuid"] = value;
			}
		}
		public string Barcode
		{
			get
			{
				return DataConvert.ToString(this.entityCus["barcode"]);
			}
			set
			{
				this.entityCus["barcode"] = value;
			}
		}
		public Sp_sellProperty()
		{
			this.entityCus = new EntityCustom("sp_sellProperty");
		}
		public Sp_sellProperty Clone()
		{
			return new Sp_sellProperty
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_sellProperty> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_sellProperty> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_sellProperty.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_sellProperty> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_sellProperty> list = new List<Sp_sellProperty>();
			Sp_sellProperty sp_sellProperty = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_sellProperty = new Sp_sellProperty();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_sellProperty.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_sellProperty);
			}
			return list;
		}
	}
}
