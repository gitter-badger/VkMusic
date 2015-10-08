using System;
using System.Collections.Generic;
using DAL;
using DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkMusicSync;

namespace VkMusic.UnitTestProject.VkMusicSyncTests
{
    [TestClass]
    public class TrackListTests
    {
        private IList<Track> testTracks;

        [TestInitialize]
        public void Initialize()
        {
            testTracks = new List<Track>
            {
                new Track
                {
                    CreatedDateTime = DateTime.MinValue,
                    Artist = "AC/DC",
                    Duration = 60000,
                    Id = 100,
                    OwnerId = 5,
                    Title = "Highway to Hell"
                },
                new Track
                {
                    CreatedDateTime = DateTime.MinValue,
                    Artist = "AC/DC",
                    Duration = 60000,
                    Id = 101,
                    OwnerId = 5,
                    Title = "Back in Black"
                },
                new Track
                {
                    CreatedDateTime = DateTime.MinValue,
                    Artist = "Scropions",
                    Duration = 60000,
                    Id = 102,
                    OwnerId = 5,
                    Title = "Still loving you"
                },
                new Track
                {
                    CreatedDateTime = DateTime.MinValue,
                    Artist = "Billy Talent",
                    Duration = 60000,
                    Id = 103,
                    OwnerId = 5,
                    Title = "Standing in the rain"
                },
                new Track
                {
                    CreatedDateTime = DateTime.MinValue,
                    Artist = "Nightwish",
                    Duration = 60000,
                    Id = 104,
                    OwnerId = 5,
                    Title = "Wishmaster"
                }
            };
        }

        [TestMethod]
        public void TrackListGetList_CommonList_SuccessfullReturn()
        {
            /*
            using (ShimsContext.Create())
            {
                ConfigureFakes();

                ITrackList tl = new TrackList(new MusicLoader("", "", 0));

                var result = tl.GetList();

                Assert.IsNotNull(testTracks);
                Assert.IsNotNull(result);
                Assert.AreNotSame(testTracks, result);
                Assert.AreEqual(testTracks.Count,result.Count);

                var len = testTracks.Count;
                for (int i = 0; i< len; i++)
                {
                    Assert.IsTrue(testTracks[i].Equals(result[i]));
                }
            }
            */
            
        }

        [TestMethod]
        public void TrackListGetList_RandomEnabled_NoDataLoss()
        {
            /*
            using (ShimsContext.Create())
            {
                ConfigureFakes();

                ITrackList tl = new TrackList(new MusicLoader("", "", 0));
                tl.IsRandomPlay = true;

                var result = tl.GetList();

                Assert.IsNotNull(testTracks);
                Assert.IsNotNull(result);
                Assert.AreNotSame(testTracks, result);
                Assert.AreEqual(testTracks.Count, result.Count);

                var len = testTracks.Count;
                for (int i = 0; i < len; i++)
                {
                    Assert.IsTrue(testTracks[i].Equals(result[i]));
                }
            }
            */

        }
        /*
        private void ConfigureFakes()
        {
            VkMusicSync.Fakes.ShimMusicLoader.ConstructorStringStringInt64 =
                (loader, s, arg3, arg4)
                    => { };

            VkMusicSync.Fakes.ShimMusicLoader.AllInstances.GetAllTracks =
                loader
                    => testTracks.Clone();
        }
        */
    }
}
