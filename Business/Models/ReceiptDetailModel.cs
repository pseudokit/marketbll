namespace Business.Models
{
    public class ReceiptDetailModel : AbstractModel
    {
        public int ReceiptId { get; set; }

        public int ProductId { get; set; }

        public decimal DiscountUnitPrice { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public ReceiptDetailModel(int id, int receiptId, 
                    int productId, 
                    decimal discountUnitPrice, 
                    decimal unitPrice, 
                    int quantity) : base(id)
        {
            ReceiptId = receiptId;
            ProductId = productId;
            DiscountUnitPrice = discountUnitPrice;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

    }
}