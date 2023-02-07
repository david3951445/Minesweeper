using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Minesweeper
{
    public static class UtilsClass
    {
        static public Image CreateImage(string path, UriKind uriKind) {
            return new Image() {
                Source = GetBitmapImage(path, uriKind)
            };       
        }
        static public BitmapImage GetBitmapImage(string path, UriKind uriKind) {
            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path, uriKind);
            bitmapImage.EndInit();

            return bitmapImage;
        }

        static public Stack<Coords> PickRandomNumbers2D(Coords dimensionSize, int amount) {
            // Draw card algorithm
            int num = dimensionSize.row * dimensionSize.col;
            List<int> list = Enumerable.Range(1, num).ToList();
            Stack<int> stack = new Stack<int>();
            Random random = new Random();
            for (int i = 0; i < amount; i++) {
                int r = random.Next(list.Count); // draw a card
                stack.Push(list[r]); // put the card into a stack
                list.RemoveAt(r); // remove the card
            }


            // Convert to 2-tuple
            Stack<Coords> numbers = new Stack<Coords>();
            while (stack.Any()) {
                int num1 = stack.Pop();
                numbers.Push(new Coords((num1 - 1) / dimensionSize.col, num1 % dimensionSize.col));
            }

            return numbers;
        }

        /// <summary>
        /// Get row and column of Children in Grid
        /// </summary>
        /// <returns></returns>
        static public Coords GetChildCoordInGrid(UIElement element) {
            return new Coords(Grid.GetRow(element), Grid.GetColumn(element));
        }      

        static public T[,] Create2DArrayWithInitialValue<T>(int numRows, int numCols, T value) {
            T[,] array2D = new T[numRows, numCols];
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numCols; j++) {
                    array2D[i, j] = value;
                }
            }
            return array2D;
        }

        static public void Fill2DArray<T>(T[,] array2D, T value) {
            int numRows = array2D.GetLength(0);
            int numCols = array2D.GetLength(1);
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numCols; j++) {
                    array2D[i, j] = value;
                }
            }
        }
    }
}
