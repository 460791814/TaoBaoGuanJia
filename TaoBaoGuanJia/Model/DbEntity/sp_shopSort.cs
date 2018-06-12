using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_shopSort
		public class Sp_shopSort
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
		/// shopId
        /// </summary>		
		private int _shopid;
        public int shopId
        {
            get{ return _shopid; }
            set{ _shopid = value; }
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
		/// shopSortId
        /// </summary>		
		private int _shopsortid;
        public int shopSortId
        {
            get{ return _shopsortid; }
            set{ _shopsortid = value; }
        }        
		/// <summary>
		/// shopSort
        /// </summary>		
		private string _shopsort;
        public string shopSort
        {
            get{ return _shopsort; }
            set{ _shopsort = value; }
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

