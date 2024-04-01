using Microsoft.EntityFrameworkCore;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilitaryProject.DAL.Repositories
{
    public class BrigadeStorageRepository : BaseRepository<BrigadeStorage>
    {
        private readonly ApplicationDbContext _db;

        public BrigadeStorageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(BrigadeStorage entity)
        {
            await _db.BrigadeStorages.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(BrigadeStorage entity)
        {
            _db.BrigadeStorages.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<BrigadeStorage>> GetAll()
        {
            return await _db.BrigadeStorages.ToListAsync();
        }

        public async Task Update(BrigadeStorage entity)
        {
            _db.BrigadeStorages.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<BrigadeStorage> Getbyid(int id)
        {
            return await _db.BrigadeStorages.FindAsync(id);
        }
    }
}

