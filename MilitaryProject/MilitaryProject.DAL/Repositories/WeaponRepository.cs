using Microsoft.EntityFrameworkCore;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilitaryProject.DAL.Repositories
{
    public class WeaponRepository : BaseRepository<Weapon>
    {
        private readonly ApplicationDbContext _db;

        public WeaponRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(Weapon entity)
        {
            await _db.Weapons.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Weapon entity)
        {
            _db.Weapons.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Weapon>> GetAll()
        {
            return await _db.Weapons.ToListAsync();
        }

        public async Task Update(Weapon entity)
        {
            _db.Weapons.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
