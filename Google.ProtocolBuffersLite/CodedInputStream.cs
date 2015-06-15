// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.CodedInputStream
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Descriptors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Google.ProtocolBuffers
{
  public sealed class CodedInputStream : ICodedInputStream
  {
    private int currentLimit = int.MaxValue;
    private int recursionLimit = 64;
    private int sizeLimit = 67108864;
    internal const int DefaultRecursionLimit = 64;
    internal const int DefaultSizeLimit = 67108864;
    public const int BufferSize = 4096;
    private readonly byte[] buffer;
    private int bufferSize;
    private int bufferSizeAfterLimit;
    private int bufferPos;
    private readonly Stream input;
    private uint lastTag;
    private uint nextTag;
    private bool hasNextTag;
    private int totalBytesRetired;
    private int recursionDepth;

    public bool ReachedLimit
    {
      get
      {
        if (this.currentLimit == int.MaxValue)
          return false;
        return this.totalBytesRetired + this.bufferPos >= this.currentLimit;
      }
    }

    public bool IsAtEnd
    {
      get
      {
        if (this.bufferPos == this.bufferSize)
          return !this.RefillBuffer(false);
        return false;
      }
    }

    private CodedInputStream(byte[] buffer, int offset, int length)
    {
      this.buffer = buffer;
      this.bufferPos = offset;
      this.bufferSize = offset + length;
      this.input = (Stream) null;
    }

    private CodedInputStream(Stream input)
    {
      this.buffer = new byte[4096];
      this.bufferSize = 0;
      this.input = input;
    }

    public static CodedInputStream CreateInstance(Stream input)
    {
      return new CodedInputStream(input);
    }

    public static CodedInputStream CreateInstance(byte[] buf)
    {
      return new CodedInputStream(buf, 0, buf.Length);
    }

    public static CodedInputStream CreateInstance(byte[] buf, int offset, int length)
    {
      return new CodedInputStream(buf, offset, length);
    }

    void ICodedInputStream.ReadMessageStart()
    {
    }

    void ICodedInputStream.ReadMessageEnd()
    {
    }

    [CLSCompliant(false)]
    public void CheckLastTagWas(uint value)
    {
      if ((int) this.lastTag != (int) value)
        throw InvalidProtocolBufferException.InvalidEndTag();
    }

    [CLSCompliant(false)]
    public bool PeekNextTag(out uint fieldTag, out string fieldName)
    {
      if (this.hasNextTag)
      {
        fieldName = (string) null;
        fieldTag = this.nextTag;
        return true;
      }
      uint num = this.lastTag;
      this.hasNextTag = this.ReadTag(out this.nextTag, out fieldName);
      this.lastTag = num;
      fieldTag = this.nextTag;
      return this.hasNextTag;
    }

    [CLSCompliant(false)]
    public bool ReadTag(out uint fieldTag, out string fieldName)
    {
      fieldName = (string) null;
      if (this.hasNextTag)
      {
        fieldTag = this.nextTag;
        this.lastTag = fieldTag;
        this.hasNextTag = false;
        return true;
      }
      if (this.IsAtEnd)
      {
        fieldTag = 0U;
        this.lastTag = fieldTag;
        return false;
      }
      fieldTag = this.ReadRawVarint32();
      this.lastTag = fieldTag;
      if ((int) this.lastTag == 0)
        throw InvalidProtocolBufferException.InvalidTag();
      return true;
    }

    public bool ReadDouble(ref double value)
    {
      if (BitConverter.IsLittleEndian && 8 <= this.bufferSize - this.bufferPos)
      {
        value = BitConverter.ToDouble(this.buffer, this.bufferPos);
        this.bufferPos += 8;
      }
      else
      {
        byte[] bytes = this.ReadRawBytes(8);
        if (!BitConverter.IsLittleEndian)
          ByteArray.Reverse(bytes);
        value = BitConverter.ToDouble(bytes, 0);
      }
      return true;
    }

    public bool ReadFloat(ref float value)
    {
      if (BitConverter.IsLittleEndian && 4 <= this.bufferSize - this.bufferPos)
      {
        value = BitConverter.ToSingle(this.buffer, this.bufferPos);
        this.bufferPos += 4;
      }
      else
      {
        byte[] bytes = this.ReadRawBytes(4);
        if (!BitConverter.IsLittleEndian)
          ByteArray.Reverse(bytes);
        value = BitConverter.ToSingle(bytes, 0);
      }
      return true;
    }

    [CLSCompliant(false)]
    public bool ReadUInt64(ref ulong value)
    {
      value = this.ReadRawVarint64();
      return true;
    }

    public bool ReadInt64(ref long value)
    {
      value = (long) this.ReadRawVarint64();
      return true;
    }

    public bool ReadInt32(ref int value)
    {
      value = (int) this.ReadRawVarint32();
      return true;
    }

    [CLSCompliant(false)]
    public bool ReadFixed64(ref ulong value)
    {
      value = this.ReadRawLittleEndian64();
      return true;
    }

    [CLSCompliant(false)]
    public bool ReadFixed32(ref uint value)
    {
      value = this.ReadRawLittleEndian32();
      return true;
    }

    public bool ReadBool(ref bool value)
    {
      value = (int) this.ReadRawVarint32() != 0;
      return true;
    }

    public bool ReadString(ref string value)
    {
      int num = (int) this.ReadRawVarint32();
      if (num == 0)
      {
        value = "";
        return true;
      }
      if (num <= this.bufferSize - this.bufferPos)
      {
        string @string = Encoding.UTF8.GetString(this.buffer, this.bufferPos, num);
        this.bufferPos += num;
        value = @string;
        return true;
      }
      value = Encoding.UTF8.GetString(this.ReadRawBytes(num), 0, num);
      return true;
    }

    public void ReadGroup(int fieldNumber, IBuilderLite builder, ExtensionRegistry extensionRegistry)
    {
      if (this.recursionDepth >= this.recursionLimit)
        throw InvalidProtocolBufferException.RecursionLimitExceeded();
      ++this.recursionDepth;
      builder.WeakMergeFrom((ICodedInputStream) this, extensionRegistry);
      this.CheckLastTagWas(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.EndGroup));
      --this.recursionDepth;
    }

    [Obsolete]
    public void ReadUnknownGroup(int fieldNumber, IBuilderLite builder)
    {
      if (this.recursionDepth >= this.recursionLimit)
        throw InvalidProtocolBufferException.RecursionLimitExceeded();
      ++this.recursionDepth;
      builder.WeakMergeFrom((ICodedInputStream) this);
      this.CheckLastTagWas(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.EndGroup));
      --this.recursionDepth;
    }

    public void ReadMessage(IBuilderLite builder, ExtensionRegistry extensionRegistry)
    {
      int byteLimit = (int) this.ReadRawVarint32();
      if (this.recursionDepth >= this.recursionLimit)
        throw InvalidProtocolBufferException.RecursionLimitExceeded();
      int oldLimit = this.PushLimit(byteLimit);
      ++this.recursionDepth;
      builder.WeakMergeFrom((ICodedInputStream) this, extensionRegistry);
      this.CheckLastTagWas(0U);
      --this.recursionDepth;
      this.PopLimit(oldLimit);
    }

    public bool ReadBytes(ref ByteString value)
    {
      int num = (int) this.ReadRawVarint32();
      if (num < this.bufferSize - this.bufferPos && num > 0)
      {
        ByteString byteString = ByteString.CopyFrom(this.buffer, this.bufferPos, num);
        this.bufferPos += num;
        value = byteString;
        return true;
      }
      value = ByteString.AttachBytes(this.ReadRawBytes(num));
      return true;
    }

    [CLSCompliant(false)]
    public bool ReadUInt32(ref uint value)
    {
      value = this.ReadRawVarint32();
      return true;
    }

    public bool ReadEnum(ref IEnumLite value, out object unknown, IEnumLiteMap mapping)
    {
      int number = (int) this.ReadRawVarint32();
      value = mapping.FindValueByNumber(number);
      if (value != null)
      {
        unknown = (object) null;
        return true;
      }
      unknown = (object) number;
      return false;
    }

    [CLSCompliant(false)]
    public bool ReadEnum<T>(ref T value, out object unknown) where T : struct, IComparable, IFormattable/*, IConvertible*/
    {
      int num = (int) this.ReadRawVarint32();
      if (Enum.IsDefined(typeof (T), (object) num))
      {
        unknown = (object) null;
        value = (T) (ValueType) num;
        return true;
      }
      unknown = (object) num;
      return false;
    }

    public bool ReadSFixed32(ref int value)
    {
      value = (int) this.ReadRawLittleEndian32();
      return true;
    }

    public bool ReadSFixed64(ref long value)
    {
      value = (long) this.ReadRawLittleEndian64();
      return true;
    }

    public bool ReadSInt32(ref int value)
    {
      value = CodedInputStream.DecodeZigZag32(this.ReadRawVarint32());
      return true;
    }

    public bool ReadSInt64(ref long value)
    {
      value = CodedInputStream.DecodeZigZag64(this.ReadRawVarint64());
      return true;
    }

    private bool BeginArray(uint fieldTag, out bool isPacked, out int oldLimit)
    {
      isPacked = WireFormat.GetTagWireType(fieldTag) == WireFormat.WireType.LengthDelimited;
      if (isPacked)
      {
        int byteLimit = (int) this.ReadRawVarint32() & int.MaxValue;
        if (byteLimit > 0)
        {
          oldLimit = this.PushLimit(byteLimit);
          return true;
        }
        oldLimit = -1;
        return false;
      }
      oldLimit = -1;
      return true;
    }

    private bool ContinueArray(uint currentTag)
    {
      uint fieldTag;
      string fieldName;
      if (!this.PeekNextTag(out fieldTag, out fieldName) || (int) fieldTag != (int) currentTag)
        return false;
      this.hasNextTag = false;
      return true;
    }

    private bool ContinueArray(uint currentTag, bool packed, int oldLimit)
    {
      if (packed)
      {
        if (!this.ReachedLimit)
          return true;
        this.PopLimit(oldLimit);
        return false;
      }
      uint fieldTag;
      string fieldName;
      if (!this.PeekNextTag(out fieldTag, out fieldName) || (int) fieldTag != (int) currentTag)
        return false;
      this.hasNextTag = false;
      return true;
    }

    [CLSCompliant(false)]
    public void ReadPrimitiveArray(FieldType fieldType, uint fieldTag, string fieldName, ICollection<object> list)
    {
      WireFormat.WireType wireType = WireFormat.GetWireType(fieldType);
      WireFormat.WireType tagWireType = WireFormat.GetTagWireType(fieldTag);
      if (wireType != tagWireType && tagWireType == WireFormat.WireType.LengthDelimited)
      {
        int oldLimit = this.PushLimit((int) this.ReadRawVarint32() & int.MaxValue);
        while (!this.ReachedLimit)
        {
          object obj = (object) null;
          if (this.ReadPrimitiveField(fieldType, ref obj))
            list.Add(obj);
        }
        this.PopLimit(oldLimit);
      }
      else
      {
        object obj = (object) null;
        do
        {
          if (this.ReadPrimitiveField(fieldType, ref obj))
            list.Add(obj);
        }
        while (this.ContinueArray(fieldTag));
      }
    }

    [CLSCompliant(false)]
    public void ReadStringArray(uint fieldTag, string fieldName, ICollection<string> list)
    {
      string str = (string) null;
      do
      {
        this.ReadString(ref str);
        list.Add(str);
      }
      while (this.ContinueArray(fieldTag));
    }

    [CLSCompliant(false)]
    public void ReadBytesArray(uint fieldTag, string fieldName, ICollection<ByteString> list)
    {
      ByteString byteString = (ByteString) null;
      do
      {
        this.ReadBytes(ref byteString);
        list.Add(byteString);
      }
      while (this.ContinueArray(fieldTag));
    }

    [CLSCompliant(false)]
    public void ReadBoolArray(uint fieldTag, string fieldName, ICollection<bool> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      bool flag = false;
      do
      {
        this.ReadBool(ref flag);
        list.Add(flag);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadInt32Array(uint fieldTag, string fieldName, ICollection<int> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      int num = 0;
      do
      {
        this.ReadInt32(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadSInt32Array(uint fieldTag, string fieldName, ICollection<int> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      int num = 0;
      do
      {
        this.ReadSInt32(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadUInt32Array(uint fieldTag, string fieldName, ICollection<uint> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      uint num = 0U;
      do
      {
        this.ReadUInt32(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadFixed32Array(uint fieldTag, string fieldName, ICollection<uint> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      uint num = 0U;
      do
      {
        this.ReadFixed32(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadSFixed32Array(uint fieldTag, string fieldName, ICollection<int> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      int num = 0;
      do
      {
        this.ReadSFixed32(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadInt64Array(uint fieldTag, string fieldName, ICollection<long> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      long num = 0L;
      do
      {
        this.ReadInt64(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadSInt64Array(uint fieldTag, string fieldName, ICollection<long> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      long num = 0L;
      do
      {
        this.ReadSInt64(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadUInt64Array(uint fieldTag, string fieldName, ICollection<ulong> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      ulong num = 0UL;
      do
      {
        this.ReadUInt64(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadFixed64Array(uint fieldTag, string fieldName, ICollection<ulong> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      ulong num = 0UL;
      do
      {
        this.ReadFixed64(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadSFixed64Array(uint fieldTag, string fieldName, ICollection<long> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      long num = 0L;
      do
      {
        this.ReadSFixed64(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadDoubleArray(uint fieldTag, string fieldName, ICollection<double> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      double num = 0.0;
      do
      {
        this.ReadDouble(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadFloatArray(uint fieldTag, string fieldName, ICollection<float> list)
    {
      bool isPacked;
      int oldLimit;
      if (!this.BeginArray(fieldTag, out isPacked, out oldLimit))
        return;
      float num = 0.0f;
      do
      {
        this.ReadFloat(ref num);
        list.Add(num);
      }
      while (this.ContinueArray(fieldTag, isPacked, oldLimit));
    }

    [CLSCompliant(false)]
    public void ReadEnumArray(uint fieldTag, string fieldName, ICollection<IEnumLite> list, out ICollection<object> unknown, IEnumLiteMap mapping)
    {
      unknown = (ICollection<object>) null;
      IEnumLite enumLite = (IEnumLite) null;
      if (WireFormat.GetTagWireType(fieldTag) == WireFormat.WireType.LengthDelimited)
      {
        int oldLimit = this.PushLimit((int) this.ReadRawVarint32() & int.MaxValue);
        while (!this.ReachedLimit)
        {
          object unknown1;
          if (this.ReadEnum(ref enumLite, out unknown1, mapping))
          {
            list.Add(enumLite);
          }
          else
          {
            if (unknown == null)
              unknown = (ICollection<object>) new List<object>();
            unknown.Add(unknown1);
          }
        }
        this.PopLimit(oldLimit);
      }
      else
      {
        do
        {
          object unknown1;
          if (this.ReadEnum(ref enumLite, out unknown1, mapping))
          {
            list.Add(enumLite);
          }
          else
          {
            if (unknown == null)
              unknown = (ICollection<object>) new List<object>();
            unknown.Add(unknown1);
          }
        }
        while (this.ContinueArray(fieldTag));
      }
    }

    [CLSCompliant(false)]
    public void ReadEnumArray<T>(uint fieldTag, string fieldName, ICollection<T> list, out ICollection<object> unknown) where T : struct, IComparable, IFormattable/*, IConvertible*/
    {
      unknown = (ICollection<object>) null;
      T obj = default (T);
      if (WireFormat.GetTagWireType(fieldTag) == WireFormat.WireType.LengthDelimited)
      {
        int oldLimit = this.PushLimit((int) this.ReadRawVarint32() & int.MaxValue);
        while (!this.ReachedLimit)
        {
          object unknown1;
          if (this.ReadEnum<T>(ref obj, out unknown1))
          {
            list.Add(obj);
          }
          else
          {
            if (unknown == null)
              unknown = (ICollection<object>) new List<object>();
            unknown.Add(unknown1);
          }
        }
        this.PopLimit(oldLimit);
      }
      else
      {
        do
        {
          object unknown1;
          if (this.ReadEnum<T>(ref obj, out unknown1))
          {
            list.Add(obj);
          }
          else
          {
            if (unknown == null)
              unknown = (ICollection<object>) new List<object>();
            unknown.Add(unknown1);
          }
        }
        while (this.ContinueArray(fieldTag));
      }
    }

    [CLSCompliant(false)]
    public void ReadMessageArray<T>(uint fieldTag, string fieldName, ICollection<T> list, T messageType, ExtensionRegistry registry) where T : IMessageLite
    {
      do
      {
        IBuilderLite builderForType = messageType.WeakCreateBuilderForType();
        this.ReadMessage(builderForType, registry);
        list.Add((T) builderForType.WeakBuildPartial());
      }
      while (this.ContinueArray(fieldTag));
    }

    [CLSCompliant(false)]
    public void ReadGroupArray<T>(uint fieldTag, string fieldName, ICollection<T> list, T messageType, ExtensionRegistry registry) where T : IMessageLite
    {
      do
      {
        IBuilderLite builderForType = messageType.WeakCreateBuilderForType();
        this.ReadGroup(WireFormat.GetTagFieldNumber(fieldTag), builderForType, registry);
        list.Add((T) builderForType.WeakBuildPartial());
      }
      while (this.ContinueArray(fieldTag));
    }

    public bool ReadPrimitiveField(FieldType fieldType, ref object value)
    {
      switch (fieldType)
      {
        case FieldType.Double:
          double num1 = 0.0;
          if (!this.ReadDouble(ref num1))
            return false;
          value = (object) num1;
          return true;
        case FieldType.Float:
          float num2 = 0.0f;
          if (!this.ReadFloat(ref num2))
            return false;
          value = (object) num2;
          return true;
        case FieldType.Int64:
          long num3 = 0L;
          if (!this.ReadInt64(ref num3))
            return false;
          value = (object) num3;
          return true;
        case FieldType.UInt64:
          ulong num4 = 0UL;
          if (!this.ReadUInt64(ref num4))
            return false;
          value = (object) num4;
          return true;
        case FieldType.Int32:
          int num5 = 0;
          if (!this.ReadInt32(ref num5))
            return false;
          value = (object) num5;
          return true;
        case FieldType.Fixed64:
          ulong num6 = 0UL;
          if (!this.ReadFixed64(ref num6))
            return false;
          value = (object) num6;
          return true;
        case FieldType.Fixed32:
          uint num7 = 0U;
          if (!this.ReadFixed32(ref num7))
            return false;
          value = (object) num7;
          return true;
        case FieldType.Bool:
          /*bool flag = false;
          if (!this.ReadBool(ref flag))
            return false;
          value = (object) (bool) (flag ? 1 : 0);
          return true;*/
          {
              bool tmp = false;
              if (ReadBool(ref tmp))
              {
                  value = tmp;
                  return true;
              }
              return false;
          }
        case FieldType.String:
          string str = (string) null;
          if (!this.ReadString(ref str))
            return false;
          value = (object) str;
          return true;
        case FieldType.Group:
          throw new ArgumentException("ReadPrimitiveField() cannot handle nested groups.");
        case FieldType.Message:
          throw new ArgumentException("ReadPrimitiveField() cannot handle embedded messages.");
        case FieldType.Bytes:
          ByteString byteString = (ByteString) null;
          if (!this.ReadBytes(ref byteString))
            return false;
          value = (object) byteString;
          return true;
        case FieldType.UInt32:
          uint num8 = 0U;
          if (!this.ReadUInt32(ref num8))
            return false;
          value = (object) num8;
          return true;
        case FieldType.SFixed32:
          int num9 = 0;
          if (!this.ReadSFixed32(ref num9))
            return false;
          value = (object) num9;
          return true;
        case FieldType.SFixed64:
          long num10 = 0L;
          if (!this.ReadSFixed64(ref num10))
            return false;
          value = (object) num10;
          return true;
        case FieldType.SInt32:
          int num11 = 0;
          if (!this.ReadSInt32(ref num11))
            return false;
          value = (object) num11;
          return true;
        case FieldType.SInt64:
          long num12 = 0L;
          if (!this.ReadSInt64(ref num12))
            return false;
          value = (object) num12;
          return true;
        case FieldType.Enum:
          throw new ArgumentException("ReadPrimitiveField() cannot handle enums.");
        default:
          throw new ArgumentOutOfRangeException("Invalid field type " + (object) fieldType);
      }
    }

    private uint SlowReadRawVarint32()
    {
      int num1 = (int) this.ReadRawByte();
      if (num1 < 128)
        return (uint) num1;
      int num2 = num1 & (int) sbyte.MaxValue;
      int num3;
      int num4;
      if ((num3 = (int) this.ReadRawByte()) < 128)
      {
        num4 = num2 | num3 << 7;
      }
      else
      {
        int num5 = num2 | (num3 & (int) sbyte.MaxValue) << 7;
        int num6;
        if ((num6 = (int) this.ReadRawByte()) < 128)
        {
          num4 = num5 | num6 << 14;
        }
        else
        {
          int num7 = num5 | (num6 & (int) sbyte.MaxValue) << 14;
          int num8;
          if ((num8 = (int) this.ReadRawByte()) < 128)
          {
            num4 = num7 | num8 << 21;
          }
          else
          {
            int num9;
            num4 = num7 | (num8 & (int) sbyte.MaxValue) << 21 | (num9 = (int) this.ReadRawByte()) << 28;
            if (num9 >= 128)
            {
              for (int index = 0; index < 5; ++index)
              {
                if ((int) this.ReadRawByte() < 128)
                  return (uint) num4;
              }
              throw InvalidProtocolBufferException.MalformedVarint();
            }
          }
        }
      }
      return (uint) num4;
    }

    [CLSCompliant(false)]
    public uint ReadRawVarint32()
    {
      if (this.bufferPos + 5 > this.bufferSize)
        return this.SlowReadRawVarint32();
      int num1 = (int) this.buffer[this.bufferPos++];
      if (num1 < 128)
        return (uint) num1;
      int num2 = num1 & (int) sbyte.MaxValue;
      byte[] numArray1 = this.buffer;
      int index1 = this.bufferPos++;
      int num3;
      int num4;
      if ((num3 = (int) numArray1[index1]) < 128)
      {
        num4 = num2 | num3 << 7;
      }
      else
      {
        int num5 = num2 | (num3 & (int) sbyte.MaxValue) << 7;
        byte[] numArray2 = this.buffer;
        int index2 = this.bufferPos++;
        int num6;
        if ((num6 = (int) numArray2[index2]) < 128)
        {
          num4 = num5 | num6 << 14;
        }
        else
        {
          int num7 = num5 | (num6 & (int) sbyte.MaxValue) << 14;
          byte[] numArray3 = this.buffer;
          int index3 = this.bufferPos++;
          int num8;
          if ((num8 = (int) numArray3[index3]) < 128)
          {
            num4 = num7 | num8 << 21;
          }
          else
          {
            int num9 = num7 | (num8 & (int) sbyte.MaxValue) << 21;
            byte[] numArray4 = this.buffer;
            int index4 = this.bufferPos++;
            int num10;
            int num11 = (num10 = (int) numArray4[index4]) << 28;
            num4 = num9 | num11;
            if (num10 >= 128)
            {
              for (int index5 = 0; index5 < 5; ++index5)
              {
                if ((int) this.ReadRawByte() < 128)
                  return (uint) num4;
              }
              throw InvalidProtocolBufferException.MalformedVarint();
            }
          }
        }
      }
      return (uint) num4;
    }

    [CLSCompliant(false)]
    public static uint ReadRawVarint32(Stream input)
    {
      int num1 = 0;
      int num2 = 0;
      while (num2 < 32)
      {
        int num3 = input.ReadByte();
        if (num3 == -1)
          throw InvalidProtocolBufferException.TruncatedMessage();
        num1 |= (num3 & (int) sbyte.MaxValue) << num2;
        if ((num3 & 128) == 0)
          return (uint) num1;
        num2 += 7;
      }
      while (num2 < 64)
      {
        int num3 = input.ReadByte();
        if (num3 == -1)
          throw InvalidProtocolBufferException.TruncatedMessage();
        if ((num3 & 128) == 0)
          return (uint) num1;
        num2 += 7;
      }
      throw InvalidProtocolBufferException.MalformedVarint();
    }

    [CLSCompliant(false)]
    public ulong ReadRawVarint64()
    {
      int num1 = 0;
      ulong num2 = 0UL;
      while (num1 < 64)
      {
        byte num3 = this.ReadRawByte();
        num2 |= (ulong) ((int) num3 & (int) sbyte.MaxValue) << num1;
        if (((int) num3 & 128) == 0)
          return num2;
        num1 += 7;
      }
      throw InvalidProtocolBufferException.MalformedVarint();
    }

    [CLSCompliant(false)]
    public uint ReadRawLittleEndian32()
    {
      return (uint) ((int) this.ReadRawByte() | (int) this.ReadRawByte() << 8 | (int) this.ReadRawByte() << 16 | (int) this.ReadRawByte() << 24);
    }

    [CLSCompliant(false)]
    public ulong ReadRawLittleEndian64()
    {
      return (ulong) ((long) this.ReadRawByte() | (long) this.ReadRawByte() << 8 | (long) this.ReadRawByte() << 16 | (long) this.ReadRawByte() << 24 | (long) this.ReadRawByte() << 32 | (long) this.ReadRawByte() << 40 | (long) this.ReadRawByte() << 48 | (long) this.ReadRawByte() << 56);
    }

    [CLSCompliant(false)]
    public static int DecodeZigZag32(uint n)
    {
      return (int) (n >> 1) ^ -((int) n & 1);
    }

    [CLSCompliant(false)]
    public static long DecodeZigZag64(ulong n)
    {
      return (long) (n >> 1) ^ -((long) n & 1L);
    }

    public int SetRecursionLimit(int limit)
    {
      if (limit < 0)
        throw new ArgumentOutOfRangeException("Recursion limit cannot be negative: " + (object) limit);
      int num = this.recursionLimit;
      this.recursionLimit = limit;
      return num;
    }

    public int SetSizeLimit(int limit)
    {
      if (limit < 0)
        throw new ArgumentOutOfRangeException("Size limit cannot be negative: " + (object) limit);
      int num = this.sizeLimit;
      this.sizeLimit = limit;
      return num;
    }

    public void ResetSizeCounter()
    {
      this.totalBytesRetired = 0;
    }

    public int PushLimit(int byteLimit)
    {
      if (byteLimit < 0)
        throw InvalidProtocolBufferException.NegativeSize();
      byteLimit += this.totalBytesRetired + this.bufferPos;
      int num = this.currentLimit;
      if (byteLimit > num)
        throw InvalidProtocolBufferException.TruncatedMessage();
      this.currentLimit = byteLimit;
      this.RecomputeBufferSizeAfterLimit();
      return num;
    }

    private void RecomputeBufferSizeAfterLimit()
    {
      this.bufferSize += this.bufferSizeAfterLimit;
      int num = this.totalBytesRetired + this.bufferSize;
      if (num > this.currentLimit)
      {
        this.bufferSizeAfterLimit = num - this.currentLimit;
        this.bufferSize -= this.bufferSizeAfterLimit;
      }
      else
        this.bufferSizeAfterLimit = 0;
    }

    public void PopLimit(int oldLimit)
    {
      this.currentLimit = oldLimit;
      this.RecomputeBufferSizeAfterLimit();
    }

    private bool RefillBuffer(bool mustSucceed)
    {
      if (this.bufferPos < this.bufferSize)
        throw new InvalidOperationException("RefillBuffer() called when buffer wasn't empty.");
      if (this.totalBytesRetired + this.bufferSize == this.currentLimit)
      {
        if (mustSucceed)
          throw InvalidProtocolBufferException.TruncatedMessage();
        return false;
      }
      this.totalBytesRetired += this.bufferSize;
      this.bufferPos = 0;
      this.bufferSize = this.input == null ? 0 : this.input.Read(this.buffer, 0, this.buffer.Length);
      if (this.bufferSize < 0)
        throw new InvalidOperationException("Stream.Read returned a negative count");
      if (this.bufferSize == 0)
      {
        if (mustSucceed)
          throw InvalidProtocolBufferException.TruncatedMessage();
        return false;
      }
      this.RecomputeBufferSizeAfterLimit();
      int num = this.totalBytesRetired + this.bufferSize + this.bufferSizeAfterLimit;
      if (num > this.sizeLimit || num < 0)
        throw InvalidProtocolBufferException.SizeLimitExceeded();
      return true;
    }

    public byte ReadRawByte()
    {
      if (this.bufferPos == this.bufferSize)
        this.RefillBuffer(true);
      return this.buffer[this.bufferPos++];
    }

    public byte[] ReadRawBytes(int size)
    {
      if (size < 0)
        throw InvalidProtocolBufferException.NegativeSize();
      if (this.totalBytesRetired + this.bufferPos + size > this.currentLimit)
      {
        this.SkipRawBytes(this.currentLimit - this.totalBytesRetired - this.bufferPos);
        throw InvalidProtocolBufferException.TruncatedMessage();
      }
      if (size <= this.bufferSize - this.bufferPos)
      {
        byte[] dst = new byte[size];
        ByteArray.Copy(this.buffer, this.bufferPos, dst, 0, size);
        this.bufferPos += size;
        return dst;
      }
      if (size < 4096)
      {
        byte[] dst = new byte[size];
        int num = this.bufferSize - this.bufferPos;
        ByteArray.Copy(this.buffer, this.bufferPos, dst, 0, num);
        this.bufferPos = this.bufferSize;
        this.RefillBuffer(true);
        while (size - num > this.bufferSize)
        {
          Buffer.BlockCopy((Array) this.buffer, 0, (Array) dst, num, this.bufferSize);
          num += this.bufferSize;
          this.bufferPos = this.bufferSize;
          this.RefillBuffer(true);
        }
        ByteArray.Copy(this.buffer, 0, dst, num, size - num);
        this.bufferPos = size - num;
        return dst;
      }
      int srcOffset = this.bufferPos;
      int num1 = this.bufferSize;
      this.totalBytesRetired += this.bufferSize;
      this.bufferPos = 0;
      this.bufferSize = 0;
      int val1 = size - (num1 - srcOffset);
      List<byte[]> list = new List<byte[]>();
      while (val1 > 0)
      {
        byte[] buffer = new byte[Math.Min(val1, 4096)];
        int offset = 0;
        while (offset < buffer.Length)
        {
          int num2 = this.input == null ? -1 : this.input.Read(buffer, offset, buffer.Length - offset);
          if (num2 <= 0)
            throw InvalidProtocolBufferException.TruncatedMessage();
          this.totalBytesRetired += num2;
          offset += num2;
        }
        val1 -= buffer.Length;
        list.Add(buffer);
      }
      byte[] dst1 = new byte[size];
      int num3 = num1 - srcOffset;
      ByteArray.Copy(this.buffer, srcOffset, dst1, 0, num3);
      foreach (byte[] numArray in list)
      {
        Buffer.BlockCopy((Array) numArray, 0, (Array) dst1, num3, numArray.Length);
        num3 += numArray.Length;
      }
      return dst1;
    }

    [CLSCompliant(false)]
    public bool SkipField()
    {
      uint tag = this.lastTag;
      switch (WireFormat.GetTagWireType(tag))
      {
        case WireFormat.WireType.Varint:
          long num1 = (long) this.ReadRawVarint64();
          return true;
        case WireFormat.WireType.Fixed64:
          long num2 = (long) this.ReadRawLittleEndian64();
          return true;
        case WireFormat.WireType.LengthDelimited:
          this.SkipRawBytes((int) this.ReadRawVarint32());
          return true;
        case WireFormat.WireType.StartGroup:
          this.SkipMessage();
          this.CheckLastTagWas(WireFormat.MakeTag(WireFormat.GetTagFieldNumber(tag), WireFormat.WireType.EndGroup));
          return true;
        case WireFormat.WireType.EndGroup:
          return false;
        case WireFormat.WireType.Fixed32:
          int num3 = (int) this.ReadRawLittleEndian32();
          return true;
        default:
          throw InvalidProtocolBufferException.InvalidWireType();
      }
    }

    public void SkipMessage()
    {
        uint tag;
        string name;
        while (ReadTag(out tag, out name))
        {
            if (!SkipField())
            {
                return;
            }
        }
    }

    public void SkipRawBytes(int size)
    {
      if (size < 0)
        throw InvalidProtocolBufferException.NegativeSize();
      if (this.totalBytesRetired + this.bufferPos + size > this.currentLimit)
      {
        this.SkipRawBytes(this.currentLimit - this.totalBytesRetired - this.bufferPos);
        throw InvalidProtocolBufferException.TruncatedMessage();
      }
      if (size <= this.bufferSize - this.bufferPos)
      {
        this.bufferPos += size;
      }
      else
      {
        int num = this.bufferSize - this.bufferPos;
        this.totalBytesRetired += num;
        this.bufferPos = 0;
        this.bufferSize = 0;
        if (num >= size)
          return;
        if (this.input == null)
          throw InvalidProtocolBufferException.TruncatedMessage();
        this.SkipImpl(size - num);
        this.totalBytesRetired += size - num;
      }
    }

    private void SkipImpl(int amountToSkip)
    {
      if (this.input.CanSeek)
      {
        long position = this.input.Position;
        this.input.Position += (long) amountToSkip;
        if (this.input.Position != position + (long) amountToSkip)
          throw InvalidProtocolBufferException.TruncatedMessage();
      }
      else
      {
        byte[] buffer = new byte[1024];
        while (amountToSkip > 0)
        {
          int num = this.input.Read(buffer, 0, buffer.Length);
          if (num <= 0)
            throw InvalidProtocolBufferException.TruncatedMessage();
          amountToSkip -= num;
        }
      }
    }
  }
}
