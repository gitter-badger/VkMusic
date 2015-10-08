using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DAL.Model;
using DAL.Repository.XMLRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VkMusic.UnitTestProject.DAL
{
    [TestClass]
    public class RepositoryTests
    {
        private IList<Track> _testList;

        [TestInitialize]
        public void Initialize()
        {
            _testList = new List<Track>
            {
                new Track { CreatedDateTime = DateTime.MinValue, Artist = "AC/DC", Duration = 60000, Id = 100, OwnerId = 5, Title = "Highway to Hell"}
            };
        }

        [TestMethod]
        public void TrackRepositorySaveTracks_CorrectPath_SuccessfullyEnd_Test()
        {
            var repository = new TrackRepository("trackrepo");

            repository.SaveTracks(_testList);


        }

        [TestMethod]
        public void TrackRepositoryGetTracks_CorrectPath_ReturnCorrectlySavedData_Test()
        {
            var repository = new TrackRepository("trackrepo");

            var result = repository.GetTracks();

            Assert.IsNotNull(result);
            Assert.AreEqual(_testList.Count, result.Count);

            for (int i = 0; i < _testList.Count; i++)
            {
                Assert.AreEqual(_testList[i].CreatedDateTime, result[i].CreatedDateTime);
                Assert.AreEqual(_testList[i].Artist, result[i].Artist);
                Assert.AreEqual(_testList[i].Duration, result[i].Duration);
                Assert.AreEqual(_testList[i].Id, result[i].Id);
                Assert.AreEqual(_testList[i].OwnerId, result[i].OwnerId);
                Assert.AreEqual(_testList[i].Title, result[i].Title);
            }
            
        }

        [TestMethod]
        public void TrackRepositoryGetTracks_PathWithNoData_ReturnZeroLengthList_Test()
        {
            var repository = new TrackRepository("unidentified");

            var result = repository.GetTracks();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsTrue(Directory.Exists("unidentified"));

            Directory.Delete("unidentified", true);

        }

    }
}
