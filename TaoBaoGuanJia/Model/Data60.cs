using Newtonsoft.Json;
using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class Data60
	{
		public List<ApiStackItem> apiStack
		{
			get;
			set;
		}

		public Item60 item
		{
			get;
			set;
		}

		public string mockData
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "params")]
		public Params60 Params
		{
			get;
			set;
		}

		public Props props
		{
			get;
			set;
		}

		public Props2 props2
		{
			get;
			set;
		}

		public Rate rate
		{
			get;
			set;
		}

		public Seller60 seller
		{
			get;
			set;
		}

		public SkuBase skuBase
		{
			get;
			set;
		}

		public Vertical vertical
		{
			get;
			set;
		}
	}
}
