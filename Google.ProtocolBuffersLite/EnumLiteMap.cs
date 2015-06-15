// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.EnumLiteMap`1
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.Reflection;

namespace Google.ProtocolBuffers
{
  public class EnumLiteMap<TEnum> : IEnumLiteMap<IEnumLite>, IEnumLiteMap where TEnum : struct, IComparable, IFormattable
  {
    private readonly SortedList<int, IEnumLite> items;

    public EnumLiteMap()
    {
      this.items = new SortedList<int, IEnumLite>();
      foreach (FieldInfo fieldInfo in typeof(TEnum).GetRuntimeFields())
      {
        TEnum @enum = (TEnum) fieldInfo.GetValue((object) null);
        this.items.Add(Convert.ToInt32((object) @enum), (IEnumLite) new EnumLiteMap<TEnum>.EnumValue(@enum));
      }

    }

    IEnumLite IEnumLiteMap.FindValueByNumber(int number)
    {
      return this.FindValueByNumber(number);
    }

    public IEnumLite FindValueByNumber(int number)
    {
      IEnumLite enumLite;
      if (!this.items.TryGetValue(number, out enumLite))
        return (IEnumLite) null;
      return enumLite;
    }

    public IEnumLite FindValueByName(string name)
    {
      if (!Enum.IsDefined(typeof (TEnum), (object) name))
        return (IEnumLite) null;
      IEnumLite enumLite;
      if (!this.items.TryGetValue((int) Enum.Parse(typeof (TEnum), name, false), out enumLite))
        return (IEnumLite) null;
      return enumLite;
    }

    public bool IsValidValue(IEnumLite value)
    {
      return this.items.ContainsKey(value.Number);
    }

    private struct EnumValue : IEnumLite
    {
      private readonly TEnum value;

      int IEnumLite.Number
      {
        get
        {
          return Convert.ToInt32((object) this.value);
        }
      }

      string IEnumLite.Name
      {
        get
        {
          return this.value.ToString();
        }
      }

      public EnumValue(TEnum value)
      {
        this.value = value;
      }
    }
  }
}
