using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ProductModel : AbstractModel
    {
        public int ProductCategoryId { get; set; }  
        
        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public ICollection<int> ReceiptDetailIds { get; set; }

        public ProductModel() { }

        public ProductModel(int id, 
                            int productCategoryId, 
                            string categoryName, 
                            string productName, 
                            decimal price,
                            ICollection<int> receiptDetailsIds) : base(id)
        {
            ProductCategoryId = productCategoryId;
            CategoryName = categoryName;
            ProductName = productName;
            Price = price;
            ReceiptDetailIds = receiptDetailsIds;
        }
    }
}
