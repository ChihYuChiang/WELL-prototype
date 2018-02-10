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
        private ResourceDictionary appR = Application.Current.Resources;

        public Window_Notification(string t)
        {
            InitializeComponent();

            Storyboard sb = (appR["Sb_FadeIn"] as Storyboard);
            sb.Begin(this);

            Bt.Content = t;
        }




        /*
        ------------------------------------------------------------
        Events
        ------------------------------------------------------------
        */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (appR["Sb_FadeIn"] as Storyboard);
            sb.Begin(this);
            Storyboard sb2 = (appR["Sb_ConRotateS"] as Storyboard);
            sb2.Begin(ui_elf_1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (appR["Sb_FadeOut"] as Storyboard);

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

            //--Anonymous function
            //(object _sender, EventArgs _e) => this.Close()
            //equals to
            //lambda(object _sender, EventArgs _e) {this.Close()}
            sb.Completed += (object _sender, EventArgs _e) => { this.Close(); };
            sb.Begin(this);
        }
    }
}
