using Newtonsoft.Json;
using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class GroupPropsItem
	{
		[JsonProperty(PropertyName = "基本信息")]
		public List<Dictionary<string, string>> BaseInfo
		{
			get;
			set;
		}
	}
}
