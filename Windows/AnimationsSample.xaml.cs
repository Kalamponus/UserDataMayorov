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
using System.Windows.Threading;

namespace UserData.Windows
{
    /// <summary>
    /// Логика взаимодействия для AnimationsSample.xaml
    /// </summary>
    public partial class AnimationsSample : Window
    {
        public AnimationsSample()
        {
            InitializeComponent();
            btnAnimation();
        }

        public void Gif(object sender, EventArgs e)
        {
            //gifManul.MediaEnded += Gif;
            gifManul.Position = TimeSpan.FromMilliseconds(1);
        }

        public void btnAnimation()
        {
            //widthAnim
            DoubleAnimation widthAnimation = new DoubleAnimation();
            widthAnimation.From = btnAnim.ActualWidth;
            widthAnimation.To = 200;
            widthAnimation.Duration = TimeSpan.FromSeconds(3);
            widthAnimation.AutoReverse = true;
            widthAnimation.RepeatBehavior = RepeatBehavior.Forever;
            btnAnim.BeginAnimation(Button.WidthProperty, widthAnimation);
            //heigthAnim
            DoubleAnimation heightAnimation = new DoubleAnimation();
            heightAnimation.From = btnAnim.ActualHeight;
            heightAnimation.To = 70;
            heightAnimation.Duration = TimeSpan.FromSeconds(3);
            heightAnimation.AutoReverse = true;
            heightAnimation.RepeatBehavior = RepeatBehavior.Forever;
            btnAnim.BeginAnimation(Button.HeightProperty, heightAnimation);
            //colourAnim
            ColorAnimation colorAnimation = new ColorAnimation();//анимация цвета
            colorAnimation.From = Color.FromRgb(255, 165, 0);
            colorAnimation.To = Color.FromRgb(0, 191, 255);
            colorAnimation.Duration = TimeSpan.FromSeconds(10);
            colorAnimation.AutoReverse = true;
            colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
            btnAnim.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            btnAnim.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            //thickness
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();
            thicknessAnimation.From = new Thickness(0);
            thicknessAnimation.To = new Thickness(50);
            thicknessAnimation.Duration = TimeSpan.FromSeconds(2);
            thicknessAnimation.AutoReverse = true;
            btnAnim.BeginAnimation(MarginProperty, thicknessAnimation);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Tag)
            {
                case "Play":
                    videoFox.Play();
                    break;
                case "Pause":
                    videoFox.Pause();
                    break;
                case "Stop":
                    videoFox.Stop();
                    break;
            }

        }

        private void slPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan duration = videoFox.NaturalDuration.TimeSpan;//получаем длительность видео в формате timespan
            double d = duration.TotalSeconds;// длительность в миллисекундах           
            videoFox.Position = TimeSpan.FromSeconds(d * slPosition.Value);//установка конкретного значения

        }

        private TimeSpan TotalTime;

       

        private void videoFox_MediaOpened(object sender, RoutedEventArgs e)
        {
           
            //TotalTime = videoFox.NaturalDuration.TimeSpan;
            //slPosition.Maximum = videoFox.NaturalDuration.TimeSpan.TotalSeconds;
            //MessageBox.Show(videoFox.NaturalDuration + " " + slPosition.Maximum);
            //// Create a timer that will update the counters and the time slider
            //DispatcherTimer timerVideoTime = new DispatcherTimer();
            //timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            //timerVideoTime.Tick += new EventHandler(timer_Tick);
            //timerVideoTime.Start();
        }
        
        

        //private void videoFox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (TotalTime.TotalSeconds > 0)
        //    {
        //        videoFox.Position = TimeSpan.FromSeconds(slPosition.Value * TotalTime.TotalSeconds);
        //    }
        //}
    }
}
