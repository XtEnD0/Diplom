using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace k1stroy.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public Data.Users CurrentUser { get; set; }
        public ProductPage(Data.Users user)
        {
            CurrentUser = user;
            DataContext = this;
            Classes.manager.Leave = 1;

            InitializeComponent();
            RoleChecker();
            NoFounds.Visibility = Visibility.Collapsed;
            LoadProducts();
        }

        private bool _isMenuOpen = false;
        public ObservableCollection<Data.Products> Products { get; set; }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            _isMenuOpen = !_isMenuOpen;

            var animation = new ThicknessAnimation
            {
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
            var animation_but = new ThicknessAnimation
            {
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            animation.To = _isMenuOpen ? new Thickness(0, 0, 0, 0) : new Thickness(-350, 0, 0, 0);
            SideMenu.BeginAnimation(Border.MarginProperty, animation);

            animation.To = _isMenuOpen ? new Thickness(350, 0, 0, 0) : new Thickness(0, 0, 0, 0);
            HamburgerButton.BeginAnimation(Border.MarginProperty, animation);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.manager.MainFrame.CanGoBack)
            {
                Classes.manager.Leave = 0;
                string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - произведен выход из учетной записи";
                File.AppendAllText("log.txt", logline + Environment.NewLine);
                Classes.manager.MainFrame.GoBack();
                Logout();
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.ProfileDialog(Classes.manager.CurrentUser);

            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Данные сохранены успешно!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateProfilePicture();
                NameBlock.Text = CurrentUser.Surname + " " + CurrentUser.Firstname + " " + CurrentUser.Patronymic;
            }
        }
        private void LoadProducts()
        {
            var products = Data.k1stroyDBEntities.GetContext().Products.ToList();
            Products = new ObservableCollection<Data.Products>(products);
            ProductsListView.ItemsSource = Products;

            var categoryList = Data.k1stroyDBEntities.GetContext().ProductCategory.ToList();
            categoryList.Insert(0, new Data.ProductCategory { CategoryName = "Все Категории" });
            CategoryComboBox.ItemsSource = categoryList;
            CategoryComboBox.SelectedIndex = 0;

            var manufacturerList = Data.k1stroyDBEntities.GetContext().Manufacturers.ToList();
            manufacturerList.Insert(0, new Data.Manufacturers { ManufacturerName = "Все Производители" });
            ManufacturerComboBox.ItemsSource = manufacturerList;
            ManufacturerComboBox.SelectedIndex = 0;
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
        private void Logout()
        {
            if (PFP?.Source is BitmapImage bitmap)
            {
                bitmap.StreamSource?.Dispose();
                bitmap.UriSource = null;
                PFP.Source = null;
            }

            Classes.manager.CurrentUser = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public List<Data.Products> _products = Data.k1stroyDBEntities.GetContext().Products.ToList();
        public void Update()
        {
            try
            {
                ListGrid.Visibility = Visibility.Visible;
                NoFounds.Visibility = Visibility.Collapsed;

                _products = Data.k1stroyDBEntities.GetContext().Products.ToList();

                if (!string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    _products = Data.k1stroyDBEntities.GetContext().Products
                        .Where(p => p.ProductName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Description.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Manufacturers.ManufacturerName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Price.ToString().Contains(SearchTextBox.Text))
                        .ToList();
                }

                var selectedCategory = CategoryComboBox.SelectedItem as Data.ProductCategory;
                if (selectedCategory != null && selectedCategory.CategoryName != "Все Категории")
                {
                    _products = _products
                        .Where(p => p.ProductCategory != null && p.ProductCategory.ID == selectedCategory.ID)
                        .ToList();
                }

                var selectedManufacturer = ManufacturerComboBox.SelectedItem as Data.Manufacturers;
                if (selectedManufacturer != null && selectedManufacturer.ManufacturerName != "Все Производители")
                {
                    _products = _products
                        .Where(p => p.Manufacturers != null && p.Manufacturers.ID == selectedManufacturer.ID)
                        .ToList();
                }

                ProductsListView.ItemsSource = _products;

                if (_products.Count == 0)
                {
                    ListGrid.Visibility = Visibility.Collapsed;
                    NoFounds.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RoleChecker()
        {
            switch (CurrentUser.Roles.Role)
            {
                case "Admin":
                    Settings.Visibility = Visibility.Visible;
                    Add.Visibility = Visibility.Visible;
                    OrderCheck.Visibility = Visibility.Visible;
                    AllSettings.Visibility = Visibility.Visible;
                    break;
                case "Manager":
                    Settings.Visibility = Visibility.Visible;
                    Add.Visibility = Visibility.Collapsed;
                    OrderCheck.Visibility = Visibility.Visible;
                    AllSettings.Visibility = Visibility.Collapsed;
                    break;
                case "Customer":
                    Settings.Visibility = Visibility.Visible;
                    Add.Visibility = Visibility.Collapsed;
                    OrderCheck.Visibility = Visibility.Visible;
                    AllSettings.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentUser.Roles.Role != "Customer")
            {

                var productListPage = Classes.manager.MainFrame.Content as ProductPage;

                if ((sender as Border).DataContext is Data.Products selectedProduct)
                {

                    Classes.manager.MainFrame.Navigate(
                        new Pages.EditPage(selectedProduct, productListPage.Update));
                }

            }
            else
            {
                var productListPage = Classes.manager.MainFrame.Content as ProductPage;
                if ((sender as Border).DataContext is Data.Products selectedProduct)
                {
                    var dialog = new Dialogs.ConfirmOrder(selectedProduct, CurrentUser);
                    var ownerWindow = Window.GetWindow((DependencyObject)sender);
                    dialog.Owner = ownerWindow;
                    dialog.ShowDialog();
                }
                    
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Classes.manager.MainFrame.Navigate(new Pages.AddPage(Update));
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void OrderCheck_Click(object sender, RoutedEventArgs e)
        {
            string RoleCheck = "";
            switch (CurrentUser.Roles.Role)
            {
                case "Admin":
                    RoleCheck = "Admin";
                    Classes.manager.MainFrame.Navigate(new Pages.OrderObservePage(RoleCheck, CurrentUser.ID));
                    break;
                case "Manager":
                    RoleCheck = "Manager";
                    Classes.manager.MainFrame.Navigate(new Pages.OrderObservePage(RoleCheck, CurrentUser.ID));
                    break;
                case "Customer":
                    RoleCheck = "Customer";
                    Classes.manager.MainFrame.Navigate(new Pages.OrderObservePage(RoleCheck, CurrentUser.ID));
                    break;
            }

        }

        private void AllSettings_Click(object sender, RoutedEventArgs e)
        {
            Classes.manager.MainFrame.Navigate(new Pages.UserParametersPage());
        }
    }
}
