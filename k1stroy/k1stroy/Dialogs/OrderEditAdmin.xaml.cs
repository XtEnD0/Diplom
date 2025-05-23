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
    /// Логика взаимодействия для OrderEditAdmin.xaml
    /// </summary>
    public partial class OrderEditAdmin : Window
    {
        public Data.Orders _currentOrder { get; set; }
        public OrderEditAdmin(Data.Orders selectedOrder)
        {
            InitializeComponent();
            _currentOrder = selectedOrder;
            StatusCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Status.ToList();
            CustomerCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Users.ToList();
            ProductCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Products.ToList();

            StatusCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Status
                .Where(d => d.ID == _currentOrder.Status.ID).FirstOrDefault();

            CustomerCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Users
                .Where(d => d.ID == _currentOrder.Users.ID).FirstOrDefault();

            ProductCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Products
                .Where(d => d.ID == _currentOrder.Products.ID).FirstOrDefault();

            OrderDatePicker.SelectedDate = _currentOrder.OrderDate;
            OrderCompleteDatePicker.SelectedDate = _currentOrder.OrderCompleteDate;
            CustomerNameTB.Text = _currentOrder.CustomerName;
            ContactDataTB.Text = _currentOrder.ContactData;
            StorageFromTB.Text = _currentOrder.StorageFrom;
            StorageToTB.Text = _currentOrder.StorageTo;
            CountTB.Text = _currentOrder.Count.ToString();

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(CountTB.Text, out int CountRes);

            var context = Data.k1stroyDBEntities.GetContext();

            var selectedStatus = StatusCB.SelectedItem as Data.Status;
            _currentOrder.OrderStatusID = selectedStatus.ID;

            var selectedProduct = ProductCB.SelectedItem as Data.Products;
            _currentOrder.OrderProductID = selectedProduct.ID;

            var selectedCustomer = CustomerCB.SelectedItem as Data.Users;
            _currentOrder.OrderCreatorID = selectedCustomer.ID;

            _currentOrder.OrderDate = OrderDatePicker.SelectedDate;
            _currentOrder.OrderCompleteDate = OrderCompleteDatePicker.SelectedDate;
            _currentOrder.CustomerName = CustomerNameTB.Text;
            _currentOrder.ContactData = ContactDataTB.Text;
            _currentOrder.StorageFrom = StorageFromTB.Text;
            _currentOrder.StorageTo = StorageToTB.Text;
            _currentOrder.Count = CountRes;

            context.Entry(_currentOrder).State = EntityState.Modified;
            context.SaveChanges();

            string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - изменена информация о заказе (ID - {_currentOrder.ID})";
            File.AppendAllText("log.txt", logline + Environment.NewLine);

            MessageBox.Show("Изменения успешно сохранены!");
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
