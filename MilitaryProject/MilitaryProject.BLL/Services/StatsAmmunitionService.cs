using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class StatsAmmunitionService : IStatsAmmunitionService
    {
        private readonly BaseRepository<Ammunition> _ammunitionRepository;
        private readonly BaseRepository<User> _userRepository;
        private readonly BaseRepository<BrigadeStorage> _brigadeStorageRepository;

        public StatsAmmunitionService(BaseRepository<Ammunition> ammunitionRepository, BaseRepository<User> userRepository, BaseRepository<BrigadeStorage> brigadeStorageRepository)
        {
            _ammunitionRepository = ammunitionRepository;
            _userRepository = userRepository;
            _brigadeStorageRepository = brigadeStorageRepository;
        }

        public async Task<BaseResponse<List<StatsAmmunitionViewModel>>> GetAll(int userID)
        {
            var user = await _userRepository.Getbyid(userID);
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();

            List<StatsAmmunitionViewModel> statsAmmunitionViewModels = new List<StatsAmmunitionViewModel>();
            for (int i = 0; i < brigadeStorage.Count; i++)
            {
                int ammunitionID = brigadeStorage[i].AmmunitionID;
                var ammunition = await _ammunitionRepository.Getbyid(ammunitionID);

                if (ammunition != null)
                {
                    StatsAmmunitionViewModel statsAmmunitionViewModel = new StatsAmmunitionViewModel
                    {
                        AmmunitionID = ammunitionID,
                        UserID = userID,
                        AmmunitionName = ammunition.Name,
                        AmmunitionType = ammunition.Type,
                        AmmunitionSize = ammunition.Size,
                        AmmunitionPrice = ammunition.Price,
                        AmmunitionCount = brigadeStorage[i].AmmunitionRemainder,
                        NeededAmmunitionCount = brigadeStorage[i].AmmunitionQuantity,
                    };
                    statsAmmunitionViewModels.Add(statsAmmunitionViewModel);
                }
            }

            return new BaseResponse<List<StatsAmmunitionViewModel>>
            {
                Data = statsAmmunitionViewModels,
                Description = "Got ammunitions successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<StatsAmmunitionViewModel>> Create(StatsAmmunitionViewModel model)
        {
            var user = await _userRepository.Getbyid(model.UserID);
            if (user == null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "User does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();

            var ammunition = (await _ammunitionRepository.GetAll()).FirstOrDefault(a => a.Name.Equals(model.AmmunitionName, StringComparison.OrdinalIgnoreCase));

            if (ammunition == null)
            {
                ammunition = new Ammunition
                {
                    Name = model.AmmunitionName,
                    Type = model.AmmunitionType,
                    Price = model.AmmunitionPrice,
                    Size = model.AmmunitionSize,
                };
                await _ammunitionRepository.Create(ammunition);
            }
            else
            {
                ammunition.Type = model.AmmunitionType;
                ammunition.Price = model.AmmunitionPrice;
                ammunition.Size = model.AmmunitionSize;
                await _ammunitionRepository.Update(ammunition);
            }

            var ammunitionID = ammunition.ID;
            var existingAmmunitionInStorage = brigadeStorage.FirstOrDefault(a => a.AmmunitionID == ammunitionID);

            if (existingAmmunitionInStorage != null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "Ammunition already exists in the storage.",
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }

            var newBrigadeStorage = new BrigadeStorage
            {
                BrigadeID = brigadeID,
                AmmunitionID = ammunitionID,
                AmmunitionQuantity = model.NeededAmmunitionCount,
                AmmunitionRemainder = model.AmmunitionCount,
                Brigade = user.Brigade,
                Ammunition = ammunition,
            };

            await _brigadeStorageRepository.Create(newBrigadeStorage);

            return new BaseResponse<StatsAmmunitionViewModel>
            {
                Data = model,
                Description = "Ammunition added to the storage successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<StatsAmmunitionViewModel>> Update(StatsAmmunitionViewModel model)
        {
            var user = await _userRepository.Getbyid(model.UserID);
            if (user == null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "User does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();
            var ammunition = await _ammunitionRepository.Getbyid(model.AmmunitionID);

            if (ammunition == null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "Ammunition does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            var existingAmmunitionInStorage = brigadeStorage.FirstOrDefault(a => a.AmmunitionID == model.AmmunitionID);
            if (existingAmmunitionInStorage == null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "Ammunition does not exist in the storage.",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            ammunition.Name = model.AmmunitionName;
            ammunition.Type = model.AmmunitionType;
            ammunition.Price = model.AmmunitionPrice;
            ammunition.Size = model.AmmunitionSize;
            await _ammunitionRepository.Update(ammunition);

            var storage = existingAmmunitionInStorage;
            storage.AmmunitionQuantity = model.NeededAmmunitionCount;
            storage.AmmunitionRemainder = model.AmmunitionCount;
            await _brigadeStorageRepository.Update(storage);

            return new BaseResponse<StatsAmmunitionViewModel>
            {
                Data = model,
                Description = "Ammunition updated successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<bool>> Delete(int userID, int ammunitionID)
        {
            var user = await _userRepository.Getbyid(userID);
            if (user == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "User does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.Where(b => b.BrigadeID == brigadeID).ToList();
            var ammunition = await _ammunitionRepository.Getbyid(ammunitionID);

            if (ammunition == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "Ammunition does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            var storage = brigadeStorage.FirstOrDefault(a => a.AmmunitionID == ammunitionID && a.BrigadeID == brigadeID);
            if (storage == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "Ammunition does not exist in the storage.",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            await _brigadeStorageRepository.Delete(storage);
            return new BaseResponse<bool>
            {
                Data = true,
                Description = "Ammunition deleted successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<StatsAmmunitionViewModel>> GetById(int userID, int ammunitionID)
        {
            var user = await _userRepository.Getbyid(userID);
            if (user == null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "User does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }
            int brigadeID = user.BrigadeID;
            var storages = await _brigadeStorageRepository.GetAll();
            var brigadeStorage = storages.FirstOrDefault(b => b.BrigadeID == brigadeID && b.AmmunitionID == ammunitionID);
            var ammunition = await _ammunitionRepository.Getbyid(ammunitionID);

            if (ammunition == null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "Ammunition does not exist",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            if (brigadeStorage == null)
            {
                return new BaseResponse<StatsAmmunitionViewModel>
                {
                    Data = null,
                    Description = "Ammunition does not exist in the storage.",
                    StatusCode = Domain.Enum.StatusCode.NotFound
                };
            }

            StatsAmmunitionViewModel statsAmmunitionViewModel = new StatsAmmunitionViewModel
            {
                AmmunitionID = ammunitionID,
                UserID = userID,
                AmmunitionName = ammunition.Name,
                AmmunitionType = ammunition.Type,
                AmmunitionSize = ammunition.Size,
                AmmunitionPrice = ammunition.Price,
                AmmunitionCount = brigadeStorage.AmmunitionRemainder,
                NeededAmmunitionCount = brigadeStorage.AmmunitionQuantity,
            };

            return new BaseResponse<StatsAmmunitionViewModel>
            {
                Data = statsAmmunitionViewModel,
                Description = "Got ammunition successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }
    }
}
