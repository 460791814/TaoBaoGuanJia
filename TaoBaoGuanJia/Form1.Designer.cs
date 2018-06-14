namespace TaoBaoGuanJia
{
    partial class Form1
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
            this.btn_caiji = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
            this.lab_url = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridViewMaster = new System.Windows.Forms.DataGridView();
            this.txt_log = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_caiji
            // 
            this.btn_caiji.Location = new System.Drawing.Point(609, 27);
            this.btn_caiji.Name = "btn_caiji";
            this.btn_caiji.Size = new System.Drawing.Size(75, 23);
            this.btn_caiji.TabIndex = 0;
            this.btn_caiji.Text = "采集";
            this.btn_caiji.UseVisualStyleBackColor = true;
            this.btn_caiji.Click += new System.EventHandler(this.btn_caiji_Click);
            // 
            // btn_export
            // 
            this.btn_export.Location = new System.Drawing.Point(16, 390);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(102, 23);
            this.btn_export.TabIndex = 1;
            this.btn_export.Text = "导出CSV文件";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // lab_url
            // 
            this.lab_url.AutoSize = true;
            this.lab_url.Location = new System.Drawing.Point(13, 30);
            this.lab_url.Name = "lab_url";
            this.lab_url.Size = new System.Drawing.Size(52, 15);
            this.lab_url.TabIndex = 2;
            this.lab_url.Text = "地址：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(71, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(532, 25);
            this.textBox1.TabIndex = 3;
            // 
            // dataGridViewMaster
            // 
            this.dataGridViewMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMaster.Location = new System.Drawing.Point(16, 72);
            this.dataGridViewMaster.Name = "dataGridViewMaster";
            this.dataGridViewMaster.RowTemplate.Height = 27;
            this.dataGridViewMaster.Size = new System.Drawing.Size(668, 185);
            this.dataGridViewMaster.TabIndex = 4;
            // 
            // txt_log
            // 
            this.txt_log.Location = new System.Drawing.Point(18, 280);
            this.txt_log.Multiline = true;
            this.txt_log.Name = "txt_log";
            this.txt_log.Size = new System.Drawing.Size(666, 88);
            this.txt_log.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 438);
            this.Controls.Add(this.txt_log);
            this.Controls.Add(this.dataGridViewMaster);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lab_url);
            this.Controls.Add(this.btn_export);
            this.Controls.Add(this.btn_caiji);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_caiji;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Label lab_url;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridViewMaster;
        private System.Windows.Forms.TextBox txt_log;
    }
}

