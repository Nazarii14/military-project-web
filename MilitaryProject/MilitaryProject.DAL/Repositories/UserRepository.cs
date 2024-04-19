using Microsoft.EntityFrameworkCore;
using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MilitaryProject.DAL.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(User entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(User entity)
        {
            _db.Users.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _db.Users
                .Include(x => x.Brigade)
                .ToListAsync();
        }

        public async Task Update(User entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<User> Getbyid(int id)
        {
            return await _db.Users
                .Include(x => x.Brigade)
                .FirstOrDefaultAsync(x => x.ID == id);
        }
    }
}
