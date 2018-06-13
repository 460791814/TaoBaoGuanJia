
using System.Collections.Generic;

namespace TaoBaoGuanJia.Model
{
	public class DownloadItemInfoViewEntity
	{
		private bool downloadAllDetail = true;

		private bool onlyDownloadMajorImg;

		private bool onlyDownThumbnails;

		private bool isDownLoadMarketPrice;

		private bool isDownLoadCustomPicture;

		private bool recordDownload;

		public Sys_shop SourceShop
		{
			get;
			set;
		}

		public IList<string> OnlineKeyList
		{
			get;
			set;
		}

		public bool DownloadAllDetail
		{
			get
			{
				return downloadAllDetail;
			}
			set
			{
				downloadAllDetail = value;
			}
		}

		public bool OnlyDownloadMajorImg
		{
			get
			{
				return onlyDownloadMajorImg;
			}
			set
			{
				onlyDownloadMajorImg = value;
			}
		}

		public bool OnlyDownThumbnails
		{
			get
			{
				return onlyDownThumbnails;
			}
			set
			{
				onlyDownThumbnails = value;
			}
		}

		public bool IsDownLoadMarketPrice
		{
			get
			{
				return isDownLoadMarketPrice;
			}
			set
			{
				isDownLoadMarketPrice = value;
			}
		}

		public bool IsDownLoadCustomPicture
		{
			get
			{
				return isDownLoadCustomPicture;
			}
			set
			{
				isDownLoadCustomPicture = value;
			}
		}

		public bool RecordDownload
		{
			get
			{
				return recordDownload;
			}
			set
			{
				recordDownload = value;
			}
		}

		public Dictionary<string, string> dicOnlinekeyAndContent
		{
			get;
			set;
		}

	
	}
}
