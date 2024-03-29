using MilitaryProject.DAL.Interface;
using MilitaryProject.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.DAL.Repositories
{
    public class RequestRepository : BaseRepository<Request>
    {
        private readonly ApplicationDbContext _db;

        public RequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(Request entity)
        {
            await _db.Requests.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        
        public async Task Delete(Request entity)
        {
            _db.Requests.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Request>> GetAll()
        {
            return await _db.Requests.ToListAsync();
        }

        public async Task Update(Request entity)
        {
            _db.Requests.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Request> Getbyid(int id)
        {
            return await _db.Requests.FindAsync(id);
        }
    }
}
