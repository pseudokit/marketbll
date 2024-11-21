using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
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
    public class ProductService : IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            ProductRepository = unitOfWork.ProductRepository;
            ProductCategoryRepository = unitOfWork.ProductCategoryRepository;
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }

        private IProductRepository ProductRepository { get; }
        private IProductCategoryRepository ProductCategoryRepository { get; }
        private IMapper Mapper { get; }
        private IUnitOfWork UnitOfWork { get; }

        public async Task AddAsync(ProductModel model)
        {
            if (string.IsNullOrEmpty(model.ProductName) || string.IsNullOrWhiteSpace(model.ProductName))
            {
                throw new MarketException();
            }
            var currentPrice = decimal.ToDouble(model.Price);
            if (double.IsNegative(currentPrice))
            {
                throw new MarketException();
            }
            await ProductRepository.AddAsync(Mapper.Map<Product>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (string.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException();
            }         
            await ProductCategoryRepository.AddAsync(Mapper.Map<ProductCategory>(categoryModel));
            await UnitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await ProductRepository.DeleteByIdAsync(modelId); 
            await UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var list =  await ProductRepository.GetAllWithDetailsAsync();
            return Mapper.Map<ProductModel[]>(list);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var list =  await ProductCategoryRepository.GetAllAsync();
            return Mapper.Map<ProductCategoryModel[]>(list);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var list = await ProductRepository.GetAllWithDetailsAsync();
            
            if (filterSearch.CategoryId != null)
            {
                list = list.Where(p => p.Category.Id == filterSearch.CategoryId);
            }
            if (filterSearch.MinPrice != null)
            {
                list = list.Where(p => p.Price >= filterSearch.MinPrice);
            }
            if (filterSearch.MaxPrice != null)
            {
                list = list.Where(p => p.Price <= filterSearch.MaxPrice);
            }
            return Mapper.Map<ProductModel[]>(list);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await ProductRepository.GetByIdWithDetailsAsync(id);
            return Mapper.Map<ProductModel>(product);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await UnitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            if (string.IsNullOrEmpty(model.ProductName))
            {
                throw new MarketException();
            }

            ProductRepository.Update(Mapper.Map<Product>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (string.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException();
            }
            ProductCategoryRepository.Update(Mapper.Map<ProductCategory>(categoryModel));
            await UnitOfWork.SaveAsync();
        }
    }
}
