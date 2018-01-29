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
using System.ComponentModel;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace WellPt
{
    public partial class MainWindow : Window
    {
        public static List<string[]> qBook = ParseCSV(new StringReader(Properties.Resources.QBook));

        private QItem dItem = new QItem() { Prompt = MainWindow.qBook[1][1] };

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

            dItem.Prompt = "test us to do so";
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
    }

    public class QItem : INotifyPropertyChanged
    {
        private string prompt;
        
        public int Id { get; set; }
        public int Ans { get; set; }
        public string[] Opts { get; set; }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
