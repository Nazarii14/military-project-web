using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
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

        public BrigadeService(BaseRepository<Brigade> brigadeRepository)
        {
            _brigadeRepository = brigadeRepository;
        }

        public async Task<BaseResponse<Brigade>> Create(BrigadeViewModel model)
        {
            var existingBrigade = (await _brigadeRepository.GetAll())
                    .FirstOrDefault(w => w.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase));

            if (existingBrigade != null)
            {
                return new BaseResponse<Brigade>
                {
                    Description = "Brigade with the same name already exists.",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            var brigade = new Brigade
            {
                Name = model.Name,
                CommanderName = model.CommanderName,
                EstablishmentDate = model.EstablishmentDate,
                Location = model.Location,
            };

            await _brigadeRepository.Create(brigade);

            return new BaseResponse<Brigade>
            {
                Data = brigade,
                Description = "Brigade created successfully.",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<Brigade>> GetById(int id)
        {
            var brigade = await _brigadeRepository.Getbyid(id);

            if (brigade == null)
            {
                return new BaseResponse<Brigade>
                {
                    Description = "Brigade not found.",
                    StatusCode = StatusCode.NotFound,
                };
            }

            return new BaseResponse<Brigade>
            {
                Data = brigade,
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<List<Brigade>>> GetAll()
        {
            var brigades = await _brigadeRepository.GetAll();

            return new BaseResponse<List<Brigade>>
            {
                Data = brigades,
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<Brigade>> Update(BrigadeViewModel model)
        {
            var brigade = await _brigadeRepository.Getbyid(model.ID);

            if (brigade == null)
            {
                return new BaseResponse<Brigade>
                {
                    Description = "Brigade not found.",
                    StatusCode = StatusCode.NotFound,
                };
            }

            brigade.Name = model.Name;
            brigade.CommanderName = model.CommanderName;
            brigade.EstablishmentDate = model.EstablishmentDate;
            brigade.Location = model.Location;

            await _brigadeRepository.Update(brigade);

            return new BaseResponse<Brigade>
            {
                Data = brigade,
                Description = "Brigade updated successfully.",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var brigade = await _brigadeRepository.Getbyid(id);

            if (brigade == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = "Brigade not found.",
                    StatusCode = StatusCode.NotFound,
                };
            }

            await _brigadeRepository.Delete(brigade);

            return new BaseResponse<bool>
            {
                Data = true,
                Description = "Brigade deleted successfully.",
                StatusCode = StatusCode.OK,
            };
        }
    }
}
