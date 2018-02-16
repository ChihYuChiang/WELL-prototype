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
using System.Windows.Threading;

namespace WellPt
{
    public partial class Window_Entrance : Window
    {
        ///--Field and property
        private ResourceDictionary appR = Application.Current.Resources;
        private DispatcherTimer slide_change;
        private Image[] slide_imgCtrls;
        private List<ImageSource> slide_imgs;
        private int slide_cSourceIndex, slide_cCtrlIndex = 0;
        
        public Data_General WDataContainer = new Data_General();
        

        ///--Constructor
        public Window_Entrance()
        {
            InitializeComponent();
            this.DataContext = this.WDataContainer;

            string[] imgUris = { "img/M_cloud_1.png", "img/M_sun_1.png" };
            this.slide_imgCtrls = new Image[] { ui_bgStory_1, ui_bgStory_2 };
            this.slide_imgs = Util.GetImageFromUri(imgUris);
            this.slide_change = new DispatcherTimer();
            this.slide_change.Interval = new TimeSpan(0, 0, 3);

            ///An anonymous function Subscribes the tick event
            this.slide_change.Tick += (object sender, EventArgs e) => { this.PlaySlideShow(); };
        }




        /*
        ------------------------------------------------------------
        Methods
        ------------------------------------------------------------
        */
        private void PlaySlideShow()
        {
            var oldCtrlIndex = this.slide_cCtrlIndex;
            this.slide_cCtrlIndex = (this.slide_cCtrlIndex + 1) % 2;
            this.slide_cSourceIndex = (this.slide_cSourceIndex + 1) % this.slide_imgs.Count;

            Image imgFadeOut = this.slide_imgCtrls[oldCtrlIndex];
            Image imgFadeIn = this.slide_imgCtrls[this.slide_cCtrlIndex];
            imgFadeIn.Source = this.slide_imgs[this.slide_cSourceIndex];

            (appR["Sb_FadeOut"] as Storyboard).Begin(imgFadeOut);
            (appR["Sb_FadeIn"] as Storyboard).Begin(imgFadeIn);
        }




        /*
        ------------------------------------------------------------
        Event handler
        ------------------------------------------------------------
        */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.slide_change.IsEnabled = true;

            (appR["Sb_FadeIn"] as Storyboard).Begin(this);
            (this.Resources["sbAnimateImage"] as Storyboard).Begin();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Command_DragWindow.RCmd?.Execute(null, this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window_Main main = new Window_Main();
            main.Show();
            this.Close();
        }
    }
}
