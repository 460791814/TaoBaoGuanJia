using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class PageDataViewForHuoyuan<T>
	{
		private int totalNum;

		private IList<T> itemsList;

		public int TotalNum
		{
			get
			{
				return totalNum;
			}
			set
			{
				totalNum = value;
			}
		}

		public IList<T> ItemsList
		{
			get
			{
				return itemsList;
			}
			set
			{
				itemsList = value;
			}
		}

		public int CurrentPage
		{
			get;
			set;
		}

		public int TotalPageCount
		{
			get;
			set;
		}

		public PageDataViewForHuoyuan()
		{
			itemsList = new List<T>();
		}
	}
}
