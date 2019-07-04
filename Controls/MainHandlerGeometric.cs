using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ssi
{
    public partial class MainHandler
    {
        public Color GetColor(int index)
        {
            //int h = 100;
            //Console.WriteLine(HSVToRGB(h, 100, 100).ToString());
            int max_colours = 20;
            int h = ((360 / max_colours) * index) % 360;
            return HSVToRGB(h, 100, 100);

        }

        private Color HSVToRGB(double h, double s, double v)
        {
            if (s == 0)
            {
                byte c = (byte)(v * 255.0);
                return Color.FromRgb(c, c, c);
            }
            //If Luminance is smaller then 0.5(50 %) then temporary_1 = Luminance x(1.0 + Saturation)
            //If Luminance is equal or larger then 0.5(50 %) then temporary_1 = Luminance + Saturation – Luminance x Saturation
            double t1;
            if (v < 0.5)
            {
                t1 = v * (1 + s);
            }
            else
            {
                t1 = v + s - v * s;
            }
            //temporary_2 = 2 x Luminance – temporary _1
            double t2 = 2 * h - t1;
            //The next step is to convert the 360 degrees in a circle to 1 by dividing the angle by 360.
            // Hue = 193 / 360 = 0.536
            double hue = h / 360;
            //temporary_R = Hue + 0.333 = 0.536 + 0.333 = 0.869
            //temporary_G = Hue = 0.536
            //temporary_B = Hue – 0.333 = 0.536 – 0.333 = 0.203
            double tr = hue + 0.333;
            tr = tr > 1 ? tr - 1 : tr;
            double tg = hue;
            double tb = hue - 0.333;
            tb = tb < 0 ? tb + 1 : tb;

            //test 1 – If 6 x temporary_R is smaller then 1, Red = temporary_2 + (temporary_1 – temporary_2) x 6 x temporary_R
            //In the case the first test is larger then 1 check the following

            ///test 2 – If 2 x temporary_R is smaller then 1, Red = temporary_1
            //In the case the second test also is larger then 1 do the following

            //test 3 – If 3 x temporary_R is smaller then 2, Red = temporary_2 + (temporary_1 – temporary_2) x(0.666 – temporary_R) x 6
            //In the case the third test also is larger then 2 you do the following
            
            
            //Red
            double red;
            if (6 * tr < 1)
            {
                red = t2 + (t1 - t2) * 9 * tr;
            }
            else if (2 * tr < 1)
            {
                red = t1;
            }
            else if (3 * tr < 2)
            {
                red = t2 + (t1 - t2) * (0.666 - tr) * 6;
            }
            else
            {
                red = t2;
            }

            //Green
            double green;
            if (6 * tg < 1)
            {
                green = t2 + (t1 - t2) * 9 * tg;
            }
            else if (2 * tg < 1)
            {
                green = t1;
            }
            else if (3 * tg < 2)
            {
                green = t2 + (t1 - t2) * (0.666 - tg) * 6;
            }
            else
            {
                green = t2;
            }

            //Blue
            double blue;
            if (6 * tb < 1)
            {
                blue = t2 + (t1 - t2) * 9 * tb;
            }
            else if (2 * tb < 1)
            {
                blue = t1;
            }
            else if (3 * tb < 2)
            {
                blue = t2 + (t1 - t2) * (0.666 - tb) * 6;
            }
            else
            {
                blue = t2;
            }

            return Color.FromRgb((byte)red, (byte)green, (byte)blue);

        }

        private void setPointList(PointList pl)
        {
            control.geometricListControl.geometricDataGrid.ItemsSource = pl;
        }

        private void setRectangleList(RectangleList rl)
        {
            control.geometricListControl.geometricDataGrid.ItemsSource = rl;
        }

        private void geometricTableUpdate()
        {
            control.geometricListControl.geometricDataGrid.Items.Refresh();
        }

        private void geometricSelectItem(AnnoListItem item, int pos)
        {
            if (item.Points != null && item.Points.Count > 0)
            {
                setPointList(item.Points);              
                geometricOverlayUpdate(item, AnnoScheme.TYPE.POINT, pos);
            }
            else if (item.Rectangles != null && item.Rectangles.Count > 0)
            {
                setRectangleList(item.Rectangles);
                geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
            }
        }


        public void geometricOverlayUpdate(AnnoListItem item, AnnoScheme.TYPE type, int pos = -1)
        {
            WriteableBitmap overlay = null;

            IMedia video = mediaList.GetFirstVideo();

            if (video != null)
            {
                overlay = video.GetOverlay();
            }
            else
            { 
                return;
            }


            try
            {
                overlay.Lock();
                overlay.Clear();
                if (item == null)
                {
                    return;
                }

                switch (type)
                {
                    case AnnoScheme.TYPE.POINT:
                        foreach (PointListItem p in item.Points)
                        {
                            if (p.XCoord != -1 && p.YCoord != -1)
                            {
                                Color color = item.Color;
                                //color.A = 128;
                                overlay.FillEllipseCentered((int)p.XCoord, (int)p.YCoord, 1, 1, color);
                            }
                        }
                        break;
                    case AnnoScheme.TYPE.RECTANGLE:
                        int count = 0;
                        foreach (RectangleListItem r in item.Rectangles)
                        {
                            count++;
                            if (r.X1Coord != -1 && r.Y1Coord != -1 && r.X2Coord != -1 && r.Y2Coord != -1)
                            {
                                //Color color = item.Color;
                                Color color = GetColor((count*2) + 1);
                                color.A /= 100;
                                color.A *= 80;
                                Console.WriteLine(r.AxCoord.ToString() + " " + r.AyCoord.ToString() + " " +
                                                  r.DxCoord.ToString() + " " + r.DyCoord.ToString());
                                overlay.FillRectangle(r.AxCoord, r.AyCoord, r.DxCoord, r.DyCoord, color);                               
                            }
                        }
                        break;
                    case AnnoScheme.TYPE.POLYGON:
                        break;
                    case AnnoScheme.TYPE.GRAPH:
                        break;
                    case AnnoScheme.TYPE.SEGMENTATION:
                        break;
                }
            }
            finally
            {
                overlay.Unlock();
            }

            
        }

        

        private static bool rightHeld;
        private static bool leftHeld;
        private static bool RightHeld
        {
            get { return rightHeld; }
            set
            {
                rightHeld = value;
                if (!rightHeld)
                {
                    RightHeldPos = new double[2] { 0, 0 };
                }
            }
        }

        private static bool LeftHeld
        {
            get { return leftHeld; }
            set
            {
                leftHeld = value;
                if (!leftHeld)
                {
                    LeftHeldPos = new double[2] { 0, 0 };
                }
            }
        }

        private static double[] rightHeldPos;
        private static double[] RightHeldPos
        {
            get
            {
                if (rightHeldPos == null)
                {
                    rightHeldPos = new double[2] { 0, 0 };
                }
                return rightHeldPos;
            }
            set
            {
                if (value.Length == 2)
                {
                    rightHeldPos = value;
                }
            }
        }

        private static double[] leftHeldPos;
        private static double[] LeftHeldPos
        {
            get
            {
                if (leftHeldPos == null)
                {
                    leftHeldPos = new double[2] { 0, 0 };
                }
                return leftHeldPos;
            }
            set
            {
                if (value.Length == 2)
                {
                    leftHeldPos = value;
                }
            }
        }

        void OnMediaMouseMove(IMedia media, double x, double y)
        {
            if (RightHeld)
            {
                if (AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.POINT)
                {
                    if (AnnoTierStatic.Selected != null &&
                        AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.POINT &&
                        control.annoListControl.annoDataGrid.SelectedItem != null &&
                        control.geometricListControl.geometricDataGrid.SelectedItem != null)
                    {
                        double deltaX = x - RightHeldPos[0];
                        double deltaY = y - RightHeldPos[1];

                        RightHeldPos = new double[] { x, y };
                        AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem;

                        foreach (PointListItem pli in control.geometricListControl.geometricDataGrid.SelectedItems)
                        {
                            pli.XCoord += deltaX;
                            pli.YCoord += deltaY;
                        }
                        geometricTableUpdate();
                        int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                        geometricOverlayUpdate(item, AnnoScheme.TYPE.POINT, pos);
                    }
                }
                else if (AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.RECTANGLE)
                {

                   
                }
            }
            else if (LeftHeld)
            {
                //draw a preview of the rectangle whilst moving

                if (AnnoTierStatic.Selected != null &&
                    AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.RECTANGLE &&
                    control.annoListControl.annoDataGrid.SelectedItem != null)
                {
                    AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem;
                    if (control.geometricListControl.geometricDataGrid.SelectedItems.Count > 1) return;
                    RectangleList rectangles = (RectangleList)item.Rectangles;
                    RectangleListItem rectangle = rectangles.Last();
                    rectangle.X2Coord = (int)x;
                    rectangle.Y2Coord = (int)y;
                    rectangle.UpdateADCoords();
                    int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
                    geometricTableUpdate();
                }
            }
        }

        void OnMediaMouseUp(IMedia media, double x, double y)
        {
            if (Mouse.RightButton == MouseButtonState.Released && RightHeld)
            {
                RightHeld = false;
            }

            if (Mouse.LeftButton == MouseButtonState.Released && LeftHeld)
            {
                LeftHeld = false;

                //if (AnnoTierStatic.Selected != null &&
                //    AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.RECTANGLE &&
                //    control.annoListControl.annoDataGrid.SelectedItem != null &&
                //    control.geometricListControl.geometricDataGrid.SelectedItem != null)
                //{
                if (AnnoTierStatic.Selected != null &&
                    AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.RECTANGLE &&
                    (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem != null)
                {
                    AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem;
                    if (control.geometricListControl.geometricDataGrid.SelectedItems.Count > 1) return;

                    RectangleList rectangles = (RectangleList)item.Rectangles;
                    RectangleListItem rectangle = rectangles.Last();
                    rectangle.X2Coord = (int)x;
                    rectangle.Y2Coord = (int)y;
                    rectangle.Confidence = 1;
                    rectangle.UpdateADCoords();
                    int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
                    geometricTableUpdate();
                }
            }

            
        }

        void OnMediaMouseDown(IMedia media, double x, double y)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (AnnoTierStatic.Selected != null &&
                    AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.POINT &&
                    control.annoListControl.annoDataGrid.SelectedItem != null &&
                    control.geometricListControl.geometricDataGrid.SelectedItem != null)
                {
                    AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem;
                    if (control.geometricListControl.geometricDataGrid.SelectedItems.Count > 1) return;
                    PointListItem point = (PointListItem)control.geometricListControl.geometricDataGrid.SelectedItem;
                    point.XCoord = x;
                    point.YCoord = y;
                    int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.POINT, pos);
                    geometricTableUpdate();
                }
                else if (AnnoTierStatic.Selected != null &&
                    AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.RECTANGLE &&
                    control.annoListControl.annoDataGrid.SelectedItem != null) 
                    //control.geometricListControl.geometricDataGrid.SelectedItem != null)
                {
                    AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem;
                    if (control.geometricListControl.geometricDataGrid.SelectedItems.Count > 1) return;
                    //RectangleListItem rectangle = (RectangleListItem)control.geometricListControl.geometricDataGrid.SelectedItem;
                    RectangleList rectangles = (RectangleList)item.Rectangles;
                    string label = (rectangles.Count + 1).ToString();
                    if (control.geometricListControl.geometricDataGrid.SelectedItem != null)
                    {
                        RectangleListItem rectangle = (RectangleListItem)control.geometricListControl.geometricDataGrid.SelectedItem;
                        //rectangle = new RectangleListItem((int)x, (int)y, (int)x + 1, (int)y + 1, label, 1);
                        //rectangles.Add(rectangle);
                        rectangle.X1Coord = (int)x;
                        rectangle.Y1Coord = (int)y;
                        rectangle.X2Coord = (int)x;
                        rectangle.Y2Coord = (int)y;
                        rectangle.UpdateADCoords();
                        control.geometricListControl.geometricDataGrid.SelectedItem = null;
                    }
                    else
                    {
                        RectangleListItem rectangle = new RectangleListItem((int)x, (int)y, (int)x + 1, (int)y + 1, label, 1);
                        rectangles.Add(rectangle);
                    }
                    
                    int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
                    geometricTableUpdate();
                }
            }
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                RightHeldPos = new double[] { x, y };
                RightHeld = true;
            }
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                leftHeldPos = new double[] { x, y };
                LeftHeld = true;
            }
        }

        private void geometricListEdit_Click(object sender, RoutedEventArgs e)
        {
            if (control.geometricListControl.geometricDataGrid.SelectedItems.Count == 1)
            {
                string name = control.geometricListControl.geometricDataGrid.SelectedItems[0].GetType().Name;
                if (name == "PointListItem")
                {
                    foreach (PointListItem item in control.geometricListControl.geometricDataGrid.SelectedItems)
                    {
                        item.Label = control.geometricListControl.editTextBox.Text;
                    }
                }
                else if (name == "RectangleListItem")
                {
                    foreach (RectangleListItem item in control.geometricListControl.geometricDataGrid.SelectedItems)
                    {
                        item.Label = control.geometricListControl.editTextBox.Text;
                    }
                }
            }
        }

        private void geometricListEdit_Focused(object sender, MouseEventArgs e)
        {
            control.geometricListControl.editTextBox.SelectAll();
        }

        private void geometricListSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (control.geometricListControl.geometricDataGrid.Items.Count > 0)
            {
                control.geometricListControl.geometricDataGrid.SelectAll();
            }
        }

        private void geometricListCopy_Click(object sender, RoutedEventArgs e)
        {
            if (control.annoListControl.annoDataGrid.SelectedItems.Count == 1)
            {
                if (control.geometricListControl.geometricDataGrid.Items[0].GetType().Name == "PointListItem")
                {
                    AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItems[0];
                    AnnoList list = (AnnoList)control.annoListControl.annoDataGrid.ItemsSource;

                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (Math.Round(list[i].Start, 2) == Math.Round(item.Stop, 2))
                        {
                            for (int j = 0; j < list[i].Points.Count; ++j)
                            {
                                list[i].Points[j].Label = item.Points[j].Label;
                                list[i].Points[j].XCoord = item.Points[j].XCoord;
                                list[i].Points[j].YCoord = item.Points[j].YCoord;
                            }
                            break;
                        }
                    }
                }
                else if (control.geometricListControl.geometricDataGrid.Items[0].GetType().Name == "RectangleListItem")
                {
                    AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItems[0];
                    AnnoList list = (AnnoList)control.annoListControl.annoDataGrid.ItemsSource;

                    for (int i = 0; i < list.Count; ++i)
                    {
                        if (Math.Round(list[i].Start, 2) == Math.Round(item.Stop, 2))
                        {
                            for (int j = 0; j < list[i].Points.Count; ++j)
                            {
                                list[i].Rectangles[j].Label = item.Rectangles[j].Label;
                                
                                list[i].Rectangles[j].X1Coord = (int)item.Rectangles[j].X1Coord;
                                list[i].Rectangles[j].Y1Coord = (int)item.Rectangles[j].Y1Coord;
                                list[i].Rectangles[j].X2Coord = (int)item.Rectangles[j].X2Coord;
                                list[i].Rectangles[j].Y2Coord = (int)item.Rectangles[j].Y2Coord;
                                list[i].Rectangles[j].UpdateADCoords();
                                
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBoxResult mb = MessageBoxResult.OK;
                mb = MessageBox.Show("Select one frame to copy", "Confirm", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void geometricList_Selection(object sender, SelectionChangedEventArgs e)
        {            
            if (control.annoListControl.annoDataGrid.SelectedItem != null)
            {
                AnnoListItem item = (AnnoListItem) control.annoListControl.annoDataGrid.SelectedItem;
                int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                if (item.Points != null)
                {
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.POINT, pos);
                }
                else if (item.Rectangles != null)
                {
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
                }
                
                
            }

            if (control.geometricListControl.geometricDataGrid.SelectedItems.Count == 1)
            {
                switch (AnnoTierStatic.Selected.AnnoList.Scheme.Type)
                {
                    case AnnoScheme.TYPE.POINT:
                        PointListItem item_p = (PointListItem)control.geometricListControl.geometricDataGrid.SelectedItems[0];
                        control.geometricListControl.editTextBox.Text = item_p.Label;
                        break;
                    case AnnoScheme.TYPE.RECTANGLE:
                        RectangleListItem item_r = (RectangleListItem)control.geometricListControl.geometricDataGrid.SelectedItems[0];
                        control.geometricListControl.editTextBox.Text = item_r.Label;
                        break;
                }
            }
        }

        private void geometricListDelete(object sender, RoutedEventArgs e)
        {
            if (control.geometricListControl.geometricDataGrid.SelectedItems.Count != 0 
                && control.annoListControl.annoDataGrid.SelectedItem != null) // and is a point
            {
                AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem;
                foreach (PointListItem point in control.geometricListControl.geometricDataGrid.SelectedItems)
                {
                    point.XCoord = -1;
                    point.YCoord = -1;
                }
                geometricTableUpdate();
                int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                geometricOverlayUpdate(item, AnnoScheme.TYPE.POINT, pos);
            }
    }

        private void geometricKeyDown(object sender, KeyEventArgs e)
        {
            if (control.annoListControl.annoDataGrid.SelectedItems.Count == 1 && control.geometricListControl.geometricDataGrid.SelectedItems.Count == 1)
            {
                int index = control.geometricListControl.geometricDataGrid.SelectedIndex;
                if (e.Key == Key.OemPeriod)
                {
                    if (index + 1 < control.geometricListControl.geometricDataGrid.Items.Count)
                    {
                        while (control.geometricListControl.geometricDataGrid.SelectedItems.Count > 0)
                        {
                            control.geometricListControl.geometricDataGrid.SelectedItems.RemoveAt(0);
                        }
                        control.geometricListControl.geometricDataGrid.SelectedItems.Add(control.geometricListControl.geometricDataGrid.Items[index + 1]);
                        geometricOverlayUpdate(null, AnnoScheme.TYPE.CONTINUOUS, 0);
                        geometricTableUpdate();
                    }
                }
                else if (e.Key == Key.OemComma)
                {
                    if (index - 1 >= 0)
                    {
                        while (control.geometricListControl.geometricDataGrid.SelectedItems.Count > 0)
                        {
                            control.geometricListControl.geometricDataGrid.SelectedItems.RemoveAt(0);
                        }
                        control.geometricListControl.geometricDataGrid.SelectedItems.Add(control.geometricListControl.geometricDataGrid.Items[index - 1]);
                        geometricOverlayUpdate(null, AnnoScheme.TYPE.CONTINUOUS, 0);
                        geometricTableUpdate();
                    }
                }
                else if (e.Key == Key.R)
                {
                    if (AnnoTierStatic.Selected != null &&
                        AnnoTierStatic.Selected.AnnoList.Scheme.Type == AnnoScheme.TYPE.RECTANGLE &&
                        control.annoListControl.annoDataGrid.SelectedItem != null)
                    {
                        AnnoListItem item = (AnnoListItem)control.annoListControl.annoDataGrid.SelectedItem;
                        RectangleList rectangles = (RectangleList)item.Rectangles;
                        RectangleListItem rectangle = new RectangleListItem(-1, -1, -1, -1, (rectangles.Count+1).ToString(), 0);
                        rectangles.Add(rectangle);
                        geometricTableUpdate();
                        // add functionality to select last in geometric list
                    }
                }
            }
        }

        public void jumpToGeometric(int pos)
        {
            if (control.annoListControl.annoDataGrid.Items.Count == 0) return;

            while (control.annoListControl.annoDataGrid.SelectedItems.Count > 0)
            {
                control.annoListControl.annoDataGrid.SelectedItems.RemoveAt(0);
            }

            control.annoListControl.annoDataGrid.SelectedItems.Add(control.annoListControl.annoDataGrid.Items[pos]);
            if (control.geometricListControl.geometricDataGrid.Items.Count != 0)
            {
                control.geometricListControl.geometricDataGrid.Items.Refresh();
                control.geometricListControl.geometricDataGrid.ScrollIntoView(control.geometricListControl.geometricDataGrid.Items[0]);
            }
            control.annoListControl.annoDataGrid.Items.Refresh();
            control.annoListControl.annoDataGrid.ScrollIntoView(control.annoListControl.annoDataGrid.Items[pos]);
            geometricOverlayUpdate(null,AnnoScheme.TYPE.CONTINUOUS, 0);
        }

        private List<AnnoList> geometricCompare = new List<AnnoList>(0);
    }
}

