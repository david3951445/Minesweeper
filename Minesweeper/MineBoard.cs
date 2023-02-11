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

        private int mineCount = 10;
        private Stack<Coords> minePositions;
        public GridElement[,] gridElements; // Store the state of elements in grid
        public GridElement.Type[,] answer; // Answer of mine board

        public Action? OnGameOvered;
        public Action? OnGameWinned;
        public event EventHandler<int>? OnFlagCounterChanged;
        public event EventHandler? OnGridElementTypeChanged;

        public int height { get; private set; } // Number of col of board
        public int width { get; private set; } // Number of row of board
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
            }
        }

        public MineBoard(int _width, int _mineCount) {
            width = _width;
            height = _width;
            mineCount = _mineCount;
            isGameOver = false;
            minePositions = UtilsClass.PickRandomNumbers2D(new Coords(height, width), mineCount);

            answer = GridElement.Type.Empty.RepeatValue(width, height);
            GenerateAnswer();

            gridElements = new GridElement(GridElement.Type.Cover).RepeatObject(width, height);

        }

        private void GenerateAnswer() {
            // Generate Mines          
            var temp = new Stack<Coords>(new Stack<Coords>(minePositions));
            while (temp.Any()) {
                Coords mineCoord = temp.Pop();
                answer[mineCoord.row, mineCoord.col] = GridElement.Type.Mine;
            }

            // Generate Numbers and Empty
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (answer[i, j] != GridElement.Type.Mine) {
                        List<Coords> surround = GetSurroundingCoords(new Coords(i, j));
                        int amount = surround.Count(c => answer[c.row, c.col] == GridElement.Type.Mine);
                        answer[i, j] = (GridElement.Type)amount;
                    }
                }
            }
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
                        int numFlags = GetSurroundingCoords(coord).Count(c => gridElements[c.row, c.col].type == GridElement.Type.Flag);
                        if (numFlags == (int)gridElements[coord.row, coord.col].type) {
                            RevealSurrounding(coord);
                        }
                        break; 
                        
                    case GridElement.Type.Cover: // Show answer below
                        ExtendConnectedEmpty(coord);
                        break;

                    default:
                        break;
                }

                OnGridElementTypeChanged?.Invoke(this, EventArgs.Empty);
            }

            if (IsWinned()) {
                OnGameWinned?.Invoke();
            }
        }

        bool IsWinned() { // Winning condition
            bool isWinned = true;
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (gridElements[i, j].type == GridElement.Type.Cover && answer[i, j] != GridElement.Type.Mine) {
                        Trace.WriteLine(i + ", " + j);
                        isWinned = false;
                    }
                }
            }
            Trace.WriteLine(isWinned);
            return isWinned;
        }

        private bool IsCoveredMine(Coords c) { // Is this coord is a covered mine
            return gridElements[c.row, c.col].type == GridElement.Type.Cover && answer[c.row, c.col] == GridElement.Type.Mine;
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
            if (IsCoveredMine(coord)) {
                SetGridElementsToAnswer();
                isGameOver = true;
                OnGameOvered?.Invoke();
            }

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
                OnGridElementTypeChanged?.Invoke(this, EventArgs.Empty);
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
