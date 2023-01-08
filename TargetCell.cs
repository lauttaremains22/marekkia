using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Marekkia
{
    public class TargetCell : CellModel
    {
        public TargetCell(int row, int col)
        {
            base.Row = row;
            base.Col = col;

            Image.BeginInit();
            Image.CacheOption = BitmapCacheOption.OnLoad;
            Image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "targetcell.png", UriKind.Absolute);
            Image.EndInit();
        }

        public BitmapImage TargetImage
        {
            get { return Image; }
        }
    }
}
