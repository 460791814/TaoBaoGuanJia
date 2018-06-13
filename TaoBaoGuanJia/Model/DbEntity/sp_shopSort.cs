using TaoBaoGuanJia.Util;
using System;
using System.Collections.Generic;
using System.Data;


namespace TaoBaoGuanJia.Model
{
	public class Sp_shopSort : BaseEntity
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
		public int Shopsortid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["shopsortid"]);
			}
			set
			{
				this.entityCus["shopsortid"] = value;
			}
		}
		public string Shopsort
		{
			get
			{
				return DataConvert.ToString(this.entityCus["shopsort"]);
			}
			set
			{
				this.entityCus["shopsort"] = value;
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
		public Sp_shopSort()
		{
			this.entityCus = new EntityCustom("sp_shopSort");
		}
		public Sp_shopSort Clone()
		{
			return new Sp_shopSort
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_shopSort> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_shopSort> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_shopSort.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_shopSort> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_shopSort> list = new List<Sp_shopSort>();
			Sp_shopSort sp_shopSort = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_shopSort = new Sp_shopSort();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_shopSort.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_shopSort);
			}
			return list;
		}
	}
}
