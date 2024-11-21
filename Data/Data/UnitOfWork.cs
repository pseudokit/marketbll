using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TradeMarketDbContext context;
        private readonly CustomerRepository customerRepository;
        private readonly PersonRepository personRepository;
        private readonly ProductRepository productRepository;
        private readonly ProductCategoryRepository productCategoryRepository;
        private readonly ReceiptRepository receiptRepository;
        private readonly ReceiptDetailRepository receiptDetailRepository;

        public UnitOfWork(TradeMarketDbContext contextDb)
        {
            context = contextDb;
            this.customerRepository = new CustomerRepository(context);
            this.personRepository = new PersonRepository(context);
            this.productRepository = new ProductRepository(context);
            this.productCategoryRepository = new ProductCategoryRepository(context);
            this.receiptRepository = new ReceiptRepository(context);
            this.receiptDetailRepository = new ReceiptDetailRepository(context);
        }

        public ICustomerRepository CustomerRepository => this.customerRepository;

        public IPersonRepository PersonRepository => this.personRepository;

        public IProductRepository ProductRepository => this.productRepository;

        public IProductCategoryRepository ProductCategoryRepository => this.productCategoryRepository;

        public IReceiptRepository ReceiptRepository => this.receiptRepository;

        public IReceiptDetailRepository ReceiptDetailRepository => this.receiptDetailRepository;

#pragma warning disable CA1816
        public void Dispose()
        {
            context.Dispose();
        }
#pragma warning restore CA1816

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
