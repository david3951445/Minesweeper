using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Minesweeper
{
    class MineBoardUI
    {
        private MineBoard _mineBoard;
        private MineBoard mineBoard {
            get => _mineBoard;
            set {
                _mineBoard = value;
                _mineBoard.OnGridElementTypeChanged += _mineBoard_onGridElementTypeChanged;
            }
        }

        bool isMouseLeftButtonDown = false; // Need to change to "bool[,] isMouseLeftButtonDowns" so that each image has its own checking
        public Grid grid; // For storing GridElementUI
        private Image? clickedImage;
        private double gridSize = 40f;

        public MineBoardUI(MineBoard mineBoard) {
            this.mineBoard = mineBoard;

            // Init grid
            int boardSize = mineBoard.width;
            grid = new Grid();
            for (int i = 0; i < boardSize; i++) {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }         
            grid.Width = gridSize * boardSize;
            grid.Height = gridSize * boardSize;
            grid.ShowGridLines = true;

            // Add image to grid
            for (int i = 0; i < boardSize; i++) {
                for (int j = 0; j < boardSize; j++) {
                    string path = GridElement.GetImgRelativePath(GridElement.Type.Cover);

                    // Use image as UI of GridElement and handle the mouse event
                    Image image = UtilsClass.CreateImage(path, UriKind.Relative);
                    image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                    image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
                    image.MouseRightButtonDown += Image_MouseRightButtonDown;

                    grid.SetChild(image, i, j);              
                }
            }
        }


        private void _mineBoard_onGridElementTypeChanged(object? sender, EventArgs e) {
            // Refresh
            for (int i = 0; i < mineBoard.width; i++) {
                for (int j = 0; j < mineBoard.width; j++) {
                    if (grid.GetChild(i, j) is Image image) {
                        string path = GridElement.GetImgRelativePath(mineBoard.gridElements[i, j].type);
                        //Trace.WriteLine(mineBoard.gridElements[i, j].type);
                        image.Source = UtilsClass.GetBitmapImage(path, UriKind.Relative);
                    }
                }
            }

            //if (grid.GetChild(e.coord.row, e.coord.col) is Image image) {
            //    string path = GridElement.GetImgRelativePath(mineBoard.answer[e.coord.row, e.coord.col]);
            //    image.Source = UtilsClass.GetBitmapImage(path, UriKind.Relative);
            //}
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (sender is Image image) {
                if (image == clickedImage) {
                    Coords elementCoord = new Coords(Grid.GetRow(image), Grid.GetColumn(image));
                    if (isMouseLeftButtonDown) { // Reveal
                        mineBoard.Reveal(elementCoord);
                    }
                }
            }
            isMouseLeftButtonDown = false;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            isMouseLeftButtonDown = true;
            if (sender is Image image) {
                clickedImage = image;
            }
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            if (sender is Image image) {
                Coords elementCoord = new Coords(Grid.GetRow(image), Grid.GetColumn(image));
                mineBoard.Flag(elementCoord);
            }
            
        }
    }
}
