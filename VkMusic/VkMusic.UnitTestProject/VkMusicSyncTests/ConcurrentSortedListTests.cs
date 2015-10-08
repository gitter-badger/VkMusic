using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkMusicSync;

namespace VkMusic.UnitTestProject.VkMusicSyncTests
{
    [TestClass]
    public class ConcurrentSortedListTests
    {
        [TestMethod]
        public void ConcurrentSortedListAdd_InParallel_CorreclyAddedItems_Test()
        {
            var csl = new ConcurrentSortedList<int, string>();
            var list = new List<string>
            {
                "4",
                "2",
                "7",
                "5",
                "1"
            };

            Parallel.ForEach(list, item => csl.Add(int.Parse(item), item));

            Assert.AreEqual(5, csl.Count);
            Assert.AreEqual("1",csl.Values[0]);
            Assert.AreEqual("2", csl.Values[1]);
            Assert.AreEqual("4", csl.Values[2]);
            Assert.AreEqual("5", csl.Values[3]);
            Assert.AreEqual("7", csl.Values[4]);
        }

        [TestMethod]
        public void ConcurrentSortedListCount_InParallel_CorreclyGetCount_Test()
        {
            var csl = new ConcurrentSortedList<int, string>();
            var list = new List<string>
            {
                "4",
                "2",
                "7",
                "5",
                "1"
            };

            int count = 0;

            Parallel.ForEach(list, item => csl.Add(int.Parse(item), item));

            Parallel.ForEach(csl.Values, i =>
            {
                count = csl.Count;
            });

            Assert.AreEqual(5,count);
        }
    }
}
