using MyHashTables;

namespace MyHashTablesTests
{
    [TestClass]
    public class NetDictionaryTests
    {
        [TestMethod]
        public void GetAfterRehash()
        {
            var intDict = new System.Collections.Generic.Dictionary<int, int>();
            int[] nums = new int[10000000];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = i;
                intDict.Add(i, i);
            }
            foreach (int n in nums)
                Assert.AreEqual(n, intDict[n]);
        }
    }
}
