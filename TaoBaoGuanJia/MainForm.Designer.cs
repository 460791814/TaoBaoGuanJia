namespace TaoBaoGuanJia
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn_caiji = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
            this.lab_url = new System.Windows.Forms.Label();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.dataGridViewMaster = new System.Windows.Forms.DataGridView();
            this.txt_OperationLog = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_SaveCsvPath = new System.Windows.Forms.Button();
            this.textBox_FileSavePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ContentHeader = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_City = new System.Windows.Forms.ComboBox();
            this.comboBox_province = new System.Windows.Forms.ComboBox();
            this.chk_IsClearAutoDeliveryDesc = new System.Windows.Forms.CheckBox();
            this.chk_IsDownPic = new System.Windows.Forms.CheckBox();
            this.chk_IsSaveIdToOutId = new System.Windows.Forms.CheckBox();
            this.chk_IsClearAssociated = new System.Windows.Forms.CheckBox();
            this.chk_MobileDesc = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip_main = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar_main = new System.Windows.Forms.ToolStripProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewLinkColumn1 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shuliang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.province = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.city = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaster)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_caiji
            // 
            this.btn_caiji.Location = new System.Drawing.Point(891, 8);
            this.btn_caiji.Name = "btn_caiji";
            this.btn_caiji.Size = new System.Drawing.Size(75, 25);
            this.btn_caiji.TabIndex = 0;
            this.btn_caiji.Text = "采集";
            this.btn_caiji.UseVisualStyleBackColor = true;
            this.btn_caiji.Click += new System.EventHandler(this.btn_caiji_Click);
            // 
            // btn_export
            // 
            this.btn_export.Location = new System.Drawing.Point(8, 24);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(130, 31);
            this.btn_export.TabIndex = 1;
            this.btn_export.Text = "生成数据包";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // lab_url
            // 
            this.lab_url.AutoSize = true;
            this.lab_url.BackColor = System.Drawing.SystemColors.Window;
            this.lab_url.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_url.Location = new System.Drawing.Point(13, 11);
            this.lab_url.Name = "lab_url";
            this.lab_url.Size = new System.Drawing.Size(52, 15);
            this.lab_url.TabIndex = 2;
            this.lab_url.Text = "地址：";
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(71, 33);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(818, 25);
            this.txt_url.TabIndex = 3;
            this.txt_url.Enter += new System.EventHandler(this.txt_url_Enter);
            this.txt_url.Leave += new System.EventHandler(this.txt_url_Leave);
            // 
            // dataGridViewMaster
            // 
            this.dataGridViewMaster.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.productName,
            this.Price,
            this.shuliang,
            this.code,
            this.province,
            this.city});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewMaster.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewMaster.Location = new System.Drawing.Point(10, 72);
            this.dataGridViewMaster.Name = "dataGridViewMaster";
            this.dataGridViewMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewMaster.RowTemplate.Height = 27;
            this.dataGridViewMaster.Size = new System.Drawing.Size(960, 245);
            this.dataGridViewMaster.TabIndex = 4;
            this.dataGridViewMaster.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMaster_CellContentClick);
            this.dataGridViewMaster.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridViewMaster_RowStateChanged);
            // 
            // txt_OperationLog
            // 
            this.txt_OperationLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_OperationLog.Location = new System.Drawing.Point(3, 21);
            this.txt_OperationLog.Multiline = true;
            this.txt_OperationLog.Name = "txt_OperationLog";
            this.txt_OperationLog.ReadOnly = true;
            this.txt_OperationLog.Size = new System.Drawing.Size(423, 133);
            this.txt_OperationLog.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(983, 589);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.Tag = "";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.lab_url);
            this.tabPage1.Controls.Add(this.btn_caiji);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(975, 560);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "宝贝复制";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_export);
            this.groupBox3.Location = new System.Drawing.Point(8, 298);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(958, 69);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_SaveCsvPath);
            this.groupBox2.Controls.Add(this.textBox_FileSavePath);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_ContentHeader);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBox_City);
            this.groupBox2.Controls.Add(this.comboBox_province);
            this.groupBox2.Controls.Add(this.chk_IsClearAutoDeliveryDesc);
            this.groupBox2.Controls.Add(this.chk_IsDownPic);
            this.groupBox2.Controls.Add(this.chk_IsSaveIdToOutId);
            this.groupBox2.Controls.Add(this.chk_IsClearAssociated);
            this.groupBox2.Controls.Add(this.chk_MobileDesc);
            this.groupBox2.Location = new System.Drawing.Point(443, 373);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(523, 157);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // btn_SaveCsvPath
            // 
            this.btn_SaveCsvPath.Location = new System.Drawing.Point(438, 17);
            this.btn_SaveCsvPath.Name = "btn_SaveCsvPath";
            this.btn_SaveCsvPath.Size = new System.Drawing.Size(75, 26);
            this.btn_SaveCsvPath.TabIndex = 17;
            this.btn_SaveCsvPath.Text = "更改";
            this.btn_SaveCsvPath.UseVisualStyleBackColor = true;
            this.btn_SaveCsvPath.Click += new System.EventHandler(this.btn_SaveCsvPath_Click);
            // 
            // textBox_FileSavePath
            // 
            this.textBox_FileSavePath.Location = new System.Drawing.Point(268, 18);
            this.textBox_FileSavePath.Name = "textBox_FileSavePath";
            this.textBox_FileSavePath.Size = new System.Drawing.Size(167, 25);
            this.textBox_FileSavePath.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(182, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "保存路径：";
            // 
            // textBox_ContentHeader
            // 
            this.textBox_ContentHeader.Location = new System.Drawing.Point(268, 47);
            this.textBox_ContentHeader.Name = "textBox_ContentHeader";
            this.textBox_ContentHeader.Size = new System.Drawing.Size(245, 25);
            this.textBox_ContentHeader.TabIndex = 14;
            this.textBox_ContentHeader.TextChanged += new System.EventHandler(this.textBox_ContentHeader_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "头部描述：";
            this.toolTip.SetToolTip(this.label2, "在描述部分头部添加内容");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "省市：";
            // 
            // comboBox_City
            // 
            this.comboBox_City.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_City.FormattingEnabled = true;
            this.comboBox_City.Location = new System.Drawing.Point(393, 77);
            this.comboBox_City.Name = "comboBox_City";
            this.comboBox_City.Size = new System.Drawing.Size(120, 23);
            this.comboBox_City.TabIndex = 11;
            this.comboBox_City.SelectedIndexChanged += new System.EventHandler(this.comboBox_City_SelectedIndexChanged);
            // 
            // comboBox_province
            // 
            this.comboBox_province.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_province.FormattingEnabled = true;
            this.comboBox_province.Location = new System.Drawing.Point(268, 77);
            this.comboBox_province.Name = "comboBox_province";
            this.comboBox_province.Size = new System.Drawing.Size(120, 23);
            this.comboBox_province.TabIndex = 10;
            this.comboBox_province.SelectedIndexChanged += new System.EventHandler(this.comboBox_province_SelectedIndexChanged);
            // 
            // chk_IsClearAutoDeliveryDesc
            // 
            this.chk_IsClearAutoDeliveryDesc.AutoSize = true;
            this.chk_IsClearAutoDeliveryDesc.Location = new System.Drawing.Point(7, 74);
            this.chk_IsClearAutoDeliveryDesc.Name = "chk_IsClearAutoDeliveryDesc";
            this.chk_IsClearAutoDeliveryDesc.Size = new System.Drawing.Size(149, 19);
            this.chk_IsClearAutoDeliveryDesc.TabIndex = 5;
            this.chk_IsClearAutoDeliveryDesc.Text = "去除自动发货描述";
            this.chk_IsClearAutoDeliveryDesc.UseVisualStyleBackColor = true;
            this.chk_IsClearAutoDeliveryDesc.CheckedChanged += new System.EventHandler(this.chk_IsClearAutoDeliveryDesc_CheckedChanged);
            // 
            // chk_IsDownPic
            // 
            this.chk_IsDownPic.AutoSize = true;
            this.chk_IsDownPic.Location = new System.Drawing.Point(7, 49);
            this.chk_IsDownPic.Name = "chk_IsDownPic";
            this.chk_IsDownPic.Size = new System.Drawing.Size(134, 19);
            this.chk_IsDownPic.TabIndex = 4;
            this.chk_IsDownPic.Text = "下载图片到本地";
            this.toolTip.SetToolTip(this.chk_IsDownPic, "解决图片盗用问题");
            this.chk_IsDownPic.UseVisualStyleBackColor = true;
            this.chk_IsDownPic.CheckedChanged += new System.EventHandler(this.chk_IsDownPic_CheckedChanged);
            // 
            // chk_IsSaveIdToOutId
            // 
            this.chk_IsSaveIdToOutId.AutoSize = true;
            this.chk_IsSaveIdToOutId.Location = new System.Drawing.Point(7, 124);
            this.chk_IsSaveIdToOutId.Name = "chk_IsSaveIdToOutId";
            this.chk_IsSaveIdToOutId.Size = new System.Drawing.Size(180, 19);
            this.chk_IsSaveIdToOutId.TabIndex = 3;
            this.chk_IsSaveIdToOutId.Text = "商品ID存储到商家编码";
            this.chk_IsSaveIdToOutId.UseVisualStyleBackColor = true;
            this.chk_IsSaveIdToOutId.CheckedChanged += new System.EventHandler(this.chk_IsSaveIdToOutId_CheckedChanged);
            // 
            // chk_IsClearAssociated
            // 
            this.chk_IsClearAssociated.AutoSize = true;
            this.chk_IsClearAssociated.Location = new System.Drawing.Point(7, 99);
            this.chk_IsClearAssociated.Name = "chk_IsClearAssociated";
            this.chk_IsClearAssociated.Size = new System.Drawing.Size(164, 19);
            this.chk_IsClearAssociated.TabIndex = 2;
            this.chk_IsClearAssociated.Text = "清理描述中关联商品";
            this.chk_IsClearAssociated.UseVisualStyleBackColor = true;
            this.chk_IsClearAssociated.CheckedChanged += new System.EventHandler(this.chk_IsClearAssociated_CheckedChanged);
            // 
            // chk_MobileDesc
            // 
            this.chk_MobileDesc.AutoSize = true;
            this.chk_MobileDesc.Location = new System.Drawing.Point(7, 24);
            this.chk_MobileDesc.Name = "chk_MobileDesc";
            this.chk_MobileDesc.Size = new System.Drawing.Size(119, 19);
            this.chk_MobileDesc.TabIndex = 0;
            this.chk_MobileDesc.Text = "下载手机详情";
            this.toolTip.SetToolTip(this.chk_MobileDesc, "勾选下载手机详情");
            this.chk_MobileDesc.UseVisualStyleBackColor = true;
            this.chk_MobileDesc.CheckedChanged += new System.EventHandler(this.chk_MobileDesc_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_OperationLog);
            this.groupBox1.Location = new System.Drawing.Point(8, 373);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 157);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输出";
            // 
            // statusStrip_main
            // 
            this.statusStrip_main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel2,
            this.toolStripProgressBar_main});
            this.statusStrip_main.Location = new System.Drawing.Point(0, 564);
            this.statusStrip_main.Name = "statusStrip_main";
            this.statusStrip_main.Size = new System.Drawing.Size(983, 25);
            this.statusStrip_main.TabIndex = 7;
            this.statusStrip_main.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(84, 20);
            this.toolStripStatusLabel1.Text = "当前时间：";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(84, 20);
            this.toolStripStatusLabel2.Text = "任务进度：";
            // 
            // toolStripProgressBar_main
            // 
            this.toolStripProgressBar_main.Name = "toolStripProgressBar_main";
            this.toolStripProgressBar_main.Size = new System.Drawing.Size(100, 19);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolTip
            // 
            this.toolTip.ToolTipTitle = "提示";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(234, 20);
            this.toolStripStatusLabel3.Text = "软件问题交流QQ群：372368352";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 52;
            // 
            // dataGridViewLinkColumn1
            // 
            this.dataGridViewLinkColumn1.DataPropertyName = "name";
            this.dataGridViewLinkColumn1.FillWeight = 369.5432F;
            this.dataGridViewLinkColumn1.HeaderText = "宝贝标题";
            this.dataGridViewLinkColumn1.Name = "dataGridViewLinkColumn1";
            this.dataGridViewLinkColumn1.ReadOnly = true;
            this.dataGridViewLinkColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLinkColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewLinkColumn1.Width = 96;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "price";
            this.dataGridViewTextBoxColumn2.FillWeight = 10.15228F;
            this.dataGridViewTextBoxColumn2.HeaderText = "价格";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 66;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "nums";
            this.dataGridViewTextBoxColumn3.FillWeight = 10.15228F;
            this.dataGridViewTextBoxColumn3.HeaderText = "数量";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 66;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "onlinekey";
            this.dataGridViewTextBoxColumn4.FillWeight = 10.15228F;
            this.dataGridViewTextBoxColumn4.HeaderText = "商品ID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 82;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "provincename";
            this.dataGridViewTextBoxColumn5.HeaderText = "省份";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 66;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "city";
            this.dataGridViewTextBoxColumn6.HeaderText = "城市";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 66;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 52;
            // 
            // productName
            // 
            this.productName.DataPropertyName = "name";
            this.productName.FillWeight = 369.5432F;
            this.productName.HeaderText = "宝贝标题";
            this.productName.Name = "productName";
            this.productName.ReadOnly = true;
            this.productName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.productName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.productName.Width = 96;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "price";
            this.Price.FillWeight = 10.15228F;
            this.Price.HeaderText = "价格";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Width = 66;
            // 
            // shuliang
            // 
            this.shuliang.DataPropertyName = "nums";
            this.shuliang.FillWeight = 10.15228F;
            this.shuliang.HeaderText = "数量";
            this.shuliang.Name = "shuliang";
            this.shuliang.Width = 66;
            // 
            // code
            // 
            this.code.DataPropertyName = "onlinekey";
            this.code.FillWeight = 10.15228F;
            this.code.HeaderText = "商品ID";
            this.code.Name = "code";
            this.code.Width = 82;
            // 
            // province
            // 
            this.province.DataPropertyName = "provincename";
            this.province.HeaderText = "省份";
            this.province.Name = "province";
            this.province.Width = 66;
            // 
            // city
            // 
            this.city.DataPropertyName = "city";
            this.city.HeaderText = "城市";
            this.city.Name = "city";
            this.city.Width = 66;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 589);
            this.Controls.Add(this.statusStrip_main);
            this.Controls.Add(this.dataGridViewMaster);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "淘宝管家V1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaster)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip_main.ResumeLayout(false);
            this.statusStrip_main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_caiji;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Label lab_url;
        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.DataGridView dataGridViewMaster;
        private System.Windows.Forms.TextBox txt_OperationLog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.StatusStrip statusStrip_main;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_main;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chk_MobileDesc;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox chk_IsClearAssociated;
        private System.Windows.Forms.CheckBox chk_IsSaveIdToOutId;
        private System.Windows.Forms.CheckBox chk_IsDownPic;
        private System.Windows.Forms.CheckBox chk_IsClearAutoDeliveryDesc;
        private System.Windows.Forms.ComboBox comboBox_province;
        private System.Windows.Forms.ComboBox comboBox_City;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ContentHeader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_SaveCsvPath;
        private System.Windows.Forms.TextBox textBox_FileSavePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewLinkColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn shuliang;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn province;
        private System.Windows.Forms.DataGridViewTextBoxColumn city;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewLinkColumn dataGridViewLinkColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}

