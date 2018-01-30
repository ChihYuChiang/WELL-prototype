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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WellPt
{
    public partial class MainWindow : Window
    {
        private QItem dItem = new QItem(1);

        public MainWindow()
        {
            InitializeComponent();

            Storyboard sb = (this.FindResource("sbAnimateImage2") as Storyboard);
            sb.Begin();

            Binding myBinding = new Binding("Prompt") { Source = dItem, Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(ui_dItem_prompt, Label.ContentProperty, myBinding);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (this.FindResource("sbAnimateImage") as Storyboard);
            sb.Begin();

            dItem.Id += 1;
        }
    }
}
