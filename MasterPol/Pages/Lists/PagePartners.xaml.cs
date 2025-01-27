using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MasterPol.Pages.Lists
{
    /// <summary>
    /// Логика взаимодействия для PagePartners.xaml
    /// </summary>
    public partial class PagePartners : Page
    {
        public PagePartners()
        {
            InitializeComponent();
            UpdateUiBasedOnUserRole();
            List<partners> p = AppConnect.model0db.partners.ToList();

            if (p.Count > 0)
            {
                tbCounter.Text = "Найдено " + p.Count + " партнёров"; 
            }
            listPartners.ItemsSource = p;
        }

        partners[] PartnerSearch()
        {
            var partner = AppConnect.model0db.partners.ToList();

            if (tbSearch != null)
            {
                partner = partner.Where(p => p.p_name.ToLowerInvariant().Contains(tbSearch.Text.ToLowerInvariant()) ||
                    Regex.Replace(p.phone, @"[+\-\s()]", "").Contains(Regex.Replace(tbSearch.Text, @"[+\-\s()]", ""))).ToList();
            }

            if (cbFilter != null && cbFilter.SelectedIndex > 0)
            {
                switch (cbFilter.SelectedIndex)
                {
                    case 1:
                        partner = partner.Where(p => p.id_pat == 1).ToList();
                        break;
                    case 2:
                        partner = partner.Where(p => p.id_pat == 2).ToList();
                        break;
                    case 3:
                        partner = partner.Where(p => p.id_pat == 3).ToList();
                        break;
                    case 4:
                        partner = partner.Where(p => p.id_pat == 4).ToList();
                        break;
                }
            }

            if (cbSort != null && cbSort.SelectedIndex > 0)
            {
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        partner = partner.OrderBy(p => p.p_name).ToList();
                        break;
                    case 2:
                        partner = partner.OrderByDescending(p => p.p_name).ToList();
                        break;
                    case 3:
                        partner = partner.OrderBy(p => p.rating).ToList();
                        break;
                    case 4:
                        partner = partner.OrderByDescending(p => p.rating).ToList();
                        break;
                }
            }

            return partner.ToArray();
        }

        private void UpdatePartnersList()
        {
            if (listPartners != null)
            {
                var updatedPartners = PartnerSearch();
                if (updatedPartners != null)
                {
                    listPartners.ItemsSource = updatedPartners;
                }
                else
                {
                    MessageBox.Show("Список партнёров пуст!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            UpdatePartnersList();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePartnersList();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePartnersList();
        }

        private void UpdateUiBasedOnUserRole()
        {
            if (CurrentUser.Current != null && CurrentUser.Current.id_ur == 1)
            {
                btnAddPartner.Visibility = Visibility.Visible;
                btnEditPartner.Visibility = Visibility.Visible;
                btnDeletePartner.Visibility = Visibility.Visible;
            }
            else
            {
                btnAddPartner.Visibility = Visibility.Collapsed;
                btnEditPartner.Visibility = Visibility.Collapsed;
                btnDeletePartner.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAddPartner_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new PageAddPartner());
        }

        private void btnEditPartner_Click(object sender, RoutedEventArgs e)
        {
            if (listPartners.SelectedItem == null)
            {
                MessageBox.Show("Выберите партнёра для редактирования.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedPartner = listPartners.SelectedItem as partners;
            if (selectedPartner == null)
            {
                MessageBox.Show("Ошибка при получение данных о партнёре.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AppFrame.frmMain.Navigate(new PageEditPartner(selectedPartner));
        }

        private void btnDeletePartner_Click(object sender, RoutedEventArgs e)
        {
            if (listPartners == null)
            {
                MessageBox.Show("Выберите партнёра для удаления.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedPartner = listPartners.SelectedItem as partners;
            if (selectedPartner == null)
            {
                MessageBox.Show("Ошибка при получении данных о партнёре.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var confirmation = MessageBox.Show($"Вы уверены, что хотите удалить \"{selectedPartner.p_name}\"", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmation == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new MasterPolEntities())
                    {
                        var partnerToDelete = context.partners.FirstOrDefault(p => p.id_p == selectedPartner.id_p);
                        if (partnerToDelete != null)
                        {
                            context.partners.Remove(partnerToDelete);
                            context.SaveChanges();
                        }
                        listPartners.ItemsSource = PartnerSearch();
                        MessageBox.Show("Партнёр успешно удалён.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
