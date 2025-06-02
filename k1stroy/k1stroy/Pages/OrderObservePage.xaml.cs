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
    /// Логика взаимодействия для OrderObservePage.xaml
    /// </summary>
    public partial class OrderObservePage : Page
    {
        public string role { get; set; }
        public int UserID { get; set; }
        public Data.Users Us { get; set; }
        public OrderObservePage(string RoleCheck, int CurrrentUserID)
        {
            role = RoleCheck;
            UserID = CurrrentUserID;
            InitializeComponent();
            NoOrders.Visibility = Visibility.Collapsed;
            NoFoundOrders.Visibility = Visibility.Collapsed;
            LoadProducts();
            

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
        private void LoadProducts()
        {
            if (role != "Customer")
            {
                OrdersViewCustomer.Visibility = Visibility.Collapsed;
                OrdersView.ItemsSource = Data.k1stroyDBEntities.GetContext().Orders.ToList();
            }
            else
            {
                OrdersView.Visibility = Visibility.Collapsed;
                OrdersViewCustomer.Visibility = Visibility.Visible;
                OrdersViewCustomer.ItemsSource = Data.k1stroyDBEntities.GetContext().Orders
                    .Where(d => d.Users.ID == UserID).ToList();
                if(Data.k1stroyDBEntities.GetContext().Orders
                    .Where(d => d.Users.ID == UserID).ToList().Count == 0)
                {
                    OrdersView.Visibility = Visibility.Collapsed;
                    OrdersViewCustomer.Visibility = Visibility.Collapsed;
                    NoOrders.Visibility = Visibility.Visible;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.manager.MainFrame.CanGoBack)
            {
                Classes.manager.MainFrame.GoBack();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void StatusCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox &&
               comboBox.DataContext is Data.Orders selectedOrder &&
               comboBox.SelectedItem is Data.Status selectedStatus)
            {
                selectedOrder.OrderStatusID = selectedStatus.ID;
                selectedOrder.Status = selectedStatus;

                Data.k1stroyDBEntities.GetContext().SaveChanges();
                OrdersView.Items.Refresh();
            }
        }

        private void OrderCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Data.Orders selectedOrder)
            {
                selectedOrder.OrderStatusID = 5;
                selectedOrder.OrderCompleteDate = DateTime.Now;
                Data.k1stroyDBEntities.GetContext().SaveChanges();
                OrdersViewCustomer.Items.Refresh();
            }
        }

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if(role == "Manager")
            {
                var OrderObservePage = Classes.manager.MainFrame.Content as OrderObservePage;
                if (sender is Button button && button.DataContext is Data.Orders selectedOrder)
                {
                    var dialog = new Dialogs.OrderEditManager(selectedOrder);
                    dialog.Owner = Application.Current.Windows.OfType<Window>()
                                   .FirstOrDefault(w => w.IsActive);
                    dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    dialog.ShowDialog();
                    OrdersView.Items.Refresh();

                }
            }
            if(role == "Admin")
            {
                var OrderObservePage = Classes.manager.MainFrame.Content as OrderObservePage;
                if (sender is Button button && button.DataContext is Data.Orders selectedOrder)
                {
                    var dialog = new Dialogs.OrderEditAdmin(selectedOrder);
                    dialog.Owner = Application.Current.Windows.OfType<Window>()
                                   .FirstOrDefault(w => w.IsActive);
                    dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    dialog.ShowDialog();
                    OrdersView.Items.Refresh();

                }
            }
            
        }

        public List<Data.Orders> _orders = Data.k1stroyDBEntities.GetContext().Orders.ToList();
        private void Update()
        {
            _orders = Data.k1stroyDBEntities.GetContext().Orders.ToList();

            if(role == "Manager"|| role == "Admin")
            {
                if (!string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    _orders = Data.k1stroyDBEntities.GetContext().Orders
                        .Where(p => p.Products.ProductName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Users.Firstname.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Users.Surname.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Users.Patronymic.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.ID.ToString().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.OrderDate.ToString().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.OrderCompleteDate.ToString().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.ContactData.ToLower().Contains(SearchTextBox.Text.ToLower())||
                                   p.CustomerName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Storages.Storage.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.StorageTo.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Products.Price.ToString().Contains(SearchTextBox.Text))
                        
                        .ToList();
                }
                OrdersView.ItemsSource = _orders;
                if (_orders.Count == 0)
                {
                    NoFoundOrders.Visibility = Visibility.Visible;
                    OrdersView.Visibility = Visibility.Collapsed;
                }
                else
                {
                    NoFoundOrders.Visibility = Visibility.Collapsed;
                    OrdersView.Visibility = Visibility.Visible;
                }
            }
            else
            {
                _orders = Data.k1stroyDBEntities.GetContext().Orders
                    .Where(d => d.Users.ID == UserID).ToList();

                if (!string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    _orders = Data.k1stroyDBEntities.GetContext().Orders
                        .Where(p =>
                        (
                                   p.Products.ProductName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Users.Firstname.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Users.Surname.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Users.Patronymic.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.ID.ToString().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.OrderDate.ToString().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.OrderCompleteDate.ToString().Contains(SearchTextBox.Text.ToLower()) ||
                                   p.Products.Price.ToString().Contains(SearchTextBox.Text))&&
                                   p.OrderCreatorID == UserID) 
                                   
                        .ToList();
                }
                OrdersViewCustomer.ItemsSource = _orders;
                if(_orders.Count == 0)
                {
                    NoFoundOrders.Visibility = Visibility.Visible;
                    OrdersViewCustomer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    NoFoundOrders.Visibility = Visibility.Collapsed;
                    OrdersViewCustomer.Visibility = Visibility.Visible;
                }
            }

            
            
            
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.OrderAddDialog(null, null, Update, null);
            var ownerWindow = Window.GetWindow((DependencyObject)sender);
            dialog.Owner = ownerWindow;
            dialog.ShowDialog();
        }
    }
}
