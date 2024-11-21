using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PersonRepository : AbstractRepository, IPersonRepository
    {
        private readonly DbSet<Person> dbSet;
        public PersonRepository(TradeMarketDbContext context) : base(context)
        {
            dbSet = context.Set<Person>();
        }

        public async Task AddAsync(Person entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Person entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var result = await dbSet.FindAsync(id);
            if (result != null)
            {
                dbSet.Remove(result);
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            var result = await dbSet.FindAsync(id);
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new ArgumentException("not found");
            }
        }

        public void Update(Person entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
