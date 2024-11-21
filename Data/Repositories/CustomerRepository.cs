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
    public class CustomerRepository : AbstractRepository, ICustomerRepository
    {
        private readonly DbSet<Customer> dbSet;

        public CustomerRepository(TradeMarketDbContext context) : base(context)
        {
            dbSet = context.Set<Customer>();
        }

        public async Task AddAsync(Customer entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var result = await dbSet.FindAsync(id);
            if (result != null)
            {
                dbSet.Remove(result);     
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await dbSet.Include(c => c.Person)
                              .Include(c => c.Receipts)!
                              .ThenInclude(c => c.ReceiptDetails)
                              .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var result = await dbSet.FindAsync(id);

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new ArgumentException("not found by id");
            }
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            List<Customer> list = await dbSet.Include(c => c.Person)
                                      .Include(c => c.Receipts)!
                                      .ThenInclude(c => c.ReceiptDetails)
                                      .ToListAsync();

            var result = list.Find(c => c.Id == id);
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new ArgumentException("not found by id");
            }
        }

        public void Update(Customer entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
