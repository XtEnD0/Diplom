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
    /// Логика взаимодействия для UserParametersPage.xaml
    /// </summary>
    public partial class UserParametersPage : Page
    {
        public UserParametersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UsersList.ItemsSource = Data.k1stroyDBEntities.GetContext().Users.ToList();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.manager.MainFrame.CanGoBack)
            {
                Classes.manager.MainFrame.GoBack();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var OrderObservePage = Classes.manager.MainFrame.Content as OrderObservePage;

                var dialog = new Dialogs.UserAdd(LoadUsers);
                dialog.Owner = Application.Current.Windows.OfType<Window>()
                               .FirstOrDefault(w => w.IsActive);
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dialog.ShowDialog();

            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        public List<Data.Users> _users = Data.k1stroyDBEntities.GetContext().Users.ToList();
        private void Update()
        {
            _users = Data.k1stroyDBEntities.GetContext().Users.ToList();

                if (!string.IsNullOrEmpty(SearchTextBox.Text))
                {
                _users = Data.k1stroyDBEntities.GetContext().Users
                        .Where(p =>p.Firstname.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Surname.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Roles.Role.ToLower().Contains(SearchTextBox.Text.ToLower())||
                                   p.Login.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Password.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Patronymic.ToLower().Contains(SearchTextBox.Text.ToLower()))
                        .ToList();
                }
               UsersList.ItemsSource = _users;
            }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            var OrderObservePage = Classes.manager.MainFrame.Content as OrderObservePage;
            if (sender is Button button && button.DataContext is Data.Users selectedUser)
            {
                var dialog = new Dialogs.UserEdit(selectedUser, LoadUsers);
                dialog.Owner = Application.Current.Windows.OfType<Window>()
                               .FirstOrDefault(w => w.IsActive);
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dialog.ShowDialog();

            }
        }
    }
}
