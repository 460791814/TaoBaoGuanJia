using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Helper
{
   public class DataHelper
    {
        public static Sys_sysSort GetSysSort(String cid)
        {

            return GetSysSortList().Find(a => a.Keys == cid);

        }
        public static List<Sys_sysSort> GetSysSortList()
        {
            string text = "select * from tb_sysSort where sysId=1 and del=0";
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_sysSort>(dataTable)?.ToList() ;

        }
        public static List<Sys_sysProperty> GetPropertyList()
        {
            string text = "select * from tb_sysProperty";
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_sysProperty>(dataTable)?.ToList();

        }
        public static List<Sys_sysProperty> GetPropertyBySortId(int sortId)
        {
            return GetPropertyList().FindAll(a => a.Sortid == sortId);
        }
        public static DataTable GetPropertyDtBySortId(int sortId)
        {
            string text = "select * from tb_sysProperty where sortid="+sortId;
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return dataTable;
        }
        public static DataTable GetPropertyValueDtBySortId(int sortId)
        {
            string text = "select * from sys_sysPropertyValue where sortid="+sortId;
           
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return dataTable;
        }
        public static Sys_sysProperty GetPropertyById(int id)
        {
            return GetPropertyList().Find(a => a.Id == id);
        
        }

        public static IList<Sys_sysProperty> GetPropertyAllTopLevelProperty(string propertyId)
        {
            IList<Sys_sysProperty> list = new List<Sys_sysProperty>();
            while (true)
            {
                Sys_sysProperty parentPropertyEntity = GetPropertyById(DataConvert.ToInt(propertyId));
                if (parentPropertyEntity == null)
                {
                    break;
                }
                list.Add(parentPropertyEntity);
                if (parentPropertyEntity.Parentid == 0)
                {
                    break;
                }
                propertyId = DataConvert.ToString((object)parentPropertyEntity.Parentid);
            }
            return list;
        }

        internal static string GetSysConfig(string v1, string v2, string v3, string v4)
        {
            string text = "select * from sys_sysConfig where sectionGroup={0} and sectionName={1} and configKey={2}";

            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            Sys_sysConfig c = DbUtil.DataTableToEntityList<Sys_sysConfig>(dataTable)?.FirstOrDefault();
            return (c==null? v4 : c.Configvalue);
        }
        public static DataTable GetSysConfigList(int sortId)
        {
            string text = "select * from sys_sysConfig " ;

            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return dataTable;
        }

        internal static Sys_sysSort GetSortBySysIdAndKeys(int sysid, string cid)
        {
            return GetSysSortList().Find(a => a.Keys == cid);
        }

        public static DataTable GetSizeDetailTableByTypeCode(string typeCode, int sysId)
        {
            string sql = "select * from Sys_sizeDetail with(nolock) where sysId=" + sysId + " and GroupType='" + DbUtil.OerateSpecialChar(typeCode) + "' and del=0";
            return SQLiteHelper.GetDataTable(sql);
        }
        public static Sys_sizeDetail GetSizeDetailBySizeValue(string sizeValue, string typeCode, int sysId)
        {
            DataTable sizeDetailTableByTypeCode = GetSizeDetailTableByTypeCode(typeCode, sysId);
            if (sizeDetailTableByTypeCode == null || sizeDetailTableByTypeCode.Rows.Count <= 0 || sizeDetailTableByTypeCode.Columns.Count <= 0)
            {
                return null;
            }
            DataRow[] array = sizeDetailTableByTypeCode.Select("SizeValue='" + DbUtil.OerateSpecialChar(sizeValue) + "'");
            if (array == null || array.Length <= 0)
            {
                return null;
            }
            Sys_sizeDetail sys_sizeDetail = new Sys_sizeDetail();
            foreach (DataColumn column in sizeDetailTableByTypeCode.Columns)
            {
                sys_sizeDetail.EntityCustom.SetValue(column.ColumnName, array[0][column]);
            }
            return sys_sizeDetail;
        }
        public static Sys_sizeGroup GetSizeGroupByGroupOnlineID(string typeCode, string groupOnlineID, int sysId)
        {
            return GetSizeGroupList().Find(a=>a.GroupType== DbUtil.OerateSpecialChar(typeCode)&&a.GroupOnlineID== DbUtil.OerateSpecialChar(groupOnlineID));
 
        }
        public static List<Sys_sizeGroup> GetSizeGroupList()
        {
            string text = "select * from tb_sizeGroup where SysId=1 and del=0";
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_sizeGroup>(dataTable)?.ToList();

        }
        internal static Sys_shopShip GetShopShipById(int v)
        {
            return null;
        }

        internal static IList<Sp_sellProperty> GetSellProperty(int id, int sysId)
        {
            throw new NotImplementedException();
        }
        public static List<Sys_sysPropertyValue> GetSysPropertyValueList()
        {
            string text = "select * from sys_sysPropertyValue";
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_sysPropertyValue>(dataTable)?.ToList();

        }
        internal static Sys_sysPropertyValue GetPropertyValueById(int v)
        {
 
            return GetSysPropertyValueList().Find(a=>a.Id==v);
        }

        internal static IList<Sys_sysPropertyValue> GetPropertyValuesByPropertyId(int v)
        {
            return GetSysPropertyValueList().FindAll(a => a.Propertyid == v);
          
        }

        internal static Sys_sysAddress GetAddressBySysIdAndCode(int sysId, string state)
        {
            //todo
            throw new NotImplementedException();
        }

        internal static IList<Sys_shopShip> GetShopShipsByShopId(int shopId)
        {
            //todo
            throw new NotImplementedException();
        }

        internal static IList<Sys_shopSort> GetShopSortsByShopId(int shopId)
        {
            //todo
            throw new NotImplementedException();
        }
    }
}
