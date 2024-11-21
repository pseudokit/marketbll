using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.CustomerRepository = unitOfWork.CustomerRepository;
            this.Mapper = mapper;
            this.UnitOfWork = unitOfWork;
        }
        private static readonly DateTime MIN_BIRTH_DATE = new DateTime(1955, 01, 01);
        private static readonly DateTime MAX_BIRTH_DATE = new DateTime(2007, 01, 01);

        private ICustomerRepository CustomerRepository { get; }
        private IMapper Mapper { get; }
        private IUnitOfWork UnitOfWork { get; }

        public async Task AddAsync(CustomerModel model)
        {
            if (model == null)
            {
                throw new MarketException();
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new MarketException();
            }
            if (model.BirthDate <= MIN_BIRTH_DATE || model.BirthDate >= MAX_BIRTH_DATE)
            {
                throw new MarketException();
            }
            await CustomerRepository.AddAsync(Mapper.Map<Customer>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await CustomerRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var list = await CustomerRepository.GetAllWithDetailsAsync();
            return Mapper.Map<CustomerModel[]>(list);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var result = await CustomerRepository.GetByIdWithDetailsAsync(id);
            return Mapper.Map<CustomerModel>(result);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await CustomerRepository.GetAllWithDetailsAsync();
            var result = customers
                         .Where(c =>
                                  c.Receipts
                                      .Any(r => r.ReceiptDetails
                                                .Any(rd => rd.ProductId == productId)));
                            
            return Mapper.Map<CustomerModel[]> (result);
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
            {
                throw new MarketException();
            }
            if (model.BirthDate <= MIN_BIRTH_DATE || model.BirthDate >= MAX_BIRTH_DATE)
            {
                throw new MarketException();
            }
            Customer customer = new Customer
            {
                Id = model.Id,
                PersonId = model.Id,
                DiscountValue = model.DiscountValue,
            };
            CustomerRepository.Update(customer);       
            await UnitOfWork.SaveAsync();
        }
    }
}
