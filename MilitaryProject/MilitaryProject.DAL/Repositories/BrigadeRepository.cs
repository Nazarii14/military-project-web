using Microsoft.EntityFrameworkCore;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilitaryProject.DAL.Repositories
{
    public class BrigadeRepository : BaseRepository<Brigade>
    {
        private readonly ApplicationDbContext _db;

        public BrigadeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(Brigade entity)
        {
            await _db.Brigades.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Brigade entity)
        {
            _db.Brigades.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Brigade>> GetAll()
        {
            return await _db.Brigades.ToListAsync();
        }

        public async Task<Brigade> Getbyid(int id)
        {
            return await _db.Brigades.FindAsync(id);
        }

        public async Task Update(Brigade entity)
        {
            _db.Brigades.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Brigade> GetById(int id)
        {
            return await _db.Brigades.FindAsync(id);
        }
    }
}