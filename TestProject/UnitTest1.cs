using Minesweeper;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Media3D;
using Xunit.Abstractions;

namespace TestProject
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public void Test1() {
            int numRows = 2; int numCols = 2;
            GridElement[,] elements1 = new GridElement[numRows, numCols];
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numCols; j++) {
                    elements1[i, j] = new GridElement(GridElement.Type.Cover);
                }
            }

            // method 2 (This will fuck up, Although it is same process with method 1)
            GridElement[,] elements2;
            elements2 = UtilsClass.Create2DArrayWithInitialValue(numRows, numCols, new GridElement(GridElement.Type.Cover));

            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numCols; j++) {
                    output.WriteLine(elements1[i, j].type.ToString());
                    output.WriteLine(elements2[i, j].type.ToString());
                }
            }

        }
    }
}