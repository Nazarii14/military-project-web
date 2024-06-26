﻿using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IRequestService
    {
        Task<BaseResponse<Request>> GetRequest(int id);
        Task<BaseResponse<List<Request>>> GetRequests();
        Task<BaseResponse<Request>> Create(CreateRequestViewModel model);
        Task<BaseResponse<Request>> Update(EditRequestViewModel model);
        Task<BaseResponse<bool>> Delete(int id);
    }
}
