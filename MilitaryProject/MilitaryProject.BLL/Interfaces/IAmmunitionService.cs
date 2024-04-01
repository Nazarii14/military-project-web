using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Ammunition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IAmmunitionService
    {
        Task<BaseResponse<Ammunition>> GetAmmunition(int id);
        Task<BaseResponse<List<Ammunition>>> GetAmmunitions();
        Task<BaseResponse<Ammunition>> CreateAmmunition(AmmunitionViewModel model);
        Task<BaseResponse<Ammunition>> UpdateAmmunition(AmmunitionViewModel model);
        Task<BaseResponse<bool>> DeleteAmmunition(int id);
    }
}
