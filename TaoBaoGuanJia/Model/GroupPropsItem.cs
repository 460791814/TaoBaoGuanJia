using Newtonsoft.Json;
using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class GroupPropsItem
	{
		[JsonProperty(PropertyName = "������Ϣ")]
		public List<Dictionary<string, string>> BaseInfo
		{
			get;
			set;
		}
	}
}
