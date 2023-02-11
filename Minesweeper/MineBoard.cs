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
        public int height { get; private set; } // Number of col of board
        public int width { get; private set; } // Number of row of board
        private int mineCount = 10;
        public GridElement[,] gridElements; // Store the state of elements in grid
        public GridElement.Type[,] answer; // Answer of mine board

        public Action? OnGameOvered;
        public Action? OnGameWinned;
        public event EventHandler<int>? OnFlagCounterChanged;
        public event EventHandler<Coords>? OnGridElementTypeChanged;

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

        public MineBoard(int _width, int _mineCount) {
            width = _width;
            height = _width;
            mineCount = _mineCount;
            isGameOver = false;
            answer = GridElement.Type.Empty.RepeatValue(width, height);
            GenerateAnswer();

            gridElements = new GridElement(GridElement.Type.Cover).RepeatObject(width, height);

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
                        if (isSweepedMinesAround(coord)) {
                            RevealSurrounding(coord);
                        }
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
                        break;

                    default:
                        break;
                }

                OnGridElementTypeChanged?.Invoke(this, coord);
            }
        }

        private bool isSweepedMinesAround(Coords coord) {
            List<Coords> surrounding = GetSurroundingCoords(coord);

            int counter = 0;
            foreach (var c in surrounding) { // Count the number of flag that is flagging the mine
                if (gridElements[c.row, c.col].type == GridElement.Type.Flag && answer[c.row, c.col] == GridElement.Type.Mine) {
                    counter++;
                }
            }

            return counter == (int)answer[coord.row, coord.col]; // Flag all mine arround
        }

        private void RevealSurrounding(Coords coord) {
            List<Coords> surrounding = GetSurroundingCoords(coord);
            surrounding.ForEach(c => ExtendConnectedEmpty(c));
        }

        private void SetGridElementsToAnswer() {
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    gridElements[i, j].type = answer[i, j];
                }
            }
        }

        private void ExtendConnectedEmpty(Coords coord) {
            if (GridElement.IsNumberType(answer[coord.row, coord.col])) {
                gridElements[coord.row, coord.col].type = answer[coord.row, coord.col];
                return;
            }

            if (answer[coord.row, coord.col] is GridElement.Type.Empty) {
                if (gridElements[coord.row, coord.col].type != GridElement.Type.Cover) {
                    return;
                }

                gridElements[coord.row, coord.col].type = answer[coord.row, coord.col];
                RevealSurrounding(coord);
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

        private List<Coords> GetSurroundingCoords(Coords coord) {
            List<Coords> list = new List<Coords>() {
                coord + new Coords(-1, 0),
                coord + new Coords(-1, 1),
                coord + new Coords(0, 1),
                coord + new Coords(1, 1),
                coord + new Coords(1, 0),
                coord + new Coords(1, -1),
                coord + new Coords(0, -1),
                coord + new Coords(-1, -1)
            };
            list.RemoveAll(c => !IsInBound(c));
            return list;
        }
    }
}
