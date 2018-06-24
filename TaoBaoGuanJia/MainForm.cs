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
using TaoBaoGuanJia.Model;
using System.Threading.Tasks;
using System.Diagnostics;


namespace TaoBaoGuanJia
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            InitData();
            InitUserConfig();
        }
        /// <summary>
        /// 初始化用户设置
        /// </summary>
        private void InitUserConfig()
        {
            chk_MobileDesc.Checked = UserSetting.Default.IsExportMobileDesc;
           // chk_SalePrice.Checked = UserSetting.Default.IsUseSalePrice;
            chk_IsClearAssociated.Checked = UserSetting.Default.IsClearAssociated;
            chk_IsSaveIdToOutId.Checked = UserSetting.Default.IsSaveIdToOutId;
            chk_IsDownPic.Checked = UserSetting.Default.IsDownPic;
            chk_IsClearAutoDeliveryDesc.Checked = UserSetting.Default.IsClearAutoDeliveryDesc;
            textBox_ContentHeader.Text = UserSetting.Default.ContentHeader;
            if (string.IsNullOrEmpty(UserSetting.Default.FileSavePath)) {
                UserSetting.Default.FileSavePath=System.Environment.CurrentDirectory + "/csv/";
                UserSetting.Default.Save();
            }
            textBox_FileSavePath.Text = UserSetting.Default.FileSavePath;

        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            InitDataGridViewMaster();
            InitProvinceCity();
        }

        private void InitProvinceCity()
        {
            comboBox_province.SelectedIndexChanged -= new EventHandler(comboBox_province_SelectedIndexChanged);
            comboBox_City.SelectedIndexChanged -= new EventHandler(comboBox_City_SelectedIndexChanged);
            List<string> list = DataHelper.GetAddressList().FindAll(a => a.Parentid == 0).Select(a => a.Addrcode).ToList();
            list.Insert(0, "默认");
            comboBox_province.DataSource = list;
            if (!string.IsNullOrEmpty(UserSetting.Default.Province))
            {
                comboBox_province.SelectedIndex = comboBox_province.Items.IndexOf(UserSetting.Default.Province);

            }
            string name = comboBox_province.SelectedValue.ToString();
            List<string> CityList = DataHelper.GetAddressList().FindAll(a => a.Parent == name).Select(a => a.Addrcode).ToList();
            if (CityList != null && CityList.Count > 0)
            {
                comboBox_City.DataSource = CityList;
                if (!string.IsNullOrEmpty(UserSetting.Default.City))
                {
                    comboBox_City.SelectedIndex = comboBox_City.Items.IndexOf(UserSetting.Default.City);
                }
            }

            comboBox_province.SelectedIndexChanged += new EventHandler(comboBox_province_SelectedIndexChanged);
            comboBox_City.SelectedIndexChanged += new EventHandler(comboBox_City_SelectedIndexChanged);
        }

        private void btn_caiji_Click(object sender, EventArgs e)
        {
            string url = txt_url.Text;
            if (string.IsNullOrEmpty(url))
            {
                MessageBoxEx.Show(this, "请输入淘宝(宝贝/店铺/列表)地址");
            }
            TaoBaoService taobaoService = new TaoBaoService();
            taobaoService.ImportProduct(url);


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
                new Task(()=> {
                    TaoBaoService taobaoService = new TaoBaoService();
                    taobaoService.ExportToCsv(list);
                }).Start();
                
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
            ControlsUtils.operationLog = txt_OperationLog;
            ControlsUtils.toolStripProgressBar_main = toolStripProgressBar_main;
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

        private void chk_MobileDesc_CheckedChanged(object sender, EventArgs e)
        {
            UserSetting.Default.IsExportMobileDesc = chk_MobileDesc.Checked;
            UserSetting.Default.Save();


        }
        /// <summary>
        /// 勾选促销价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_SalePrice_CheckedChanged(object sender, EventArgs e)
        {
          //  UserSetting.Default.IsUseSalePrice = chk_SalePrice.Checked;
          //  UserSetting.Default.Save();
        }

        private void chk_IsClearAssociated_CheckedChanged(object sender, EventArgs e)
        {
            UserSetting.Default.IsClearAssociated = chk_IsClearAssociated.Checked;
            UserSetting.Default.Save();
        }

        private void chk_IsSaveIdToOutId_CheckedChanged(object sender, EventArgs e)
        {
            UserSetting.Default.IsSaveIdToOutId = chk_IsSaveIdToOutId.Checked;
            UserSetting.Default.Save();
        }

        private void chk_IsDownPic_CheckedChanged(object sender, EventArgs e)
        {
            UserSetting.Default.IsDownPic = chk_IsDownPic.Checked;
            UserSetting.Default.Save();
        }

        private void chk_IsClearAutoDeliveryDesc_CheckedChanged(object sender, EventArgs e)
        {
            UserSetting.Default.IsClearAutoDeliveryDesc = chk_IsClearAutoDeliveryDesc.Checked;
            UserSetting.Default.Save();
        }

        private void comboBox_province_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = comboBox_province.SelectedValue.ToString();
            List<string> list = DataHelper.GetAddressList().FindAll(a => a.Parent == name).Select(a => a.Addrcode).ToList();
            if (list != null && list.Count > 0)
            {
                comboBox_City.DataSource = list;
                UserSetting.Default.Province = name;
                UserSetting.Default.City = list[0];
                UserSetting.Default.Save();
            }
            else
            {
                comboBox_City.DataSource = null;
            }
        }

        private void comboBox_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserSetting.Default.City = comboBox_City.SelectedValue.ToString();
            UserSetting.Default.Save();
        }

        private void textBox_ContentHeader_TextChanged(object sender, EventArgs e)
        {
            UserSetting.Default.ContentHeader = textBox_ContentHeader.Text.Trim();
            UserSetting.Default.Save();
        }

        private void btn_SaveCsvPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox_FileSavePath.Text = foldPath;
                UserSetting.Default.FileSavePath = foldPath;
                UserSetting.Default.Save();
            }
        }

        private void dataGridViewMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewMaster.Columns[e.ColumnIndex].Name == "linkInfo")
            {
                string id = this.dataGridViewMaster.Rows[e.RowIndex].Cells[5].Value.ToString();
                System.Diagnostics.Process.Start("https://item.taobao.com/item.htm?id="+id);
            }
            if (dataGridViewMaster.Columns[e.ColumnIndex].Name == "linkDelete") {
                //删除
                string id = this.dataGridViewMaster.Rows[e.RowIndex].Cells[1].Value.ToString();
                DataHelper.DeleteItemByIds(id);
                ControlsUtils.RefreshDataGridViewMaster();
            }
        }
        private string Notes = "请输入淘宝宝贝详情地址";
        private void txt_url_Enter(object sender, EventArgs e)
        {
            //  进入时，清空
            if (txt_url.Text == Notes) { 
                this.txt_url.Text = "";
                txt_url.ForeColor = Color.Black;
            }
        }

        private void txt_url_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_url.Text)) { 
                this.txt_url.Text = Notes;
                txt_url.ForeColor = Color.LightGray;
            }
        }
    }
}
