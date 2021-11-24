using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace UserData.Pages
{
    public partial class PageUsersList : Page
    {
        List<users> users;
        List<users> users1;
        ChangingPage cp = new ChangingPage();
        public PageUsersList()
        {
            InitializeComponent();
            users = BaseConnect.BaseModel.users.ToList();
            lbUsersList.ItemsSource = users;
            lbGenderFilter.ItemsSource = BaseConnect.BaseModel.genders.ToList();
            lbGenderFilter.SelectedValuePath = "id";
            lbGenderFilter.DisplayMemberPath = "gender";
            users1 = users;
            DataContext = cp;//поместил объект в ресурсы страницы
        }

        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
            //senser содержит объект, который вызвал данное событие, но только у него объектный тип, надо преобразовать
            ListBox lb = (ListBox)sender;//lb содержит ссылку на тот список, который вызвал это событие
            int index = Convert.ToInt32(lb.Uid);//получаем ID элемента списка. в данном случае оно совпадает с id user
            lb.ItemsSource = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == index).ToList();
            lb.DisplayMemberPath = "traits.trait";//показываем пользователю текстовое описание качества
        }
        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == ind);//запись о текущем пользователе
            usersimage UI = BaseConnect.BaseModel.usersimage.FirstOrDefault(x => x.id_user == ind);//получаем запись о картинке для текущего пользователя
            BitmapImage BI = new BitmapImage();
            if (UI != null)//если для текущего пользователя существует запись о его катринке
            {
                if (UI.path != null)//если присутствует путь к картинке
                {
                    BI = new BitmapImage(new Uri(UI.path, UriKind.Relative));
                }
                else//если присутствуют двоичные данные
                {
                    BI.BeginInit();//начать инициализацию BitmapImage (для помещения данных из какого-либо потока)
                    BI.StreamSource = new MemoryStream(UI.image);//помещаем в источник данных двоичные данные из потока
                    BI.EndInit();//закончить инициализацию
                }
            }
            else//если в базе не содержится картинки, то ставим заглушку
            {
                switch (U.gender)//в зависимости от пола пользователя устанавливаем ту или иную картинку
                {
                    case 1:
                        BI = new BitmapImage(new Uri(@"/Images/Male.png", UriKind.Relative));
                        break;
                    case 2:
                        BI = new BitmapImage(new Uri(@"/Images/Female.png", UriKind.Relative));
                        break;
                    default:
                        BI = new BitmapImage(new Uri(@"/Images/Other.png", UriKind.Relative));
                        break;
                }
            }
            IMG.Source = BI;//помещаем картинку в image
        }
        private void BtmAddImage_Click(object sender, RoutedEventArgs e)
        {
            Button BTN = (Button)sender;
            int ind = Convert.ToInt32(BTN.Uid);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg"; // задаем расширение по умолчанию
            openFileDialog.Filter = "Изображения |*.jpg;*.png"; // задаем фильтр на форматы файлов
            var result = openFileDialog.ShowDialog();
            if (result == true)//если файл выбран
            {
                System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);//создаем изображение
                ImageConverter IC = new ImageConverter();//конвертер изображения в массив байт
                byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));//непосредственно конвертация
                usersimage UI = new usersimage() { id_user = ind, image = ByteArr };//создаем новый объект usersimage
                BaseConnect.BaseModel.usersimage.Add(UI);//добавляем его в модель
                BaseConnect.BaseModel.SaveChanges();//синхронизируем с базой
                MessageBox.Show("Изображение добавлено в базу");
            }
            else
            {
                MessageBox.Show("Операция отменена");
            }
        }
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;//определяем, какой текстовый блок был нажат           
            //изменение номера страници при нажатии на кнопку
            switch (tb.Uid)
            {
                case "prev":
                    cp.CurrentPage--;
                    break;
                case "next":
                    cp.CurrentPage++;
                    break;
                default:
                    cp.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            

            //определение списка
            lbUsersList.ItemsSource = users1.Skip(cp.CurrentPage * cp.CountPage - cp.CountPage).Take(cp.CountPage).ToList();

            txtCurrentPage.Text = "Текущая страница: " + (cp.CurrentPage).ToString();


        }
        private void Filter(object sender, RoutedEventArgs e)
        {

            //фильтр по полу
            if (lbGenderFilter.SelectedValue != null)//если пункт из списка не выбран, то сам фильтр работать не будет
            {
                users1 = users1.Where(x => x.gender == (int)lbGenderFilter.SelectedValue).ToList();
            }

            //фильтр по имени
            if (txtNameFilter.Text != "")
            {
                users1 = users1.Where(x => x.name.Contains(txtNameFilter.Text)).ToList();
            }

            lbUsersList.ItemsSource = users1;// возвращаем результат в виде списка, к которому применялись активные фильтры
            cp.Countlist = users1.Count;//меняем количество элементов в списке для постраничной навигации
        }
        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                cp.CountPage = Convert.ToInt32(txtPageCount.Text);
            }
            catch
            {
                cp.CountPage = users1.Count;
            }
            cp.Countlist = users.Count;
            lbUsersList.ItemsSource = users1.Skip(0).Take(cp.CountPage).ToList();
        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            switch (RB.Uid)
            {
                case "name":
                    users1 = users1.OrderBy(x => x.name).ToList();
                    break;
                case "DR":
                    users1 = users1.OrderBy(x => x.dr).ToList();
                    break;
            }
            if (RBReverse.IsChecked == true) users1.Reverse();
            lbUsersList.ItemsSource = users1;
        }
        private void btnGo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lbUsersList.ItemsSource = users;//в качестве источника данных новый список
        }

        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new adminMenu());
        }
    }
}
