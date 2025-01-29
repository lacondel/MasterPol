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
    /// Логика взаимодействия для PageEditPartner.xaml
    /// </summary>
    public partial class PageEditPartner : Page
    {
        private partners editingPartner;

        public PageEditPartner(partners selectedPartner)
        {
            InitializeComponent();
            LoadFilterOptions();
            editingPartner = selectedPartner;
            LoadPartnerData();
        }

        private void btnEditPartner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new MasterPolEntities())
                {
                    var selectedTypeItem = editPartnerType.SelectedItem as ComboBoxItem;
                    string selectedTypeName = selectedTypeItem.Content.ToString();
                    var selectedType = AppConnect.model0db.partner_types.FirstOrDefault(p => p.pat_name == selectedTypeName);

                    partners newPartner = new partners
                    {
                        p_name = editPartnerName.Text,
                        id_pat = selectedType.id_pat,
                        director = editDirector.Text,
                        email = editEmail.Text,
                        phone = editPhone.Text,
                        address = editAddress.Text,
                        INN = decimal.TryParse(editINN.Text, out decimal innValue) ? (decimal?)innValue : null,
                        rating = Int32.Parse(editRating.Text),
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

        private void LoadPartnerData()
        {
            editPartnerName.Text = editingPartner.p_name;
            editPartnerType.SelectedItem = AppConnect.model0db.partner_types.FirstOrDefault(p => p.id_pat == editingPartner.id_pat).pat_name;
            editDirector.Text = editingPartner.director;
            editEmail.Text = editingPartner.email;
            editPhone.Text = editingPartner.phone;
            editAddress.Text = editingPartner.address;
            editINN.Text = editingPartner.INN.ToString();
            editRating.Text = editingPartner.rating.ToString();
        }

        private void LoadFilterOptions()
        {
            try
            {
                using (var context = new MasterPolEntities())
                {
                    var partnerTypes = context.partner_types.Select(p => p.pat_name).ToList();

                    editPartnerType.ItemsSource = partnerTypes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фильтров:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
