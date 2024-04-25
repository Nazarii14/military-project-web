using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.BrigadeStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class BrigadeStorageService : IBrigadeStorageService
    {
        private readonly BaseRepository<BrigadeStorage> _brigadeStorageRepository;

        public BrigadeStorageService(BaseRepository<BrigadeStorage> brigadeStorageRepository)
        {
            _brigadeStorageRepository = brigadeStorageRepository;
        }

        public async Task<BaseResponse<BrigadeStorage>> Create(BrigadeStorageViewModel model)
        {
            var brigadeStorage = new BrigadeStorage
            {
                // Initialize BrigadeStorage properties from ViewModel
                BrigadeID = model.BrigadeID,
                WeaponID = model.WeaponID,
                AmmunitionID = model.AmmunitionID,
                WeaponQuantity = model.WeaponQuantity,
                AmmunitionQuantity = model.AmmunitionQuantity,
                WeaponRemainder = model.WeaponRemainder,
                AmmunitionRemainder = model.AmmunitionRemainder
            };

            await _brigadeStorageRepository.Create(brigadeStorage);

            return new BaseResponse<BrigadeStorage>
            {
                Data = brigadeStorage,
                Description = "BrigadeStorage created successfully.",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<BrigadeStorage>> GetById(int id)
        {
            var brigadeStorage = await _brigadeStorageRepository.Getbyid(id);

            if (brigadeStorage == null)
            {
                return new BaseResponse<BrigadeStorage>
                {
                    Description = "BrigadeStorage not found.",
                    StatusCode = StatusCode.NotFound,
                };
            }

            return new BaseResponse<BrigadeStorage>
            {
                Data = brigadeStorage,
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<List<BrigadeStorage>>> GetAll()
        {
            var brigadeStorages = await _brigadeStorageRepository.GetAll();

            return new BaseResponse<List<BrigadeStorage>>
            {
                Data = brigadeStorages,
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<BrigadeStorage>> Update(BrigadeStorageViewModel model)
        {
            var brigadeStorage = await _brigadeStorageRepository.Getbyid(model.ID);

            if (brigadeStorage == null)
            {
                return new BaseResponse<BrigadeStorage>
                {
                    Description = "BrigadeStorage not found.",
                    StatusCode = StatusCode.NotFound,
                };
            }

            brigadeStorage.BrigadeID = model.BrigadeID;
            brigadeStorage.WeaponID = model.WeaponID;
            brigadeStorage.AmmunitionID = model.AmmunitionID;
            brigadeStorage.WeaponQuantity = model.WeaponQuantity;
            brigadeStorage.AmmunitionQuantity = model.AmmunitionQuantity;
            brigadeStorage.WeaponRemainder = model.WeaponRemainder;
            brigadeStorage.AmmunitionRemainder = model.AmmunitionRemainder;

            await _brigadeStorageRepository.Update(brigadeStorage);

            return new BaseResponse<BrigadeStorage>
            {
                Data = brigadeStorage,
                Description = "BrigadeStorage updated successfully.",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var brigadeStorage = await _brigadeStorageRepository.Getbyid(id);

            if (brigadeStorage == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "BrigadeStorage not found.",
                    StatusCode = StatusCode.NotFound,
                };
            }

            await _brigadeStorageRepository.Delete(brigadeStorage);

            return new BaseResponse<bool>
            {
                Data = true,
                Description = "BrigadeStorage deleted successfully.",
                StatusCode = StatusCode.OK,
            };
        }
    }
}
