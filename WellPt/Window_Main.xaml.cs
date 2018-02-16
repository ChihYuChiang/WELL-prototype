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
        ///--Field and property
        private ResourceDictionary appR = Application.Current.Resources;
        private int correctCount, checkedOption = 0;

        public Data_QItem DItem { get; set; }
        public Data_General WDataContainer { get; set; }


        ///--Constructor
        public Window_Main()
        {
            InitializeComponent();

            this.DataContext = this;
            this.DItem = new Data_QItem(1);
            this.WDataContainer = new Data_General();
        }




        /*
        ------------------------------------------------------------
        Event handler
        ------------------------------------------------------------
        */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (appR["Sb_FadeIn"] as Storyboard).Begin(this);
            (appR["Sb_ConRotateS"] as Storyboard).Begin(ui_elf_1);
            (appR["Sb_HaloS"] as Storyboard).Begin(ui_sun_1);
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Command_DragWindow.RCmd?.Execute(null, this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Resources["sbAnimateImage4"] as Storyboard).Begin();

            ui_dItem_opt1.IsChecked = false;
            ui_dItem_opt2.IsChecked = false;
            ui_dItem_opt3.IsChecked = false;
            ui_dItem_opt4.IsChecked = false;

            if (checkedOption == DItem.Answer) { correctCount += 1; }

            if (DItem.Id == Data_General.QBook.Count - 1 || correctCount == 3)
            {
                ///Primary window mask
                WDataContainer.Ui_Mask_Zindex = 100;
                WDataContainer.Ui_Mask_Opacity = 0.7;
                WDataContainer.Ui_Mask_Fill = appR["Brush_BG_Dark"] as SolidColorBrush;

                ///Open notification window
                Window_Notification note = new Window_Notification(NotificationType.greeting) { Owner = this };
                note.ShowDialog();
                return;
            }
            
            (Resources["sbAnimateImage"] as Storyboard).Begin();

            ///Next item
            DItem.Id += 1;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Char[] charArray = (sender as RadioButton).Name.ToCharArray();
            checkedOption = (int)Char.GetNumericValue(charArray[charArray.Length - 1]);
        }
    }
}
