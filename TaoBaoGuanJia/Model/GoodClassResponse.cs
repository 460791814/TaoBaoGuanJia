using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class GoodClassResponse : ApiResponseForHuoyuan
	{
		public List<SysClass> SysClasses
		{
			get;
			set;
		}
	}
}
