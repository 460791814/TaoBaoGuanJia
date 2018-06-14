using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TaoBaoGuanJia.Model;
using TaoBaoGuanJia.Util;
using Dapper;

namespace TaoBaoGuanJia.Helper
{
   public class DataHelper
    {
        private static SQLiteConnection conn = openDataConnection();

 
        /// <summary>
        /// 打开数据库链接；
        /// </summary>
        /// <returns></returns>
        private static SQLiteConnection openDataConnection()
        {
            var conn = SQLiteBaseRepository.SimpleDbConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

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
            string text = "select * from tb_sysPropertyValue where sortid="+sortId;
           
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return dataTable;
        }

        internal static List<ProductItem> GetProductItemList(string ids)
        {
      
            string strSql = "select p.*,s.sysSortId,c.content,c.wirelessdesc from sp_item p left join sp_sysSort s on s.itemId=p.id left join sp_itemContent c on p.id=c.itemId where id in ("+ ids + ") and p.del=0 and p.sysId=1 order by p.showurl asc";
            return conn.Query<ProductItem>(strSql.ToString(),null)?.ToList(); ;
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
            string text =string.Format( "select * from sys_sysConfig where sectionGroup='{0}' and sectionName='{1}' and configKey='{2}'", v1, v2,  v3);

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
            string sql = "select * from tb_sizeDetail with(nolock) where sysId=" + sysId + " and GroupType='" + DbUtil.OerateSpecialChar(typeCode) + "' and del=0";
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
            string text = "select * from Sys_shopShip where id=" + v;

            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_shopShip>(dataTable)?.FirstOrDefault();
        }

        internal static IList<Sp_sellProperty> GetSellProperty(int itemid)
        {
            string text = "select * from Sp_sellProperty where itemid=" + itemid;

            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sp_sellProperty>(dataTable);
        }
        public static List<Sys_sysPropertyValue> GetSysPropertyValueList()
        {
            string text = "select * from tb_sysPropertyValue";
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
            string text = "select * from Sys_sysAddress where sysid="+sysId+" and addrCode='"+ state + "' and del=0";

            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_sysAddress>(dataTable)?.FirstOrDefault();
            
        }

        internal static IList<Sys_shopShip> GetShopShipsByShopId(int shopId)
        {
            return null;
            //todo
            string text = "select * from Sys_shopShip where shopId="+shopId;

            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_shopShip>(dataTable);
        }

        internal static IList<Sys_shopSort> GetShopSortsByShopId(int shopId)
        {
            return null;
            string text = "select * from Sys_shopSort where isDelete=0 and shopId="+shopId;
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_shopSort>(dataTable);
        }

        internal static string GetSortKeyBySortId(int sysSortId)
        {
            string sql = "SELECT * from tb_sysSort where  id="+sysSortId;
            return conn.Query<Sys_sysSort>(sql, null)?.FirstOrDefault()?.Keys;
        }

        internal static Sys_sysSort GetSortById(int id)
        {
            return GetSysSortList().Find(a => a.Id == id);
        }

        internal static void InsertSpPictures(int num, IList<Sp_pictures> spPicturesList)
        {
            if (spPicturesList != null) {
                foreach (var item in spPicturesList)
                {
                    item.Itemid = num;
                    InsertSpPictures(item);
                }
            }
        }
        internal static void InsertSpPictures(Sp_pictures spPicturesList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sp_pictures(");
            strSql.Append("ItemId,localPath,url,keys,picIndex,isModify,shopId");
            strSql.Append(") values (");
            strSql.Append("@ItemId,@localPath,@url,@keys,@picIndex,@isModify,@shopId");
            strSql.Append(") ;");
            conn.Execute(strSql.ToString(), spPicturesList);
        }

        internal static void InsertSpSellPropertyList(int num, IList<Sp_sellProperty> spSellPropertyList)
        {
            if (spSellPropertyList != null)
            {
                foreach (var item in spSellPropertyList)
                {
                    item.Itemid = num;
                    InsertSpSellProperty(item);
                }
            }
        }

        private static void InsertSpSellProperty(Sp_sellProperty item)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sp_sellProperty(");
            strSql.Append("modifyTime,name,newOrOld,salePrice,salePriceBeginDate,salePriceEndDate,standardProductId,state,productIdType,saleAttr,itemId,skuId,barcode,shopId,sysId,sellProInfos,price,nums,code,remark");
            strSql.Append(") values (");
            strSql.Append("@modifyTime,@name,@newOrOld,@salePrice,@salePriceBeginDate,@salePriceEndDate,@standardProductId,@state,@productIdType,@saleAttr,@itemId,@skuId,@barcode,@shopId,@sysId,@sellProInfos,@price,@nums,@code,@remark");
            strSql.Append(") ;");
            conn.Execute(strSql.ToString(), item);
        }

        internal static void InsertSpPropertyList(int num, IList<Sp_property> spPropertyList)
        {
            if (spPropertyList != null)
            {
                foreach (var item in spPropertyList)
                {
                    item.Itemid = num;
                    InsertSpProperty(item);
                }
            }
        }
        internal static void InsertSpProperty(Sp_property spPropertyList)
        {
            if (spPropertyList == null) { return; }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sp_property(");
            strSql.Append("isSellPro,Aliasname,picUrl,url,itemId,shopId,sysId,propertyId,name,value,modifyTime,propertyKeys");
            strSql.Append(") values (");
            strSql.Append("@isSellPro,@Aliasname,@picUrl,@url,@itemId,@shopId,@sysId,@propertyId,@name,@value,@modifyTime,@propertyKeys");
            strSql.Append(") ;");
            conn.Execute(strSql.ToString(), spPropertyList);
        }
        internal static void InsertSpItemContent(Sp_itemContent spItemContent)
        {
            if (spItemContent == null) { return; }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sp_itemContent(");
            strSql.Append("itemId,content,uploadFailMsg,faultreason,modifyDate,wirelessdesc");
            strSql.Append(") values (");
            strSql.Append("@itemId,@content,@uploadFailMsg,@faultreason,@modifyDate,@wirelessdesc");
            strSql.Append(") ;");
            conn.Execute(strSql.ToString(), spItemContent);
        }

        internal static void InsertFoodSecurity(Sp_foodSecurity spFodSecurity)
        {
            if (spFodSecurity == null) { return; }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sp_foodSecurity(");
            strSql.Append("period,foodAdditive,supplier,productDateStart,productDateEnd,stockDateStart,stockDateEnd,healthProductNo,itemId,prdLicenseNo,designCode,factory,factorySite,contact,mix,planStorage");
            strSql.Append(") values (");
            strSql.Append("@period,@foodAdditive,@supplier,@productDateStart,@productDateEnd,@stockDateStart,@stockDateEnd,@healthProductNo,@itemId,@prdLicenseNo,@designCode,@factory,@factorySite,@contact,@mix,@planStorage");
            strSql.Append(") ;");
            conn.Execute(strSql.ToString(), spFodSecurity);
        }

        internal static IList<Sp_pictures> GetPhotos(string itemId)
        {
            string sql = "SELECT * from Sp_pictures where itemId=" + itemId;
            return conn.Query<Sp_pictures>(sql, null)?.ToList();
        }

        internal static Sp_foodSecurity GetFoodSecurityByItemId(string itemId)
        {
            string sql = "SELECT * from Sp_foodSecurity where itemId=" + itemId;
            return conn.Query<Sp_foodSecurity>(sql, null)?.FirstOrDefault();
        }

        internal static Sp_sysSort GetSpSysSort(int itemId)
        {
            string sql = "SELECT * from Sp_sysSort where itemId="+ itemId;
            return conn.Query<Sp_sysSort>(sql, null)?.FirstOrDefault();
        }

        internal static IList<Sys_sysEnumItem> GetEnumItemByCode(Sys_sysEnumItem model)
        {
            string sql = "SELECT * from sys_sysEnumItem where enumtypecode=@enumtypecode and sysid=@Sysid";
            return conn.Query<Sys_sysEnumItem>(sql, model)?.ToList();
        }

        internal static void InsertSpSysSort(Sp_sysSort spSysSort)
        {
            if (spSysSort == null) { return; }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sp_sysSort(");
            strSql.Append("itemId,shopId,sysId,sysSortId,sysSortName,sysSortPath,modifyTime");
            strSql.Append(") values (");
            strSql.Append("@itemId,@shopId,@sysId,@sysSortId,@sysSortName,@sysSortPath,@modifyTime");
            strSql.Append(") ;");
            conn.Execute(strSql.ToString(), spSysSort);
        }

        internal static IList<Sys_sysProperty> GetSellPropertyBySortId(int sortId)
        {
            string text = "select * from tb_sysProperty where sortid=" + sortId;
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_sysProperty>(dataTable);
        }

        internal static int InsertSpItem(Sp_item spItem)
        {
            if (spItem == null) { return 0; }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sp_item(");
            strSql.Append("photo,sellType,sellTypeName,price,priceRise,nums,limitNums,validDate,shipWay,useShipTpl,name,shipTplId,shipWayName,shipSlow,shipFast,shipEMS,onSell,onSellDate,onSellHour,onSellMin,payType,sysId,isRmd,isReturn,isTicket,ticketName,isRepair,repairName,isAutoPub,isVirtual,sszgUserName,kcItemId,shopId,code,kcItemName,state,onlineKey,listTime,delistTime,photoModified,detailUrl,showUrl,shopSortIds,newOrOld,shopSortNames,operateTypes,crtDate,modifyDate,synchState,synchDetail,del,discount,integrity,weight,province,subStock,size,afterSaleId,isPaipaiNewStock,barcode,sellPoint,provinceName,city,cityName");
            strSql.Append(") values (");
            strSql.Append("@photo,@sellType,@sellTypeName,@price,@priceRise,@nums,@limitNums,@validDate,@shipWay,@useShipTpl,@name,@shipTplId,@shipWayName,@shipSlow,@shipFast,@shipEMS,@onSell,@onSellDate,@onSellHour,@onSellMin,@payType,@sysId,@isRmd,@isReturn,@isTicket,@ticketName,@isRepair,@repairName,@isAutoPub,@isVirtual,@sszgUserName,@kcItemId,@shopId,@code,@kcItemName,@state,@onlineKey,@listTime,@delistTime,@photoModified,@detailUrl,@showUrl,@shopSortIds,@newOrOld,@shopSortNames,@operateTypes,@crtDate,@modifyDate,@synchState,@synchDetail,@del,@discount,@integrity,@weight,@province,@subStock,@size,@afterSaleId,@isPaipaiNewStock,@barcode,@sellPoint,@provinceName,@city,@cityName");
            strSql.Append(") ;select last_insert_rowid() newid;");

            return conn.Query<int>(strSql.ToString(), spItem).FirstOrDefault();
        }

        internal static IList<Sp_property> GetPropertyListByItemId(int itemId)
        {
            string sql = "SELECT * from Sp_property where itemId=" + itemId;
            return conn.Query<Sp_property>(sql, null)?.ToList();
        }

        internal static IList<Sp_sellProperty> GetSellProperty(int itemId, int sysId)
        {
            string sql = "SELECT * from Sp_sellProperty where itemId=" + itemId;
            return conn.Query<Sp_sellProperty>(sql, null)?.ToList();
        }
    }
}
