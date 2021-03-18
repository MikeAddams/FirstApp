
namespace App.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public float Price { get; set; }

        //public int Quantity { get; set; }
        //public int ImageId { get; set; }

        public int ThumbNailId { get; set; }
        public Image ThumbNail { get; set; }

        public int ManagerId { get; set; }
    }
}