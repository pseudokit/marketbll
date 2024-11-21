using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            ReceiptRepository = unitOfWork.ReceiptRepository;
            ReceiptDetailRepository = unitOfWork.ReceiptDetailRepository;
            ProductRepository = unitOfWork.ProductRepository;
        }

        private IMapper Mapper { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IReceiptRepository ReceiptRepository { get; }
        private IReceiptDetailRepository ReceiptDetailRepository { get; }
        private IProductRepository ProductRepository { get; }

        public async Task AddAsync(ReceiptModel model)
        {
            await ReceiptRepository.AddAsync(Mapper.Map<Receipt>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            Receipt receipt = await ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt == null)
            {
                throw new MarketException();
            }
            
            if (receipt.ReceiptDetails == null)
            {
                var product = await ProductRepository.GetByIdAsync(productId);
                if (product == null) { throw new MarketException(); }
                ReceiptDetail receiptDetail = new ReceiptDetail
                {
                    ReceiptId = receipt.Id,
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    DiscountUnitPrice = product.Price - Convert.ToDecimal((receipt.Customer.DiscountValue / 100.0)) * product.Price,
                };
                await ReceiptDetailRepository.AddAsync(receiptDetail);
                await UnitOfWork.SaveAsync();
                return;
            }

            var rd = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);
            if (rd != null)
            {
                rd.Quantity += quantity;
            }
            else
            {
                var product = await ProductRepository.GetByIdAsync(productId);
                ReceiptDetail receiptDetail = new ReceiptDetail
                {
                    ReceiptId = receipt.Id,
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    DiscountUnitPrice = product.Price - Convert.ToDecimal((receipt.Customer.DiscountValue / 100.0)) * product.Price,

                };
                await ReceiptDetailRepository.AddAsync(receiptDetail);
            }
            await UnitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await ReceiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;
            await UnitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            foreach (var item in receipt.ReceiptDetails)
            {
                ReceiptDetailRepository.Delete(item);
            }
            await ReceiptRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var list = await ReceiptRepository.GetAllWithDetailsAsync();
            return Mapper.Map<IEnumerable<ReceiptModel>>(list);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var item = await ReceiptRepository.GetByIdWithDetailsAsync(id);
            return Mapper.Map<ReceiptModel>(item);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return Mapper.Map<ReceiptDetailModel[]>(receipt.ReceiptDetails);
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receiptsList = await ReceiptRepository.GetAllWithDetailsAsync();
            return Mapper.Map<ReceiptModel[]>
                    (
                        receiptsList.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                    );
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            var receiptDetail = receipt.ReceiptDetails.FirstOrDefault(r => r.ProductId == productId);
            receiptDetail.Quantity -= quantity;
            if (receiptDetail.Quantity <= 0)
            {
                ReceiptDetailRepository.Delete(receiptDetail);
            }
            await UnitOfWork.SaveAsync();
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            var sum = receipt.ReceiptDetails.Sum(r =>  r.Quantity * r.DiscountUnitPrice);
            return sum;
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            ReceiptRepository.Update(Mapper.Map<Receipt>(model));
            await UnitOfWork.SaveAsync();
        }
    }
}
