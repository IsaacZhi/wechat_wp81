// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.CodedOutputStream
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Collections;
using Google.ProtocolBuffers.Descriptors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Google.ProtocolBuffers
{
  public sealed class CodedOutputStream : ICodedOutputStream
  {
    public static readonly int DefaultBufferSize = 4096;
    private const int LittleEndian64Size = 8;
    private const int LittleEndian32Size = 4;
    private readonly byte[] buffer;
    private readonly int limit;
    private int position;
    private readonly Stream output;

    public int SpaceLeft
    {
      get
      {
        if (this.output == null)
          return this.limit - this.position;
        throw new InvalidOperationException("SpaceLeft can only be called on CodedOutputStreams that are writing to a flat array.");
      }
    }

    private CodedOutputStream(byte[] buffer, int offset, int length)
    {
      this.output = (Stream) null;
      this.buffer = buffer;
      this.position = offset;
      this.limit = offset + length;
    }

    private CodedOutputStream(Stream output, byte[] buffer)
    {
      this.output = output;
      this.buffer = buffer;
      this.position = 0;
      this.limit = buffer.Length;
    }

    public static int ComputeDoubleSize(int fieldNumber, double value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 8;
    }

    public static int ComputeFloatSize(int fieldNumber, float value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 4;
    }

    [CLSCompliant(false)]
    public static int ComputeUInt64Size(int fieldNumber, ulong value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint64Size(value);
    }

    public static int ComputeInt64Size(int fieldNumber, long value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint64Size((ulong) value);
    }

    public static int ComputeInt32Size(int fieldNumber, int value)
    {
      if (value >= 0)
        return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint32Size((uint) value);
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 10;
    }

    [CLSCompliant(false)]
    public static int ComputeFixed64Size(int fieldNumber, ulong value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 8;
    }

    [CLSCompliant(false)]
    public static int ComputeFixed32Size(int fieldNumber, uint value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 4;
    }

    public static int ComputeBoolSize(int fieldNumber, bool value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 1;
    }

    public static int ComputeStringSize(int fieldNumber, string value)
    {
      int byteCount = Encoding.UTF8.GetByteCount(value);
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint32Size((uint) byteCount) + byteCount;
    }

    public static int ComputeGroupSize(int fieldNumber, IMessageLite value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) * 2 + value.SerializedSize;
    }

    [Obsolete]
    public static int ComputeUnknownGroupSize(int fieldNumber, IMessageLite value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) * 2 + value.SerializedSize;
    }

    public static int ComputeMessageSize(int fieldNumber, IMessageLite value)
    {
      int serializedSize = value.SerializedSize;
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint32Size((uint) serializedSize) + serializedSize;
    }

    public static int ComputeBytesSize(int fieldNumber, ByteString value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint32Size((uint) value.Length) + value.Length;
    }

    [CLSCompliant(false)]
    public static int ComputeUInt32Size(int fieldNumber, uint value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint32Size(value);
    }

    public static int ComputeEnumSize(int fieldNumber, int value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeEnumSizeNoTag(value);
    }

    public static int ComputeSFixed32Size(int fieldNumber, int value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 4;
    }

    public static int ComputeSFixed64Size(int fieldNumber, long value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + 8;
    }

    public static int ComputeSInt32Size(int fieldNumber, int value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint32Size(CodedOutputStream.EncodeZigZag32(value));
    }

    public static int ComputeSInt64Size(int fieldNumber, long value)
    {
      return CodedOutputStream.ComputeTagSize(fieldNumber) + CodedOutputStream.ComputeRawVarint64Size(CodedOutputStream.EncodeZigZag64(value));
    }

    public static int ComputeDoubleSizeNoTag(double value)
    {
      return 8;
    }

    public static int ComputeFloatSizeNoTag(float value)
    {
      return 4;
    }

    [CLSCompliant(false)]
    public static int ComputeUInt64SizeNoTag(ulong value)
    {
      return CodedOutputStream.ComputeRawVarint64Size(value);
    }

    public static int ComputeInt64SizeNoTag(long value)
    {
      return CodedOutputStream.ComputeRawVarint64Size((ulong) value);
    }

    public static int ComputeInt32SizeNoTag(int value)
    {
      if (value >= 0)
        return CodedOutputStream.ComputeRawVarint32Size((uint) value);
      return 10;
    }

    [CLSCompliant(false)]
    public static int ComputeFixed64SizeNoTag(ulong value)
    {
      return 8;
    }

    [CLSCompliant(false)]
    public static int ComputeFixed32SizeNoTag(uint value)
    {
      return 4;
    }

    public static int ComputeBoolSizeNoTag(bool value)
    {
      return 1;
    }

    public static int ComputeStringSizeNoTag(string value)
    {
      int byteCount = Encoding.UTF8.GetByteCount(value);
      return CodedOutputStream.ComputeRawVarint32Size((uint) byteCount) + byteCount;
    }

    public static int ComputeGroupSizeNoTag(IMessageLite value)
    {
      return value.SerializedSize;
    }

    [Obsolete]
    public static int ComputeUnknownGroupSizeNoTag(IMessageLite value)
    {
      return value.SerializedSize;
    }

    public static int ComputeMessageSizeNoTag(IMessageLite value)
    {
      int serializedSize = value.SerializedSize;
      return CodedOutputStream.ComputeRawVarint32Size((uint) serializedSize) + serializedSize;
    }

    public static int ComputeBytesSizeNoTag(ByteString value)
    {
      return CodedOutputStream.ComputeRawVarint32Size((uint) value.Length) + value.Length;
    }

    [CLSCompliant(false)]
    public static int ComputeUInt32SizeNoTag(uint value)
    {
      return CodedOutputStream.ComputeRawVarint32Size(value);
    }

    public static int ComputeEnumSizeNoTag(int value)
    {
      return CodedOutputStream.ComputeInt32SizeNoTag(value);
    }

    public static int ComputeSFixed32SizeNoTag(int value)
    {
      return 4;
    }

    public static int ComputeSFixed64SizeNoTag(long value)
    {
      return 8;
    }

    public static int ComputeSInt32SizeNoTag(int value)
    {
      return CodedOutputStream.ComputeRawVarint32Size(CodedOutputStream.EncodeZigZag32(value));
    }

    public static int ComputeSInt64SizeNoTag(long value)
    {
      return CodedOutputStream.ComputeRawVarint64Size(CodedOutputStream.EncodeZigZag64(value));
    }

    public static int ComputeMessageSetExtensionSize(int fieldNumber, IMessageLite value)
    {
      return CodedOutputStream.ComputeTagSize(1) * 2 + CodedOutputStream.ComputeUInt32Size(2, (uint) fieldNumber) + CodedOutputStream.ComputeMessageSize(3, value);
    }

    public static int ComputeRawMessageSetExtensionSize(int fieldNumber, ByteString value)
    {
      return CodedOutputStream.ComputeTagSize(1) * 2 + CodedOutputStream.ComputeUInt32Size(2, (uint) fieldNumber) + CodedOutputStream.ComputeBytesSize(3, value);
    }

    [CLSCompliant(false)]
    public static int ComputeRawVarint32Size(uint value)
    {
      if (((int) value & (int) sbyte.MinValue) == 0)
        return 1;
      if (((int) value & -16384) == 0)
        return 2;
      if (((int) value & -2097152) == 0)
        return 3;
      return ((int) value & -268435456) == 0 ? 4 : 5;
    }

    [CLSCompliant(false)]
    public static int ComputeRawVarint64Size(ulong value)
    {
        if ((value & (0xffffffffffffffffL << 7)) == 0)
        {
            return 1;
        }
        if ((value & (0xffffffffffffffffL << 14)) == 0)
        {
            return 2;
        }
        if ((value & (0xffffffffffffffffL << 21)) == 0)
        {
            return 3;
        }
        if ((value & (0xffffffffffffffffL << 28)) == 0)
        {
            return 4;
        }
        if ((value & (0xffffffffffffffffL << 35)) == 0)
        {
            return 5;
        }
        if ((value & (0xffffffffffffffffL << 42)) == 0)
        {
            return 6;
        }
        if ((value & (0xffffffffffffffffL << 49)) == 0)
        {
            return 7;
        }
        if ((value & (0xffffffffffffffffL << 56)) == 0)
        {
            return 8;
        }
        if ((value & (0xffffffffffffffffL << 63)) == 0)
        {
            return 9;
        }
        return 10;
    }

    public static int ComputeFieldSize(FieldType fieldType, int fieldNumber, object value)
    {
      switch (fieldType)
      {
        case FieldType.Double:
          return CodedOutputStream.ComputeDoubleSize(fieldNumber, (double) value);
        case FieldType.Float:
          return CodedOutputStream.ComputeFloatSize(fieldNumber, (float) value);
        case FieldType.Int64:
          return CodedOutputStream.ComputeInt64Size(fieldNumber, (long) value);
        case FieldType.UInt64:
          return CodedOutputStream.ComputeUInt64Size(fieldNumber, (ulong) value);
        case FieldType.Int32:
          return CodedOutputStream.ComputeInt32Size(fieldNumber, (int) value);
        case FieldType.Fixed64:
          return CodedOutputStream.ComputeFixed64Size(fieldNumber, (ulong) value);
        case FieldType.Fixed32:
          return CodedOutputStream.ComputeFixed32Size(fieldNumber, (uint) value);
        case FieldType.Bool:
          return CodedOutputStream.ComputeBoolSize(fieldNumber, (bool) value);
        case FieldType.String:
          return CodedOutputStream.ComputeStringSize(fieldNumber, (string) value);
        case FieldType.Group:
          return CodedOutputStream.ComputeGroupSize(fieldNumber, (IMessageLite) value);
        case FieldType.Message:
          return CodedOutputStream.ComputeMessageSize(fieldNumber, (IMessageLite) value);
        case FieldType.Bytes:
          return CodedOutputStream.ComputeBytesSize(fieldNumber, (ByteString) value);
        case FieldType.UInt32:
          return CodedOutputStream.ComputeUInt32Size(fieldNumber, (uint) value);
        case FieldType.SFixed32:
          return CodedOutputStream.ComputeSFixed32Size(fieldNumber, (int) value);
        case FieldType.SFixed64:
          return CodedOutputStream.ComputeSFixed64Size(fieldNumber, (long) value);
        case FieldType.SInt32:
          return CodedOutputStream.ComputeSInt32Size(fieldNumber, (int) value);
        case FieldType.SInt64:
          return CodedOutputStream.ComputeSInt64Size(fieldNumber, (long) value);
        case FieldType.Enum:
          /*if (value is Enum)
            return CodedOutputStream.ComputeEnumSize(fieldNumber, ((IConvertible) value).ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
          return CodedOutputStream.ComputeEnumSize(fieldNumber, ((IEnumLite) value).Number);*/
          if (value is Enum)
          {
              return ComputeEnumSize(fieldNumber, Convert.ToInt32(value));
          }
          else
          {
              return ComputeEnumSize(fieldNumber, ((IEnumLite)value).Number);
          }
        default:
          throw new ArgumentOutOfRangeException("Invalid field type " + (object) fieldType);
      }
    }

    public static int ComputeFieldSizeNoTag(FieldType fieldType, object value)
    {
      switch (fieldType)
      {
        case FieldType.Double:
          return CodedOutputStream.ComputeDoubleSizeNoTag((double) value);
        case FieldType.Float:
          return CodedOutputStream.ComputeFloatSizeNoTag((float) value);
        case FieldType.Int64:
          return CodedOutputStream.ComputeInt64SizeNoTag((long) value);
        case FieldType.UInt64:
          return CodedOutputStream.ComputeUInt64SizeNoTag((ulong) value);
        case FieldType.Int32:
          return CodedOutputStream.ComputeInt32SizeNoTag((int) value);
        case FieldType.Fixed64:
          return CodedOutputStream.ComputeFixed64SizeNoTag((ulong) value);
        case FieldType.Fixed32:
          return CodedOutputStream.ComputeFixed32SizeNoTag((uint) value);
        case FieldType.Bool:
          return CodedOutputStream.ComputeBoolSizeNoTag((bool) value);
        case FieldType.String:
          return CodedOutputStream.ComputeStringSizeNoTag((string) value);
        case FieldType.Group:
          return CodedOutputStream.ComputeGroupSizeNoTag((IMessageLite) value);
        case FieldType.Message:
          return CodedOutputStream.ComputeMessageSizeNoTag((IMessageLite) value);
        case FieldType.Bytes:
          return CodedOutputStream.ComputeBytesSizeNoTag((ByteString) value);
        case FieldType.UInt32:
          return CodedOutputStream.ComputeUInt32SizeNoTag((uint) value);
        case FieldType.SFixed32:
          return CodedOutputStream.ComputeSFixed32SizeNoTag((int) value);
        case FieldType.SFixed64:
          return CodedOutputStream.ComputeSFixed64SizeNoTag((long) value);
        case FieldType.SInt32:
          return CodedOutputStream.ComputeSInt32SizeNoTag((int) value);
        case FieldType.SInt64:
          return CodedOutputStream.ComputeSInt64SizeNoTag((long) value);
        case FieldType.Enum:
          if (value is Enum)
          {
              return ComputeEnumSizeNoTag(Convert.ToInt32(value));
              //return CodedOutputStream.ComputeEnumSizeNoTag(((IConvertible) value).ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
          }
          else
          {
              return CodedOutputStream.ComputeEnumSizeNoTag(((IEnumLite)value).Number);
          }
        default:
          throw new ArgumentOutOfRangeException("Invalid field type " + (object) fieldType);
      }
    }

    public static int ComputeTagSize(int fieldNumber)
    {
      return CodedOutputStream.ComputeRawVarint32Size(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.Varint));
    }

    public static CodedOutputStream CreateInstance(Stream output)
    {
      return CodedOutputStream.CreateInstance(output, CodedOutputStream.DefaultBufferSize);
    }

    public static CodedOutputStream CreateInstance(Stream output, int bufferSize)
    {
      return new CodedOutputStream(output, new byte[bufferSize]);
    }

    public static CodedOutputStream CreateInstance(byte[] flatArray)
    {
      return CodedOutputStream.CreateInstance(flatArray, 0, flatArray.Length);
    }

    public static CodedOutputStream CreateInstance(byte[] flatArray, int offset, int length)
    {
      return new CodedOutputStream(flatArray, offset, length);
    }

    void ICodedOutputStream.WriteMessageStart()
    {
    }

    void ICodedOutputStream.WriteMessageEnd()
    {
      this.Flush();
    }

    [Obsolete]
    public void WriteUnknownGroup(int fieldNumber, IMessageLite value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.StartGroup);
      value.WriteTo((ICodedOutputStream) this);
      this.WriteTag(fieldNumber, WireFormat.WireType.EndGroup);
    }

    public void WriteUnknownBytes(int fieldNumber, ByteString value)
    {
      this.WriteBytes(fieldNumber, (string) null, value);
    }

    [CLSCompliant(false)]
    public void WriteUnknownField(int fieldNumber, WireFormat.WireType wireType, ulong value)
    {
      if (wireType == WireFormat.WireType.Varint)
        this.WriteUInt64(fieldNumber, (string) null, value);
      else if (wireType == WireFormat.WireType.Fixed32)
      {
        this.WriteFixed32(fieldNumber, (string) null, (uint) value);
      }
      else
      {
        if (wireType != WireFormat.WireType.Fixed64)
          throw InvalidProtocolBufferException.InvalidWireType();
        this.WriteFixed64(fieldNumber, (string) null, value);
      }
    }

    public void WriteField(FieldType fieldType, int fieldNumber, string fieldName, object value)
    {
      switch (fieldType)
      {
        case FieldType.Double:
          this.WriteDouble(fieldNumber, fieldName, (double) value);
          break;
        case FieldType.Float:
          this.WriteFloat(fieldNumber, fieldName, (float) value);
          break;
        case FieldType.Int64:
          this.WriteInt64(fieldNumber, fieldName, (long) value);
          break;
        case FieldType.UInt64:
          this.WriteUInt64(fieldNumber, fieldName, (ulong) value);
          break;
        case FieldType.Int32:
          this.WriteInt32(fieldNumber, fieldName, (int) value);
          break;
        case FieldType.Fixed64:
          this.WriteFixed64(fieldNumber, fieldName, (ulong) value);
          break;
        case FieldType.Fixed32:
          this.WriteFixed32(fieldNumber, fieldName, (uint) value);
          break;
        case FieldType.Bool:
          this.WriteBool(fieldNumber, fieldName, (bool) value);
          break;
        case FieldType.String:
          this.WriteString(fieldNumber, fieldName, (string) value);
          break;
        case FieldType.Group:
          this.WriteGroup(fieldNumber, fieldName, (IMessageLite) value);
          break;
        case FieldType.Message:
          this.WriteMessage(fieldNumber, fieldName, (IMessageLite) value);
          break;
        case FieldType.Bytes:
          this.WriteBytes(fieldNumber, fieldName, (ByteString) value);
          break;
        case FieldType.UInt32:
          this.WriteUInt32(fieldNumber, fieldName, (uint) value);
          break;
        case FieldType.SFixed32:
          this.WriteSFixed32(fieldNumber, fieldName, (int) value);
          break;
        case FieldType.SFixed64:
          this.WriteSFixed64(fieldNumber, fieldName, (long) value);
          break;
        case FieldType.SInt32:
          this.WriteSInt32(fieldNumber, fieldName, (int) value);
          break;
        case FieldType.SInt64:
          this.WriteSInt64(fieldNumber, fieldName, (long) value);
          break;
        case FieldType.Enum:
          if (value is Enum)
          {
            this.WriteEnum(fieldNumber, fieldName, (int) value, (object) null);
            break;
          }
          this.WriteEnum(fieldNumber, fieldName, ((IEnumLite) value).Number, (object) null);
          break;
      }
    }

    public void WriteDouble(int fieldNumber, string fieldName, double value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Fixed64);
      this.WriteDoubleNoTag(value);
    }

    public void WriteFloat(int fieldNumber, string fieldName, float value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Fixed32);
      this.WriteFloatNoTag(value);
    }

    [CLSCompliant(false)]
    public void WriteUInt64(int fieldNumber, string fieldName, ulong value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      this.WriteRawVarint64(value);
    }

    public void WriteInt64(int fieldNumber, string fieldName, long value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      this.WriteRawVarint64((ulong) value);
    }

    public void WriteInt32(int fieldNumber, string fieldName, int value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      if (value >= 0)
        this.WriteRawVarint32((uint) value);
      else
        this.WriteRawVarint64((ulong) value);
    }

    [CLSCompliant(false)]
    public void WriteFixed64(int fieldNumber, string fieldName, ulong value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Fixed64);
      this.WriteRawLittleEndian64(value);
    }

    [CLSCompliant(false)]
    public void WriteFixed32(int fieldNumber, string fieldName, uint value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Fixed32);
      this.WriteRawLittleEndian32(value);
    }

    public void WriteBool(int fieldNumber, string fieldName, bool value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      this.WriteRawByte(value ? (byte) 1 : (byte) 0);
    }

    public void WriteString(int fieldNumber, string fieldName, string value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      int byteCount = Encoding.UTF8.GetByteCount(value);
      this.WriteRawVarint32((uint) byteCount);
      if (this.limit - this.position >= byteCount)
      {
        Encoding.UTF8.GetBytes(value, 0, value.Length, this.buffer, this.position);
        this.position += byteCount;
      }
      else
        this.WriteRawBytes(Encoding.UTF8.GetBytes(value));
    }

    public void WriteGroup(int fieldNumber, string fieldName, IMessageLite value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.StartGroup);
      value.WriteTo((ICodedOutputStream) this);
      this.WriteTag(fieldNumber, WireFormat.WireType.EndGroup);
    }

    public void WriteMessage(int fieldNumber, string fieldName, IMessageLite value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) value.SerializedSize);
      value.WriteTo((ICodedOutputStream) this);
    }

    public void WriteBytes(int fieldNumber, string fieldName, ByteString value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) value.Length);
      value.WriteRawBytesTo(this);
    }

    [CLSCompliant(false)]
    public void WriteUInt32(int fieldNumber, string fieldName, uint value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      this.WriteRawVarint32(value);
    }

    public void WriteEnum(int fieldNumber, string fieldName, int value, object rawValue)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      this.WriteInt32NoTag(value);
    }

    public void WriteSFixed32(int fieldNumber, string fieldName, int value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Fixed32);
      this.WriteRawLittleEndian32((uint) value);
    }

    public void WriteSFixed64(int fieldNumber, string fieldName, long value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Fixed64);
      this.WriteRawLittleEndian64((ulong) value);
    }

    public void WriteSInt32(int fieldNumber, string fieldName, int value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      this.WriteRawVarint32(CodedOutputStream.EncodeZigZag32(value));
    }

    public void WriteSInt64(int fieldNumber, string fieldName, long value)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.Varint);
      this.WriteRawVarint64(CodedOutputStream.EncodeZigZag64(value));
    }

    public void WriteMessageSetExtension(int fieldNumber, string fieldName, IMessageLite value)
    {
      this.WriteTag(1, WireFormat.WireType.StartGroup);
      this.WriteUInt32(2, "type_id", (uint) fieldNumber);
      this.WriteMessage(3, "message", value);
      this.WriteTag(1, WireFormat.WireType.EndGroup);
    }

    public void WriteMessageSetExtension(int fieldNumber, string fieldName, ByteString value)
    {
      this.WriteTag(1, WireFormat.WireType.StartGroup);
      this.WriteUInt32(2, "type_id", (uint) fieldNumber);
      this.WriteBytes(3, "message", value);
      this.WriteTag(1, WireFormat.WireType.EndGroup);
    }

    public void WriteFieldNoTag(FieldType fieldType, object value)
    {
      switch (fieldType)
      {
        case FieldType.Double:
          this.WriteDoubleNoTag((double) value);
          break;
        case FieldType.Float:
          this.WriteFloatNoTag((float) value);
          break;
        case FieldType.Int64:
          this.WriteInt64NoTag((long) value);
          break;
        case FieldType.UInt64:
          this.WriteUInt64NoTag((ulong) value);
          break;
        case FieldType.Int32:
          this.WriteInt32NoTag((int) value);
          break;
        case FieldType.Fixed64:
          this.WriteFixed64NoTag((ulong) value);
          break;
        case FieldType.Fixed32:
          this.WriteFixed32NoTag((uint) value);
          break;
        case FieldType.Bool:
          this.WriteBoolNoTag((bool) value);
          break;
        case FieldType.String:
          this.WriteStringNoTag((string) value);
          break;
        case FieldType.Group:
          this.WriteGroupNoTag((IMessageLite) value);
          break;
        case FieldType.Message:
          this.WriteMessageNoTag((IMessageLite) value);
          break;
        case FieldType.Bytes:
          this.WriteBytesNoTag((ByteString) value);
          break;
        case FieldType.UInt32:
          this.WriteUInt32NoTag((uint) value);
          break;
        case FieldType.SFixed32:
          this.WriteSFixed32NoTag((int) value);
          break;
        case FieldType.SFixed64:
          this.WriteSFixed64NoTag((long) value);
          break;
        case FieldType.SInt32:
          this.WriteSInt32NoTag((int) value);
          break;
        case FieldType.SInt64:
          this.WriteSInt64NoTag((long) value);
          break;
        case FieldType.Enum:
          if (value is Enum)
          {
            this.WriteEnumNoTag((int) value);
            break;
          }
          this.WriteEnumNoTag(((IEnumLite) value).Number);
          break;
      }
    }

    public void WriteDoubleNoTag(double value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      if (!BitConverter.IsLittleEndian)
        ByteArray.Reverse(bytes);
      if (this.limit - this.position >= 8)
      {
        this.buffer[this.position++] = bytes[0];
        this.buffer[this.position++] = bytes[1];
        this.buffer[this.position++] = bytes[2];
        this.buffer[this.position++] = bytes[3];
        this.buffer[this.position++] = bytes[4];
        this.buffer[this.position++] = bytes[5];
        this.buffer[this.position++] = bytes[6];
        this.buffer[this.position++] = bytes[7];
      }
      else
        this.WriteRawBytes(bytes, 0, 8);
    }

    public void WriteFloatNoTag(float value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      if (!BitConverter.IsLittleEndian)
        ByteArray.Reverse(bytes);
      if (this.limit - this.position >= 4)
      {
        this.buffer[this.position++] = bytes[0];
        this.buffer[this.position++] = bytes[1];
        this.buffer[this.position++] = bytes[2];
        this.buffer[this.position++] = bytes[3];
      }
      else
        this.WriteRawBytes(bytes, 0, 4);
    }

    [CLSCompliant(false)]
    public void WriteUInt64NoTag(ulong value)
    {
      this.WriteRawVarint64(value);
    }

    public void WriteInt64NoTag(long value)
    {
      this.WriteRawVarint64((ulong) value);
    }

    public void WriteInt32NoTag(int value)
    {
      if (value >= 0)
        this.WriteRawVarint32((uint) value);
      else
        this.WriteRawVarint64((ulong) value);
    }

    [CLSCompliant(false)]
    public void WriteFixed64NoTag(ulong value)
    {
      this.WriteRawLittleEndian64(value);
    }

    [CLSCompliant(false)]
    public void WriteFixed32NoTag(uint value)
    {
      this.WriteRawLittleEndian32(value);
    }

    public void WriteBoolNoTag(bool value)
    {
      this.WriteRawByte(value ? (byte) 1 : (byte) 0);
    }

    public void WriteStringNoTag(string value)
    {
      int byteCount = Encoding.UTF8.GetByteCount(value);
      this.WriteRawVarint32((uint) byteCount);
      if (this.limit - this.position >= byteCount)
      {
        Encoding.UTF8.GetBytes(value, 0, value.Length, this.buffer, this.position);
        this.position += byteCount;
      }
      else
        this.WriteRawBytes(Encoding.UTF8.GetBytes(value));
    }

    public void WriteGroupNoTag(IMessageLite value)
    {
      value.WriteTo((ICodedOutputStream) this);
    }

    public void WriteMessageNoTag(IMessageLite value)
    {
      this.WriteRawVarint32((uint) value.SerializedSize);
      value.WriteTo((ICodedOutputStream) this);
    }

    public void WriteBytesNoTag(ByteString value)
    {
      this.WriteRawVarint32((uint) value.Length);
      value.WriteRawBytesTo(this);
    }

    [CLSCompliant(false)]
    public void WriteUInt32NoTag(uint value)
    {
      this.WriteRawVarint32(value);
    }

    public void WriteEnumNoTag(int value)
    {
      this.WriteInt32NoTag(value);
    }

    public void WriteSFixed32NoTag(int value)
    {
      this.WriteRawLittleEndian32((uint) value);
    }

    public void WriteSFixed64NoTag(long value)
    {
      this.WriteRawLittleEndian64((ulong) value);
    }

    public void WriteSInt32NoTag(int value)
    {
      this.WriteRawVarint32(CodedOutputStream.EncodeZigZag32(value));
    }

    public void WriteSInt64NoTag(long value)
    {
      this.WriteRawVarint64(CodedOutputStream.EncodeZigZag64(value));
    }

    public void WriteArray(FieldType fieldType, int fieldNumber, string fieldName, IEnumerable list)
    {
      foreach (object obj in list)
        this.WriteField(fieldType, fieldNumber, fieldName, obj);
    }

    public void WriteGroupArray<T>(int fieldNumber, string fieldName, IEnumerable<T> list) where T : IMessageLite
    {
      foreach (T obj in list)
      {
        IMessageLite messageLite = (IMessageLite) obj;
        this.WriteGroup(fieldNumber, fieldName, messageLite);
      }
    }

    public void WriteMessageArray<T>(int fieldNumber, string fieldName, IEnumerable<T> list) where T : IMessageLite
    {
      foreach (T obj in list)
      {
        IMessageLite messageLite = (IMessageLite) obj;
        this.WriteMessage(fieldNumber, fieldName, messageLite);
      }
    }

    public void WriteStringArray(int fieldNumber, string fieldName, IEnumerable<string> list)
    {
      foreach (string str in list)
        this.WriteString(fieldNumber, fieldName, str);
    }

    public void WriteBytesArray(int fieldNumber, string fieldName, IEnumerable<ByteString> list)
    {
      foreach (ByteString byteString in list)
        this.WriteBytes(fieldNumber, fieldName, byteString);
    }

    public void WriteBoolArray(int fieldNumber, string fieldName, IEnumerable<bool> list)
    {
      foreach (bool flag in list)
        this.WriteBool(fieldNumber, fieldName, flag);
    }

    public void WriteInt32Array(int fieldNumber, string fieldName, IEnumerable<int> list)
    {
      foreach (int num in list)
        this.WriteInt32(fieldNumber, fieldName, num);
    }

    public void WriteSInt32Array(int fieldNumber, string fieldName, IEnumerable<int> list)
    {
      foreach (int num in list)
        this.WriteSInt32(fieldNumber, fieldName, num);
    }

    public void WriteUInt32Array(int fieldNumber, string fieldName, IEnumerable<uint> list)
    {
      foreach (uint num in list)
        this.WriteUInt32(fieldNumber, fieldName, num);
    }

    public void WriteFixed32Array(int fieldNumber, string fieldName, IEnumerable<uint> list)
    {
      foreach (uint num in list)
        this.WriteFixed32(fieldNumber, fieldName, num);
    }

    public void WriteSFixed32Array(int fieldNumber, string fieldName, IEnumerable<int> list)
    {
      foreach (int num in list)
        this.WriteSFixed32(fieldNumber, fieldName, num);
    }

    public void WriteInt64Array(int fieldNumber, string fieldName, IEnumerable<long> list)
    {
      foreach (long num in list)
        this.WriteInt64(fieldNumber, fieldName, num);
    }

    public void WriteSInt64Array(int fieldNumber, string fieldName, IEnumerable<long> list)
    {
      foreach (long num in list)
        this.WriteSInt64(fieldNumber, fieldName, num);
    }

    public void WriteUInt64Array(int fieldNumber, string fieldName, IEnumerable<ulong> list)
    {
      foreach (ulong num in list)
        this.WriteUInt64(fieldNumber, fieldName, num);
    }

    public void WriteFixed64Array(int fieldNumber, string fieldName, IEnumerable<ulong> list)
    {
      foreach (ulong num in list)
        this.WriteFixed64(fieldNumber, fieldName, num);
    }

    public void WriteSFixed64Array(int fieldNumber, string fieldName, IEnumerable<long> list)
    {
      foreach (long num in list)
        this.WriteSFixed64(fieldNumber, fieldName, num);
    }

    public void WriteDoubleArray(int fieldNumber, string fieldName, IEnumerable<double> list)
    {
      foreach (double num in list)
        this.WriteDouble(fieldNumber, fieldName, num);
    }

    public void WriteFloatArray(int fieldNumber, string fieldName, IEnumerable<float> list)
    {
      foreach (float num in list)
        this.WriteFloat(fieldNumber, fieldName, num);
    }

    [CLSCompliant(false)]
    public void WriteEnumArray<T>(int fieldNumber, string fieldName, IEnumerable<T> list) where T : struct, IComparable, IFormattable/*, IConvertible*/
    {
      if (list is ICastArray)
      {
        foreach (int num in ((ICastArray) list).CastArray<int>())
          this.WriteEnum(fieldNumber, fieldName, num, (object) null);
      }
      else
      {
        foreach (T obj1 in list)
        {
          object obj2 = (object) obj1;
          this.WriteEnum(fieldNumber, fieldName, (int) obj2, (object) null);
        }
      }
    }

    public void WritePackedArray(FieldType fieldType, int fieldNumber, string fieldName, IEnumerable list)
    {
      int num = 0;
      foreach (object obj in list)
        num += CodedOutputStream.ComputeFieldSizeNoTag(fieldType, obj);
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) num);
      foreach (object obj in list)
        this.WriteFieldNoTag(fieldType, obj);
    }

    public void WritePackedGroupArray<T>(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<T> list) where T : IMessageLite
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (T obj in list)
        this.WriteGroupNoTag((IMessageLite) obj);
    }

    public void WritePackedMessageArray<T>(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<T> list) where T : IMessageLite
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (T obj in list)
        this.WriteMessageNoTag((IMessageLite) obj);
    }

    public void WritePackedStringArray(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<string> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (string str in list)
        this.WriteStringNoTag(str);
    }

    public void WritePackedBytesArray(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<ByteString> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (ByteString byteString in list)
        this.WriteBytesNoTag(byteString);
    }

    public void WritePackedBoolArray(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<bool> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (bool flag in list)
        this.WriteBoolNoTag(flag);
    }

    public void WritePackedInt32Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<int> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (int num in list)
        this.WriteInt32NoTag(num);
    }

    public void WritePackedSInt32Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<int> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (int num in list)
        this.WriteSInt32NoTag(num);
    }

    public void WritePackedUInt32Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<uint> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (uint num in list)
        this.WriteUInt32NoTag(num);
    }

    public void WritePackedFixed32Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<uint> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (uint num in list)
        this.WriteFixed32NoTag(num);
    }

    public void WritePackedSFixed32Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<int> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (int num in list)
        this.WriteSFixed32NoTag(num);
    }

    public void WritePackedInt64Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<long> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (long num in list)
        this.WriteInt64NoTag(num);
    }

    public void WritePackedSInt64Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<long> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (long num in list)
        this.WriteSInt64NoTag(num);
    }

    public void WritePackedUInt64Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<ulong> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (ulong num in list)
        this.WriteUInt64NoTag(num);
    }

    public void WritePackedFixed64Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<ulong> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (ulong num in list)
        this.WriteFixed64NoTag(num);
    }

    public void WritePackedSFixed64Array(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<long> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (long num in list)
        this.WriteSFixed64NoTag(num);
    }

    public void WritePackedDoubleArray(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<double> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (double num in list)
        this.WriteDoubleNoTag(num);
    }

    public void WritePackedFloatArray(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<float> list)
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      foreach (float num in list)
        this.WriteFloatNoTag(num);
    }

    [CLSCompliant(false)]
    public void WritePackedEnumArray<T>(int fieldNumber, string fieldName, int calculatedSize, IEnumerable<T> list) where T : struct, IComparable, IFormattable/*, IConvertible*/
    {
      this.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
      this.WriteRawVarint32((uint) calculatedSize);
      if (list is ICastArray)
      {
        foreach (int num in ((ICastArray) list).CastArray<int>())
          this.WriteEnumNoTag(num);
      }
      else
      {
        foreach (T obj in list)
          this.WriteEnumNoTag((int) (object) obj);
      }
    }

    [CLSCompliant(false)]
    public void WriteTag(int fieldNumber, WireFormat.WireType type)
    {
      this.WriteRawVarint32(WireFormat.MakeTag(fieldNumber, type));
    }

    [CLSCompliant(false)]
    public void WriteRawVarint32(uint value)
    {
      while (value > (uint) sbyte.MaxValue && this.position < this.limit)
      {
        this.buffer[this.position++] = (byte) ((int) value & (int) sbyte.MaxValue | 128);
        value >>= 7;
      }
      while (value > (uint) sbyte.MaxValue)
      {
        this.WriteRawByte((byte) ((int) value & (int) sbyte.MaxValue | 128));
        value >>= 7;
      }
      if (this.position < this.limit)
        this.buffer[this.position++] = (byte) value;
      else
        this.WriteRawByte((byte) value);
    }

    [CLSCompliant(false)]
    public void WriteRawVarint64(ulong value)
    {
      while (value > (ulong) sbyte.MaxValue && this.position < this.limit)
      {
        this.buffer[this.position++] = (byte) (value & (ulong) sbyte.MaxValue | 128UL);
        value >>= 7;
      }
      while (value > (ulong) sbyte.MaxValue)
      {
        this.WriteRawByte((byte) (value & (ulong) sbyte.MaxValue | 128UL));
        value >>= 7;
      }
      if (this.position < this.limit)
        this.buffer[this.position++] = (byte) value;
      else
        this.WriteRawByte((byte) value);
    }

    [CLSCompliant(false)]
    public void WriteRawLittleEndian32(uint value)
    {
      if (this.position + 4 > this.limit)
      {
        this.WriteRawByte((byte) value);
        this.WriteRawByte((byte) (value >> 8));
        this.WriteRawByte((byte) (value >> 16));
        this.WriteRawByte((byte) (value >> 24));
      }
      else
      {
        this.buffer[this.position++] = (byte) value;
        this.buffer[this.position++] = (byte) (value >> 8);
        this.buffer[this.position++] = (byte) (value >> 16);
        this.buffer[this.position++] = (byte) (value >> 24);
      }
    }

    [CLSCompliant(false)]
    public void WriteRawLittleEndian64(ulong value)
    {
      if (this.position + 8 > this.limit)
      {
        this.WriteRawByte((byte) value);
        this.WriteRawByte((byte) (value >> 8));
        this.WriteRawByte((byte) (value >> 16));
        this.WriteRawByte((byte) (value >> 24));
        this.WriteRawByte((byte) (value >> 32));
        this.WriteRawByte((byte) (value >> 40));
        this.WriteRawByte((byte) (value >> 48));
        this.WriteRawByte((byte) (value >> 56));
      }
      else
      {
        this.buffer[this.position++] = (byte) value;
        this.buffer[this.position++] = (byte) (value >> 8);
        this.buffer[this.position++] = (byte) (value >> 16);
        this.buffer[this.position++] = (byte) (value >> 24);
        this.buffer[this.position++] = (byte) (value >> 32);
        this.buffer[this.position++] = (byte) (value >> 40);
        this.buffer[this.position++] = (byte) (value >> 48);
        this.buffer[this.position++] = (byte) (value >> 56);
      }
    }

    public void WriteRawByte(byte value)
    {
      if (this.position == this.limit)
        this.RefreshBuffer();
      this.buffer[this.position++] = value;
    }

    [CLSCompliant(false)]
    public void WriteRawByte(uint value)
    {
      this.WriteRawByte((byte) value);
    }

    public void WriteRawBytes(byte[] value)
    {
      this.WriteRawBytes(value, 0, value.Length);
    }

    public void WriteRawBytes(byte[] value, int offset, int length)
    {
      if (this.limit - this.position >= length)
      {
        ByteArray.Copy(value, offset, this.buffer, this.position, length);
        this.position += length;
      }
      else
      {
        int count = this.limit - this.position;
        ByteArray.Copy(value, offset, this.buffer, this.position, count);
        offset += count;
        length -= count;
        this.position = this.limit;
        this.RefreshBuffer();
        if (length <= this.limit)
        {
          ByteArray.Copy(value, offset, this.buffer, 0, length);
          this.position = length;
        }
        else
          this.output.Write(value, offset, length);
      }
    }

    [CLSCompliant(false)]
    public static uint EncodeZigZag32(int n)
    {
      return (uint) (n << 1 ^ n >> 31);
    }

    [CLSCompliant(false)]
    public static ulong EncodeZigZag64(long n)
    {
      return (ulong) (n << 1 ^ n >> 63);
    }

    private void RefreshBuffer()
    {
      if (this.output == null)
        throw new CodedOutputStream.OutOfSpaceException();
      this.output.Write(this.buffer, 0, this.position);
      this.position = 0;
    }

    public void Flush()
    {
      if (this.output == null)
        return;
      this.RefreshBuffer();
    }

    public void CheckNoSpaceLeft()
    {
      if (this.SpaceLeft != 0)
        throw new InvalidOperationException("Did not write as much data as expected.");
    }

    public sealed class OutOfSpaceException : IOException
    {
      internal OutOfSpaceException()
        : base("CodedOutputStream was writing to a flat byte array and ran out of space.")
      {
      }
    }
  }
}
