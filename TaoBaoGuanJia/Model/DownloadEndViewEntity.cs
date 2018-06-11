using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class DownloadEndViewEntity
	{
		public int TotalCount
		{
			get;
			set;
		}

		public int SuccessCount
		{
			get;
			set;
		}

		public int FailCount
		{
			get;
			set;
		}

		public List<string> ErrorMsgList
		{
			get;
			set;
		}

	
	}
}
