using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Minesweeper
{
    public static class MyExtension
    {
        /// <summary>
        /// Get child in Grid given row and col
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static UIElement GetChild(this Grid grid, int row, int col) {
            foreach (var child in grid.Children.Cast<UIElement>()) {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == col) {
                    return child;
                }
            }
            return default;
        }

        /// <summary>
        /// Set child on specific row and column of Grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="element"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        static public void SetChild(this Grid grid, UIElement element, int row, int col) {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
            grid.Children.Add(element);
        }
    }
}
