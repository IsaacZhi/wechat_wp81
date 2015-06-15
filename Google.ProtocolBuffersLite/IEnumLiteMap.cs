// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.IEnumLiteMap
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

namespace Google.ProtocolBuffers
{
    public interface IEnumLiteMap<T> : IEnumLiteMap
        where T : IEnumLite
    {
        new T FindValueByNumber(int number);
    }

    public interface IEnumLiteMap
    {
        bool IsValidValue(IEnumLite value);
        IEnumLite FindValueByNumber(int number);
        IEnumLite FindValueByName(string name);
    }
  
  /*public interface IEnumLiteMap
  {
    bool IsValidValue(IEnumLite value);

    IEnumLite FindValueByNumber(int number);

    IEnumLite FindValueByName(string name);
  }
  
  public interface IEnumLiteMap<T> : IEnumLiteMap where T : IEnumLite
  {
    T FindValueByNumber(int number);
  }*/
}
