using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.MilitaryRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Services
{
    public class MilitaryRouteService : IMilitaryRouteService
    {
        private readonly BaseRepository<MilitaryRoute> _militaryRouteRepository;
        private readonly BaseRepository<User> _volunteerRepository;
        private readonly BaseRepository<Weapon> _weaponRepository;
        private readonly BaseRepository<Ammunition> _ammunitionRepository;

        public MilitaryRouteService(BaseRepository<MilitaryRoute> militaryRouteRepository,
                                 BaseRepository<User> volunteerRepository,
                                 BaseRepository<Weapon> weaponRepository,
                                 BaseRepository<Ammunition> ammunitionRepository)
        {
            _militaryRouteRepository = militaryRouteRepository;
            _volunteerRepository = volunteerRepository;
            _weaponRepository = weaponRepository;
            _ammunitionRepository = ammunitionRepository;
        }

        public async Task<BaseResponse<MilitaryRoute>> GetMilitaryRoute(int id)
        {
            var response = await _militaryRouteRepository.GetAll();
            var militaryRoute = response.FirstOrDefault(b => b.ID == id);

            if (militaryRoute == null)
            {
                return new BaseResponse<MilitaryRoute>
                {
                    Description = "MilitaryRoute does not exist",
                    StatusCode = StatusCode.NotFound,
                };
            }

            return new BaseResponse<MilitaryRoute>
            {
                Data = militaryRoute,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }

        public async Task<BaseResponse<List<MilitaryRoute>>> GetMilitaryRoutes()
        {
            var response = await _militaryRouteRepository.GetAll();

            return new BaseResponse<List<MilitaryRoute>>
            {
                Data = response,
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }

        public async Task<BaseResponse<MilitaryRoute>> Create(CreateMilitaryRouteViewModel model)
        {
            var volunteerResponse = await _volunteerRepository.GetAll();
            var volunteer = volunteerResponse.FirstOrDefault(b => b.Lastname == model.VolunteerLastName);

            if (volunteer == null)
            {
                return new BaseResponse<MilitaryRoute>
                {
                    Description = "Volunteer does not exist",
                    StatusCode = StatusCode.NotFound,
                };
            }

            var weaponResponse = await _weaponRepository.GetAll();
            var weapon = weaponResponse.FirstOrDefault(b => b.Name == model.WeaponName);

            if (weapon == null)
            {
                return new BaseResponse<MilitaryRoute>
                {
                    Description = "Weapon does not exist",
                    StatusCode = StatusCode.NotFound,
                };
            }

            var ammunitionResponse = await _ammunitionRepository.GetAll();
            var ammunition = ammunitionResponse.FirstOrDefault(b => b.Name == model.AmmunitionName);

            if (ammunition == null)
            {
                return new BaseResponse<MilitaryRoute>
                {
                    Description = "Ammunition does not exist",
                    StatusCode = StatusCode.NotFound,
                };
            }

            var militaryRoute = new MilitaryRoute
            {
                VolunteerID = volunteer.ID,
                WeaponID = weapon.ID,
                AmmunitionID = ammunition.ID,
                StartPoint = model.StartPoint,
                Destination = model.Destination,
                WeaponQuantity = model.WeaponQuantity,
                AmmunitionQuantity = model.AmmunitionQuantity,
                DeliveryDate = model.DeliveryDate,
                Volunteer = volunteer,
                Weapon = weapon,
                Ammunition = ammunition,
            };

            await _militaryRouteRepository.Create(militaryRoute);

            return new BaseResponse<MilitaryRoute>
            {
                Data = militaryRoute,
                Description = "MilitaryRoute created successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<MilitaryRoute>> Update(EditMilitaryRouteViewModel model)
        {
            var response = await _militaryRouteRepository.GetAll();
            var militaryRoute = response.FirstOrDefault(b => b.ID == model.ID);

            if (militaryRoute == null)
            {
                return new BaseResponse<MilitaryRoute>
                {
                    Description = "MilitaryRoute does not exist",
                    StatusCode = StatusCode.NotFound,
                };
            }

            militaryRoute.StartPoint = model.StartPoint;
            militaryRoute.Destination = model.Destination;
            militaryRoute.WeaponQuantity = model.WeaponQuantity;
            militaryRoute.AmmunitionQuantity = model.AmmunitionQuantity;
            militaryRoute.DeliveryDate = model.DeliveryDate;

            await _militaryRouteRepository.Update(militaryRoute);

            return new BaseResponse<MilitaryRoute>
            {
                Data = militaryRoute,
                Description = "MilitaryRoute updated successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var response = await _militaryRouteRepository.GetAll();
            var militaryRoute = response.FirstOrDefault(b => b.ID == id);

            if (militaryRoute == null)
            {
                return new BaseResponse<bool>
                {
                    Description = "MilitaryRoute does not exist",
                    StatusCode = StatusCode.NotFound,
                };
            }

            await _militaryRouteRepository.Delete(militaryRoute);
            return new BaseResponse<bool>
            {
                Data = true,
                Description = "MilitaryRoute deleted successfully",
                StatusCode = Domain.Enum.StatusCode.OK,
            };
        }
    }
}
