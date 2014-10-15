using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    class Table
    {
        /// <summary>Коллекция игроков текущего стола</summary>
        public List<Player> PlayerList = new List<Player>();

        /// <summary>Коллекция карт текущего стола</summary>
        List<Cart> CartPool;

        /// <summary>Берет из колоды карту</summary>
        public Cart GetNextCart()
        {
            int index = MyRandom.R.Next(0, this.CartPool.Count);

            try { return this.CartPool[index]; }
            finally 
            { 
                //Удалим карту из пула
                this.CartPool.RemoveAt(index); 
            }
        }

        /// <summary>инициализируеться новая колода и всем игрокам по 2 карты</summary>
        public void StartGame()
        {
            //Инициализация колоды
            this.CartPool = new List<Cart>();

            for (int i = 2; i <= 11; i++)
            {
                this.CartPool.Add(new Cart(i));//4 масти от 2-х до 11
                this.CartPool.Add(new Cart(i));
                this.CartPool.Add(new Cart(i));
                this.CartPool.Add(new Cart(i));
            }

            //Выдадим по 2- стартовые карты всем игрокам
            foreach (Player p in PlayerList)
            {
                p.AddCard(GetNextCart());
                p.AddCard(GetNextCart());
            }


        }

        /// <summary>По очереди ходят все ии и определяеться победитель</summary>
        public void EndGame()
        {
            //По очереди ходят все игроки
            foreach (Player pc in PlayerList)
            {
                /// <summary>Логика хода ИИ</summary>
                if (!pc.isHuman())
                {
                    while (pc.Score < 17)
                    {
                        pc.AddCard(GetNextCart());
                    }
                }
            }
        }
    }
}
