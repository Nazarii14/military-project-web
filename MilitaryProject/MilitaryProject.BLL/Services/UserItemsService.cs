using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.UserItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class UserItemsService : IUserItemsService
    {
        private readonly BaseRepository<UserItems> _userItemsRepository;

        public UserItemsService(BaseRepository<UserItems> userItemsRepository)
        {
            _userItemsRepository = userItemsRepository;
        }

        public async Task<BaseResponse<UserItems>> GetUserItem(int id)
        {
            try
            {
                var response = await _userItemsRepository.GetAll();
                var userItem = response.FirstOrDefault(u => u.ID == id);

                if (userItem == null)
                {
                    return new BaseResponse<UserItems>
                    {
                        Description = "User item does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<UserItems>
                {
                    Data = userItem,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserItems>
                {
                    Description = $"[GetUserItem] Error: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<UserItems>>> GetUserItems()
        {
            try
            {
                var response = await _userItemsRepository.GetAll();

                return new BaseResponse<List<UserItems>>
                {
                    Data = response,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserItems>>
                {
                    Description = $"[GetUserItems] Error: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<UserItems>> CreateUserItems(UserItemsViewModel model)
        {
            try
            {
                var response = await _userItemsRepository.GetAll();
                var existingUserItem = response.FirstOrDefault(u => u.UserID == model.UserID && u.WeaponID == model.WeaponID && u.AmmunitionID == model.AmmunitionID);

                if (existingUserItem != null)
                {
                    return new BaseResponse<UserItems>
                    {
                        Description = "User item already exists",
                    };
                }

                var newUserItem = new UserItems
                {
                    UserID = model.UserID,
                    WeaponID = model.WeaponID,
                    AmmunitionID = model.AmmunitionID
                };

                await _userItemsRepository.Create(newUserItem);

                return new BaseResponse<UserItems>
                {
                    Data = newUserItem,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserItems>
                {
                    Description = $"[CreateUserItems] Error: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public async Task<BaseResponse<UserItems>> UpdateUserItems(UserItemsViewModel userItem)
        {
            try
            {
                var response = await _userItemsRepository.GetAll();
                var existingUserItem = response.FirstOrDefault(u => u.ID == userItem.ID);

                if (existingUserItem == null)
                {
                    return new BaseResponse<UserItems>
                    {
                        Description = "User item does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }

                existingUserItem.UserID = userItem.UserID;
                existingUserItem.WeaponID = userItem.WeaponID;
                existingUserItem.AmmunitionID = userItem.AmmunitionID;

                await _userItemsRepository.Update(existingUserItem);

                return new BaseResponse<UserItems>
                {
                    Data = existingUserItem,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserItems>
                {
                    Description = $"[UpdateUserItems] Error: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteUserItems(int id)
        {
            try
            {
                var response = await _userItemsRepository.GetAll();
                var userItem = response.FirstOrDefault(u => u.ID == id);

                if (userItem == null)
                {
                    return new BaseResponse<bool>
                    {
                        Data = false,
                        Description = "User item does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _userItemsRepository.Delete(userItem);

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Description = $"[DeleteUserItems] Error: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
