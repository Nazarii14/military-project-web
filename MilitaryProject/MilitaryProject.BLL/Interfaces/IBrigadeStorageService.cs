using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.BrigadeStorage;
using MilitaryProject.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IBrigadeStorageService
    {
        Task<BaseResponse<BrigadeStorage>> Create(BrigadeStorageViewModel model);
        Task<BaseResponse<BrigadeStorage>> GetById(int id);
        Task<BaseResponse<List<BrigadeStorage>>> GetAll();
        Task<BaseResponse<BrigadeStorage>> Update(BrigadeStorageViewModel model);
        Task<BaseResponse<bool>> Delete(int id);
    }
}
