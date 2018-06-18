using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Service;
using TaoBaoGuanJia.Extension;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
    
            Init();
        }

        private void Init()
        {
            InitDataGridViewMaster();
        }

        private void btn_caiji_Click(object sender, EventArgs e)
        {
            string url = txt_url.Text;
            if (string.IsNullOrEmpty(url)) {
                MessageBoxEx.Show(this, "请输入淘宝(宝贝/店铺/列表)地址");
            }
            TaoBaoThread taoBaoThread = new TaoBaoThread();
            taoBaoThread.mainForm = this;
            taoBaoThread.Handler(url);


        }
        /// <summary>
        /// 在线程中操作窗体的控件
        /// </summary>
        /// <param name="action"></param>
        public void OpeMainFormControl(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action); //返回主线程（创建控件的线程）
            }
            else
            {
                action();
            }
        }
        private void ch_OnCheckBoxClicked(object sender, datagridviewCheckboxHeaderEventArgs e)
        {
            dataGridViewMaster.EndEdit();
            foreach (DataGridViewRow dgvRow in this.dataGridViewMaster.Rows)
            {
                if (e.CheckedState)
                {
                    dgvRow.Cells[0].Value = true;
                }
                else
                {
                    dgvRow.Cells[0].Value = false;
                }
            }
        }
        private void Test()
        {
            int a = DataHelper.InsertSpItem(new Model.Sp_item()
            {
                Name = "123",

            });
            // var dd= DataHelper.GetSysSortList();
        }


        private void btn_export_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (DataGridViewRow dgvRow in this.dataGridViewMaster.Rows)
            {
                if (DataConvert.ToBoolean(dgvRow.Cells[0].Value))
                {
                    list.Add(DataConvert.ToString(dgvRow.Cells[1].Value));
                }
            }
            if (list.Count <= 0)
            {
                MessageBoxEx.Show(this, "请勾选需要导出的宝贝");
            }
            else
            {
                TaoBaoService taobaoService = new TaoBaoService();
                taobaoService.ExportToCsv(list);
            }
        }

        private void dataGridViewMaster_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //显示在HeaderCell上
            for (int i = 0; i < this.dataGridViewMaster.Rows.Count; i++)
            {
                DataGridViewRow r = this.dataGridViewMaster.Rows[i];
                r.HeaderCell.Value = string.Format("{0}", i + 1);
            }
            this.dataGridViewMaster.Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dataGridViewMaster.ClearSelection();
            this.Resize += new EventHandler(Form1_Resize);

            X = this.Width;
            Y = this.Height;
            setTag(this);
            Form1_Resize(new object(), new EventArgs());

            ControlsUtils.mainForm = this;
            ControlsUtils.dataGridViewMaster = this.dataGridViewMaster;
 
        }
        #region 多线程操作控件
        private delegate void RefreshDataGridViewMasterDelegate();
        public void InitDataGridViewMaster()
        {
     
            var list = DataHelper.GetProductItemList();
            dataGridViewMaster.AutoGenerateColumns = false;
            dataGridViewMaster.DataSource = list;
            //创建复选框列     
            DataGridViewCheckBoxColumn checkboxCol = new DataGridViewCheckBoxColumn();
            //创建复选框列头单元格  
            datagridviewCheckboxHeaderCell ch = new datagridviewCheckboxHeaderCell();
            //设置复选框那列的单元格为复选框  
            checkboxCol.HeaderCell = ch;

            //将此复选框列添加到DataGridView中  
            this.dataGridViewMaster.Columns.Insert(0, checkboxCol);

            ch.OnCheckBoxClicked += new datagridviewcheckboxHeaderEventHander(ch_OnCheckBoxClicked);//关联单击事件  
     
        }

        #endregion
        #region 窗体缩放

        private float X;
        private float Y;
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);
                Single currentSize = Convert.ToSingle(mytag[4]) * Math.Min(newx, newy);
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }

        }
        void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
         //   this.Text = this.Width.ToString() + " " + this.Height.ToString();

        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "当前系统时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        
        }
    }
}
