using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Ammunition; 
using System.Security.Claims;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IAmmunitionService
    {
        Task<BaseResponse<Ammunition>> GetAmmunitionById(int id);
        Task<BaseResponse<List<Ammunition>>> GetAllAmmunition();
        Task<BaseResponse<Ammunition>> CreateAmmunition(AmmunitionViewModel model);
        Task<BaseResponse<Ammunition>> UpdateAmmunition(AmmunitionViewModel model);
        Task<BaseResponse<bool>> DeleteAmmunition(int id);
    }
}
