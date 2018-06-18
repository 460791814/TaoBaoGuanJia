using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Management;
using System.IO;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace TaoBaoGuanJia.Util
{
    /// <summary>
    /// 常用工具类(UpdateTime:10.8.1017)
    /// </summary>
    public static class Utils
    {
        #region 缓存设置\获取
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <returns></returns>
        public static object GetApplication(string name)
        {
            return HttpContext.Current.Application[name];
        }
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <param name="value">缓存数据，任意字符串或对像</param>
        public static void SetApplication(string name, object value)
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application[name] = value;
            HttpContext.Current.Application.UnLock();
        }
        #endregion

        #region 缓存设置\获取 cache
        /// <summary>
        /// 从 System.Web.Caching.Cache 对象检索指定项
        /// </summary>
        /// <returns>要检索的缓存项的标识符</returns>
        public static object Cache(string key)
        {
            return System.Web.HttpRuntime.Cache.Get(key);
        }
        /// <summary>
        /// System.Web.Caching.Cache 对象插入项，该项带有一个缓存键引用其位置
        /// </summary>
        /// <param name="key">用于引用该项的缓存键</param>
        /// <param name="value">要插入缓存中的对象(null 表示移除)</param>
        public static void Cache(string key, object value)
        {
            if (value == null)
                System.Web.HttpRuntime.Cache.Remove(key);
            else
                System.Web.HttpRuntime.Cache.Insert(key, value);
        }



        /// <summary>
        /// System.Web.Caching.Cache 对象插入项，该项带有一个缓存键引用其位置
        /// </summary>
        /// <param name="key">用于引用该项的缓存键</param>
        /// <param name="value">要插入缓存中的对象</param>
        /// <param name="absoluteExpiration">所插入对象将到期并被从缓存中移除的时间。要避免可能的本地时间问题（例如从标准时间改为夏时制），请使用 System.DateTime.UtcNow 而不是 System.DateTime.Now 作为此参数值。如果使用绝对到期，则 slidingExpiration 参数必须为 System.Web.Caching.Cache.NoSlidingExpiration</param>
        public static void Cache(string key, object value, DateTime absoluteExpiration)
        {
            System.Web.HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// System.Web.Caching.Cache 对象插入项，该项带有一个缓存键引用其位置
        /// </summary>
        /// <param name="key">用于引用该项的缓存键</param>
        /// <param name="value">要插入缓存中的对象</param>
        /// <param name="dependencies">所插入对象的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含 null</param>
        /// <param name="absoluteExpiration">所插入对象将到期并被从缓存中移除的时间。要避免可能的本地时间问题（例如从标准时间改为夏时制），请使用 System.DateTime.UtcNow 而不是 System.DateTime.Now 作为此参数值。如果使用绝对到期，则 slidingExpiration 参数必须为 System.Web.Caching.Cache.NoSlidingExpiration</param>
        /// <param name="slidingExpiration">最后一次访问所插入对象时与该对象到期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。如果使用可调到期，则 absoluteExpiration 参数必须为 System.Web.Caching.Cache.NoAbsoluteExpiration</param>
        public static void Cache(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }
        #endregion

        #region 列表翻页HTML代码

        /// <summary>
        /// 获取列表页连接地址
        /// </summary>
        /// <param name="sModuleName"></param>
        /// <param name="sClassID"></param>
        /// <returns></returns>
        public static string GetListLinkUrl(string sModuleName, string sClassID)
        {
            return string.Format("/{0}-class-{1}.html", sModuleName.Substring(0, 1), sClassID);
        }
        /// <summary>
        /// 获取资料列表URL地址
        /// </summary>
        /// <param name="iClassID"></param>
        /// <param name="iChapterID"></param>
        /// <param name="iNodeID"></param>
        /// <param name="iSoftType"></param>
        /// <param name="iDownType"></param>
        /// <param name="iSoftArea"></param>
        /// <param name="iSoftVersion"></param>
        /// <param name="iSoftYear"></param>
        /// <param name="iIsFree"></param>
        /// <param name="iIsHot"></param>
        /// <param name="iIsElite"></param>
        /// <param name="iIsSchool"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iCurrentPage"></param>
        /// <returns></returns>
        public static string GetSoftLinkURL(int iClassID, int iChapterID, int iNodeID, int iSoftType, int iDownType, int iSoftArea, int iSoftVersion, int iSoftYear, int iIsFree, int iIsHot, int iIsElite, int iIsSchool, int iPageSize, int iPoint, int iCurrentPage)
        {
            return string.Format("/s-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}-p{14}.html", iClassID, iChapterID, iNodeID, iSoftType, iDownType, iSoftArea, iSoftVersion, iSoftYear, iIsFree, iIsHot, iIsElite, iIsSchool, iPageSize, iPoint, 1);
        }
        /// <summary>
        /// 获取资料URL路径，不带页码及[.html]
        /// </summary>
        /// <param name="iClassID"></param>
        /// <param name="iChapterID"></param>
        /// <param name="iNodeID"></param>
        /// <param name="iSoftType"></param>
        /// <param name="iDownType"></param>
        /// <param name="iSoftArea"></param>
        /// <param name="iSoftVersion"></param>
        /// <param name="iSoftYear"></param>
        /// <param name="iIsFree"></param>
        /// <param name="iIsHot"></param>
        /// <param name="iIsElite"></param>
        /// <param name="iIsSchool"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public static string GetSoftLinkPath(int iClassID, int iChapterID, int iNodeID, int iSoftType, int iDownType, int iSoftArea, int iSoftVersion, int iSoftYear, int iIsFree, int iIsHot, int iIsElite, int iIsSchool, int iPageSize, int iPoint)
        {
            return string.Format("/s-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}", iClassID, iChapterID, iNodeID, iSoftType, iDownType, iSoftArea, iSoftVersion, iSoftYear, iIsFree, iIsHot, iIsElite, iIsSchool, iPageSize, iPoint);
        }
        /// <summary>
        /// 获取当前文件带参数路径
        /// </summary>
        /// <returns>返回带参数路径，以 '?'或 '&amp;'结束</returns>
        public static string GetCurrentUrl()
        {
            StringBuilder UrlStr = new StringBuilder();
            UrlStr.Append(HttpContext.Current.Request.CurrentExecutionFilePath);
            UrlStr.Append("?");
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                UrlStr.AppendFormat("{0}={1}&amp;", name, HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name]));
            }
            return UrlStr.ToString();
        }
        /// <summary>
        /// 获取当前文件带参数路径
        /// </summary>
        /// <param name="rParas">排除参数</param>
        /// <returns>返回带参数路径，以 '?' 或 '&amp;'结束</returns>
        public static string GetCurrentUrl(string rParas)
        {
            rParas = rParas.ToLower();
            StringBuilder UrlStr = new StringBuilder();
            UrlStr.Append(HttpContext.Current.Request.CurrentExecutionFilePath);
            UrlStr.Append("?");
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                if (rParas.IndexOf(name.ToLower()) < 0)
                    UrlStr.AppendFormat("{0}={1}&amp;", name, HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name]));
            }
            return UrlStr.ToString();
        }
        /// <summary>
        /// 获取页面翻页代码
        /// </summary>
        /// <param name="DataSourceCount">记录总数</param>
        /// <param name="PageCount">页面总数</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页码</param>
        /// <param name="Nums">按钮数量</param>
        /// <returns></returns>
        public static string ShowPage(int DataSourceCount, int PageSize, int CurrentPage, int Nums)
        {
            return ShowPage(DataSourceCount, PageSize, CurrentPage, Nums, HttpContext.Current.Request.CurrentExecutionFilePath);
        }

        /// <summary>
        /// 用户中心订单列表分页
        /// </summary>
        public static string ShowPageUOrder(int DataSourceCount, int PageSize, int CurrentPage, int Nums)
        {
            string LinkPage = HttpContext.Current.Request.CurrentExecutionFilePath;

            int PageCount = DataSourceCount / PageSize;
            if ((DataSourceCount % PageSize) > 0) PageCount++;
            StringBuilder QueryStr = new StringBuilder();
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                if ((name + "").ToLower() != "pageindex")
                    QueryStr.Append("&amp;" + name + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name] + ""));
            }
            //设置当前页
            if (PageCount < 1) PageCount = 1;
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > PageCount) CurrentPage = PageCount;

            StringBuilder PageStr = new StringBuilder();
            int iBegin = CurrentPage;
            int iEnd = CurrentPage;
            int i = Nums;
            while (true)
            {
                if (iBegin > 1)
                {
                    iBegin--;
                    i--;
                }
                if ((iEnd < PageCount) && (i > 1))
                {
                    iEnd++;
                    i--;
                }
                if (((iBegin <= 1) && (iEnd >= PageCount)) || (i <= 1)) break;
            }


            /*
            <li><a href="javascript:;"><</a></li>
                        <li class="checked"><a href="javascript:;">1</a></li>
                        <li><a href="javascript:;">2</a></li>
                        <li><a href="javascript:;">3</a></li>
                        <li><a href="javascript:;">4</a></li>
                        <li><a href="javascript:;">5</a></li>
                        <li><span class="gl">……</span></li>
                        <li><a href="javascript:;">10</a></li>
                        <li><a href="javascript:;">></a></li>
                        <li class="pagination-options">
                            <span>跳转</span>
                            <input type="text" class="pagination-text" value="1">
                            <span>页</span>
                            <input type="submit" value="跳转" class="pagination-btn">
                        </li>
            */



            if (CurrentPage > 1) PageStr.Append("<li><a href=\"" + LinkPage + "?pageindex=" + (CurrentPage - 1) + QueryStr.ToString() + "\"><</a></li> ");
            if (iBegin > 1) PageStr.Append("<li><a href=\"" + LinkPage + "?pageindex=1" + QueryStr.ToString() + "\">1..</a></li> ");
            for (i = iBegin; i <= iEnd; i++)
            {
                if (i == CurrentPage)
                    PageStr.Append(" <li><a href=\"#\">" + i.ToString() + "</a></li> ");
                else
                    PageStr.Append(" <li><a href=\"" + LinkPage + "?pageindex=" + i.ToString() + QueryStr.ToString() + "\">" + i.ToString() + "</a></li> ");
            }
            if (iEnd < PageCount) PageStr.Append(" <li><a href=\"" + LinkPage + "?pageindex=" + PageCount.ToString() + QueryStr.ToString() + "\"> .." + PageCount.ToString() + "</a></li>");
            if (CurrentPage < PageCount) PageStr.Append(" <li><a href=\"" + LinkPage + "?pageindex=" + (CurrentPage + 1) + QueryStr.ToString() + "\">></a></li> ");

            return PageStr.ToString();
        }

        public static string ShowPageQsjt(int DataSourceCount, int PageSize, int CurrentPage, int Nums)
        {
            string LinkPage = HttpContext.Current.Request.CurrentExecutionFilePath;

            int PageCount = DataSourceCount / PageSize;
            if ((DataSourceCount % PageSize) > 0) PageCount++;
            StringBuilder QueryStr = new StringBuilder();
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                if ((name + "").ToLower() != "pageindex")
                    QueryStr.Append("&amp;" + name + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name] + ""));
            }
            //设置当前页
            if (PageCount < 1) PageCount = 1;
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > PageCount) CurrentPage = PageCount;

            StringBuilder PageStr = new StringBuilder();
            int iBegin = CurrentPage;
            int iEnd = CurrentPage;
            int i = Nums;
            while (true)
            {
                if (iBegin > 1)
                {
                    iBegin--;
                    i--;
                }
                if ((iEnd < PageCount) && (i > 1))
                {
                    iEnd++;
                    i--;
                }
                if (((iBegin <= 1) && (iEnd >= PageCount)) || (i <= 1)) break;
            }

            if (CurrentPage > 1) PageStr.Append("<a href=\"" + LinkPage + "?pageindex=" + (CurrentPage - 1) + QueryStr.ToString() + "\">上一页</a> ");
            if (iBegin > 1) PageStr.Append("<a href=\"" + LinkPage + "?pageindex=1" + QueryStr.ToString() + "\">1..</a> ");
            for (i = iBegin; i <= iEnd; i++)
            {
                if (i == CurrentPage)
                    PageStr.Append(" <a href=\"#\">" + i.ToString() + "</a> ");
                else
                    PageStr.Append(" <a href=\"" + LinkPage + "?pageindex=" + i.ToString() + QueryStr.ToString() + "\">" + i.ToString() + "</a> ");
            }
            if (iEnd < PageCount) PageStr.Append(" <a href=\"" + LinkPage + "?pageindex=" + PageCount.ToString() + QueryStr.ToString() + "\"> .." + PageCount.ToString() + "</a>");
            if (CurrentPage < PageCount) PageStr.Append(" <a href=\"" + LinkPage + "?pageindex=" + (CurrentPage + 1) + QueryStr.ToString() + "\">下一页</a> ");

            return PageStr.ToString();
        }
        public static string ShowPageForQsList(int DataSourceCount, int PageSize, int CurrentPage, int Nums)
        {
            string LinkPage = HttpContext.Current.Request.CurrentExecutionFilePath;
            int PageCount = DataSourceCount / PageSize;
            if ((DataSourceCount % PageSize) > 0) PageCount++;
            StringBuilder QueryStr = new StringBuilder();
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                if ((name + "").ToLower() != "pageindex")
                    QueryStr.Append("&amp;" + name + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name] + ""));
            }
            //设置当前页
            if (PageCount < 1) PageCount = 1;
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > PageCount) CurrentPage = PageCount;
            StringBuilder PageStr = new StringBuilder("");
            int iBegin = CurrentPage;
            int iEnd = CurrentPage;
            int i = Nums;
            while (true)
            {
                if (iBegin > 1)
                {
                    iBegin--;
                    i--;
                }
                if ((iEnd < PageCount) && (i > 1))
                {
                    iEnd++;
                    i--;
                }
                if (((iBegin <= 1) && (iEnd >= PageCount)) || (i <= 1)) break;
            }
            PageStr.Append("<p>共&nbsp;&nbsp;<span>" + DataSourceCount + "</span>&nbsp;&nbsp;&nbsp;件商品&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"red\">" + CurrentPage + "</span></p>");

            PageStr.Append("<div class=\"rss-page clearfix\">");
            if (CurrentPage > 1)
            {
                PageStr.Append(" <a class=\"sprve\"  href=\"" + LinkPage + "?pageindex=" + (CurrentPage - 1) + QueryStr.ToString() + "\" title=\"上一页\" ></a> ");
            }
            else
            {
                PageStr.Append(" <a class=\"sprve\" disabled=\"disabled\"  href=\"" + LinkPage + "?pageindex=" + (CurrentPage - 1) + QueryStr.ToString() + "\" title=\"上一页\"></a> ");
            }


            if (CurrentPage < PageCount)
            {
                PageStr.Append(" <a class=\"snext\" href=\"" + LinkPage + "?pageindex=" + (CurrentPage + 1) + QueryStr.ToString() + "\" title=\"下一页\"></a>");
            }
            else
            {
                PageStr.Append(" <a class=\"snext\" disabled=\"disabled\" href=\"" + LinkPage + "?pageindex=" + (CurrentPage + 1) + QueryStr.ToString() + "\" title=\"下一页\"></a>");
            }
            PageStr.Append("</div>");
            PageStr.Append("");
            return PageStr.ToString();
        }

        /// <summary>
        /// 获取页面翻页代码
        /// </summary>
        /// <param name="DataSourceCount">记录总数</param>
        /// <param name="PageCount">页面总数</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页码</param>
        /// <param name="Nums">按钮数量</param>
        /// <param name="LinkPage">连接页面</param>
        /// <returns></returns>
        public static string ShowPage(int DataSourceCount, int PageSize, int CurrentPage, int Nums, string LinkPage)
        {
            int PageCount = DataSourceCount / PageSize;
            if ((DataSourceCount % PageSize) > 0) PageCount++;
            StringBuilder QueryStr = new StringBuilder();
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                if ((name + "").ToLower() != "pageindex")
                    QueryStr.Append("&amp;" + name + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name] + ""));
            }
            //设置当前页
            if (PageCount < 1) PageCount = 1;
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > PageCount) CurrentPage = PageCount;
            StringBuilder PageStr = new StringBuilder("<ul class=\"pagination\">");
            int iBegin = CurrentPage;
            int iEnd = CurrentPage;
            int i = Nums;
            while (true)
            {
                if (iBegin > 1)
                {
                    iBegin--;
                    i--;
                }
                if ((iEnd < PageCount) && (i > 1))
                {
                    iEnd++;
                    i--;
                }
                if (((iBegin <= 1) && (iEnd >= PageCount)) || (i <= 1)) break;
            }
            //PageStr.Append("<span class=\"rowscount\" title=\"每次最多显示10000条记录\">总数 " + DataSourceCount + "</span>");


            if (CurrentPage > 1) PageStr.Append("<li class=\"paginate_button\"><a  href=\"" + LinkPage + "?pageindex=" + (CurrentPage - 1) + QueryStr.ToString() + "\">上一页</a></li> ");
            if (iBegin > 1) PageStr.Append(" <li class=\"paginate_button\"> <a class=\"page\" href=\"" + LinkPage + "?pageindex=1" + QueryStr.ToString() + "\">1..</a></li>");
            for (i = iBegin; i <= iEnd; i++)
            {
                if (i == CurrentPage)
                    PageStr.Append(" <li class=\"paginate_button active\"><a href=\"#\">" + i.ToString() + "</a></li> ");
                else
                    PageStr.Append("<li class=\"paginate_button\"> <a   href=\"" + LinkPage + "?pageindex=" + i.ToString() + QueryStr.ToString() + "\">" + i.ToString() + "</a></li> ");
            }

            if (iEnd < PageCount) PageStr.Append(" <li class=\"paginate_button\"><a class=\"page\" href=\"" + LinkPage + "?pageindex=" + PageCount.ToString() + QueryStr.ToString() + "\"> .." + PageCount.ToString() + "</a></li>");
            if (CurrentPage < PageCount) PageStr.Append("<li class=\"paginate_button\"> <a class=\"nextpage\" href=\"" + LinkPage + "?pageindex=" + (CurrentPage + 1) + QueryStr.ToString() + "\">下一页</a></li> ");
            //PageStr.Append(" <span class=\"jump\"><input name=\"page\" size=\"5\" type=\"text\" value=\"" + (CurrentPage + 1).ToString() + "\" class=\"pageinput\" maxlength=\"9\" /> <input type=\"button\" value=\"跳转\" class=\"gobutton\" onclick=\"window.location.href='" + LinkPage + "?page='+this.form.page.value+'" + QueryStr.ToString() + "'\" /></span>");
            PageStr.Append("</ul>");
            return PageStr.ToString();
        }


        /// <summary>
        /// 生成翻页代码
        /// </summary>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="btnCount">按钮数量</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="sLinkUrl">跳转地址</param>
        /// <returns></returns>
        public static string ShowPageV(int totalPageCount, int btnCount, int currentPageIndex, string sLinkUrl)
        {
            int proPage = currentPageIndex - 1;
            int nextPage = proPage + 2;
            if (proPage < 1)
            {
                proPage = 1;
            }
            if (nextPage > totalPageCount)
            {
                nextPage = totalPageCount;
            }
            if (totalPageCount < 1 || btnCount < 1)
            {
                return "";
            }
            int start = currentPageIndex - (int)(Math.Ceiling(Convert.ToDouble(btnCount / 2)) - 1);
            if (btnCount < totalPageCount)
            {
                if (start < 1)
                {
                    start = 1;
                }
                else if (start + btnCount > totalPageCount)
                {
                    start = totalPageCount - btnCount + 1;
                }
            }
            else
            {
                start = 1;
            }
            int end = start + btnCount - 1;
            if (end > totalPageCount)
            {
                end = totalPageCount;
            }
            StringBuilder newNumberStr = new StringBuilder();
            if (currentPageIndex <= 1)
            {
                newNumberStr.Append("<a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">首页</a><a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">上一页</a>");
            }
            else
            {
                newNumberStr.AppendFormat("<a href=\"{0}-p1.html\">首页</a><a href=\"{0}-p{1}.html\">上一页</a>", sLinkUrl, proPage);
            }
            for (var i = start; i <= end; i++)
            {

                if (i == currentPageIndex)
                {
                    newNumberStr.AppendFormat("<a href=\"javascript:void(0);\" class=\"current\">" + i + "</a>");
                }
                else
                {
                    newNumberStr.AppendFormat("<a href=\"{0}-p{1}.html\">" + i + "</a>", sLinkUrl, i);
                }
            }
            if (currentPageIndex == totalPageCount)
            {
                newNumberStr.Append("<a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">下一页</a><a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">末页</a>");
            }
            else
            {
                newNumberStr.AppendFormat("<a href=\"{0}-p{1}.html\">下一页</a><a href=\"{0}-p{2}.html\">末页</a>", sLinkUrl, nextPage, totalPageCount);
            }
            newNumberStr.AppendFormat("<a href=\"javascript:void(0);\" class=\"ln\">共{0}页</a>", totalPageCount);
            if (totalPageCount > 1)
            {
                return newNumberStr.ToString();
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 生成翻页代码
        /// </summary>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="pageSize">按钮数量</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <returns></returns>
        public static string ShowPageT(int totalPageCount, int btnCount, int currentPageIndex)
        {
            return ShowPageT(totalPageCount, btnCount, currentPageIndex, HttpContext.Current.Request.CurrentExecutionFilePath);
        }
        /// <summary>
        /// 生成翻页代码
        /// </summary>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="btnCount">按钮数量</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="sLinkUrl">跳转地址</param>
        /// <returns></returns>
        public static string ShowPageT(int totalPageCount, int btnCount, int currentPageIndex, string sLinkUrl)
        {
            StringBuilder QueryStr = new StringBuilder();
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                if ((name + "").ToLower() != "page")
                    QueryStr.Append("&amp;" + name + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name] + ""));
            }
            int proPage = currentPageIndex - 1;
            int nextPage = proPage + 2;
            if (proPage < 1)
            {
                proPage = 1;
            }
            if (nextPage > totalPageCount)
            {
                nextPage = totalPageCount;
            }
            if (totalPageCount < 1 || btnCount < 1)
            {
                return "";
            }
            int start = currentPageIndex - (int)(Math.Ceiling(Convert.ToDouble(btnCount / 2)) - 1);
            if (btnCount < totalPageCount)
            {
                if (start < 1)
                {
                    start = 1;
                }
                else if (start + btnCount > totalPageCount)
                {
                    start = totalPageCount - btnCount + 1;
                }
            }
            else
            {
                start = 1;
            }
            int end = start + btnCount - 1;
            if (end > totalPageCount)
            {
                end = totalPageCount;
            }
            StringBuilder newNumberStr = new StringBuilder();
            if (currentPageIndex <= 1)
            {
                newNumberStr.Append("<a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">首页</a><a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">上一页</a>");
            }
            else
            {
                newNumberStr.AppendFormat("<a href=\"{0}?page=1{1}\">首页</a><a href=\"{0}?page={2}{1}\">上一页</a>", sLinkUrl, QueryStr.ToString(), proPage);
            }
            for (var i = start; i <= end; i++)
            {

                if (i == currentPageIndex)
                {
                    newNumberStr.AppendFormat("<a href=\"javascript:void(0);\" class=\"current\">" + i + "</a>");
                }
                else
                {
                    newNumberStr.AppendFormat("<a href=\"{0}?page={1}{2}\">" + i + "</a>", sLinkUrl, i, QueryStr.ToString());
                }
            }
            if (currentPageIndex == totalPageCount)
            {
                newNumberStr.Append("<a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">下一页</a><a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"disabled\">末页</a>");
            }
            else
            {
                newNumberStr.AppendFormat("<a href=\"{0}?page={2}{1}\">下一页</a><a href=\"{0}?page={3}{1}\">末页</a>", sLinkUrl, QueryStr.ToString(), nextPage, totalPageCount);
            }
            newNumberStr.AppendFormat("<a href=\"javascript:void(0);\" class=\"ln\">共{0}页</a>", totalPageCount);
            if (totalPageCount > 1)
            {
                return newNumberStr.ToString();
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 获取页面翻页代码(float:Right)
        /// </summary>
        /// <param name="DataSourceCount">记录总数</param>
        /// <param name="PageCount">页面总数</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页码</param>
        /// <param name="Nums">按钮数量</param>
        /// <returns></returns>
        public static string ShowPageRt(int DataSourceCount, int PageCount, int PageSize, int CurrentPage, int Nums)
        {
            return ShowPageRt(DataSourceCount, PageCount, PageSize, CurrentPage, Nums, HttpContext.Current.Request.CurrentExecutionFilePath);
        }
        /// <summary>
        /// 获取页面翻页代码(float:Right)
        /// </summary>
        /// <param name="DataSourceCount">记录总数</param>
        /// <param name="PageCount">页面总数</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页码</param>
        /// <param name="Nums">按钮数量</param>
        /// <returns></returns>
        public static string ShowPageRi(int DataSourceCount, int PageCount, int PageSize, int CurrentPage, int Nums)
        {
            return ShowPageRt(DataSourceCount, PageCount, PageSize, CurrentPage, Nums, HttpContext.Current.Request.CurrentExecutionFilePath);
        }
        /// <summary>
        /// 获取页面翻页代码(float:Right)
        /// </summary>
        /// <param name="DataSourceCount">记录总数</param>
        /// <param name="PageCount">页面总数</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页码</param>
        /// <param name="Nums">按钮数量</param>
        /// <param name="LinkPage">连接页面</param>
        /// <returns></returns>
        public static string ShowPageRi(int DataSourceCount, int PageCount, int PageSize, int CurrentPage, int Nums, string LinkPage)
        {
            return ShowPageRt(DataSourceCount, PageCount, PageSize, CurrentPage, Nums, LinkPage);
        }
        /// <summary>
        /// 获取页面翻页代码(float:Right)
        /// </summary>
        /// <param name="DataSourceCount">记录总数</param>
        /// <param name="PageCount">页面总数</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页码</param>
        /// <param name="Nums">按钮数量</param>
        /// <param name="LinkPage">连接页面</param>
        /// <returns></returns>
        public static string ShowPageRt(int DataSourceCount, int PageCount, int PageSize, int CurrentPage, int Nums, string LinkPage)
        {
            StringBuilder QueryStr = new StringBuilder();
            string sNextPageNo = "";//跳转的页码
            foreach (string name in HttpContext.Current.Request.QueryString)
            {
                if ((name + "").ToLower() != "page")
                    QueryStr.Append("&amp;" + name + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.QueryString[name] + ""));
            }
            //设置当前页
            if (PageCount < 1) PageCount = 1;
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > PageCount) CurrentPage = PageCount;
            StringBuilder PageStr = new StringBuilder("<form name=\"PageForm\" style=\"margin: 0px; padding: 0px;\" onsubmit=\"return false;\">");
            int iBegin = CurrentPage;
            int iEnd = CurrentPage;
            int i = Nums;
            while (true)
            {
                if (iBegin > 1)
                {
                    iBegin--;
                    i--;
                }
                if ((iEnd < PageCount) && (i > 1))
                {
                    iEnd++;
                    i--;
                }
                if (((iBegin <= 1) && (iEnd >= PageCount)) || (i <= 1)) break;
            }
            #region//跳转页的页码
            if (CurrentPage >= PageCount)
                sNextPageNo = "";
            else
                sNextPageNo = (CurrentPage + 1).ToString();
            #endregion
            PageStr.Append(" <span class=\"jump\"><input id=\"txtpage\" name=\"page\" size=\"5\" type=\"text\" value=\"" + sNextPageNo + "\" class=\"pageinput\" maxlength=\"9\" /> <input type=\"button\" value=\"跳转\" class=\"gobutton\" onclick=\"if($('#txtpage').val()=='')  {alert('请输入要跳转的页码');} else{ window.location.href='" + LinkPage + "?page='+this.form.page.value+'" + QueryStr.ToString() + "'}\" /></span>");
            if (iEnd < PageCount) PageStr.Append(" <a class=\"page\" href=\"" + LinkPage + "?page=" + PageCount.ToString() + QueryStr.ToString() + "\">.." + PageCount.ToString() + "</a>");
            //if (CurrentPage < PageCount) PageStr.Append(" <a class=\"nextpage\" href=\"" + LinkPage + "?page=" + (CurrentPage + 1) + QueryStr.ToString() + "\">下页</a> ");
            for (i = iEnd; i >= iBegin; i--)
            {
                if (i == CurrentPage)
                    PageStr.Append(" <span class=\"page_on\">" + i.ToString() + "</span> ");
                else
                    PageStr.Append(" <a class=\"page\" href=\"" + LinkPage + "?page=" + i.ToString() + QueryStr.ToString() + "\">" + i.ToString() + "</a> ");
            }
            //if (CurrentPage > 1) PageStr.Append(" <a class=\"prepage\" href=\"" + LinkPage + "?page=" + (CurrentPage - 1) + QueryStr.ToString() + "\">上页</a> ");
            if (iBegin > 1) PageStr.Append(" <a class=\"page\" href=\"" + LinkPage + "?page=1" + QueryStr.ToString() + "\">1..</a>");
            //PageStr.Append("<span class=\"rowscount\" title=\"每次最多显示10000条记录\">总数 " + DataSourceCount + "</span>");
            PageStr.Append("</form>");
            return PageStr.ToString();
        }
        #endregion

        #region 字符过滤\截取\统计\获取\输出


        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }

        /// <summary>
        /// 是否包含过滤符号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool InFilterPunc(string str)
        {
            string[] puncs = File.ReadAllText(MapPath("/Config/filterPunc.cfg")).Split('|');
            str = str.Replace(" ", "");
            foreach (string punc in puncs)
            {
                if (str.IndexOf(punc) > 0) return true;
            }
            return false;
        }
        /// <summary>
        /// 是否包含过滤字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool InFilterWord(string str)
        {
            string[] words = File.ReadAllText(MapPath("/Config/filterWord.cfg")).Split('|');
            str = str.Replace(" ", "");
            foreach (string word in words)
            {
                if (str.IndexOf(word) > 0) return true;
            }
            return false;
        }
        /// <summary>
        /// 过滤SQL语句查询
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string CheckStr(string Str)
        {
            return Str.Replace("'", "''");
        }
        /// <summary>
        /// 过滤导致JSON错误的字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ReplaceJsonBadWord(string Str)
        {
            StringBuilder KeyStr = new StringBuilder(Str);
            KeyStr.Replace("\\", "\\\\");
            KeyStr.Replace("\"", "\\\"");
            KeyStr.Replace("\n", "<br />");
            KeyStr.Replace("\r", "");
            return KeyStr.ToString();
        }
        /// <summary>
        /// 过滤会导致XML错误的字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ReplaceXmlBadWord(string Str)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in Str)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.Append(" ");//info.AppendFormat(" ", ss);
                else info.Append(cc);
            }
            return HttpContext.Current.Server.HtmlEncode(info.ToString() + "");
        }
        /// <summary>
        /// @把一个字符串中的 低序位 ASCII 字符 替换成[&amp;#x]字符
        /// @转换 ASCII 0 - 8 -> &amp;#x0 - &amp;#x8
        /// @转换 ASCII 11 - 12 -> &amp;#xB - &amp;#xC
        /// @转换 ASCII 14 - 31 -> &amp;#xE - &amp;#x1F
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat("&#x{0:X};", ss);
                else info.Append(cc);
            }
            return info.ToString();
        }
        /// <summary>
        /// @把一个字符串中的下列字符替换成 低序位 ASCII 字符
        /// @转换 &amp;#x0- &amp;#x8 -> ASCII 0 - 8
        /// @转换 &amp;#xB - &amp;#xC -> ASCII 11 - 12
        /// @转换 &amp;#xE - &amp;#x1F -> ASCII 14 - 31
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetLowOrderASCIICharacters(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            int pos, startIndex = 0, len = input.Length;
            if (len <= 4) return input;
            StringBuilder result = new StringBuilder();
            while ((pos = input.IndexOf("&#x", startIndex)) >= 0)
            {
                bool needReplace = false;
                string rOldV = string.Empty, rNewV = string.Empty;
                int le = (len - pos < 6) ? len - pos : 6;
                int p = input.IndexOf(";", pos, le);
                if (p >= 0)
                {
                    rOldV = input.Substring(pos, p - pos + 1);
                    // 计算 对应的低位字符
                    short ss;
                    if (short.TryParse(rOldV.Substring(3, p - pos - 3), System.Globalization.NumberStyles.AllowHexSpecifier, null, out ss))
                    {
                        if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                        {
                            needReplace = true;
                            rNewV = Convert.ToChar(ss).ToString();
                        }
                    }
                    pos = p + 1;
                }
                else pos += le;
                string part = input.Substring(startIndex, pos - startIndex);
                if (needReplace) result.Append(part.Replace(rOldV, rNewV));
                else result.Append(part);
                startIndex = pos;
            }
            result.Append(input.Substring(startIndex));
            return result.ToString();
        }
        /// <summary>
        /// 过滤SQL语句查询
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ReplaceSQL(string Str)
        {
            return Str.Replace("'", "''");
        }
        /// <summary>
        /// 格式化资料简介
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ReplaceIntroCrlf(string Str)
        {
            StringBuilder KeyStr = new StringBuilder(Str);
            KeyStr.Replace("\n", "<br />");
            KeyStr.Replace("\r", "");
            return KeyStr.ToString();
        }
        /// <summary>
        /// 过滤搜索关键词特殊字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ReplaceSearchKey(string Str)
        {
            string[] TempWord = { "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "{", "}", "|", "\"", "<", ">", "?", "｀", "～", "！", "＠", "＃", "＄", "％", "＾", "＆", "＊", "（", "）", "＿", "＋", "｛", "｝", "｜", "：", "＂", "“", "”", "＜", "＞", "？", "=", "[", "]", "\\", ";", "'", ",", ".", "/", "－", "＝", "［", "］", "＼", "；", "＇", "，", "．", "／", "", "", "" };
            StringBuilder KeyStr = new StringBuilder(Str);
            foreach (string St in TempWord)
            {
                if (Str.IndexOf(St) > -1)
                {
                    KeyStr.Replace(St, " ");
                }
            }
            System.Text.RegularExpressions.Regex Re = new System.Text.RegularExpressions.Regex(" +");
            return Re.Replace(KeyStr.ToString(), " ");
        }
        /// <summary>
        /// 过滤非法或特殊字符(允许：@-_.)
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ReplaceQuery(string Str)
        {
            string[] TempWord = { "`", "~", "!", "#", "$", "%", "^", "&", "*", "(", ")", "+", "{", "}", "|", ":", "\"", "<", ">", "?", "｀", "～", "！", "＠", "＃", "＄", "％", "＾", "＆", "＊", "（", "）", "＿", "＋", "｛", "｝", "｜", "：", "＂", "“", "”", "＜", "＞", "？", "=", "[", "]", "\\", ";", "'", ",", "/", "－", "＝", "［", "］", "＼", "；", "＇", "，", "．", "／", "", "", "" };
            StringBuilder KeyStr = new StringBuilder(Str);
            foreach (string St in TempWord)
            {
                if (Str.IndexOf(St) > -1)
                {
                    KeyStr.Replace(St, "");
                }
            }
            return KeyStr.ToString();
        }
        /// <summary>
        /// 判断是否含有非法特殊字符(允许：@-_.)
        /// </summary>
        /// <param name="Str"></param>
        /// <returns>True/False</returns>
        public static bool IncSpecialWord(string Str)
        {
            string[] TempWord = { "`", "~", "!", "#", "$", "%", "^", "&", "*", "(", ")", "+", "{", "}", "|", ":", "\"", "<", ">", "?", "｀", "～", "！", "＠", "＃", "＄", "％", "＾", "＆", "＊", "（", "）", "＿", "＋", "｛", "｝", "｜", "：", "＂", "“", "”", "＜", "＞", "？", "=", "[", "]", "\\", ";", "'", ",", "/", "－", "＝", "［", "］", "＼", "；", "＇", "，", "．", "／", "", "", "", " " };
            foreach (string St in TempWord)
            {
                if (Str.IndexOf(St) > -1)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 统计字符串长度，中文按2个字符计算
        /// </summary>
        /// <param name="oString">字符串</param>
        /// <returns></returns>
        public static int Length(string oString)
        {
            byte[] strArray = System.Text.Encoding.Default.GetBytes(oString);
            return strArray.Length;
        }

        /// <summary>
        /// 截取字符串，中文按2个字符计算
        /// </summary>
        /// <param name="Str">字符串</param>
        /// <param name="intLen">截取长度</param>
        /// <returns></returns>
        public static string CutString(string Str, int intLen)
        {
            if (string.IsNullOrEmpty(Str)) return "";
            int iStrLen = Str.Length;
            decimal c = 0;
            int t = 0;
            for (int i = 0; i < iStrLen; i++)
            {
                c = Math.Abs(char.ConvertToUtf32(Str.Substring(i, 1), 0));
                if (c > 255)
                {
                    t = t + 2;
                }
                else
                {
                    t++;
                }
                if (t >= intLen)
                {
                    return Str.Substring(0, i) + ".";
                }
            }
            return Str;
        }
        /// <summary>
        /// 截取字符串，中文按2个字符计算
        /// </summary>
        /// <param name="Str">字符串</param>
        /// <param name="intLen">截取长度</param>
        /// <param name="pre">替换字符(替换截取掉的字符串)</param>
        /// <returns></returns>
        public static string CutString(string Str, int intLen, string pre)
        {
            if (string.IsNullOrEmpty(Str)) return "";
            int iStrLen = Str.Length;
            decimal c = 0;
            int t = 0;
            for (int i = 0; i < iStrLen; i++)
            {
                c = Math.Abs(char.ConvertToUtf32(Str.Substring(i, 1), 0));
                if (c > 255)
                {
                    t = t + 2;
                }
                else
                {
                    t++;
                }
                if (t >= intLen)
                {
                    return Str.Substring(0, i) + pre;
                }
            }
            return Str;
        }
        /// <summary>
        /// 格式化字符串字体
        /// </summary>
        /// <param name="sString">字符串</param>
        /// <param name="sType">字体类型</param>
        /// <returns></returns>
        public static string StringFont(string sString, string sType)
        {
            switch (sType)
            {
                case "1": return "<strong>" + sString + "</strong>";
                case "2": return "<em>" + sString + "</em>";
                case "3": return "<strong><em>" + sString + "</em></strong>";
                case "4": return "<u>" + sString + "</u>";
                case "5": return "<strong><u>" + sString + "</u></strong>";
                case "6": return "<em><u>" + sString + "</u></em>";
                case "7": return "<strong><em><u>" + sString + "</u></em></strong>";
                default: return sString;
            }
        }
        /// <summary>
        /// 格式化字符串颜色
        /// </summary>
        /// <param name="sString">字符串</param>
        /// <param name="sColor">颜色</param>
        /// <returns></returns>
        public static string StringColor(string sString, string sColor)
        {
            if (string.IsNullOrEmpty(sColor)) return sString;
            return "<span style=\"color:" + sColor + ";\">" + sString + "</span>";
        }
        /// <summary>
        /// 获取Request集合内容
        /// </summary>
        /// <param name="ObjName">集合名称</param>
        /// <returns>获取数据/null</returns>
        public static string Request(string ObjName)
        {
            return HttpContext.Current.Request[ObjName];
        }
        /// <summary>
        /// 获取Get集合内容
        /// </summary>
        /// <param name="ObjName">集合名称</param>
        /// <returns>获取数据/null</returns>
        public static string QueryString(string ObjName)
        {
            return HttpContext.Current.Request.QueryString[ObjName];
        }
        /// <summary>
        /// 获取Get集合内容并返回int
        /// </summary>
        /// <param name="ObjName"></param>
        /// <returns></returns>
        public static int QueryStringInt(string ObjName)
        {
            return GetInt(HttpContext.Current.Request.QueryString[ObjName]);
        }
        /// <summary>
        /// 获取带中文Get集合内容
        /// </summary>
        /// <param name="ObjName">集合名称</param>
        /// <returns>获取数据/null</returns>
        public static string QueryStringGBK(string ObjName)
        {
            Type Type = HttpContext.Current.Request.GetType();
            System.Reflection.PropertyInfo property = Type.GetProperty("QueryStringBytes", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.NonPublic);
            byte[] TempBytes = (byte[])property.GetValue(HttpContext.Current.Request, null);

            string TempString = HttpUtility.UrlDecode(TempBytes, System.Text.Encoding.Default);

            if (string.IsNullOrEmpty(TempString))
            {
                return string.Empty;
            }
            string[] arrQuery = TempString.ToLower().Split('&');
            foreach (string sQuery in arrQuery)
            {
                if (sQuery.Substring(0, sQuery.IndexOf("=")) == ObjName.ToLower())
                {
                    return sQuery.Substring(sQuery.IndexOf("=") + 1);
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取Post集合内容
        /// </summary>
        /// <param name="ObjName">集合名称</param>
        /// <returns>获取数据/null</returns>
        public static string Form(string ObjName)
        {
            return HttpContext.Current.Request.Form[ObjName];
        }
        /// <summary>
        /// 获取Post集合内容并返回int
        /// </summary>
        /// <param name="ObjName"></param>
        /// <returns></returns>
        public static int FormInt(string ObjName)
        {
            return GetInt(HttpContext.Current.Request.Form[ObjName]);
        }
        /// <summary>
        /// 获取Post集合内容并返回Decimal
        /// </summary>
        /// <param name="ObjName"></param>
        /// <returns></returns>
        public static Decimal FormDecimal(string ObjName)
        {
            return GetDecimal(HttpContext.Current.Request.Form[ObjName]);
        }
        /// <summary>
        /// 向浏览器输出内容
        /// </summary>
        /// <param name="oText"></param>
        public static void Write(object oText)
        {
            Write(oText, false);
        }
        /// <summary>
        /// 向浏览器输出内容
        /// </summary>
        /// <param name="oText"></param>
        /// <param name="isEnd">是否终止后面代码执行</param>
        public static void Write(object oText, bool isEnd)
        {
            HttpContext.Current.Response.Write(oText);
            if (isEnd) HttpContext.Current.Response.End();
        }
        #endregion

        #region 字符/数字/日期判断和转换
        /// <summary>
        /// 获取特定长度的随机数验证码字符[a-zA-Z0-9]
        /// </summary>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string GetRanString(int iLength)
        {
            string so = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] strArr = so.Split(',');
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < iLength; i++)
            {
                code += strArr[rand.Next(0, strArr.Length)];
            }

            return code;

        }
        /// <summary>
        /// 判断是否为数字(整数)
        /// </summary>
        /// <param name="Str"></param>
        /// <returns>True/False</returns>
        public static bool IsNumber(string Str)
        {
            if (String.IsNullOrEmpty(Str)) return false;
            for (int i = 0; i < Str.Length; i++)
            {
                if (Char.IsNumber(Str[i]) == false)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 判断是否为数字(包含小数点)
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string Str)
        {
            if (String.IsNullOrEmpty(Str)) return false;
            for (int i = 0; i < Str.Length; i++)
            {
                if (Char.IsNumber(Str[i]) == false)
                {
                    if (Str[i] != '.')
                        return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 将字符串转换为数字
        /// </summary>
        /// <param name="Str">待转换字符串</param>
        /// <returns>转换后的数字</returns>
        public static int GetInt(string Str)
        {
            if (String.IsNullOrEmpty(Str)) return 0;
            try
            {
                if (Str.IndexOf('.') > -1)
                {
                    return Convert.ToInt32(Convert.ToSingle(Str));
                }
                else
                {
                    return Convert.ToInt32(Str);
                }
            }
            catch { return 0; }
        }
        /// <summary>
        /// 将字符串转换为数字
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static int GetInt(object Obj)
        {
            if (Obj == null)
            {
                return 0;
            }
            return GetInt(Obj.ToString());
        }
        /// <summary>
        /// 将字符串转换为数字
        /// </summary>
        /// <param name="Str">待转换字符串</param>
        /// <returns>转换后的数字</returns>
        public static int GetInt(Single Str)
        {
            return Convert.ToInt32(Str);
        }
        /// <summary>
        /// 将Array字符串转换为Array数字
        /// </summary>
        /// <param name="arrayString">以[,]分隔的字符串</param>
        /// <returns></returns>
        public static int[] ArrayInt(string arrayString)
        {
            return ArrayInt(arrayString.Split(','));
        }
        /// <summary>
        /// 将Array字符串转换为Array数字
        /// </summary>
        /// <param name="arrayString">以符号分隔的字符串</param>
        /// <param name="separator">分隔符号</param>
        /// <returns></returns>
        public static int[] ArrayInt(string arrayString, params char[] separator)
        {
            return ArrayInt(arrayString.Split(separator));
        }
        /// <summary>
        /// 将Array字符串转换为Array数字
        /// </summary>
        /// <param name="arrayString">Array对像</param>
        /// <returns></returns>
        public static int[] ArrayInt(string[] arrayString)
        {
            int[] iArray = new int[arrayString.Length];
            int iCount = 0;
            foreach (string tStr in arrayString)
            {
                if (IsNumber(tStr))
                {
                    iArray[iCount] = Convert.ToInt32(tStr);
                    iCount++;
                }
            }
            //HttpContext.Current.Response.Write(iArray[0].ToString() + "|"+iArray[1].ToString());
            return iArray;
        }
        /// <summary>
        /// 将字符串转换为单精度数字
        /// </summary>
        /// <param name="Str">待转换字符串</param>
        /// <returns>转换后的单精度数字</returns>
        public static Single GetSingle(string Str)
        {
            if (IsNumeric(Str))
                return Convert.ToSingle(Str);
            else
                return 0;
        }
        /// <summary>
        /// 将字符串转换为双精度数字
        /// </summary>
        /// <param name="Str">待转换字符串</param>
        /// <returns>转换后的双精度数字</returns>
        public static Double GetDouble(string Str)
        {
            if (IsNumeric(Str))
                return Convert.ToDouble(Str);
            else
                return 0;
        }
        /// <summary>
        /// 将字符串转换为十进制数字
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static Decimal GetDecimal(string Str)
        {
            if (IsNumeric(Str))
                return Convert.ToDecimal(Str);
            else
                return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static Decimal GetDecimal(object Str)
        {
            try { return Convert.ToDecimal(Str); }
            catch { return 0; }
        }
        /// <summary>
        /// 将字符串转换为时间格式
        /// </summary>
        /// <param name="Str">待转换字符串</param>
        /// <returns>转换后的时间</returns>
        public static DateTime GetDateTime(string Str)
        {
            try
            {
                return Convert.ToDateTime(Str);
            }
            catch
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Str">待转换字符串</param>
        /// <param name="def">转换失败后默认值</param>
        /// <returns>转换后的时间</returns>
        public static DateTime GetDateTime(string Str, DateTime def)
        {
            try
            {
                return Convert.ToDateTime(Str);
            }
            catch
            {
                return def;
            }
        }
        /// <summary>
        /// 判断字符串是否是时间格式
        /// </summary>
        /// <param name="Str">字符串</param>
        /// <returns>True/False</returns>
        public static bool ChkDateTime(string Str)
        {
            return IsDateTime(Str);
        }
        /// <summary>
        /// 判断字符串是否是时间格式
        /// </summary>
        /// <param name="Str">字符串</param>
        /// <returns>True/False</returns>
        public static bool IsDateTime(string Str)
        {
            try
            {
                Convert.ToDateTime(Str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 验证邮箱是否合法
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns>True(合法)/False</returns>
        public static bool EmailValidate(string strEmail)
        {
            return IsEmail(strEmail);
        }
        /// <summary>
        /// 验证邮箱是否合法
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns>True(合法)/False</returns>
        public static bool IsEmail(string strEmail)
        {
            if (string.IsNullOrEmpty(strEmail)) return false;
            if ((strEmail.IndexOf("@") < 0)) return false;
            string TempWords = "abcdefghijklmnopqrstuvwxyz_-.0123456789@";
            char[] Chars = strEmail.ToLower().ToCharArray();
            foreach (char sChar in Chars)
            {
                if (TempWords.IndexOf(sChar) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 是否为IPSect
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPSect(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            if (string.IsNullOrEmpty(strEmail)) return false;
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static bool IsValidDoEmail(string strEmail)
        {
            if (string.IsNullOrEmpty(strEmail)) return false;
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            if (string.IsNullOrEmpty(strUrl)) return false;
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        /// <summary>
        /// 检测是否是正确手机
        /// </summary>
        /// <param name="strMobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string strMobile)
        {
            if (string.IsNullOrEmpty(strMobile)) return false;
            return Regex.IsMatch(strMobile, @"(^0?((13)|(15)|(18))\d{9}$)");
        }
        /// <summary>
        /// 检测是否是正确电话
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsPhone(string strNumber)
        {
            if (string.IsNullOrEmpty(strNumber)) return false;
            return Regex.IsMatch(strNumber, @"(^(\d{3,4}-?|\(\d{3,4}\))+?(\d{8}|\d{7})+?(-\d{4}|\(\d{4}\))?$)|(^0?((13)|(15)|(18))\d{9}$)");
        }
        /// <summary>
        /// 检测是否是正确身份证号码
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsCard(string strNumber)
        {
            if (string.IsNullOrEmpty(strNumber)) return false;
            return Regex.IsMatch(strNumber, @"(^(\d{6})(18|19|20)+?(\d{2})(0+?\d|1[012])(0+?\d|[12]\d|3[01])(\d{3})(\d|x|X)+?$)|(^\d{15}$)");
        }
        /// <summary>
        /// 根据资料下载地址匹配后缀名返回类型
        /// </summary>
        /// <param name="sFileAddress">资料地址</param>
        /// <returns></returns>
        public static string GetSoftExt(object sFileAddress)
        {
            if (sFileAddress == null)
            {
                return "other";
            }
            string sExt = "";
            if (sFileAddress.ToString().LastIndexOf(".") > 0)
            {
                sExt = sFileAddress.ToString().Substring(sFileAddress.ToString().LastIndexOf(".") + 1);
            }
            else
            {
                sExt = sFileAddress.ToString().ToLower();
            }
            switch (sExt.ToLower())
            {
                case "rar":
                case "zip":
                    return "rar";
                case "xls":
                case "xlsx":
                    return "xls";
                case "doc":
                case "docx":
                    return "doc";
                case "ppt":
                case "pptx":
                    return "ppt";
                case "pdf":
                    return "pdf";
                case "swf":
                    return "swf";
                case "mp3":
                    return "mp3";
                default:
                    return "other";
            }
        }
        #endregion

        #region Md5获取工具
        /// <summary>
        /// 获取32位Md5加密字符串
        /// </summary>
        /// <param name="dataStr">等加密字符串</param>
        /// <returns>32位Md5字符串</returns>
        public static string Md5(string dataStr)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dataStr, "MD5").ToLower();
        }

        /// <summary>
        /// 获取Md5加密字符串
        /// </summary>
        /// <param name="dataStr">等加密字符串</param>
        /// <param name="iType">加密位数(16/32)</param>
        /// <returns>Md5字符串</returns>
        public static string Md5(string dataStr, int iType)
        {
            if (iType == 16)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dataStr, "MD5").ToLower().Substring(8, 16);
            }
            else
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dataStr, "MD5").ToLower();
            }
        }
        /// <summary>
        /// 将32位Md5字符串转换成16位字符串
        /// </summary>
        /// <param name="dataStr">待转换字符串</param>
        /// <returns>已转换的16位字符串</returns>
        public static string Md5To16(string dataStr)
        {
            if (dataStr.Length < 31) return dataStr;
            //对32位MD5码进行截位成16位
            return dataStr.Substring(8, 16);
        }
        #endregion

        #region 路径/URL/文件夹/IO处理
        /// <summary>
        /// 将虚拟路径转为物理路径
        /// </summary>
        /// <param name="path">虚拟路径</param>
        /// <returns>物理路径</returns>
        public static string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// 根据时间得到上传文件存放目录(以[/]结尾)
        /// </summary>
        /// <returns>yyyy-MM/dd/</returns>
        public static string GetSaveFolder()
        {
            return DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
        }

        /// <summary>
        /// 根据资料上传时间得到HTML文件存放目录(以[/]结尾)
        /// </summary>
        /// <returns>yyyy-MM/</returns>
        public static string GetHTMLFolder(DateTime sTime)
        {
            string YearStr = sTime.Year.ToString();
            string MonthStr = "0" + sTime.Month.ToString();
            return YearStr.Substring(2, 2) + MonthStr.Substring(MonthStr.Length - 2, 2) + "/";
        }

        /// <summary>
        /// 生成一个GUID
        /// </summary>
        /// <returns></returns>
        public static string GetNewGuid()
        {
            return System.Guid.NewGuid().ToString().Replace("-", "");
        }
        /// <summary>
        /// 生成文件夹
        /// </summary>
        /// <param name="path">文件夹相对路径</param>
        /// <returns>True/False</returns>
        public static bool CreateFolder(string path)
        {
            try
            {
                path = HttpContext.Current.Server.MapPath(path);
                if (System.IO.Directory.Exists(path) == false)
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 生成一个文件
        /// </summary>
        /// <param name="sPath">文件相对路径</param>
        /// <param name="sContent">文件内容</param>
        /// <returns>True/False</returns>
        public static bool CreateFile(string sPath, string sContent)
        {
            try
            {
                System.IO.StreamWriter SWriter = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath(sPath), false, Encoding.GetEncoding("gb2312"));
                SWriter.Write(sContent);
                SWriter.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 生成一个文件
        /// </summary>
        /// <param name="sFolder">文件相对目录</param>
        /// <param name="sFileName">文件名称</param>
        /// <param name="sContent">文件内容</param>
        /// <returns>True/False</returns>
        public static bool CreateFile(string sFolder, string sFileName, string sContent)
        {
            try
            {
                string FolderStr = HttpContext.Current.Server.MapPath(sFolder);
                if (System.IO.Directory.Exists(FolderStr) == false)
                {
                    System.IO.Directory.CreateDirectory(FolderStr);
                }
                System.IO.StreamWriter SWriter = new System.IO.StreamWriter(System.IO.Path.Combine(FolderStr, sFileName), false, Encoding.GetEncoding("gb2312"));
                SWriter.Write(sContent);
                SWriter.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 读取一个文件
        /// </summary>
        /// <param name="sPath">文件路径</param>
        /// <returns>文件内容</returns>
        public static string ReadFile(string sPath)
        {
            try
            {
                //打开文件
                System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Current.Server.MapPath(sPath), Encoding.GetEncoding("gb2312"));
                //StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(sPath), Encoding.Default);               
                //读取流
                string sHtml = reader.ReadToEnd();
                //关闭流
                reader.Close();
                return sHtml;
            }
            catch
            {
                return "File.Err:" + sPath;
            }
        }

        /// <summary>
        /// 获取来源页面URL
        /// </summary>
        public static string ComeUrl()
        {
            string comeUrl = HttpContext.Current.Request["ComeUrl"];
            if (!string.IsNullOrEmpty(comeUrl))
                return HttpContext.Current.Server.UrlDecode(comeUrl.Trim());
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                comeUrl = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            else
            {
                comeUrl = "/";
            }
            return comeUrl;
        }
        #endregion

        #region WebRequest
        /// <summary>
        /// 发送一个WebRequest请求，并返回页面代码(GET)
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>返回接收到的页面内容</returns>
        public static string SendWebRequest(string url)
        {
            return SendWebRequest(url, null, "GET", Encoding.UTF8);
        }
        /// <summary>
        /// 发送一个WebRequest请求，并返回页面代码(POST)
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <returns>返回接收到的页面内容</returns>
        public static string SendWebRequest(string url, string data)
        {
            return SendWebRequest(url, data, "POST", Encoding.UTF8);
        }
        /// <summary>
        /// 发送一个WebRequest请求，并返回页面代码
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="method">请求方式(GET/POST)</param>
        /// <returns>返回接收到的页面内容</returns>
        public static string SendWebRequest(string url, string data, string method)
        {
            return SendWebRequest(url, data, method, Encoding.UTF8);
        }
        /// <summary>
        /// 发送一个WebRequest请求，并返回页面代码
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="method">请求方式(GET/POST)</param>
        /// <param name="wEnc">收发数据编码(Encoding)</param>
        /// <returns>返回接收到的页面内容</returns>
        public static string SendWebRequest(string url, string data, string method, Encoding wEnc)
        {
            return SendWebRequest(url, data, method, wEnc, wEnc);
        }
        /// <summary>
        /// 发送一个WebRequest请求，并返回页面代码
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="method">请求方式(GET/POST)</param>
        /// <param name="wEnc">发送数据编码(Encoding.UTF8)</param>
        /// <param name="rEnc">接收数据编码(Encoding.UTF8)</param>
        /// <returns>返回接收到的页面内容</returns>
        public static string SendWebRequest(string url, string data, string method, Encoding wEnc, Encoding rEnc)
        {
            WebResponse WRes;
            WebRequest WReq = WebRequest.Create(url);
            WReq.Method = string.Compare(method, "GET", true) == 0 ? "GET" : "POST";
            WReq.ContentType = "application/x-www-form-urlencoded";
            //WReq.ContentType = "text/html";
            WReq.Credentials = CredentialCache.DefaultCredentials;
            WReq.Timeout = 10000;//10秒
            if (string.IsNullOrEmpty(data) == false)
            {
                byte[] dataByte = wEnc.GetBytes(data);
                WReq.ContentLength = dataByte.Length;
                Stream writer = WReq.GetRequestStream();
                writer.Write(dataByte, 0, dataByte.Length);
                writer.Close();
            }
            try { WRes = WReq.GetResponse(); }
            catch (WebException WExc)
            {
                #region 获取错误信息流-艾延路（便于获取新浪微博错误码）
                if (WExc.Response != null)
                {
                    using (StreamReader reader = new StreamReader(WExc.Response.GetResponseStream()))
                    {
                        string errorInfo = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                #endregion

                throw new Exception(string.Format("message:{0};status:{1};url:{2};data:{3}", WExc.Message, WExc.Status, url, data));
                //WRes = WExc.Response;
            }
            StreamReader SRea = new StreamReader(WRes.GetResponseStream(), rEnc);
            string reString = SRea.ReadToEnd();
            WRes.Close(); SRea.Dispose();
            return reString;
        }
        #endregion

        #region OTHER

        /// <summary>
        /// 返回两个时间间隔(d，h, m，s)
        /// </summary>
        /// <param name="DateInterval">d，h, m，s</param>
        /// <param name="Date1"></param>
        /// <param name="Date2"></param>
        /// <returns></returns>
        public static long DateDiff(string DateInterval, DateTime Date1, DateTime Date2)
        {
            TimeSpan ts = Date2 - Date1;
            switch (DateInterval.ToLower())
            {
                case "d":
                case "day":
                    return (long)ts.Days;
                case "h":
                case "hour":
                    return (long)ts.TotalHours;
                case "m":
                case "minute":
                    return (long)ts.TotalMinutes;
                case "s":
                case "second":
                    return (long)ts.TotalSeconds;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 获取客户端标识码
        /// </summary>
        /// <returns></returns>
        public static string GetClientSN()
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"], "MD5").ToLower();
        }
        /// <summary>
        /// 获取客户端标识码
        /// </summary>
        /// <param name="iType">位数(16/32)</param>
        /// <returns></returns>
        public static string GetClientSN(int iType)
        {
            if (iType == 16)
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(HttpContext.Current.Request.UserAgent, "MD5").ToLower().Substring(8, 16);
            else
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(HttpContext.Current.Request.UserAgent, "MD5").ToLower();
        }
        /// <summary>
        /// 获取客户端IP地址，如果是通过代理访问，则获取代理IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string IPStr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IPStr) || IPStr.IndexOf("unknown") > 0)
            {
                IPStr = HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                if (IPStr.IndexOf(",") > 0)
                {
                    IPStr = IPStr.Substring(IPStr.LastIndexOf(",") + 1);
                }
                else if (IPStr.IndexOf(";") > 0)
                {
                    IPStr = IPStr.Substring(IPStr.LastIndexOf(";") + 1);
                }
                IPStr = IPStr.Trim();
                //内网判断
                if (IPStr.Substring(0, 3) == "10." || IPStr.Substring(0, 4) == "172." || IPStr.Substring(0, 4) == "192.")
                {
                    IPStr = HttpContext.Current.Request.UserHostAddress;
                }
            }
            return IPStr;
        }
        /// <summary>
        /// 获取下载文件SN识别码
        /// </summary>
        /// <param name="FileAddress"></param>
        /// <returns></returns>
        public static string GetDownFileSn(string FileAddress)
        {
            string FileName = FileAddress.Substring(FileAddress.LastIndexOf("/") + 1);
            string Md5String = GetClientIP() + "xcvk6x8h" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "a2dfej1p" + FileName.ToLower();
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Md5String, "MD5").ToLower();
        }
        /// <summary>
        /// 获取下载文件SN识别码
        /// </summary>
        /// <param name="FileAddress"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string GetDownFileSn(string FileAddress, string ipAddress)
        {
            string FileName = FileAddress.Substring(FileAddress.LastIndexOf("/") + 1);
            string Md5String = ipAddress + "xcvk6x8h" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "a2dfej1p" + FileName.ToLower();
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Md5String, "MD5").ToLower();
        }
        /// <summary>
        /// 判断是否来自外部提交
        /// </summary>
        /// <returns></returns>
        public static bool IsOutsidePost()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return true;
            }
            else if (HttpContext.Current.Request.UrlReferrer.Host != HttpContext.Current.Request.Url.Host)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 输出错误页并终止执行
        /// </summary>
        /// <param name="sContent">错误信息内容(可带HTML代码)</param>
        public static void ErrorPage(string sContent)
        {
            StringBuilder oHTML = new StringBuilder();
            oHTML.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            oHTML.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
            oHTML.Append("<head>\n");
            oHTML.Append("<meta content=\"text/html; charset=gb2312\" http-equiv=\"Content-Type\" />\n");
            oHTML.Append("<title>提示信息</title>\n");
            oHTML.Append("<style type=\"text/css\">\n");
            oHTML.Append(".board {border: #a7c5e2 1px solid;padding: 1px;width: 470px;}\n");
            oHTML.Append(".topInfo {text-align: left;background: #ebf3fb;padding: 12px;font: bold 16px verdana;color: #4a8f00;}\n");
            oHTML.Append(".tipContent {padding: 15px;border-top: #d2e2f4 1px solid;border-bottom: #d2e2f4 1px solid;background: #fff;color:Red;line-height: 22px;min-height:	120px;max-height: 300px;text-indent: 26px;text-align: left;font-size: 15px;}\n");
            oHTML.Append(".butInfo {text-align:right;background: #ebf3fb;padding: 8px;font: bold 15px verdana;color: #4a8f00;}\n");
            oHTML.Append("</style>\n");
            oHTML.Append("</head>\n");
            oHTML.Append("<body style=\"margin-top: 80px\">\n");
            oHTML.Append("<center>\n");
            oHTML.Append("<div class=\"board\">\n");
            oHTML.Append("<div class=\"topInfo\">学科网提示您</div>\n");
            oHTML.AppendFormat("<div class=\"tipContent\">{0}</div>\n", sContent);
            oHTML.Append("<div class=\"butInfo\"><a href=\"/\">返回到首页</a></div>\n");
            oHTML.Append("</div>\n");
            oHTML.Append("</center>\n");
            oHTML.Append("</body>\n");
            oHTML.Append("</html>\n");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(oHTML.ToString());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 跳转到主站提示信息
        /// </summary>
        /// <param name="sMessage">提示信息(不能带html)</param>
        public static void ErrorJump(string sMessage)
        {
            HttpContext.Current.Response.Redirect(string.Format("http://www.zxxk.com/Error.aspx?msg={0}", sMessage));
            HttpContext.Current.Response.End();
        }
        #endregion

        #region cookies
        /// <summary>
        /// 写入COOKIES
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        /// <param name="domain"></param>
        public static void AddCookies(String keyName, String value, DateTime? expires = null, string domain = "dtilovey.com")
        {
            if (expires == null)
            {
                expires = DateTime.Now.AddDays(1);
            }

            HttpCookie cookie = new HttpCookie(keyName, value);
            cookie.Expires = expires.Value;
            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }


            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void ClearCookies(String keyName)
        {


            HttpCookie cookie = new HttpCookie(keyName);
            cookie.Expires = DateTime.Now.AddDays(-1);



            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 获取cookis
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static String GetCookies(String keyName)
        {
            if (HttpContext.Current.Request.Cookies[keyName] != null)
            {
                return HttpContext.Current.Request.Cookies[keyName].Value;
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 将对象转换为布尔值
        /// </summary>
        /// <param name="Obj">待转换对象</param>
        /// <returns>true/false</returns>
        public static bool GetBool(object Obj)
        {
            try
            {
                return Convert.ToBoolean(Obj);
            }
            catch
            {
                if (null == Obj) return false;
                if (Obj.ToString() == "1" || Obj.ToString().ToLower() == "true")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 按精品频道分类把资料归到某学科ID上
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public static string JingPinToChannel(string classid)
        {
            switch (classid)
            {
                case "79": return "13"; //语文
                case "77": return "12"; //数学
                case "80": return "14"; //英语
                case "78": return "11"; //物理
                case "76": return "10"; //化学
                case "81": return "18"; //生物
                case "82": return "17"; //政治
                case "84": return "15"; //历史
                case "83": return "16"; //地理
                default: return "19";
            }
        }
        /// <summary>
        /// 格式化大小
        /// </summary>
        /// <param name="softsize"></param>
        /// <returns></returns>
        public static string GetSoftSize(string softsize)
        {
            int SoftSize = Convert.ToInt32(softsize);
            if (SoftSize < 1024)
            {
                return SoftSize + "KB";
            }
            else
            {
                SoftSize = SoftSize / 1024;
                return SoftSize + "MB";
            }
        }

        /// <summary>
        /// 添加Where条件
        /// </summary>
        /// <param name="strwhere">已有Where条件</param>
        /// <param name="newwhere">新增Where条件</param>
        /// <returns></returns>
        public static void AddWhere(this StringBuilder strwhere, string newwhere)
        {
            if (string.IsNullOrEmpty(strwhere.ToString()))
            {
                strwhere.Append($" where {newwhere}");
            }
            else
            {
                strwhere.Append($" and {newwhere}");
            }
        }

        /// <summary>
        /// 根据key获取配置文件中的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? "";
        }
        public static string OerateSpecialChar(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str = str.Replace("'", "''");
            return str;
        }
        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 剔除文件名中的字符串中非法的字符
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileNameWithOutInvalid(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "";
            }
            fileName = fileName.Replace(" ", "").Replace("\u3000", "");//去除空格跟全角空白字符
            StringBuilder builder = new StringBuilder(fileName);
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                // fileName.Replace(invalidChar.ToString(), string.Empty);
                builder.Replace(invalidChar.ToString(), string.Empty);
            }
            return builder.ToString();
        }
    }
}