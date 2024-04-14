using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Ammunition;
using System.Security.Claims;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IAmmunitionService
    {
        Task<BaseResponse<Ammunition>> GetById(int id);
        Task<BaseResponse<List<Ammunition>>> GetAll();
        Task<BaseResponse<Ammunition>> Create(AmmunitionViewModel model);
        Task<BaseResponse<Ammunition>> Update(AmmunitionViewModel model);
        Task<BaseResponse<bool>> Delete(int id);
    }
}
