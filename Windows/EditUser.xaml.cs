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

namespace UserData.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        auth User;
        List<users_to_traits> LUTT;
        public EditUser()
        {
            InitializeComponent();
            
        }
        public EditUser(auth SelectedUser)
        {
            InitializeComponent();
            try
            {
                editTraits.ItemsSource = BaseConnect.BaseModel.traits.ToList();                
                editTraits.DisplayMemberPath = "trait";
                User = SelectedUser;
                LUTT = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == User.id).ToList();
                List<users_to_traits> uttL = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == User.id).ToList();
                editName.Text = User.users.name.ToString();
                editDR.Text = User.users.dr.ToString("yyyy MMMM dd");
                editGender.Text = User.users.genders.gender.ToString();
                
                List<traits> Traits = BaseConnect.BaseModel.traits.ToList();
                
                foreach (users_to_traits UT in LUTT)
                {
                    foreach(traits item in editTraits.Items)
                    {
                        if(item.id == UT.id_trait && UT.id_user == User.id)
                        {
                            editTraits.SelectedItems.Add(item);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Bнформации о вас нет в системе или она не полная");
            }
        }
        
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
                     
            try
            {

                //BaseConnect.BaseModel.users.Where(x => x.id == User.id).First().name = editName.ToString();
                //BaseConnect.BaseModel.users.Where(x => x.id == User.id).First().dr = Convert.ToDateTime(editDR.Text);
                //BaseConnect.BaseModel.users.Where(x => x.id == User.id).First().genders.gender = editGender.Text;
                User.users.name = editName.Text;
                User.users.dr = Convert.ToDateTime(editDR.Text.ToString());
                User.users.genders.gender = editGender.Text;
                foreach (traits t in editTraits.SelectedItems)
                {
                    
                    foreach(users_to_traits utt in LUTT)
                    if (editTraits.SelectedItems.Contains(BaseConnect.BaseModel.traits.Where(x => x.id == utt.id_trait).First()) == false)
                    {
                        BaseConnect.BaseModel.users_to_traits.Remove(utt);
                    }

                }
                foreach (traits t in editTraits.SelectedItems)
                {
                    if((LUTT.Count == 0)||(LUTT.Contains(BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == User.id && x.id_trait == t.id).First()) == false))
                    {
                        users_to_traits newUTT = new users_to_traits();
                        newUTT.id_trait = t.id;
                        newUTT.id_user = User.id;
                        BaseConnect.BaseModel.users_to_traits.Add(newUTT);
                    }
                }
                BaseConnect.BaseModel.SaveChanges();
                MessageBox.Show("Изменения успешно применены");
            }
            catch
            {
                MessageBox.Show("Изменения не были применены");
            }
            finally
            {
                this.Close();
            }
        }
    }
}
