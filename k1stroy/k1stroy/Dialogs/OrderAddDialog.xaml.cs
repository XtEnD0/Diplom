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
    /// Логика взаимодействия для OrderAddDialog.xaml
    /// </summary>
    public partial class OrderAddDialog : Window
    {
        public Data.Products CurrentEl { get; set; }
        public Data.Users CurrentUser { get; set; }
        public Action ShowWindow { get; set; }
        public String Status { get; set; }
        public OrderAddDialog(Data.Products selectedProduct, Data.Users CurrUser, Action Show, String _status)
        {
            InitializeComponent();

            CurrentEl = selectedProduct;
            CurrentUser = CurrUser;
            DataContext = this;
            ShowWindow = Show;
            Status = _status;

            StatusCheck();

        }

        private void StatusCheck()
        {
            StatusCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Status.ToList();
            CustomerCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Users.ToList();
            ProductCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Products.ToList();
            StorageFromTB.ItemsSource = Data.k1stroyDBEntities.GetContext().Storages.ToList();

            if (Status == "Selected")
            {
                StatusCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Status
                    .Where(d => d.ID == 1).FirstOrDefault();
                StatusCB.IsEnabled = false;


                CustomerCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Users
                    .Where(d => d.ID == CurrentUser.ID).FirstOrDefault();
                CustomerCB.IsEnabled = false;

                ProductCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Products
                    .Where(d => d.ID == CurrentEl.ID).FirstOrDefault();

                OrderDatePicker.SelectedDate = DateTime.Now;
                OrderDatePicker.IsEnabled = false;
                OrderCompleteDatePicker.IsEnabled = false;

                CountTB.Text = "1";
            }
            else
            {
                OrderDatePicker.SelectedDate = DateTime.Now;

                StatusCB.IsEnabled = true;
                CustomerCB.IsEnabled = true;
                OrderDatePicker.IsEnabled = true;
                OrderCompleteDatePicker.IsEnabled = true;

                CountTB.Text = "1";
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductCB.SelectedItem as Data.Products;
            var selectedCreator = CustomerCB.SelectedItem as Data.Users;
            var selectedStatus = StatusCB.SelectedItem as Data.Status;
            var selectedStorage = StorageFromTB.SelectedItem as Data.Storages;

            Int32.TryParse(CountTB.Text, out int CountRes);
            if(CountRes > selectedProduct.InStock)
            {
                MessageBox.Show("невозможно указать количество большее чем есть на складе", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {

                    

                    var newOrder = new Data.Orders
                    {
                        OrderCreatorID = selectedCreator.ID,
                        OrderProductID = selectedProduct.ID,
                        ContactData = ContactDataTB.Text,
                        CustomerName = CustomerNameTB.Text,
                        StorageFrom = selectedStorage.ID,
                        StorageTo = StorageToTB.Text,
                        Count = CountRes,
                        OrderDate = DateTime.Now,
                        OrderCompleteDate = null,
                        OrderStatusID = selectedStatus.ID
                    };

                    var context = Data.k1stroyDBEntities.GetContext();
                    context.Orders.Add(newOrder);
                    context.SaveChanges();

                    string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - создан Заказ(ID - {newOrder.ID}, заказчик - {selectedCreator.Surname} {selectedCreator.Firstname} {selectedCreator.Patronymic})";
                    File.AppendAllText("log.txt", logline + Environment.NewLine);
                    MessageBox.Show("Заказ успешно Оформлен!");

                    ShowWindow?.Invoke();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания: {ex.Message}\n\n{ex.InnerException?.Message}");
                }
            }

        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ShowWindow?.Invoke();
            Close();
        }
    }
}
