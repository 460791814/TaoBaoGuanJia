using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TaoBaoGuanJia.Extension;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Service
{
   public class TaoBaoThread
    {
        public MainForm mainForm;
        TaoBaoService taoBaoService = new TaoBaoService();
        public void Handler(string url)
        {
            Uri u = new Uri(url);
            string path=  u.AbsolutePath;
            if (path.IndexOf("item.htm") > -1) {
                //详情
               string id= GetId(url);
                if (!string.IsNullOrEmpty(id)) {
                    Thread single = new Thread(Single);
                    single.IsBackground = true;
                    single.Start(id);
                  
                }
            }
            else if (path.IndexOf("search") > -1)
            {
                //列表
            }
            else if (path.IndexOf(".taobao.com") > -1)
            {
                //列表
            }
        }

 

        /// <summary>
        /// 单个采集
        /// </summary>
        /// <param name="obj"></param>
        void Single(object obj)
        {
           ControlsUtils.AppendTextLog("123");
          //    MessageBoxEx.Show(mainForm, "12323");
          //  string id = obj as string;
          //  taoBaoService.CollectToDB(id);

          //  ControlsUtils.RefreshDataGridViewMaster();
        }
        public static string GetId(string url)
        {

            string reg = @"id=(\d{10,})";
            Match match = Regex.Match(url, reg);
            if (match.Success) {
                return match.Groups[1].Value;
            }
            return null;
        }
    }
}
