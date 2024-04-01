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

namespace MilitaryProject.UnitTest
{
    [TestFixture]
    public class UnitTestWeapon
    {
        private WeaponService _weaponService;
        private ApplicationDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.AddRange(new List<Weapon>
            {
                new Weapon { ID = 1, Name = "Weapon1", Type = "Type1", Price = 100.00m, Weight = 10.5f },
                new Weapon { ID = 2, Name = "Weapon2", Type = "Type2", Price = 200.00m, Weight = 20.5f },
                new Weapon { ID = 3, Name = "Weapon3", Type = "Type3", Price = 300.00m, Weight = 30.5f }
            });
            _dbContext.SaveChanges();
            _weaponService = new WeaponService(new WeaponRepository(_dbContext));

        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetWeapon_WhenWeaponExists_ReturnsWeapon()
        {
            int existingWeaponId = 1;
            var expectedWeapon = new Weapon { ID = existingWeaponId, Name = "Weapon1", Type = "Type1", Price = 100.00m, Weight = 10.5f };

            var result = await _weaponService.GetWeapon(existingWeaponId);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedWeapon.ID, result.Data.ID);
            Assert.AreEqual(expectedWeapon.Name, result.Data.Name);
            Assert.AreEqual(expectedWeapon.Type, result.Data.Type);
            Assert.AreEqual(expectedWeapon.Price, result.Data.Price);
            Assert.AreEqual(expectedWeapon.Weight, result.Data.Weight);
        }

        [Test]
        public async Task GetWeapons_WhenWeaponsExist_ReturnsWeaponsList()
        {
            var expectedWeapons = new List<Weapon>
            {
                new Weapon { ID = 1, Name = "Weapon1", Type = "Type1", Price = 100.00m, Weight = 10.5f },
                new Weapon { ID = 2, Name = "Weapon2", Type = "Type2", Price = 200.00m, Weight = 20.5f },
                new Weapon { ID = 3, Name = "Weapon3", Type = "Type3", Price = 300.00m, Weight = 30.5f }
            };
            var result = await _weaponService.GetWeapons();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedWeapons.Count, result.Data.Count);
            for (int i = 0; i < expectedWeapons.Count; i++)
            {
                Assert.AreEqual(expectedWeapons[i].ID, result.Data[i].ID);
                Assert.AreEqual(expectedWeapons[i].Name, result.Data[i].Name);
                Assert.AreEqual(expectedWeapons[i].Type, result.Data[i].Type);
                Assert.AreEqual(expectedWeapons[i].Price, result.Data[i].Price);
                Assert.AreEqual(expectedWeapons[i].Weight, result.Data[i].Weight);
            }
        }

        [Test]
        public async Task GetWeapon_WhenWeaponDoesNotExist_ReturnsError()
        {
            var nonExistingWeaponId = -1;
            var result = await _weaponService.GetWeapon(nonExistingWeaponId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task GetWeapons_WhenNoWeaponsExist_ReturnsEmptyList()
        {
            _dbContext.RemoveRange(_dbContext.Weapons);
            _dbContext.SaveChanges();
            var result = await _weaponService.GetWeapons();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Count);
        }

        [Test]
        public async Task CreateWeapon_WithValidData_CreatesNewWeapon()
        {
            var newWeapon = new WeaponViewModel
            {
                Name = "Weapon4",
                Type = "Type4",
                Price = 400.00m,
                Weight = 40.5f
            };
            var result = await _weaponService.Create(newWeapon);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(newWeapon.Name, result.Data.Name);
            Assert.AreEqual(newWeapon.Type, result.Data.Type);
            Assert.AreEqual(newWeapon.Price, result.Data.Price);
            Assert.AreEqual(newWeapon.Weight, result.Data.Weight);
        }

        [Test]
        public async Task CreateWeapon_WithDuplicateName_ReturnsError()
        {
            var existingWeapon = new WeaponViewModel
            { 
                Name = "Weapon1",
                Type = "Type1",
                Price = 100.00m,
                Weight = 10.5f
            };
            var result = await _weaponService.Create(existingWeapon);
            Assert.AreEqual(StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("Weapon with the same name already exists.", result.Description);
        }

        [Test]
        public async Task UpdateWeapon_WithValidData_UpdatesExistingWeapon()
        {
            var existingWeaponId = 1;
            var updatedWeapon = new WeaponViewModel
            {
                ID = existingWeaponId,
                Name = "Weapon1",
                Type = "Type1Updated",
                Price = 150.00m,
                Weight = 15.5f
            };
            var result = await _weaponService.Update(updatedWeapon);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(updatedWeapon.Name, result.Data.Name);
            Assert.AreEqual(updatedWeapon.Type, result.Data.Type);
            Assert.AreEqual(updatedWeapon.Price, result.Data.Price);
            Assert.AreEqual(updatedWeapon.Weight, result.Data.Weight);
        }

        [Test]
        public async Task UpdateWeapon_WithNonExistingWeapon_ReturnsError()
        {
            var nonExistingWeapon = new WeaponViewModel
            {
                Name = "WeaponNonExisting",
                Type = "TypeNonExisting",
                Price = 150.00m,
                Weight = 15.5f
            };
            var result = await _weaponService.Update(nonExistingWeapon);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task DeleteWeapon_WithExistingWeapon_DeletesWeapon()
        {
            var existingWeaponId = 1;
            var result = await _weaponService.Delete(existingWeaponId);
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual("Weapon deleted successfully", result.Description);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteWeapon_WithNonExistingWeapon_ReturnsError()
        {
            var nonExistingWeaponId = -1;
            var result = await _weaponService.Delete(nonExistingWeaponId);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.IsFalse(result.Data);
        }
    }
}
