using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Minesweeper
{
    class MineBoard 
    {
        public int width { get; private set; }
        public int height { get; private set; }
        private int mineCount = 10;
        public GridElement[,] gridElements;
        public GridElement.Type[,] answer;
        public Action OnGameOvered;
        private bool _isGameOver;
        public bool isGameOver {
            get => _isGameOver;
            set {
                _isGameOver = value;
            }
        }

        public event EventHandler<MyEventArgs> onGridElementTypeChanged;
        public class MyEventArgs : EventArgs
        {   
            public Coords coord;
        }

        public MineBoard(int _width, int _mineCount) {          
            width = _width;
            height = _width;
            mineCount = _mineCount;
            GenerateAnswer();
            gridElements = Create2DArrayWithInitialValue(width, height, new GridElement(GridElement.Type.Cover));
            
        }

        private GridElement[,] Create2DArrayWithInitialValue(int numRows, int numCols, GridElement value) {
            // method 1
            GridElement[,] elements = new GridElement[numRows, numCols];
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numCols; j++) {
                    elements[i, j] = new GridElement(GridElement.Type.Cover);
                }
            }

            // method 2 (This will fuck up, Although it is same process with method 1)
            //elements = UtilsClass.Create2DArrayWithInitialValue(width, height, new GridElement(GridElement.Type.Cover));

            return elements;
        }

        private void GenerateAnswer() {
            answer = UtilsClass.Create2DArrayWithInitialValue(width, height, GridElement.Type.Empty);
  
            // Generate Mines
            Stack<Coords> mines = UtilsClass.PickRandomNumbers2D(new Coords(height, width), mineCount);
            while (mines.Any()) {
                Coords mineCoord = mines.Pop();
                //gridElements[mineCoord.row, mineCoord.col] = new GridElement(GridElement.Type.Mine);
                answer[mineCoord.row, mineCoord.col] = GridElement.Type.Mine;
            }

            // Generate Numbers and Empty
            for (int row = 0; row < height; row++) {
                for (int col = 0; col < width; col++) {
                    if (answer[row, col] != GridElement.Type.Mine) {
                        int amount = FindNumberOfMineAround(new Coords(row, col));
                        //gridElements[row, col] = new GridElement((GridElement.Type)amount);
                        answer[row, col] = (GridElement.Type)amount;
                    }
                }
            }
        }

        private int FindNumberOfMineAround(Coords origin) {
            Coords[] offsets = new Coords[] {
                new Coords(-1, -1),
                new Coords(0, -1),
                new Coords(1, -1),
                new Coords(1, 0),
                new Coords(1, 1),
                new Coords(0, 1),
                new Coords(-1, 1),
                new Coords(-1, 0)
            };

            int count = 0;
            foreach (var offset in offsets) {
                Coords coord = new Coords(origin.row + offset.row, origin.col + offset.col);
                if (IsInBound(coord)) {
                    if (answer[coord.row, coord.col] == GridElement.Type.Mine) {
                        count++;
                    }
                }
            }
            
            return count;
        }

        private bool IsInBound(Coords coord) {
            return coord.row >= 0 && coord.row < height && coord.col >= 0 && coord.col < width;
        }

        public void Reveal(Coords coord) {
            if (!isGameOver) {
                switch (gridElements[coord.row, coord.col].type) {
                    case GridElement.Type.Empty:
                        // ExtendConnectedEmpty();
                        break;
                    case GridElement.Type.One:
                    case GridElement.Type.Two:
                    case GridElement.Type.Three:
                    case GridElement.Type.Four:
                    case GridElement.Type.Five:
                    case GridElement.Type.Six:
                    case GridElement.Type.Seven:
                    case GridElement.Type.Eight:
                        // RevealSurrounding();
                        break;                     
                    case GridElement.Type.Cover: // Show answer below
                        gridElements[coord.row, coord.col].type = answer[coord.row, coord.col];
                        onGridElementTypeChanged.Invoke(this, new MyEventArgs() { coord = coord });

                        if (gridElements[coord.row, coord.col].type == GridElement.Type.Mine) {
                            isGameOver = true;
                            OnGameOvered.Invoke();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void Flag(Coords coord) {
            if (!isGameOver) {
                switch (gridElements[coord.row, coord.col].type) {
                    case GridElement.Type.Flag:
                        gridElements[coord.row, coord.col].type = GridElement.Type.Cover;
                        break;
                    case GridElement.Type.Cover:
                        gridElements[coord.row, coord.col].type = GridElement.Type.Flag;
                        // flagCounter--;
                        break;
                    default:
                        break;
                }
                onGridElementTypeChanged.Invoke(this, new MyEventArgs() { coord = coord });
            }
        }
    }
}
