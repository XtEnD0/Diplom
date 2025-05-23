using System;
using System.Collections.Generic;
using System.IO;
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
            Classes.manager.Leave = 0;
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
                return;
            }
            if (Data.k1stroyDBEntities.GetContext().Users
                        .Any(d => d.Login == loginTB.Text &&
                        d.Password == passwordPB.Password))
            {
                var user = Data.k1stroyDBEntities.GetContext().Users
                    .Where(d => d.Login == loginTB.Text
                    && d.Password == passwordPB.Password).FirstOrDefault();
                Classes.manager.CurrentUser = user;
                MessageBox.Show($"Добро пожаловать, {user.Surname} {user.Firstname} {user.Patronymic}!","Выполнен вход в учетную запись", MessageBoxButton.OK, MessageBoxImage.Information);
                string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - выполнен вход в учетную запись ({user.Surname} {user.Firstname} {user.Patronymic})";
                File.AppendAllText("log.txt", logline + Environment.NewLine);
                switch (user.Roles.Role)
                {
                    case "Admin":
                        Classes.manager.MainFrame.Navigate(new Pages.ProductPage(user));
                        break;
                    case "Manager":
                        Classes.manager.MainFrame.Navigate(new Pages.ProductPage(user));
                        break;
                    case "Customer":
                        Classes.manager.MainFrame.Navigate(new Pages.ProductPage(user));
                        break;

                }
            }
            else
            {
                switch (loginTB.Text) {
                    case "kittycat":
                        if (passwordPB.Password == "cat") {
                            
                            cat.Visibility = Visibility.Visible;
                            MessageBox.Show("Пасхалка активирована");
                            string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Активирована пасхалка (кот)";
                            File.AppendAllText("log.txt", logline + Environment.NewLine);

                        }
                        else
                        {
                            MessageBox.Show("Неверный Логин или Пароль", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        break;
                    default:
                        MessageBox.Show("Неверный Логин или Пароль", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;






                }
            }

        }
    }
}
