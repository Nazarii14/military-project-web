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
            //var responce = await _weaponRepository.GetAll();
            //var weapon = responce.FirstOrDefault(b => b.ID == id);
            var weapon = await _weaponRepository.Getbyid(id);

            if (weapon == null)
            {
                return new BaseResponse<Weapon>
                {
                    Description = "Weapon does not exist",
                    StatusCode = StatusCode.NotFound,
                };
            }

            return new BaseResponse<Weapon>
            {
                Data = weapon,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }

        public async Task<BaseResponse<List<Weapon>>> GetWeapons()
        {
            var responce = await _weaponRepository.GetAll();

            return new BaseResponse<List<Weapon>>
            {
                Data = responce,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }

        public async Task<BaseResponse<Weapon>> Create(WeaponViewModel model)
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

        public async Task<BaseResponse<Weapon>> Update(WeaponViewModel model)
        {
            var response = await _weaponRepository.GetAll();
            var weapon = response.FirstOrDefault(b => b.ID == model.ID);

            if (weapon == null)
            {
                return new BaseResponse<Weapon>
                {
                    Description = "Weapon does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            weapon.Name = model.Name;
            weapon.Type = model.Type;
            weapon.Price = model.Price;
            weapon.Weight = model.Weight;
            await _weaponRepository.Update(weapon);

            return new BaseResponse<Weapon>
            {
                Description = "Weapon created successfully",
                StatusCode = StatusCode.OK,
                Data = weapon
            };
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var response = await _weaponRepository.GetAll();
            var weapon = response.FirstOrDefault(b => b.ID == id);

            if (weapon == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "Weapon does not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            await _weaponRepository.Delete(weapon);
            return new BaseResponse<bool>
            {
                Data = true,
                Description = "Weapon deleted successfully",
                StatusCode = StatusCode.OK
            };
        }
    }
}
