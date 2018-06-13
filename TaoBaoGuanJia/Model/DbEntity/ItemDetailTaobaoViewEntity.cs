
using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class ItemDetailTaobaoViewEntity
	{
		public Sp_item SpItem
		{
			get;
			set;
		}

		public IList<Sp_property> SpPropertyList
		{
			get;
			set;
		}

		public IList<Sp_sellProperty> SpSellPropertyList
		{
			get;
			set;
		}

		public IList<Sp_pictures> SpPicturesList
		{
			get;
			set;
		}

		public Sp_itemContent SpItemContent
		{
			get;
			set;
		}

		public Sp_sysSort SpSysSort
		{
			get;
			set;
		}

		public Sp_foodSecurity SpFodSecurity
		{
			get;
			set;
		}

		public IList<Sp_shopSort> SpShopSortList
		{
			get;
			set;
		}


	}
}
