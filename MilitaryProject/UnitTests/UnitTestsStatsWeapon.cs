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
using Microsoft.EntityFrameworkCore.InMemory;
using MilitaryProject.DAL;
using System.Data.Entity;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.ViewModels.Request;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Stats;

namespace StatsWeaponUnitTests
{
    [TestFixture]
    public class StatsWeaponUnitTests
    {
        private StatsWeaponService _statsWeaponService;
        private ApplicationDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase228")
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

            _dbContext.AddRange(new List<User>
            {
                new User { ID = 1, BrigadeID = 1, Name = "User1", Lastname = "LastName1", Password = "Pass1", Email = "email1@gmail.com", Age = 22, Role = Role.Commander},
                new User { ID = 2, BrigadeID = 1, Name = "User2", Lastname = "LastName2", Password = "Pass2", Email = "email2@gmail.com", Age = 23, Role = Role.Soldier},
                new User { ID = 3, BrigadeID = 3, Name = "User3", Lastname = "LastName3", Password = "Pass3", Email = "email3@gmail.com", Age = 24, Role = Role.Commander}
            });

            _dbContext.AddRange(new List<BrigadeStorage>
            {
                new BrigadeStorage {ID = 1, BrigadeID = 1, WeaponID = 2, AmmunitionID = 1, WeaponQuantity = 10, WeaponRemainder = 5, AmmunitionQuantity = 0, AmmunitionRemainder = 0 },
                new BrigadeStorage {ID = 2, BrigadeID = 1, WeaponID = 1, AmmunitionID = 2, WeaponQuantity = 20, WeaponRemainder = 10, AmmunitionQuantity = 0, AmmunitionRemainder = 0 },
                new BrigadeStorage {ID = 3, BrigadeID = 3, WeaponID = 3, AmmunitionID = 3, WeaponQuantity = 30, WeaponRemainder = 15, AmmunitionQuantity = 0, AmmunitionRemainder = 0},
            });

            _dbContext.SaveChanges();
            _statsWeaponService = new StatsWeaponService(new WeaponRepository(_dbContext), new UserRepository(_dbContext), new BrigadeStorageRepository(_dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetByIdWhenExists_ReturnsWeapon()
        {
            int userID = 1, weaponID = 1;
            var result = await _statsWeaponService.GetById(userID, weaponID);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(1, result.Data.UserID);
            Assert.AreEqual(1, result.Data.WeaponID);
            Assert.AreEqual(20, result.Data.NeededWeaponCount);
            Assert.AreEqual(10, result.Data.WeaponCount);
            Assert.AreEqual("Weapon1", result.Data.WeaponName);
            Assert.AreEqual("Type1", result.Data.WeaponType);
            Assert.AreEqual(100.00m, result.Data.WeaponPrice);
            Assert.AreEqual(10.5f, result.Data.WeaponWeight);
        }

        [Test]
        public async Task GetByIdWhenNotExists_ReturnsNotFound()
        {
            int userID = 1, weaponID = -1;
            var result = await _statsWeaponService.GetById(userID, weaponID);

            Assert.IsNull(result.Data);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task GetByIdWhenBrigadeNotExists_ReturnsNotFound()
        {
            int userID = -1, weaponID = 1;
            var result = await _statsWeaponService.GetById(userID, weaponID);

            Assert.IsNull(result.Data);
            Assert.AreEqual("User does not exist", result.Description);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task GetAllWhenExists_ReturnsWeapons()
        {
            int userID = 1;

            var result = await _statsWeaponService.GetAll(userID);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(2, result.Data.Count());

            Assert.AreEqual(1, result.Data.ElementAt(0).UserID);
            Assert.AreEqual(2, result.Data.ElementAt(0).WeaponID);
            Assert.AreEqual(10, result.Data.ElementAt(0).NeededWeaponCount);
            Assert.AreEqual(5, result.Data.ElementAt(0).WeaponCount);
            Assert.AreEqual("Weapon2", result.Data.ElementAt(0).WeaponName);
            Assert.AreEqual("Type2", result.Data.ElementAt(0).WeaponType);
            Assert.AreEqual(200.00m, result.Data.ElementAt(0).WeaponPrice);
            Assert.AreEqual(20.5f, result.Data.ElementAt(0).WeaponWeight);

            Assert.AreEqual(1, result.Data.ElementAt(1).UserID);
            Assert.AreEqual(1, result.Data.ElementAt(1).WeaponID);
            Assert.AreEqual(20, result.Data.ElementAt(1).NeededWeaponCount);
            Assert.AreEqual(10, result.Data.ElementAt(1).WeaponCount);
            Assert.AreEqual("Weapon1", result.Data.ElementAt(1).WeaponName);
            Assert.AreEqual("Type1", result.Data.ElementAt(1).WeaponType);
            Assert.AreEqual(100.00m, result.Data.ElementAt(1).WeaponPrice);
            Assert.AreEqual(10.5f, result.Data.ElementAt(1).WeaponWeight);
        }

        [Test]
        public async Task GetAllWhenNotExists_ReturnsEmptyList()
        {
            _dbContext.RemoveRange(_dbContext.BrigadeStorages);
            _dbContext.SaveChanges();
            int userID = 1;
            var result = await _statsWeaponService.GetAll(userID);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Count());
        }

        [Test]
        public async Task CreateWithValidData_CreatesWeapon()
        {
            var newWeapon = new StatsWeaponViewModel
            {
                UserID = 1,
                WeaponID = 3,
                NeededWeaponCount = 40,
                WeaponCount = 20,
                WeaponName = "Name3",
                WeaponType = "Type3",
                WeaponPrice = 300.00m,
                WeaponWeight = 30.5f,
            };

            var result = await _statsWeaponService.Create(newWeapon);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(1, result.Data.UserID);
            Assert.AreEqual(3, result.Data.WeaponID);
            Assert.AreEqual(40, result.Data.NeededWeaponCount);
            Assert.AreEqual(20, result.Data.WeaponCount);
            Assert.AreEqual("Name3", result.Data.WeaponName);
            Assert.AreEqual("Type3", result.Data.WeaponType);
            Assert.AreEqual(300.00m, result.Data.WeaponPrice);
            Assert.AreEqual(30.5f, result.Data.WeaponWeight);
        }

        [Test]
        public async Task CreateWithInvalidData_ReturnsBadRequest()
        {
            var newWeapon = new StatsWeaponViewModel
            {
                UserID = 1,
                WeaponID = 1,
                NeededWeaponCount = 40,
                WeaponCount = 20,
                WeaponName = "Weapon1",
                WeaponType = "Type3",
                WeaponPrice = 300.00m,
                WeaponWeight = 30.5f,
            };

            var result = await _statsWeaponService.Create(newWeapon);
            Assert.IsNull(result.Data);
            Assert.AreEqual(StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("Weapon already exists in the storage", result.Description);
        }

        [Test]
        public async Task UpdateWithValidData_UpdatesWeapon()
        {
            var updatedWeapon = new StatsWeaponViewModel
            {
                UserID = 1,
                WeaponID = 1,
                NeededWeaponCount = 40,
                WeaponCount = 20,
                WeaponName = "Name3",
                WeaponType = "Type3",
                WeaponPrice = 300.00m,
                WeaponWeight = 30.5f,
            };

            var result = await _statsWeaponService.Update(updatedWeapon);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(1, result.Data.UserID);
            Assert.AreEqual(1, result.Data.WeaponID);
            Assert.AreEqual(40, result.Data.NeededWeaponCount);
            Assert.AreEqual(20, result.Data.WeaponCount);
            Assert.AreEqual("Name3", result.Data.WeaponName);
            Assert.AreEqual("Type3", result.Data.WeaponType);
            Assert.AreEqual(300.00m, result.Data.WeaponPrice);
            Assert.AreEqual(30.5f, result.Data.WeaponWeight);
        }

        [Test]
        public async Task UpdateWithInvalidData_ReturnsBadRequest()
        {
            var updatedWeapon = new StatsWeaponViewModel
            {
                UserID = 1,
                WeaponID = 3,
                NeededWeaponCount = 40,
                WeaponCount = 20,
                WeaponName = "Name3",
                WeaponType = "Type3",
                WeaponPrice = 300.00m,
                WeaponWeight = 30.5f,
            };

            var result = await _statsWeaponService.Update(updatedWeapon);
            Assert.IsNull(result.Data);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist in the storage", result.Description);
        }

        [Test]
        public async Task DeleteWeaponWithValidId_DeletesWeapon()
        {
            int userID = 1, weaponID = 1;
            var result = await _statsWeaponService.Delete(userID, weaponID);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteWeaponWithInvalidId_ReturnsNotFound()
        {
            int userID = 1, weaponID = -1;
            var result = await _statsWeaponService.Delete(userID, weaponID);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.IsFalse(result.Data);
        }
    }
}
