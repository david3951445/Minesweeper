using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Reflection.Metadata.Ecma335;

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

        /// <summary>
        /// Init 2d array with same object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array2D"></param>
        public static void Init<T>(this T[,] array2D) where T : new() {
            for (int i = 0; i < array2D.GetLength(0); i++) {
                for (int j = 0; j < array2D.GetLength(1); j++) {
                    array2D[i, j] = new T();
                }
            }
        }

        /// <summary>
        /// Given row and col, init 2D array with same value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="numRows"></param>
        /// <param name="numCols"></param>
        /// <returns></returns>
        public static T[,] RepeatObject<T>(this T value, int numRows, int numCols) where T : ICloneable {
            T[,] array2D = new T[numRows, numCols];
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numCols; j++) {
                    if (value.Clone() is T t) {
                        array2D[i, j] = t;
                    }
                }
            }
            return array2D;
        }

        /// <summary>
        /// Given row and col, init 2D array with same value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="numRows"></param>
        /// <param name="numCols"></param>
        /// <returns></returns>
        public static T[,] RepeatValue<T>(this T value, int numRows, int numCols) where T : struct {
            T[,] array2D = new T[numRows, numCols];
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numCols; j++) {
                    array2D[i, j] = value;
                }
            }
            return array2D;
        }
    }
}
