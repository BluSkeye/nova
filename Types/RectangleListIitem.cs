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

        private int clothing_state = 0; // 0 fully clothed, 1 partially clothed, 2 naked
        private int body_type = 0; // 0 full body, 1 upper body, 2 lower body, 3 partial body
        private int gender = 0; // 0 male, 1 female

        private int x1;
        private int y1;
        private int x2;
        private int y2;
        private string label;
        private double confidence;

        private bool seletcted;
        private int area;


        private void UpdateArea()
        {
            int width = Dx - Ax;
            int height = Dy - Ay;
            area = width * height;
        }
        public int Area
        {
            get{ return area; }
        }

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
            UpdateArea();
            OnPropertyChanged("Ax Ay Dx Dy");
        }

        public bool Selected
        {
            set
            {
                seletcted = value;
                OnPropertyChanged("selected");
            }
            get { return seletcted; }
        }

        public int Gender
        {
            set
            {
                gender = value;
                OnPropertyChanged("gender");
            }
            get { return gender; }
        }

        public int ClothingState
        {
            set
            {
                clothing_state = value;
                OnPropertyChanged("clothing state");
            }
            get { return clothing_state; }
        }

        public int BodyType
        {
            set
            {
                body_type = value;
                OnPropertyChanged("body type");
            }
            get { return body_type; }
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

        public double XCoord
        {
            get { return Ax; }
        }

        public double YCoord
        {
            get { return Ay; }
        }

        public RectangleListItem(int x1, int y1, int x2, int y2, int body_type, int clothing_state, int gender, string label, double confidence)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            UpdateADCoords();
            this.label = label;
            this.confidence = confidence;
            this.body_type = body_type;
            this.clothing_state = clothing_state;
            this.gender = gender;
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