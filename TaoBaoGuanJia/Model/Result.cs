using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class Result
	{
		public Size Size
		{
			get;
			set;
		}

		public Parameter Parameter
		{
			get;
			set;
		}

		public ItemDesc ItemDesc
		{
			get;
			set;
		}

		public List<Modules> Modules
		{
			get;
			set;
		}
	}
}
