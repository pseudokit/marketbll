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
    public class ReceiptRepository : AbstractRepository, IReceiptRepository
    {
        private readonly DbSet<Receipt> dbSet;

        public ReceiptRepository(TradeMarketDbContext context) : base(context)
        {
            dbSet = context.Set<Receipt>();
        }

        public async Task AddAsync(Receipt entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Receipt entity)
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

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await dbSet.Include(r => r.Customer)
                              .Include(r => r.ReceiptDetails)!
                              .ThenInclude(rd => rd.Product)!
                              .ThenInclude(p => p!.Category)
                              .ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(int id)
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

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            List<Receipt> list =  await dbSet.Include(r => r.Customer)
                              .Include(r => r.ReceiptDetails)!
                              .ThenInclude(rd => rd.Product)
                              .ThenInclude(p => p!.Category)
                              .ToListAsync();

            var result = list.FirstOrDefault(r => r.Id == id);

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new ArgumentException("not found");
            }
        }

        public void Update(Receipt entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
