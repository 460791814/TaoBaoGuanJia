﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaoBaoGuanJia.Helper;
using TaoBaoGuanJia.Service;

namespace TaoBaoGuanJia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_caiji_Click(object sender, EventArgs e)
        {
            //  Test();
            new TaoBaoService().CollectToDB("568769081629");
         
        }

        private void Test()
        {
          int a=  DataHelper.InsertSpItem(new Model.Sp_item() {
                 Name="123",
                 
            });
           // var dd= DataHelper.GetSysSortList();
        }

        private void TaoBaoCaiJi()
        {
            TaoBaoService taobaoService = new TaoBaoService();

        }
    }
}
