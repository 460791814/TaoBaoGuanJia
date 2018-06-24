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
        /// <summary>
        /// 系统基础数据
        /// </summary>
        private static SQLiteConnection conn = openDataConnection();
        /// <summary>
        /// 用户生产数据
        /// </summary>
        private static SQLiteConnection userConn = openUserDataConnection();

        private static SQLiteConnection openUserDataConnection()
        {
            var conn = SQLiteBaseRepository.SimpleDbConnection(System.Environment.CurrentDirectory + @"/data/tbgj.db");

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// 打开数据库链接；
        /// </summary>
        /// <returns></returns>
        private static SQLiteConnection openDataConnection()
        {
            var conn = SQLiteBaseRepository.SimpleDbConnection(System.Environment.CurrentDirectory + @"/data/data.db");
 
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        public static Sys_sysSort GetSysSort(String cid)
        {
            string sql = "SELECT * from sys_sysSort where sysId=1 and del=0 and Keys=@Keys";
            return conn.Query<Sys_sysSort>(sql,new Sys_sysSort() { Keys=cid })?.FirstOrDefault();
        }
  

        public static List<Sys_sysProperty> GetPropertyBySortId(int sortId)
        {
            string sql = "select * from sys_sysProperty where sortid=@sortid";
            return conn.Query<Sys_sysProperty>(sql, new Sys_sysProperty() { Sortid = sortId })?.ToList();
 
        }
        public static DataTable GetPropertyDtBySortId(int sortId)
        {
            string text = "select * from sys_sysProperty where sortid="+sortId;
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return dataTable;
        }
        public static DataTable GetPropertyValueDtBySortId(int sortId)
        {
            string text = "select * from sys_sysPropertyValue where sortid="+sortId;
           
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return dataTable;
        }

        internal static List<ProductItem> GetProductItemList(string ids)
        {
      
            string strSql = "select p.*,s.sysSortId,c.content,c.wirelessdesc from sp_item p left join sp_sysSort s on s.itemId=p.id left join sp_itemContent c on p.id=c.itemId where id in ("+ ids + ") and p.del=0 and p.sysId=1 order by p.showurl asc";
            return userConn.Query<ProductItem>(strSql.ToString(),null)?.ToList(); ;
        }

        public static Sys_sysProperty GetPropertyById(int id)
        {
            string sql = "SELECT * from sys_sysProperty where id=@id";
            return conn.Query<Sys_sysProperty>(sql, new Sys_sysProperty() { Id = id })?.FirstOrDefault();
         
        
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
            string sql = string.Format( "select * from sys_sysConfig where sectionGroup='{0}' and sectionName='{1}' and configKey='{2}'", v1, v2,  v3);
            Sys_sysConfig c = conn.Query<Sys_sysConfig>(sql,null)?.FirstOrDefault();
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
            string sql = "SELECT * from sys_sysSort where  sysId=1 and del=0 and Keys=@Keys";
            return conn.Query<Sys_sysSort>(sql, new Sys_sysSort() { Keys = cid })?.FirstOrDefault();
     
        }

        public static DataTable GetSizeDetailTableByTypeCode(string typeCode, int sysId)
        {
            string sql = "select * from sys_sizeDetail with(nolock) where sysId=" + sysId + " and GroupType='" + DbUtil.OerateSpecialChar(typeCode) + "' and del=0";
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
            string sql = "select * from sys_sizeGroup where SysId=1 and del=0";
            return conn.Query<Sys_sizeGroup>(sql,null)?.ToList();

        }
        internal static Sys_shopShip GetShopShipById(int v)
        {
            return null;
            string text = "select * from Sys_shopShip where id=" + v;
            return conn.Query<Sys_shopShip>(text, null)?.FirstOrDefault();
        }

        internal static IList<Sp_sellProperty> GetSellProperty(int itemid)
        {
            string text = "select * from Sp_sellProperty where itemid=" + itemid;

     
            return userConn.Query<Sp_sellProperty>(text, null)?.ToList();
        }
        public static List<Sys_sysPropertyValue> GetSysPropertyValueList()
        {
            string text = "select * from sys_sysPropertyValue";
            DataTable dataTable = SQLiteHelper.GetDataTable(text);
            return DbUtil.DataTableToEntityList<Sys_sysPropertyValue>(dataTable)?.ToList();

        }
        internal static Sys_sysPropertyValue GetPropertyValueById(int v)
        {
            string sql = "select * from sys_sysPropertyValue where id=@id";
            return conn.Query<Sys_sysPropertyValue>(sql,new Sys_sysPropertyValue() { Id=v })?.FirstOrDefault();
        }

        internal static IList<Sys_sysPropertyValue> GetPropertyValuesByPropertyId(int v)
        {
            string sql = "select * from sys_sysPropertyValue where propertyid=@propertyid";
            return conn.Query<Sys_sysPropertyValue>(sql, new Sys_sysPropertyValue() { Propertyid = v })?.ToList();
        }

        internal static Sys_sysAddress GetAddressBySysIdAndCode(int sysId, string state)
        {
            string text = "select * from Sys_sysAddress where sysid="+sysId+" and addrCode='"+ state + "' and del=0";

            return conn.Query<Sys_sysAddress>(text,null)?.FirstOrDefault();
            
        }
        /// <summary>
        /// 获取淘宝所有省市
        /// </summary>
        /// <returns></returns>
        public static List<Sys_sysAddress> GetAddressList() {
            string text = "select * from Sys_sysAddress where sysid=1 and del=0";
            return conn.Query<Sys_sysAddress>(text, null)?.ToList();
        }
        internal static IList<Sys_shopShip> GetShopShipsByShopId(int shopId)
        {
            return null;
            //todo
            string text = "select * from Sys_shopShip where shopId="+shopId;

            return conn.Query<Sys_shopShip>(text,null)?.ToList();
        }

        internal static IList<Sys_shopSort> GetShopSortsByShopId(int shopId)
        {
            return null;
            string sql = "select * from Sys_shopSort where isDelete=0 and shopId="+shopId;
            return conn.Query<Sys_shopSort>(sql,null)?.ToList();
        }

        internal static string GetSortKeyBySortId(int sysSortId)
        {
            string sql = "SELECT * from sys_sysSort where  id="+sysSortId;
            return conn.Query<Sys_sysSort>(sql, null)?.FirstOrDefault()?.Keys;
        }

        internal static Sys_sysSort GetSortById(int id)
        {
            string sql = "SELECT * from sys_sysSort where id=@id";
            return conn.Query<Sys_sysSort>(sql, new Sys_sysSort() { Id=id })?.FirstOrDefault();
  
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
            userConn.Execute(strSql.ToString(), spPicturesList);
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
            userConn.Execute(strSql.ToString(), item);
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
            userConn.Execute(strSql.ToString(), spPropertyList);
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
            userConn.Execute(strSql.ToString(), spItemContent);
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
            userConn.Execute(strSql.ToString(), spFodSecurity);
        }

        internal static IList<Sp_pictures> GetPhotos(string itemId)
        {
            string sql = "SELECT * from Sp_pictures where itemId=" + itemId;
            return userConn.Query<Sp_pictures>(sql, null)?.ToList();
        }

        internal static Sp_foodSecurity GetFoodSecurityByItemId(string itemId)
        {
            string sql = "SELECT * from Sp_foodSecurity where itemId=" + itemId;
            return userConn.Query<Sp_foodSecurity>(sql, null)?.FirstOrDefault();
        }

        internal static Sp_sysSort GetSpSysSort(int itemId)
        {
            string sql = "SELECT * from Sp_sysSort where itemId="+ itemId;
            return userConn.Query<Sp_sysSort>(sql, null)?.FirstOrDefault();
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
            userConn.Execute(strSql.ToString(), spSysSort);
        }

        internal static IList<Sys_sysProperty> GetSellPropertyBySortId(int sortId)
        {
            string text = "select * from sys_sysProperty where sortid=" + sortId;
            return conn.Query<Sys_sysProperty>(text,null)?.ToList();
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

            return userConn.Query<int>(strSql.ToString(), spItem).FirstOrDefault();
        }

        internal static IList<Sp_property> GetPropertyListByItemId(int itemId)
        {
            string sql = "SELECT * from Sp_property where itemId=" + itemId;
            return userConn.Query<Sp_property>(sql, null)?.ToList();
        }

        internal static IList<Sp_sellProperty> GetSellProperty(int itemId, int sysId)
        {
            string sql = "SELECT * from Sp_sellProperty where itemId=" + itemId;
            return userConn.Query<Sp_sellProperty>(sql, null)?.ToList();
        }
        internal static IList<Sp_item> GetProductItemList()
        {
            string sql = "SELECT * from Sp_item";
            return userConn.Query<Sp_item>(sql, null)?.ToList();
        }

        public static void AddUserConfig(tb_userconfig model)
        {
            string strSql = "insert into tb_userconfig(configkey,configvalue) values (@configkey,@configvalue)";
            conn.Execute(strSql, model);
        }
        public static void UpdateUserConfigByKey(tb_userconfig model)
        {
            string strSql = "update sys_userconfig set configvalue=@configvalue where configkey=@configkey";
            conn.Execute(strSql, model);
        }
        public static tb_userconfig GetUserConfigByKey(string key)
        {
            string strSql = "select * from  tb_userconfig  where configkey=@configkey";
            return conn.Query<tb_userconfig>(strSql, new tb_userconfig() {  configkey=key})?.FirstOrDefault();
        }
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteItemByIds(string ids) {
            string strSql = "delete from sp_item where id in ("+ids+")";
            userConn.Execute(strSql, null);
        }
    }
}
