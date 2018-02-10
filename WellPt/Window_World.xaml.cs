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
        public DataContainer WDataContainer = new DataContainer();


        //--Constructor
        public Window_World()
        {
            InitializeComponent();

            this.DataContext = this.WDataContainer;
        }




        /*
        ------------------------------------------------------------
        Events
        ------------------------------------------------------------
        */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (this.Resources["WindowOn"] as Storyboard);
            sb.Begin(this);
        }
    }
}
