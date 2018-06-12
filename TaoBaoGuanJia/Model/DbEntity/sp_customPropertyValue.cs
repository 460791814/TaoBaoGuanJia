using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_customPropertyValue
		public class Sp_customPropertyValue
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
		/// sysId
        /// </summary>		
		private int _sysid;
        public int sysId
        {
            get{ return _sysid; }
            set{ _sysid = value; }
        }        
		/// <summary>
		/// customPropertyId
        /// </summary>		
		private int _custompropertyid;
        public int customPropertyId
        {
            get{ return _custompropertyid; }
            set{ _custompropertyid = value; }
        }        
		/// <summary>
		/// customPropertyName
        /// </summary>		
		private string _custompropertyname;
        public string customPropertyName
        {
            get{ return _custompropertyname; }
            set{ _custompropertyname = value; }
        }        
		/// <summary>
		/// customPropertyValue
        /// </summary>		
		private string _custompropertyvalue;
        public string customPropertyValue
        {
            get{ return _custompropertyvalue; }
            set{ _custompropertyvalue = value; }
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
		/// picFile
        /// </summary>		
		private string _picfile;
        public string picFile
        {
            get{ return _picfile; }
            set{ _picfile = value; }
        }        
		/// <summary>
		/// customPropertyValueId
        /// </summary>		
		private string _custompropertyvalueid;
        public string customPropertyValueId
        {
            get{ return _custompropertyvalueid; }
            set{ _custompropertyvalueid = value; }
        }        
		/// <summary>
		/// customPropertyAliasName
        /// </summary>		
		private string _custompropertyaliasname;
        public string customPropertyAliasName
        {
            get{ return _custompropertyaliasname; }
            set{ _custompropertyaliasname = value; }
        }        
		/// <summary>
		/// Custompropertyvaluename
        /// </summary>		
		private string _custompropertyvaluename;
        public string Custompropertyvaluename
        {
            get{ return _custompropertyvaluename; }
            set{ _custompropertyvaluename = value; }
        }        
		   
	}
}

