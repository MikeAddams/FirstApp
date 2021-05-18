namespace App.Models
{
    public class ProductCRUDResultModel
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public ProductDetailsModel ProductDetails { get; set; }
    }
}
