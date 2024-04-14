using MilitaryProject.Domain.Entity;
using MilitaryProject.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.DAL.Repositories
{
    public class MilitaryRouteRepository : BaseRepository<MilitaryRoute>
    {
        private readonly ApplicationDbContext _db;

        public MilitaryRouteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(MilitaryRoute entity)
        {
            await _db.MilitaryRoutes.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(MilitaryRoute entity)
        {
            _db.MilitaryRoutes.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<MilitaryRoute>> GetAll()
        {
            return await _db.MilitaryRoutes
                .Include(r => r.Volunteer)
                .Include(r => r.Weapon)
                .Include(r => r.Ammunition)
                .ToListAsync();
        }

        public async Task Update(MilitaryRoute entity)
        {
            _db.MilitaryRoutes.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<MilitaryRoute> Getbyid(int id)
        {
            return await _db.MilitaryRoutes
                .FindAsync(id);
        }
    }
}
