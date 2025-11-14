using System.Windows;
using LaptopShopApp.Models;
using LaptopShopApp.Repositories;
using LaptopShopApp.Services;

namespace LaptopShopApp.Views
{
    public partial class CustomerManagementWindow : Window
    {
        private readonly CustomerRepository _repository;
        private readonly AuthenticationService _authService;
        private readonly User _currentUser;
        private Customer? _selectedCustomer;

        public CustomerManagementWindow(User user)
        {
            InitializeComponent();
            _repository = new CustomerRepository();
            _authService = new AuthenticationService();
            _currentUser = user;
            
            ConfigurePermissions();
            LoadData();
        }

        private void ConfigurePermissions()
        {
            // Only Admin can fully manage customers
            bool canCreate = _authService.HasPermission("Create", "Customer");
            bool canUpdate = _authService.HasPermission("Update", "Customer");
            bool canDelete = _authService.HasPermission("Delete", "Customer");

            BtnAdd.IsEnabled = canCreate;
            BtnEdit.IsEnabled = canUpdate;
            BtnDelete.IsEnabled = canDelete;
        }

        private void LoadData()
        {
            try
            {
                var customers = _repository.GetAll();
                DgCustomers.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            _selectedCustomer = null;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgCustomers.SelectedItem is Customer customer)
            {
                _selectedCustomer = customer;
                TxtCustomerName.Text = customer.CustomerName;
                TxtEmail.Text = customer.Email;
                TxtPhone.Text = customer.Phone;
                TxtAddress.Text = customer.Address;
            }
            else
            {
                MessageBox.Show("Please select a customer to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!_authService.HasPermission("Delete", "Customer"))
            {
                MessageBox.Show("You don't have permission to delete customers.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DgCustomers.SelectedItem is Customer customer)
            {
                var result = MessageBox.Show($"Are you sure you want to delete customer {customer.CustomerName}?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (_repository.Delete(customer.CustomerID))
                        {
                            MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                            ClearForm();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCustomerName.Text))
            {
                MessageBox.Show("Customer name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var customer = new Customer
                {
                    CustomerName = TxtCustomerName.Text,
                    Email = TxtEmail.Text,
                    Phone = TxtPhone.Text,
                    Address = TxtAddress.Text
                };

                bool success;
                if (_selectedCustomer == null)
                {
                    if (!_authService.HasPermission("Create", "Customer"))
                    {
                        MessageBox.Show("You don't have permission to add customers.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    success = _repository.Add(customer);
                }
                else
                {
                    if (!_authService.HasPermission("Update", "Customer"))
                    {
                        MessageBox.Show("You don't have permission to update customers.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    customer.CustomerID = _selectedCustomer.CustomerID;
                    success = _repository.Update(customer);
                }

                if (success)
                {
                    MessageBox.Show("Customer saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to save customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving customer: {ex.Message}", "ex}", MessageBoxButton.OK, MessageBoxImage.Error);
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
            _selectedCustomer = null;
            TxtCustomerName.Clear();
            TxtEmail.Clear();
            TxtPhone.Clear();
            TxtAddress.Clear();
        }
    }
}
