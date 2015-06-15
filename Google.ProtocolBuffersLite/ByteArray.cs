// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.ByteArray
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;

namespace Google.ProtocolBuffers
{
  internal static class ByteArray
  {
    private const int CopyThreshold = 12;

    public static void Copy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
    {
      if (count > 12)
        Buffer.BlockCopy((Array) src, srcOffset, (Array) dst, dstOffset, count);
      else
        ByteArray.ByteCopy(src, srcOffset, dst, dstOffset, count);
    }

    public static void ByteCopy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
    {
      int num = srcOffset + count;
      for (int index = srcOffset; index < num; ++index)
        dst[dstOffset++] = src[index];
    }

    public static void Reverse(byte[] bytes)
    {
      int index1 = 0;
      for (int index2 = bytes.Length - 1; index1 < index2; --index2)
      {
        byte num = bytes[index1];
        bytes[index1] = bytes[index2];
        bytes[index2] = num;
        ++index1;
      }
    }
  }
}
