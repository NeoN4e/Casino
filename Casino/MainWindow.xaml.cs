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
        Table table;
        Player human;

        public MainWindow()
        {
            InitializeComponent();

            //Binding bind = new Binding(); // Создаём привязку
            //bind.Path = new PropertyPath("human"); //Смя Свойства!!!!
            //bind.Source = this; // Источник данніх
         
        }
        
        private void StartGame(object sender, RoutedEventArgs re)
        {
            //Создадим стол
            this.table = new Table();

            //Добавим игроков
            this.human = new Player(PlayerType.Human);
                    
            //human.OnCartAdd     += (p, e) => { this.PlayerScore.Content = p.Score; };
            //human.OnOverflow    += (p, c) => { this.PlayerScore.Foreground = Brushes.Red;};
            //human.On21Score     += (p, c) => { this.PlayerScore.Foreground = Brushes.Green; };

            this.table.PlayerList.Add(human);

            Player Casino = new Player(PlayerType.PC);
            //Casino.OnCartAdd += (p, e) => { this.CasinoScore.Content = p.Score; };
            this.table.PlayerList.Add(Casino);
 
            //Начнем
            table.StartGame();

            Binding bind = new Binding(); // Создаём привязку
            //bind.Path = new PropertyPath("list"); //Смя Свойства!!!!
            bind.Source = this.human; // Источник данніх
            //bind.UpdateSourceTrigger = UpdateSourceTrigger.Default;

            //bind.Mode = BindingMode.TwoWay;
            ICPlayer.SetBinding(ItemsControl.ItemsSourceProperty, bind);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.human.AddCard(this.table.GetNextCart());
        }

       // void InitCartImage()
       // {
       //     //Удалить
       //     BitmapImage bi = new BitmapImage(new Uri("Card.PNG", UriKind.RelativeOrAbsolute));
       //     CroppedBitmap cb;
       //     Image img;

       //     for (int i = 0; i < 10; i++)
       //     {
       //         cb = new CroppedBitmap(bi, new Int32Rect(5 + 64 * i, 5, 59, 80));
       //         img = new Image();
       //         img.Source = cb;
       //         img.Margin = new Thickness(5, 0, 0, 0);
       //         this.SpCasino.Children.Add(img);
       //     }
       //}


    }
}
