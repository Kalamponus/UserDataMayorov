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
            DataContext = cp;
        }

        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
  
            ListBox lb = (ListBox)sender;
            int index = Convert.ToInt32(lb.Uid);
            lb.ItemsSource = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == index).ToList();
            lb.DisplayMemberPath = "traits.trait";
        }
        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == ind);
            usersimage UI = BaseConnect.BaseModel.usersimage.FirstOrDefault(x => x.id_user == ind);
            BitmapImage BI = new BitmapImage();
            if (UI != null)
            {
                if (UI.path != null)
                {
                    BI = new BitmapImage(new Uri(UI.path, UriKind.Relative));
                }
                else
                {
                    BI.BeginInit();
                    BI.StreamSource = new MemoryStream(UI.image);
                    BI.EndInit();
                }
            }
            else
            {
                switch (U.gender)
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
            IMG.Source = BI;
        }
        private void BtmAddImage_Click(object sender, RoutedEventArgs e)
        {
            Button BTN = (Button)sender;
            int ind = Convert.ToInt32(BTN.Uid);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg"; 
            openFileDialog.Filter = "Изображения |*.jpg;*.png"; 
            var result = openFileDialog.ShowDialog();
            if (result == true)//если файл выбран
            {
                System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);
                ImageConverter IC = new ImageConverter();
                byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));
                usersimage UI = new usersimage() { id_user = ind, image = ByteArr };
                BaseConnect.BaseModel.usersimage.Add(UI);
                BaseConnect.BaseModel.SaveChanges();
                MessageBox.Show("Изображение добавлено в базу");
            }
            else
            {
                MessageBox.Show("Операция отменена");
            }
        }
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;      

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
            //фильтр по имени
            if (txtNameFilter.Text != "")
            {
                users1 = users.Where(x => x.name.Contains(txtNameFilter.Text)).ToList();
            }
            if (lbGenderFilter.SelectedValue != null)
            {
                users1 = users.Where(x => x.gender == (int)lbGenderFilter.SelectedValue).ToList();
            }
            lbUsersList.ItemsSource = users1;
            cp.Countlist = users1.Count;
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
            lbUsersList.ItemsSource = users;
        }

        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new adminMenu());
        }
        private void UserImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image im = (System.Windows.Controls.Image)sender;
            int index = Convert.ToInt32(im.Uid);
            Windows.Gallery G = new Windows.Gallery(index);
            G.Show();
        }
    }
}
