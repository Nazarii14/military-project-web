using Microsoft.EntityFrameworkCore;
using MilitaryProject.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryProject.DAL.Configurations;


namespace MilitaryProject.DAL
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Brigade> Brigades { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Ammunition> Ammunitions { get; set; }
        public DbSet<UserItems> UserItems { get; set; }
        public DbSet<BrigadeStorage> BrigadeStorages { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<MilitaryRoute> MilitaryRoutes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BrigadeConfiguration());
            modelBuilder.ApplyConfiguration(new WeaponConfiguration());
            modelBuilder.ApplyConfiguration(new AmmunitionConfiguration());
            modelBuilder.ApplyConfiguration(new UserItemsConfiguration());
            modelBuilder.ApplyConfiguration(new BrigadeStorageConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new MilitaryRouteConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
