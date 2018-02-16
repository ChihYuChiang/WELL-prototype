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
using System.ComponentModel;

namespace WellPt
{
    public partial class Window_Notification : Window
    {
        ///--Field and property
        private int currentDialog = 0;
        private TypeWriter typeWriter;
        private ResourceDictionary appR = Application.Current.Resources;

        public Data_Notification Notification { get; set; }


        ///--Constructor
        public Window_Notification(NotificationType type)
        {
            InitializeComponent();

            this.DataContext = this;
            this.Notification = new Data_Notification(type);
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

            ///Initialize the dialog with the first string
            Notification.DStr = Notification.DialogStrs[currentDialog];

            ///Typewriter
            typeWriter = new TypeWriter(ui_dialog, handler_typewriterStateChanged);
            typeWriter.TargetText = Notification.DStr;
            typeWriter.StartTypewrite();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ///Primary window unmask
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

            ///Window fadeout and close
            ///By subscribing the sb.Completed event
            ///Use different argument names to avoid conflict
            Storyboard sb = appR["Sb_FadeOut"] as Storyboard;
            sb.Completed += (object _sender, EventArgs _e) => { this.Close(); };
            sb.Begin(this);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (typeWriter.GetStatus() == ClockState.Active)
            {
                typeWriter.FillTypewrite();
            }
            else
            {
                currentDialog += 1;
                Notification.DStr = Notification.DialogStrs[currentDialog];
                typeWriter.TargetText = Notification.DStr;
                typeWriter.StartTypewrite();
            }
        }

        private void handler_typewriterStateChanged(object sender, EventArgs e)
        {
            switch (typeWriter.GetStatus())
            {
                case ClockState.Active:
                    (appR["Sb_BIndicator"] as Storyboard).Stop(ui_bIndicator);
                    break;
                case ClockState.Filling:
                case ClockState.Stopped:
                    (appR["Sb_BIndicator"] as Storyboard).Begin(ui_bIndicator, true);
                    break;
            }
        }
    }
}
