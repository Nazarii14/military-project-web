using MilitaryProject.BLL.Interfaces;
using MilitaryProject.BLL.Services;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;

namespace MilitaryProject
{
    public static class Initalizer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<BaseRepository<User>, UserRepository>();
            services.AddScoped<BaseRepository<Brigade>, BrigadeRepository>();
            services.AddScoped<BaseRepository<BrigadeStorage>, BrigadeStorageRepository>();
            services.AddScoped<BaseRepository<Weapon>, WeaponRepository>();
            services.AddScoped<BaseRepository<Request>, RequestRepository>();
            services.AddScoped<BaseRepository<Ammunition>, AmmunitionRepository>();
            services.AddScoped<BaseRepository<MilitaryRoute>, MilitaryRouteRepository>();
            services.AddScoped<BaseRepository<UserItems>, UserItemsRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBrigadeService, BrigadeService>();
            services.AddScoped<IBrigadeStorageService, BrigadeStorageService>();
            services.AddScoped<IWeaponService, WeaponService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IAmmunitionService, AmmunitionService>();
            services.AddScoped<IMilitaryRouteService, MilitaryRouteService>();
            services.AddScoped<IUserItemsService, UserItemsService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}