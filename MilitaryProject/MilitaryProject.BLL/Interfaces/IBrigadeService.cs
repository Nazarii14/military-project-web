using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Brigade;
using System.Security.Claims;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IBrigadeService
    {
        Task<BaseResponse<Brigade>> Create(BrigadeViewModel model);
        Task<BaseResponse<Brigade>> GetById(int id);
        Task<BaseResponse<List<Brigade>>> GetAll();
        Task<BaseResponse<Brigade>> Update(BrigadeViewModel model);
        Task<BaseResponse<bool>> Delete(int id);
    }
}
