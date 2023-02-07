using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct Coords
    {
        public int row;
        public int col;

        public Coords(int _row, int _col) {
            row = _row;
            col = _col;
        }

        public override string ToString() => $"({row}, {col})";
        public static Coords operator +(Coords a, Coords b) => new Coords(a.row + b.row, a.col + b.col);
    }
}
