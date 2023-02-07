using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
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

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();

            Restart();
        }

        private MineBoardUI mineBoardUI;
        private MineBoard mineBoard;

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

        }

        private void restartButton_Click(object sender, RoutedEventArgs e) {
            Restart();
        }
        private void Restart() {
            int boardSize = (int)boardSizeSlider.Value;
            int mineCount = (int)mineCountSlider.Value;

            // Create mine board
            mineBoard = new MineBoard(boardSize, mineCount);
            mineBoard.OnGameOvered += () => gameMessageTextBox.Visibility = Visibility.Visible;
            mineBoard.isGameOver = false;
            mineBoardUI = new MineBoardUI(mineBoard);

            // Add mine board to root grid
            Grid.SetRow(mineBoardUI.grid, 1);
            rootGrid.Children.Add(mineBoardUI.grid);

            // 
            gameMessageTextBox.Visibility = Visibility.Hidden;
        }
    }
}
