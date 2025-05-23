using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для UserEdit.xaml
    /// </summary>
    public partial class UserEdit : Window
    {
        private Data.Users _currentUser { get; set; }
        private readonly Action _Update;
        public UserEdit(Data.Users user, Action load)
        {
            InitializeComponent();
            _currentUser = user;
            _Update = load;
            RoleCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Roles.ToList();

            FirstnameTB.Text = _currentUser.Firstname;
            SurnameTB.Text = _currentUser.Surname;
            PatronymicTB.Text = _currentUser.Patronymic;
            RoleCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Roles
                .Where(d => d.ID == _currentUser.RoleID).FirstOrDefault();
            LoginTB.Text = _currentUser.Login;
            PasswordTB.Text = _currentUser.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var context = Data.k1stroyDBEntities.GetContext();

                var selectedRole = RoleCB.SelectedItem as Data.Roles;

                _currentUser.Firstname = FirstnameTB.Text;
                _currentUser.Surname = SurnameTB.Text;
                _currentUser.Patronymic = PatronymicTB.Text;
                _currentUser.Login = LoginTB.Text;
                _currentUser.Password = PasswordTB.Text;
                _currentUser.RoleID = selectedRole.ID;

                context.Entry(_currentUser).State = EntityState.Modified;

                context.SaveChanges();

                string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Отредактирован пользователь ({_currentUser.ID}, {_currentUser.Surname} {_currentUser.Firstname} {_currentUser.Patronymic})";
                File.AppendAllText("log.txt", logline + Environment.NewLine);

                MessageBox.Show("Изменения сохранены!");
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
