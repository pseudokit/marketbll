using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Customer : BaseEntity
    {
        public int PersonId { get; set; }

        public int DiscountValue { get; set; }

        public Person? Person { get; set; }

        public virtual ICollection<Receipt>? Receipts { get; set; }

        public Customer() { }

        public Customer(int id, int personId, int discountValue) : base(id)
        {
            PersonId = personId;
            DiscountValue = discountValue;
        }
    }
}
