// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.IMessageLite
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System.IO;

namespace Google.ProtocolBuffers
{
  public interface IMessageLite
  {
    bool IsInitialized { get; }

    int SerializedSize { get; }

    IMessageLite WeakDefaultInstanceForType { get; }

    void WriteTo(ICodedOutputStream output);

    void WriteDelimitedTo(Stream output);

    bool Equals(object other);

    int GetHashCode();

    string ToString();

    void PrintTo(TextWriter writer);

    ByteString ToByteString();

    byte[] ToByteArray();

    void WriteTo(Stream output);

    IBuilderLite WeakCreateBuilderForType();

    IBuilderLite WeakToBuilder();
  }
  
  public interface IMessageLite<TMessage> : IMessageLite
  {
    TMessage DefaultInstanceForType { get; }
  }
  
  public interface IMessageLite<TMessage, TBuilder> : IMessageLite<TMessage>, IMessageLite where TMessage : IMessageLite<TMessage, TBuilder> where TBuilder : IBuilderLite<TMessage, TBuilder>
  {
    TBuilder CreateBuilderForType();

    TBuilder ToBuilder();
  }
}
