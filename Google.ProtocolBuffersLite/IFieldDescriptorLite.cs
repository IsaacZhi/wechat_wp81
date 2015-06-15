// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.IFieldDescriptorLite
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Descriptors;
using System;

namespace Google.ProtocolBuffers
{
  public interface IFieldDescriptorLite : IComparable<IFieldDescriptorLite>
  {
    bool IsRepeated { get; }

    bool IsRequired { get; }

    bool IsPacked { get; }

    bool IsExtension { get; }

    bool MessageSetWireFormat { get; }

    int FieldNumber { get; }

    string Name { get; }

    string FullName { get; }

    IEnumLiteMap EnumType { get; }

    FieldType FieldType { get; }

    MappedType MappedType { get; }

    object DefaultValue { get; }
  }
}
