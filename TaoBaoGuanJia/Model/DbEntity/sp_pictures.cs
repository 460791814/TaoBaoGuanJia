using TaoBaoGuanJia.Util;
using System;
using System.Collections.Generic;
using System.Data;
namespace TaoBaoGuanJia.Model
{
	public class Sp_pictures : BaseEntity
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
		public string Localpath
		{
			get
			{
				return DataConvert.ToString(this.entityCus["localpath"]);
			}
			set
			{
				this.entityCus["localpath"] = value;
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
		public string Keys
		{
			get
			{
				return DataConvert.ToString(this.entityCus["keys"]);
			}
			set
			{
				this.entityCus["keys"] = value;
			}
		}
		public int Picindex
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["picindex"]);
			}
			set
			{
				this.entityCus["picindex"] = value;
			}
		}
		public int Ismodify
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["ismodify"]);
			}
			set
			{
				this.entityCus["ismodify"] = value;
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
		public Sp_pictures()
		{
			this.entityCus = new EntityCustom("sp_pictures");
		}
		public Sp_pictures Clone()
		{
			return new Sp_pictures
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_pictures> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_pictures> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_pictures.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_pictures> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_pictures> list = new List<Sp_pictures>();
			Sp_pictures sp_pictures = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_pictures = new Sp_pictures();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_pictures.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_pictures);
			}
			return list;
		}
	}
}
