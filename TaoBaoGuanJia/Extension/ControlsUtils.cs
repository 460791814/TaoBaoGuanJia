using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaoBaoGuanJia.Helper;

namespace TaoBaoGuanJia.Extension
{
    //控件代理
    public static class ControlsUtils
    {
        public static MainForm mainForm;
        public static DataGridView dataGridViewMaster;
        public static TextBox txtLog;
        public static DialogResult DialogResult;
        //空参委托
        private delegate void EmptyDelegate();
        //string单参数委托
        private delegate void StringDelegate(string str);
        /// <summary>
        /// 刷新产品列表
        /// </summary>
        public static void RefreshDataGridViewMaster()
        {
 
            if (mainForm.InvokeRequired)
            {
               
                dataGridViewMaster.Invoke(new EmptyDelegate(RefreshDataGridViewMaster));
            }
            else
            {
                var list = DataHelper.GetProductItemList();
                dataGridViewMaster.DataSource = list;
            }
        }
        /// <summary>
        /// 弹出提示
        /// </summary>
        /// <param name="msg"></param>
        public static void Msg(string msg)
        {
            if (mainForm.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(Msg);
                mainForm.Invoke(d,msg);
            }
            else
            {
                MessageBoxEx.Show(mainForm, msg);
            }
        }
        /// <summary>
        /// 输出操作日记
        /// </summary>
        /// <param name="msg"></param>
        public static void AppendTextLog(string msg)
        {
            if (mainForm.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(Msg);
                mainForm.Invoke(d,msg);
            }
            else
            {
                txtLog.AppendText(msg);
                txtLog.AppendText(Environment.NewLine);
                txtLog.ScrollToCaret();
            }
        }
    }
}
