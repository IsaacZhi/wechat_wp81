// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.ExtendableMessageLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Google.ProtocolBuffers
{
  public abstract class ExtendableMessageLite<TMessage, TBuilder> : GeneratedMessageLite<TMessage, TBuilder> where TMessage : GeneratedMessageLite<TMessage, TBuilder> where TBuilder : GeneratedBuilderLite<TMessage, TBuilder>
  {
    private readonly FieldSet extensions = FieldSet.CreateInstance();

    internal FieldSet Extensions
    {
      get
      {
        return this.extensions;
      }
    }

    protected bool ExtensionsAreInitialized
    {
      get
      {
        return this.extensions.IsInitialized;
      }
    }

    public override bool IsInitialized
    {
      get
      {
        return this.ExtensionsAreInitialized;
      }
    }

    protected int ExtensionsSerializedSize
    {
      get
      {
        return this.extensions.SerializedSize;
      }
    }

    public override bool Equals(object obj)
    {
      ExtendableMessageLite<TMessage, TBuilder> extendableMessageLite = obj as ExtendableMessageLite<TMessage, TBuilder>;
      if (!object.ReferenceEquals((object) null, (object) extendableMessageLite))
        return Dictionaries.Equals<IFieldDescriptorLite, object>(this.extensions.AllFields, extendableMessageLite.extensions.AllFields);
      return false;
    }

    public override int GetHashCode()
    {
      return Dictionaries.GetHashCode<IFieldDescriptorLite, object>(this.extensions.AllFields);
    }

    public override void PrintTo(TextWriter writer)
    {
      foreach (KeyValuePair<IFieldDescriptorLite, object> keyValuePair in (IEnumerable<KeyValuePair<IFieldDescriptorLite, object>>) this.extensions.AllFields)
      {
        string name = string.Format("[{0}]", (object) keyValuePair.Key.FullName);
        if (keyValuePair.Key.IsRepeated)
        {
          foreach (object obj in (IEnumerable) keyValuePair.Value)
            GeneratedMessageLite<TMessage, TBuilder>.PrintField(name, true, obj, writer);
        }
        else
          GeneratedMessageLite<TMessage, TBuilder>.PrintField(name, true, keyValuePair.Value, writer);
      }
    }

    public bool HasExtension<TExtension>(GeneratedExtensionLite<TMessage, TExtension> extension)
    {
      this.VerifyExtensionContainingType<TExtension>(extension);
      return this.extensions.HasField(extension.Descriptor);
    }

    public int GetExtensionCount<TExtension>(GeneratedExtensionLite<TMessage, IList<TExtension>> extension)
    {
      this.VerifyExtensionContainingType<IList<TExtension>>(extension);
      return this.extensions.GetRepeatedFieldCount(extension.Descriptor);
    }

    public TExtension GetExtension<TExtension>(GeneratedExtensionLite<TMessage, TExtension> extension)
    {
      this.VerifyExtensionContainingType<TExtension>(extension);
      object obj = this.extensions[extension.Descriptor];
      if (obj == null)
        return extension.DefaultValue;
      return (TExtension) extension.FromReflectionType(obj);
    }

    public TExtension GetExtension<TExtension>(GeneratedExtensionLite<TMessage, IList<TExtension>> extension, int index)
    {
      this.VerifyExtensionContainingType<IList<TExtension>>(extension);
      return (TExtension) extension.SingularFromReflectionType(this.extensions[extension.Descriptor, index]);
    }

    protected ExtendableMessageLite<TMessage, TBuilder>.ExtensionWriter CreateExtensionWriter(ExtendableMessageLite<TMessage, TBuilder> message)
    {
      return new ExtendableMessageLite<TMessage, TBuilder>.ExtensionWriter(message);
    }

    internal void VerifyExtensionContainingType<TExtension>(GeneratedExtensionLite<TMessage, TExtension> extension)
    {
      if (!object.ReferenceEquals((object) extension.ContainingTypeDefaultInstance, (object) this.DefaultInstanceForType))
        throw new ArgumentException(string.Format("Extension is for type \"{0}\" which does not match message type \"{1}\".", (object) extension.ContainingTypeDefaultInstance, (object) this.DefaultInstanceForType));
    }

    protected class ExtensionWriter
    {
      private KeyValuePair<IFieldDescriptorLite, object>? next = new KeyValuePair<IFieldDescriptorLite, object>?();
      private readonly IEnumerator<KeyValuePair<IFieldDescriptorLite, object>> iterator;
      private readonly FieldSet extensions;

      internal ExtensionWriter(ExtendableMessageLite<TMessage, TBuilder> message)
      {
        this.extensions = message.extensions;
        this.iterator = message.extensions.GetEnumerator();
        if (!this.iterator.MoveNext())
          return;
        this.next = new KeyValuePair<IFieldDescriptorLite, object>?(this.iterator.Current);
      }

      public void WriteUntil(int end, ICodedOutputStream output)
      {
        for (; this.next.HasValue && this.next.Value.Key.FieldNumber < end; this.next = !this.iterator.MoveNext() ? new KeyValuePair<IFieldDescriptorLite, object>?() : new KeyValuePair<IFieldDescriptorLite, object>?(this.iterator.Current))
          this.extensions.WriteField(this.next.Value.Key, this.next.Value.Value, output);
      }
    }
  }
}
