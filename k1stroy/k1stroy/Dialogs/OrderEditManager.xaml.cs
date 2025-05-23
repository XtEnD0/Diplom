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
    /// Логика взаимодействия для OrderEditManager.xaml
    /// </summary>
    public partial class OrderEditManager : Window
    {
        public Data.Orders _currentOrder { get; set; }
        public string OldStatus { get; set; }
        public OrderEditManager(Data.Orders selectedOrder)
        {
            InitializeComponent();
            _currentOrder = selectedOrder;
            StatusCB.ItemsSource = Data.k1stroyDBEntities.GetContext().Status.ToList();
            StatusCB.SelectedItem = Data.k1stroyDBEntities.GetContext().Status
                .Where(d => d.ID == _currentOrder.Status.ID).FirstOrDefault();
            OldStatus = StatusCB.Text;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var context = Data.k1stroyDBEntities.GetContext();
            var selectedStatus = StatusCB.SelectedItem as Data.Status;
            _currentOrder.OrderStatusID = selectedStatus.ID;
            if(selectedStatus.ID == 4|| selectedStatus.ID == 5)
            {
                _currentOrder.OrderCompleteDate = DateTime.Now;
            }
            else
            {
                _currentOrder.OrderCompleteDate = null;
            }
            context.Entry(_currentOrder).State = EntityState.Modified;
            context.SaveChanges();

            string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - изменен статус заказа (ID - {_currentOrder.ID}) с {OldStatus} на {selectedStatus.StatusName}";
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
