using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class YouZSku
	{
		public IList<SkuTree> tree
		{
			get;
			set;
		}

		public IList<SkuList> list
		{
			get;
			set;
		}

		public string price
		{
			get;
			set;
		}

		public int stock_num
		{
			get;
			set;
		}
	}
}
