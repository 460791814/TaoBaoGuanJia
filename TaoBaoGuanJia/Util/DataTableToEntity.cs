﻿
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaoBaoGuanJia.Model;

namespace CopyTool.Util
{
   public class DataTableToEntity
    {
 
        public static IList<T> TransDataTableToEntityList<T>(DataTable dt)where T:BaseEntity,new()
        {
            IList<T> list = new List<T>();
            T sys_sysProperty = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sys_sysProperty = new T();
                foreach (DataColumn column in dt.Columns)
                {
                    sys_sysProperty.EntityCustom.SetValue(column.ColumnName, dt.Rows[i][column]);
                }
                list.Add(sys_sysProperty);
            }
            return list;
        }
    }
}
