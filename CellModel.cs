using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Marekkia
{
    public class CellModel
    {
        private int _row;
        private int _column;
        private BitmapImage _image;
        //private int _arrow = (int) Arrows.DEFAULT;
        private int _arrowKey;
        private string _arrowName;



        private Dictionary<int, string> arrows = new Dictionary<int, string>()
        {
            {0, "NORTH_ARROW"},
            {1, "SOUTH_ARROW"},
            {2, "EAST_ARROW"},
            {3, "WEST_ARROW"},
            {4, "NORTH_EAST_ARROW"},
            {5, "NORTH_WEST_ARROW"},
            {6, "SOUTH_EAST_ARROW"},
            {7, "SOUTH_WEST_ARROW"},
            {8, "DEFAULT"}
        };

        public CellModel()
        {
            _image = new BitmapImage();
        }


        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int Col
        {
            get { return _column; }
            set { _column = value; }
        }

        public int ArrowKey
        {
            get { return _arrowKey; }
            set
            {
                _arrowKey = value;

                string arrowName = arrows.GetValueOrDefault(value, arrows.ElementAt(8).Value);

                _arrowName = arrowName;

            }
        }

        public string ArrowName
        {
            get { return _arrowName; }
        }

        public Dictionary<int, string> Arrows
        {
            get { return arrows; }
        }


    }
}
