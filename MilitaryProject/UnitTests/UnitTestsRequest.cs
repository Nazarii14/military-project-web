using NUnit.Framework;
using MilitaryProject.BLL.Services;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Weapon;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MilitaryProject.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using MilitaryProject.DAL;
using System.Data.Entity;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.ViewModels.Request;
using MilitaryProject.BLL.Interfaces;

namespace UnitTests
{
    [TestFixture]
    public class UnitTestRequest
    {
        private RequestService _requestService;
        private ApplicationDbContext _dbContext;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);

            _dbContext.AddRange(new List<Brigade>
            {
                new Brigade { ID = 1, Name = "Brigade1", CommanderName = "Commander1", EstablishmentDate = DateTime.Now.AddDays(30), Location = "Location1" },
                new Brigade { ID = 2, Name = "Brigade2", CommanderName = "Commander2", EstablishmentDate = DateTime.Now.AddDays(20), Location = "Location2" },
                new Brigade { ID = 3, Name = "Brigade3", CommanderName = "Commander3", EstablishmentDate = DateTime.Now.AddDays(10), Location = "Location3" }
            });

            _dbContext.AddRange(new List<Weapon>
            {
                new Weapon { ID = 1, Name = "Weapon1", Type = "Type1", Price = 100.00m, Weight = 10.5f },
                new Weapon { ID = 2, Name = "Weapon2", Type = "Type2", Price = 200.00m, Weight = 20.5f },
                new Weapon { ID = 3, Name = "Weapon3", Type = "Type3", Price = 300.00m, Weight = 30.5f }
            });

            _dbContext.AddRange(new List<Ammunition>
            {
                new Ammunition { ID = 1, Name = "Ammunition1", Type = "Type1", Price = 100.00m, Size = "S" },
                new Ammunition { ID = 2, Name = "Ammunition2", Type = "Type2", Price = 200.00m, Size = "M" },
                new Ammunition { ID = 3, Name = "Ammunition3", Type = "Type3", Price = 300.00m, Size = "X" }
            });

            _dbContext.AddRange(new List<Request>
            {
                new Request { ID = 1, BrigadeID = 1, WeaponID = 1, AmmunitionID = 1, WeaponQuantity = 10, AmmunitionQuantity = 10, Message = "Message1", RequestStatus = "Status1" },
                new Request { ID = 2, BrigadeID = 2, WeaponID = 2, AmmunitionID = 2, WeaponQuantity = 20, AmmunitionQuantity = 20, Message = "Message2", RequestStatus = "Status2" },
                new Request { ID = 3, BrigadeID = 3, WeaponID = 3, AmmunitionID = 3, WeaponQuantity = 30, AmmunitionQuantity = 30, Message = "Message3", RequestStatus = "Status3" }
            });
            _dbContext.SaveChanges();
            _requestService = new RequestService(new RequestRepository(_dbContext), new BrigadeRepository(_dbContext), new WeaponRepository(_dbContext), new AmmunitionRepository(_dbContext));
        }

        [TearDown]
        public void TearDown()
        {
             _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetRequest_WhenRequestExists_ReturnsRequest()
        {
            int existingRequestId = 1;
            var expectedRequest = new Request { ID = existingRequestId, BrigadeID = 1, WeaponID = 1, AmmunitionID = 1, WeaponQuantity = 10, AmmunitionQuantity = 10, Message = "Message1", RequestStatus = "Status1" };

            var result = await _requestService.GetRequest(existingRequestId);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedRequest.ID, result.Data.ID);
            Assert.AreEqual(expectedRequest.BrigadeID, result.Data.BrigadeID);
            Assert.AreEqual(expectedRequest.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(expectedRequest.AmmunitionID, result.Data.AmmunitionID);
            Assert.AreEqual(expectedRequest.WeaponQuantity, result.Data.WeaponQuantity);
            Assert.AreEqual(expectedRequest.AmmunitionQuantity, result.Data.AmmunitionQuantity);
            Assert.AreEqual(expectedRequest.Message, result.Data.Message);
            Assert.AreEqual(expectedRequest.RequestStatus, result.Data.RequestStatus);
        }

        [Test]
        public async Task GetRequests_WhenRequestsExist_ReturnsRequestsList()
        {
            var expectedRequests = new List<Request>
            {
                new Request { ID = 1, BrigadeID = 1, WeaponID = 1, AmmunitionID = 1, WeaponQuantity = 10, AmmunitionQuantity = 10, Message = "Message1", RequestStatus = "Status1" },
                new Request { ID = 2, BrigadeID = 2, WeaponID = 2, AmmunitionID = 2, WeaponQuantity = 20, AmmunitionQuantity = 20, Message = "Message2", RequestStatus = "Status2" },
                new Request { ID = 3, BrigadeID = 3, WeaponID = 3, AmmunitionID = 3, WeaponQuantity = 30, AmmunitionQuantity = 30, Message = "Message3", RequestStatus = "Status3" }
            };
            var result = await _requestService.GetRequests();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedRequests.Count, result.Data.Count);
            for (int i = 0; i < expectedRequests.Count; i++)
            {
                Assert.AreEqual(expectedRequests[i].ID, result.Data[i].ID);
                Assert.AreEqual(expectedRequests[i].BrigadeID, result.Data[i].BrigadeID);
                Assert.AreEqual(expectedRequests[i].WeaponID, result.Data[i].WeaponID);
                Assert.AreEqual(expectedRequests[i].AmmunitionID, result.Data[i].AmmunitionID);
                Assert.AreEqual(expectedRequests[i].WeaponQuantity, result.Data[i].WeaponQuantity);
                Assert.AreEqual(expectedRequests[i].AmmunitionQuantity, result.Data[i].AmmunitionQuantity);
                Assert.AreEqual(expectedRequests[i].Message, result.Data[i].Message);
                Assert.AreEqual(expectedRequests[i].RequestStatus, result.Data[i].RequestStatus);
            }
        }

        [Test]
        public async Task GetRequest_WhenRequestDoesNotExist_ReturnsError()
        {
            var nonExistingRequestId = -1;
            var result = await _requestService.GetRequest(nonExistingRequestId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task CreateRequest_WhenRequestIsValid_ReturnsRequest()
        {
            var requestViewModel = new RequestViewModel
            {
                BrigadeID = 1,
                WeaponID = 1,
                AmmunitionID = 1,
                WeaponQuantity = 10,
                AmmunitionQuantity = 10,
                Message = "Message1",
                RequestStatus = "Status1"
            };

            var result = await _requestService.CreateRequest(requestViewModel);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(requestViewModel.BrigadeID, result.Data.BrigadeID);
            Assert.AreEqual(requestViewModel.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(requestViewModel.AmmunitionID, result.Data.AmmunitionID);
            Assert.AreEqual(requestViewModel.WeaponQuantity, result.Data.WeaponQuantity);
            Assert.AreEqual(requestViewModel.AmmunitionQuantity, result.Data.AmmunitionQuantity);
            Assert.AreEqual(requestViewModel.Message, result.Data.Message);
            Assert.AreEqual(requestViewModel.RequestStatus, result.Data.RequestStatus);
        }

        [Test]
        public async Task CreateRequest_WhenBrigadeDoesNotExist_ReturnsError()
        {
            var requestViewModel = new RequestViewModel
            {
                BrigadeID = -1,
                WeaponID = 1,
                AmmunitionID = 1,
                WeaponQuantity = 10,
                AmmunitionQuantity = 10,
                Message = "Message1",
                RequestStatus = "Status1"
            };

            var result = await _requestService.CreateRequest(requestViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Brigade does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task CreateRequest_WhenWeaponDoesNotExist_ReturnsError()
        {
            var requestViewModel = new RequestViewModel
            {
                BrigadeID = 1,
                WeaponID = -1,
                AmmunitionID = 1,
                WeaponQuantity = 10,
                AmmunitionQuantity = 10,
                Message = "Message1",
                RequestStatus = "Status1"
            };

            var result = await _requestService.CreateRequest(requestViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task CreateRequest_WhenAmmunitionDoesNotExist_ReturnsError()
        {
            var requestViewModel = new RequestViewModel
            {
                BrigadeID = 1,
                WeaponID = 1,
                AmmunitionID = -1,
                WeaponQuantity = 10,
                AmmunitionQuantity = 10,
                Message = "Message1",
                RequestStatus = "Status1"
            };

            var result = await _requestService.CreateRequest(requestViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateRequest_WhenRequestExists_UpdatesRequest()
        {
            var requestViewModel = new RequestViewModel
            {
                ID = 1,
                BrigadeID = 1,
                WeaponID = 1,
                AmmunitionID = 1,
                WeaponQuantity = 10,
                AmmunitionQuantity = 10,
                Message = "Message1",
                RequestStatus = "Status1"
            };

            var result = await _requestService.UpdateRequest(requestViewModel);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(requestViewModel.ID, result.Data.ID);
            Assert.AreEqual(requestViewModel.BrigadeID, result.Data.BrigadeID);
            Assert.AreEqual(requestViewModel.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(requestViewModel.AmmunitionID, result.Data.AmmunitionID);
            Assert.AreEqual(requestViewModel.WeaponQuantity, result.Data.WeaponQuantity);
            Assert.AreEqual(requestViewModel.AmmunitionQuantity, result.Data.AmmunitionQuantity);
            Assert.AreEqual(requestViewModel.Message, result.Data.Message);
            Assert.AreEqual(requestViewModel.RequestStatus, result.Data.RequestStatus);
        }

        [Test]
        public async Task UpdateRequest_WhenRequestDoesNotExist_ReturnsError()
        {
            var requestViewModel = new RequestViewModel
            {
                ID = -1,
                BrigadeID = 1,
                WeaponID = 1,
                AmmunitionID = 1,
                WeaponQuantity = 10,
                AmmunitionQuantity = 10,
                Message = "Message1",
                RequestStatus = "Status1"
            };

            var result = await _requestService.UpdateRequest(requestViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Request does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task DeleteRequest_WhenRequestExists_DeletesRequest()
        {
            int existingRequestId = 1;
            var result = await _requestService.DeleteRequest(existingRequestId);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteRequest_WithNonExistingWeapon_ReturnsError()
        {
            int nonExistingRequestId = -1;
            var result = await _requestService.DeleteRequest(nonExistingRequestId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Request does not exist", result.Description);
            Assert.IsFalse(result.Data);
        }
    }
}
