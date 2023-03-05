using System;
using System.Collections;

namespace MyHashTables
{
    public struct OptimizedKeyValuePair<K, V> : IKeyValuePair<K, V>
    {
        public K Key { get; set; }

        public V Value { get; set; }

        public bool IsEmpty { get; set; }

        public int Next { get; set; }
    }

    public class MyOptimizedDictionary<K, V> : IMyDictionary<K, V>, IEnumerable<IKeyValuePair<K, V>>
    {
        private int[] buckets;
        private OptimizedKeyValuePair<K, V>[] entries;
        private int freeList = -1;
        private int lastEntrieIndex = 0;

        private int[] bucketSizes =
        {
            11, 23, 47, 97, 197, 397, 797, 1597, 3203, 6421, 12853, 25717,
            51437, 102877, 205759, 411527, 823117, 1646237, 3292489, 6584983,
            13169977, 26339969, 52679969, 105359939, 210719881, 421439783,
            842879579, 1685759167
        };
        private int curBucketSizeIndex = 0;

        public MyOptimizedDictionary()
        {
            int size = bucketSizes[curBucketSizeIndex];
            buckets = new int[size];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = -1;
            }
            entries = new OptimizedKeyValuePair<K, V>[size];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].Next = -1;
            }
        }
        public MyOptimizedDictionary((K key, V value)[] pairs) : this()
        {
            foreach (var pair in pairs)
                Add(pair.key, pair.value);
        }

        private int GetBucketIndex(K key)
        {
            if (key == null)
                throw new ArgumentNullException();
            return key.GetHashCode() % buckets.Length;
        }

        private void Rehash()
        {
            if (curBucketSizeIndex == bucketSizes.Length)
                return;
            int newSize = bucketSizes[++curBucketSizeIndex];
            var tempArr = new OptimizedKeyValuePair<K, V>[entries.Length];
            for (int i = 0; i < entries.Length; i++)
            {
                tempArr[i] = entries[i];
            }
            entries = new OptimizedKeyValuePair<K, V>[newSize];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].Next = -1;
            }
            Count = 0;
            lastEntrieIndex = 0;
            freeList = -1;
            buckets = new int[newSize];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = -1;
            }
            foreach (var pair in tempArr)
                Add(pair.Key, pair.Value);
        }

        public V this[K key]
        {
            get
            {
                int bucketIndex = GetBucketIndex(key);
                for (int i = buckets[bucketIndex]; i != -1; i = entries[i].Next)
                {
                    if (entries[i].Key.Equals(key))
                        return entries[i].Value;
                }
                throw new ArgumentException("Key is not present in dictionary");
            }
            set
            {
                int bucketIndex = GetBucketIndex(key);
                for (int i = buckets[bucketIndex]; i != -1; i = entries[i].Next)
                {
                    if (entries[i].Key.Equals(key))
                    {
                        entries[i].Value = value;
                        return;
                    }
                }
                Add(key, value);
            }
        }

        public int Count { get; private set; }

        public bool Add(K key, V value)
        {
            if (Count == entries.Length - 1)
                Rehash();

            int bucketIndex = GetBucketIndex(key);
            for (int i = buckets[bucketIndex]; i != -1; i = entries[i].Next)
            {
                if (entries[i].Key.Equals(key))
                {
                    return false;
                }
            }
            Count++;
            int entrieIndex = freeList >= 0 ? freeList : lastEntrieIndex++;
            entries[entrieIndex].Key = key;
            entries[entrieIndex].Value = value;
            entries[entrieIndex].IsEmpty = false;
            freeList = entries[entrieIndex].Next;
            entries[entrieIndex].Next = buckets[bucketIndex];
            buckets[bucketIndex] = entrieIndex;
            return true;
        }

        public bool ContainsKey(K key)
        {
            int bucketIndex = GetBucketIndex(key);
            for (int i = buckets[bucketIndex]; i != -1; i = entries[i].Next)
                if (entries[i].Key.Equals(key))
                    return true;
            return false;
        }

        public bool ContainsValue(V value)
        {
            foreach (var entrie in this)
                if (entrie.Value.Equals(value))
                    return true;
            return false;
        }

        public bool Remove(K key)
        {
            int bucketIndex = GetBucketIndex(key);
            for (int entrieIndex = buckets[bucketIndex]; entrieIndex != -1; entrieIndex = entries[entrieIndex].Next)
                if (entries[entrieIndex].Key.Equals(key))
                {
                    entries[entrieIndex].Key = default;
                    entries[entrieIndex].Value = default;
                    entries[entrieIndex].IsEmpty = true;
                    buckets[bucketIndex] = entries[entrieIndex].Next;
                    entries[entrieIndex].Next = freeList;
                    freeList = entrieIndex;
                    if (freeList == lastEntrieIndex)
                        lastEntrieIndex--;
                    Count--;
                    return true;
                }
            return false;
        }

        public IEnumerator<IKeyValuePair<K, V>> GetEnumerator()
        {
            int curCount = 0;
            foreach (var entrie in entries)
            {
                if (curCount >= Count)
                    yield break;
                if (!entrie.IsEmpty && entrie.Key != null && entrie.Value != null)
                {
                    curCount++;
                    yield return entrie;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
