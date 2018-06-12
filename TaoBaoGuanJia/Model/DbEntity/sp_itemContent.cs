using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_itemContent
		public class Sp_itemContent
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
		/// content
        /// </summary>		
		private string _content;
        public string content
        {
            get{ return _content; }
            set{ _content = value; }
        }        
		/// <summary>
		/// uploadFailMsg
        /// </summary>		
		private string _uploadfailmsg;
        public string uploadFailMsg
        {
            get{ return _uploadfailmsg; }
            set{ _uploadfailmsg = value; }
        }        
		/// <summary>
		/// faultreason
        /// </summary>		
		private string _faultreason;
        public string faultreason
        {
            get{ return _faultreason; }
            set{ _faultreason = value; }
        }        
		/// <summary>
		/// modifyDate
        /// </summary>		
		private DateTime _modifydate;
        public DateTime modifyDate
        {
            get{ return _modifydate; }
            set{ _modifydate = value; }
        }        
		/// <summary>
		/// wirelessdesc
        /// </summary>		
		private string _wirelessdesc;
        public string wirelessdesc
        {
            get{ return _wirelessdesc; }
            set{ _wirelessdesc = value; }
        }        
		   
	}
}

