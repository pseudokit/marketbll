using Business.Models;
using System.Collections.Generic;

namespace Business.Services
{
    public class ProductCategoryModel: AbstractModel
    {
        public string CategoryName { get; set; }

        public ICollection<int> ProductIds { get; set; }

        public ProductCategoryModel() { }

        public ProductCategoryModel(int id, string categoryName, ICollection<int> productIds)
            : base(id)
        {
            CategoryName = categoryName;
            ProductIds = productIds;
        }
    }
}