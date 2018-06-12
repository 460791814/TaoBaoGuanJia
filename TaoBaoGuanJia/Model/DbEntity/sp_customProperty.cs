using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_customProperty
		public class Sp_customProperty
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
		/// customPropertyName
        /// </summary>		
		private string _custompropertyname;
        public string customPropertyName
        {
            get{ return _custompropertyname; }
            set{ _custompropertyname = value; }
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
		/// allowPic
        /// </summary>		
		private int _allowpic;
        public int allowPic
        {
            get{ return _allowpic; }
            set{ _allowpic = value; }
        }        
		   
	}
}

