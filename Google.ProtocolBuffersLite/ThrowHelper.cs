// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.ThrowHelper
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.Collections.Generic;

namespace Google.ProtocolBuffers
{
  public static class ThrowHelper
  {
    public static void ThrowIfNull(object value, string name)
    {
      if (value == null)
        throw new ArgumentNullException(name);
    }

    public static void ThrowIfNull(object value)
    {
      if (value == null)
        throw new ArgumentNullException();
    }

    public static void ThrowIfAnyNull<T>(IEnumerable<T> sequence)
    {
      foreach (T obj in sequence)
      {
        if ((object) obj == null)
          throw new ArgumentNullException();
      }
    }

    public static Exception CreateMissingMethod(Type type, string methodName)
    {
      //return (Exception) new MissingMethodException(string.Format("The method '{0}' was not found on type {1}", (object) methodName, (object) type));
        return new System.ArgumentException(String.Format("The method '{0}' was not found on type {1}.", methodName, type));
    }
  }
}
