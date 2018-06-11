using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class ShopResponse : ApiResponseForHuoyuan
	{
		public List<Shop> Shops
		{
			get;
			set;
		}
	}
}
