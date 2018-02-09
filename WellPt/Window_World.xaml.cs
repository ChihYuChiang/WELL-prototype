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

        public Window_World()
        {
            InitializeComponent();
            this.DataContext = this.WDataContainer;

            Storyboard sb = (this.FindResource("WindowOn") as Storyboard);
            Storyboard.SetTarget(sb, this);
            sb.Begin();
        }
    }
}
