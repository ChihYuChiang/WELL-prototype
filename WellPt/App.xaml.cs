using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
    public partial class App : Application {}




    /*
    ------------------------------------------------------------
    Utility class
    ------------------------------------------------------------
    */
    public struct Util
    {
        public static void Typewrite(TextBlock textBlock, int typeSpeed = 10)
        {
            string targetText = textBlock.Text;
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, targetText.Length / typeSpeed * 1000);
            DiscreteStringKeyFrame discreteStringKeyFrame;
            StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames();
            stringAnimationUsingKeyFrames.Duration = new Duration(timeSpan);

            string tmp = string.Empty;
            foreach (char c in targetText)
            {
                discreteStringKeyFrame = new DiscreteStringKeyFrame();
                discreteStringKeyFrame.KeyTime = KeyTime.Paced;
                tmp += c;
                discreteStringKeyFrame.Value = tmp;
                stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
            }

            Storyboard sb = Application.Current.Resources["Sb_TypeWriter"] as Storyboard;
            sb.Children.Add(stringAnimationUsingKeyFrames);

            sb.Begin(textBlock, true);
        }

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

            string line = parser.ReadLine();

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




    /*
    ------------------------------------------------------------
    Data class
    ------------------------------------------------------------
    */
    public class Data_General : INotifyPropertyChanged
    {
        ///--Field and property
        public static List<string[]> QBook = Util.ParseCSV(new StringReader(WellPt.Properties.Resources.QBook));
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


        ///--Constructor
        public Data_General() { Ui_Mask_Opacity = 0.1; Ui_Mask_Zindex = 0; }


        ///--Property change event handle
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

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
                    OnPropertyChanged("DStr");
                }
            }
        }


        ///--Method


        ///--Constructor
        public Data_Notification(string type)
        {
            switch (type)
            {
                case "greeting":
                default:
                    Portrait = Util.GetImageFromUri("img/M_elf_1.png");
                    DialogStrs = new string[] { "Hello, I am a cute elf!", "Nice to meet you.", "How are you doing recently?" };
                    BtnStr = "Hello";
                    break;
            }
        }


        ///--Property change event handle
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
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
