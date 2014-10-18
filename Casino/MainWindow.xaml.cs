using System;
using System.Collections.Generic;
//using System.Linq;
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

namespace Casino
{
    
    public partial class MainWindow : Window
    {
        Game Table;
        
        /// <summary>Бинд объектов на форму</summary>
        void Bind()
        {
            //Бинд объектов на форму
            Binding CasinoBind = new Binding();
            CasinoBind.Source = Table.Pc;
            LbCasino.SetBinding(Label.ContentProperty, CasinoBind);


            Binding PlayerBind = new Binding();
            PlayerBind.Source = Table.Human;
            LbPlayer.SetBinding(Label.ContentProperty, PlayerBind);

            Binding StatysBind = new Binding();
            StatysBind.Path = new PropertyPath("GameStatys");
            StatysBind.Source = Table;
            LbWiner.SetBinding(Label.ContentProperty, StatysBind);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string tableId)
        {
            InitializeComponent();

            int gameId = Int32.Parse(tableId);
            this.Table = Game.LoadGame(gameId);
            this.Title = "Стол № " + gameId;
            Bind();
        }

        private void StartGame(object sender, RoutedEventArgs re)
        {
            //Создадим стол игру
            //this.Table = new Game();
            Table.StartGame();
            Bind();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PlayerGetCart(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Table.HumanAddCart();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Для начала нужно нажать СТАРТ!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EndGame(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Table.EndGame();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Для начала нужно нажать СТАРТ!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Table != null)
            {
                Game.SaveGame(this.Table);
            }
        }


    }
}
