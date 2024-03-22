using MilitaryProject.BLL.Interfaces;
using MilitaryProject.BLL.Services;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;

namespace MilitaryProject
{
    public static class Initalizer
    {
        public static void InitializeRepositories (this IServiceCollection services)
        {
            services.AddScoped<BaseRepository<User>, UserRepository>();
            services.AddScoped<BaseRepository<Weapon>, WeaponRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWeaponService, WeaponService>();
        }
    }
}
