using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.ViewModels.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;
using Microsoft.EntityFrameworkCore;


namespace MilitaryProject.BLL.Services
{
    public class StatsService : IStatsService
    {
        private readonly BaseRepository<User> _userRepository;
        private readonly BaseRepository<BrigadeStorage> _brigadeStorageRepository;

        public StatsService(BaseRepository<User> userRepository, BaseRepository<BrigadeStorage> brigadeStorageRepository)
        {
            _userRepository = userRepository;
            _brigadeStorageRepository = brigadeStorageRepository;
        }

        public async Task<BaseResponse<StatisticsViewModel>> GetBrigadeStatistics(int userId)
        {
            var user = await _userRepository.Getbyid(userId);
            if (user == null)
            {
                return new BaseResponse<StatisticsViewModel>
                {
                    Data = null,
                    Description = "User not found",
                    StatusCode = StatusCode.NotFound,
                };
            }

            int brigadeId = user.BrigadeID;

            var brigadeStorageItems = _brigadeStorageRepository.GetAll().Result
                .Where(bs => bs.BrigadeID == brigadeId).ToList();

            var weaponTypeCounts = new Dictionary<string, int>();
            var ammunitionTypeCounts = new Dictionary<string, int>();
            var weaponWeightCounts = new Dictionary<float, int>();
            var ammunitionSizeCounts = new Dictionary<string, int>();
            var weaponPriceCount = new Dictionary<decimal, int>();
            var ammunitionPriceCount = new Dictionary<decimal, int>();

            foreach (var item in brigadeStorageItems)
            {
                if (item.Weapon != null)
                {
                    if (!weaponTypeCounts.ContainsKey(item.Weapon.Type))
                    {
                        weaponTypeCounts[item.Weapon.Type] = 0;
                    }
                    weaponTypeCounts[item.Weapon.Type] += item.WeaponQuantity;

                    if (!weaponWeightCounts.ContainsKey(item.Weapon.Weight))
                    {
                        weaponWeightCounts[item.Weapon.Weight] = 0;
                    }
                    weaponWeightCounts[item.Weapon.Weight] += item.WeaponQuantity;

                    if (!weaponPriceCount.ContainsKey(item.Weapon.Price))
                    {
                        weaponPriceCount[item.Weapon.Price] = 0;
                    }
                    weaponPriceCount[item.Weapon.Price] += item.WeaponQuantity;
                }

                if (item.Ammunition != null)
                {
                    if (!ammunitionTypeCounts.ContainsKey(item.Ammunition.Type))
                    {
                        ammunitionTypeCounts[item.Ammunition.Type] = 0;
                    }
                    ammunitionTypeCounts[item.Ammunition.Type] += item.AmmunitionQuantity;

                    if (!ammunitionSizeCounts.ContainsKey(item.Ammunition.Size))
                    {
                        ammunitionSizeCounts[item.Ammunition.Size] = 0;
                    }
                    ammunitionSizeCounts[item.Ammunition.Size] += item.AmmunitionQuantity;

                    if (!ammunitionPriceCount.ContainsKey(item.Ammunition.Price))
                    {
                        ammunitionPriceCount[item.Ammunition.Price] = 0;
                    }
                    ammunitionPriceCount[item.Ammunition.Price] += item.AmmunitionQuantity;
                }
            }

            var statisticsViewModel = new StatisticsViewModel
            {
                WeaponTypeCounts = weaponTypeCounts,
                AmmunitionTypeCounts = ammunitionTypeCounts,
                WeaponWeightCounts = weaponWeightCounts,
                AmmunitionSizeCounts = ammunitionSizeCounts,
                WeaponPriceCount = weaponPriceCount,
                AmmunitionPriceCount = ammunitionPriceCount,
            };

            return new BaseResponse<StatisticsViewModel>
            {
                Data = statisticsViewModel,
                Description = "Brigade statistics retrieved successfully.",
                StatusCode = StatusCode.OK,
            };
        }
    }
}
