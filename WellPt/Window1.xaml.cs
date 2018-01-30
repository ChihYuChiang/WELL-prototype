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

namespace WellPt
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            Storyboard sb = this.FindResource("sbAnimateImage") as Storyboard;
            sb.Begin();

            Storyboard sb2 = this.FindResource("bindicate") as Storyboard;
            sb2.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            string t = "this is a testing string message.  is a testing string message.";
            TypewriteTextblock(t, txtb, new TimeSpan(0, 0, 0, 0, t.Length / 10 * 1000));
        }

        private void TypewriteTextblock(string textToAnimate, TextBlock txt, TimeSpan timeSpan)
        {
            Storyboard sb2 = this.FindResource("bindicate") as Storyboard;
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

            Storyboard sb = this.FindResource("story") as Storyboard;
            sb.Children.Add(stringAnimationUsingKeyFrames);

            sb.Begin(txt);
        }

        private void storyCompleted(object sender, EventArgs e)
        {
            Storyboard sb = this.FindResource("bindicate") as Storyboard;
            sb.Begin();
        }
    }
}
