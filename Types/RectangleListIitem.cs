using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssi
{
    public class RectangleListItem : IObservableListItem
    {
        private double x1;
        private double y1;

        private double x2;
        private double y2;
        private string label;
        private double confidence;

        public double X1Coord
        {
            get { return x1; }
            set
            {
                x1 = value;
                OnPropertyChanged("X1");
            }
        }

        public double X2Coord
        {
            get { return x2; }
            set
            {
                x2 = value;
                OnPropertyChanged("X2");
            }
        }

        public double Y1Coord
        {
            get { return y1; }
            set
            {
                y1 = value;
                OnPropertyChanged("Y1");
            }
        }
        public double Y2Coord
        {
            get { return y2; }
            set
            {
                y2 = value;
                OnPropertyChanged("Y2");
            }
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

        public RectangleListItem(double x1, double y1, double x2, double y2, string label, double confidence)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
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