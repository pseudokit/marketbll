using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Product : BaseEntity
    {
        public int ProductCategoryId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public ProductCategory? Category { get; set; }

        public virtual ICollection<ReceiptDetail>? ReceiptDetails { get; set; }

        public Product() { }

        public Product(int Id, int productCategoryId, string productName, decimal price)
            : base(Id)
        {
            ProductCategoryId = productCategoryId;
            ProductName = productName;
            Price = price;
        }
    }
}
