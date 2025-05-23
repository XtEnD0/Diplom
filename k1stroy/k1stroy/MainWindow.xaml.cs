using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace k1stroy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Classes.manager.GetImageData();
            string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - запуск клиента";
            File.AppendAllText("log.txt", logline + Environment.NewLine);
            Classes.manager.MainFrame = MainFrame;
            Classes.manager.MainFrame.Navigate(new Pages.LoginPage());
            this.Closing += MainWindow_Closing;
    }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (Classes.manager.Leave != 0)
            {
            var dialog = new Dialogs.ConfirmClose();
                        dialog.Owner = this;
                        dialog.ShowDialog();
                        if (!dialog.IsConfirmed)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - клиент закрыт";
                            File.AppendAllText("log.txt", logline + Environment.NewLine);
                        }
            }
            else
            {
                string logline = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - клиент закрыт";
                File.AppendAllText("log.txt", logline + Environment.NewLine);
            }
            
        }
    }

    //private static k1stroyDBEntities _context;
    //public static k1stroyDBEntities GetContext()
    //{
    //    if (_context == null)
    //    {
    //        _context = new k1stroyDBEntities();
    //    }
    //    return _context;
    //}
}

