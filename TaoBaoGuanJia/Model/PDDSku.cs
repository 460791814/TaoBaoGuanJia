using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class PDDSku
	{
		public string sku_id
		{
			get;
			set;
		}

		public string thumb_url
		{
			get;
			set;
		}

		public int quantity
		{
			get;
			set;
		}

		public decimal normal_price
		{
			get;
			set;
		}

		public decimal group_price
		{
			get;
			set;
		}

		public decimal old_group_price
		{
			get;
			set;
		}

		public IList<PDDSpecs> specs
		{
			get;
			set;
		}
	}
}
