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
            services.AddScoped<BaseRepository<Brigade>, BrigadeRepository>();
            services.AddScoped<BaseRepository<UserItems>, UserItemsRepository>();

        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBrigadeService, BrigadeService>();
            services.AddScoped<IUserItemsService, UserItemsService>();
        }
    }
}
