using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Brigade;
using System.Security.Claims;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IBrigadeService
    {
        Task<BaseResponse<ClaimsIdentity>> Create(CreateBrigadeViewModel model);
        // Task<BaseResponse<IEnumerable<ReadBrigadeViewModel>>> GetAll();
        Task<BaseResponse<ClaimsIdentity>> GetById(int id);
        Task<BaseResponse<ClaimsIdentity>> Edit(EditBrigadeViewModel model);
        Task<BaseResponse<bool>> Delete(int id);
    }
}
