// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.GeneratedBuilderLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;

namespace Google.ProtocolBuffers
{
  public abstract class GeneratedBuilderLite<TMessage, TBuilder> : AbstractBuilderLite<TMessage, TBuilder> where TMessage : GeneratedMessageLite<TMessage, TBuilder> where TBuilder : GeneratedBuilderLite<TMessage, TBuilder>
  {
    protected abstract TMessage MessageBeingBuilt { get; }

    public override TBuilder MergeFrom(IMessageLite other)
    {
      return this.ThisBuilder;
    }

    public abstract TBuilder MergeFrom(TMessage other);

    [CLSCompliant(false)]
    protected virtual bool ParseUnknownField(ICodedInputStream input, ExtensionRegistry extensionRegistry, uint tag, string fieldName)
    {
      return input.SkipField();
    }

    public TMessage BuildParsed()
    {
      if (!this.IsInitialized)
        throw new UninitializedMessageException((IMessageLite) this.MessageBeingBuilt).AsInvalidProtocolBufferException();
      return this.BuildPartial();
    }

    public override TMessage Build()
    {
      if (!this.IsInitialized)
        throw new UninitializedMessageException((IMessageLite) this.MessageBeingBuilt);
      return this.BuildPartial();
    }
  }
}
