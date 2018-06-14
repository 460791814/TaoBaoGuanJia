using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
    public class Sys_sysEnumItem : BaseEntity
    {
        private EntityCustom entityCus;
        public override EntityCustom EntityCustom
        {
            get
            {
                return this.entityCus;
            }
        }
        public int Id
        {
            get
            {
                return DataConvert.ToInt(this.entityCus["id"]);
            }
            set
            {
                this.entityCus["id"] = value;
            }
        }
        public int Sysid
        {
            get
            {
                return DataConvert.ToInt(this.entityCus["sysid"]);
            }
            set
            {
                this.entityCus["sysid"] = value;
            }
        }
        public string Enumtypecode
        {
            get
            {
                return DataConvert.ToString(this.entityCus["enumtypecode"]);
            }
            set
            {
                this.entityCus["enumtypecode"] = value;
            }
        }
        public string Enumitemname
        {
            get
            {
                return DataConvert.ToString(this.entityCus["enumitemname"]);
            }
            set
            {
                this.entityCus["enumitemname"] = value;
            }
        }
        public string Value
        {
            get
            {
                return DataConvert.ToString(this.entityCus["value"]);
            }
            set
            {
                this.entityCus["value"] = value;
            }
        }
        public int Sequence
        {
            get
            {
                return DataConvert.ToInt(this.entityCus["sequence"]);
            }
            set
            {
                this.entityCus["sequence"] = value;
            }
        }
        public string Remark
        {
            get
            {
                return DataConvert.ToString(this.entityCus["remark"]);
            }
            set
            {
                this.entityCus["remark"] = value;
            }
        }
        public System.DateTime Modifytime
        {
            get
            {
                return DataConvert.ToDateTime(this.entityCus["modifytime"]);
            }
            set
            {
                this.entityCus["modifytime"] = value;
            }
        }
        public int Del
        {
            get
            {
                return DataConvert.ToInt(this.entityCus["del"]);
            }
            set
            {
                this.entityCus["del"] = value;
            }
        }
        public Sys_sysEnumItem()
        {
            this.entityCus = new EntityCustom("sys_sysEnumItem");
        }
        public Sys_sysEnumItem Clone()
        {
            return new Sys_sysEnumItem
            {
                entityCus = this.entityCus.Clone()
            };
        }
        public static string ConnectValuesWithChar(System.Collections.Generic.IList<Sys_sysEnumItem> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            System.Collections.Generic.IList<EntityCustom> list2 = new System.Collections.Generic.List<EntityCustom>();
            for (int i = 0; i < list.Count; i++)
            {
                list2.Add(list[i].EntityCustom);
            }
            return ValuesConnectUtil.ConnectValuesWithChar(list2, fieldName, connectChar, wrapCharType, trim);
        }
        public static string ConnectValuesWithChar(System.Collections.Generic.IList<Sys_sysEnumItem> list, string fieldName, WrapCharType wrapCharType, bool trim)
        {
            return Sys_sysEnumItem.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
        }
        public static System.Collections.Generic.IList<Sys_sysEnumItem> TransDataTableToEntityList(DataTable dt)
        {
            System.Collections.Generic.IList<Sys_sysEnumItem> list = new System.Collections.Generic.List<Sys_sysEnumItem>();
            Sys_sysEnumItem sys_sysEnumItem = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sys_sysEnumItem = new Sys_sysEnumItem();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    sys_sysEnumItem.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
                }
                list.Add(sys_sysEnumItem);
            }
            return list;
        }
    }
}
