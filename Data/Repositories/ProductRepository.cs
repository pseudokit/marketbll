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
    public class ProductRepository : AbstractRepository, IProductRepository
    {
        private readonly DbSet<Product> dbSet;

        public ProductRepository(TradeMarketDbContext context) : base(context)
        {
            dbSet = context.Set<Product>();
        }

        public async Task AddAsync(Product entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Product entity)
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

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await dbSet.Include(p => p.ReceiptDetails)
                              .Include(p => p.Category)
                              .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
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

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            List<Product> list =  await dbSet.Include(p => p.ReceiptDetails)
                                    .Include(p => p.Category)
                                    .ToListAsync();
            var result = list!.Find(p => p.Id == id);
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new ArgumentException("not found");
            }
        }

        public void Update(Product entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
