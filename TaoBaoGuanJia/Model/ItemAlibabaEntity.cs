using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class ItemAlibabaEntity
	{
		public string BeginAmount
		{
			get;
			set;
		}

		public string CanBookedAmount
		{
			get;
			set;
		}

		public string DetailUrl
		{
			get;
			set;
		}

		public List<DiscountPriceRange> DiscountPriceRanges
		{
			get;
			set;
		}

		public FreightInfo FreightInfo
		{
			get;
			set;
		}

		public List<ImageList> ImageList
		{
			get;
			set;
		}

		public string IsOfferSupportOnlineTrade
		{
			get;
			set;
		}

		public string IsProcessing
		{
			get;
			set;
		}

		public string IsSKUOffer
		{
			get;
			set;
		}

		public string IsSupportMix
		{
			get;
			set;
		}

		public string MemberId
		{
			get;
			set;
		}

		public string OfferId
		{
			get;
			set;
		}

		public string Price
		{
			get;
			set;
		}

		public string PriceDisplay
		{
			get;
			set;
		}

		public List<PriceRange> PriceRanges
		{
			get;
			set;
		}

		public string PriceUnit
		{
			get;
			set;
		}

		public List<ProductFeatureList> ProductFeatureList
		{
			get;
			set;
		}

		public string RateAverageStarLevel
		{
			get;
			set;
		}

		public string SaledCount
		{
			get;
			set;
		}

		public Dictionary<string, SkuMap> SkuMap
		{
			get;
			set;
		}

		public List<SkuProp> SkuProps
		{
			get;
			set;
		}

		public string Subject
		{
			get;
			set;
		}

		public string Unit
		{
			get;
			set;
		}

		public OfferSign OfferSign
		{
			get;
			set;
		}
	}
}
