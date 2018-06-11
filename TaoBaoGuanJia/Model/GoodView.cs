using System.Collections.Generic;
using Top.Api.Domain;

namespace TaoBaoGuanJia.Model
{
	public class GoodView
	{
		public Goods Goods
		{
			get;
			set;
		}

		public List<Propertys> GoodPropertys
		{
			get;
			set;
		}

		public List<GoodSKU> GoodSkus
		{
			get;
			set;
		}

		public List<GoodImage> GoodImages
		{
			get;
			set;
		}

		public FoodSecurity FoodSecurity
		{
			get;
			set;
		}
	}
}
