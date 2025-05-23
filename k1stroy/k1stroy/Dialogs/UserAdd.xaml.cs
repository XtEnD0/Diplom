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
using System.Windows.Shapes;

namespace k1stroy.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для UserAdd.xaml
    /// </summary>
    public partial class UserAdd : Window
    {
        private readonly Action _Update;
        public UserAdd(Action load)
        {
            _Update = load;
            InitializeComponent();
            RoleCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Roles.ToList();

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRole = ((Data.Roles)RoleCB.SelectedItem).ID;

            try
            {
                var newUser = new Data.Users
                {
                    Firstname = FirstnameTB.Text,
                    Surname = SurnameTB.Text,
                    Patronymic = PatronymicTB.Text,
                    Login = LoginTB.Text,
                    Password = PasswordTB.Text,
                    RoleID = selectedRole
                };

                var context = Data.k1stroyDBEntities.GetContext();
                context.Users.Add(newUser);
                context.SaveChanges();

                string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Зарегистрирован пользователь({newUser.ID}, {newUser.Surname} {newUser.Firstname} {newUser.Patronymic})";
                File.AppendAllText("log.txt", logline + Environment.NewLine);

                MessageBox.Show("Пользователь успешно добавлен!");
                _Update?.Invoke();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}\n\n{ex.InnerException?.Message}");
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
