using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IStatsWeaponService
    {
        Task<BaseResponse<List<StatsWeaponViewModel>>> GetAll(int userID);
        Task<BaseResponse<StatsWeaponViewModel>> Create(StatsWeaponViewModel model);
        Task<BaseResponse<StatsWeaponViewModel>> Update(StatsWeaponViewModel model);
        Task<BaseResponse<bool>> Delete(int userID, int weaponID);
        Task<BaseResponse<StatsWeaponViewModel>> GetById(int userID, int weaponID);
    }
}
