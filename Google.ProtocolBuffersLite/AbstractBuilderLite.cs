// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.AbstractBuilderLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.IO;

namespace Google.ProtocolBuffers
{
  public abstract class AbstractBuilderLite<TMessage, TBuilder> : IBuilderLite<TMessage, TBuilder>, IBuilderLite where TMessage : AbstractMessageLite<TMessage, TBuilder> where TBuilder : AbstractBuilderLite<TMessage, TBuilder>
  {
    protected abstract TBuilder ThisBuilder { get; }

    public abstract bool IsInitialized { get; }

    public abstract TMessage DefaultInstanceForType { get; }

    IMessageLite IBuilderLite.WeakDefaultInstanceForType
    {
      get
      {
        return (IMessageLite) this.DefaultInstanceForType;
      }
    }

    public abstract TBuilder Clear();

    public abstract TBuilder Clone();

    public abstract TMessage Build();

    public abstract TMessage BuildPartial();

    public abstract TBuilder MergeFrom(IMessageLite other);

    public abstract TBuilder MergeFrom(ICodedInputStream input, ExtensionRegistry extensionRegistry);

    public virtual TBuilder MergeFrom(ICodedInputStream input)
    {
      return this.MergeFrom(input, ExtensionRegistry.CreateInstance());
    }

    public TBuilder MergeDelimitedFrom(Stream input)
    {
      return this.MergeDelimitedFrom(input, ExtensionRegistry.CreateInstance());
    }

    public TBuilder MergeDelimitedFrom(Stream input, ExtensionRegistry extensionRegistry)
    {
      int size = (int) CodedInputStream.ReadRawVarint32(input);
      return this.MergeFrom((Stream) new AbstractBuilderLite<TMessage, TBuilder>.LimitedInputStream(input, size), extensionRegistry);
    }

    public TBuilder MergeFrom(ByteString data)
    {
      return this.MergeFrom(data, ExtensionRegistry.CreateInstance());
    }

    public TBuilder MergeFrom(ByteString data, ExtensionRegistry extensionRegistry)
    {
      CodedInputStream codedInput = data.CreateCodedInput();
      this.MergeFrom((ICodedInputStream) codedInput, extensionRegistry);
      codedInput.CheckLastTagWas(0U);
      return this.ThisBuilder;
    }

    public TBuilder MergeFrom(byte[] data)
    {
      CodedInputStream instance = CodedInputStream.CreateInstance(data);
      this.MergeFrom((ICodedInputStream) instance);
      instance.CheckLastTagWas(0U);
      return this.ThisBuilder;
    }

    public TBuilder MergeFrom(byte[] data, ExtensionRegistry extensionRegistry)
    {
      CodedInputStream instance = CodedInputStream.CreateInstance(data);
      this.MergeFrom((ICodedInputStream) instance, extensionRegistry);
      instance.CheckLastTagWas(0U);
      return this.ThisBuilder;
    }

    public TBuilder MergeFrom(Stream input)
    {
      CodedInputStream instance = CodedInputStream.CreateInstance(input);
      this.MergeFrom((ICodedInputStream) instance);
      instance.CheckLastTagWas(0U);
      return this.ThisBuilder;
    }

    public TBuilder MergeFrom(Stream input, ExtensionRegistry extensionRegistry)
    {
      CodedInputStream instance = CodedInputStream.CreateInstance(input);
      this.MergeFrom((ICodedInputStream) instance, extensionRegistry);
      instance.CheckLastTagWas(0U);
      return this.ThisBuilder;
    }

    IBuilderLite IBuilderLite.WeakClear()
    {
      return (IBuilderLite) this.Clear();
    }

    IBuilderLite IBuilderLite.WeakMergeFrom(IMessageLite message)
    {
      return (IBuilderLite) this.MergeFrom(message);
    }

    IBuilderLite IBuilderLite.WeakMergeFrom(ByteString data)
    {
      return (IBuilderLite) this.MergeFrom(data);
    }

    IBuilderLite IBuilderLite.WeakMergeFrom(ByteString data, ExtensionRegistry registry)
    {
      return (IBuilderLite) this.MergeFrom(data, registry);
    }

    IBuilderLite IBuilderLite.WeakMergeFrom(ICodedInputStream input)
    {
      return (IBuilderLite) this.MergeFrom(input);
    }

    IBuilderLite IBuilderLite.WeakMergeFrom(ICodedInputStream input, ExtensionRegistry registry)
    {
      return (IBuilderLite) this.MergeFrom(input, registry);
    }

    IMessageLite IBuilderLite.WeakBuild()
    {
      return (IMessageLite) this.Build();
    }

    IMessageLite IBuilderLite.WeakBuildPartial()
    {
      return (IMessageLite) this.BuildPartial();
    }

    IBuilderLite IBuilderLite.WeakClone()
    {
      return (IBuilderLite) this.Clone();
    }

    private class LimitedInputStream : Stream
    {
      private readonly Stream proxied;
      private int bytesLeft;

      public override bool CanRead
      {
        get
        {
          return true;
        }
      }

      public override bool CanSeek
      {
        get
        {
          return false;
        }
      }

      public override bool CanWrite
      {
        get
        {
          return false;
        }
      }

      public override long Length
      {
        get
        {
          throw new NotSupportedException();
        }
      }

      public override long Position
      {
        get
        {
          throw new NotSupportedException();
        }
        set
        {
          throw new NotSupportedException();
        }
      }

      internal LimitedInputStream(Stream proxied, int size)
      {
        this.proxied = proxied;
        this.bytesLeft = size;
      }

      public override void Flush()
      {
      }

      public override int Read(byte[] buffer, int offset, int count)
      {
        if (this.bytesLeft <= 0)
          return 0;
        int num = this.proxied.Read(buffer, offset, Math.Min(this.bytesLeft, count));
        this.bytesLeft -= num;
        return num;
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
        throw new NotSupportedException();
      }

      public override void SetLength(long value)
      {
        throw new NotSupportedException();
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
        throw new NotSupportedException();
      }
    }
  }
}
