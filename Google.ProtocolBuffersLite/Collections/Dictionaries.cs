// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.Collections.Dictionaries
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System.Collections;
using System.Collections.Generic;

namespace Google.ProtocolBuffers.Collections
{
  public static class Dictionaries
  {
    public static bool Equals<TKey, TValue>(IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
    {
      if (left.Count != right.Count)
        return false;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) left)
      {
        TValue obj;
        if (!right.TryGetValue(keyValuePair.Key, out obj))
          return false;
        IEnumerable left1 = (object) keyValuePair.Value as IEnumerable;
        IEnumerable right1 = (object) obj as IEnumerable;
        if (left1 == null || right1 == null)
        {
          if (!object.Equals((object) keyValuePair.Value, (object) obj))
            return false;
        }
        else if (!Enumerables.Equals(left1, right1))
          return false;
      }
      return true;
    }

    public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    {
      if (!dictionary.IsReadOnly)
        return (IDictionary<TKey, TValue>) new ReadOnlyDictionary<TKey, TValue>(dictionary);
      return dictionary;
    }

    public static int GetHashCode<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    {
      int num1 = 31;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) dictionary)
      {
        int num2 = keyValuePair.Key.GetHashCode() ^ Dictionaries.GetDeepHashCode((object) keyValuePair.Value);
        num1 ^= num2;
      }
      return num1;
    }

    private static int GetDeepHashCode(object value)
    {
      IEnumerable enumerable = value as IEnumerable;
      if (enumerable == null)
        return value.GetHashCode();
      int num = 29;
      foreach (object obj in enumerable)
        num = num * 37 + obj.GetHashCode();
      return num;
    }
  }
}
