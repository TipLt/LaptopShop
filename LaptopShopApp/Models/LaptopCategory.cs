namespace LaptopShopApp.Models
{
    // Junction table for many-to-many relationship between Laptop and Category
    public class LaptopCategory
    {
        public int LaptopID { get; set; }
        public int CategoryID { get; set; }
    }
}
