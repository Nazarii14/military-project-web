using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Brigade;
using System.Security.Claims;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IBrigadeService
    {
        Task<BaseResponse<Brigade>> GetBrigade(int id);
        Task<BaseResponse<List<Brigade>>> GetBrigades();
        Task<BaseResponse<Brigade>> CreateBrigade(BrigadeViewModel model);
        Task<BaseResponse<Brigade>> UpdateBrigade(BrigadeViewModel model);
        Task<BaseResponse<bool>> DeleteBrigade(int id);
    }
}
