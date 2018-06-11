using System.Collections.Generic;
using Top.Api.Domain;

namespace TaoBaoGuanJia.Model
{
	public class ItemJsonEntity60
	{
		public string api
		{
			get;
			set;
		}

		public string v
		{
			get;
			set;
		}

		public List<string> ret
		{
			get;
			set;
		}

		public Data60 data
		{
			get;
			set;
		}

		public string price
		{
			get;
			set;
		}

		public string discountPrice
		{
			get;
			set;
		}

		public List<Sku> SkuList
		{
			get;
			set;
		}

		public List<Sku> DiscountSkuList
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		public long num
		{
			get;
			set;
		}

		public string propertyAlias
		{
			get;
			set;
		}
	}
}
