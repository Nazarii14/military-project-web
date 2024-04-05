using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.MilitaryRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IMilitaryRouteService
    {
        Task<BaseResponse<MilitaryRoute>> GetMilitaryRoute(int id);
        Task<BaseResponse<List<MilitaryRoute>>> GetMilitaryRoutes();
        Task<BaseResponse<MilitaryRoute>> Create(CreateMilitaryRouteViewModel militaryRoute);
        Task<BaseResponse<MilitaryRoute>> Update(EditMilitaryRouteViewModel militaryRoute);
        Task<BaseResponse<bool>> Delete(int id);
    }
}
