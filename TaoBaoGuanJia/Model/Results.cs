using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class Results
	{
		public string itemID
		{
			get;
			set;
		}

		public string vitemID
		{
			get;
			set;
		}

		public string stock
		{
			get;
			set;
		}

		public string itemName
		{
			get;
			set;
		}

		public string item_detail
		{
			get;
			set;
		}

		public string price
		{
			get;
			set;
		}

		public List<string> Imgs
		{
			get;
			set;
		}

		public List<string> thumbImgs
		{
			get;
			set;
		}

		public List<skuValue> sku
		{
			get;
			set;
		}

		public List<string> titles
		{
			get;
			set;
		}

		public List<AttrList> attr_list
		{
			get;
			set;
		}
	}
}
