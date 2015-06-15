// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.ExtendableBuilderLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Descriptors;
using System;
using System.Collections.Generic;

namespace Google.ProtocolBuffers
{
  public abstract class ExtendableBuilderLite<TMessage, TBuilder> : GeneratedBuilderLite<TMessage, TBuilder> where TMessage : ExtendableMessageLite<TMessage, TBuilder> where TBuilder : GeneratedBuilderLite<TMessage, TBuilder>
  {
    public object this[IFieldDescriptorLite field, int index]
    {
      set
      {
        if (!field.IsExtension)
          throw new NotSupportedException("Not supported in the lite runtime.");
        this.MessageBeingBuilt.Extensions[field, index] = value;
      }
    }

    public object this[IFieldDescriptorLite field]
    {
      set
      {
        if (!field.IsExtension)
          throw new NotSupportedException("Not supported in the lite runtime.");
        this.MessageBeingBuilt.Extensions[field] = value;
      }
    }

    public bool HasExtension<TExtension>(GeneratedExtensionLite<TMessage, TExtension> extension)
    {
      return this.MessageBeingBuilt.HasExtension<TExtension>(extension);
    }

    public int GetExtensionCount<TExtension>(GeneratedExtensionLite<TMessage, IList<TExtension>> extension)
    {
      return this.MessageBeingBuilt.GetExtensionCount<TExtension>(extension);
    }

    public TExtension GetExtension<TExtension>(GeneratedExtensionLite<TMessage, TExtension> extension)
    {
      return this.MessageBeingBuilt.GetExtension<TExtension>(extension);
    }

    public TExtension GetExtension<TExtension>(GeneratedExtensionLite<TMessage, IList<TExtension>> extension, int index)
    {
      return this.MessageBeingBuilt.GetExtension<TExtension>(extension, index);
    }

    public TBuilder SetExtension<TExtension>(GeneratedExtensionLite<TMessage, TExtension> extension, TExtension value)
    {
      ExtendableMessageLite<TMessage, TBuilder> extendableMessageLite = (ExtendableMessageLite<TMessage, TBuilder>) this.MessageBeingBuilt;
      extendableMessageLite.VerifyExtensionContainingType<TExtension>(extension);
      extendableMessageLite.Extensions[extension.Descriptor] = extension.ToReflectionType((object) value);
      return this.ThisBuilder;
    }

    public TBuilder SetExtension<TExtension>(GeneratedExtensionLite<TMessage, IList<TExtension>> extension, int index, TExtension value)
    {
      ExtendableMessageLite<TMessage, TBuilder> extendableMessageLite = (ExtendableMessageLite<TMessage, TBuilder>) this.MessageBeingBuilt;
      extendableMessageLite.VerifyExtensionContainingType<IList<TExtension>>(extension);
      extendableMessageLite.Extensions[extension.Descriptor, index] = extension.SingularToReflectionType((object) value);
      return this.ThisBuilder;
    }

    public TBuilder AddExtension<TExtension>(GeneratedExtensionLite<TMessage, IList<TExtension>> extension, TExtension value)
    {
      ExtendableMessageLite<TMessage, TBuilder> extendableMessageLite = (ExtendableMessageLite<TMessage, TBuilder>) this.MessageBeingBuilt;
      extendableMessageLite.VerifyExtensionContainingType<IList<TExtension>>(extension);
      extendableMessageLite.Extensions.AddRepeatedField(extension.Descriptor, extension.SingularToReflectionType((object) value));
      return this.ThisBuilder;
    }

    public TBuilder ClearExtension<TExtension>(GeneratedExtensionLite<TMessage, TExtension> extension)
    {
      ExtendableMessageLite<TMessage, TBuilder> extendableMessageLite = (ExtendableMessageLite<TMessage, TBuilder>) this.MessageBeingBuilt;
      extendableMessageLite.VerifyExtensionContainingType<TExtension>(extension);
      extendableMessageLite.Extensions.ClearField(extension.Descriptor);
      return this.ThisBuilder;
    }

    [CLSCompliant(false)]
    protected override bool ParseUnknownField(ICodedInputStream input, ExtensionRegistry extensionRegistry, uint tag, string fieldName)
    {
      FieldSet extensions = this.MessageBeingBuilt.Extensions;
      WireFormat.WireType tagWireType = WireFormat.GetTagWireType(tag);
      int tagFieldNumber = WireFormat.GetTagFieldNumber(tag);
      IGeneratedExtensionLite generatedExtensionLite = extensionRegistry[(IMessageLite) this.DefaultInstanceForType, tagFieldNumber];
      if (generatedExtensionLite == null)
        return input.SkipField();
      IFieldDescriptorLite descriptor = generatedExtensionLite.Descriptor;
      if (descriptor == null)
        return input.SkipField();
      WireFormat.WireType wireType1 = descriptor.IsPacked ? WireFormat.WireType.LengthDelimited : WireFormat.GetWireType(descriptor.FieldType);
      if (tagWireType != wireType1)
      {
        WireFormat.WireType wireType2 = WireFormat.GetWireType(descriptor.FieldType);
        if (tagWireType != wireType2 && (!descriptor.IsRepeated || tagWireType != WireFormat.WireType.LengthDelimited || wireType2 != WireFormat.WireType.Varint && wireType2 != WireFormat.WireType.Fixed32 && wireType2 != WireFormat.WireType.Fixed64))
          return input.SkipField();
      }
      if (!descriptor.IsRepeated && tagWireType != WireFormat.GetWireType(descriptor.FieldType))
        return input.SkipField();
      switch (descriptor.FieldType)
      {
        case FieldType.Group:
        case FieldType.Message:
          if (!descriptor.IsRepeated)
          {
            IBuilderLite builder = (extensions[generatedExtensionLite.Descriptor] as IMessageLite ?? generatedExtensionLite.MessageDefaultInstance).WeakToBuilder();
            if (descriptor.FieldType == FieldType.Group)
              input.ReadGroup(descriptor.FieldNumber, builder, extensionRegistry);
            else
              input.ReadMessage(builder, extensionRegistry);
            extensions[descriptor] = (object) builder.WeakBuild();
            break;
          }
          List<IMessageLite> list1 = new List<IMessageLite>();
          if (descriptor.FieldType == FieldType.Group)
            input.ReadGroupArray<IMessageLite>(tag, fieldName, (ICollection<IMessageLite>) list1, generatedExtensionLite.MessageDefaultInstance, extensionRegistry);
          else
            input.ReadMessageArray<IMessageLite>(tag, fieldName, (ICollection<IMessageLite>) list1, generatedExtensionLite.MessageDefaultInstance, extensionRegistry);
          foreach (IMessageLite messageLite in list1)
            extensions.AddRepeatedField(descriptor, (object) messageLite);
          return true;
        case FieldType.Enum:
          if (!descriptor.IsRepeated)
          {
            IEnumLite enumLite = (IEnumLite) null;
            object unknown;
            if (input.ReadEnum(ref enumLite, out unknown, descriptor.EnumType))
            {
              extensions[descriptor] = (object) enumLite;
              break;
            }
            break;
          }
          List<IEnumLite> list2 = new List<IEnumLite>();
          ICollection<object> unknown1;
          input.ReadEnumArray(tag, fieldName, (ICollection<IEnumLite>) list2, out unknown1, descriptor.EnumType);
          using (List<IEnumLite>.Enumerator enumerator = list2.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              IEnumLite current = enumerator.Current;
              extensions.AddRepeatedField(descriptor, (object) current);
            }
            break;
          }
        default:
          if (!descriptor.IsRepeated)
          {
            object obj = (object) null;
            if (input.ReadPrimitiveField(descriptor.FieldType, ref obj))
            {
              extensions[descriptor] = obj;
              break;
            }
            break;
          }
          List<object> list3 = new List<object>();
          input.ReadPrimitiveArray(descriptor.FieldType, tag, fieldName, (ICollection<object>) list3);
          using (List<object>.Enumerator enumerator = list3.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              object current = enumerator.Current;
              extensions.AddRepeatedField(descriptor, current);
            }
            break;
          }
      }
      return true;
    }

    public TBuilder ClearField(IFieldDescriptorLite field)
    {
      if (!field.IsExtension)
        throw new NotSupportedException("Not supported in the lite runtime.");
      this.MessageBeingBuilt.Extensions.ClearField(field);
      return this.ThisBuilder;
    }

    public TBuilder AddRepeatedField(IFieldDescriptorLite field, object value)
    {
      if (!field.IsExtension)
        throw new NotSupportedException("Not supported in the lite runtime.");
      this.MessageBeingBuilt.Extensions.AddRepeatedField(field, value);
      return this.ThisBuilder;
    }

    protected void MergeExtensionFields(ExtendableMessageLite<TMessage, TBuilder> other)
    {
      this.MessageBeingBuilt.Extensions.MergeFrom(other.Extensions);
    }
  }
}
