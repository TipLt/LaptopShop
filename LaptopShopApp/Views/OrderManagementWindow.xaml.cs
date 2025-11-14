using System.Windows;
using System.Windows.Controls;
using LaptopShopApp.Models;
using LaptopShopApp.Repositories;
using LaptopShopApp.Services;

namespace LaptopShopApp.Views
{
    public partial class OrderManagementWindow : Window
    {
        private readonly OrderRepository _repository;
        private readonly AuthenticationService _authService;
        private readonly User _currentUser;
        private Order? _selectedOrder;

        public OrderManagementWindow(User user)
        {
            InitializeComponent();
            _repository = new OrderRepository();
            _authService = new AuthenticationService();
            _currentUser = user;
            
            ConfigurePermissions();
            LoadData();
        }

        private void ConfigurePermissions()
        {
            bool canCreate = _authService.HasPermission("Create", "Order");
            BtnAdd.IsEnabled = canCreate;
        }

        private void LoadData()
        {
            try
            {
                var orders = _repository.GetAll();
                DgOrders.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!_authService.HasPermission("Create", "Order"))
            {
                MessageBox.Show("You don't have permission to create orders.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            ClearForm();
            _selectedOrder = null;
        }

        private void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (DgOrders.SelectedItem is Order order)
            {
                try
                {
                    var details = _repository.GetOrderDetails(order.OrderID);
                    var detailsText = $"Order ID: {order.OrderID}\n" +
                                    $"Customer ID: {order.CustomerID}\n" +
                                    $"Order Date: {order.OrderDate:dd/MM/yyyy HH:mm}\n" +
                                    $"Status: {order.Status}\n" +
                                    $"Total Amount: {order.TotalAmount:N0} VND\n\n" +
                                    "Order Items:\n";

                    foreach (var detail in details)
                    {
                        detailsText += $"- Laptop ID: {detail.LaptopID}, Quantity: {detail.Quantity}, " +
                                     $"Unit Price: {detail.UnitPrice:N0} VND, Subtotal: {detail.Subtotal:N0} VND\n";
                    }

                    MessageBox.Show(detailsText, "Order Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading order details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an order to view details.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!_authService.HasPermission("Create", "Order"))
            {
                MessageBox.Show("You don't have permission to create orders.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtCustomerID.Text, out int customerId))
            {
                MessageBox.Show("Please enter a valid Customer ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtTotalAmount.Text, out decimal totalAmount))
            {
                MessageBox.Show("Please enter a valid total amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var order = new Order
                {
                    CustomerID = customerId,
                    TotalAmount = totalAmount,
                    Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Pending",
                    Notes = TxtNotes.Text,
                    OrderDate = DateTime.Now
                };

                if (_repository.Add(order))
                {
                    MessageBox.Show("Order created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to create order.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ClearForm()
        {
            _selectedOrder = null;
            TxtCustomerID.Clear();
            TxtTotalAmount.Clear();
            TxtNotes.Clear();
            CmbStatus.SelectedIndex = 0;
        }
    }
}
