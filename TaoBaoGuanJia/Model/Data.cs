using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class Data
	{
		public ItemInfoModel ItemInfoModel
		{
			get;
			set;
		}

		public SkuModel SkuModel
		{
			get;
			set;
		}

		public List<Prop> Props
		{
			get;
			set;
		}

		public DescInfo DescInfo
		{
			get;
			set;
		}

		public Extras Extras
		{
			get;
			set;
		}

		public Seller Seller
		{
			get;
			set;
		}

		public List<ApiStackMobile> ApiStack
		{
			get;
			set;
		}
	}
}
