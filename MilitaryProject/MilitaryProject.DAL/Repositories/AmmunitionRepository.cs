using MilitaryProject.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryProject.DAL.Interface;

namespace MilitaryProject.DAL.Repositories
{
    public class AmmunitionRepository : BaseRepository<Ammunition>
    {
        private readonly ApplicationDbContext _db;

        public AmmunitionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(Ammunition entity)

        {
            await _db.Ammunitions.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Ammunition entity)
        {
            _db.Ammunitions.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Ammunition>> GetAll()
        {
            return await _db.Ammunitions.ToListAsync();
        }

        public async Task Update(Ammunition entity)
        {
            _db.Ammunitions.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Ammunition> Getbyid(int id)
        {
            return await _db.Ammunitions.FindAsync(id);
        }
    }
}
