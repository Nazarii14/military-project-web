using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IStatsService
    {
        Task<BaseResponse<StatisticsViewModel>> GetBrigadeStatistics(int userId);
    }
}
