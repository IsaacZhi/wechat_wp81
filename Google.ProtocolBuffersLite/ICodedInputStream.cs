// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.ICodedInputStream
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Descriptors;
using System;
using System.Collections.Generic;

namespace Google.ProtocolBuffers
{
  public interface ICodedInputStream
  {
    bool IsAtEnd { get; }

    void ReadMessageStart();

    void ReadMessageEnd();

    [CLSCompliant(false)]
    bool ReadTag(out uint fieldTag, out string fieldName);

    bool ReadDouble(ref double value);

    bool ReadFloat(ref float value);

    [CLSCompliant(false)]
    bool ReadUInt64(ref ulong value);

    bool ReadInt64(ref long value);

    bool ReadInt32(ref int value);

    [CLSCompliant(false)]
    bool ReadFixed64(ref ulong value);

    [CLSCompliant(false)]
    bool ReadFixed32(ref uint value);

    bool ReadBool(ref bool value);

    bool ReadString(ref string value);

    void ReadGroup(int fieldNumber, IBuilderLite builder, ExtensionRegistry extensionRegistry);

    [Obsolete]
    void ReadUnknownGroup(int fieldNumber, IBuilderLite builder);

    void ReadMessage(IBuilderLite builder, ExtensionRegistry extensionRegistry);

    bool ReadBytes(ref ByteString value);

    [CLSCompliant(false)]
    bool ReadUInt32(ref uint value);

    bool ReadEnum(ref IEnumLite value, out object unknown, IEnumLiteMap mapping);

    [CLSCompliant(false)]
    bool ReadEnum<T>(ref T value, out object unknown) where T : struct, IComparable, IFormattable/*, IConvertible*/;

    bool ReadSFixed32(ref int value);

    bool ReadSFixed64(ref long value);

    bool ReadSInt32(ref int value);

    bool ReadSInt64(ref long value);

    [CLSCompliant(false)]
    void ReadPrimitiveArray(FieldType fieldType, uint fieldTag, string fieldName, ICollection<object> list);

    [CLSCompliant(false)]
    void ReadEnumArray(uint fieldTag, string fieldName, ICollection<IEnumLite> list, out ICollection<object> unknown, IEnumLiteMap mapping);

    [CLSCompliant(false)]
    void ReadEnumArray<T>(uint fieldTag, string fieldName, ICollection<T> list, out ICollection<object> unknown) where T : struct, IComparable, IFormattable/*, IConvertible*/;

    [CLSCompliant(false)]
    void ReadMessageArray<T>(uint fieldTag, string fieldName, ICollection<T> list, T messageType, ExtensionRegistry registry) where T : IMessageLite;

    [CLSCompliant(false)]
    void ReadGroupArray<T>(uint fieldTag, string fieldName, ICollection<T> list, T messageType, ExtensionRegistry registry) where T : IMessageLite;

    bool ReadPrimitiveField(FieldType fieldType, ref object value);

    [CLSCompliant(false)]
    bool SkipField();

    [CLSCompliant(false)]
    void ReadStringArray(uint fieldTag, string fieldName, ICollection<string> list);

    [CLSCompliant(false)]
    void ReadBytesArray(uint fieldTag, string fieldName, ICollection<ByteString> list);

    [CLSCompliant(false)]
    void ReadBoolArray(uint fieldTag, string fieldName, ICollection<bool> list);

    [CLSCompliant(false)]
    void ReadInt32Array(uint fieldTag, string fieldName, ICollection<int> list);

    [CLSCompliant(false)]
    void ReadSInt32Array(uint fieldTag, string fieldName, ICollection<int> list);

    [CLSCompliant(false)]
    void ReadUInt32Array(uint fieldTag, string fieldName, ICollection<uint> list);

    [CLSCompliant(false)]
    void ReadFixed32Array(uint fieldTag, string fieldName, ICollection<uint> list);

    [CLSCompliant(false)]
    void ReadSFixed32Array(uint fieldTag, string fieldName, ICollection<int> list);

    [CLSCompliant(false)]
    void ReadInt64Array(uint fieldTag, string fieldName, ICollection<long> list);

    [CLSCompliant(false)]
    void ReadSInt64Array(uint fieldTag, string fieldName, ICollection<long> list);

    [CLSCompliant(false)]
    void ReadUInt64Array(uint fieldTag, string fieldName, ICollection<ulong> list);

    [CLSCompliant(false)]
    void ReadFixed64Array(uint fieldTag, string fieldName, ICollection<ulong> list);

    [CLSCompliant(false)]
    void ReadSFixed64Array(uint fieldTag, string fieldName, ICollection<long> list);

    [CLSCompliant(false)]
    void ReadDoubleArray(uint fieldTag, string fieldName, ICollection<double> list);

    [CLSCompliant(false)]
    void ReadFloatArray(uint fieldTag, string fieldName, ICollection<float> list);
  }
}
