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
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace WellPt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (this.FindResource("sbAnimateImage") as Storyboard);
            sb.Begin();

            StringReader csvReader = new StringReader(Properties.Resources.QBook);     
            List<string[]> qBook = ParseCSV(csvReader);
        }

        public List<string[]> ParseCSV(StringReader text)
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
}
