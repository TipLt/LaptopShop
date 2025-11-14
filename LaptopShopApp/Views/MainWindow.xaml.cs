using System.Windows;
using LaptopShopApp.Models;
using LaptopShopApp.Repositories;
using LaptopShopApp.Services;

namespace LaptopShopApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly User _currentUser;
        private readonly AuthenticationService _authService;

        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _authService = new AuthenticationService();
            
            TxtUserInfo.Text = $"Welcome, {user.FullName} ({user.Role})";
            LoadDashboardData();
            ConfigureMenuByRole();
        }

        private void LoadDashboardData()
        {
            try
            {
                var laptopRepo = new LaptopRepository();
                var orderRepo = new OrderRepository();
                var customerRepo = new CustomerRepository();

                TxtTotalLaptops.Text = laptopRepo.GetAll().Count.ToString();
                TxtTotalOrders.Text = orderRepo.GetAll().Count.ToString();
                TxtTotalCustomers.Text = customerRepo.GetAll().Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureMenuByRole()
        {
            // Configure menu visibility based on role
            BtnLaptops.IsEnabled = _authService.HasPermission("Read", "Laptop");
            BtnOrders.IsEnabled = _authService.HasPermission("Read", "Order");
            BtnCustomers.IsEnabled = _authService.HasPermission("Read", "Customer");
        }

        private void BtnLaptops_Click(object sender, RoutedEventArgs e)
        {
            var window = WindowFactory.CreateWindow("LaptopManagement", _currentUser);
            window.Show();
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            var window = WindowFactory.CreateWindow("OrderManagement", _currentUser);
            window.Show();
        }

        private void BtnCustomers_Click(object sender, RoutedEventArgs e)
        {
            var window = WindowFactory.CreateWindow("CustomerManagement", _currentUser);
            window.Show();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _authService.Logout();
            var loginWindow = WindowFactory.CreateWindow("Login");
            loginWindow.Show();
            this.Close();
        }
    }
}
