// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.GeneratedMessageLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Google.ProtocolBuffers
{
  public abstract class GeneratedMessageLite<TMessage, TBuilder> : AbstractMessageLite<TMessage, TBuilder> where TMessage : GeneratedMessageLite<TMessage, TBuilder> where TBuilder : GeneratedBuilderLite<TMessage, TBuilder>
  {
    protected abstract TMessage ThisMessage { get; }

    public override sealed string ToString()
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        this.PrintTo((TextWriter) stringWriter);
        return stringWriter.ToString();
      }
    }

    protected static void PrintField<T>(string name, IList<T> value, TextWriter writer)
    {
      foreach (T obj in (IEnumerable<T>) value)
        GeneratedMessageLite<TMessage, TBuilder>.PrintField(name, true, (object) obj, writer);
    }

    protected static void PrintField(string name, bool hasValue, object value, TextWriter writer)
    {
      if (!hasValue)
        return;
      if (value is IMessageLite)
      {
        writer.WriteLine("{0} {{", (object) name);
        ((IMessageLite) value).PrintTo(writer);
        writer.WriteLine("}");
      }
      else if (value is ByteString || value is string)
      {
        writer.Write("{0}: \"", (object) name);
        if (value is string)
          GeneratedMessageLite<TMessage, TBuilder>.EscapeBytes((IEnumerable<byte>) Encoding.UTF8.GetBytes((string) value), writer);
        else
          GeneratedMessageLite<TMessage, TBuilder>.EscapeBytes((IEnumerable<byte>) value, writer);
        writer.WriteLine("\"");
      }
      else if (value is bool)
        writer.WriteLine("{0}: {1}", (object) name, (bool) value ? (object) "true" : (object) "false");
      else if (value is IEnumLite)
        writer.WriteLine("{0}: {1}", (object) name, (object) ((IEnumLite) value).Name);
      else
        writer.WriteLine("{0}: {1}", (object) name, (object) (value).ToString());
    }

    private static void EscapeBytes(IEnumerable<byte> input, TextWriter writer)
    {
      foreach (byte num1 in input)
      {
        byte num2 = num1;
        if ((uint) num2 <= 34U)
        {
          switch (num2)
          {
            case (byte) 7:
              writer.Write("\\a");
              continue;
            case (byte) 8:
              writer.Write("\\b");
              continue;
            case (byte) 9:
              writer.Write("\\t");
              continue;
            case (byte) 10:
              writer.Write("\\n");
              continue;
            case (byte) 11:
              writer.Write("\\v");
              continue;
            case (byte) 12:
              writer.Write("\\f");
              continue;
            case (byte) 13:
              writer.Write("\\r");
              continue;
            case (byte) 34:
              writer.Write("\\\"");
              continue;
          }
        }
        else if ((int) num2 != 39)
        {
          if ((int) num2 == 92)
          {
            writer.Write("\\\\");
            continue;
          }
        }
        else
        {
          writer.Write("\\'");
          continue;
        }
        if ((int) num1 >= 32 && (int) num1 < 128)
        {
          writer.Write((char) num1);
        }
        else
        {
          writer.Write('\\');
          writer.Write((char) (48 + ((int) num1 >> 6 & 3)));
          writer.Write((char) (48 + ((int) num1 >> 3 & 7)));
          writer.Write((char) (48 + ((int) num1 & 7)));
        }
      }
    }
  }
}
