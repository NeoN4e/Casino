using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
   /// <summary>Описывает карты</summary>
    [Serializable]
    class Cart 
    {
        /// <summary>Масть Карты Пика Крестик Чирва Бубна</summary>
        public enum CartType { Picas, Clovers, Hearts, Squares }

        /// <summary>Очки текущей Карты</summary>
        public int Score
        { 
            get
            { 
                if (cartId == 12) return 11; //туз
                if (cartId <= 8) return this.cartId + 2; //От 2-х до 10
                else return this.cartId - 7; //Валет дама король
            }
        }

        /// <summary>Картинк текущей карты</summary>
        public System.Windows.Media.Imaging.CroppedBitmap ImgeSourse
        {
            get
            {
                //System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage(new Uri("Card.PNG", UriKind.RelativeOrAbsolute));
                System.Windows.Media.Imaging.BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Card.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                System.Windows.Media.Imaging.CroppedBitmap cb = new System.Windows.Media.Imaging.CroppedBitmap(bs, new System.Windows.Int32Rect(5 + 64 * this.cartId, 5 + 85 * (int)this.ct, 59, 80));
                
                return cb;
            }
        }

        /// <summary>порядковый номер карты от 0 до 12(туз)</summary>
        int cartId;
        /// <summary>Масть</summary>
        CartType ct;

        /// <summary>Конструктор</summary>
        public Cart(int id,CartType type) 
        { 
            this.cartId = id;
            this.ct = type;
        }
    }

    /// <summary>Описывает игрока казино</summary>
    [Serializable]
    class Player : IEnumerable<Cart> , INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>ТипИгрока</summary>
        public enum PlayerType { Human, PC }

        /// <summary>Набранные Очки</summary>
        int score = 0;
        public int Score 
        { 
            get { return this.score; }
            private set 
            { 
                this.score = value;

                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                //if (this.score == 21 && On21Score != null) On21Score(this, null);

                //if (this.score > 21 && OnOverflow != null) OnOverflow(this, null);
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

            //if (OnCartAdd != null) OnCartAdd(this, c);

            if (CollectionChanged != null) CollectionChanged(this ,new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

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
        //public event EventDelegate OnCartAdd;
        //public delegate void EventDelegate(Player p,Cart c);

        /// <summary>Очко</summary>
        //public event EventDelegate On21Score;

        /// <summary>Перебор</summary>
        //public event EventDelegate OnOverflow;

        #region Интерфейсы
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
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
   
}
