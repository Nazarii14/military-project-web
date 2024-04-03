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
using MilitaryProject.Domain.ViewModels.MilitaryRoute;
using MilitaryProject.BLL.Interfaces;

namespace MilitaryRouteUnitTests
{
    [TestFixture]
    public class MilitaryRouteUnitTests
    {
        private MilitaryRouteService _militaryRouteService;
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

            _dbContext.AddRange(new List<User>
            {
                new User { ID = 1, Name = "User1", Lastname = "LastName1", Password = "Pass1", Email = "email1@gmail.com", Age = 22, Role = Role.Volunteer},
                new User { ID = 2, Name = "User2", Lastname = "LastName2", Password = "Pass2", Email = "email2@gmail.com", Age = 23, Role = Role.Volunteer},
                new User { ID = 3, Name = "User3", Lastname = "LastName3", Password = "Pass3", Email = "email3@gmail.com", Age = 24, Role = Role.Volunteer}
            });

            _dbContext.AddRange(new List<MilitaryRoute>
            {
                new MilitaryRoute { ID = 1, VolunteerID = 1, WeaponID = 1, WeaponQuantity = 10,AmmunitionID = 1, AmmunitionQuantity = 5, StartPoint = "Point1", Destination = "DPoint1", DeliveryDate = DateTime.Today},
                new MilitaryRoute { ID = 2, VolunteerID = 2, WeaponID = 2, WeaponQuantity = 20,AmmunitionID = 2, AmmunitionQuantity = 10, StartPoint = "Point2", Destination = "DPoint2", DeliveryDate = DateTime.Today},
                new MilitaryRoute { ID = 3, VolunteerID = 3, WeaponID = 3, WeaponQuantity = 30,AmmunitionID = 3, AmmunitionQuantity = 15, StartPoint = "Point3", Destination = "DPoint3", DeliveryDate = DateTime.Today}
            });

            _dbContext.SaveChanges();
            _militaryRouteService = new MilitaryRouteService(new MilitaryRouteRepository(_dbContext), new UserRepository(_dbContext), new WeaponRepository(_dbContext), new AmmunitionRepository(_dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetMilitaryRoute_WhenMilitaryRouteExists_ReturnsMilitaryRoute()
        {
            int existingMilitaryRouteID = 1;
            var weapon = _dbContext.Weapons.Find(1);
            var ammunition = _dbContext.Ammunitions.Find(1);
            var volunteer = _dbContext.Users.Find(1);
            var expectedResult = new MilitaryRoute
            {
                ID = 1,
                VolunteerID = 1,
                WeaponID = 1,
                WeaponQuantity = 10,
                AmmunitionID = 1,
                AmmunitionQuantity = 5,
                StartPoint = "Point1",
                Destination = "DPoint1",
                DeliveryDate = DateTime.Today,
                Weapon = weapon,
                Ammunition = ammunition,
                Volunteer = volunteer
            };
            var result = await _militaryRouteService.GetMilitaryRoute(existingMilitaryRouteID);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedResult.ID, result.Data.ID);
            Assert.AreEqual(expectedResult.VolunteerID, result.Data.VolunteerID);
            Assert.AreEqual(expectedResult.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(expectedResult.WeaponQuantity, result.Data.WeaponQuantity);
            Assert.AreEqual(expectedResult.AmmunitionID, result.Data.AmmunitionID);
            Assert.AreEqual(expectedResult.AmmunitionQuantity, result.Data.AmmunitionQuantity);
            Assert.AreEqual(expectedResult.StartPoint, result.Data.StartPoint);
            Assert.AreEqual(expectedResult.Destination, result.Data.Destination);
            Assert.AreEqual(expectedResult.DeliveryDate, result.Data.DeliveryDate);
            Assert.AreEqual(expectedResult.Weapon, result.Data.Weapon);
            Assert.AreEqual(expectedResult.Ammunition, result.Data.Ammunition);
            Assert.AreEqual(expectedResult.Volunteer, result.Data.Volunteer);
        }

        [Test]
        public async Task GetMilitaryRoute_WhenMilitaryRouteDoesNotExists_ReturnsMilitaryRouteNotFound()
        {
            int nonExistingMilitaryRouteID = 100;
            var result = await _militaryRouteService.GetMilitaryRoute(nonExistingMilitaryRouteID);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task GetMilitaryRoutes_WhenMilitaryRoutesExists_ReturnsMilitaryRoutes()
        {
            var expectedResult = new List<MilitaryRoute>
            {
                new MilitaryRoute
                {
                    ID = 1,
                    VolunteerID = 1,
                    WeaponID = 1,
                    WeaponQuantity = 10,
                    AmmunitionID = 1,
                    AmmunitionQuantity = 5,
                    StartPoint = "Point1",
                    Destination = "DPoint1",
                    DeliveryDate = DateTime.Today,
                    Weapon = _dbContext.Weapons.Find(1),
                    Ammunition = _dbContext.Ammunitions.Find(1),
                    Volunteer = _dbContext.Users.Find(1)
                },
                new MilitaryRoute
                {
                    ID = 2,
                    VolunteerID = 2,
                    WeaponID = 2,
                    WeaponQuantity = 20,
                    AmmunitionID = 2,
                    AmmunitionQuantity = 10,
                    StartPoint = "Point2",
                    Destination = "DPoint2",
                    DeliveryDate = DateTime.Today,
                    Weapon = _dbContext.Weapons.Find(2),
                    Ammunition = _dbContext.Ammunitions.Find(2),
                    Volunteer = _dbContext.Users.Find(2)
                },
                new MilitaryRoute
                {
                    ID = 3,
                    VolunteerID = 3,
                    WeaponID = 3,
                    WeaponQuantity = 30,
                    AmmunitionID = 3,
                    AmmunitionQuantity = 15,
                    StartPoint = "Point3",
                    Destination = "DPoint3",
                    DeliveryDate = DateTime.Today,
                    Weapon = _dbContext.Weapons.Find(3),
                    Ammunition = _dbContext.Ammunitions.Find(3),
                    Volunteer = _dbContext.Users.Find(3)
                }
            };
            var result = await _militaryRouteService.GetMilitaryRoutes();

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedResult.Count, result.Data.Count());
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, result.Data.ElementAt(i).ID);
                Assert.AreEqual(expectedResult[i].VolunteerID, result.Data.ElementAt(i).VolunteerID);
                Assert.AreEqual(expectedResult[i].WeaponID, result.Data.ElementAt(i).WeaponID);
                Assert.AreEqual(expectedResult[i].WeaponQuantity, result.Data.ElementAt(i).WeaponQuantity);
                Assert.AreEqual(expectedResult[i].AmmunitionQuantity, result.Data.ElementAt(i).AmmunitionQuantity);
            }
        }

        [Test]
        public async Task GetMilitaryRoutes_WhenNoMilitaryRoutesExist_ReturnsEmptyList()
        {
            _dbContext.RemoveRange(_dbContext.MilitaryRoutes);
            _dbContext.SaveChanges();
            var result = await _militaryRouteService.GetMilitaryRoutes();

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Count());
        }

        [Test]
        public async Task AddMilitaryRoute_ReturnsMilitaryRouteAdded()
        {
            var militaryRouteViewModel = new CreateMilitaryRouteViewModel
            {
                VolunteerID = 1,
                WeaponID = 1,
                WeaponQuantity = 10,
                AmmunitionID = 1,
                AmmunitionQuantity = 5,
                StartPoint = "Point1",
                Destination = "DPoint1",
                DeliveryDate = DateTime.Today,
                WeaponName = "Weapon1",
                AmmunitionName = "Ammunition1",
                VolunteerLastName = "LastName1"
            };
            var result = await _militaryRouteService.Create(militaryRouteViewModel);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(militaryRouteViewModel.VolunteerID, result.Data.VolunteerID);
            Assert.AreEqual(militaryRouteViewModel.WeaponID, result.Data.WeaponID);
            Assert.AreEqual(militaryRouteViewModel.WeaponQuantity, result.Data.WeaponQuantity);
            Assert.AreEqual(militaryRouteViewModel.AmmunitionID, result.Data.AmmunitionID);
            Assert.AreEqual(militaryRouteViewModel.AmmunitionQuantity, result.Data.AmmunitionQuantity);
            Assert.AreEqual(militaryRouteViewModel.StartPoint, result.Data.StartPoint);
            Assert.AreEqual(militaryRouteViewModel.Destination, result.Data.Destination);
            Assert.AreEqual(militaryRouteViewModel.DeliveryDate, result.Data.DeliveryDate);
            Assert.AreEqual(militaryRouteViewModel.WeaponName, result.Data.Weapon.Name);
            Assert.AreEqual(militaryRouteViewModel.AmmunitionName, result.Data.Ammunition.Name);
            Assert.AreEqual(militaryRouteViewModel.VolunteerLastName, result.Data.Volunteer.Lastname);
        }

        [Test]
        public async Task AddMilitaryRoute_WhenVolunteerDoesNotExist_ReturnsVolunteerNotFound()
        {
            var militaryRouteViewModel = new CreateMilitaryRouteViewModel
            {
                VolunteerID = 100,
                WeaponID = 1,
                WeaponQuantity = 10,
                AmmunitionID = 1,
                AmmunitionQuantity = 5,
                StartPoint = "Point1",
                Destination = "DPoint1",
                DeliveryDate = DateTime.Today,
                WeaponName = "Weapon1",
                AmmunitionName = "Ammunition1",
                VolunteerLastName = "NonExistingLastName"
            };
            var result = await _militaryRouteService.Create(militaryRouteViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Volunteer does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task AddMilitaryRoute_WhenWeaponDoesNotExist_ReturnsWeaponNotFound()
        {
            var militaryRouteViewModel = new CreateMilitaryRouteViewModel
            {
                VolunteerID = 1,
                WeaponID = 100,
                WeaponQuantity = 10,
                AmmunitionID = 1,
                AmmunitionQuantity = 5,
                StartPoint = "Point1",
                Destination = "DPoint1",
                DeliveryDate = DateTime.Today,
                WeaponName = "NonExistingWeapon",
                AmmunitionName = "Ammunition1",
                VolunteerLastName = "LastName1"
            };
            var result = await _militaryRouteService.Create(militaryRouteViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Weapon does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task AddMilitaryRoute_WhenAmmunitionDoesNotExist_ReturnsAmmunitionNotFound()
        {
            var militaryRouteViewModel = new CreateMilitaryRouteViewModel
            {
                VolunteerID = 1,
                WeaponID = 1,
                WeaponQuantity = 10,
                AmmunitionID = 100,
                AmmunitionQuantity = 5,
                StartPoint = "Point1",
                Destination = "DPoint1",
                DeliveryDate = DateTime.Today,
                WeaponName = "Weapon1",
                AmmunitionName = "NonExistingAmmunition",
                VolunteerLastName = "LastName1"
            };
            var result = await _militaryRouteService.Create(militaryRouteViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Ammunition does not exist", result.Description);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task UpdateMilitaryRoute_WhenMilitaryRouteExists_ReturnsMilitaryRouteUpdated()
        {
            int existingMilitaryRouteID = 1;
            var militaryRouteViewModel = new EditMilitaryRouteViewModel
            {
                ID = 1,
                WeaponQuantity = 10,
                AmmunitionQuantity = 5,
                StartPoint = "Point1",
                Destination = "DPoint1",
                DeliveryDate = DateTime.Today,
            };
            var result = await _militaryRouteService.Update(militaryRouteViewModel);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(militaryRouteViewModel.ID, result.Data.ID);
            Assert.AreEqual(militaryRouteViewModel.WeaponQuantity, result.Data.WeaponQuantity);
            Assert.AreEqual(militaryRouteViewModel.AmmunitionQuantity, result.Data.AmmunitionQuantity);
            Assert.AreEqual(militaryRouteViewModel.StartPoint, result.Data.StartPoint);
            Assert.AreEqual(militaryRouteViewModel.Destination, result.Data.Destination);
            Assert.AreEqual(militaryRouteViewModel.DeliveryDate, result.Data.DeliveryDate);
        }

        [Test]
        public async Task UpdateMilitaryRoute_WhenMilitaryRouteDoesNotExists_ReturnsMilitaryRouteNotFound()
        {
            int nonExistingMilitaryRouteID = 100;
            var militaryRouteViewModel = new EditMilitaryRouteViewModel
            {
                ID = 100,
                WeaponQuantity = 10,
                AmmunitionQuantity = 5,
                StartPoint = "Point1",
                Destination = "DPoint1",
                DeliveryDate = DateTime.Today,
            };
            var result = await _militaryRouteService.Update(militaryRouteViewModel);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task DeleteMilitaryRoute_WhenMilitaryRouteExists_ReturnsMilitaryRouteDeleted()
        {
            int existingMilitaryRouteID = 1;
            var result = await _militaryRouteService.Delete(existingMilitaryRouteID);

            Assert.AreEqual(StatusCode.OK, result.StatusCode);
            Assert.AreEqual(true, result.Data);
        }

        [Test]
        public async Task DeleteMilitaryRoute_WhenMilitaryRouteDoesNotExists_ReturnsMilitaryRouteNotFound()
        {
            int nonExistingMilitaryRouteID = 100;
            var result = await _militaryRouteService.Delete(nonExistingMilitaryRouteID);

            Assert.AreEqual(StatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(false, result.Data);
        }
    }
}
