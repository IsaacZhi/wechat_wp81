// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.Collections.Lists
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Google.ProtocolBuffers.Collections
{
  public static class Lists
  {
    public static IList<T> AsReadOnly<T>(IList<T> list)
    {
      return Lists<T>.AsReadOnly(list);
    }

    public static bool Equals<T>(IList<T> left, IList<T> right)
    {
      if (left == right)
        return true;
      if (left == null || right == null || left.Count != right.Count)
        return false;
      IEqualityComparer<T> equalityComparer = (IEqualityComparer<T>) EqualityComparer<T>.Default;
      for (int index = 0; index < left.Count; ++index)
      {
        if (!equalityComparer.Equals(left[index], right[index]))
          return false;
      }
      return true;
    }

    public static int GetHashCode<T>(IList<T> list)
    {
      int num = 31;
      foreach (T obj in (IEnumerable<T>) list)
        num = num * 29 + obj.GetHashCode();
      return num;
    }
  }
  
  public static class Lists<T>
  {
    private static readonly ReadOnlyCollection<T> empty = new ReadOnlyCollection<T>(new T[0]);

    public static ReadOnlyCollection<T> Empty
    {
      get
      {
        return empty;
      }
    }

    public static IList<T> AsReadOnly(IList<T> list)
    {
      if (!list.IsReadOnly)
        return new ReadOnlyCollection<T>(list);
      return list;
    }
  }
}
