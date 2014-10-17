using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    /// <summary>Описывает игрока казино</summary>
    [Serializable]
    class Player : IEnumerable<Cart>, INotifyCollectionChanged, INotifyPropertyChanged
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

                //Уведомление об изменении коллекции( для отображения на форме)
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Score"));
             }
        }

        /// <summary>Человек или Компьютер</summary>
        public PlayerType pt {get; private set;}

        /// <summary>Коллекция Карт Игрока</summary>
        List<Cart> CartPool = new List<Cart>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pt">Тип игроки ПС или Хуман</param>
        public Player(PlayerType pt = PlayerType.PC)
        {
            this.pt = pt;
        }


        /// <summary>Добавляет карту в руку</summary>
        /// <returns>Количество очков после добавления</returns>
        public int AddCard(Cart c)
        {
            CartPool.Add(c);
            this.Score += c.Score;

            //Уведомление об изменении коллекции( для отображения на форме)
            if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            //Если счет стал Больше 21 возможно есть тузы и нужно пересчитать
            if (this.score > 21)
            {
                int tyzQty = 0 ;// Кво тузов в руке игрока
                int newScore = 0;

                foreach (Cart item in CartPool)
                {
                    newScore += item.Score;
                    if (item.cartId == 12) tyzQty++; 
                }

                while (tyzQty > 0 && newScore > 21)
                {
                    newScore -= 10;
                    tyzQty--;
                }

                this.Score = newScore;
            }

            return this.Score;
        }

        /// <summary>Начать новый раунд(сбросить карты и счет)</summary>
        public void Reset()
        {
            CartPool.Clear();
            this.Score = 0;
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


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
