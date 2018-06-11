using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class ItemPinDDEntity
	{
		public int cat_id
		{
			get;
			set;
		}

		public string goods_name
		{
			get;
			set;
		}

		public string goods_desc
		{
			get;
			set;
		}

		public int is_onsale
		{
			get;
			set;
		}

		public IList<PDDSku> Sku
		{
			get;
			set;
		}

		public IList<PDDGallery> gallery
		{
			get;
			set;
		}
	}
}
