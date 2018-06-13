using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
    public class Sys_sysAddress : BaseEntity
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
        public int Parentid
        {
            get
            {
                return DataConvert.ToInt(this.entityCus["parentid"]);
            }
            set
            {
                this.entityCus["parentid"] = value;
            }
        }
        public string Parent
        {
            get
            {
                return DataConvert.ToString(this.entityCus["parent"]);
            }
            set
            {
                this.entityCus["parent"] = value;
            }
        }
        public string Addrcode
        {
            get
            {
                return DataConvert.ToString(this.entityCus["addrcode"]);
            }
            set
            {
                this.entityCus["addrcode"] = value;
            }
        }
        public string Address
        {
            get
            {
                return DataConvert.ToString(this.entityCus["address"]);
            }
            set
            {
                this.entityCus["address"] = value;
            }
        }
        public int Levels
        {
            get
            {
                return DataConvert.ToInt(this.entityCus["levels"]);
            }
            set
            {
                this.entityCus["levels"] = value;
            }
        }
        public int Orders
        {
            get
            {
                return DataConvert.ToInt(this.entityCus["orders"]);
            }
            set
            {
                this.entityCus["orders"] = value;
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
        public Sys_sysAddress()
        {
            this.entityCus = new EntityCustom("sys_sysAddress");
        }
        public Sys_sysAddress Clone()
        {
            return new Sys_sysAddress
            {
                entityCus = this.entityCus.Clone()
            };
        }
        public static string ConnectValuesWithChar(System.Collections.Generic.IList<Sys_sysAddress> list, string fieldName, char connectChar, WrapCharType wrapCharType, bool trim)
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
        public static string ConnectValuesWithChar(System.Collections.Generic.IList<Sys_sysAddress> list, string fieldName, WrapCharType wrapCharType, bool trim)
        {
            return Sys_sysAddress.ConnectValuesWithChar(list, fieldName, ',', wrapCharType, trim);
        }
        public static System.Collections.Generic.IList<Sys_sysAddress> TransDataTableToEntityList(DataTable dt)
        {
            System.Collections.Generic.IList<Sys_sysAddress> list = new System.Collections.Generic.List<Sys_sysAddress>();
            Sys_sysAddress sys_sysAddress = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sys_sysAddress = new Sys_sysAddress();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    sys_sysAddress.EntityCustom.SetValue(dataColumn.ColumnName, dt.Rows[i][dataColumn]);
                }
                list.Add(sys_sysAddress);
            }
            return list;
        }
    }
}
