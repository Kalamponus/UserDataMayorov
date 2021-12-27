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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserData.Pages;

namespace UserData
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new login());
            LoadPages.MainFrame = mainFrame;
            BaseConnect.BaseModel = new Entities();
            Windows.AnimationsSample animationsSample = new Windows.AnimationsSample();
            animationsSample.Show();
            Windows.Diagram diagram = new Windows.Diagram();
            diagram.Show();
        }
    }
}
