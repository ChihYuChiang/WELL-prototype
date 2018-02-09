using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace WellPt
{
    public partial class Window_Entrance : Window
    {
        public DataContainer WDataContainer = new DataContainer();

        public Window_Entrance()
        {
            InitializeComponent();
            this.DataContext = this.WDataContainer;

            Storyboard sb3 = (this.FindResource("WindowOn") as Storyboard);
            Storyboard.SetTarget(sb3, this);
            sb3.Begin();

            Storyboard sb = this.FindResource("sbAnimateImage") as Storyboard;
            sb.Begin();

            Storyboard sb2 = this.FindResource("bindicate") as Storyboard;
            sb2.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window_Main main = new Window_Main();
            main.Show();
            this.Close();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            string t = "this is a testing string message.  is a testing string message.";
            Util.TypewriteTextblock(this, t, txtb, new TimeSpan(0, 0, 0, 0, t.Length / 10 * 1000));
        }

        private void storyCompleted(object sender, EventArgs e)
        {
            Storyboard sb = this.FindResource("bindicate") as Storyboard;
            sb.Begin();
        }
    }
}
