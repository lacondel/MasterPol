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
using MasterPol.AppData;

namespace MasterPol.Pages.Authorization
{
    /// <summary>
    /// Логика взаимодействия для PageRegistration.xaml
    /// </summary>
    public partial class PageRegistration : Page
    {
        public PageRegistration()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.model0db.users.Count(u => u.login == tbRegLogin.Text) > 0)
            {
                MessageBox.Show("Пользователь с таким логином уже соществует!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                users userObj = new users()
                {
                    login = tbRegLogin.Text,
                    password = pbRegPass.Password,
                    id_ur = 2
                };
                AppConnect.model0db.users.Add(userObj);
                AppConnect.model0db.SaveChanges();
                MessageBox.Show("Новый пользователь успещно создан!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frmMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("При добавлении данных возникла ошибка:\n" + ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rpChanged (object sender, RoutedEventArgs e)
        {
            PasswordMatch(sender, e);
        }

        private void rprChanged (object sender, RoutedEventArgs e)
        {
            PasswordMatch(sender, e);
        }

        private void PasswordMatch(object sender, RoutedEventArgs e)
        {
            if (pbRegPass.Password != pbRegPassRepeat.Password)
            {
                btnReg.IsEnabled = false;
                pbRegPass.Background = Brushes.Red;
                pbRegPassRepeat.Background = Brushes.Red;
            }
            else
            {
                btnReg.IsEnabled = true;
                pbRegPass.Background = Brushes.Green;
                pbRegPassRepeat.Background= Brushes.Green;
            }
        }
    }
}
