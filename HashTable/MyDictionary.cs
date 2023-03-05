using System;
using System.Collections;

namespace MyHashTables
{
    public class KeyValuePair<K, V> : IKeyValuePair<K, V>
    {
        public K Key { get; private set; }
        public V Value { get; set; }

        public KeyValuePair(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }

    public class MyDictionary<K, V> : IMyDictionary<K, V>, IEnumerable<IKeyValuePair<K, V>>
    {
        private LinkedList<KeyValuePair<K, V>>[] nodes;
        private const int loadFactor = 2;
        public int Count { get; private set; }

        public MyDictionary(int startSize = 11)
        {
            nodes = new LinkedList<KeyValuePair<K, V>>[startSize];
        }

        public MyDictionary((K key, V value)[] pairs) : this(pairs.Length * 2)
        {
            foreach (var pair in pairs)
                Add(pair.key, pair.value);
        }

        private int GetShortHash(K key)
        {
            if (key == null)
                throw new ArgumentNullException();
            return key.GetHashCode() % nodes.Length;
        }

        private bool TryGetPair(K key, out KeyValuePair<K, V> outPair)
        {
            int shortHash = GetShortHash(key);
            if (nodes[shortHash] != null)
            {
                foreach (var pair in nodes[shortHash])
                    if (pair.Key.Equals(key))
                    {
                        outPair = pair;
                        return true;
                    }
            }
            outPair = null;
            return false;
        }

        private void Rehash()
        {
            var tempArr = new KeyValuePair<K, V>[Count];
            int indexTempArr = 0;
            foreach(var pair in this)
                tempArr[indexTempArr++] = (KeyValuePair<K, V>)pair;
            nodes = new LinkedList<KeyValuePair<K, V>>[nodes.Length * 2];
            Count = 0;
            foreach (var pair in tempArr)
                Add(pair.Key, pair.Value);
        }

        public bool ContainsKey(K key)
        {
            int shortHash = GetShortHash(key);
            if (nodes[shortHash] != null)
            {
                foreach (var pair in nodes[shortHash])
                    if (pair.Key.Equals(key))
                        return true;
            }
            return false;
        }

        public bool ContainsValue(V value)
        {
            if (value != null)
                foreach (var pair in this)
                    if (pair.Value.Equals(value))
                        return true;
            return false;
        }

        /// <returns>False if key is already present</returns>
        public bool Add(K key, V value)
        {
            if (Count / nodes.Length >= loadFactor)
                Rehash();
            int shortHash = GetShortHash(key);
            if (nodes[shortHash] == null)
            {
                nodes[shortHash] = new LinkedList<KeyValuePair<K, V>>();
                nodes[shortHash].AddLast(new KeyValuePair<K, V>(key, value));
            }
            else
            {
                foreach (var pair in nodes[shortHash])
                {
                    if (pair.Key.Equals(key))
                        return false;
                }
                nodes[shortHash].AddLast(new KeyValuePair<K, V>(key, value));
            }
            Count++;
            return true;
        }

        /// <returns>False if key is not present</returns>
        public bool Remove(K key)
        {
            int shortHash = GetShortHash(key);
            if(TryGetPair(key, out KeyValuePair<K, V> pair))
            {
                nodes[shortHash].Remove(pair);
                Count--;
                return true;
            }
            return false;
        }

        public IEnumerator<IKeyValuePair<K, V>> GetEnumerator()
        {
            foreach(var node in nodes)
            {
                if(node != null)
                {
                    foreach (var pair in node)
                        yield return pair;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public V this[K key]
        {
            get
            {
                if (TryGetPair(key, out KeyValuePair<K, V> pair))
                    return pair.Value;
                throw new ArgumentException("Key is not present in dictionary");
            }
            set
            {
                if (TryGetPair(key, out KeyValuePair<K, V> pair))
                    pair.Value = value;
                else
                    Add(key, value);
            }
        }
    }
}
