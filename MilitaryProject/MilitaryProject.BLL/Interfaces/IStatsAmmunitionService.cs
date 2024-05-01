using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IStatsAmmunitionService
    {
        Task<BaseResponse<List<StatsAmmunitionViewModel>>> GetAll(int userID);
        Task<BaseResponse<StatsAmmunitionViewModel>> Create(StatsAmmunitionViewModel model);
        Task<BaseResponse<StatsAmmunitionViewModel>> Update(StatsAmmunitionViewModel model);
        Task<BaseResponse<bool>> Delete(int userID, int weaponID);
        Task<BaseResponse<StatsAmmunitionViewModel>> GetById(int userID, int weaponID);
    }
}
