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
    public partial class Window_Main : Window
    {
        private QItem dItem = new QItem(1);

        public Window_Main()
        {
            InitializeComponent();

            Storyboard sb = (this.FindResource("sbAnimateImage2") as Storyboard);
            sb.Begin();

            Binding myBinding = new Binding("Prompt") { Source = dItem, Mode = BindingMode.OneWay, Converter = new Converter_PromptId() };
            BindingOperations.SetBinding(ui_dItem_prompt, Label.ContentProperty, myBinding);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(dItem.Id == Util.qBook.Count - 1)
            {
                Window_Notification note = new Window_Notification("hello");
                note.Show();
                return;
            }
            
            Storyboard sb = (this.FindResource("sbAnimateImage") as Storyboard);
            sb.Begin();

            dItem.Id += 1;
        }
    }
}
