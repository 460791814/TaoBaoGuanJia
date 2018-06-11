using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class SkuProperies
	{
		public List<AttrList> attr_list
		{
			get;
			set;
		}

		public Dictionary<string, skuValue> sku
		{
			get;
			set;
		}
	}
}
