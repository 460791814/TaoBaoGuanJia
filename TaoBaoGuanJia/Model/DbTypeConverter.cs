
using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

namespace TaoBaoGuanJia.Model
{
	public sealed class DbTypeConverter
	{
		public static DbType GetDbTypeByValue(object fieldValue)
		{
			DbType result = DbType.String;
			if (fieldValue == null)
			{
				return result;
			}
			Type type = fieldValue.GetType();
			if (type.Equals(typeof(DateTime)))
			{
				result = DbType.DateTime;
			}
			else if (type.Equals(typeof(int)))
			{
				result = DbType.Int32;
			}
			else if (type.Equals(typeof(bool)))
			{
				result = DbType.Boolean;
			}
			else if (type.Equals(typeof(double)))
			{
				result = DbType.Double;
			}
			else if (type.Equals(typeof(byte[])))
			{
				result = DbType.Binary;
			}
			else if (type.Equals(typeof(float)))
			{
				result = DbType.Single;
			}
			else if (type.Equals(typeof(decimal)))
			{
				result = DbType.Decimal;
			}
			else if (type.Equals(typeof(short)))
			{
				result = DbType.Int16;
			}
			else if (type.Equals(typeof(long)))
			{
				result = DbType.Int64;
			}
			return result;
		}

		public static OleDbType GetOleDbParameterType(DbType type)
		{
			OleDbType oleDbType = OleDbType.VarChar;
			switch (type)
			{
			case DbType.Boolean:
				return OleDbType.Boolean;
			case DbType.DateTime:
				return OleDbType.DBTimeStamp;
			case DbType.Date:
				return OleDbType.DBDate;
			case DbType.Decimal:
				return OleDbType.Decimal;
			case DbType.Binary:
				return OleDbType.LongVarBinary;
			case DbType.Double:
				return OleDbType.Double;
			case DbType.Single:
				return OleDbType.Single;
			case DbType.Int16:
				return OleDbType.SmallInt;
			case DbType.Int32:
				return OleDbType.Integer;
			case DbType.Currency:
				return OleDbType.Currency;
			case DbType.Int64:
				return OleDbType.BigInt;
			case DbType.Object:
				return OleDbType.Variant;
			case DbType.String:
				return OleDbType.VarChar;
			default:
				return OleDbType.VarChar;
			}
		}

		public static SqlDbType GetSqlDbParameterType(DbType type, string typeString)
		{
			SqlDbType result = SqlDbType.Int;
			switch (type)
			{
			case DbType.DateTime:
				result = SqlDbType.DateTime;
				break;
			case DbType.Binary:
				result = SqlDbType.Image;
				break;
			case DbType.Date:
				result = SqlDbType.DateTime;
				break;
			case DbType.Decimal:
				result = SqlDbType.Decimal;
				break;
			case DbType.Double:
				result = SqlDbType.Decimal;
				break;
			case DbType.Single:
				result = SqlDbType.Float;
				break;
			case DbType.Int32:
				result = SqlDbType.Int;
				break;
			case DbType.Int64:
				result = SqlDbType.BigInt;
				break;
			case DbType.Boolean:
				result = SqlDbType.Bit;
				break;
			case DbType.Object:
				result = SqlDbType.Image;
				break;
			case DbType.String:
				result = ((!(typeString == "ntext")) ? SqlDbType.NVarChar : SqlDbType.NText);
				break;
			case DbType.AnsiString:
				result = SqlDbType.NVarChar;
				break;
			}
			return result;
		}

		public static OdbcType GetOdbcParameterType(DbType type)
		{
			OdbcType result = OdbcType.Int;
			switch (type)
			{
			case DbType.DateTime:
				result = OdbcType.DateTime;
				break;
			case DbType.Date:
				result = OdbcType.DateTime;
				break;
			case DbType.Decimal:
				result = OdbcType.Decimal;
				break;
			case DbType.Double:
				result = OdbcType.Double;
				break;
			case DbType.Single:
				result = OdbcType.Double;
				break;
			case DbType.Int32:
				result = OdbcType.Int;
				break;
			case DbType.Int64:
				result = OdbcType.BigInt;
				break;
			case DbType.Object:
				result = OdbcType.Image;
				break;
			case DbType.String:
				result = OdbcType.NVarChar;
				break;
			}
			return result;
		}

	
	}
}
