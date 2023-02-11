// If the UI of GridElement need to extend, then this class is needed

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media.Imaging;

//namespace Minesweeper
//{
//    class GridElementUI
//    {
//        /// <summary>
//        /// Correspounding controller class
//        /// </summary>
//        private GridElement _gridElement;
//        public GridElement gridElement {
//            get => _gridElement;
//            set {
//                _gridElement = value;
//            }
//        }
//        public Image image;
//        bool isMouseLeftButtonDown = false;

//        public GridElementUI(GridElement _gridElement) {
//            gridElement = _gridElement;
//            string path = GridElement.GetImgRelativePath(GridElement.Type.Cover);
//            image = UtilsClass.CreateImage(path, UriKind.Relative);
//            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
//            image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
//        }

//        private void Image_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
//            if (isMouseLeftButtonDown) { // Reveal

//            }
//            else {
//                isMouseLeftButtonDown = false;
//            }
//        }

//        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
//            isMouseLeftButtonDown = true;
//        }


//    }
//}
