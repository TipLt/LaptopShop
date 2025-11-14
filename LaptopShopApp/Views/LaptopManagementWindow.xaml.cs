using System.Windows;
using System.Windows.Controls;
using LaptopShopApp.Models;
using LaptopShopApp.Repositories;
using LaptopShopApp.Services;

namespace LaptopShopApp.Views
{
    public partial class LaptopManagementWindow : Window
    {
        private readonly LaptopRepository _repository;
        private readonly AuthenticationService _authService;
        private readonly User _currentUser;
        private Laptop? _selectedLaptop;

        public LaptopManagementWindow(User user)
        {
            InitializeComponent();
            _repository = new LaptopRepository();
            _authService = new AuthenticationService();
            _currentUser = user;
            
            ConfigurePermissions();
            LoadData();
        }

        private void ConfigurePermissions()
        {
            bool canCreate = _authService.HasPermission("Create", "Laptop");
            bool canUpdate = _authService.HasPermission("Update", "Laptop");
            bool canDelete = _authService.HasPermission("Delete", "Laptop");

            BtnAdd.IsEnabled = canCreate;
            BtnEdit.IsEnabled = canUpdate;
            BtnDelete.IsEnabled = canDelete;
            BtnSave.IsEnabled = canCreate || canUpdate;
        }

        private void LoadData()
        {
            try
            {
                var laptops = _repository.GetAll();
                DgLaptops.ItemsSource = laptops;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!_authService.HasPermission("Create", "Laptop"))
            {
                MessageBox.Show("You don't have permission to add laptops.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            ClearForm();
            _selectedLaptop = null;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!_authService.HasPermission("Update", "Laptop"))
            {
                MessageBox.Show("You don't have permission to edit laptops.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DgLaptops.SelectedItem is Laptop laptop)
            {
                _selectedLaptop = laptop;
                TxtBrand.Text = laptop.Brand;
                TxtModel.Text = laptop.Model;
                TxtProcessor.Text = laptop.Processor;
                TxtRAM.Text = laptop.RAM;
                TxtStorage.Text = laptop.Storage;
                TxtGPU.Text = laptop.GPU;
                TxtPrice.Text = laptop.Price.ToString();
                TxtStock.Text = laptop.Stock.ToString();
                TxtDescription.Text = laptop.Description;
            }
            else
            {
                MessageBox.Show("Please select a laptop to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!_authService.HasPermission("Delete", "Laptop"))
            {
                MessageBox.Show("You don't have permission to delete laptops.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DgLaptops.SelectedItem is Laptop laptop)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {laptop.Brand} {laptop.Model}?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (_repository.Delete(laptop.LaptopID))
                        {
                            MessageBox.Show("Laptop deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                            ClearForm();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting laptop: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a laptop to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBrand.Text) || string.IsNullOrWhiteSpace(TxtModel.Text))
            {
                MessageBox.Show("Brand and Model are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtStock.Text, out int stock))
            {
                MessageBox.Show("Please enter a valid stock number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var laptop = new Laptop
                {
                    Brand = TxtBrand.Text,
                    Model = TxtModel.Text,
                    Processor = TxtProcessor.Text,
                    RAM = TxtRAM.Text,
                    Storage = TxtStorage.Text,
                    GPU = TxtGPU.Text,
                    Price = price,
                    Stock = stock,
                    Description = TxtDescription.Text
                };

                bool success;
                if (_selectedLaptop == null)
                {
                    if (!_authService.HasPermission("Create", "Laptop"))
                    {
                        MessageBox.Show("You don't have permission to add laptops.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    success = _repository.Add(laptop);
                }
                else
                {
                    if (!_authService.HasPermission("Update", "Laptop"))
                    {
                        MessageBox.Show("You don't have permission to update laptops.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    laptop.LaptopID = _selectedLaptop.LaptopID;
                    success = _repository.Update(laptop);
                }

                if (success)
                {
                    MessageBox.Show("Laptop saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to save laptop.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving laptop: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            _selectedLaptop = null;
            TxtBrand.Clear();
            TxtModel.Clear();
            TxtProcessor.Clear();
            TxtRAM.Clear();
            TxtStorage.Clear();
            TxtGPU.Clear();
            TxtPrice.Clear();
            TxtStock.Clear();
            TxtDescription.Clear();
        }
    }
}
