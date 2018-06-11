using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class Rule
	{
		public List<List<string[]>> Tables
		{
			get;
			set;
		}

		public string Desc
		{
			get;
			set;
		}

		public string Disclaimer
		{
			get;
			set;
		}

		public bool Empty
		{
			get;
			set;
		}

		public List<string> Images
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		public string Anchor
		{
			get;
			set;
		}
	}
}
