// Decompiled with JetBrains decompiler
// Type: Google.ProtocolBuffers.Collections.PopsicleList`1
// Assembly: Google.ProtocolBuffersLite, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589
// MVID: 41252B53-436F-44EC-87E1-8005252CEB74
// Assembly location: D:\wechat\WinPhone_SDK\SDK开发包_For_Windows Phone8\Google.ProtocolBuffersLite.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Google.ProtocolBuffers.Collections
{
  public sealed class PopsicleList<T> : IPopsicleList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ICastArray
  {
    private static readonly bool CheckForNull = (object) default (T) == null;
    private static readonly T[] EmptySet = new T[0];
    private List<T> items;
    private bool readOnly;

    public T this[int index]
    {
      get
      {
        if (this.items == null)
          throw new ArgumentOutOfRangeException();
        return this.items[index];
      }
      set
      {
        this.ValidateModification();
        if (PopsicleList<T>.CheckForNull)
          Google.ProtocolBuffers.ThrowHelper.ThrowIfNull((object) value);
        this.items[index] = value;
      }
    }

    public int Count
    {
      get
      {
        if (this.items != null)
          return this.items.Count;
        return 0;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return this.readOnly;
      }
    }

    public void MakeReadOnly()
    {
      this.readOnly = true;
    }

    public int IndexOf(T item)
    {
      if (this.items != null)
        return this.items.IndexOf(item);
      return -1;
    }

    public void Insert(int index, T item)
    {
      this.ValidateModification();
      if (PopsicleList<T>.CheckForNull)
        Google.ProtocolBuffers.ThrowHelper.ThrowIfNull((object) item);
      this.items.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      this.ValidateModification();
      this.items.RemoveAt(index);
    }

    public void Add(T item)
    {
      this.ValidateModification();
      if (PopsicleList<T>.CheckForNull)
        Google.ProtocolBuffers.ThrowHelper.ThrowIfNull((object) item);
      this.items.Add(item);
    }

    public void Clear()
    {
      this.ValidateModification();
      this.items.Clear();
    }

    public bool Contains(T item)
    {
      if (this.items != null)
        return this.items.Contains(item);
      return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      if (this.items == null)
        return;
      this.items.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
      this.ValidateModification();
      return this.items.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return ((IEnumerable<T>) this.items ?? (IEnumerable<T>) PopsicleList<T>.EmptySet).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public void Add(IEnumerable<T> collection)
    {
      this.ValidateModification();
      Google.ProtocolBuffers.ThrowHelper.ThrowIfNull((object) collection);
      if (!PopsicleList<T>.CheckForNull || collection is PopsicleList<T>)
        this.items.AddRange(collection);
      else if (collection is ICollection<T>)
      {
        Google.ProtocolBuffers.ThrowHelper.ThrowIfAnyNull<T>(collection);
        this.items.AddRange(collection);
      }
      else
      {
        foreach (T obj in collection)
        {
          Google.ProtocolBuffers.ThrowHelper.ThrowIfNull((object) obj);
          this.items.Add(obj);
        }
      }
    }

    private void ValidateModification()
    {
      if (this.readOnly)
        throw new NotSupportedException("List is read-only");
      if (this.items != null)
        return;
      this.items = new List<T>();
    }

    IEnumerable<TItemType> ICastArray.CastArray<TItemType>()
    {
      /*if (this.items == null)
        return (IEnumerable<TItemType>) PopsicleList<TItemType>.EmptySet;
      return (IEnumerable<TItemType>) this.items.ToArray();*/

      if (items == null)
      {
          return PopsicleList<TItemType>.EmptySet;
      }
      return (TItemType[])(object)items.ToArray();
    }
  }
}
