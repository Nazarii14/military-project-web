using NUnit.Framework;
using MilitaryProject.BLL.Services;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MilitaryProject.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using MilitaryProject.DAL;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Request;
using MilitaryProject.Domain.ViewModels.Stats;

namespace StatsAmmunitionUnitTests
{
    [TestFixture]
    public class StatsAmmunitionUnitTests
    {
        private StatsAmmunitionService _statsAmmunitionService;
        private ApplicationDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase147")
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
                new Ammunition { ID = 3, Name = "Ammunition3", Type = "Type3", Price = 300.00m, Size = "L" }
            });

            _dbContext.AddRange(new List<User>
            {
                new User { ID = 1, BrigadeID = 1, Name = "User1", Lastname = "LastName1", Password = "Pass1", Email = "email1@gmail.com", Age = 22, Role = Role.Commander},
                new User { ID = 2, BrigadeID = 1, Name = "User2", Lastname = "LastName2", Password = "Pass2", Email = "email2@gmail.com", Age = 23, Role = Role.Soldier},
                new User { ID = 3, BrigadeID = 3, Name = "User3", Lastname = "LastName3", Password = "Pass3", Email = "email3@gmail.com", Age = 24, Role = Role.Commander}
            });

            _dbContext.AddRange(new List<BrigadeStorage>
            {
                new BrigadeStorage {ID = 1, BrigadeID = 1, WeaponID = 2, AmmunitionID = 1, WeaponQuantity = 10, WeaponRemainder = 5, AmmunitionQuantity = 20, AmmunitionRemainder = 10 },
                new BrigadeStorage {ID = 2, BrigadeID = 1, WeaponID = 1, AmmunitionID = 2, WeaponQuantity = 20, WeaponRemainder = 10, AmmunitionQuantity = 30, AmmunitionRemainder = 15 },
                new BrigadeStorage {ID = 3, BrigadeID = 3, WeaponID = 3, AmmunitionID = 3, WeaponQuantity = 30, WeaponRemainder = 15, AmmunitionQuantity = 40, AmmunitionRemainder = 20 }
            });

            _dbContext.SaveChanges();
            _statsAmmunitionService = new StatsAmmunitionService(new AmmunitionRepository(_dbContext), new UserRepository(_dbContext), new BrigadeStorageRepository(_dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetByIdWhenExists_ReturnsAmmunition()
        {
            int userID = 1, ammunitionID = 1;
            var result = await _statsAmmunitionService.GetById(userID, ammunitionID);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(1, result.Data.UserID);
            Assert.AreEqual(1, result.Data.AmmunitionID);
            Assert.AreEqual(20, result.Data.NeededAmmunitionCount);
            Assert.AreEqual(10, result.Data.AmmunitionCount);
            Assert.AreEqual("Ammunition1", result.Data.AmmunitionName);
            Assert.AreEqual("Type1", result.Data.AmmunitionType);
            Assert.AreEqual(100.00m, result.Data.AmmunitionPrice);
        }

        [Test]
        public async Task GetByIdWhenNotExists_ReturnsNotFound()
        {
            int userID = 1, ammunitionID = -1;
            var result = await _statsAmmunitionService.GetById(userID, ammunitionID);

            Assert.IsNull(result.Data);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task GetByIdWhenBrigadeNotExists_ReturnsNotFound()
        {
            int userID = -1, ammunitionID = 1;
            var result = await _statsAmmunitionService.GetById(userID, ammunitionID);

            Assert.IsNull(result.Data);
            Assert.AreEqual("User does not exist", result.Description);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task GetAllWhenExists_ReturnsAmmunition()
        {
            int userID = 1;
            var result = await _statsAmmunitionService.GetAll(userID);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(2, result.Data.Count());

            Assert.AreEqual(1, result.Data.First().UserID);
            Assert.AreEqual(1, result.Data.First().AmmunitionID);
            Assert.AreEqual(20, result.Data.First().NeededAmmunitionCount);
            Assert.AreEqual(10, result.Data.First().AmmunitionCount);
            Assert.AreEqual("Ammunition1", result.Data.First().AmmunitionName);
            Assert.AreEqual("Type1", result.Data.First().AmmunitionType);
            Assert.AreEqual(100.00m, result.Data.First().AmmunitionPrice);

            Assert.AreEqual(1, result.Data.Last().UserID);
            Assert.AreEqual(2, result.Data.Last().AmmunitionID);
            Assert.AreEqual(30, result.Data.Last().NeededAmmunitionCount);
            Assert.AreEqual(15, result.Data.Last().AmmunitionCount);
            Assert.AreEqual("Ammunition2", result.Data.Last().AmmunitionName);
            Assert.AreEqual("Type2", result.Data.Last().AmmunitionType);
            Assert.AreEqual(200.00m, result.Data.Last().AmmunitionPrice);
        }

        [Test]
        public async Task GetAllWhenNotExists_ReturnsEmptyList()
        {
            _dbContext.RemoveRange(_dbContext.BrigadeStorages);
            _dbContext.SaveChanges();
            int userID = 1;
            var result = await _statsAmmunitionService.GetAll(userID);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Count());
        }

        [Test]
        public async Task CreateWithValidData_CreatesAmmunition()
        {
            var newAmmunition = new StatsAmmunitionViewModel
            {
                UserID = 1,
                AmmunitionID = 3,
                NeededAmmunitionCount = 40,
                AmmunitionCount = 20,
                AmmunitionName = "Ammunition3",
                AmmunitionSize = "L",
                AmmunitionType = "Type3",
                AmmunitionPrice = 300.00m,
            };

            var result = await _statsAmmunitionService.Create(newAmmunition);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(1, result.Data.UserID);
            Assert.AreEqual(3, result.Data.AmmunitionID);
            Assert.AreEqual(40, result.Data.NeededAmmunitionCount);
            Assert.AreEqual(20, result.Data.AmmunitionCount);
            Assert.AreEqual("Ammunition3", result.Data.AmmunitionName);
            Assert.AreEqual("Type3", result.Data.AmmunitionType);
            Assert.AreEqual("L", result.Data.AmmunitionSize);
            Assert.AreEqual(300.00m, result.Data.AmmunitionPrice);
        }

        [Test]
        public async Task CreateWithInvalidData_ReturnsError()
        {
            var newAmmunition = new StatsAmmunitionViewModel
            {
                UserID = -1,
                AmmunitionID = 1,
                NeededAmmunitionCount = -20,
                AmmunitionCount = -10,
                AmmunitionName = "",
                AmmunitionType = "",
                AmmunitionPrice = -100.00m,
            };

            var result = await _statsAmmunitionService.Create(newAmmunition);

            Assert.IsNull(result.Data);
            Assert.AreEqual("User does not exist", result.Description);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task UpdateWithValidData_UpdatesAmmunition()
        {
            var updateAmmunition = new StatsAmmunitionViewModel
            {
                UserID = 1,
                AmmunitionID = 1,
                NeededAmmunitionCount = 25,
                AmmunitionCount = 12,
                AmmunitionName = "UpdatedName1",
                AmmunitionType = "UpdatedType1",
                AmmunitionPrice = 110.00m,
            };

            var result = await _statsAmmunitionService.Update(updateAmmunition);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(1, result.Data.UserID);
            Assert.AreEqual(1, result.Data.AmmunitionID);
            Assert.AreEqual(25, result.Data.NeededAmmunitionCount);
            Assert.AreEqual(12, result.Data.AmmunitionCount);
            Assert.AreEqual("UpdatedName1", result.Data.AmmunitionName);
            Assert.AreEqual("UpdatedType1", result.Data.AmmunitionType);
            Assert.AreEqual(110.00m, result.Data.AmmunitionPrice);
        }

        [Test]
        public async Task UpdateWithInvalidData_ReturnsError()
        {
            var updateAmmunition = new StatsAmmunitionViewModel
            {
                UserID = -1,
                AmmunitionID = 1,
                NeededAmmunitionCount = -20,
                AmmunitionCount = -10,
                AmmunitionName = "",
                AmmunitionType = "",
                AmmunitionPrice = -100.00m,
            };

            var result = await _statsAmmunitionService.Update(updateAmmunition);

            Assert.IsNull(result.Data);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("User does not exist", result.Description);
        }

        [Test]
        public async Task DeleteWithValidData_DeletesAmmunition()
        {
            int userID = 1, ammunitionID = 1;
            var result = await _statsAmmunitionService.Delete(userID, ammunitionID);

            Assert.IsTrue(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual("Ammunition deleted successfully", result.Description);
        }

        [Test]
        public async Task DeleteWithInvalidData_ReturnsError()
        {
            int userID = -1, ammunitionID = 1;
            var result = await _statsAmmunitionService.Delete(userID, ammunitionID);

            Assert.IsFalse(result.Data);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("User does not exist", result.Description);
        }
    }
}
