using TaoBaoGuanJia.Util;
using System;
using System.Collections.Generic;
using System.Data;
namespace TaoBaoGuanJia.Model
{
	public class Sp_itemContent : BaseEntity
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
		public string Content
		{
			get
			{
				return DataConvert.ToString(this.entityCus["content"]);
			}
			set
			{
				this.entityCus["content"] = value;
			}
		}
		public string Uploadfailmsg
		{
			get
			{
				return DataConvert.ToString(this.entityCus["uploadfailmsg"]);
			}
			set
			{
				this.entityCus["uploadfailmsg"] = value;
			}
		}
		public string Faultreason
		{
			get
			{
				return DataConvert.ToString(this.entityCus["faultreason"]);
			}
			set
			{
				this.entityCus["faultreason"] = value;
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
		public string Wirelessdesc
		{
			get
			{
				return DataConvert.ToString(this.entityCus["wirelessdesc"]);
			}
			set
			{
				this.entityCus["wirelessdesc"] = value;
			}
		}
		public Sp_itemContent()
		{
			this.entityCus = new EntityCustom("sp_itemContent");
		}
		public Sp_itemContent Clone()
		{
			return new Sp_itemContent
			{
				entityCus = this.entityCus.Clone()
			};
		}
		public static string ConnectValuesWithChar(IList<Sp_itemContent> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
		public static string ConnectValuesWithChar(IList<Sp_itemContent> list, string fieldName, WrapCharType wrapCharType, bool trim)
		{
			return Sp_itemContent.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
		}
		public static IList<Sp_itemContent> TransDataTableToEntityList(DataTable dt)
		{
			IList<Sp_itemContent> list = new List<Sp_itemContent>();
			Sp_itemContent sp_itemContent = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sp_itemContent = new Sp_itemContent();
				foreach (DataColumn dataColumn in dt.Columns)
				{
					sp_itemContent.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
				}
				list.Add(sp_itemContent);
			}
			return list;
		}
	}
}
