using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Minesweeper
{
    class GridElement
    {
        
        private Type _type;
        public Type type {
            get => _type;
            set {
                _type = value;
            } 
        }
        private bool isCovered;

        public GridElement(Type _type) {
            type = _type;
        }
        

        public Image GetImage() {
            return UtilsClass.CreateImage(GetImgRelativePath(type), UriKind.Relative);
        }

        static public string GetImgRelativePath(Type type) {
            switch (type) {
                case Type.Empty: return @"img\empty.png";
                case Type.Flag: return @"img\flag.png";
                case Type.Mine: return @"img\mine.png";
                case Type.Cover: return @"img\cover.png";
                case Type.One: return @"img\1.png";
                case Type.Two: return @"img\2.png";
                case Type.Three: return @"img\3.png";
                case Type.Four: return @"img\4.png";
                case Type.Five: return @"img\5.png";
                case Type.Six: return @"img\6.png";
                case Type.Seven: return @"img\7.png";
                case Type.Eight: return @"img\8.png";
                default: throw new Exception("No such grid element type");
            }
        }
        public enum Type
        {
            Empty,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Flag,
            Mine,
            Cover
        }
    }
}
