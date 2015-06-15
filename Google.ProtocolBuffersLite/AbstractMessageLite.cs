// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.AbstractMessageLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System.IO;

namespace Google.ProtocolBuffers
{
  public abstract class AbstractMessageLite<TMessage, TBuilder> : IMessageLite<TMessage, TBuilder>, IMessageLite<TMessage>, IMessageLite where TMessage : AbstractMessageLite<TMessage, TBuilder> where TBuilder : AbstractBuilderLite<TMessage, TBuilder>
  {
    public abstract TMessage DefaultInstanceForType { get; }

    public abstract bool IsInitialized { get; }

    public abstract int SerializedSize { get; }

    IMessageLite IMessageLite.WeakDefaultInstanceForType
    {
      get
      {
        return (IMessageLite) this.DefaultInstanceForType;
      }
    }

    public abstract TBuilder CreateBuilderForType();

    public abstract TBuilder ToBuilder();

    public abstract void WriteTo(ICodedOutputStream output);

    public abstract void PrintTo(TextWriter writer);

    public ByteString ToByteString()
    {
      ByteString.CodedBuilder codedBuilder = new ByteString.CodedBuilder(this.SerializedSize);
      this.WriteTo((ICodedOutputStream) codedBuilder.CodedOutput);
      return codedBuilder.Build();
    }

    public byte[] ToByteArray()
    {
      byte[] flatArray = new byte[this.SerializedSize];
      CodedOutputStream instance = CodedOutputStream.CreateInstance(flatArray);
      this.WriteTo((ICodedOutputStream) instance);
      instance.CheckNoSpaceLeft();
      return flatArray;
    }

    public void WriteTo(Stream output)
    {
      CodedOutputStream instance = CodedOutputStream.CreateInstance(output);
      this.WriteTo((ICodedOutputStream) instance);
      instance.Flush();
    }

    public void WriteDelimitedTo(Stream output)
    {
      CodedOutputStream instance = CodedOutputStream.CreateInstance(output);
      instance.WriteRawVarint32((uint) this.SerializedSize);
      this.WriteTo((ICodedOutputStream) instance);
      instance.Flush();
    }

    IBuilderLite IMessageLite.WeakCreateBuilderForType()
    {
      return (IBuilderLite) this.CreateBuilderForType();
    }

    IBuilderLite IMessageLite.WeakToBuilder()
    {
      return (IBuilderLite) this.ToBuilder();
    }
  }
}
