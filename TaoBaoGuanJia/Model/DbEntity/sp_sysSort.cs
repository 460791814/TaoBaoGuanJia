using TaoBaoGuanJia.Util;
using System;
using System.Collections.Generic;
using System.Data;
namespace TaoBaoGuanJia.Model
{
	public class Sp_sysSort : BaseEntity
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
		public int Syssortid
		{
			get
			{
				return DataConvert.ToInt(this.entityCus["syssortid"]);
			}
			set
			{
				this.entityCus["syssortid"] = value;
			}
		}
		public string Syssortname
		{
			get
			{
				return DataConvert.ToString(this.entityCus["syssortname"]);
			}
			set
			{
				this.entityCus["syssortname"] = value;
			}
		}
		public string Syssortpath
		{
			get
			{
				return DataConvert.ToString(this.entityCus["syssortpath"]);
			}
			set
			{
				this.entityCus["syssortpath"] = value;
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
		public Sp_sysSort()
		{
			this.entityCus = new EntityCustom("sp_sysSort");
		}
		public Sp_sysSort Clone()
		{
			return new Sp_sysSort
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_sysSort> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_sysSort> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_sysSort.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_sysSort> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_sysSort> list = new List<Sp_sysSort>();
			Sp_sysSort sp_sysSort = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_sysSort = new Sp_sysSort();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_sysSort.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_sysSort);
			}
			return list;
		}
	}
}
