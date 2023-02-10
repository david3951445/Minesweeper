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
        /// <summary>
        /// Coords(-1, 0)
        /// </summary>
        public static Coords up = new Coords(-1, 0);
        /// <summary>
        /// Coords(1, 0)
        /// </summary>
        public static Coords down = new Coords(1, 0);
        /// <summary>
        /// Coords(0, -1)
        /// </summary>
        public static Coords left = new Coords(0, -1);
        /// <summary>
        /// Coords(0, 1)
        /// </summary>
        public static Coords right = new Coords(0, 1);

        public Coords(int _row, int _col) {
            row = _row;
            col = _col;
        }

        public override string ToString() => $"({row}, {col})";
        public static Coords operator +(Coords a, Coords b) => new Coords(a.row + b.row, a.col + b.col);
        /// <summary>
        /// Get new coordinates that rotate itself 90 degree
        /// </summary>
        /// <returns></returns>
        public Coords Rotate90() { 
            return new Coords(-col, row);
        }
    }
}
