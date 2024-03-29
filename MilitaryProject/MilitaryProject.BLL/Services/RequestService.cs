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
                        StatusCode = StatusCode.NotFound
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
                    StatusCode = StatusCode.InternalServerError
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
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Request>>()
                {
                    Description = $"[GetRequests] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Request>> CreateRequest(RequestViewModel model)
        {
            try
            {
                var request = new Request
                {
                    BrigadeID = model.BrigadeID,
                    WeaponID = model.WeaponID,
                    AmmunitionID = model.AmmunitionID,
                    WeaponQuantity = model.WeaponQuantity,
                    AmmunitionQuantity = model.AmmunitionQuantity,
                    Message = model.Message,
                    RequestStatus = model.RequestStatus,
                    Brigade = _brigadeRepository.Getbyid(model.BrigadeID).Result,
                    Weapon = _weaponRepository.Getbyid(model.WeaponID).Result,
                    Ammunition = _ammunitionRepository.Getbyid(model.AmmunitionID).Result
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

        public async Task<BaseResponse<Request>> UpdateRequest(RequestViewModel model)
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

                request.BrigadeID = model.BrigadeID;
                request.WeaponID = model.WeaponID;
                request.AmmunitionID = model.AmmunitionID;
                request.WeaponQuantity = model.WeaponQuantity;
                request.AmmunitionQuantity = model.AmmunitionQuantity;
                request.Message = model.Message;
                request.RequestStatus = model.RequestStatus;
                request.Brigade = _brigadeRepository.Getbyid(model.BrigadeID).Result;
                request.Weapon = _weaponRepository.Getbyid(model.WeaponID).Result;
                request.Ammunition = _ammunitionRepository.Getbyid(model.AmmunitionID).Result;
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

        public async Task<BaseResponse<bool>> DeleteRequest(int id)
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
