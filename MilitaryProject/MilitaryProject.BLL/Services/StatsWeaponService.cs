using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class StatsWeaponService : IStatsWeaponService
    {
        private readonly BaseRepository<Weapon> _weaponRepository;
        private readonly BaseRepository<User> _userRepository;
        private readonly BaseRepository<BrigadeStorage> _brigadeStorageRepository;

        public StatsWeaponService(BaseRepository<Weapon> weaponRepository, BaseRepository<User> userRepository, BaseRepository<BrigadeStorage> brigadeStorageRepository)
        {
            _brigadeStorageRepository = brigadeStorageRepository;
            _userRepository = userRepository;
            _weaponRepository = weaponRepository;
        }

        public async Task<BaseResponse<List<StatsWeaponViewModel>>> GetAll(int userID)
        {
            var user = await _userRepository.Getbyid(userID);
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();

            List<StatsWeaponViewModel> statsWeaponViewModels = new List<StatsWeaponViewModel>();
            for (int i = 0; i < brigadeStorage.Count; i++)
            {
                int weaponID = brigadeStorage[i].WeaponID;
                var weapon = await _weaponRepository.Getbyid(weaponID);
                StatsWeaponViewModel statsWeaponViewModel = new StatsWeaponViewModel
                {
                    WeaponID = weaponID,
                    UserID = userID,
                    WeaponName = weapon.Name,
                    WeaponType = weapon.Type,
                    WeaponWeight = weapon.Weight,
                    WeaponPrice = weapon.Price,
                    WeaponCount = brigadeStorage[i].WeaponRemainder,
                    NeededWeaponCount = brigadeStorage[i].WeaponQuantity,
                };
                statsWeaponViewModels.Add(statsWeaponViewModel);
            }

            return new BaseResponse<List<StatsWeaponViewModel>>
            {
                Data = statsWeaponViewModels,
                Description = "Got weapons successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<StatsWeaponViewModel>> Create(StatsWeaponViewModel model)
        {
            var user = await _userRepository.Getbyid(model.UserID);
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();
            var weapon = (await _weaponRepository.GetAll()).FirstOrDefault(w => w.Name.Equals(model.WeaponName, StringComparison.OrdinalIgnoreCase));
            
            if (weapon == null)
            {
                weapon = new Weapon
                {
                    Name = model.WeaponName,
                    Type = model.WeaponType,
                    Price = model.WeaponPrice,
                    Weight = model.WeaponWeight,
                };
                await _weaponRepository.Create(weapon);
            }
            else
            {
                weapon.Type = model.WeaponType;
                weapon.Price = model.WeaponPrice;
                weapon.Weight = model.WeaponWeight;
                await _weaponRepository.Update(weapon);
            }
            var weaponID = weapon.ID;

            var existingWeaponInStoage = brigadeStorage.FirstOrDefault(w => w.WeaponID == weaponID);
            if (existingWeaponInStoage != null)
            {
                return new BaseResponse<StatsWeaponViewModel>
                {
                    Data = null,
                    Description = "Weapon already exists in the storage",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
            
            var newBrigadeStorage = new BrigadeStorage
            {
                BrigadeID = brigadeID,
                WeaponID = weaponID,
                WeaponQuantity = model.NeededWeaponCount,
                WeaponRemainder = model.WeaponCount,
                Brigade = user.Brigade,
                Weapon = weapon,
            };

            await _brigadeStorageRepository.Create(newBrigadeStorage);

            return new BaseResponse<StatsWeaponViewModel>
            {
                Data = model,
                Description = "Weapon added to the storage successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<StatsWeaponViewModel>> Update(StatsWeaponViewModel model)
        {
            var user = await _userRepository.Getbyid(model.UserID);
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();
            var weapon = await _weaponRepository.Getbyid(model.WeaponID);

            if (weapon == null)
            {
                return new BaseResponse<StatsWeaponViewModel>
                {
                    Data = null,
                    Description = "Weapon does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            var weaponID = weapon.ID;
            var existingWeaponInStoage = brigadeStorage.FirstOrDefault(w => w.WeaponID == weaponID);
            if (existingWeaponInStoage == null)
            {
                return new BaseResponse<StatsWeaponViewModel>
                {
                    Data = null,
                    Description = "Weapon does not exist in the storage",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            weapon.Name = model.WeaponName;
            weapon.Type = model.WeaponType;
            weapon.Price = model.WeaponPrice;
            weapon.Weight = model.WeaponWeight;
            await _weaponRepository.Update(weapon);

            var storage = brigadeStorage.FirstOrDefault(w => w.WeaponID == weaponID);
            storage.WeaponQuantity = model.NeededWeaponCount;
            storage.WeaponRemainder = model.WeaponCount;
            await _brigadeStorageRepository.Update(storage);

            return new BaseResponse<StatsWeaponViewModel>
            {
                Data = model,
                Description = "Weapon updated successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<bool>> Delete(int userID, int weaponID)
        {
            var user = await _userRepository.Getbyid(userID);
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();
            var weapon = await _weaponRepository.Getbyid(weaponID);

            if (weapon == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "Weapon does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            var storage = brigadeStorage.FirstOrDefault(w => w.WeaponID == weaponID && w.BrigadeID == brigadeID);
            if (storage == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "Weapon does not exist in the storage.",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            await _brigadeStorageRepository.Delete(storage);
            return new BaseResponse<bool>
            {
                Data = true,
                Description = "Weapon deleted successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<StatsWeaponViewModel>> GetById(int userID, int weaponID)
        {
            var user = await _userRepository.Getbyid(userID);
            if (user == null)
            {
                return new BaseResponse<StatsWeaponViewModel>
                {
                    Data = null,
                    Description = "User does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.FirstOrDefault(b => b.BrigadeID == brigadeID && b.WeaponID == weaponID);
            var weapon = await _weaponRepository.Getbyid(weaponID);

            if (weapon == null)
            {
                return new BaseResponse<StatsWeaponViewModel>
                {
                    Data = null,
                    Description = "Weapon does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            if (brigadeStorage == null)
            {
                return new BaseResponse<StatsWeaponViewModel>
                {
                    Data = null,
                    Description = "Weapon does not exist in the storage.",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            StatsWeaponViewModel statsWeaponViewModel = new StatsWeaponViewModel
            {
                WeaponID = weaponID,
                UserID = userID,
                WeaponName = weapon.Name,
                WeaponType = weapon.Type,
                WeaponWeight = weapon.Weight,
                WeaponPrice = weapon.Price,
                WeaponCount = brigadeStorage.WeaponRemainder,
                NeededWeaponCount = brigadeStorage.WeaponQuantity,
            };

            return new BaseResponse<StatsWeaponViewModel>
            {
                Data = statsWeaponViewModel,
                Description = "Got weapon successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }
    }
}
