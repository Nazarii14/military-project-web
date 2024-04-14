using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.UserItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class UserItemsService : IUserItemsService
    {
        private readonly BaseRepository<UserItems> _userItemsRepository;
        private readonly BaseRepository<User> _userRepository;
        private readonly BaseRepository<Weapon> _weaponRepository;
        private readonly BaseRepository<Ammunition> _ammunitionRepository;

        public UserItemsService(BaseRepository<UserItems> userItemsRepository, BaseRepository<User> userRepository, BaseRepository<Weapon> weaponRepository, BaseRepository<Ammunition> ammunitionRepository)
        {
            _userItemsRepository = userItemsRepository;
            _userRepository = userRepository;
            _weaponRepository = weaponRepository;
            _ammunitionRepository = ammunitionRepository;
        }

        public async Task<BaseResponse<UserItems>> GetUserItem(int id)
        {
            var response = await _userItemsRepository.GetAll();
            var userItem = response.FirstOrDefault(u => u.ID == id);

            if (userItem == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "User item does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            return new BaseResponse<UserItems>
            {
                Data = userItem,
                StatusCode = StatusCode.OK
            };
        }

        public async Task<BaseResponse<List<UserItems>>> GetUserItems()
        {
            var response = await _userItemsRepository.GetAll();

            return new BaseResponse<List<UserItems>>
            {
                Data = response,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }

        public async Task<BaseResponse<UserItems>> Create(UserItemsViewModel model)
        {
            var response = await _userItemsRepository.GetAll();
            var existingUserItem = response.FirstOrDefault(u => u.UserID == model.UserID && u.WeaponID == model.WeaponID && u.AmmunitionID == model.AmmunitionID);

            if (existingUserItem != null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "User item already exists",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            var responseUser = await _userRepository.GetAll();
            var user = responseUser.FirstOrDefault(u => u.ID == model.UserID);
            if (user == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "User does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            var responseWeapon = await _weaponRepository.GetAll();
            var weapon = responseWeapon.FirstOrDefault(w => w.ID == model.WeaponID);
            if (weapon == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "Weapon does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            var responseAmmunition = await _ammunitionRepository.GetAll();
            var ammunition = responseAmmunition.FirstOrDefault(a => a.ID == model.AmmunitionID);
            if (ammunition == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "Ammunition does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            var newUserItem = new UserItems
            {
                UserID = model.UserID,
                WeaponID = model.WeaponID,
                AmmunitionID = model.AmmunitionID,
                User = user,
                Weapon = weapon,
                Ammunition = ammunition,
            };

            await _userItemsRepository.Create(newUserItem);

            return new BaseResponse<UserItems>
            {
                Data = newUserItem,
                StatusCode = StatusCode.OK
            };
        }


        public async Task<BaseResponse<UserItems>> Update(UserItemsViewModel model)
        {
            var response = await _userItemsRepository.GetAll();
            var existingUserItem = response.FirstOrDefault(u => u.ID == model.ID);

            if (existingUserItem == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "User item does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            var responseUser = await _userRepository.GetAll();
            var user = responseUser.FirstOrDefault(u => u.ID == model.UserID);
            if (user == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "User does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            var responseWeapon = await _weaponRepository.GetAll();
            var weapon = responseWeapon.FirstOrDefault(w => w.ID == model.WeaponID);
            if (weapon == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "Weapon does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            var responseAmmunition = await _ammunitionRepository.GetAll();
            var ammunition = responseAmmunition.FirstOrDefault(a => a.ID == model.AmmunitionID);
            if (ammunition == null)
            {
                return new BaseResponse<UserItems>
                {
                    Description = "Ammunition does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            existingUserItem.UserID = model.UserID;
            existingUserItem.WeaponID = model.WeaponID;
            existingUserItem.AmmunitionID = model.AmmunitionID;
            existingUserItem.User = user;
            existingUserItem.Weapon = weapon;
            existingUserItem.Ammunition = ammunition;

            await _userItemsRepository.Update(existingUserItem);

            return new BaseResponse<UserItems>
            {
                Data = existingUserItem,
                StatusCode = StatusCode.OK
            };
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var response = await _userItemsRepository.GetAll();
            var userItem = response.FirstOrDefault(u => u.ID == id);

            if (userItem == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "User item does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            await _userItemsRepository.Delete(userItem);

            return new BaseResponse<bool>
            {
                Data = true,
                StatusCode = StatusCode.OK
            };
        }
    }
}
