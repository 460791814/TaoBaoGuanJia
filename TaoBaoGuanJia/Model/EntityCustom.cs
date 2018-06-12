using System;
using System.Collections;
using System.Collections.Generic;
using TaoBaoGuanJia.Util;

namespace TaoBaoGuanJia.Model
{
	public class EntityCustom : IEnumerable
	{
		private Hashtable fieldHt = new Hashtable();

		private string name;

		public string Name => name;

		public IList<Field> Fields
		{
			get
			{
				IList<Field> list = new List<Field>();
				IDictionaryEnumerator enumerator = fieldHt.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						list.Add((Field)((DictionaryEntry)enumerator.Current).Value);
					}
					return list;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					disposable?.Dispose();
				}
			}
		}

		public object this[string fieldName]
		{
			get
			{
				return GetValue(fieldName);
			}
			set
			{
				SetValue(fieldName, value);
			}
		}

		public EntityCustom()
		{
			name = "";
		}

		public EntityCustom(string name)
		{
			this.name = name;
		}

		public bool IfExistField(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return false;
			}
			Field field = GetField(fieldName);
			if (field == null)
			{
				return false;
			}
			return true;
		}

		public Field GetField(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return null;
			}
			IList<Field> fields = Fields;
			for (int i = 0; i < fields.Count; i++)
			{
				if (fields[i].FieldName.ToLower() == fieldName.ToLower())
				{
					return fields[i];
				}
			}
			return null;
		}

		public void ClearField()
		{
			fieldHt.Clear();
		}

		public void ClearField(string fieldName)
		{
			if (!string.IsNullOrEmpty(fieldName))
			{
				fieldName = fieldName.Trim().ToLower();
				if (fieldHt.ContainsKey(fieldName))
				{
					fieldHt.Remove(fieldName);
				}
			}
		}

		public object GetValue(string fieldName)
		{
			if (fieldName != null && fieldName.Trim().Length > 0)
			{
				fieldName = fieldName.ToLower().Trim();
				if (!fieldHt.ContainsKey(fieldName))
				{
					return null;
				}
				Field field = (Field)fieldHt[fieldName];
				return field?.FieldValue;
			}
			throw new Exception("未指定字段名");
		}

		public void SetValue(string fieldName, object fieldValue)
		{
			if (fieldName != null && fieldName.Trim().Length > 0)
			{
				fieldName = fieldName.ToLower().Trim();
				Field field = null;
				if (!fieldHt.ContainsKey(fieldName))
				{
					AddField(fieldName, fieldValue);
				}
				field = (Field)fieldHt[fieldName];
				field.FieldValue = fieldValue;
				return;
			}
			throw new Exception("未指定字段名");
		}

		private void AddField(string fieldName, object fieldValue)
		{
			if (fieldName != null && fieldName.Trim().Length > 0)
			{
				fieldName = fieldName.ToLower().Trim();
				if (fieldHt.ContainsKey(fieldName))
				{
					throw new Exception("实体中已存在名称为" + fieldName + "的字段");
				}
				Field field = new Field(false);
				field.FieldType = DbTypeConverter.GetDbTypeByValue(fieldValue);
				field.FieldName = fieldName;
				field.FieldValue = fieldValue;
				fieldHt.Add(fieldName, field);
				return;
			}
			throw new Exception("实体字段名称不能为空");
		}

		private void AddField(Field field)
		{
			if (field == null)
			{
				throw new Exception("参数不能为空");
			}
			string text = field.FieldName.Trim().ToLower();
			if (fieldHt.ContainsKey(text))
			{
				throw new Exception("实体中已存在名称为" + text + "的字段");
			}
			fieldHt.Add(text, field);
		}

		public string GetString(string fieldName)
		{
			object obj = this[fieldName];
			if (obj != null)
			{
				string text = "";
				if (obj is DateTime)
				{
					return ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
				}
				return obj.ToString();
			}
			return "";
		}

		public string GetStringNull(string fieldName)
		{
			object obj = this[fieldName];
			if (obj != null)
			{
				string text = null;
				if (obj is DateTime)
				{
					return ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
				}
				return obj.ToString();
			}
			return null;
		}

		public int GetInt(string fieldName)
		{
			return DataConvert.ToInt(this[fieldName]);
		}

		public long GetLong(string fieldName)
		{
			return DataConvert.ToLong(this[fieldName]);
		}

		public bool GetBoolean(string fieldName)
		{
			return DataConvert.ToBoolean(this[fieldName]);
		}

		public DateTime GetDate(string fieldName)
		{
			return DataConvert.ToDateTime(this[fieldName]);
		}

		public string GetDate(string fieldName, string format)
		{
			return DataConvert.ToDateTime(this[fieldName]).ToString(format);
		}

		private string GetCusFieldName(string entityFieldName, string[] entityFieldNames, string[] cusFieldNames)
		{
			if (entityFieldName != null && entityFieldNames != null && cusFieldNames != null)
			{
				entityFieldName = entityFieldName.ToLower();
				string text = null;
				for (int i = 0; i < entityFieldNames.Length; i++)
				{
					if (entityFieldNames[i] != null && entityFieldName == entityFieldNames[i].ToLower().Trim())
					{
						text = cusFieldNames[i];
						return text?.Trim().ToLower();
					}
				}
				return null;
			}
			return null;
		}

		public EntityCustom Clone()
		{
			EntityCustom entityCustom = new EntityCustom(Name);
			for (int i = 0; i < Fields.Count; i++)
			{
				entityCustom[Fields[i].FieldName] = Fields[i].FieldValue;
			}
			return entityCustom;
		}

		public IEnumerator GetEnumerator()
		{
			for (int i = 0; i < Fields.Count; i++)
			{
				yield return (object)Fields[i];
			}
		}
	}
}
