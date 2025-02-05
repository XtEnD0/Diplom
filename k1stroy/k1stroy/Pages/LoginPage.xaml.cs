using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace k1stroy.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public int q = 0;
        public LoginPage()
        {
            InitializeComponent();
        }


        private void loginTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (q == 0)
            {
                loginTB.Text = "";
                q++;

            }

        }

        private void passwordTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            passwordTB.Visibility = Visibility.Collapsed;
            passwordPB.Visibility = Visibility.Visible;
            passwordPB.Focus();
        }

        private void loginBut_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder err = new StringBuilder();
            if (String.IsNullOrEmpty(loginTB.Text) || loginTB.Text == "логин")
            {
                err.AppendLine("Пожалуйста введите логин");
            }
            if (String.IsNullOrEmpty(passwordPB.Password))
            {
                err.AppendLine("Пожалуйста введите пароль");
            }
            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                cat.Visibility = Visibility.Visible;
            }
        }
    }
}
