using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class GoodListResponse : ApiResponseForHuoyuan
	{
		public List<GoodView> GoodViews
		{
			get;
			set;
		}
	}
}
