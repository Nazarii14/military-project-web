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

            var result = await _weaponService.GetWeapon(existingWeaponId);

            Assert.AreEqual(Domain.Enum.StatusCode.OK, result.StatusCode);
            Assert.AreEqual(existingWeapon, result.Data);
        }
    }
}
