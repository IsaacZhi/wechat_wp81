// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.SortedList`2
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Google.ProtocolBuffers
{
  internal sealed class SortedList<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
  {
    private readonly IDictionary<TKey, TValue> wrapped = (IDictionary<TKey, TValue>) new Dictionary<TKey, TValue>();

    public ICollection<TKey> Keys
    {
      get
      {
        List<TKey> list = new List<TKey>(this.wrapped.Count);
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
          list.Add(keyValuePair.Key);
        return (ICollection<TKey>) list;
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        List<TValue> list = new List<TValue>(this.wrapped.Count);
        foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
          list.Add(keyValuePair.Value);
        return (ICollection<TValue>) list;
      }
    }

    public TValue this[TKey key]
    {
      get
      {
        return this.wrapped[key];
      }
      set
      {
        this.wrapped[key] = value;
      }
    }

    public int Count
    {
      get
      {
        return this.wrapped.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return this.wrapped.IsReadOnly;
      }
    }

    public SortedList()
    {
    }

    public SortedList(IDictionary<TKey, TValue> dictionary)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) dictionary)
        this.Add(keyValuePair.Key, keyValuePair.Value);
    }

    public void Add(TKey key, TValue value)
    {
      this.wrapped.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
      return this.wrapped.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
      return this.wrapped.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      return this.wrapped.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      this.wrapped.Add(item);
    }

    public void Clear()
    {
      this.wrapped.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      return this.wrapped.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      this.wrapped.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      return this.wrapped.Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      IComparer<TKey> comparer = (IComparer<TKey>) Comparer<TKey>.Default;
      List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>((IEnumerable<KeyValuePair<TKey, TValue>>) this.wrapped);
      list.Sort((Comparison<KeyValuePair<TKey, TValue>>) ((x, y) => comparer.Compare(x.Key, y.Key)));
      return (IEnumerator<KeyValuePair<TKey, TValue>>) list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
