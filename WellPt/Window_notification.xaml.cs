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
using System.Windows.Media.Animation;

namespace WellPt
{
    public partial class Window_Notification : Window
    {
        public Window_Notification(string t)
        {
            InitializeComponent();

            Storyboard sb = (this.FindResource("WindowOn") as Storyboard);
            Storyboard.SetTarget(sb, this);
            sb.Begin();

            Bt.Content = t;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (this.FindResource("WindowOff") as Storyboard);
            Storyboard.SetTarget(sb, this);

            //--Anonymous function
            //(object _sender, EventArgs _e) => this.Close()
            //equals to
            //lambda(object _sender, EventArgs _e) {this.Close()}
            sb.Completed += (object _sender, EventArgs _e) =>
            {
                foreach (Window w in Application.Current.Windows)
                {
                    Type[] targetArray = 
                    {
                        typeof(Window_Main),
                        typeof(Window_Entrance),
                        typeof(Window_World)
                    };
                    if (targetArray.Contains(w.GetType()))
                    {
                        (w as Window_Main).WDataContainer.Ui_Mask_Opacity = 0.05;
                        (w as Window_Main).WDataContainer.Ui_Mask_Zindex = 0;
                    }
                }
                this.Close();
            };
            sb.Begin();
        }
    }
}
