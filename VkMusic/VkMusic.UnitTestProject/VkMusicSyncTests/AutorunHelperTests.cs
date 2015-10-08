using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkMusicSync;

namespace VkMusic.UnitTestProject.VkMusicSyncTests
{
    [TestClass]
    public class AutorunHelperTests
    {
        private AutorunHelper _helper;
        [TestInitialize]
        public void Initialize()
        {
            // Unregister autorun 
            _helper = new AutorunHelper("test1");

            if(_helper.IsAutorunRegistered)
                _helper.RegisterUnregisterAutorun();
        }

        [TestMethod]
        public void AutorunHelperRegisterUnregister_CommonConditions_SuccessfullyRegisterAndUnregister_Test()
        {
            _helper.RegisterUnregisterAutorun();

            Assert.IsTrue(_helper.IsAutorunRegistered);

            _helper.RegisterUnregisterAutorun();

            Assert.IsFalse(_helper.IsAutorunRegistered);
        }
    }
}
