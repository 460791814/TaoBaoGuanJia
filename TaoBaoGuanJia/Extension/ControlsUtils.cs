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
        public static MainForm mainForm;//主窗体对象
        public static DataGridView dataGridViewMaster;//列表
        public static TextBox operationLog;//操作日记
        public static ToolStripProgressBar toolStripProgressBar_main;//进度条
        //空参委托
        private delegate void EmptyDelegate();
        //string单参数委托
        private delegate void StringDelegate(string str);
        private delegate void ProgressBarDelegate(int max, int value);
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
        public static void OperationLog(string msg)
        {
            if (mainForm.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(OperationLog);
                mainForm.Invoke(d,msg);
            }
            else
            {
                operationLog.AppendText(msg);
                operationLog.AppendText(Environment.NewLine);
                operationLog.ScrollToCaret();
               
            }
        }
        /// <summary>
        /// 进度条
        /// </summary>
        /// <param name="msg"></param>
        public static void SetProgressBarValue(int max,int value)
        {
            if (mainForm.InvokeRequired)
            {
                ProgressBarDelegate d = new ProgressBarDelegate(SetProgressBarValue);
                mainForm.Invoke(d, max, value);
            }
            else
            {
                toolStripProgressBar_main.Maximum = max;
                toolStripProgressBar_main.Value = value;

            }
        }
    }
}
