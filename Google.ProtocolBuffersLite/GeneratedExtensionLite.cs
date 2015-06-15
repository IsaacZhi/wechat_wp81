// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.GeneratedExtensionLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Descriptors;
using System.Collections.Generic;

namespace Google.ProtocolBuffers
{
  public class GeneratedExtensionLite<TContainingType, TExtensionType> : IGeneratedExtensionLite where TContainingType : IMessageLite
  {
    private static readonly IList<object> Empty = (IList<object>) new object[0];
    private readonly TContainingType containingTypeDefaultInstance;
    private readonly TExtensionType defaultValue;
    private readonly IMessageLite messageDefaultInstance;
    private readonly ExtensionDescriptorLite descriptor;

    public IFieldDescriptorLite Descriptor
    {
      get
      {
        return (IFieldDescriptorLite) this.descriptor;
      }
    }

    public TExtensionType DefaultValue
    {
      get
      {
        return this.defaultValue;
      }
    }

    object IGeneratedExtensionLite.ContainingType
    {
      get
      {
        return (object) this.ContainingTypeDefaultInstance;
      }
    }

    public TContainingType ContainingTypeDefaultInstance
    {
      get
      {
        return this.containingTypeDefaultInstance;
      }
    }

    public int Number
    {
      get
      {
        return this.descriptor.FieldNumber;
      }
    }

    public IMessageLite MessageDefaultInstance
    {
      get
      {
        return this.messageDefaultInstance;
      }
    }

    protected GeneratedExtensionLite(TContainingType containingTypeDefaultInstance, TExtensionType defaultValue, IMessageLite messageDefaultInstance, ExtensionDescriptorLite descriptor)
    {
      this.containingTypeDefaultInstance = containingTypeDefaultInstance;
      this.messageDefaultInstance = messageDefaultInstance;
      this.defaultValue = defaultValue;
      this.descriptor = descriptor;
    }

    public GeneratedExtensionLite(string fullName, TContainingType containingTypeDefaultInstance, TExtensionType defaultValue, IMessageLite messageDefaultInstance, IEnumLiteMap enumTypeMap, int number, FieldType type)
      : this(containingTypeDefaultInstance, defaultValue, messageDefaultInstance, new ExtensionDescriptorLite(fullName, enumTypeMap, number, type, (object) defaultValue, false, false))
    {
    }

    protected GeneratedExtensionLite(string fullName, TContainingType containingTypeDefaultInstance, TExtensionType defaultValue, IMessageLite messageDefaultInstance, IEnumLiteMap enumTypeMap, int number, FieldType type, bool isPacked)
      : this(containingTypeDefaultInstance, defaultValue, messageDefaultInstance, new ExtensionDescriptorLite(fullName, enumTypeMap, number, type, (object) GeneratedExtensionLite<TContainingType, TExtensionType>.Empty, true, isPacked))
    {
    }

    public virtual object ToReflectionType(object value)
    {
      return this.SingularToReflectionType(value);
    }

    public object SingularToReflectionType(object value)
    {
      if (this.descriptor.MappedType != MappedType.Enum)
        return value;
      return (object) this.descriptor.EnumType.FindValueByNumber((int) value);
    }

    public virtual object FromReflectionType(object value)
    {
      return this.SingularFromReflectionType(value);
    }

    public object SingularFromReflectionType(object value)
    {
      switch (this.Descriptor.MappedType)
      {
        case MappedType.Message:
          if (value is TExtensionType)
            return value;
          return (object) this.MessageDefaultInstance.WeakCreateBuilderForType().WeakMergeFrom((IMessageLite) value).WeakBuild();
        case MappedType.Enum:
          return (object) ((IEnumLite) value).Number;
        default:
          return value;
      }
    }
  }
}
