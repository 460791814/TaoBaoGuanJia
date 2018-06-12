using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TaoBaoGuanJia.Model{
	 	//sp_item
		public class Sp_item
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
		/// name
        /// </summary>		
		private string _name;
        public string name
        {
            get{ return _name; }
            set{ _name = value; }
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
		/// shopId
        /// </summary>		
		private int _shopid;
        public int shopId
        {
            get{ return _shopid; }
            set{ _shopid = value; }
        }        
		/// <summary>
		/// newOrOld
        /// </summary>		
		private string _neworold;
        public string newOrOld
        {
            get{ return _neworold; }
            set{ _neworold = value; }
        }        
		/// <summary>
		/// province
        /// </summary>		
		private string _province;
        public string province
        {
            get{ return _province; }
            set{ _province = value; }
        }        
		/// <summary>
		/// provinceName
        /// </summary>		
		private string _provincename;
        public string provinceName
        {
            get{ return _provincename; }
            set{ _provincename = value; }
        }        
		/// <summary>
		/// city
        /// </summary>		
		private string _city;
        public string city
        {
            get{ return _city; }
            set{ _city = value; }
        }        
		/// <summary>
		/// cityName
        /// </summary>		
		private string _cityname;
        public string cityName
        {
            get{ return _cityname; }
            set{ _cityname = value; }
        }        
		/// <summary>
		/// photo
        /// </summary>		
		private string _photo;
        public string photo
        {
            get{ return _photo; }
            set{ _photo = value; }
        }        
		/// <summary>
		/// sellType
        /// </summary>		
		private string _selltype;
        public string sellType
        {
            get{ return _selltype; }
            set{ _selltype = value; }
        }        
		/// <summary>
		/// sellTypeName
        /// </summary>		
		private string _selltypename;
        public string sellTypeName
        {
            get{ return _selltypename; }
            set{ _selltypename = value; }
        }        
		/// <summary>
		/// price
        /// </summary>		
		private decimal _price;
        public decimal price
        {
            get{ return _price; }
            set{ _price = value; }
        }        
		/// <summary>
		/// priceRise
        /// </summary>		
		private decimal _pricerise;
        public decimal priceRise
        {
            get{ return _pricerise; }
            set{ _pricerise = value; }
        }        
		/// <summary>
		/// nums
        /// </summary>		
		private int _nums;
        public int nums
        {
            get{ return _nums; }
            set{ _nums = value; }
        }        
		/// <summary>
		/// limitNums
        /// </summary>		
		private int _limitnums;
        public int limitNums
        {
            get{ return _limitnums; }
            set{ _limitnums = value; }
        }        
		/// <summary>
		/// validDate
        /// </summary>		
		private string _validdate;
        public string validDate
        {
            get{ return _validdate; }
            set{ _validdate = value; }
        }        
		/// <summary>
		/// shipWay
        /// </summary>		
		private string _shipway;
        public string shipWay
        {
            get{ return _shipway; }
            set{ _shipway = value; }
        }        
		/// <summary>
		/// useShipTpl
        /// </summary>		
		private string _useshiptpl;
        public string useShipTpl
        {
            get{ return _useshiptpl; }
            set{ _useshiptpl = value; }
        }        
		/// <summary>
		/// shipTplId
        /// </summary>		
		private int _shiptplid;
        public int shipTplId
        {
            get{ return _shiptplid; }
            set{ _shiptplid = value; }
        }        
		/// <summary>
		/// shipWayName
        /// </summary>		
		private string _shipwayname;
        public string shipWayName
        {
            get{ return _shipwayname; }
            set{ _shipwayname = value; }
        }        
		/// <summary>
		/// shipSlow
        /// </summary>		
		private decimal _shipslow;
        public decimal shipSlow
        {
            get{ return _shipslow; }
            set{ _shipslow = value; }
        }        
		/// <summary>
		/// shipFast
        /// </summary>		
		private decimal _shipfast;
        public decimal shipFast
        {
            get{ return _shipfast; }
            set{ _shipfast = value; }
        }        
		/// <summary>
		/// shipEMS
        /// </summary>		
		private decimal _shipems;
        public decimal shipEMS
        {
            get{ return _shipems; }
            set{ _shipems = value; }
        }        
		/// <summary>
		/// onSell
        /// </summary>		
		private string _onsell;
        public string onSell
        {
            get{ return _onsell; }
            set{ _onsell = value; }
        }        
		/// <summary>
		/// onSellDate
        /// </summary>		
		private DateTime _onselldate;
        public DateTime onSellDate
        {
            get{ return _onselldate; }
            set{ _onselldate = value; }
        }        
		/// <summary>
		/// onSellHour
        /// </summary>		
		private string _onsellhour;
        public string onSellHour
        {
            get{ return _onsellhour; }
            set{ _onsellhour = value; }
        }        
		/// <summary>
		/// onSellMin
        /// </summary>		
		private string _onsellmin;
        public string onSellMin
        {
            get{ return _onsellmin; }
            set{ _onsellmin = value; }
        }        
		/// <summary>
		/// payType
        /// </summary>		
		private string _paytype;
        public string payType
        {
            get{ return _paytype; }
            set{ _paytype = value; }
        }        
		/// <summary>
		/// isRmd
        /// </summary>		
		private string _isrmd;
        public string isRmd
        {
            get{ return _isrmd; }
            set{ _isrmd = value; }
        }        
		/// <summary>
		/// isReturn
        /// </summary>		
		private string _isreturn;
        public string isReturn
        {
            get{ return _isreturn; }
            set{ _isreturn = value; }
        }        
		/// <summary>
		/// isTicket
        /// </summary>		
		private string _isticket;
        public string isTicket
        {
            get{ return _isticket; }
            set{ _isticket = value; }
        }        
		/// <summary>
		/// ticketName
        /// </summary>		
		private string _ticketname;
        public string ticketName
        {
            get{ return _ticketname; }
            set{ _ticketname = value; }
        }        
		/// <summary>
		/// isRepair
        /// </summary>		
		private string _isrepair;
        public string isRepair
        {
            get{ return _isrepair; }
            set{ _isrepair = value; }
        }        
		/// <summary>
		/// repairName
        /// </summary>		
		private string _repairname;
        public string repairName
        {
            get{ return _repairname; }
            set{ _repairname = value; }
        }        
		/// <summary>
		/// isAutoPub
        /// </summary>		
		private string _isautopub;
        public string isAutoPub
        {
            get{ return _isautopub; }
            set{ _isautopub = value; }
        }        
		/// <summary>
		/// isVirtual
        /// </summary>		
		private string _isvirtual;
        public string isVirtual
        {
            get{ return _isvirtual; }
            set{ _isvirtual = value; }
        }        
		/// <summary>
		/// sszgUserName
        /// </summary>		
		private string _sszgusername;
        public string sszgUserName
        {
            get{ return _sszgusername; }
            set{ _sszgusername = value; }
        }        
		/// <summary>
		/// kcItemId
        /// </summary>		
		private int _kcitemid;
        public int kcItemId
        {
            get{ return _kcitemid; }
            set{ _kcitemid = value; }
        }        
		/// <summary>
		/// code
        /// </summary>		
		private string _code;
        public string code
        {
            get{ return _code; }
            set{ _code = value; }
        }        
		/// <summary>
		/// kcItemName
        /// </summary>		
		private string _kcitemname;
        public string kcItemName
        {
            get{ return _kcitemname; }
            set{ _kcitemname = value; }
        }        
		/// <summary>
		/// state
        /// </summary>		
		private int _state;
        public int state
        {
            get{ return _state; }
            set{ _state = value; }
        }        
		/// <summary>
		/// onlineKey
        /// </summary>		
		private string _onlinekey;
        public string onlineKey
        {
            get{ return _onlinekey; }
            set{ _onlinekey = value; }
        }        
		/// <summary>
		/// listTime
        /// </summary>		
		private DateTime _listtime;
        public DateTime listTime
        {
            get{ return _listtime; }
            set{ _listtime = value; }
        }        
		/// <summary>
		/// delistTime
        /// </summary>		
		private DateTime _delisttime;
        public DateTime delistTime
        {
            get{ return _delisttime; }
            set{ _delisttime = value; }
        }        
		/// <summary>
		/// photoModified
        /// </summary>		
		private int _photomodified;
        public int photoModified
        {
            get{ return _photomodified; }
            set{ _photomodified = value; }
        }        
		/// <summary>
		/// detailUrl
        /// </summary>		
		private string _detailurl;
        public string detailUrl
        {
            get{ return _detailurl; }
            set{ _detailurl = value; }
        }        
		/// <summary>
		/// showUrl
        /// </summary>		
		private string _showurl;
        public string showUrl
        {
            get{ return _showurl; }
            set{ _showurl = value; }
        }        
		/// <summary>
		/// shopSortIds
        /// </summary>		
		private string _shopsortids;
        public string shopSortIds
        {
            get{ return _shopsortids; }
            set{ _shopsortids = value; }
        }        
		/// <summary>
		/// shopSortNames
        /// </summary>		
		private string _shopsortnames;
        public string shopSortNames
        {
            get{ return _shopsortnames; }
            set{ _shopsortnames = value; }
        }        
		/// <summary>
		/// operateTypes
        /// </summary>		
		private string _operatetypes;
        public string operateTypes
        {
            get{ return _operatetypes; }
            set{ _operatetypes = value; }
        }        
		/// <summary>
		/// crtDate
        /// </summary>		
		private DateTime _crtdate;
        public DateTime crtDate
        {
            get{ return _crtdate; }
            set{ _crtdate = value; }
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
		/// synchState
        /// </summary>		
		private int _synchstate;
        public int synchState
        {
            get{ return _synchstate; }
            set{ _synchstate = value; }
        }        
		/// <summary>
		/// synchDetail
        /// </summary>		
		private int _synchdetail;
        public int synchDetail
        {
            get{ return _synchdetail; }
            set{ _synchdetail = value; }
        }        
		/// <summary>
		/// del
        /// </summary>		
		private int _del;
        public int del
        {
            get{ return _del; }
            set{ _del = value; }
        }        
		/// <summary>
		/// discount
        /// </summary>		
		private string _discount;
        public string discount
        {
            get{ return _discount; }
            set{ _discount = value; }
        }        
		/// <summary>
		/// integrity
        /// </summary>		
		private int _integrity;
        public int integrity
        {
            get{ return _integrity; }
            set{ _integrity = value; }
        }        
		/// <summary>
		/// weight
        /// </summary>		
		private decimal _weight;
        public decimal weight
        {
            get{ return _weight; }
            set{ _weight = value; }
        }        
		/// <summary>
		/// subStock
        /// </summary>		
		private int _substock;
        public int subStock
        {
            get{ return _substock; }
            set{ _substock = value; }
        }        
		/// <summary>
		/// size
        /// </summary>		
		private string _size;
        public string size
        {
            get{ return _size; }
            set{ _size = value; }
        }        
		/// <summary>
		/// afterSaleId
        /// </summary>		
		private int _aftersaleid;
        public int afterSaleId
        {
            get{ return _aftersaleid; }
            set{ _aftersaleid = value; }
        }        
		/// <summary>
		/// isPaipaiNewStock
        /// </summary>		
		private int _ispaipainewstock;
        public int isPaipaiNewStock
        {
            get{ return _ispaipainewstock; }
            set{ _ispaipainewstock = value; }
        }        
		/// <summary>
		/// barcode
        /// </summary>		
		private string _barcode;
        public string barcode
        {
            get{ return _barcode; }
            set{ _barcode = value; }
        }        
		/// <summary>
		/// sellPoint
        /// </summary>		
		private string _sellpoint;
        public string sellPoint
        {
            get{ return _sellpoint; }
            set{ _sellpoint = value; }
        }        
		   
	}
}

