using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ssi
{
    /// <summary>
    /// Interaction logic for AnnoTierNewPointSchemeWindow.xaml
    /// </summary>
    public partial class AnnoTierNewRectangleSchemeWindow : Window
    {
        private AnnoScheme scheme;

        public AnnoTierNewRectangleSchemeWindow(ref AnnoScheme scheme)
        {
            InitializeComponent();

            this.scheme = scheme;

            nameTextBox.Text = scheme.Name;
            srTextBox.Text = scheme.SampleRate.ToString();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            scheme.Name = nameTextBox.Text == "" ? Defaults.Strings.Unkown : nameTextBox.Text;
            double value;
            if (double.TryParse(srTextBox.Text, out value))
            {
                scheme.SampleRate = value;
            }

            scheme.MinOrBackColor = System.Windows.Media.Color.FromRgb(0,0,0);
            
            scheme.MaxOrForeColor = System.Windows.Media.Color.FromRgb(0, 0, 0); 

            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
