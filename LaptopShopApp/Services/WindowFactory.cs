using System.Windows;
using LaptopShopApp.Models;

namespace LaptopShopApp.Services
{
    // Factory Pattern - Window Creation
    public class WindowFactory
    {
        public static Window CreateWindow(string windowType, User? user = null)
        {
            return windowType switch
            {
                "Login" => new Views.LoginWindow(),
                "Main" => new Views.MainWindow(user!),
                "LaptopManagement" => new Views.LaptopManagementWindow(user!),
                "OrderManagement" => new Views.OrderManagementWindow(user!),
                "CustomerManagement" => new Views.CustomerManagementWindow(user!),
                _ => throw new ArgumentException($"Unknown window type: {windowType}")
            };
        }
    }
}
