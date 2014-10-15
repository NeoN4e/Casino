using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    /// <summary>
    /// Класс Сонвертер для отображения на Карт На Форме
    /// </summary>
    [System.Windows.Data.ValueConversion(typeof(Cart), typeof(string))]
    class CartImgConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Cart) return String.Format("Карта с номиналом {0}", (value as Cart).Score);
            else return value.ToString();

            //return new BitmapImage(
            //    new Uri(
            //        System.IO.Directory.GetCurrentDirectory() + "\\" + (string)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>Описывает карты</summary>
    [Serializable]
    class Cart 
    {
        public int Score;
        public Cart(int score)

        { this.Score = score; }

        public System.Windows.Controls.Image GetImage()
        {
            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage(new Uri("Card.PNG", UriKind.RelativeOrAbsolute));
            System.Windows.Media.Imaging.CroppedBitmap cb = new System.Windows.Media.Imaging.CroppedBitmap(bi, new System.Windows.Int32Rect(5 + 64 * 0, 5, 59, 80));
           
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Source = cb;
            img.Margin = new System.Windows.Thickness(5, 0, 0, 0);

            return img;
        }

        public override string ToString()
        {
            return ""+this.Score+" ";
        }
    }

    /// <summary>ТипИгрока</summary>
    enum PlayerType {Human,PC}

    /// <summary>Описывает игрока казино</summary>
    [Serializable]
    class Player : IEnumerable<Cart> , INotifyCollectionChanged
    {
        /// <summary>Набранные Очки</summary>
        int score = 0;
        public int Score 
        { 
            get { return this.score; }
            private set 
            { 
                this.score = value;

                if (this.score == 21 && On21Score != null) On21Score(this, null);

                if (this.score > 21 && OnOverflow != null) OnOverflow(this, null);
            }
        }
        
        /// <summary>Человек или Компьютер</summary>
        PlayerType pt;

        /// <summary>Коллекция Карт Игрока</summary>
        List<Cart> CartPool = new List<Cart>();

        public Player(PlayerType pt = PlayerType.PC)
        {
            this.pt = pt;
        }

        /// <summary>Являеться ли текущий игрок человеком</summary>
        /// <returns></returns>
        public bool isHuman()
        {
            if (this.pt == PlayerType.Human) return true;
            return false;
        }

        /// <summary>Добавляет карту в руку</summary>
        /// <returns>Количество очков после добавления</returns>
        public int AddCard(Cart c)
        {
            CartPool.Add(c);
            this.Score += c.Score;

            if (OnCartAdd != null) OnCartAdd(this, c);

            if (CollectionChanged != null) CollectionChanged(this,new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            return this.Score;
        }

        /// <summary>Начать новый раунд(сбросить карты и счет)</summary>
        public void Reset()
        {
            CartPool.Clear();
            this.Score = 0;
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>Получение карты игроком</summary>
        public event EventDelegate OnCartAdd;
        public delegate void EventDelegate(Player p,Cart c);

        /// <summary>Очко</summary>
        public event EventDelegate On21Score;

        /// <summary>Перебор</summary>
        public event EventDelegate OnOverflow;

        public IEnumerator<Cart> GetEnumerator()
        {
           //return this.CartPool.GetEnumerator();
            foreach (Cart item in CartPool)
            { 
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.CartPool.GetEnumerator();
        }


        public event NotifyCollectionChangedEventHandler CollectionChanged;

    }
   
}
