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
        private AnnoScheme.TYPE CURRENT_TYPE;
        public void geometricOverlayUpdate(AnnoListItem item, AnnoScheme.TYPE type, int pos = -1)
        {
            WriteableBitmap overlay = null;
            CURRENT_TYPE = type;
            IMedia video = mediaList.GetFirstVideo();

            if (video != null)
            {
                overlay = video.GetOverlay();
            }
            else
            { 
                return;
            }

            overlay.Lock();
            overlay.Clear();
            
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
                    foreach (RectangleListItem r in item.Rectangles)
                    {
                        if (r.X1Coord != -1 && r.Y1Coord != -1 && r.X2Coord != -1 && r.Y2Coord != -1)
                        {
                            Color color = item.Color;
                            color.A /= 4;
                            color.A *= 3;
                            //color.A = 128;
                            int x1 = (int)r.X1Coord;
                            int x2 = (int)r.X2Coord;

                            int y1 = (int)r.Y1Coord;
                            int y2 = (int)r.Y2Coord;

                            double p1 = Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2));
                            double p2 = Math.Sqrt(Math.Pow(x2, 2) + Math.Pow(y2, 2));

                            int volume = Math.Abs(x1 - x2) * Math.Abs(y1 - y2);

                            bool line = false;
                            if (x1 == x2 || y1 == y2)
                            {
                                line = true;
                            }
                            Console.WriteLine(overlay.PixelHeight.ToString() + " " + overlay.PixelWidth.ToString());
                            Console.WriteLine(overlay.Height.ToString() + " " + overlay.Width.ToString());
                            try
                            {
                                if (p1 < p2)
                                {
                                    if (!line)
                                    {
                                        overlay.FillRectangle(x1, y1, x2, y2, color);
                                    }
                                    else
                                    {
                                        overlay.DrawLine(x1, y1, x2, y2, color);
                                    }

                                }
                                else
                                {
                                    if (!line)
                                    {
                                        overlay.FillRectangle(x2, y2, x1, y1, color);
                                    }
                                    else
                                    {
                                        overlay.DrawLine(x2, y2, x1, y1, color);
                                    }

                                }
                            }
                            catch (Exception) { }
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

            overlay.Unlock();
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
                if (CURRENT_TYPE == AnnoScheme.TYPE.POINT)
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
                else if (CURRENT_TYPE == AnnoScheme.TYPE.RECTANGLE)
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
                    string output1 = "---m--- x: " + ((int)x).ToString() + " y: " + ((int)y).ToString();
                    //Console.WriteLine(output1);
                    rectangle.X2Coord = x;
                    rectangle.Y2Coord = y;
                    string output = "+++m++ x1: " + ((int)rectangle.X1Coord).ToString() + " y1: " + ((int)rectangle.Y1Coord).ToString() +
                                    " x2: " + ((int)rectangle.X2Coord).ToString() + " y2: " + ((int)rectangle.Y2Coord).ToString();
                    Console.WriteLine(output);


                    rectangle.X2Coord = x;
                    rectangle.Y2Coord = y;
                    geometricTableUpdate();
                    int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
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
            }

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

                string output1 = "---u--- x: " + ((int)x).ToString() + " y: " + ((int)y).ToString();
                //Console.WriteLine(output1);

                rectangle.X2Coord = x;
                rectangle.Y2Coord = y;

                string output = "+++u+++ x1: " + ((int)rectangle.X1Coord).ToString() + " y1: " + ((int)rectangle.Y1Coord).ToString() +
                                    " x2: " + ((int)rectangle.X2Coord).ToString() + " y2: " + ((int)rectangle.Y2Coord).ToString() + "\n";
                Console.WriteLine(output);
                geometricTableUpdate();
                int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
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
                    geometricTableUpdate();
                    int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.POINT, pos);
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
                    string label = "";
                    string output1 = "\n---d--- x: " + ((int)x).ToString() + " y: " + ((int)y).ToString();
                    //Console.WriteLine(output1);
                    RectangleListItem rectangle = new RectangleListItem(x, y, x+1, y+1, label, 1);
                    rectangles.Add(rectangle);


                    string output = "+++d+++ x1: " + ((int)rectangle.X1Coord).ToString() + " y1: " + ((int)rectangle.Y1Coord).ToString() +
                                    " x2: " + ((int)rectangle.X2Coord).ToString() + " y2: " + ((int)rectangle.Y2Coord).ToString();
                    Console.WriteLine(output);


                    geometricTableUpdate();
                    int pos = control.annoListControl.annoDataGrid.SelectedIndex;
                    geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
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
                                list[i].Rectangles[j].X1Coord = item.Rectangles[j].X1Coord;
                                list[i].Rectangles[j].Y1Coord = item.Rectangles[j].Y1Coord;
                                list[i].Rectangles[j].X2Coord = item.Rectangles[j].X2Coord;
                                list[i].Rectangles[j].Y2Coord = item.Rectangles[j].Y2Coord;
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
                geometricOverlayUpdate(item, AnnoScheme.TYPE.POINT, pos);
                geometricOverlayUpdate(item, AnnoScheme.TYPE.RECTANGLE, pos);
            }

            if (control.geometricListControl.geometricDataGrid.SelectedItems.Count == 1)
            {
                PointListItem item = (PointListItem)control.geometricListControl.geometricDataGrid.SelectedItems[0];
                control.geometricListControl.editTextBox.Text = item.Label;
            }
            else if (control.geometricListControl.geometricDataGrid.SelectedItems.Count == 1)
            {
                RectangleListItem item = (RectangleListItem)control.geometricListControl.geometricDataGrid.SelectedItems[0];
                control.geometricListControl.editTextBox.Text = item.Label;
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
                        geometricTableUpdate();
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

        }

        private List<AnnoList> geometricCompare = new List<AnnoList>(0);
    }
}

