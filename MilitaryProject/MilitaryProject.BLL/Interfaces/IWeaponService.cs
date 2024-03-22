using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IWeaponService
    {
        Task<BaseResponse<Weapon>> GetWeapon(int id);
        Task<BaseResponse<List<Weapon>>> GetWeapons();
        Task<BaseResponse<Weapon>> CreateWeapon(WeaponViewModel model);
        Task<BaseResponse<Weapon>> UpdateWeapon(WeaponViewModel model);
        Task<BaseResponse<bool>> DeleteWeapon(int id);
    }
}
