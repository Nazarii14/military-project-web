using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class RequestService : IRequestService
    {
        private readonly BaseRepository<Request> _requestRepository;
        private readonly BaseRepository<Brigade> _brigadeRepository;
        private readonly BaseRepository<Weapon> _weaponRepository;
        private readonly BaseRepository<Ammunition> _ammunitionRepository;

        public RequestService(BaseRepository<Request> requestRepository,
                      BaseRepository<Brigade> brigadeRepository,
                      BaseRepository<Weapon> weaponRepository,
                      BaseRepository<Ammunition> ammunitionRepository)
        {
            _requestRepository = requestRepository;
            _brigadeRepository = brigadeRepository;
            _weaponRepository = weaponRepository;
            _ammunitionRepository = ammunitionRepository;
        }

        public async Task<BaseResponse<Request>> GetRequest(int id)
        {
            try
            {
                var response = await _requestRepository.GetAll();
                var request = response.FirstOrDefault(b => b.ID == id);

                if (request == null)
                {
                    return new BaseResponse<Request>
                    {
                        Description = "Request does not exist",
                        StatusCode = StatusCode.NotFound,
                    };
                }

                return new BaseResponse<Request>
                {
                    Data = request,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Request>()
                {
                    Description = $"[GetRequest] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<BaseResponse<List<Request>>> GetRequests()
        {
            try
            {
                var response = await _requestRepository.GetAll();

                return new BaseResponse<List<Request>>
                {
                    Data = response,
                    StatusCode = Domain.Enum.StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Request>>()
                {
                    Description = $"[GetRequests] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<BaseResponse<Request>> Create(CreateRequestViewModel model)
        {
            try
            {
                var responseBrigade = await _brigadeRepository.GetAll();

                var brigade = responseBrigade.FirstOrDefault(b => b.Name == model.BrigadeName);
                if (brigade == null)
                {
                    return new BaseResponse<Request>
                    {
                        Description = "Brigade does not exist",
                        StatusCode = StatusCode.NotFound,
                    };
                }

                var responseWeapon = await _weaponRepository.GetAll();
                var weapon = responseWeapon.FirstOrDefault(b => b.Name == model.WeaponName);
                if (weapon == null)
                {
                    return new BaseResponse<Request>
                    {
                        Description = "Weapon does not exist",
                        StatusCode = StatusCode.NotFound,
                    };
                }

                var responseAmmunition = await _ammunitionRepository.GetAll();
                var ammunition = responseAmmunition.FirstOrDefault(b => b.Name == model.AmmunitionName);
                if (ammunition == null)
                {
                    return new BaseResponse<Request>
                    {
                        Description = "Ammunition does not exist",
                        StatusCode = StatusCode.NotFound,
                    };
                }

                var request = new Request
                {
                    BrigadeID = brigade.ID,
                    WeaponID = weapon.ID,
                    AmmunitionID = ammunition.ID,
                    WeaponQuantity = model.WeaponQuantity,
                    AmmunitionQuantity = model.AmmunitionQuantity,
                    Message = model.Message,
                    RequestStatus = model.RequestStatus,
                    Brigade = brigade,
                    Weapon = weapon,
                    Ammunition = ammunition,
                };

                await _requestRepository.Create(request);

                return new BaseResponse<Request>
                {
                    Data = request,
                    Description = "Request created successfully.",
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Request>
                {
                    Description = $"Failed to create request: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<BaseResponse<Request>> Update(EditRequestViewModel model)
        {
            try
            {
                var response = await _requestRepository.GetAll();
                var request = response.FirstOrDefault(b => b.ID == model.ID);

                if (request == null)
                {
                    return new BaseResponse<Request>
                    {
                        Description = "Request does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }

                request.WeaponQuantity = model.WeaponQuantity;
                request.AmmunitionQuantity = model.AmmunitionQuantity;
                request.Message = model.Message;
                request.RequestStatus = model.RequestStatus;

                await _requestRepository.Update(request);

                return new BaseResponse<Request>
                {
                    Description = "Request created successfully",
                    StatusCode = StatusCode.OK,
                    Data = request
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Request>()
                {
                    Description = $"Failed to update request : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            try
            {
                var response = await _requestRepository.GetAll();
                var request = response.FirstOrDefault(b => b.ID == id);

                if (request == null)
                {
                    return new BaseResponse<bool>
                    {
                        Data = false,
                        Description = "Request does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _requestRepository.Delete(request);
                return new BaseResponse<bool>
                {
                    Data = true,
                    Description = "Request deleted successfully",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = $"Failed to delete request: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
