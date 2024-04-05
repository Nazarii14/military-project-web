using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.UserItems;
using System.Security.Claims;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IUserItemsService
    {
        Task<BaseResponse<UserItems>> GetUserItem(int id);
        Task<BaseResponse<List<UserItems>>> GetUserItems();
        Task<BaseResponse<UserItems>> Create(UserItemsViewModel model);
        Task<BaseResponse<UserItems>> Update(UserItemsViewModel model);
        Task<BaseResponse<bool>> Delete(int id);
    }
}
