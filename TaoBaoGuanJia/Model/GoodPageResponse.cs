namespace TaoBaoGuanJia.Model
{
	public class GoodPageResponse : ApiResponseForHuoyuan
	{
		public PageDataViewForHuoyuan<GoodView> GoodViewPage
		{
			get;
			set;
		}
	}
}
