using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilitaryProject.DAL.Repositories
{
    public class UserItemsRepository : BaseRepository<UserItems>
    {
        private readonly ApplicationDbContext _db;

        public UserItemsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(UserItems entity)
        {
            await _db.UserItems.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(UserItems entity)
        {
            _db.UserItems.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<UserItems>> GetAll()
        {
            return await _db.UserItems
                .Include(r => r.User)
                .Include(r => r.Weapon)
                .Include(r => r.Ammunition)
                .ToListAsync();
        }

        public async Task Update(UserItems entity)
        {
            _db.UserItems.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<UserItems> Getbyid(int id)
        {
            return await _db.UserItems.FindAsync(id);
        }
    }
}
