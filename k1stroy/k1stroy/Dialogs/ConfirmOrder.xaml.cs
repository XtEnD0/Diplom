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
    /// Логика взаимодействия для ConfirmOrder.xaml
    /// </summary>
    public partial class ConfirmOrder : Window
    {
        public Data.Products CurrentEl { get; set; }
        public Data.Users CurrentUser { get; set; }
        public string ProductSin { get; set; }
        public ConfirmOrder(Data.Products selectedProduct, Data.Users CurrUser)
        {
            InitializeComponent();

            CurrentEl = selectedProduct;
            CurrentUser = CurrUser;
            ProductSin = CurrentEl.ProductName.ToString();
            DataContext = this;

            NameTB.Text = CurrentEl.ProductName;
            ArticleTB.Text = CurrentEl.Article;
            PriceTB.Text = CurrentEl.Price.ToString();
            InStockTB.Text = CurrentEl.InStock.ToString();
            DescTB.Text = CurrentEl.Description;
            CharTB.Text = CurrentEl.Characteristics;
            ManufacturerComboBox.Text = CurrentEl.Manufacturers.ManufacturerName;
            CategoryCB.Text = CurrentEl.ProductCategory.CategoryName;
            TypeCB.Text = CurrentEl.ProductType.TypeName;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEl.InStock != 0)
            {

                try
                {
                    var newOrder = new Data.Orders
                    {
                        OrderCustomerID = CurrentUser.ID,
                        OrderProductID = CurrentEl.ID,
                        OrderDate = DateTime.Now,
                        OrderCompleteDate = null,
                        OrderStatusID = 1
                    };

                    var context = Data.k1stroyDBEntities.GetContext();
                    context.Orders.Add(newOrder);
                    context.SaveChanges();

                    string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - создан Заказ(ID - {newOrder.ID}, заказчик - {CurrentUser.Surname} {CurrentUser.Firstname} {CurrentUser.Patronymic})";
                    File.AppendAllText("log.txt", logline + Environment.NewLine);
                    MessageBox.Show("Заказ успешно создан!");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания: {ex.Message}\n\n{ex.InnerException?.Message}");
                }
            }
            else
            {
                MessageBox.Show("товара нет в наличии","Ошибка!");
            }
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
