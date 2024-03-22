using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class WeaponService : IWeaponService
    {
        private readonly BaseRepository<Weapon> _weaponRepository;

        public WeaponService(BaseRepository<Weapon> weaponRepository)
        {
            _weaponRepository = weaponRepository;
        }

        public async Task<BaseResponse<Weapon>> GetWeapon(int id)
        {
            try
            {
                var responce = await _weaponRepository.GetAll();
                var weapon = responce.FirstOrDefault(b => b.ID == id);

                if (weapon == null)
                {
                    return new BaseResponse<Weapon>
                    {
                        Description = "Weapon does not exist",
                        StatusCode = StatusCode.NotFount
                    };
                }

                return new BaseResponse<Weapon>
                {
                    Data = weapon,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Weapon>()
                {
                    Description = $"[GetWeapon] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<Weapon>>> GetWeapons()
        {
            try
            {
                var responce = await _weaponRepository.GetAll();

                return new BaseResponse<List<Weapon>>
                {
                    Data = responce,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Weapon>>()
                {
                    Description = $"[GetWeapons] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Weapon>> CreateWeapon(WeaponViewModel model)
        {
            try
            {
                var existingWeapon = (await _weaponRepository.GetAll())
                    .FirstOrDefault(w => w.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase));

                if (existingWeapon != null)
                {
                    return new BaseResponse<Weapon>
                    {
                        Description = "Weapon with the same name already exists.",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                var weapon = new Weapon
                {
                    Name = model.Name,
                    Type = model.Type,
                    Price = model.Price,
                    Weight = model.Weight,
                };

                await _weaponRepository.Create(weapon);

                return new BaseResponse<Weapon>
                {
                    Data = weapon,
                    Description = "Weapon created successfully.",
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Weapon>
                {
                    Description = $"Failed to create weapon: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }



        public async Task<BaseResponse<Weapon>> UpdateWeapon(WeaponViewModel model)
        {
            try
            {
                var response = await _weaponRepository.GetAll();
                var weapon = response.FirstOrDefault(b => b.Name == model.Name);

                if (weapon == null)
                {
                    return new BaseResponse<Weapon>
                    {
                        Description = "Weapon does not exist",
                        StatusCode = StatusCode.NotFount
                    };
                }

                var newWeapon = new Weapon()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Type = model.Type,
                    Price = model.Price,
                    Weight = model.Weight,
                };

                return new BaseResponse<Weapon>
                {
                    Data = weapon,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Weapon>()
                {
                    Description = $"[UpdateWeapon] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteWeapon(int id)
        {
            try
            {
                var response = await _weaponRepository.GetAll();
                var weapon = response.FirstOrDefault(b => b.ID == id);

                if (weapon == null)
                {
                    return new BaseResponse<bool>
                    {
                        Data = false,
                        Description = "Weapon does not exist",
                        StatusCode = StatusCode.NotFount
                    };
                }

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteWeapon] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
