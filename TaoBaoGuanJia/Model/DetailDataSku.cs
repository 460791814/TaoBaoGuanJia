using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class DetailDataSku
	{
		public string discount
		{
			get;
			set;
		}

		public string discountPrice
		{
			get;
			set;
		}

		public string price
		{
			get;
			set;
		}

		public string retailPrice
		{
			get;
			set;
		}

		public string canBookCount
		{
			get;
			set;
		}

		public string saleCount
		{
			get;
			set;
		}

		public List<List<int>> priceRange
		{
			get;
			set;
		}

		public List<List<int>> priceRangeOriginal
		{
			get;
			set;
		}

		public List<SkuProp> skuProps
		{
			get;
			set;
		}

		public Dictionary<string, SkuMap> skuMap
		{
			get;
			set;
		}
	}
}
