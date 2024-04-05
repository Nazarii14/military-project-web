using NUnit.Framework;
using MilitaryProject.BLL.Services;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Ammunition;
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

namespace AmmunitionUnitTests
{
    public class AmmunitionUnitTests
    {
        private AmmunitionService _ammunitionService;
        private ApplicationDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.AddRange(new List<Ammunition>
            {
                new Ammunition { ID = 1, Name = "Ammunition1", Type = "Type1", Price = 100.00m, Size = "Size1" },
                new Ammunition { ID = 2, Name = "Ammunition2", Type = "Type2", Price = 200.00m, Size = "Size2" },
                new Ammunition { ID = 3, Name = "Ammunition3", Type = "Type3", Price = 300.00m, Size = "Size3" }
            });
            _dbContext.SaveChanges();
            _ammunitionService = new AmmunitionService(new AmmunitionRepository(_dbContext));

        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAmmunition_WhenAmmunitionExists_ReturnsAmmunition()
        {
            int existingAmmunitionId = 1;
            var expectedAmmunition = new Ammunition { ID = existingAmmunitionId, Name = "Ammunition1", Type = "Type1", Price = 100.00m, Size = "Size1" };

            var result = await _ammunitionService.GetById(existingAmmunitionId);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedAmmunition.ID, result.Data.ID);
            Assert.AreEqual(expectedAmmunition.Name, result.Data.Name);
            Assert.AreEqual(expectedAmmunition.Type, result.Data.Type);
            Assert.AreEqual(expectedAmmunition.Price, result.Data.Price);
            Assert.AreEqual(expectedAmmunition.Size, result.Data.Size);
        }

        [Test]
        public async Task GetAmmunition_WhenAmmunitionDoesNotExist_ReturnsError()
        {
            var nonExistingAmmunitionId = -1;
            var result = await _ammunitionService.GetById(nonExistingAmmunitionId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task GetAmmunitions_WhenAmmunitionsExist_ReturnsAmmunitionsList()
        {
            var expectedAmmunitions = new List<Ammunition>
            {
                new Ammunition { ID = 1, Name = "Ammunition1", Type = "Type1", Price = 100.00m, Size = "Size1" },
                new Ammunition { ID = 2, Name = "Ammunition2", Type = "Type2", Price = 200.00m, Size = "Size2" },
                new Ammunition { ID = 3, Name = "Ammunition3", Type = "Type3", Price = 300.00m, Size = "Size3" }
            };
            var result = await _ammunitionService.GetAll();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedAmmunitions.Count, result.Data.Count);
            for (int i = 0; i < expectedAmmunitions.Count; i++)
            {
                Assert.AreEqual(expectedAmmunitions[i].ID, result.Data[i].ID);
                Assert.AreEqual(expectedAmmunitions[i].Name, result.Data[i].Name);
                Assert.AreEqual(expectedAmmunitions[i].Type, result.Data[i].Type);
                Assert.AreEqual(expectedAmmunitions[i].Price, result.Data[i].Price);
                Assert.AreEqual(expectedAmmunitions[i].Size, result.Data[i].Size);
            }
        }

        [Test]
        public async Task GetAmmunitions_WhenNoAmmunitionsExist_ReturnsEmptyList()
        {
            _dbContext.RemoveRange(_dbContext.Ammunitions);
            _dbContext.SaveChanges();
            var result = await _ammunitionService.GetAll();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Count);
        }

        [Test]
        public async Task CreateAmmunition_WithValidData_CreatesNewAmmunition()
        {
            var newAmmunition = new AmmunitionViewModel
            {
                Name = "Ammunition4",
                Type = "Type4",
                Price = 400.00m,
                Size = "Size4"
            };
            var result = await _ammunitionService.Create(newAmmunition);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(newAmmunition.Name, result.Data.Name);
            Assert.AreEqual(newAmmunition.Type, result.Data.Type);
            Assert.AreEqual(newAmmunition.Price, result.Data.Price);
            Assert.AreEqual(newAmmunition.Size, result.Data.Size);
        }

        [Test]
        public async Task CreateAmmunition_WithDuplicateName_ReturnsError()
        {
            var existingAmmunition = new AmmunitionViewModel
            {
                Name = "Ammunition1",
                Type = "Type1",
                Price = 100.00m,
                Size = "Size1"
            };
            var result = await _ammunitionService.Create(existingAmmunition);
            Assert.AreEqual(StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("Ammunition already exists", result.Description);
        }

        [Test]
        public async Task UpdateAmmunition_WithValidData_UpdatesExistingAmmunition()
        {
            var existingAmmunitionId = 1;
            var updatedAmmunition = new AmmunitionViewModel
            {
                ID = existingAmmunitionId,
                Name = "Ammunition1",
                Type = "Type1Updated",
                Price = 150.00m,
                Size = "Size1Updated"
            };
            var result = await _ammunitionService.Update(updatedAmmunition);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(updatedAmmunition.Name, result.Data.Name);
            Assert.AreEqual(updatedAmmunition.Type, result.Data.Type);
            Assert.AreEqual(updatedAmmunition.Price, result.Data.Price);
            Assert.AreEqual(updatedAmmunition.Size, result.Data.Size);
        }

        [Test]
        public async Task UpdateAmmunition_WithNonExistingAmmunition_ReturnsError()
        {
            var nonExistingAmmunition = new AmmunitionViewModel
            {
                Name = "AmmunitionNonExisting",
                Type = "TypeNonExisting",
                Price = 150.00m,
                Size = "SizeNonExisting"
            };
            var result = await _ammunitionService.Update(nonExistingAmmunition);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task DeleteAmmunition_WithExistingAmmunition_DeletesAmmunition()
        {
            var existingAmmunitionId = 1;
            var result = await _ammunitionService.Delete(existingAmmunitionId);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual("Ammunition deleted successfully", result.Description);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteAmmunition_WithNonExistingAmmunition_ReturnsError()
        {
            var nonExistingAmmunitionId = -1;
            var result = await _ammunitionService.Delete(nonExistingAmmunitionId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.IsFalse(result.Data);
        }
    }
}
