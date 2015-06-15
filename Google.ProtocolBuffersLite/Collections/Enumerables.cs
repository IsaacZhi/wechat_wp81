// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.Collections.Enumerables
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.Collections;

namespace Google.ProtocolBuffers.Collections
{
  public static class Enumerables
  {
    public static bool Equals(IEnumerable left, IEnumerable right)
    {
      IEnumerator enumerator = left.GetEnumerator();
      try
      {
        foreach (object objB in right)
        {
          if (!enumerator.MoveNext() || !object.Equals(enumerator.Current, objB))
            return false;
        }
        if (enumerator.MoveNext())
          return false;
      }
      finally
      {
        IDisposable disposable = enumerator as IDisposable;
        if (disposable != null)
          disposable.Dispose();
      }
      return true;
    }
  }
}
