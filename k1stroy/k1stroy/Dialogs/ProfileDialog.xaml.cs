using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для ProfileDialog.xaml
    /// </summary>
    public partial class ProfileDialog : Window
    {
        public string oldUser;
        public string newUser;
        public static string sur;
        public static string nam;
        public static string pat;
        public Data.Users CurrentUser { get; set; }
        public ProfileDialog(Data.Users user)
        {
            InitializeComponent();
            CurrentUser = user;
            DataContext = this;
            sur = CurrentUser.Surname;
            nam = CurrentUser.Firstname;
            pat = CurrentUser.Patronymic;

            oldUser = CurrentUser.Surname+" "+CurrentUser.Firstname+" "+CurrentUser.Patronymic;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder err = new StringBuilder();
                if (String.IsNullOrEmpty(SurnameBox.Text))
                {
                    err.AppendLine("Фамилия не может быть пустой!");
                }
                if (String.IsNullOrEmpty(FirstNameBox.Text))
                {
                    err.AppendLine("Имя не может быть пустым!");
                }
                if (err.Length > 0)
                {
                    CurrentUser.Surname = sur;
                    CurrentUser.Firstname = nam;
                    CurrentUser.Patronymic = pat;
                    SurnameBox.Text = sur;
                    FirstNameBox.Text = nam;
                    PatronymicBox.Text = pat;
                    MessageBox.Show(err.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                Data.k1stroyDBEntities.GetContext().SaveChanges();
                DialogResult = true;
                newUser = CurrentUser.Surname + " " + CurrentUser.Firstname + " " + CurrentUser.Patronymic;
                string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - изменена информация о пользователе({oldUser} -> {newUser})";
                File.AppendAllText("log.txt", logline + Environment.NewLine);
                Close();    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Surname = sur;
            CurrentUser.Firstname = nam;
            CurrentUser.Patronymic = pat;
            SurnameBox.Text = sur;
            FirstNameBox.Text = nam;
            PatronymicBox.Text = pat;
            DialogResult = false;
            Close();
        }

        private void PFP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выберите фото профиля",
                Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    CurrentUser.PfpName = openFileDialog.FileName;
                    CurrentUser.PfpPicture = File.ReadAllBytes(openFileDialog.FileName);
                    string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - изменена информация о пользователе(Фотография профиля)";
                    File.AppendAllText("log.txt", logline + Environment.NewLine);
                    Data.k1stroyDBEntities.GetContext().SaveChanges();
                    UpdateProfilePicture();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            MessageBox.Show(err.ErrorMessage + "");
                        }
                    }
                }
            }
        }
        private void UpdateProfilePicture()
        {
            if (Classes.manager.CurrentUser?.PfpPicture != null)
            {
                using (var stream = new MemoryStream(Classes.manager.CurrentUser.PfpPicture))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    PFP.Source = image;
                }
            }
        }
    }
}
