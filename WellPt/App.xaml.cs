using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
using Microsoft.VisualBasic.FileIO;

namespace WellPt
{
    public partial class App : Application
    {
        public App()
        {
            CommandBinding binding = new CommandBinding(Command_CloseWindow.RCmd, Command_CloseWindow.executed, Command_CloseWindow.canExecute);

            //Register CommandBinding for all windows
            CommandManager.RegisterClassCommandBinding(typeof(Window), binding);
        }
    }




    /*
    ------------------------------------------------------------
    Utility class
    ------------------------------------------------------------
    */
    public struct Util
    {
        ///--Method
        public static List<string[]> ParseCSV(StringReader text)
        {
            string[] fields;
            List<string[]> parsedData = new List<string[]>();

            TextFieldParser parser = new TextFieldParser(text);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            while (!parser.EndOfData)
            {
                fields = parser.ReadFields();
                parsedData.Add(fields);
            }

            parser.Close();

            return parsedData;
        }

        public static List<ImageSource> GetImageFromUri(string[] uris)
        {
            List<ImageSource> imgs = new List<ImageSource>();

            foreach (string uri in uris)
            {
                ImageSource img = new BitmapImage(new Uri(uri, UriKind.Relative));
                imgs.Add(img);
            }

            return imgs;
        }

        public static ImageSource GetImageFromUri(string uri)
        {
            return new BitmapImage(new Uri(uri, UriKind.Relative));
        }
    }

    public class TypeWriter
    {
        ///--Field and property
        private string targetText;
        private EventHandler stateChagedHandler;
        private Storyboard sb;

        public double TypeSpeed { get; set; }
        public TextBlock TargetTextBlock { get; set; }
        public string TargetText
        {
            get { return this.targetText; }
            set
            {
                this.targetText = value;
                this.sb = SetSb();
                this.sb.CurrentStateInvalidated += this.stateChagedHandler;
            }
        }


        ///--Method
        private Storyboard SetSb()
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(this.TargetText.Length / this.TypeSpeed * 1000));
            DiscreteStringKeyFrame discreteStringKeyFrame;
            StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames();
            stringAnimationUsingKeyFrames.Duration = new Duration(timeSpan);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath(TextBlock.TextProperty));
            
            string tmp = string.Empty;
            foreach (char c in this.TargetText)
            {
                discreteStringKeyFrame = new DiscreteStringKeyFrame();
                discreteStringKeyFrame.KeyTime = KeyTime.Paced;
                tmp += c;
                discreteStringKeyFrame.Value = tmp;
                stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
            }

            Storyboard sb = Application.Current.Resources["Sb_TypeWriter"] as Storyboard;
            sb.Children.Add(stringAnimationUsingKeyFrames);

            return sb;
        }

        public void StartTypewrite()
        {
            sb.Begin(this.TargetTextBlock, true);
        }

        public void FillTypewrite()
        {
            sb.SkipToFill(this.TargetTextBlock);
        }

        public ClockState GetStatus()
        {
            return sb.GetCurrentState(this.TargetTextBlock);
        }


        ///--Constructor
        public TypeWriter(TextBlock targetTextBlock, EventHandler eventHandler, double typeSpeed=15.00)
        {
            this.TypeSpeed = typeSpeed;
            this.TargetTextBlock = targetTextBlock;
            this.stateChagedHandler = eventHandler;
        }
    }




    /*
    ------------------------------------------------------------
    Command class
    - Define commands here and bind in app()
    ------------------------------------------------------------
    */
    public class Command_CloseWindow
    {
        ///--Field and property
        public static RoutedCommand RCmd { get; set; }


        ///--Method
        public static void executed(object sender, ExecutedRoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(e.Source as Control);

            ///Window fadeout and close
            ///By subscribing the sb.Completed event
            ///Use different argument names to avoid conflict
            Storyboard sb = Application.Current.Resources["Sb_FadeOut"] as Storyboard;
            sb.Completed += (object _sender, EventArgs _e) => { currentWindow.Close(); };
            sb.Begin(currentWindow);
        }

        public static void canExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        ///--Constructor
        static Command_CloseWindow()
        {
            RCmd = new RoutedCommand();
        }
    }




    /*
    ------------------------------------------------------------
    Data class
    ------------------------------------------------------------
    */
    public class Data_General : INotifyPropertyChanged
    {
        ///--Field and property
        public readonly static List<string[]> QBook = Util.ParseCSV(new StringReader(WellPt.Properties.Resources.QBook));
        public readonly static List<Data_StrItem> StrBook = initializeStrBook();
        private double ui_mask_opacity;
        private int ui_mask_zindex;

        public double Ui_Mask_Opacity
        {
            get { return this.ui_mask_opacity; }
            set
            {
                if (this.ui_mask_opacity != value)
                {
                    this.ui_mask_opacity = value;
                    this.NotifyPropertyChanged("Ui_Mask_Opacity");
                }
            }
        }
        public int Ui_Mask_Zindex
        {
            get { return this.ui_mask_zindex; }
            set
            {
                if (this.ui_mask_zindex != value)
                {
                    this.ui_mask_zindex = value;
                    this.NotifyPropertyChanged("Ui_Mask_Zindex");
                }
            }
        }


        ///--Method
        private static List<Data_StrItem> initializeStrBook()
        {
            List<Data_StrItem> strBook = new List<Data_StrItem>();
            List<string[]> rawStrBook = Util.ParseCSV(new StringReader(WellPt.Properties.Resources.StringBook));

            foreach (string[] item in rawStrBook)
            {
                string itemSid = item[0];
                string[] itemContent = item.Skip(1).ToArray(); ///LINQ
                strBook.Add(new Data_StrItem(itemSid, itemContent));
            }
            return strBook;
        }


        ///--Constructor
        public Data_General() { Ui_Mask_Opacity = 0.1; Ui_Mask_Zindex = 0; }


        ///--Property change event handle
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public enum NotificationType { greeting, completion };
    public class Data_Notification : INotifyPropertyChanged
    {
        ///--Field and property
        private string dStr;

        public ImageSource Portrait { get; set; }
        public string[] DialogStrs { get; set; }
        public string BtnStr { get; set; }
        public string DStr
        {
            get { return dStr; }
            set
            {
                if (dStr != value)
                {
                    dStr = value;
                    NotifyPropertyChanged("DStr");
                }
            }
        }


        ///--Constructor
        public Data_Notification(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.greeting:
                default:
                    Portrait = Util.GetImageFromUri("img/M_elf_1.png");
                    IEnumerable<string[]> tmp = from strItem in Data_General.StrBook
                                                where strItem.Sid == "note_2"
                                                select strItem.Content;

                    DialogStrs = tmp.FirstOrDefault().ToArray();
                    BtnStr = "Hello";
                    break;
            }
        }


        ///--Property change event handle
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class Data_QItem : INotifyPropertyChanged
    {
        ///--Field and property
        private int id;
        private string prompt;
        private string[] options;

        public string Image { set; get; }
        public int Answer { set; get; }
        public string Feedback { set; get; }
        public int Id
        {
            get { return this.id; }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    this.NotifyPropertyChanged("Id");

                    this.Prompt = Data_General.QBook[this.Id][1];
                    this.Options = new string[]
                    {
                        Data_General.QBook[this.Id][2],
                        Data_General.QBook[this.Id][3],
                        Data_General.QBook[this.Id][4],
                        Data_General.QBook[this.Id][5]
                    };
                    this.Image = Data_General.QBook[this.Id][6];
                    this.Answer = Int32.Parse(Data_General.QBook[this.Id][7]);
                    this.Feedback = Data_General.QBook[this.Id][8];
                }
            }
        }
        public string Prompt
        {
            get { return this.prompt; }
            set
            {
                if (this.prompt != value)
                {
                    this.prompt = value;
                    this.NotifyPropertyChanged("Prompt");
                }
            }
        }
        public string[] Options
        {
            get { return this.options; }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.NotifyPropertyChanged("Options");
                }
            }
        }


        ///--Constructor
        public Data_QItem(int id) { this.Id = id; }


        ///--Property change event handle
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class Data_StrItem
    {
        ///--Field and property
        private string sid;
        private string[] content;

        public string Sid { get { return this.sid; } }
        public string[] Content { get { return this.content; } }


        ///--Constructor
        public Data_StrItem( string rsid, string[] rcontent )
        {
            this.sid = rsid;
            this.content = rcontent;
        }
    }



    /*
    ------------------------------------------------------------
    Converter
    ------------------------------------------------------------
    */
    public class Converter_QId : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, System.Globalization.CultureInfo culture)
        {
            return "Q" + value + " of " + (Data_General.QBook.Count - 1).ToString();
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class Converter_QOption : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, System.Globalization.CultureInfo culture)
        {
            string[] options = (string[])value;
            return options[System.Convert.ToInt16(parameter) - 1]; ///Have to use obvious system.convert as the value from XAML is not actually int
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
