using System.Collections.Generic;
using Top.Api.Domain;

namespace TaoBaoGuanJia.Model
{
	public class ItemTaobaoEntity
	{
		public string Api
		{
			get;
			set;
		}

		public Data Data
		{
			get;
			set;
		}

		public List<Sku> SkuList
		{
			get;
			set;
		}
	}
}
