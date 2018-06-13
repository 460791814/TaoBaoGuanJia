
using System;
using System.Collections.Generic;
using System.Data;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Sp_item : BaseEntity
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
		public decimal MarketPrice
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["marketprice"]);
			}
			set
			{
				this.entityCus["marketprice"] = value;
			}
		}
		public string Province
		{
			get
			{
				return DataConvert.ToString(this.entityCus["province"]);
			}
			set
			{
				this.entityCus["province"] = value;
			}
		}
		public string Provincename
		{
			get
			{
				return DataConvert.ToString(this.entityCus["provincename"]);
			}
			set
			{
				this.entityCus["provincename"] = value;
			}
		}
		public string City
		{
			get
			{
				return DataConvert.ToString(this.entityCus["city"]);
			}
			set
			{
				this.entityCus["city"] = value;
			}
		}
		public string Cityname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["cityname"]);
			}
			set
			{
				this.entityCus["cityname"] = value;
			}
		}
		public string Photo
		{
			get
			{
				return DataConvert.ToString(this.entityCus["photo"]);
			}
			set
			{
				this.entityCus["photo"] = value;
			}
		}
		public string Selltype
		{
			get
			{
				return DataConvert.ToString(this.entityCus["selltype"]);
			}
			set
			{
				this.entityCus["selltype"] = value;
			}
		}
		public string Selltypename
		{
			get
			{
				return DataConvert.ToString(this.entityCus["selltypename"]);
			}
			set
			{
				this.entityCus["selltypename"] = value;
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
		public decimal Pricerise
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["pricerise"]);
			}
			set
			{
				this.entityCus["pricerise"] = value;
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
		public int Limitnums
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["limitnums"]);
			}
			set
			{
				this.entityCus["limitnums"] = value;
			}
		}
		public string Validdate
		{
			get
			{
				return DataConvert.ToString(this.entityCus["validdate"]);
			}
			set
			{
				this.entityCus["validdate"] = value;
			}
		}
		public string Shipway
		{
			get
			{
				return DataConvert.ToString(this.entityCus["shipway"]);
			}
			set
			{
				this.entityCus["shipway"] = value;
			}
		}
		public string Useshiptpl
		{
			get
			{
				return DataConvert.ToString(this.entityCus["useshiptpl"]);
			}
			set
			{
				this.entityCus["useshiptpl"] = value;
			}
		}
		public int Shiptplid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["shiptplid"]);
			}
			set
			{
				this.entityCus["shiptplid"] = value;
			}
		}
		public string Shipwayname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["shipwayname"]);
			}
			set
			{
				this.entityCus["shipwayname"] = value;
			}
		}
		public decimal Shipslow
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["shipslow"]);
			}
			set
			{
				this.entityCus["shipslow"] = value;
			}
		}
		public decimal Shipfast
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["shipfast"]);
			}
			set
			{
				this.entityCus["shipfast"] = value;
			}
		}
		public decimal Shipems
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["shipems"]);
			}
			set
			{
				this.entityCus["shipems"] = value;
			}
		}
		public string Onsell
		{
			get
			{
				return DataConvert.ToString(this.entityCus["onsell"]);
			}
			set
			{
				this.entityCus["onsell"] = value;
			}
		}
		public DateTime Onselldate
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["onselldate"]);
			}
			set
			{
				this.entityCus["onselldate"] = value;
			}
		}
		public string Onsellhour
		{
			get
			{
				return DataConvert.ToString(this.entityCus["onsellhour"]);
			}
			set
			{
				this.entityCus["onsellhour"] = value;
			}
		}
		public string Onsellmin
		{
			get
			{
				return DataConvert.ToString(this.entityCus["onsellmin"]);
			}
			set
			{
				this.entityCus["onsellmin"] = value;
			}
		}
		public string Paytype
		{
			get
			{
				return DataConvert.ToString(this.entityCus["paytype"]);
			}
			set
			{
				this.entityCus["paytype"] = value;
			}
		}
		public string Isrmd
		{
			get
			{
				return DataConvert.ToString(this.entityCus["isrmd"]);
			}
			set
			{
				this.entityCus["isrmd"] = value;
			}
		}
		public string Isreturn
		{
			get
			{
				return DataConvert.ToString(this.entityCus["isreturn"]);
			}
			set
			{
				this.entityCus["isreturn"] = value;
			}
		}
		public string Isticket
		{
			get
			{
				return DataConvert.ToString(this.entityCus["isticket"]);
			}
			set
			{
				this.entityCus["isticket"] = value;
			}
		}
		public string Ticketname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["ticketname"]);
			}
			set
			{
				this.entityCus["ticketname"] = value;
			}
		}
		public string Isrepair
		{
			get
			{
				return DataConvert.ToString(this.entityCus["isrepair"]);
			}
			set
			{
				this.entityCus["isrepair"] = value;
			}
		}
		public string Repairname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["repairname"]);
			}
			set
			{
				this.entityCus["repairname"] = value;
			}
		}
		public string Isautopub
		{
			get
			{
				return DataConvert.ToString(this.entityCus["isautopub"]);
			}
			set
			{
				this.entityCus["isautopub"] = value;
			}
		}
		public string Isvirtual
		{
			get
			{
				return DataConvert.ToString(this.entityCus["isvirtual"]);
			}
			set
			{
				this.entityCus["isvirtual"] = value;
			}
		}
		public string Sszgusername
		{
			get
			{
				return DataConvert.ToString(this.entityCus["sszgusername"]);
			}
			set
			{
				this.entityCus["sszgusername"] = value;
			}
		}
		public int Kcitemid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["kcitemid"]);
			}
			set
			{
				this.entityCus["kcitemid"] = value;
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
		public string Kcitemname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["kcitemname"]);
			}
			set
			{
				this.entityCus["kcitemname"] = value;
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
		public string Onlinekey
		{
			get
			{
				return DataConvert.ToString(this.entityCus["onlinekey"]);
			}
			set
			{
				this.entityCus["onlinekey"] = value;
			}
		}
		public DateTime Listtime
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["listtime"]);
			}
			set
			{
				this.entityCus["listtime"] = value;
			}
		}
		public DateTime Delisttime
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["delisttime"]);
			}
			set
			{
				this.entityCus["delisttime"] = value;
			}
		}
		public int Photomodified
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["photomodified"]);
			}
			set
			{
				this.entityCus["photomodified"] = value;
			}
		}
		public string Detailurl
		{
			get
			{
				return DataConvert.ToString(this.entityCus["detailurl"]);
			}
			set
			{
				this.entityCus["detailurl"] = value;
			}
		}
		public string Showurl
		{
			get
			{
				return DataConvert.ToString(this.entityCus["showurl"]);
			}
			set
			{
				this.entityCus["showurl"] = value;
			}
		}
		public string Shopsortids
		{
			get
			{
				return DataConvert.ToString(this.entityCus["shopsortids"]);
			}
			set
			{
				this.entityCus["shopsortids"] = value;
			}
		}
		public string Shopsortnames
		{
			get
			{
				return DataConvert.ToString(this.entityCus["shopsortnames"]);
			}
			set
			{
				this.entityCus["shopsortnames"] = value;
			}
		}
		public string Operatetypes
		{
			get
			{
				return DataConvert.ToString(this.entityCus["operatetypes"]);
			}
			set
			{
				this.entityCus["operatetypes"] = value;
			}
		}
		public DateTime Crtdate
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["crtdate"]);
			}
			set
			{
				this.entityCus["crtdate"] = value;
			}
		}
		public DateTime Modifydate
		{
			get
			{
				return DataConvert.ToDateTime(this.entityCus["modifydate"]);
			}
			set
			{
				this.entityCus["modifydate"] = value;
			}
		}
		public int Synchstate
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["synchstate"]);
			}
			set
			{
				this.entityCus["synchstate"] = value;
			}
		}
		public int Synchdetail
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["synchdetail"]);
			}
			set
			{
				this.entityCus["synchdetail"] = value;
			}
		}
		public int Del
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["del"]);
			}
			set
			{
				this.entityCus["del"] = value;
			}
		}
		public string Discount
		{
			get
			{
				return DataConvert.ToString(this.entityCus["discount"]);
			}
			set
			{
				this.entityCus["discount"] = value;
			}
		}
		public int Integrity
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["integrity"]);
			}
			set
			{
				this.entityCus["integrity"] = value;
			}
		}
		public decimal Weight
		{
			get
			{
				return DataConvert.ToDecimal(this.entityCus["weight"]);
			}
			set
			{
				this.entityCus["weight"] = value;
			}
		}
		public int Substock
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["substock"]);
			}
			set
			{
				this.entityCus["substock"] = value;
			}
		}
		public string Size
		{
			get
			{
				return DataConvert.ToString(this.entityCus["size"]);
			}
			set
			{
				this.entityCus["size"] = value;
			}
		}
		public int Aftersaleid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["aftersaleid"]);
			}
			set
			{
				this.entityCus["aftersaleid"] = value;
			}
		}
		public int Ispaipainewstock
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["ispaipainewstock"]);
			}
			set
			{
				this.entityCus["ispaipainewstock"] = value;
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
		public string SellPoint
		{
			get
			{
				return DataConvert.ToString(this.entityCus["sellpoint"]);
			}
			set
			{
				this.entityCus["sellpoint"] = value;
			}
		}
		public Sp_item()
		{
			this.entityCus = new EntityCustom("sp_item");
		}
		public Sp_item Clone()
		{
			return new Sp_item
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_item> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_item> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_item.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_item> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_item> list = new List<Sp_item>();
			Sp_item sp_item = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_item = new Sp_item();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_item.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_item);
			}
			return list;
		}
	}
}
