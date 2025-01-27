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
using MasterPol.AppData.DbMethods;
using MasterPol.Pages.Lists;

namespace MasterPol.Pages.Authorization
{
    /// <summary>
    /// Логика взаимодействия для PageAuthorization.xaml
    /// </summary>
    public partial class PageAuthorization : Page
    {
        public PageAuthorization()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppConnect.model0db.users.FirstOrDefault(u => u.login == tbLogin.Text && u.password == pbPassword.Password);
                if (userObj == null)
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    CurrentUser.Current = userObj;

                    switch (userObj.id_ur)
                    {
                        case 1:
                            MessageBox.Show("Приветствуем вас Администратор!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frmMain.Navigate(new PagePartners());
                            break;
                        case 2:
                            MessageBox.Show("Добро пожаловать в МастерПол!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frmMain.Navigate(new PagePartners());
                            break;
                        default:
                            MessageBox.Show("Возникла ошибка!\nМы уже работаем над этим!\nЗайдите немного позже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка:\n" + ex.Message.ToString(), "Системная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new PageRegistration());
        }
    }
}
