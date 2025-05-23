using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        public string Path;
        public Data.Products _currentEl = new Data.Products();
        private readonly Action _Update;
        public EditPage(Data.Products product, Action Update)
        {
            InitializeComponent();
            _currentEl = product;
            _Update = Update;
            DataContext = _currentEl;
            CategoryCB.ItemsSource = Data.k1stroyDBEntities.GetContext().ProductCategory.ToList();
            TypeCB.ItemsSource = Data.k1stroyDBEntities.GetContext().ProductType.ToList();
            ManufacturerComboBox.ItemsSource = Data.k1stroyDBEntities.GetContext().Manufacturers.ToList();

            NameTB.Text = _currentEl.ProductName;
            ManufacturerComboBox.SelectedItem = Data.k1stroyDBEntities.GetContext().Manufacturers
                .Where(d => d.ID == _currentEl.Manufacturers.ID).FirstOrDefault();
            CategoryCB.SelectedItem = Data.k1stroyDBEntities.GetContext().ProductCategory
                .Where(d => d.ID == _currentEl.ProductCategory.ID).FirstOrDefault();
            TypeCB.SelectedItem = Data.k1stroyDBEntities.GetContext().ProductType
                .Where(d => d.ID == _currentEl.ProductType.ID).FirstOrDefault();
            ArticleTB.Text = _currentEl.Article;
            PriceTB.Text = _currentEl.Price.ToString();
            InStockTB.Text = _currentEl.InStock.ToString();
            DescTB.Text = _currentEl.Description;
            CharTB.Text = _currentEl.Characteristics;
            


        }

        private void ProductPhotoSelecter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выберите фото",
                Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.EndInit();

                    // Отображаем фото в контроле
                    ProductPhotoSelecter.Source = bitmap;
                    _currentEl.ProductPhotoName = openFileDialog.FileName;
                    Path = _currentEl.ProductPhotoName;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                        
                    
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.manager.MainFrame.CanGoBack)
            {
                Classes.manager.MainFrame.GoBack();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder err = new StringBuilder();
            if (String.IsNullOrEmpty(NameTB.Text))
            {
                err.AppendLine("Введите Наименование!");
            }
            if (String.IsNullOrEmpty(ArticleTB.Text))
            {
                err.AppendLine("Введите Артикул!");
            }
            if (String.IsNullOrEmpty(PriceTB.Text))
            {
                err.AppendLine("Введите Стоимость!");
            }
            if (String.IsNullOrEmpty(InStockTB.Text))
            {
                err.AppendLine("Введите Количество продукта!");
            }
            if (String.IsNullOrEmpty(ManufacturerComboBox.Text))
            {
                err.AppendLine("Выберите Производителя!");
            }
            if (String.IsNullOrEmpty(CategoryCB.Text))
            {
                err.AppendLine("Выберите Категорию!");
            }
            if (String.IsNullOrEmpty(TypeCB.Text))
            {
                err.AppendLine("Выберите Тип продукта!");
            }
            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(),"Внимание!",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    var context = Data.k1stroyDBEntities.GetContext();

                    var selectedType = TypeCB.SelectedItem as Data.ProductType;
                    var selectedManufacturer = ManufacturerComboBox.SelectedItem as Data.Manufacturers;
                    var selectedCategory = CategoryCB.SelectedItem as Data.ProductCategory;

                    _currentEl.ProductName = NameTB.Text;
                    _currentEl.Article = ArticleTB.Text;
                    _currentEl.Price = decimal.Parse(PriceTB.Text);
                    _currentEl.InStock = int.Parse(InStockTB.Text);
                    _currentEl.Description = DescTB.Text;
                    _currentEl.Characteristics = CharTB.Text;

                    _currentEl.TypeID = selectedType.ID;
                    _currentEl.ManufacturerID = selectedManufacturer.ID;
                    _currentEl.CategoryID = selectedCategory.ID;

                    if (!string.IsNullOrEmpty(Path))
                    {
                        _currentEl.ProductPhotoName = System.IO.Path.GetFileName(Path);
                        _currentEl.Photo = File.ReadAllBytes(Path);
                    }

                    context.Entry(_currentEl).State = EntityState.Modified;

                    context.SaveChanges();
                    string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - изменена информация о продукте(ID - {_currentEl.ID})";
                    File.AppendAllText("log.txt", logline + Environment.NewLine);

                    MessageBox.Show("Изменения успешно сохранены!");
                    _Update?.Invoke();
                    if (Classes.manager.MainFrame.CanGoBack)
                    {
                        Classes.manager.MainFrame.GoBack();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Проверьте правильность ввода числовых полей (Цена, Количество)");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                    Debug.WriteLine(ex.ToString());
                }

            }

        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Вы уверены, что хотите удалить этот продукт?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var context = Data.k1stroyDBEntities.GetContext();
                    if (context.Entry(_currentEl).State == EntityState.Detached)
                    {
                        context.Products.Attach(_currentEl);
                    }

                    string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - продукт удален(ID - {_currentEl.ID}, Наименование - {_currentEl.ProductName})";
                    File.AppendAllText("log.txt", logline + Environment.NewLine);

                    context.Products.Remove(_currentEl);
                    context.SaveChanges();

                    MessageBox.Show("Продукт успешно удален!");
                    _Update?.Invoke();
                    if (Classes.manager.MainFrame.CanGoBack)
                    {
                        Classes.manager.MainFrame.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }
    }
}
