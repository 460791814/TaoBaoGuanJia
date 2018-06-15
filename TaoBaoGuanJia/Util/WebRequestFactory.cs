using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TaoBaoGuanJia.Util
{
    public class WebRequestFactory
    {
        public static WebRequest GetWebRequest(string url)
        {
            Uri requestUri;
            if (!url.EndsWith("."))
            {
                requestUri = new Uri(url);
            }
            else
            {
                requestUri = UrlUtil.MakeSpecialUri(url);
            }
            return WebRequest.Create(requestUri);
        }
        public static HttpWebRequest GetHttpWebRequest(string url)
        {
            return (HttpWebRequest)WebRequestFactory.GetWebRequest(url);
        }
    }
}
