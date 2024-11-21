using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string CategoryName { get; set; } = string.Empty;

        public virtual ICollection<Product>? Products { get; set; }

        public ProductCategory() { }

        public ProductCategory(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
