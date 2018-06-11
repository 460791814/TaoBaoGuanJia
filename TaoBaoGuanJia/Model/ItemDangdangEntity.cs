using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class ItemDangdangEntity
	{
		public Product_info_new Product_info_new
		{
			get;
			set;
		}

		public List<Product_desc_sorted> Product_desc_sorted
		{
			get;
			set;
		}

		public Product_attr product_attr
		{
			get;
			set;
		}
	}
}
