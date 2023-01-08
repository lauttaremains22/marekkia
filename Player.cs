using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Marekkia
{
    public class Player : CellModel
    {
        // CREATE PLAYER IMAGE BUTTON (BITMAP) AND ALL ATTRIBUTES LIKE MarekkiaStorage, Accumulator, Stats, etc..

        MarekkiaStorage _playerStorage;
        Accumulator _playerAccum;


        public Player(int row, int col)
        {
            base.Row = row;
            base.Col = col;

            _playerStorage = new MarekkiaStorage();
            _playerAccum = new Accumulator();

            Image.BeginInit();
            Image.CacheOption = BitmapCacheOption.OnLoad;
            Image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "PlayerImage.png", UriKind.Absolute);
            Image.EndInit();

        }

        public BitmapImage PlayerImage
        {
            get { return Image; }
        }

    }
}
