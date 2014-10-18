using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    [Serializable]
    class Game : INotifyPropertyChanged
    {
        #region Статические члены
        
        /// <summary>ССылка на неуправляимую память для хранения игр</summary>
        static System.IO.MemoryStream MS = new System.IO.MemoryStream();

        /// <summary>Чтение из памяти</summary>
        static System.Collections.Hashtable GetSaveTable()
        {
            try
            {
                if (MS.Length == 0) return new System.Collections.Hashtable();

                //Само чтение
                MS.Seek(0, System.IO.SeekOrigin.Begin);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                
                return (System.Collections.Hashtable)bf.Deserialize(MS);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        /// <summary>Сохранение игры</summary>
        public static bool SaveGame(Game game)
        {
            try
            {
                System.Collections.Hashtable ht = GetSaveTable();

                if (ht.ContainsKey(game.GameID)) ht.Remove(game.GameID);

                ht.Add(game.GameID,game);
                
                MS.Seek(0, System.IO.SeekOrigin.Begin);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(MS, ht);

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static Game LoadGame(int gameId)
        {
            try
            {
                System.Collections.Hashtable ht = GetSaveTable();

                if (ht.ContainsKey(gameId)) return (Game)ht[gameId];

                return new Game(gameId);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                throw ex;  
            }
        }
        #endregion
        #region Поля

        public Game(int gameId)
        { this.GameID = gameId; }

        /// <summary>Игрок Человек</summary>
        public Player Human{get; private set;}

        /// <summary>Игрок Компьютер</summary>
        public Player Pc{get; private set;}

        /// <summary>Стока и сменем победителя если пусто - игра еще идет</summary>
        public string GameStatys
        {
            get{return this.gameStatys;}
            private set 
            { 
                this.gameStatys = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("GameStatys"));
            }
        }
        string gameStatys;

        /// <summary>Коллекция карт текущего стола</summary>
        List<Cart> CartPool;

        /// <summary>Номер стола</summary>
        int GameID { get; set; }
        #endregion

        /// <summary>Берет из колоды карту</summary>
        Cart GetNextCart()
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
            this.GameStatys = "";

            //Инициализация колоды
            this.CartPool = new List<Cart>();

            for (int i = 0; i <= 12; i++)
            {
                this.CartPool.Add(new Cart(i, Cart.CartType.Picas));
                this.CartPool.Add(new Cart(i, Cart.CartType.Clovers));
                this.CartPool.Add(new Cart(i, Cart.CartType.Squares));
                this.CartPool.Add(new Cart(i, Cart.CartType.Hearts));
            }

            //Выдадим по 2- стартовые карты всем игрокам
            this.Human = new Player(Player.PlayerType.Human);
            this.Human.AddCard(GetNextCart());
            this.Human.AddCard(GetNextCart());

            this.Pc = new Player();
            this.Pc.AddCard(GetNextCart());
            this.Pc.AddCard(GetNextCart());

        }
    
        /// <summary>Ход казино и Определение победителя</summary>
        public void EndGame()
        {
            while (this.Pc.Score < 17)
            {
                this.Pc.AddCard(GetNextCart());
            }

            if (Human.Score == 21 || Human.Score > Pc.Score || Pc.Score > 21)
                this.GameStatys = "Player WIN";
            
            if  (Human.Score > 21 || Human.Score < Pc.Score)
                this.GameStatys = "Casino WIN";

            if (Human.Score == Pc.Score)
                this.GameStatys = "Draw GAME";
 
           
         }

        /// <summary>Игрок человек получает карту</summary>
        public void HumanAddCart()
        {
            if (Human.Score >= 21) return;

            this.Human.AddCard(GetNextCart());
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
