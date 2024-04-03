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
using MilitaryProject.Domain.ViewModels.Brigade;
using MilitaryProject.BLL.Interfaces;

namespace UnitTest
{
    [TestFixture]
    public class BrigadeTest
    {
        private BrigadeService _brigadeService;
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
                new Brigade { ID = 1, Name = "Brigade1", CommanderName = "Commander1", EstablishmentDate = new System.DateTime(2021, 1, 1), Location = "Location1" },
                new Brigade { ID = 2, Name = "Brigade2", CommanderName = "Commander2", EstablishmentDate = new System.DateTime(2021, 2, 2), Location = "Location2" },
                new Brigade { ID = 3, Name = "Brigade3", CommanderName = "Commander3", EstablishmentDate = new System.DateTime(2021, 3, 3), Location = "Location3" }
            });
            _dbContext.SaveChanges();
            _brigadeService = new BrigadeService(new BrigadeRepository(_dbContext));

        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetBrigade_WhenBrigadeExists_ReturnsBrigade()
        {
            int existingBrigadeId = 1;
            var expectedBrigade = new Brigade { ID = existingBrigadeId, Name = "Brigade1", CommanderName = "Commander1", EstablishmentDate = new System.DateTime(2021, 1, 1), Location = "Location1" };

            var result = await _brigadeService.GetById(existingBrigadeId);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedBrigade.ID, result.Data.ID);
            Assert.AreEqual(expectedBrigade.Name, result.Data.Name);
            Assert.AreEqual(expectedBrigade.CommanderName, result.Data.CommanderName);
            Assert.AreEqual(expectedBrigade.EstablishmentDate, result.Data.EstablishmentDate);
            Assert.AreEqual(expectedBrigade.Location, result.Data.Location);
        }

        [Test]
        public async Task GetBrigade_WhenBrigadeExists_ReturnsBrigadeList()
        {
            var expectedBrigades = new List<Brigade>
            {
                new Brigade { ID = 1, Name = "Brigade1", CommanderName = "Commander1", EstablishmentDate = new System.DateTime(2021, 1, 1), Location = "Location1" },
                new Brigade { ID = 2, Name = "Brigade2", CommanderName = "Commander2", EstablishmentDate = new System.DateTime(2021, 2, 2), Location = "Location2" },
                new Brigade { ID = 3, Name = "Brigade3", CommanderName = "Commander3", EstablishmentDate = new System.DateTime(2021, 3, 3), Location = "Location3" }
            };

            var result = await _brigadeService.GetAll();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedBrigades.Count, result.Data.Count);

            for (int i = 0; i < expectedBrigades.Count; i++)
            {
                Assert.AreEqual(expectedBrigades[i].ID, result.Data[i].ID);
                Assert.AreEqual(expectedBrigades[i].Name, result.Data[i].Name);
                Assert.AreEqual(expectedBrigades[i].CommanderName, result.Data[i].CommanderName);
                Assert.AreEqual(expectedBrigades[i].EstablishmentDate, result.Data[i].EstablishmentDate);
                Assert.AreEqual(expectedBrigades[i].Location, result.Data[i].Location);
            }
        }

        [Test]
        public async Task GetBrigade_WhenBrigadeDoestNotExist_ReturnsError()
        {
            var nonExistingBrigadeId = -1;
            var result = await _brigadeService.GetById(nonExistingBrigadeId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task GetBrigades_WhenNoBrigadesExist_ReturnsEmptyList()
        {
            _dbContext.Brigades.RemoveRange(_dbContext.Brigades);
            _dbContext.SaveChanges();
            var result = await _brigadeService.GetAll();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Count);
        }

        [Test]
        public async Task CreateBrigade_WithValidData_CreateNewBrigade()
        {
            var newBrigade = new BrigadeViewModel
            {
                Name = "Brigade4",
                CommanderName = "Commander4",
                EstablishmentDate = new System.DateTime(2021, 4, 4),
                Location = "Location4"
            };

            var result = await _brigadeService.Create(newBrigade);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(newBrigade.Name, result.Data.Name);
            Assert.AreEqual(newBrigade.CommanderName, result.Data.CommanderName);
            Assert.AreEqual(newBrigade.EstablishmentDate, result.Data.EstablishmentDate);
            Assert.AreEqual(newBrigade.Location, result.Data.Location);
        }

        [Test]
        public async Task CreateBrigade_WithDuplicateName_ReturnsError()
        {
            var existingBrigade = new BrigadeViewModel
            {
                Name = "Brigade1",
                CommanderName = "Commander1",
                EstablishmentDate = new System.DateTime(2021, 1, 1),
                Location = "Location1"
            };

            var result = await _brigadeService.Create(existingBrigade);
            Assert.AreEqual(StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("Brigade with the same name already exists.", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateBrigade_WithValidData_UpdatesBrigade()
        {
            var existingBrigadeId = 1;
            var updatedBrigade = new BrigadeViewModel
            {
                ID = existingBrigadeId,
                Name = "Brigade1Updated",
                CommanderName = "Commander1Updated",
                EstablishmentDate = new System.DateTime(2021, 1, 1),
                Location = "Location1Updated"
            };

            var result = await _brigadeService.Update(updatedBrigade);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(updatedBrigade.ID, result.Data.ID);
            Assert.AreEqual(updatedBrigade.Name, result.Data.Name);
            Assert.AreEqual(updatedBrigade.CommanderName, result.Data.CommanderName);
            Assert.AreEqual(updatedBrigade.EstablishmentDate, result.Data.EstablishmentDate);
            Assert.AreEqual(updatedBrigade.Location, result.Data.Location);
        }

        [Test]
        public async Task UpdateBrigade_WithNonExistingBrigade_ReturnsError()
        {
            var nonExistingBrigade = new BrigadeViewModel
            {
                ID = -1,
                Name = "BrigadeNonExisting",
                CommanderName = "CommanderNonExisting",
                EstablishmentDate = new System.DateTime(2021, 1, 1),
                Location = "LocationNonExisting"
            };

            var result = await _brigadeService.Update(nonExistingBrigade);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Brigade not found.", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task DeleteBrigade_WithValidData_DeletesBrigade()
        {
            var existingBrigadeId = 1;
            var result = await _brigadeService.Delete(existingBrigadeId);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteBrigade_WithNonExistingBrigade_ReturnsError()
        {
            var nonExistingBrigadeId = -1;
            var result = await _brigadeService.Delete(nonExistingBrigadeId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Brigade not found.", result.Description);
            Assert.IsFalse(result.Data);
        }
    }
}
