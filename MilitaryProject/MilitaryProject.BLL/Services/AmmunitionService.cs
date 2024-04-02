using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Ammunition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class AmmunitionService: IAmmunitionService
    {
        private readonly BaseRepository<Ammunition> _ammunitionRepository;

        public AmmunitionService(BaseRepository<Ammunition> ammunitionRepository)
        {
            _ammunitionRepository = ammunitionRepository;
        }

        public async Task<BaseResponse<Ammunition>> GetById(int id)
        {
            try
            {
                var responce = await _ammunitionRepository.GetAll();
                var ammunition = responce.FirstOrDefault(b => b.ID == id);

                if (ammunition == null)
                {
                    return new BaseResponse<Ammunition>
                    {
                        Description = "Ammunition does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<Ammunition>
                {
                    Data = ammunition,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Ammunition>()
                {
                    Description = $"[GetAmmunition] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<Ammunition>>> GetAll()
        {
            try
            {
                var responce = await _ammunitionRepository.GetAll();
                return new BaseResponse<List<Ammunition>>
                {
                    Data = responce,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Ammunition>>()
                {
                    Description = $"[GetAmmunitions] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Ammunition>> Create(AmmunitionViewModel model)
        {
            try
            {
                var existingAmmunition = (await _ammunitionRepository.GetAll())
                    .FirstOrDefault(w => w.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase));
                if (existingAmmunition != null)
                {
                    return new BaseResponse<Ammunition>
                    {
                        Description = "Ammunition already exists",
                        StatusCode = StatusCode.InternalServerError
                    };
                }
                var ammunition = new Ammunition
                {
                    Name = model.Name,
                    Type = model.Type,
                    Price = model.Price,
                    Size = model.Size,
                };

                await _ammunitionRepository.Create(ammunition);
                return new BaseResponse<Ammunition>
                {
                    Data = ammunition,
                    StatusCode = Domain.Enum.StatusCode.OK,
                    Description = "Ammunition created successfully",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Ammunition>()
                {
                    Description = $"[CreateAmmunition] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<BaseResponse<Ammunition>> Update(AmmunitionViewModel model)
        {
            try
            {
                var existingAmmunition = (await _ammunitionRepository.GetAll())
                    .FirstOrDefault(b => b.ID == model.ID);
                if (existingAmmunition == null)
                {
                    return new BaseResponse<Ammunition>
                    {
                        Description = "Ammunition does not exist",
                        StatusCode = StatusCode.NotFound
                    };
                }
                existingAmmunition.Name = model.Name;
                existingAmmunition.Type = model.Type;
                existingAmmunition.Price = model.Price;
                existingAmmunition.Size = model.Size;

                await _ammunitionRepository.Update(existingAmmunition);
                return new BaseResponse<Ammunition>
                {
                    Data = existingAmmunition,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Ammunition>()
                {
                    Description = $"[UpdateAmmunition] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<BaseResponse<bool>> Delete(int id)
        {
            try
            {
                var existingAmmunition = (await _ammunitionRepository.GetAll()).FirstOrDefault(b => b.ID == id);
                if (existingAmmunition == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Ammunition does not exist",
                        StatusCode = StatusCode.NotFound,
                        Data = false
                    };
                }
                await _ammunitionRepository.Delete(existingAmmunition);
                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = Domain.Enum.StatusCode.OK,
                    Description = "Ammunition deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteAmmunition] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
