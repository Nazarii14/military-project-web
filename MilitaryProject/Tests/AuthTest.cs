using MilitaryProject.BLL.Services;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Helpers;
using MilitaryProject.Domain.ViewModels.User;
using Moq;
using System.Security.Claims;

namespace UnitTest
{
    [TestClass]
    public class AuthTest
    {
        [TestMethod]
        public async Task SighUpTest()
        {
            var mockRepository = new Mock<BaseRepository<User>>();
            var userService = new UserService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<User>());
        
            var signup = new SignupViewModel
            {
                Email = "test@gmail.com",
                Password = "TestPassword123",
                Name = "Test",
                Lastname = "Test",
                Age = 20
            };

            var response = await userService.SignUp(signup);
            var claims = response.Data.Claims.ToList();
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            Assert.AreEqual(StatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual("User added", response.Description);
            Assert.AreEqual("test@gmail.com", emailClaim);
            Assert.AreEqual("Guest", roleClaim);
        }

        [TestMethod]
        public async Task SighUpTest_ExistingUser()
        {
            var existingUser = new User
            {
                Email = "test@gmail.com",
                Password = HashPasswordHelper.HashPassword("TestPassword123"),
                Name = "Test",
                Lastname = "Test",
                Age = 20,
                Role = Role.Guest
            };

            var mockRepository = new Mock<BaseRepository<User>>();
            var userService = new UserService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<User> { existingUser });

            var signup = new SignupViewModel
            {
                Email = "test@gmail.com",
                Password = "TestPassword123",
                Name = "Test",
                Lastname = "Test",
                Age = 20
            };

            var response = await userService.SignUp(signup);

            Assert.AreEqual("User is already exist", response.Description);
        }      

        [TestMethod]
        public async Task LoginTest()
        {
            var existingUser = new User
            {
                Email = "test@gmail.com",
                Password = HashPasswordHelper.HashPassword("TestPassword123"),
                Name = "Test",
                Lastname = "Test",
                Age = 20,
                Role = Role.Guest
            };

            var mockRepository = new Mock<BaseRepository<User>>();
            var userService = new UserService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<User> { existingUser });           

            var login = new LoginViewModel
            {
                Email = "test@gmail.com",
                Password = "TestPassword123"
            };

            var response = await userService.Login(login);
            var claims = response.Data.Claims.ToList();
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            Assert.AreEqual(StatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual("User logined", response.Description);
            Assert.AreEqual("test@gmail.com", emailClaim);
            Assert.AreEqual("Guest", roleClaim);
        }

        [TestMethod]
        public async Task LoginTest_NonExistingUser()
        {
            var existingUser = new User
            {
                Email = "test@gmail.com",
                Password = HashPasswordHelper.HashPassword("TestPassword123"),
                Name = "Test",
                Lastname = "Test",
                Age = 20
            };

            var mockRepository = new Mock<BaseRepository<User>>();
            var userService = new UserService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<User> { existingUser });

            var login = new LoginViewModel
            {
                Email = "TEST@gmail.com",
                Password = "TestPassword123"
            };

            var response = await userService.Login(login);

            Assert.AreEqual("User does not exist", response.Description);
        }

        [TestMethod]
        public async Task LoginTest_IncorrectCredentials()
        {
            var existingUser = new User
            {
                Email = "test@gmail.com",
                Password = HashPasswordHelper.HashPassword("TestPassword123"),
                Name = "Test",
                Lastname = "Test",
                Age = 20
            };

            var mockRepository = new Mock<BaseRepository<User>>();
            var userService = new UserService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<User> { existingUser });

            var login = new LoginViewModel
            {
                Email = "test@gmail.com",
                Password = "Incorrect"
            };

            var response = await userService.Login(login);

            Assert.AreEqual("Invalid password", response.Description);
        }
    }
}