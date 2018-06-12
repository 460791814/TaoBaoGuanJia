using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_property
		public class Sp_property
    {
   		     
      	/// <summary>
		/// id
        /// </summary>		
		private int _id;
        public int id
        {
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// itemId
        /// </summary>		
		private int _itemid;
        public int itemId
        {
            get{ return _itemid; }
            set{ _itemid = value; }
        }        
		/// <summary>
		/// shopId
        /// </summary>		
		private int _shopid;
        public int shopId
        {
            get{ return _shopid; }
            set{ _shopid = value; }
        }        
		/// <summary>
		/// sysId
        /// </summary>		
		private int _sysid;
        public int sysId
        {
            get{ return _sysid; }
            set{ _sysid = value; }
        }        
		/// <summary>
		/// propertyId
        /// </summary>		
		private int _propertyid;
        public int Propertyid
        {
            get{ return _propertyid; }
            set{ _propertyid = value; }
        }        
		/// <summary>
		/// name
        /// </summary>		
		private string _name;
        public string Name
        {
            get{ return _name; }
            set{ _name = value; }
        }        
		/// <summary>
		/// value
        /// </summary>		
		private string _value;
        public string Value
        {
            get{ return _value; }
            set{ _value = value; }
        }        
		/// <summary>
		/// modifyTime
        /// </summary>		
		private DateTime _modifytime;
        public DateTime modifyTime
        {
            get{ return _modifytime; }
            set{ _modifytime = value; }
        }        
		/// <summary>
		/// propertyKeys
        /// </summary>		
		private string _propertykeys;
        public string propertyKeys
        {
            get{ return _propertykeys; }
            set{ _propertykeys = value; }
        }        
		/// <summary>
		/// isSellPro
        /// </summary>		
		private int _issellpro;
        public int Issellpro
        {
            get{ return _issellpro; }
            set{ _issellpro = value; }
        }        
		/// <summary>
		/// Aliasname
        /// </summary>		
		private string _aliasname;
        public string Aliasname
        {
            get{ return _aliasname; }
            set{ _aliasname = value; }
        }        
		/// <summary>
		/// picUrl
        /// </summary>		
		private string _picurl;
        public string PicUrl
        {
            get{ return _picurl; }
            set{ _picurl = value; }
        }        
		/// <summary>
		/// url
        /// </summary>		
		private string _url;
        public string Url
        {
            get{ return _url; }
            set{ _url = value; }
        }
        public Sys_sysProperty SysProperty
        {
            get;
            set;
        }
    }
}

