﻿using System;
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
        public int CorrectCount { get; set; }
        public int CheckedOption { get; set; }

        public Window_Main()
        {
            InitializeComponent();
            this.CorrectCount = 0;

            Storyboard sb = (this.FindResource("sbAnimateImage2") as Storyboard);
            sb.Begin();

            Binding bind_qId = new Binding("Id") { Source = dItem, Mode = BindingMode.OneWay, Converter = new Converter_QId() };
            Binding bind_qPrompt = new Binding("Prompt") { Source = dItem, Mode = BindingMode.OneWay };
            Binding bind_qOption1 = new Binding("Options") { Source = dItem, Mode = BindingMode.OneWay, Converter = new Converter_QOption(), ConverterParameter = 1 };
            Binding bind_qOption2 = new Binding("Options") { Source = dItem, Mode = BindingMode.OneWay, Converter = new Converter_QOption(), ConverterParameter = 2 };
            Binding bind_qOption3 = new Binding("Options") { Source = dItem, Mode = BindingMode.OneWay, Converter = new Converter_QOption(), ConverterParameter = 3 };
            Binding bind_qOption4 = new Binding("Options") { Source = dItem, Mode = BindingMode.OneWay, Converter = new Converter_QOption(), ConverterParameter = 4 };
            BindingOperations.SetBinding(ui_dItem_id, Label.ContentProperty, bind_qId);
            BindingOperations.SetBinding(ui_dItem_prompt, Label.ContentProperty, bind_qPrompt);
            BindingOperations.SetBinding(ui_dItem_opt1, Label.ContentProperty, bind_qOption1);
            BindingOperations.SetBinding(ui_dItem_opt2, Label.ContentProperty, bind_qOption2);
            BindingOperations.SetBinding(ui_dItem_opt3, Label.ContentProperty, bind_qOption3);
            BindingOperations.SetBinding(ui_dItem_opt4, Label.ContentProperty, bind_qOption4);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ui_dItem_opt1.IsChecked = false;
            ui_dItem_opt2.IsChecked = false;
            ui_dItem_opt3.IsChecked = false;
            ui_dItem_opt4.IsChecked = false;

            if (CheckedOption == dItem.Answer) { CorrectCount += 1; }

            if (dItem.Id == Util.qBook.Count - 1 || CorrectCount == 3)
            {
                Window_Notification note = new Window_Notification("hello");
                note.Show();
                return;
            }
            
            Storyboard sb = (this.FindResource("sbAnimateImage") as Storyboard);
            sb.Begin();

            dItem.Id += 1;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Char[] charArray = (sender as RadioButton).Name.ToCharArray();
            CheckedOption = (int)Char.GetNumericValue(charArray[charArray.Length - 1]);
        }
    }
}
