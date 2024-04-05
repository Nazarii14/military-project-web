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
using MilitaryProject.Domain.ViewModels.UserItems;
using MilitaryProject.BLL.Interfaces;

namespace UserItemsUnitTests
{
    public class UserItemsUnitTests
    {
        private UserItemsService _userItemsService;
        private ApplicationDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDatabase")
              .Options;

            _dbContext = new ApplicationDbContext(options);

            _dbContext.AddRange(new List<User>
            {
                new User { ID = 1, Name = "User1", Lastname = "LastName1", Password = "Pass1", Email = "email1@gmail.com", Age = 22, Role = Role.Volunteer},
                new User { ID = 2, Name = "User2", Lastname = "LastName2", Password = "Pass2", Email = "email2@gmail.com", Age = 23, Role = Role.Volunteer},
                new User { ID = 3, Name = "User3", Lastname = "LastName3", Password = "Pass3", Email = "email3@gmail.com", Age = 24, Role = Role.Volunteer}
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

            _dbContext.AddRange(new List<UserItems>
            {
                new UserItems { ID = 1, UserID = 1, WeaponID = 1, AmmunitionID = 1 },
                new UserItems { ID = 2, UserID = 2, WeaponID = 2, AmmunitionID = 2 },
                new UserItems { ID = 3, UserID = 3, WeaponID = 3, AmmunitionID = 3 }
            });

            _dbContext.SaveChanges();
            _userItemsService = new UserItemsService(new UserItemsRepository(_dbContext), new UserRepository(_dbContext), new WeaponRepository(_dbContext), new AmmunitionRepository(_dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetUserItem_WhenExists_ReturnUserItem()
        {
            int existingUserItemId = 1;
            var expectedUserItem = new UserItems { ID = 1, UserID = 1, WeaponID = 1, AmmunitionID = 1 };

            var result = await _userItemsService.GetUserItem(existingUserItemId);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedUserItem.ID, result.Data.ID);
            Assert.AreEqual(expectedUserItem.UserID, result.Data.UserID);
            Assert.AreEqual(expectedUserItem.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(expectedUserItem.AmmunitionID, result.Data.AmmunitionID);
        }

        [Test]
        public async Task GetUserItem_WhenNotExists_ReturnsNotFound()
        {
            int notExistingUserItemId = -1;

            var result = await _userItemsService.GetUserItem(notExistingUserItemId);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task GetUserItems_ReturnsUserItemsList()
        {
            var expectedUserItems = new List<UserItems>
            {
                new UserItems { ID = 1, UserID = 1, WeaponID = 1, AmmunitionID = 1 },
                new UserItems { ID = 2, UserID = 2, WeaponID = 2, AmmunitionID = 2 },
                new UserItems { ID = 3, UserID = 3, WeaponID = 3, AmmunitionID = 3 }
            };

            var result = await _userItemsService.GetUserItems();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedUserItems.Count, result.Data.Count);
            for (int i = 0; i < expectedUserItems.Count; i++)
            {
                Assert.AreEqual(expectedUserItems[i].ID, result.Data[i].ID);
                Assert.AreEqual(expectedUserItems[i].UserID, result.Data[i].UserID);
                Assert.AreEqual(expectedUserItems[i].WeaponID, result.Data[i].WeaponID);
                Assert.AreEqual(expectedUserItems[i].AmmunitionID, result.Data[i].AmmunitionID);
            }
        }

        [Test]
        public async Task GetUserItems_WhenNotExist_ReturnsEmptyList()
        {
            _dbContext.RemoveRange(_dbContext.UserItems);
            _dbContext.SaveChanges();
            var result = await _userItemsService.GetUserItems();
            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Count);
        }

        [Test]
        public async Task CreateUserItem_WhenUserItemDoesNotExist_ReturnsUserItem()
        {
            var userItem = new UserItemsViewModel { ID = 4, UserID = 1, WeaponID = 2, AmmunitionID = 3 };

            var result = await _userItemsService.Create(userItem);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(userItem.ID, result.Data.ID);
            Assert.AreEqual(userItem.UserID, result.Data.UserID);
            Assert.AreEqual(userItem.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(userItem.AmmunitionID, result.Data.AmmunitionID);
        }

        [Test]
        public async Task CreateUserItem_WhenUserItemExists_ReturnsError()
        {
            var existingUserItem = new UserItemsViewModel { ID = 1, UserID = 1, WeaponID = 1, AmmunitionID = 1 };

            var result = await _userItemsService.Create(existingUserItem);

            Assert.AreEqual(StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("User item already exists", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task CreateUserItem_WhenUserDoesNotExist_ReturnsError()
        {
            var toAdd = new UserItemsViewModel { UserID = -1, WeaponID = 1, AmmunitionID = 1 };
            var result = await _userItemsService.Create(toAdd);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("User does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task CreateUserItem_WhenWeaponDoesNotExist_ReturnsError()
        {
            var toAdd = new UserItemsViewModel { UserID = 1, WeaponID = -1, AmmunitionID = 1 };
            var result = await _userItemsService.Create(toAdd);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task CreateUserItem_WhenAmmunitionDoesNotExist_ReturnsError()
        {
            var toAdd = new UserItemsViewModel { UserID = 1, WeaponID = 1, AmmunitionID = -1 };
            var result = await _userItemsService.Create(toAdd);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateUserItem_WhenUserItemExists_ReturnsUserItem()
        {
            var existingUserItem = new UserItemsViewModel { ID = 1, UserID = 2, WeaponID = 2, AmmunitionID = 1 };

            var result = await _userItemsService.Update(existingUserItem);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(existingUserItem.ID, result.Data.ID);
            Assert.AreEqual(existingUserItem.UserID, result.Data.UserID);
            Assert.AreEqual(existingUserItem.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(existingUserItem.AmmunitionID, result.Data.AmmunitionID);
        }

        [Test]
        public async Task UpdateUserItem_WhenUserItemDoesNotExist_ReturnsError()
        {
            var notExistingUserItem = new UserItemsViewModel { ID = -1, UserID = 1, WeaponID = 1, AmmunitionID = 1 };

            var result = await _userItemsService.Update(notExistingUserItem);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("User item does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateUserItem_WhenUserDoesNotExist_ReturnsError()
        {
            var toUpdate = new UserItemsViewModel { ID = 1, UserID = -1, WeaponID = 1, AmmunitionID = 1 };
            var result = await _userItemsService.Update(toUpdate);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("User does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateUserItem_WhenWeaponDoesNotExist_ReturnsError()
        {
            var toUpdate = new UserItemsViewModel { ID = 1, UserID = 1, WeaponID = -1, AmmunitionID = 1 };
            var result = await _userItemsService.Update(toUpdate);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateUserItem_WhenAmmunitionDoesNotExist_ReturnsError()
        {
            var toUpdate = new UserItemsViewModel { ID = 1, UserID = 1, WeaponID = 1, AmmunitionID = -1 };
            var result = await _userItemsService.Update(toUpdate);
            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task DeleteUserItem_WhenUserItemExists_ReturnsUserItem()
        {
            int existingUserItemId = 1;

            var result = await _userItemsService.Delete(existingUserItemId);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteUserItem_WhenUserItemDoesNotExist_ReturnsError()
        {
            int notExistingUserItemId = -1;

            var result = await _userItemsService.Delete(notExistingUserItemId);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.IsFalse(result.Data);
        }
    }
}
