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
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.ViewModels.Stats;
using MilitaryProject.BLL.Interfaces;

namespace StatsUnitTests
{
    public class StatsUnitTests
    {
        private StatsService _statsService;
        private ApplicationDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
            _dbContext = new ApplicationDbContext(options);

            _dbContext.AddRange(new List<User>
            {
                new User { ID = 1, Name = "User1", BrigadeID = 1, Lastname = "Lastname1", Email = "Example1@gmil.com", Password = "passs1" },
                new User { ID = 2, Name = "User2", BrigadeID = 2, Lastname = "Lastname2", Email = "Example2@gmil.com", Password = "passs2" },
                new User { ID = 3, Name = "User3", BrigadeID = 3, Lastname = "Lastname3", Email = "Example3@gmil.com", Password = "passs3" },
            });

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

            _dbContext.AddRange(new List<BrigadeStorage>
            {
                new BrigadeStorage { ID = 1, BrigadeID = 1, WeaponID = 1, AmmunitionID = 1, WeaponQuantity = 10, AmmunitionQuantity = 25 },
                new BrigadeStorage { ID = 2, BrigadeID = 2, WeaponID = 2, AmmunitionID = 2, WeaponQuantity = 20, AmmunitionQuantity = 50 },
                new BrigadeStorage { ID = 3, BrigadeID = 3, WeaponID = 3, AmmunitionID = 3, WeaponQuantity = 30, AmmunitionQuantity = 75 },
            });

            _dbContext.SaveChanges();
            _statsService = new StatsService(new UserRepository(_dbContext), new BrigadeStorageRepository(_dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetBrigadeStatistics_ValidUserId_ReturnsCorrectStatistics()
        {
            var userId = 1; 
            var expectedWeaponTypeCounts = new Dictionary<string, int> { { "Type1", 10 } };
            var expectedAmmunitionTypeCounts = new Dictionary<string, int> { { "Type1", 25 } };
            var expectedWeaponWeightCounts = new Dictionary<float, int> { { 10.5f, 10 } };
            var expectedAmmunitionSizeCounts = new Dictionary<string, int> { { "S", 25 } };
            var expectedWeaponPriceCount = new Dictionary<decimal, int> { { 100.00m, 10 } };
            var expectedAmmunitionPriceCount = new Dictionary<decimal, int> { { 100.00m, 25 } };

            var result = await _statsService.GetBrigadeStatistics(userId);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual("Brigade statistics retrieved successfully.", result.Description);
            Assert.NotNull(result.Data);

            Assert.AreEqual(expectedWeaponTypeCounts.Count, result.Data.WeaponTypeCounts.Count);
            foreach (var kvp in expectedWeaponTypeCounts)
            {
                Assert.IsTrue(result.Data.WeaponTypeCounts.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, result.Data.WeaponTypeCounts[kvp.Key]);
            }

            Assert.AreEqual(expectedAmmunitionTypeCounts.Count, result.Data.AmmunitionTypeCounts.Count);
            foreach (var kvp in expectedAmmunitionTypeCounts)
            {
                Assert.IsTrue(result.Data.AmmunitionTypeCounts.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, result.Data.AmmunitionTypeCounts[kvp.Key]);
            }

            Assert.AreEqual(expectedWeaponWeightCounts.Count, result.Data.WeaponWeightCounts.Count);
            foreach (var kvp in expectedWeaponWeightCounts)
            {
                Assert.IsTrue(result.Data.WeaponWeightCounts.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, result.Data.WeaponWeightCounts[kvp.Key]);
            }

            Assert.AreEqual(expectedAmmunitionSizeCounts.Count, result.Data.AmmunitionSizeCounts.Count);
            foreach (var kvp in expectedAmmunitionSizeCounts)
            {
                Assert.IsTrue(result.Data.AmmunitionSizeCounts.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, result.Data.AmmunitionSizeCounts[kvp.Key]);
            }

            Assert.AreEqual(expectedWeaponPriceCount.Count, result.Data.WeaponPriceCount.Count);
            foreach (var kvp in expectedWeaponPriceCount)
            {
                Assert.IsTrue(result.Data.WeaponPriceCount.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, result.Data.WeaponPriceCount[kvp.Key]);
            }

            Assert.AreEqual(expectedAmmunitionPriceCount.Count, result.Data.AmmunitionPriceCount.Count);
            foreach (var kvp in expectedAmmunitionPriceCount)
            {
                Assert.IsTrue(result.Data.AmmunitionPriceCount.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, result.Data.AmmunitionPriceCount[kvp.Key]);
            }
        }

        [Test]
        public async Task GetBrigadeStatistics_NonValidUserId_ReturnsNotFound()
        {
            int noxExistentUserId = -1;
            var result = await _statsService.GetBrigadeStatistics(noxExistentUserId);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("User not found", result.Description);
        }

        [Test]
        public async Task GetBrigadeStatistics_EmptyBrigadeStorage_ReturnsNotFound()
        {
            _dbContext.BrigadeStorages.RemoveRange(_dbContext.BrigadeStorages);
            _dbContext.SaveChanges();

            var userId = 1;
            var result = await _statsService.GetBrigadeStatistics(userId);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            
            Assert.IsEmpty(result.Data.WeaponTypeCounts);
            Assert.IsEmpty(result.Data.AmmunitionTypeCounts);
            Assert.IsEmpty(result.Data.WeaponWeightCounts);
            Assert.IsEmpty(result.Data.AmmunitionSizeCounts);
            Assert.IsEmpty(result.Data.WeaponPriceCount);
            Assert.IsEmpty(result.Data.AmmunitionPriceCount);
        }
    }
}
