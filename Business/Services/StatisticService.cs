using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            ProductRepository = unitOfWork.ProductRepository;
            ReceiptDetailRepository = unitOfWork.ReceiptDetailRepository;
            ReceiptRepository = unitOfWork.ReceiptRepository;
        }

        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }
        private IProductRepository ProductRepository { get; }
        private IReceiptDetailRepository ReceiptDetailRepository { get; }
        private IReceiptRepository ReceiptRepository { get; }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await ReceiptRepository.GetAllWithDetailsAsync();
            var receiptDetailsByCustomer = receipts
                                        .Where(r => r.CustomerId == customerId)
                                        .SelectMany(r => r.ReceiptDetails)
                                        .OrderByDescending(rd => rd.Quantity);

            var products = receiptDetailsByCustomer
                                            .Select(rd => rd.Product)
                                            .Take(productCount);
        
            return Mapper.Map<ProductModel[]>(products);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await ReceiptRepository.GetAllWithDetailsAsync();
            var result = receipts
                       .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                       .SelectMany(r => r.ReceiptDetails)
                       .Where(rd => rd.Product.Category.Id == categoryId)
                       .Sum(p => p.Quantity * p.DiscountUnitPrice);
            return result;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receipts = await ReceiptDetailRepository.GetAllWithDetailsAsync();
            var receiptDetailsByCustomer = receipts
                                        .OrderByDescending(rd => rd.Quantity);

            var products = receiptDetailsByCustomer
                                            .Select(rd => rd.Product)
                                            .Take(productCount);

            return Mapper.Map<ProductModel[]>(products);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await ReceiptRepository.GetAllWithDetailsAsync();

            var list = receipts
                            .Where(r => r.OperationDate >= DateTime.MinValue && r.OperationDate <= endDate)
                            .Select(r => new CustomerActivityModel
                            {
                                CustomerId = r.Customer.Id,
                                CustomerName = r.Customer.Person.Name + " " + r.Customer.Person.Surname,
                                ReceiptSum = Decimal.ToInt32(r.ReceiptDetails.Sum(rd => rd.Quantity * rd.DiscountUnitPrice))
                            })
                            .GroupBy(c => c.CustomerId)
                                .Select
                                 (
                                    g =>
                                        new CustomerActivityModel
                                        {
                                            CustomerId = g.Key,
                                            CustomerName = g.Select(t => t.CustomerName).ToList()[0],

                                            ReceiptSum = g.Sum(c => c.ReceiptSum)
                                        }
                                 )
                             .OrderByDescending(c => c.ReceiptSum)
                             .Take(customerCount);
            return list;
        }
    }
}
