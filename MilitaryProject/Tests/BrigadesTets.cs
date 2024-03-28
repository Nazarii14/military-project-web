using Azure;
using MilitaryProject.BLL.Services;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Helpers;
using MilitaryProject.Domain.ViewModels.Brigade;
using MilitaryProject.Domain.ViewModels.User;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class BrigadeTest
    {
        [TestMethod]
        public async Task GetBrigadesTest()
        {
            var brigade1 = new Brigade
            {
                ID = 1,
                Name = "test1",
                CommanderName = "test1",
                EstablishmentDate = DateTime.Now,
                Location = "test1"
            };

            var brigade2 = new Brigade
            {
                ID = 2,
                Name = "test2",
                CommanderName = "test2",
                EstablishmentDate = DateTime.Now,
                Location = "test2"
            };

            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { brigade1, brigade2 });          

            var response = await brigadeService.GetBrigades();

            Assert.AreEqual(StatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Data);
            Assert.IsTrue(response.Data.Count == 2);
        }

        [TestMethod]
        public async Task GetBrigadeTest()
        {
            var brigade1 = new Brigade
            {
                ID = 1,
                Name = "test1",
                CommanderName = "test1",
                EstablishmentDate = DateTime.Now,
                Location = "test1"
            };

            var brigade2 = new Brigade
            {
                ID = 2,
                Name = "test2",
                CommanderName = "test2",
                EstablishmentDate = DateTime.Now,
                Location = "test2"
            };

            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { brigade1, brigade2 });

            var response1 = await brigadeService.GetBrigade(1);
            var response2 = await brigadeService.GetBrigade(2);

            Assert.AreEqual(StatusCode.OK, response1.StatusCode);
            Assert.IsNotNull(response1.Data);
            Assert.AreEqual(response1.Data.Name, "test1");

            Assert.AreEqual(StatusCode.OK, response2.StatusCode);
            Assert.IsNotNull(response2.Data);
            Assert.AreEqual(response2.Data.Name, "test2");
        }

        [TestMethod]
        public async Task GetBrigadeTest_NonExistingBrigade()
        {
            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { });

            var response = await brigadeService.GetBrigade(1);

            Assert.AreEqual(response.Description, "Brigade does not exist");
        }

        [TestMethod]
        public async Task CreateBrigadeTest()
        {
            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { });

            var brigade = new BrigadeViewModel
            {
                ID = 1,
                Name = "test",
                CommanderName = "test",
                EstablishmentDate = DateTime.Now,
                Location = "test"
            };

            var response = await brigadeService.CreateBrigade(brigade);

            Assert.AreEqual(StatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(response.Data.Name, "test");
        }

        [TestMethod]
        public async Task CreateBrigadeTest_Existing()
        {
            var brigade1 = new Brigade
            {
                ID = 1,
                Name = "test",
                CommanderName = "test",
                EstablishmentDate = DateTime.Now,
                Location = "test"
            };

            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { brigade1 });

            var brigade = new BrigadeViewModel
            {
                ID = 1,
                Name = "test",
                CommanderName = "test",
                EstablishmentDate = DateTime.Now,
                Location = "test"
            };

            var response = await brigadeService.CreateBrigade(brigade);

            Assert.AreEqual(response.Description, "Brigade is already exist");
        }

        [TestMethod]
        public async Task UpdateBrigadeTest()
        {
            var brigade1 = new Brigade
            {
                ID = 1,
                Name = "test1",
                CommanderName = "test1",
                EstablishmentDate = DateTime.Now,
                Location = "test1"
            };

            var brigade2 = new Brigade
            {
                ID = 2,
                Name = "test2",
                CommanderName = "test2",
                EstablishmentDate = DateTime.Now,
                Location = "test2"
            };

            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { brigade1, brigade2 });

            var brigade = new BrigadeViewModel
            {
                ID = 1,
                Name = "TEST",
                CommanderName = "TEST",
                EstablishmentDate = DateTime.Now,
                Location = "TEST"
            };

            var response = await brigadeService.UpdateBrigade(brigade);

            Assert.AreEqual(StatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(response.Data.Name, "TEST");
        }

        [TestMethod]
        public async Task UpdateBrigadeTest_NonExistingBrigade()
        {
            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { });

            var brigade = new BrigadeViewModel
            {
                ID = 1,
                Name = "TEST",
                CommanderName = "TEST",
                EstablishmentDate = DateTime.Now,
                Location = "TEST"
            };

            var response = await brigadeService.UpdateBrigade(brigade);

            Assert.AreEqual(response.Description, "Brigade does not exist");
        }

        [TestMethod]
        public async Task DeleteBrigadesTest()
        {
            var brigade1 = new Brigade
            {
                ID = 1,
                Name = "test1",
                CommanderName = "test1",
                EstablishmentDate = DateTime.Now,
                Location = "test1"
            };

            var brigade2 = new Brigade
            {
                ID = 2,
                Name = "test2",
                CommanderName = "test2",
                EstablishmentDate = DateTime.Now,
                Location = "test2"
            };

            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { brigade1, brigade2 });

            var response = await brigadeService.DeleteBrigade(2);

            Assert.AreEqual(StatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Data);
            Assert.IsTrue(response.Data == true);
        }

        [TestMethod]
        public async Task DeleteBrigadesTest_NonExistingBrigade()
        {
            var mockRepository = new Mock<BaseRepository<Brigade>>();
            var brigadeService = new BrigadeService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Brigade> { });

            var response = await brigadeService.DeleteBrigade(2);

            Assert.AreEqual(response.Description, "Brigade does not exist");
        }
    }
}
