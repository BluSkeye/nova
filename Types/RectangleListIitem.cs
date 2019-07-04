using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi
{
    public class RectangleListItem : IObservableListItem
    {
        private int Ax;
        private int Ay;
        private int Dx;
        private int Dy;

        private int x1;
        private int y1;
        private int x2;
        private int y2;
        private string label;
        private double confidence;

        public void UpdateADCoords()
        {
            //  A---B
            //  |   |
            //  C---D
            int Ax = Math.Min(this.x1, this.x2);
            int Ay = Math.Min(this.y1, this.y2);
            int Dx = Math.Max(this.x1, this.x2);
            int Dy = Math.Max(this.y1, this.y2);

            this.Ax = Ax;
            this.Ay = Ay;
            this.Dx = Dx;
            this.Dy = Dy;
        }

        public double XCoord
        {
            get { return Ax; }
        }

        public double YCoord
        {
            get { return Ay; }
        }

        public int AxCoord
        {
            get { return Ax; }
        }

        public int DxCoord
        {
            get { return Dx; }
        }

        public int AyCoord
        {
            get { return Ay; }
        }
        public int DyCoord
        {
            get { return Dy; }
        }

        public int X1Coord
        {
            set
            {
                x1 = value;
            }
            get { return x1; }
        }

        public int X2Coord
        {
            set
            {
                x2 = value;
            }
            get { return x2; }
        }

        public int Y1Coord
        {
            set
            {
                y1 = value;
            }
            get { return y1; }
        }
        public int Y2Coord
        {
            set
            {
                y2 = value;
            }
            get { return y2; }
        }

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                OnPropertyChanged("Label");
            }
        }

        public double Confidence
        {
            get { return confidence; }
            set
            {
                confidence = value;
                OnPropertyChanged("Confidence");
            }
        }

        public RectangleListItem(int x1, int y1, int x2, int y2, string label, double confidence)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            UpdateADCoords();
            this.label = label;
            this.confidence = confidence;
        }

        public class RectangleListItemComparer : IComparer<RectangleListItem>
        {
            int IComparer<RectangleListItem>.Compare(RectangleListItem a, RectangleListItem b)
            {


                if (a.x1 + a.y1 + a.x2 + a.y2 < b.x1 + b.x2 + b.y1 + b.y2)
                {
                    return -1;
                }
                else if (a.x1 + a.y1 + a.x2 + a.y2 > b.x1 + b.x2 + b.y1 + b.y2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}