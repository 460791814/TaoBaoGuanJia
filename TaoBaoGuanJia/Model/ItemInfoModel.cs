using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class ItemInfoModel
	{
		public string ItemId
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public List<string> picsPath
		{
			get;
			set;
		}

		public int Favcount
		{
			get;
			set;
		}

		public string StuffStatus
		{
			get;
			set;
		}

		public string ItemUrl
		{
			get;
			set;
		}

		public bool Sku
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		public string SaleLine
		{
			get;
			set;
		}

		public int CategoryId
		{
			get;
			set;
		}

		public bool IsMakeup
		{
			get;
			set;
		}

		public string ItemTypeName
		{
			get;
			set;
		}

		public string FreightPayer
		{
			get;
			set;
		}

		public string PostFee
		{
			get;
			set;
		}

		public string ExpressFee
		{
			get;
			set;
		}

		public string EmsFee
		{
			get;
			set;
		}
	}
}
