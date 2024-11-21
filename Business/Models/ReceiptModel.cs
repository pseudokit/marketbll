using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class ReceiptModel : AbstractModel
    {
        public int CustomerId { get; set; }

        public DateTime OperationDate { get; set; }

        public bool IsCheckedOut { get; set; }

        public ICollection<int> ReceiptDetailsIds { get; set; }

        public ReceiptModel() { }

        public ReceiptModel(int id, int customerId,
                            DateTime operationDate, bool isCheckedOut, ICollection<int> receiptDetailsIds) : base(id)
        {
            CustomerId = customerId;
            OperationDate = operationDate;
            IsCheckedOut = isCheckedOut;
            ReceiptDetailsIds = receiptDetailsIds;
        }
    }
}