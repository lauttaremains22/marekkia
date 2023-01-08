using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Marekkia
{
    class EmptyCell : CellModel
    {

        public EmptyCell(int row, int col)
        {
            base.Row = row;
            base.Col = col;

            Image.BeginInit();
            Image.CacheOption = BitmapCacheOption.OnLoad;
            Image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "emptyspace.png", UriKind.Absolute);
            Image.EndInit();

        }

        public BitmapImage EmptyImage
        {
            get { return Image; }
        }
    }
}
