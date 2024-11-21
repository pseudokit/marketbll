using System;
using System.Collections;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Receipt : BaseEntity
    {
        public int CustomerId { get; set; }

        public DateTime OperationDate { get; set; }

        public bool IsCheckedOut { get; set; }

        public Customer? Customer { get; set; }

        public virtual ICollection<ReceiptDetail>? ReceiptDetails { get; set; }

        public Receipt() : base() { }

        public Receipt(int Id, int customerId, DateTime operationDate, bool isCheckedOut)
            : base(Id)
        {
            CustomerId = customerId;
            OperationDate = operationDate;
            IsCheckedOut = isCheckedOut;
        }
    }
}
