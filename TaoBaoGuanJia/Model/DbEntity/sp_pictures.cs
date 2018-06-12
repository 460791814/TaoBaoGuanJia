using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_pictures
		public class Sp_pictures
    {
   		     
      	/// <summary>
		/// ID
        /// </summary>		
		private int _id;
        public int ID
        {
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// ItemId
        /// </summary>		
		private int _itemid;
        public int ItemId
        {
            get{ return _itemid; }
            set{ _itemid = value; }
        }        
		/// <summary>
		/// localPath
        /// </summary>		
		private string _localpath;
        public string localPath
        {
            get{ return _localpath; }
            set{ _localpath = value; }
        }        
		/// <summary>
		/// url
        /// </summary>		
		private string _url;
        public string url
        {
            get{ return _url; }
            set{ _url = value; }
        }        
		/// <summary>
		/// keys
        /// </summary>		
		private string _keys;
        public string keys
        {
            get{ return _keys; }
            set{ _keys = value; }
        }        
		/// <summary>
		/// picIndex
        /// </summary>		
		private int _picindex;
        public int picIndex
        {
            get{ return _picindex; }
            set{ _picindex = value; }
        }        
		/// <summary>
		/// isModify
        /// </summary>		
		private int _ismodify;
        public int isModify
        {
            get{ return _ismodify; }
            set{ _ismodify = value; }
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
		   
	}
}

