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
    public class ReceiptDetailRepository : AbstractRepository, IReceiptDetailRepository
    {
        private readonly DbSet<ReceiptDetail> dbSet;

        public ReceiptDetailRepository(TradeMarketDbContext context) : base(context)
        {
            dbSet = context.Set<ReceiptDetail>();
        }

        public async Task AddAsync(ReceiptDetail entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(ReceiptDetail entity)
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

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await dbSet.Include(rd => rd.Product)
                              .Include(rd => rd.Product!.Category)
                              .Include(rd => rd.Receipt)
                              .Select(rd => rd)
                              .ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
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

        public void Update(ReceiptDetail entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
