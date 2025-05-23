using System;
using System.Collections.Generic;
using System.IO;
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

namespace k1stroy.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ConfirmClose.xaml
    /// </summary>
    public partial class ConfirmClose : Window
    {
        public bool IsConfirmed { get; private set; }
        public ConfirmClose()
        {
            InitializeComponent();

        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            this.Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.Close();
        }
   
    }
}
