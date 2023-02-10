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

        public Action? OnGameOvered;
        public Action? OnGameWinned;
        public event EventHandler<int>? OnFlagCounterChanged;

        private bool _isGameOver;
        private bool isGameOver {
            get => _isGameOver;
            set {
                _isGameOver = value;
            }
        }
        private int _flagCounter;
        public int flagCounter {
            get => _flagCounter;
            set {
                _flagCounter = value;
                OnFlagCounterChanged?.Invoke(this, _flagCounter);
                if (_flagCounter == 0) {
                    OnGameWinned?.Invoke();
                }
            }
        }

        public event EventHandler<Coords>? OnGridElementTypeChanged;

        public MineBoard(int _width, int _mineCount) {
            width = _width;
            height = _width;
            mineCount = _mineCount;
            isGameOver = false;

            answer = UtilsClass.Create2DArrayWithInitialValue(width, height, GridElement.Type.Empty);
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
                        if (answer[coord.row, coord.col] == GridElement.Type.Mine) {
                            SetGridElementsToAnswer();
                            isGameOver = true;
                            OnGameOvered?.Invoke();
                        }
                        else {
                            ExtendConnectedEmpty(coord);
                        }

                        OnGridElementTypeChanged?.Invoke(this, coord);
                    
                        break;

                    default:
                        break;
                }
            }
        }

        private void SetGridElementsToAnswer() {
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    gridElements[i, j].type = answer[i, j];
                }
            }
        }

        private void ExtendConnectedEmpty(Coords coord) {
            if (!IsInBound(coord)) {
                return;
            }

            if (GridElement.IsNumberType(answer[coord.row, coord.col])) {
                gridElements[coord.row, coord.col].type = answer[coord.row, coord.col];
                return;
            }

            if (answer[coord.row, coord.col] == GridElement.Type.Empty) {
                if (gridElements[coord.row, coord.col].type != GridElement.Type.Cover) {
                    return;
                }

                gridElements[coord.row, coord.col].type = answer[coord.row, coord.col];
                ExtendConnectedEmpty(coord + Coords.up);
                ExtendConnectedEmpty(coord + new Coords(-1, 1));
                ExtendConnectedEmpty(coord + Coords.right);
                ExtendConnectedEmpty(coord + new Coords(1, 1));
                ExtendConnectedEmpty(coord + Coords.down);
                ExtendConnectedEmpty(coord + new Coords(1, -1));
                ExtendConnectedEmpty(coord + Coords.left);
                ExtendConnectedEmpty(coord + new Coords(-1, -1));
            }
            return;
        }

        public void Flag(Coords coord) {
            if (!isGameOver) {
                switch (gridElements[coord.row, coord.col].type) {
                    case GridElement.Type.Flag:
                        gridElements[coord.row, coord.col].type = GridElement.Type.Cover;
                        flagCounter++;
                        break;
                    case GridElement.Type.Cover:
                        gridElements[coord.row, coord.col].type = GridElement.Type.Flag;
                        flagCounter--;
                        break;
                    default:
                        break;
                }
                OnGridElementTypeChanged?.Invoke(this, coord);
            }
        }
    }
}
