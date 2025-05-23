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
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        private readonly Action _Update;
        public string Path;
        public AddPage(Action Update)
        {
            InitializeComponent();
            _Update = Update;

            CategoryCB.ItemsSource = Data.k1stroyDBEntities.GetContext().ProductCategory.ToList();
            TypeCB.ItemsSource = Data.k1stroyDBEntities.GetContext().ProductType.ToList();
            ManufacturerComboBox.ItemsSource = Data.k1stroyDBEntities.GetContext().Manufacturers.ToList();

            BitmapImage bitmap = new BitmapImage(new Uri("/Res/imagenull.png", UriKind.Relative));
            ProductPhotoSelecter.Source = bitmap;


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
                MessageBox.Show(err.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    byte[] photoBytes = null;
                    if (!string.IsNullOrEmpty(Path))
                    {
                        try
                        {
                            photoBytes = File.ReadAllBytes(Path);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка чтения файла: {ex.Message}");
                            return;
                        }
                    }

                    var newProduct = new Data.Products
                    {
                        ProductName = NameTB.Text,
                        ManufacturerID = ((Data.Manufacturers)ManufacturerComboBox.SelectedItem).ID,
                        CategoryID = ((Data.ProductCategory)CategoryCB.SelectedItem).ID,
                        TypeID = ((Data.ProductType)TypeCB.SelectedItem).ID,
                        Article = ArticleTB.Text,
                        Price = Decimal.Parse(PriceTB.Text),
                        InStock = Int32.Parse(InStockTB.Text),
                        Description = DescTB.Text,
                        Characteristics = CharTB.Text,
                        ProductPhotoName = Path,
                        Photo = photoBytes
                    };

                    var context = Data.k1stroyDBEntities.GetContext();
                    context.Products.Add(newProduct);
                    context.SaveChanges();

                    string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - создан продукт(ID - {newProduct.ID}, Наименование - {newProduct.ProductName})";
                    File.AppendAllText("log.txt", logline + Environment.NewLine);

                    _Update?.Invoke();
                    MessageBox.Show("Товар успешно добавлен!");
                    Classes.manager.MainFrame.GoBack();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                    MessageBox.Show($"Ошибки валидации:\n{string.Join("\n", errorMessages)}");
                }
            }

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

                    ProductPhotoSelecter.Source = bitmap;
                    Path = openFileDialog.FileName;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());


                }
            }
        }
    }
}
