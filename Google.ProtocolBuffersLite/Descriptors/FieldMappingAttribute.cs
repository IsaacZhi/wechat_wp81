// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.Descriptors.FieldMappingAttribute
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Google.ProtocolBuffers.Descriptors
{
  [AttributeUsage(AttributeTargets.Field)]
  [CLSCompliant(false)]
  public sealed class FieldMappingAttribute : Attribute
  {
    private static readonly IDictionary<FieldType, FieldMappingAttribute> FieldTypeToMappedTypeMap = FieldMappingAttribute.MapFieldTypes();

    public MappedType MappedType { get; private set; }

    public WireFormat.WireType WireType { get; private set; }

    public FieldMappingAttribute(MappedType mappedType, WireFormat.WireType wireType)
    {
      this.MappedType = mappedType;
      this.WireType = wireType;
    }

    /*private static IDictionary<FieldType, FieldMappingAttribute> MapFieldTypes()
    {
      Dictionary<FieldType, FieldMappingAttribute> dictionary = new Dictionary<FieldType, FieldMappingAttribute>();
      foreach (FieldInfo fieldInfo in typeof(FieldType).GetRuntimeFields())
      {
        FieldType index = (FieldType) fieldInfo.GetValue((object) null);
        FieldMappingAttribute mappingAttribute = (FieldMappingAttribute) fieldInfo.GetCustomAttributes(typeof (FieldMappingAttribute), false)[0];
        dictionary[index] = mappingAttribute;
      }
      return Dictionaries.AsReadOnly<FieldType, FieldMappingAttribute>((IDictionary<FieldType, FieldMappingAttribute>) dictionary);
    }*/

    private static IDictionary<FieldType, FieldMappingAttribute> MapFieldTypes()
    {
        var map = new Dictionary<FieldType, FieldMappingAttribute>();
        foreach (FieldInfo field in typeof(FieldType).GetRuntimeFields())
        {
            FieldType fieldType = (FieldType)field.GetValue(null);
            /*FieldMappingAttribute mapping =
                (FieldMappingAttribute)field.GetCustomAttributes(typeof(FieldMappingAttribute), false)[0];*/
            FieldMappingAttribute mapping =
                (FieldMappingAttribute)field.GetCustomAttributes(typeof(FieldMappingAttribute), false).GetEnumerator().Current;
            map[fieldType] = mapping;
        }
        return Dictionaries.AsReadOnly(map);
    }

    internal static MappedType MappedTypeFromFieldType(FieldType type)
    {
      return FieldMappingAttribute.FieldTypeToMappedTypeMap[type].MappedType;
    }

    internal static WireFormat.WireType WireTypeFromFieldType(FieldType type, bool packed)
    {
      if (!packed)
        return FieldMappingAttribute.FieldTypeToMappedTypeMap[type].WireType;
      return WireFormat.WireType.LengthDelimited;
    }
  }
}
