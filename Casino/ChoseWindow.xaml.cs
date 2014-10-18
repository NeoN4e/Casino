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
using System.Windows.Shapes;

namespace Casino
{
    /// <summary>
    /// Логика взаимодействия для ChoseWindow.xaml
    /// </summary>
    public partial class ChoseWindow : Window
    {
        public ChoseWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            
            MainWindow w = new MainWindow();
            w.ShowDialog();

            this.Show();
        }

    }
}
