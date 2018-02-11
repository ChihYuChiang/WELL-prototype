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
    Utility functions
    ------------------------------------------------------------
    */
    public class Util
    {
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

        public static void TypewriteTextblock(Window window, string textToAnimate, TextBlock textBlock, TimeSpan timeSpan)
        {
            DiscreteStringKeyFrame discreteStringKeyFrame;
            StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames();
            stringAnimationUsingKeyFrames.Duration = new Duration(timeSpan);

            string tmp = string.Empty;
            foreach (char c in textToAnimate)
            {
                discreteStringKeyFrame = new DiscreteStringKeyFrame();
                discreteStringKeyFrame.KeyTime = KeyTime.Paced;
                tmp += c;
                discreteStringKeyFrame.Value = tmp;
                stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
            }
            Storyboard.SetTargetName(stringAnimationUsingKeyFrames, textBlock.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath(TextBlock.TextProperty));

            Storyboard sb = Application.Current.Resources["Sb_TypeWriter"] as Storyboard;
            sb.Children.Add(stringAnimationUsingKeyFrames);

            sb.Begin(textBlock);
        }

        public static List<ImageSource> GetImagesFromUris(string[] uris)
        {
            List<ImageSource> imgs = new List<ImageSource>();

            foreach (string uri in uris)
            {
                ImageSource img = new BitmapImage(new Uri(uri, UriKind.Relative));
                imgs.Add(img);
            }

            return imgs;
        }
    }




    /*
    ------------------------------------------------------------
    Data
    ------------------------------------------------------------
    */
    public class DataContainer : INotifyPropertyChanged
    {
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
        public DataContainer() { this.Ui_Mask_Opacity = 0.1; this.Ui_Mask_Zindex = 0; }


        ///--Property change event handle
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class QItem : INotifyPropertyChanged
    {
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

                    this.Prompt = DataContainer.QBook[this.Id][1];
                    this.Options = new string[]
                    {
                        DataContainer.QBook[this.Id][2],
                        DataContainer.QBook[this.Id][3],
                        DataContainer.QBook[this.Id][4],
                        DataContainer.QBook[this.Id][5]
                    };
                    this.Image = DataContainer.QBook[this.Id][6];
                    this.Answer = Int32.Parse(DataContainer.QBook[this.Id][7]);
                    this.Feedback = DataContainer.QBook[this.Id][8];
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
        public QItem(int id) { this.Id = id; }


        ///--Property change event handle
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }




    /*
    ------------------------------------------------------------
    Converters
    ------------------------------------------------------------
    */
    public class Converter_QId : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, System.Globalization.CultureInfo culture)
        {
            return "Q" + value + " of " + (DataContainer.QBook.Count - 1).ToString();
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
