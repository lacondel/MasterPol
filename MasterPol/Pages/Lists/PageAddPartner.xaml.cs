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

namespace MasterPol.Pages.Lists
{
    /// <summary>
    /// Логика взаимодействия для PageAddPartner.xaml
    /// </summary>
    public partial class PageAddPartner : Page
    {
        public PageAddPartner()
        {
            InitializeComponent();
        }

        private void btnAddPartner_Click(object sender, RoutedEventArgs e)
        {
            if (addPartnerName.Text == null || addPartnerType.SelectedItem == null || addPhone.Text == null || addRating.Text == null)
            {
                MessageBox.Show("Заполните все обязательные поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                using (var context = new MasterPolEntities())
                {
                    var selectedTypeItem = addPartnerType.SelectedItem as ComboBoxItem;
                    string selectedTypeName = selectedTypeItem.Content.ToString();
                    var selectedType = AppConnect.model0db.partner_types.FirstOrDefault(p => p.pat_name == selectedTypeName);

                    partners newPartner = new partners
                    {
                        p_name = addPartnerName.Text,
                        id_pat = selectedType.id_pat,
                        director = addDirector.Text,
                        email = addEmail.Text,
                        phone = addPhone.Text,
                        address = addAddress.Text,
                        INN = decimal.TryParse(addINN.Text, out decimal innValue) ? (decimal?)innValue : null,
                        rating = Int32.Parse(addRating.Text),
                    };
                    context.partners.Add(newPartner);
                    context.SaveChanges();
                    MessageBox.Show("Партнёр успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frmMain.Navigate(new PagePartners());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка при добавлении данных:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }
    }
}
