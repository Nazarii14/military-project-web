using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Brigade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class BrigadeService : IBrigadeService
    {
        private readonly BaseRepository<Brigade> _brigadeRepository;

        public BrigadeService (BaseRepository<Brigade> brigadeRepository)
        {
            _brigadeRepository = brigadeRepository;
        }

        public async Task<BaseResponse<Brigade>> GetBrigade(int id)
        {
            try
            {
                var responce = await _brigadeRepository.GetAll();
                var brigade = responce.FirstOrDefault(b => b.ID == id);

                if (brigade == null)
                {
                    return new BaseResponse<Brigade>
                    {
                        Description = "Brigade does not exist",
                    };
                }

                return new BaseResponse<Brigade>
                {
                    Data = brigade,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Brigade>()
                {
                    Description = $"[GetBrigade] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<Brigade>>> GetBrigades()
        {
            try
            {
                var responce = await _brigadeRepository.GetAll();

                return new BaseResponse<List<Brigade>>
                {
                    Data = responce,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Brigade>>()
                {
                    Description = $"[GetBrigades] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Brigade>> CreateBrigade(BrigadeViewModel model)
        {
            try
            {
                var responce = await _brigadeRepository.GetAll();
                var brigade = responce.FirstOrDefault(b => b.Name == model.Name);

                if (brigade != null)
                {
                    return new BaseResponse<Brigade>
                    {
                        Description = "Brigade is already exist"
                    };
                }

                return new BaseResponse<Brigade>
                {
                    Data = brigade,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Brigade>()
                {
                    Description = $"[CreateBrigade] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Brigade>> UpdateBrigade(BrigadeViewModel model)
        {
            try
            {
                var responce = await _brigadeRepository.GetAll();
                var brigade = responce.FirstOrDefault(b => b.ID == model.ID);

                if (brigade == null)
                {
                    return new BaseResponse<Brigade>
                    {
                        Description = "Brigade does not exist"
                    };
                }

                var newBrigade = new Brigade()
                {
                    ID = model.ID,
                    Name = model.Name,
                    CommanderName = model.CommanderName,
                    EstablishmentDate = model.EstablishmentDate,
                    Location = model.Location
                };

                return new BaseResponse<Brigade>
                {
                    Data = brigade,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Brigade>()
                {
                    Description = $"[CreateBrigade] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteBrigade(int id)
        {
            try
            {
                var responce = await _brigadeRepository.GetAll();
                var brigade = responce.FirstOrDefault(b => b.ID == id);

                if (brigade == null)
                {
                    return new BaseResponse<bool>
                    {
                        Data=false,
                        Description = "Brigade does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteBrigade] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
