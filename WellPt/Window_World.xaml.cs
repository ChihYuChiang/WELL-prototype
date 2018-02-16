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
    public partial class Window_World : Window
    {
        ///--Field and property
        private ResourceDictionary appR = Application.Current.Resources;
        
        public Data_General WDataContainer { get; set; }


        ///--Constructor
        public Window_World()
        {
            InitializeComponent();

            this.WDataContainer = new Data_General();
            this.DataContext = this.WDataContainer;
        }




        /*
        ------------------------------------------------------------
        Event handler
        ------------------------------------------------------------
        */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (appR["Sb_FadeIn"] as Storyboard).Begin(this);
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Command_DragWindow.RCmd?.Execute(null, this);
        }
    }
}
