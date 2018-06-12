using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_foodSecurity
		public class Sp_foodSecurity
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
		private long _itemid;
        public long itemId
        {
            get{ return _itemid; }
            set{ _itemid = value; }
        }        
		/// <summary>
		/// prdLicenseNo
        /// </summary>		
		private string _prdlicenseno;
        public string prdLicenseNo
        {
            get{ return _prdlicenseno; }
            set{ _prdlicenseno = value; }
        }        
		/// <summary>
		/// designCode
        /// </summary>		
		private string _designcode;
        public string designCode
        {
            get{ return _designcode; }
            set{ _designcode = value; }
        }        
		/// <summary>
		/// factory
        /// </summary>		
		private string _factory;
        public string factory
        {
            get{ return _factory; }
            set{ _factory = value; }
        }        
		/// <summary>
		/// factorySite
        /// </summary>		
		private string _factorysite;
        public string factorySite
        {
            get{ return _factorysite; }
            set{ _factorysite = value; }
        }        
		/// <summary>
		/// contact
        /// </summary>		
		private string _contact;
        public string contact
        {
            get{ return _contact; }
            set{ _contact = value; }
        }        
		/// <summary>
		/// mix
        /// </summary>		
		private string _mix;
        public string mix
        {
            get{ return _mix; }
            set{ _mix = value; }
        }        
		/// <summary>
		/// planStorage
        /// </summary>		
		private string _planstorage;
        public string planStorage
        {
            get{ return _planstorage; }
            set{ _planstorage = value; }
        }        
		/// <summary>
		/// period
        /// </summary>		
		private string _period;
        public string period
        {
            get{ return _period; }
            set{ _period = value; }
        }        
		/// <summary>
		/// foodAdditive
        /// </summary>		
		private string _foodadditive;
        public string foodAdditive
        {
            get{ return _foodadditive; }
            set{ _foodadditive = value; }
        }        
		/// <summary>
		/// supplier
        /// </summary>		
		private string _supplier;
        public string supplier
        {
            get{ return _supplier; }
            set{ _supplier = value; }
        }        
		/// <summary>
		/// productDateStart
        /// </summary>		
		private DateTime _productdatestart;
        public DateTime productDateStart
        {
            get{ return _productdatestart; }
            set{ _productdatestart = value; }
        }        
		/// <summary>
		/// productDateEnd
        /// </summary>		
		private DateTime _productdateend;
        public DateTime productDateEnd
        {
            get{ return _productdateend; }
            set{ _productdateend = value; }
        }        
		/// <summary>
		/// stockDateStart
        /// </summary>		
		private DateTime _stockdatestart;
        public DateTime stockDateStart
        {
            get{ return _stockdatestart; }
            set{ _stockdatestart = value; }
        }        
		/// <summary>
		/// stockDateEnd
        /// </summary>		
		private DateTime _stockdateend;
        public DateTime stockDateEnd
        {
            get{ return _stockdateend; }
            set{ _stockdateend = value; }
        }        
		/// <summary>
		/// healthProductNo
        /// </summary>		
		private string _healthproductno;
        public string healthProductNo
        {
            get{ return _healthproductno; }
            set{ _healthproductno = value; }
        }        
		   
	}
}

