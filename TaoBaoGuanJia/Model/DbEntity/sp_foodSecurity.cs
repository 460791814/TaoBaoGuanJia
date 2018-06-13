using TaoBaoGuanJia.Util;
using System;
using System.Collections.Generic;
using System.Data;
namespace TaoBaoGuanJia.Model
{
	public class Sp_foodSecurity : BaseEntity
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
		public long Itemid
		{
			get
			{
				return DataConvert.ToLong(this.entityCus["itemid"]);
			}
			set
			{
				this.entityCus["itemid"] = value;
			}
		}
		public string Prdlicenseno
		{
			get
			{
				return DataConvert.ToString(this.entityCus["prdlicenseno"]);
			}
			set
			{
				this.entityCus["prdlicenseno"] = value;
			}
		}
		public string Designcode
		{
			get
			{
				return DataConvert.ToString(this.entityCus["designcode"]);
			}
			set
			{
				this.entityCus["designcode"] = value;
			}
		}
		public string Factory
		{
			get
			{
				return DataConvert.ToString(this.entityCus["factory"]);
			}
			set
			{
				this.entityCus["factory"] = value;
			}
		}
		public string Factorysite
		{
			get
			{
				return DataConvert.ToString(this.entityCus["factorysite"]);
			}
			set
			{
				this.entityCus["factorysite"] = value;
			}
		}
		public string Contact
		{
			get
			{
				return DataConvert.ToString(this.entityCus["contact"]);
			}
			set
			{
				this.entityCus["contact"] = value;
			}
		}
		public string Mix
		{
			get
			{
				return DataConvert.ToString(this.entityCus["mix"]);
			}
			set
			{
				this.entityCus["mix"] = value;
			}
		}
		public string Planstorage
		{
			get
			{
				return DataConvert.ToString(this.entityCus["planstorage"]);
			}
			set
			{
				this.entityCus["planstorage"] = value;
			}
		}
		public string Period
		{
			get
			{
				return DataConvert.ToString(this.entityCus["period"]);
			}
			set
			{
				this.entityCus["period"] = value;
			}
		}
		public string Foodadditive
		{
			get
			{
				return DataConvert.ToString(this.entityCus["foodadditive"]);
			}
			set
			{
				this.entityCus["foodadditive"] = value;
			}
		}
		public string Supplier
		{
			get
			{
				return DataConvert.ToString(this.entityCus["supplier"]);
			}
			set
			{
				this.entityCus["supplier"] = value;
			}
		}
		public DateTime Productdatestart
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["productdatestart"]);
			}
			set
			{
				this.entityCus["productdatestart"] = value;
			}
		}
		public DateTime Productdateend
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["productdateend"]);
			}
			set
			{
				this.entityCus["productdateend"] = value;
			}
		}
		public DateTime Stockdatestart
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["stockdatestart"]);
			}
			set
			{
				this.entityCus["stockdatestart"] = value;
			}
		}
		public DateTime Stockdateend
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["stockdateend"]);
			}
			set
			{
				this.entityCus["stockdateend"] = value;
			}
		}
		public string HealthProductNo
		{
			get
			{
				return DataConvert.ToString(this.entityCus["healthproductno"]);
			}
			set
			{
				this.entityCus["healthproductno"] = value;
			}
		}
		public Sp_foodSecurity()
		{
			this.entityCus = new EntityCustom("sp_foodSecurity");
		}
		public Sp_foodSecurity Clone()
		{
			return new Sp_foodSecurity
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_foodSecurity> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_foodSecurity> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_foodSecurity.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_foodSecurity> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_foodSecurity> list = new List<Sp_foodSecurity>();
			Sp_foodSecurity sp_foodSecurity = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_foodSecurity = new Sp_foodSecurity();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_foodSecurity.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_foodSecurity);
			}
			return list;
		}
	}
}
