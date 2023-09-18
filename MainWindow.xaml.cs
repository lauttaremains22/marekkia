using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
using Color = System.Drawing.Color;
using System.IO;

namespace Marekkia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        int clicks = 0;

        public MainWindow()
        {
            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.CacheOption = BitmapCacheOption.OnLoad;
            logo.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            logo.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "logo.png", UriKind.Absolute);
            logo.EndInit();

            this.Icon = logo;

            MainWindowGame windowGame = new MainWindowGame(6, 6);
            this.DataContext = windowGame;
        }

        private void Init()
        {
           
        }

        private void ButtonArrow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (clicks == 0)
            {
                ((MainWindowGame)this.DataContext).StartTime();
                clicks++;
            } 

            if (((Button)sender).DataContext is TargetCell)
            {
                ((MainWindowGame)this.DataContext).EndTime = (int) ((MainWindowGame)this.DataContext).Timer.ElapsedMilliseconds / 1000;
                ((MainWindowGame)this.DataContext).LandTime = ((MainWindowGame)this.DataContext).EndTime - ((MainWindowGame)this.DataContext).InitTime;
            }

            var cellModel = ((Button)sender).DataContext as CellModel;

            bool changed = ((MainWindowGame)this.DataContext).ChangePlayerCell(cellModel.Row, cellModel.Col);

            if (changed)
            {
                ((MainWindowGame)this.DataContext).CurrentCell = cellModel;
                ((MainWindowGame)this.DataContext).CurrentCellImage = cellModel.Image;
                ((MainWindowGame)this.DataContext).CurrentCellChanged = true;
            }


        }

        private void InstructionButton_Click(object sender, MouseButtonEventArgs e)
        {
            string instructionInput = InstructionInput.Text;
            Console.WriteLine(instructionInput);
        }


    }
}
