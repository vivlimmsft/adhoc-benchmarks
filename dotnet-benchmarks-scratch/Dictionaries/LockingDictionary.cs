using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_benchmarks_scratch.Dictionaries;

public class LockingDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> dictionary = new();
    private readonly object dictionaryLock = new();

    public TValue this[TKey key]
    {
        get
        {
            lock (this.dictionaryLock)
            {
                return this.dictionary[key];
            }
        }

        set
        {
            lock (this.dictionaryLock)
            {
                this.dictionary[key] = value;
            }
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            lock (this.dictionaryLock)
            {
                return this.dictionary.Keys;
            }
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            lock (this.dictionaryLock)
            {
                return this.dictionary.Values;
            }
        }
    }

    public int Count
    {
        get
        {
            lock (this.dictionaryLock)
            {
                return this.dictionary.Count;
            }
        }
    }

    public bool IsReadOnly
    {
        get
        {
            lock (this.dictionaryLock)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).IsReadOnly;
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        lock (this.dictionaryLock)
        {
            this.dictionary.Add(key, value);
        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        lock (this.dictionaryLock)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).Add(item);
        }
    }

    public void Clear()
    {
        lock (this.dictionaryLock)
        {
            this.dictionary.Clear();
        }
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        lock (this.dictionaryLock)
        {
            return this.dictionary.Contains(item);
        }
    }

    public bool ContainsKey(TKey key)
    {
        lock (this.dictionaryLock)
        {
            return this.dictionary.ContainsKey(key);
        }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        lock (this.dictionaryLock)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).CopyTo(array, arrayIndex);
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        throw new InvalidOperationException("Can't directly enumerate a LockingDictionary");
    }

    public bool Remove(TKey key)
    {
        lock (this.dictionaryLock)
        {
            return this.dictionary.Remove(key);
        }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        lock (this.dictionaryLock)
        {
            return this.dictionary.Remove(item.Key);
        }
    }

    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue value)
    {
        lock (this.dictionaryLock)
        {
            return this.dictionary.TryGetValue(key, out value!);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new InvalidOperationException("Can't directly enumerate a LockingDictionary");
    }
}
