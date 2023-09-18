using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;

namespace Marekkia
{
    /// <summary>
    /// All the main info for the game.
    /// </summary>
    public class MainWindowGame : INotifyPropertyChanged
    {

        private Dictionary<int, BitmapImage> instructions = new Dictionary<int, BitmapImage>();
        //const int SMALL_SIZE = 6, MEDIUM_SIZE = 8, LARGE_SIZE = 10;
        int _rows, _columns;

        //private Object[,] _mainBoard;

        // MAIN BOARD:
        // Both empty cells (cells behind arrow ones) and arrow cells.
        private List<CellModels> _mainBoard;

        // VISIBLE BOARD:
        // Just the board that the user sees.
        private List<CellModel> _visibleBoard;

        // PLAYER:
        // The player entity.
        private Player _player;

        // TARGET CELL:
        // The cell the player should reach to win the game.
        private TargetCell _target;

        private int _playersQty;

        private CellModel _currentCell;
        private BitmapImage _currentCellImage;
        private bool _currentCellChanged = false;

        private bool _validatedStep = true;

        private Stopwatch _timer;

        private int _initTime, _endTime, _landTime, _totalTime;
        private int _accumArrowsQty;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowGame(int rows, int columns)
        {
            _mainBoard = new List<CellModels>(rows * columns);
            _visibleBoard = new List<CellModel>();
            _rows = rows;
            _columns = columns;
            _playersQty = 1;

            LoadImages();
            Init();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Method created just to load the initial data for the game.
        /// </summary>
        private void LoadImages()
        {

            BitmapImage northArrowIMG = new BitmapImage();
            northArrowIMG.BeginInit();
            northArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            northArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            northArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "north arrow.png", UriKind.Absolute);
            Console.WriteLine("north arrow path: " + AppDomain.CurrentDomain.BaseDirectory);
            northArrowIMG.EndInit();

            BitmapImage southArrowIMG = new BitmapImage();
            southArrowIMG.BeginInit();
            southArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            southArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            southArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "south arrow.png", UriKind.Absolute);
            southArrowIMG.EndInit();

            BitmapImage eastArrowIMG = new BitmapImage();
            eastArrowIMG.BeginInit();
            eastArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            eastArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            eastArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "east arrow.png", UriKind.Absolute);
            eastArrowIMG.EndInit();

            BitmapImage westArrowIMG = new BitmapImage();
            westArrowIMG.BeginInit();
            westArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            westArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            westArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "west arrow.png", UriKind.Absolute);
            westArrowIMG.EndInit();

            BitmapImage northEastArrowIMG = new BitmapImage();
            northEastArrowIMG.BeginInit();
            northEastArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            northEastArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            northEastArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "north east arrow button.png", UriKind.Absolute);
            northEastArrowIMG.EndInit();

            BitmapImage northWestArrowIMG = new BitmapImage();
            northWestArrowIMG.BeginInit();
            northWestArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            northWestArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            northWestArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "north west arrow button.png", UriKind.Absolute);
            northWestArrowIMG.EndInit();

            BitmapImage southEastArrowIMG = new BitmapImage();
            southEastArrowIMG.BeginInit();
            southEastArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            southEastArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            southEastArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "south east arrow button.png", UriKind.Absolute);
            southEastArrowIMG.EndInit();

            BitmapImage southWestArrowIMG = new BitmapImage();
            southWestArrowIMG.BeginInit();
            southWestArrowIMG.CacheOption = BitmapCacheOption.OnLoad;
            southWestArrowIMG.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            southWestArrowIMG.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "south west arrow button.png", UriKind.Absolute);
            southWestArrowIMG.EndInit();

            instructions.Add(0, northArrowIMG);
            instructions.Add(1, southArrowIMG);
            instructions.Add(2, eastArrowIMG);
            instructions.Add(3, westArrowIMG);
            instructions.Add(4, northEastArrowIMG);
            instructions.Add(5, northWestArrowIMG);
            instructions.Add(6, southEastArrowIMG);
            instructions.Add(7, southWestArrowIMG);
        }

        /// <summary>
        /// Method created to initialize all the logic for the game.
        /// </summary>
        private void Init()
        {

            Random arrowGenerator = new Random();
            Random playerIndexGenerator = new Random();
            Random targetIndexGenerator = new Random();

            bool isPlayerIndex = false;
            bool playerIndexAssigned = false;

            bool isTargetIndex = false;
            bool targetIndexAssigned = false;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {

                    if (!targetIndexAssigned)
                    {
                        if (r == 0)
                        {
                            int targetIndex = targetIndexGenerator.Next(0, 2);
                            if (targetIndex == 1)
                            {
                                isTargetIndex = true;
                                targetIndexAssigned = true;
                                _target = new TargetCell(r, c);
                                PutSingleModelIntoARow(_target);
                            }

                        }
                    }

                    if (!playerIndexAssigned)
                    {

                        if (r == Rows - 1)
                        {
                            int playerIndex = playerIndexGenerator.Next(0, 2);
                            if (playerIndex == 1)
                            {
                                isPlayerIndex = true;
                                playerIndexAssigned = true;

                                _player = new Player(r, c);
                                this.CurrentCell = _player;

                                // set default arrow key for no-arrowed cell (8).
                                this.CurrentCell.ArrowKey = 8;

                                PutSingleModelIntoARow(_player);
                            }

                            _playersQty++;

                        }

                    }

                    if (!isPlayerIndex && !isTargetIndex)
                    {
                        int arrowKey = arrowGenerator.Next(0, 8);
                        bool tryArrow = instructions.TryGetValue(arrowKey, out BitmapImage arrow);
                        if (tryArrow)
                        {
                            var cell = new CellModel
                            {
                                Row = r,
                                Col = c,
                                Image = arrow,
                                ArrowKey = arrowKey
                            };

                            CellModels cellModels = new CellModels();

                            cellModels.Add(new EmptyCell(r, c));
                            cellModels.Add(cell);

                            _mainBoard.Add(cellModels);
                        }

                    }

                    isPlayerIndex = false;
                    isTargetIndex = false;

                }

            }

            TakeVisibleMainboardValues();

        }

        private void PutSingleModelIntoARow(CellModel cell)
        {

            CellModels cellModels = new CellModels();

            cellModels.Add(new EmptyCell(cell.Row, cell.Col));
            cellModels.Add(cell);

            _mainBoard.Add(cellModels);
        }

        private void TakeVisibleMainboardValues()
        {
            try
            {
                List<CellModel> localVisibleBoard = new List<CellModel>();

                foreach (CellModels cells in MainBoard)
                {
                    cells.Row = cells[cells.Count - 1].Row;
                    cells.Col = cells[cells.Count - 1].Col;
                    localVisibleBoard.Add(cells[cells.Count - 1]);
                    Console.WriteLine("ppapapap");
                }

                VisibleBoard = localVisibleBoard;

            }
            catch { }

        }

        public bool ChangePlayerCell(int row, int col)
        {
            try
            {
                Player plycell = (Player)VisibleBoard.Find(p => p is Player);

                if (_validatedStep = ValidateStep(plycell, row, col))
                {

                    int direction = getPlayerDirection(plycell.Row, plycell.Col, row, col);

                    // Accumulate the instruction if player's direction doesn't match with
                    // the cell arrow. Also ignore not-arrowed cells.

                    if (direction != CurrentCell.ArrowKey && CurrentCell.ArrowKey != 8)
                    {
                        plycell.PlayerAccum.SetInstruction(CurrentCell.ArrowKey);
                        AccumArrowsQty = plycell.PlayerAccum.Instructions.Count;
                    }

                    RemoveOldPlayerCell(plycell, MainBoard);
                    plycell.Row = row;
                    plycell.Col = col;
                    MainBoard.Find(c => c.Row == row && c.Col == col).Add(plycell);
                    TakeVisibleMainboardValues();

                    return true;

                }

            }
            catch { }

            return false;

        }

        private void RemoveOldPlayerCell(Player targetPly, List<CellModels> mainboard)
        {
            mainboard.Find(c => c.Row == targetPly.Row && c.Col == targetPly.Col).RemoveAll(p => p is Player);
        }

        private bool ValidateStep(Player p, int newRow, int newCol)
        {
            if (p.Row > newRow + 1 ||
                p.Row < newRow - 1 ||
                p.Col > newCol + 1 ||
                p.Col < newCol - 1 ||
                (p.Col == newCol && p.Row == newRow))
                return false;
            else
                return true;
        }

        public void StartTime()
        {
            _timer = new Stopwatch();
            _timer.Start();
            _initTime = (int)_timer.ElapsedMilliseconds / 1000;
        }

        // Directions


        /// <summary>
        /// Get the direction the player moved to with a numeric index.
        /// </summary>
        /// <param name="oldRow"></param>
        /// <param name="oldCol"></param>
        /// <param name="newRow"></param>
        /// <param name="newCol"></param>
        /// <returns></returns>
        private int getPlayerDirection(int oldRow, int oldCol, int newRow, int newCol)
        {

            if (oldRow == newRow + 1 && oldCol == newCol)
                return 0;
            else if (oldRow == newRow - 1 && oldCol == newCol)
                return 1;
            else if (oldRow == newRow && oldCol == newCol - 1)
                return 2;
            else if (oldRow == newRow && oldCol == newCol + 1)
                return 3;
            else if (oldRow == newRow + 1 && oldCol == newCol - 1)
                return 4;
            else if (oldRow == newRow + 1 && oldCol == newCol + 1)
                return 5;
            else if (oldRow == newRow - 1 && oldCol == newCol - 1)
                return 6;
            else if (oldRow == newRow - 1 && oldCol == newCol + 1)
                return 7;
            else
                return 8;

        }

        // Properties

        public Player SpawnedPlayer
        {
            get { return _player; }
            set { _player = value; }
        }

        public List<CellModels> MainBoard
        {
            get
            {
                return _mainBoard;
            }
            set
            {
                _mainBoard = value;
                OnPropertyChanged("MainBoard");
            }
        }

        public List<CellModel> VisibleBoard
        {
            get
            {
                return _visibleBoard;
            }
            set
            {
                _visibleBoard = value;
                OnPropertyChanged("VisibleBoard");
            }
        }

        public int Rows
        {
            get
            {
                return _rows;
            }
        }

        public int Columns
        {
            get
            {
                return _columns;
            }
        }

        public int PlayersQty
        {
            get
            {
                return _playersQty;
            }
            set
            {
                _playersQty = value;
            }
        }

        public Stopwatch Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public int InitTime
        {
            get { return _initTime; }
            set { _initTime = value; }
        }

        public int EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public int LandTime
        {
            get { return _landTime; }
            set { _landTime = value; OnPropertyChanged("LandTime"); }
        }

        public int AccumArrowsQty
        {
            get { return _accumArrowsQty; }
            set { _accumArrowsQty = value; OnPropertyChanged("AccumArrowsQty"); }
        }

        public BitmapImage CurrentCellImage
        {
            get { return _currentCellImage; }
            set
            {

                if (_validatedStep)
                    _currentCellImage = value;

                OnPropertyChanged("CurrentCellImage");

            }
        }

        public CellModel CurrentCell
        {
            get { return _currentCell; }
            set { _currentCell = value; }
        }

        public bool CurrentCellChanged
        {
            get { return _currentCellChanged; }
            set { _currentCellChanged = value; }
        }

    }


}


