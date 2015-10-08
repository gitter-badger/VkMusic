using System.Collections.Generic;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VkMusic.UnitTestProject.DAL
{
    [TestClass]
    public class ListExtensionsTests
    {     

        [TestMethod]
        public void ListExtensionShuffle_CommonList_EndWithNoExceptions_Test()
        {
            var list = new List<string> {"4","2","7","5","1"};

            list.Shuffle();

            Assert.IsNotNull(list);
            Assert.AreEqual(5,list.Count);
        }

        [TestMethod]
        public void ListExtensionSwap_Change2Values_SuccessfullyChanged_Test()
        {
            var list = new List<string> { "4", "2" };

            list.Swap(0,1);

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("2", list[0]);
            Assert.AreEqual("4", list[1]);
        }


    }
}
