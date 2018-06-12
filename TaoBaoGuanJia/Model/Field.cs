using System;
using System.Data;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class Field
	{
		private bool needConvertType = true;

		private string fieldName;

		private object fieldValue;

		private string fieldDesc;

		private bool isKey;

		private DbType fieldType = DbType.String;

		private string fieldTypeString;

		private bool autoIncrease;

		private bool hasDefault;

		private object defaultValue = false;

		private bool is_Nullable = true;

		public bool HasDefault
		{
			get
			{
				return hasDefault;
			}
			set
			{
				hasDefault = value;
			}
		}

		public object DefaultValue
		{
			get
			{
				return defaultValue;
			}
			set
			{
				defaultValue = value;
			}
		}

		public bool Is_Nullable
		{
			get
			{
				return is_Nullable;
			}
			set
			{
				is_Nullable = value;
			}
		}

		public string FieldName
		{
			get
			{
				return fieldName;
			}
			set
			{
				fieldName = value;
			}
		}

		public object FieldValue
		{
			get
			{
				return fieldValue;
			}
			set
			{
				object obj = null;
				try
				{
					obj = (fieldValue = ConvertValueType(value));
				}
				catch
				{
					throw new Exception("给字段" + FieldName + "赋值时，数据类型不匹配");
				}
			}
		}

		public bool IsKey
		{
			get
			{
				return isKey;
			}
			set
			{
				isKey = value;
			}
		}

		public DbType FieldType
		{
			get
			{
				return fieldType;
			}
			set
			{
				fieldType = value;
			}
		}

		public string FieldTypeString
		{
			get
			{
				return fieldTypeString;
			}
			set
			{
				fieldTypeString = value;
			}
		}

		public string FieldDesc
		{
			get
			{
				return fieldDesc;
			}
			set
			{
				fieldDesc = value;
			}
		}

		public bool AutoIncrease
		{
			get
			{
				return autoIncrease;
			}
			set
			{
				autoIncrease = value;
			}
		}

		public Field()
		{
			needConvertType = true;
		}

		public Field(bool needConvertType)
		{
			this.needConvertType = needConvertType;
		}

		private object ConvertValueType(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value == DBNull.Value)
			{
				return null;
			}
			if (!needConvertType)
			{
				return value;
			}
			switch (fieldType)
			{
			case DbType.Boolean:
				return DataConvert.ToBoolean(value);
			case DbType.Int16:
				return DataConvert.ToShort(value);
			case DbType.Int32:
				return DataConvert.ToInt(value);
			case DbType.Int64:
				return DataConvert.ToLong(value);
			case DbType.Single:
				return Convert.ChangeType(value, typeof(float));
			case DbType.Double:
				return Convert.ChangeType(value, typeof(double));
			case DbType.Currency:
				return Convert.ChangeType(value, typeof(decimal));
			case DbType.Decimal:
				return Convert.ChangeType(value, typeof(decimal));
			case DbType.DateTime:
				return Convert.ChangeType(value, typeof(DateTime));
			case DbType.Object:
				return Convert.ChangeType(value, typeof(object));
			case DbType.Binary:
				return Convert.ChangeType(value, typeof(byte[]));
			default:
				return Convert.ChangeType(value, typeof(string));
			}
		}

		public void ForcibleSetFieldValue(object value)
		{
			object obj = null;
			try
			{
				obj = (fieldValue = ConvertValueType(value));
			}
			catch
			{
			}
		}

		public Field Clone()
		{
			Field field = new Field();
			field.AutoIncrease = AutoIncrease;
			field.FieldDesc = FieldDesc;
			field.FieldName = FieldName;
			field.FieldType = FieldType;
			field.FieldValue = FieldValue;
			field.IsKey = IsKey;
			field.DefaultValue = DefaultValue;
			field.HasDefault = HasDefault;
			field.Is_Nullable = Is_Nullable;
			field.FieldTypeString = FieldTypeString;
			return field;
		}
	}
}
