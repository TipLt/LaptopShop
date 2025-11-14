using System.Windows;
using LaptopShopApp.Services;

namespace LaptopShopApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthenticationService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = TxtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                TxtMessage.Text = "Please enter username and password";
                return;
            }

            try
            {
                var user = _authService.Login(username, password);
                if (user != null)
                {
                    var mainWindow = WindowFactory.CreateWindow("Main", user) as MainWindow;
                    mainWindow?.Show();
                    this.Close();
                }
                else
                {
                    TxtMessage.Text = "Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                TxtMessage.Text = $"Error: {ex.Message}";
            }
        }
    }
}
