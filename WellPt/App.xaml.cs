using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.IO;
using System.ComponentModel;
using Microsoft.VisualBasic.FileIO;

namespace WellPt
{
    public partial class App : Application {}

    public class Util
    {
        public static List<string[]> qBook = ParseCSV(new StringReader(WellPt.Properties.Resources.QBook));

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

        public static void TypewriteTextblock(Window window, string textToAnimate, TextBlock txt, TimeSpan timeSpan)
        {
            Storyboard sb2 = window.FindResource("bindicate") as Storyboard;
            sb2.Stop();

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
            Storyboard.SetTargetName(stringAnimationUsingKeyFrames, txt.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath(TextBlock.TextProperty));

            Storyboard sb = window.FindResource("story") as Storyboard;
            sb.Children.Add(stringAnimationUsingKeyFrames);

            sb.Begin(txt);
        }
    }


    public class QItem : INotifyPropertyChanged
    {
        private string prompt;
        private int id;

        public int Ans { get; set; }
        public string[] Opts { get; set; }

        public int Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                this.Prompt = Util.qBook[this.Id][1];
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

        public QItem(int id) { this.Id = id; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }


    public class Converter_PromptId : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, System.Globalization.CultureInfo culture)
        {
            return "Q" + value + " of " + (Util.qBook.Count - 1).ToString();
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
