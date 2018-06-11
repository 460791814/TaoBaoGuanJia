using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class FxItemDetail
	{
		public string itemId
		{
			get;
			set;
		}

		public string itemName
		{
			get;
			set;
		}

		public string itemStock
		{
			get;
			set;
		}

		public string itemPrice
		{
			get;
			set;
		}

		public List<skuValue> itemSkuList
		{
			get;
			set;
		}

		public List<string> imgDescription
		{
			get;
			set;
		}

		public List<ItemContent> itemContent
		{
			get;
			set;
		}

		public List<string> itemImgTitles
		{
			get;
			set;
		}
	}
}
