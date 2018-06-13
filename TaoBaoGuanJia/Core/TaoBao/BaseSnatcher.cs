using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaoBaoGuanJia.Model;
using Top.Api.Response;

namespace TaoBaoGuanJia.Core.TaoBao
{
    public abstract class BaseSnatcher
    {
        public abstract int SysId
        {
            get;
        }
        public abstract ItemGetResponse SnatchItem(string itemOnlineKey, bool snatchPromotionPrice);
        public abstract IList<ProductInfo> SnatchItems(string shopNick, bool isTmall, long categoryId);
        public abstract IList<ProductInfo> SnatchItemsNew(string shopNick, bool isTmall, long categoryId, int pageNum, string shopId);
        public abstract int GetTotalPage(string shopNick, bool isTmall, long categoryId, out string shopId);
        public abstract string InvokeWebRequest(string url);
        public abstract string InvokeWebRequestNew(string url, string encode);
    }
}
