using MyHashTables;

namespace MyHashTablesTests
{
    [TestClass]
    public class MyOptimizedHashSetTests
    {
        private int[] contained = new int[] { 1, 2, 42, 1575, 34212 };
        private int[] notContained = new int[] { 9999, 43532, 843454, 232323 };

        [TestMethod]
        public void ContainsValues()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            for (int i = 0; i < contained.Length; i++)
            {
                Assert.IsTrue(set.Contains(contained[i]));
            }
        }

        [TestMethod]
        public void NotContainsValues()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            for (int i = 0; i < notContained.Length; i++)
            {
                Assert.IsFalse(set.Contains(notContained[i]));
            }
        }

        [TestMethod]
        public void AddAlreadyContained()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            Assert.IsFalse(set.Add(contained[0]));
        }

        [TestMethod]
        public void RemoveNotContained()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            Assert.IsFalse(set.Remove(notContained[0]));
        }

        [TestMethod]
        public void CountSimpleCheck()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            Assert.AreEqual(contained.Length, set.Count);
        }

        [TestMethod]
        public void CountAfterRemove()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            set.Remove(contained[0]);
            Assert.AreEqual(contained.Length - 1, set.Count);
            set.Remove(contained[1]);
            Assert.AreEqual(contained.Length - 2, set.Count);
        }

        [TestMethod]
        public void CountAfterAdd()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            foreach(int n in notContained)
                set.Add(n);
            Assert.AreEqual(contained.Length + notContained.Length, set.Count);
        }

        [TestMethod]
        public void CountAfterAddAndRemove()
        {
            var set = new MyOptimizedHashSet<int>(contained);
            foreach (int n in notContained)
                set.Add(n);
            set.Remove(notContained[0]);
            Assert.AreEqual(contained.Length + notContained.Length - 1, set.Count);
        }
    }
}