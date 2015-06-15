// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.GeneratedRepeatExtensionLite`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using Google.ProtocolBuffers.Descriptors;
using System.Collections;
using System.Collections.Generic;

namespace Google.ProtocolBuffers
{
  public class GeneratedRepeatExtensionLite<TContainingType, TExtensionType> : GeneratedExtensionLite<TContainingType, IList<TExtensionType>> where TContainingType : IMessageLite
  {
    public GeneratedRepeatExtensionLite(string fullName, TContainingType containingTypeDefaultInstance, IMessageLite messageDefaultInstance, IEnumLiteMap enumTypeMap, int number, FieldType type, bool isPacked)
      : base(fullName, containingTypeDefaultInstance, (IList<TExtensionType>) new List<TExtensionType>(), messageDefaultInstance, enumTypeMap, number, type, isPacked)
    {
    }

    public override object ToReflectionType(object value)
    {
      IList<object> list = (IList<object>) new List<object>();
      foreach (object obj in (IEnumerable) value)
        list.Add(this.SingularToReflectionType(obj));
      return (object) list;
    }

    public override object FromReflectionType(object value)
    {
      List<TExtensionType> list = new List<TExtensionType>();
      foreach (object obj in (IEnumerable) value)
        list.Add((TExtensionType) this.SingularFromReflectionType(obj));
      return (object) list;
    }
  }
}
