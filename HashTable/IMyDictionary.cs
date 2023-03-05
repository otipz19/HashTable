namespace MyHashTables
{
    public interface IKeyValuePair<K, V>
    {
        public K Key { get; }
        public V Value { get; set; }
    }

    public interface IMyDictionary<K, V>
    {
        public int Count { get; }
        public bool ContainsKey(K key);
        public bool ContainsValue(V value);
        public bool Add(K key, V value);
        public bool Remove(K key);
        public V this[K key] { get; set; }
    }
}
