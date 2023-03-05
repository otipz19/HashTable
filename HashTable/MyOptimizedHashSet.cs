using System;
using System.Text;
using System.Text.Json;

namespace MyHashTables
{
    public class MyOptimizedHashSet<T>
    {
        internal struct Entrie<T>
        {
            public T Value { get; set; }
            public int Next { get; set; }
        }

        private int[] buckets;
        private Entrie<T>[] entries;
        private int freeList = -1;
        private int lastEntrieIndex;
        private int itemsCount;
        public int Count => itemsCount;
        
        public MyOptimizedHashSet(int startCapacity = 11)
        {
            buckets = new int[startCapacity];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = -1;
            }
            entries = new Entrie<T>[startCapacity];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].Next = -1;
            }
        }

        public MyOptimizedHashSet(T[] values) : this(values.Length * 2)
        {
            foreach (T value in values)
                Add(value);
        }

        private int GetBucketIndex(T value)
        {
            if (value == null)
                throw new ArgumentNullException();
            int shortHash = value.GetHashCode() % buckets.Length;
            return shortHash;
        }

        public bool Contains(T value)
        {
            int bucketIndex = GetBucketIndex(value);
            for (int i = buckets[bucketIndex]; i != -1; i = entries[i].Next)
                if (entries[i].Value.Equals(value))
                    return true;
            return false;
        }

        /// <returns>False if there is already value in collection</returns>
        public bool Add(T value)
        {
            int bucketIndex = GetBucketIndex(value);
            for (int i = buckets[bucketIndex]; i != -1; i = entries[i].Next)
                if (entries[i].Value.Equals(value))
                    return false;
            if (itemsCount++ == entries.Length)
                ResizeEntriesArray();
            int entrieIndex = freeList >= 0 ? freeList : lastEntrieIndex++;
            entries[entrieIndex].Value = value;
            freeList = entries[entrieIndex].Next;
            entries[entrieIndex].Next = buckets[bucketIndex];
            buckets[bucketIndex] = entrieIndex;
            return true;
        }

        private void ResizeEntriesArray()
        {
            Entrie<T>[] resizedItems = new Entrie<T>[entries.Length * 2];
            for (int i = 0; i < entries.Length; i++)
            {
                resizedItems[i] = entries[i];
            }
            entries = resizedItems;
        }

        private void Rehash()
        {

        }

        /// <returns>False if there is no value in collection</returns>
        public bool Remove(T value)
        {
            int bucketIndex = GetBucketIndex(value);
            for (int entrieIndex = buckets[bucketIndex]; entrieIndex != -1; entrieIndex = entries[entrieIndex].Next)
                if (entries[entrieIndex].Value.Equals(value))
                {
                    entries[entrieIndex].Value = default;
                    buckets[bucketIndex] = entries[entrieIndex].Next;
                    entries[entrieIndex].Next = freeList;
                    freeList = entrieIndex;
                    if (freeList == lastEntrieIndex)
                        lastEntrieIndex--;
                    itemsCount--;
                    return true;
                }
            return false;
        }
    }
}
