using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_sysSort
		public class Sp_sysSort
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
		/// sysSortId
        /// </summary>		
		private int _syssortid;
        public int sysSortId
        {
            get{ return _syssortid; }
            set{ _syssortid = value; }
        }        
		/// <summary>
		/// sysSortName
        /// </summary>		
		private string _syssortname;
        public string sysSortName
        {
            get{ return _syssortname; }
            set{ _syssortname = value; }
        }        
		/// <summary>
		/// sysSortPath
        /// </summary>		
		private string _syssortpath;
        public string sysSortPath
        {
            get{ return _syssortpath; }
            set{ _syssortpath = value; }
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
		   
	}
}

