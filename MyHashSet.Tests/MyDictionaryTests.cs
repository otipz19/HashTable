using MyHashTables;
using System.Reflection;

namespace MyHashTablesTests
{
    [TestClass]
    public class MyDictionaryTests
    {
        private (int key, char value)[] pairs;
        private IMyDictionary<int, char> dict;
        private IMyDictionary<int, char> otherDict;

        [TestInitialize]
        public void Initialize()
        {
            pairs = new (int key, char value)['z' - 'a' + 1];
            for (int i = 0; i < pairs.Length; i++)
            {
                pairs[i] = (i, (char)('a' + (char)i));
            }
            dict = new MyDictionary<int, char>(pairs);
            otherDict = new MyDictionary<int, char>();
        }

        [TestMethod]
        public void CheckContainsAllKeys()
        {
            foreach (var pair in pairs)
                Assert.IsTrue(dict.ContainsKey(pair.key));
        }

        [TestMethod]
        public void CheckContainsAllValues()
        {
            foreach (var pair in pairs)
                Assert.IsTrue(dict.ContainsValue(pair.value));
        }

        [TestMethod]
        public void AddNewPair()
        {
            Assert.IsTrue(otherDict.Add(pairs[0].key, pairs[0].value));
            Assert.IsTrue(otherDict.ContainsKey(pairs[0].key));
            Assert.IsTrue(otherDict.ContainsValue(pairs[0].value));
        }

        [TestMethod]
        public void AddAlreadyContainedPair()
        {
            Assert.IsTrue(otherDict.Add(pairs[0].key, pairs[0].value));
            Assert.IsFalse(otherDict.Add(pairs[0].key, pairs[0].value));
            Assert.AreEqual(1, otherDict.Count);
            char[] chars = new char[] { 'a' };
            int i = 0;
            foreach(var pair in (MyDictionary<int, char>)otherDict)
            {
                Assert.AreEqual(chars[i++], pair.Value);
            }
        }

        [TestMethod]
        public void RemovePair()
        {
            otherDict.Add(pairs[0].key, pairs[0].value);
            Assert.IsTrue(otherDict.Remove(pairs[0].key));
            Assert.AreEqual(0, otherDict.Count);
        }

        [TestMethod]
        public void RemoveNotContainedPair()
        {
            otherDict.Add(pairs[0].key, pairs[0].value);
            Assert.IsFalse(otherDict.Remove(pairs[1].key));
            Assert.AreEqual(1, otherDict.Count);
        }

        [TestMethod]
        public void RemoveFromEmptyDictionary()
        {
            Assert.IsFalse(otherDict.Remove(pairs[0].key));
        }

        [TestMethod]
        public void GetByIndexer()
        {
            Assert.AreEqual('a', dict[0]);
        }

        [TestMethod]
        public void GetByIndexerException()
        {
            Assert.ThrowsException<ArgumentException>(() => otherDict[0]);
            Assert.ThrowsException<ArgumentException>(() => dict[248]);
        }

        [TestMethod]
        public void SetByIndexer()
        {
            otherDict[1456] = 'a';
            Assert.AreEqual('a', otherDict[1456]);
            Assert.AreEqual(1, otherDict.Count);
        }

        [TestMethod]
        public void GetAfterRehash()
        {
            var intDict = new MyDictionary<int, int>();
            int[] nums = new int[10000000];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = i;
                Assert.IsTrue(intDict.Add(i, i));
            }
            foreach (int n in nums)
                Assert.AreEqual(n, intDict[n]);
        }
    }
}
