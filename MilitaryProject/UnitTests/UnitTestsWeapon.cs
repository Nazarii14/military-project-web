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

namespace MilitaryProject.UnitTest
{
    [TestFixture]
    public class UnitTestWeapon
    {
        private Mock<BaseRepository<Weapon>> _mockRepository;
        private WeaponService _weaponService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<BaseRepository<Weapon>>();
            _weaponService = new WeaponService(_mockRepository.Object);
        }

        [Test]
        public async Task GetWeapon_WhenWeaponExists_ReturnsWeapon()
        {
            int existingWeaponId = 1;
            var existingWeapon = new Weapon { ID = existingWeaponId };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon> { existingWeapon });

            var result = await _weaponService.GetById(existingWeaponId);

            Assert.AreEqual(Domain.Enum.StatusCode.OK, result.StatusCode);
            Assert.AreEqual(existingWeapon, result.Data);
        }
        [Test]
        public async Task GetWeapon_WhenWeaponDoesNotExist_ReturnsNotFound()
        {
            int nonExistingWeaponId = -1; 
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon>());

            var result = await _weaponService.GetById(nonExistingWeaponId);

            Assert.AreEqual(StatusCode.NotFount, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task GetWeapons_WhenWeaponsExist_ReturnsWeaponsList()
        {
            var weaponsList = new List<Weapon> { new Weapon(), new Weapon(), new Weapon() };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(weaponsList);

            var result = await _weaponService.GetAll();

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(weaponsList, result.Data);
        }

        [Test]
        public async Task GetWeapons_WhenNoWeaponsExist_ReturnsEmptyList()
        {
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon>());

            var result = await _weaponService.GetAll();

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsEmpty(result.Data);
        }

        [Test]
        public async Task CreateWeapon_WithValidData_CreatesNewWeapon()
        {
            var newWeaponViewModel = new WeaponViewModel 
            {
               Name = "Test",
               Type = "Type",
               Price = 100.00m,
               Weight = 10.5f
            };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon>());

            var result = await _weaponService.Create(newWeaponViewModel);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task CreateWeapon_WithDuplicateName_ReturnsError()
        {
            var existingWeapon = new Weapon 
            {
                Name = "Test",
                Type = "Type",
                Price = 100,
            };
            var newWeaponViewModel = new WeaponViewModel { Name = existingWeapon.Name };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon> { existingWeapon });

            var result = await _weaponService.Create(newWeaponViewModel);

            Assert.AreEqual(StatusCode.InternalServerError, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateWeapon_WithValidData_UpdatesExistingWeapon()
        {
            var existingWeapon = new Weapon
            { 
                Name = "Test",
                Type = "Type",
                Price = 100,
                Weight = 10
            };
            var updatedWeaponViewModel = new WeaponViewModel
            {
                Name = existingWeapon.Name,
                Type = existingWeapon.Type,
                Price = existingWeapon.Price,
                Weight = existingWeapon.Weight,
            };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon> { existingWeapon });

            var result = await _weaponService.Update(updatedWeaponViewModel);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task UpdateWeapon_WithNonExistingWeapon_ReturnsError()
        {
            var nonExistingWeaponId = -1;
            var updatedWeaponViewModel = new WeaponViewModel 
            {
                Name = "New Name",
                Type = "Type new",
                Price = 1000,
                Weight = 5
            };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon>());

            var result = await _weaponService.Update(updatedWeaponViewModel);

            Assert.AreEqual(StatusCode.NotFount, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task DeleteWeapon_WithExistingWeapon_DeletesWeapon()
        {
            var existingWeapon = new Weapon 
            {
                Name = "New Name",
                Type = "Type new",
                Price = 1000,
                Weight = 5
            };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon> { existingWeapon });

            var result = await _weaponService.Delete(existingWeapon.ID);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteWeapon_WithNonExistingWeapon_ReturnsError()
        {
            var nonExistingWeaponId = -1;
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Weapon>());

            var result = await _weaponService.Delete(nonExistingWeaponId);

            Assert.AreEqual(StatusCode.NotFount, result.StatusCode);
            Assert.IsFalse(result.Data);
        }
    }
}
