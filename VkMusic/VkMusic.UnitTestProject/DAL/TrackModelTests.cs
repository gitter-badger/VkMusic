using System.Collections.Generic;
using DAL;
using DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VkMusic.UnitTestProject.DAL
{
    [TestClass]
    public class TrackModelTests
    {     

        [TestMethod]
        public void TrackEqual_EqualTracks_ReturnTrue_Test()
        {
            Track track1 = new Track {Id = 1};
            Track track2 = new Track { Id = 1 };

            var result = track1.Equals(track2);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void TrackEqual_UnequalTracks_ReturnFalse_Test()
        {
            Track track1 = new Track { Id = 1 };
            Track track2 = new Track { Id = 2 };

            var result = track1.Equals(track2);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void TrackEqual_NullOtherTrack_ReturnFalse_Test()
        {
            Track track1 = new Track { Id = 1 };

            var result = track1.Equals(null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);

        }
    }
}
