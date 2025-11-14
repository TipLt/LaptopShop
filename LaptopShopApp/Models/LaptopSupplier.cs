namespace LaptopShopApp.Models
{
    // Junction table for many-to-many relationship between Laptop and Supplier
    public class LaptopSupplier
    {
        public int LaptopID { get; set; }
        public int SupplierID { get; set; }
        public DateTime SupplyDate { get; set; }
        public decimal SupplyPrice { get; set; }
    }
}
