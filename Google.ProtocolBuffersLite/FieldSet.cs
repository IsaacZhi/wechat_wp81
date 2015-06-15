// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.FieldSet
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Collections;
using Google.ProtocolBuffers.Descriptors;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Google.ProtocolBuffers
{
  internal sealed class FieldSet
  {
    private static readonly FieldSet defaultInstance = new FieldSet((IDictionary<IFieldDescriptorLite, object>) new Dictionary<IFieldDescriptorLite, object>()).MakeImmutable();
    private IDictionary<IFieldDescriptorLite, object> fields;

    internal static FieldSet DefaultInstance
    {
      get
      {
        return FieldSet.defaultInstance;
      }
    }

    internal IDictionary<IFieldDescriptorLite, object> AllFields
    {
      get
      {
        return Dictionaries.AsReadOnly<IFieldDescriptorLite, object>(this.fields);
      }
    }

    internal object this[IFieldDescriptorLite field]
    {
      get
      {
        object obj;
        if (this.fields.TryGetValue(field, out obj))
          return obj;
        if (field.MappedType != MappedType.Message)
          return field.DefaultValue;
        if (field.IsRepeated)
          return (object) new List<object>();
        return (object) null;
      }
      set
      {
        if (field.IsRepeated)
        {
          List<object> list1 = value as List<object>;
          if (list1 == null)
            throw new ArgumentException("Wrong object type used with protocol message reflection.");
          List<object> list2 = new List<object>((IEnumerable<object>) list1);
          foreach (object obj in list2)
            FieldSet.VerifyType(field, obj);
          value = (object) list2;
        }
        else
          FieldSet.VerifyType(field, value);
        this.fields[field] = value;
      }
    }

    internal object this[IFieldDescriptorLite field, int index]
    {
      get
      {
        if (!field.IsRepeated)
          throw new ArgumentException("Indexer specifying field and index can only be called on repeated fields.");
        return ((IList<object>) this[field])[index];
      }
      set
      {
        if (!field.IsRepeated)
          throw new ArgumentException("Indexer specifying field and index can only be called on repeated fields.");
        FieldSet.VerifyType(field, value);
        object obj;
        if (!this.fields.TryGetValue(field, out obj))
          throw new ArgumentOutOfRangeException();
        ((IList<object>) obj)[index] = value;
      }
    }

    internal bool IsInitialized
    {
      get
      {
        foreach (KeyValuePair<IFieldDescriptorLite, object> keyValuePair in (IEnumerable<KeyValuePair<IFieldDescriptorLite, object>>) this.fields)
        {
          IFieldDescriptorLite key = keyValuePair.Key;
          if (key.MappedType == MappedType.Message)
          {
            if (key.IsRepeated)
            {
              foreach (IMessageLite messageLite in (IEnumerable) keyValuePair.Value)
              {
                if (!messageLite.IsInitialized)
                  return false;
              }
            }
            else if (!((IMessageLite) keyValuePair.Value).IsInitialized)
              return false;
          }
        }
        return true;
      }
    }

    public int SerializedSize
    {
      get
      {
        int num1 = 0;
        foreach (KeyValuePair<IFieldDescriptorLite, object> keyValuePair in (IEnumerable<KeyValuePair<IFieldDescriptorLite, object>>) this.fields)
        {
          IFieldDescriptorLite key = keyValuePair.Key;
          object obj1 = keyValuePair.Value;
          if (key.IsExtension && key.MessageSetWireFormat)
            num1 += CodedOutputStream.ComputeMessageSetExtensionSize(key.FieldNumber, (IMessageLite) obj1);
          else if (key.IsRepeated)
          {
            IEnumerable enumerable = (IEnumerable) obj1;
            if (key.IsPacked)
            {
              int num2 = 0;
              foreach (object obj2 in enumerable)
                num2 += CodedOutputStream.ComputeFieldSizeNoTag(key.FieldType, obj2);
              num1 += num2 + CodedOutputStream.ComputeTagSize(key.FieldNumber) + CodedOutputStream.ComputeRawVarint32Size((uint) num2);
            }
            else
            {
              foreach (object obj2 in enumerable)
                num1 += CodedOutputStream.ComputeFieldSize(key.FieldType, key.FieldNumber, obj2);
            }
          }
          else
            num1 += CodedOutputStream.ComputeFieldSize(key.FieldType, key.FieldNumber, obj1);
        }
        return num1;
      }
    }

    private FieldSet(IDictionary<IFieldDescriptorLite, object> fields)
    {
      this.fields = fields;
    }

    public static FieldSet CreateInstance()
    {
      return new FieldSet((IDictionary<IFieldDescriptorLite, object>) new SortedList<IFieldDescriptorLite, object>());
    }

    internal FieldSet MakeImmutable()
    {
      bool flag = false;
      foreach (object obj in (IEnumerable<object>) this.fields.Values)
      {
        IList<object> list = obj as IList<object>;
        if (list != null && !list.IsReadOnly)
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        SortedList<IFieldDescriptorLite, object> sortedList = new SortedList<IFieldDescriptorLite, object>();
        foreach (KeyValuePair<IFieldDescriptorLite, object> keyValuePair in (IEnumerable<KeyValuePair<IFieldDescriptorLite, object>>) this.fields)
        {
          IList<object> list = keyValuePair.Value as IList<object>;
          sortedList[keyValuePair.Key] = list == null ? keyValuePair.Value : (object) Lists.AsReadOnly<object>(list);
        }
        this.fields = (IDictionary<IFieldDescriptorLite, object>) sortedList;
      }
      this.fields = Dictionaries.AsReadOnly<IFieldDescriptorLite, object>(this.fields);
      return this;
    }

    public bool HasField(IFieldDescriptorLite field)
    {
      if (field.IsRepeated)
        throw new ArgumentException("HasField() can only be called on non-repeated fields.");
      return this.fields.ContainsKey(field);
    }

    internal void Clear()
    {
      this.fields.Clear();
    }

    internal void AddRepeatedField(IFieldDescriptorLite field, object value)
    {
      if (!field.IsRepeated)
        throw new ArgumentException("AddRepeatedField can only be called on repeated fields.");
      FieldSet.VerifyType(field, value);
      object obj;
      if (!this.fields.TryGetValue(field, out obj))
      {
        obj = (object) new List<object>();
        this.fields[field] = obj;
      }
      ((ICollection<object>) obj).Add(value);
    }

    internal IEnumerator<KeyValuePair<IFieldDescriptorLite, object>> GetEnumerator()
    {
      return this.fields.GetEnumerator();
    }

    internal bool IsInitializedWithRespectTo(IEnumerable typeFields)
    {
      foreach (IFieldDescriptorLite field in typeFields)
      {
        if (field.IsRequired && !this.HasField(field))
          return false;
      }
      return this.IsInitialized;
    }

    public void ClearField(IFieldDescriptorLite field)
    {
      this.fields.Remove(field);
    }

    public int GetRepeatedFieldCount(IFieldDescriptorLite field)
    {
      if (!field.IsRepeated)
        throw new ArgumentException("GetRepeatedFieldCount() can only be called on repeated fields.");
      return ((ICollection<object>) this[field]).Count;
    }

    public void MergeFrom(FieldSet other)
    {
      foreach (KeyValuePair<IFieldDescriptorLite, object> keyValuePair in (IEnumerable<KeyValuePair<IFieldDescriptorLite, object>>) other.fields)
        this.MergeField(keyValuePair.Key, keyValuePair.Value);
    }

    private void MergeField(IFieldDescriptorLite field, object mergeValue)
    {
      object obj1;
      this.fields.TryGetValue(field, out obj1);
      if (field.IsRepeated)
      {
        if (obj1 == null)
        {
          obj1 = (object) new List<object>();
          this.fields[field] = obj1;
        }
        IList<object> list = (IList<object>) obj1;
        foreach (object obj2 in (IEnumerable) mergeValue)
          list.Add(obj2);
      }
      else if (field.MappedType == MappedType.Message && obj1 != null)
      {
        IMessageLite messageLite = ((IMessageLite) obj1).WeakToBuilder().WeakMergeFrom((IMessageLite) mergeValue).WeakBuild();
        this[field] = (object) messageLite;
      }
      else
        this[field] = mergeValue;
    }

    public void WriteTo(ICodedOutputStream output)
    {
      foreach (KeyValuePair<IFieldDescriptorLite, object> keyValuePair in (IEnumerable<KeyValuePair<IFieldDescriptorLite, object>>) this.fields)
        this.WriteField(keyValuePair.Key, keyValuePair.Value, output);
    }

    public void WriteField(IFieldDescriptorLite field, object value, ICodedOutputStream output)
    {
      if (field.IsExtension && field.MessageSetWireFormat)
        output.WriteMessageSetExtension(field.FieldNumber, field.Name, (IMessageLite) value);
      else if (field.IsRepeated)
      {
        IEnumerable list = (IEnumerable) value;
        if (field.IsPacked)
          output.WritePackedArray(field.FieldType, field.FieldNumber, field.Name, list);
        else
          output.WriteArray(field.FieldType, field.FieldNumber, field.Name, list);
      }
      else
        output.WriteField(field.FieldType, field.FieldNumber, field.Name, value);
    }

    private static void VerifyType(IFieldDescriptorLite field, object value)
    {
      ThrowHelper.ThrowIfNull(value, "value");
      bool flag = false;
      switch (field.MappedType)
      {
        case MappedType.Int32:
          flag = value is int;
          break;
        case MappedType.Int64:
          flag = value is long;
          break;
        case MappedType.UInt32:
          flag = value is uint;
          break;
        case MappedType.UInt64:
          flag = value is ulong;
          break;
        case MappedType.Single:
          flag = value is float;
          break;
        case MappedType.Double:
          flag = value is double;
          break;
        case MappedType.Boolean:
          flag = value is bool;
          break;
        case MappedType.String:
          flag = value is string;
          break;
        case MappedType.ByteString:
          flag = value is ByteString;
          break;
        case MappedType.Message:
          flag = value is IMessageLite;
          break;
        case MappedType.Enum:
          IEnumLite enumLite = value as IEnumLite;
          flag = enumLite != null && field.EnumType.IsValidValue(enumLite);
          break;
      }
      if (!flag)
        throw new ArgumentException("Wrong object type used with protocol message reflection.");
    }
  }
}
