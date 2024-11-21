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
    public class ProductCategoryRepository : AbstractRepository, IProductCategoryRepository
    {
        private readonly DbSet<ProductCategory> dbSet;

        public ProductCategoryRepository(TradeMarketDbContext context) : base(context)
        {
            dbSet = context.Set<ProductCategory>();
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(ProductCategory entity)
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

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
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

        public void Update(ProductCategory entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
