using System;
using System.Collections.Generic;
using System.IO;
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

namespace UserData.Windows
{
    /// <summary>
    /// Логика взаимодействия для Gallery.xaml
    /// </summary>
    public partial class Gallery : Window
    {
        List<usersimage> LUI;
        BitmapImage BI;
        int CountUsersImages;
        int CurrentUserImage = 0;
        public Gallery(int id)
        {
            InitializeComponent();
            Title = "Галерея пользователя: " + BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == id).name;//заголовок окна страницы
            LUI = BaseConnect.BaseModel.usersimage.Where(x => x.id_user == id).ToList();//сам список картинок
            CountUsersImages = LUI.Count;

            BI = new BitmapImage();
            if (CountUsersImages > 0)
            {
                BI.BeginInit();//начало инициации изображения
                BI.StreamSource = new MemoryStream(LUI[0].image);//создание изображения из массива байт 
                BI.EndInit();//конец инцицации изображения
            }
            else
            {
                BI = new BitmapImage(new Uri(@"/Images/Other.png", UriKind.Relative));
            }

            ImgUserPic.Source = BI;
        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CountUsersImages > 0)
            {
                CurrentUserImage--;
                if (CurrentUserImage < 0) CurrentUserImage = CountUsersImages - 1;//карусель
                BI = new BitmapImage();
                BI.BeginInit();//начало инициации изображения
                BI.StreamSource = new MemoryStream(LUI[CurrentUserImage].image);//создание изображения из массива байт                    
                BI.EndInit();//конец инцицации изображения
                ImgUserPic.Source = BI;
            }               
        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CountUsersImages > 0)
            {
                CurrentUserImage++;
                if (CurrentUserImage >= CountUsersImages) CurrentUserImage = 0;//карусель
                BI = new BitmapImage();
                BI.BeginInit();//начало инициации изображения
                BI.StreamSource = new MemoryStream(LUI[CurrentUserImage].image);//создание изображения из массива байт                    
                BI.EndInit();//конец инцицации изображения
                ImgUserPic.Source = BI;
            }               
        }

        private void Ava_Click(object sender, RoutedEventArgs e)
        {
            if (CountUsersImages > 0)
            {
                for (int i = 0; i < CountUsersImages; i++)
                {
                    LUI[i].avatar = i == CurrentUserImage;
                }
                BaseConnect.BaseModel.SaveChanges();
                MessageBox.Show("Аватар успешно изменен");
                BI = new BitmapImage();
                BI.BeginInit();//начало инициации изображения
                BI.StreamSource = new MemoryStream(LUI[CurrentUserImage].image);//создание изображения из массива байт                    
                BI.EndInit();//конец инцицации изображения
                ImgUserPic.Source = BI;
            }               
        }
    }
}
