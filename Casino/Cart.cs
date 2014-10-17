using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        public CroppedBitmap ImgeSourse
        {
            get
            {
                //Достаём картинку из ресурсов и вырезаем из неё нужный фрагмент
                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Card.GetHbitmap()
                                    ,IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                CroppedBitmap cb = new CroppedBitmap(bs, new System.Windows.Int32Rect(5 + 64 * this.cartId, 5 + 85 * (int)this.Ctype, 59, 80));

                return cb;
            }
        }

        /// <summary>порядковый номер карты от 0 до 12(туз)</summary>
        public int cartId{get; private set;}
        /// <summary>Масть</summary>
        CartType Ctype;

        /// <summary>Конструктор</summary>
        public Cart(int id, CartType type)
        {
            this.cartId = id;
            this.Ctype = type;
        }
    }
}
