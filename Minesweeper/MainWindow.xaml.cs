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
using System.Windows.Threading;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int gameTimerCounter;
        private DispatcherTimer? gameTimer;

        private MineBoardUI? mineBoardUI;
        private MineBoard? mineBoard;
        public MainWindow() {
            InitializeComponent();

            Restart();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

        }

        private void restartButton_Click(object sender, RoutedEventArgs e) {
            Restart();
        }

        private void Restart() {
            int boardSize = (int)Math.Round(boardSizeSlider.Value);
            int mineCount = (int)Math.Round(mineCountSlider.Value);

            // Set timer
            gameTimerCounter = 0;

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Start();

            // Create mine board
            mineBoard = new MineBoard(boardSize, mineCount);
            mineBoard.OnGameOvered += () => gameMessageTextBox.Text = "Game Over!"; // Disable the board when game over
            mineBoard.OnGameWinned += () => gameMessageTextBox.Text = "Game Win!";
            mineBoard.OnFlagCounterChanged += (object? sender, int num) => flagTextBox.Text = num.ToString();
            mineBoard.flagCounter = mineCount;
            mineBoardUI = new MineBoardUI(mineBoard);

            // Add mine board to root grid
            Grid.SetRow(mineBoardUI.grid, 1);
            rootGrid.Children.Add(mineBoardUI.grid);

            // Game message
            gameMessageTextBox.Text = "Game Start!";
        }

        private void GameTimer_Tick(object? sender, EventArgs e) {
            gameTimerCounter++;
            timeTextBox.Text = gameTimerCounter.ToString() + " s";
        }

    }
}
