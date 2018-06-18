using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaoBaoGuanJia.Helper;

namespace TaoBaoGuanJia.Extension
{
   public static class MainFormUtils
    {
        public static MainForm mainForm;
        public static DataGridView dataGridViewMaster;
        public static void RefreshDataGridViewMaster() {
            var list = DataHelper.GetProductItemList();
            dataGridViewMaster.DataSource = list;
        }
    }
}
