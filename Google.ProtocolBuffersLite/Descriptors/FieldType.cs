﻿// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.Descriptors.FieldType
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers;

namespace Google.ProtocolBuffers.Descriptors
{
  public enum FieldType
  {
    [FieldMapping(MappedType.Double, WireFormat.WireType.Fixed64)] Double,
    [FieldMapping(MappedType.Single, WireFormat.WireType.Fixed32)] Float,
    [FieldMapping(MappedType.Int64, WireFormat.WireType.Varint)] Int64,
    [FieldMapping(MappedType.UInt64, WireFormat.WireType.Varint)] UInt64,
    [FieldMapping(MappedType.Int32, WireFormat.WireType.Varint)] Int32,
    [FieldMapping(MappedType.UInt64, WireFormat.WireType.Fixed64)] Fixed64,
    [FieldMapping(MappedType.UInt32, WireFormat.WireType.Fixed32)] Fixed32,
    [FieldMapping(MappedType.Boolean, WireFormat.WireType.Varint)] Bool,
    [FieldMapping(MappedType.String, WireFormat.WireType.LengthDelimited)] String,
    [FieldMapping(MappedType.Message, WireFormat.WireType.StartGroup)] Group,
    [FieldMapping(MappedType.Message, WireFormat.WireType.LengthDelimited)] Message,
    [FieldMapping(MappedType.ByteString, WireFormat.WireType.LengthDelimited)] Bytes,
    [FieldMapping(MappedType.UInt32, WireFormat.WireType.Varint)] UInt32,
    [FieldMapping(MappedType.Int32, WireFormat.WireType.Fixed32)] SFixed32,
    [FieldMapping(MappedType.Int64, WireFormat.WireType.Fixed64)] SFixed64,
    [FieldMapping(MappedType.Int32, WireFormat.WireType.Varint)] SInt32,
    [FieldMapping(MappedType.Int64, WireFormat.WireType.Varint)] SInt64,
    [FieldMapping(MappedType.Enum, WireFormat.WireType.Varint)] Enum,
  }
}
