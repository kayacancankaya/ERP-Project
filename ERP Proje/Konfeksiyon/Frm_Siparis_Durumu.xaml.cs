using Layer_2_Common.Type;
using Layer_Business;
using Layer_Data;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Layer_UI.Konfeksiyon
{
    /// <summary>
    /// Interaction logic for Frm_Siparis_Durumu.xaml
    /// </summary>
    public partial class Frm_Siparis_Durumu : Window
    {
        public Frm_Siparis_Durumu()
        {
            InitializeComponent();

        }

        Variables variables = new();
        public void listele_click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            try
            {
                variables.Query = "select * from vbvKonfeksiyonGenelListe where 1=1";

                if (cb_kapali_siparis_gosterme.IsChecked == true)
                {
                    variables.Query = variables.Query + " and sthar_htur='H' ";
                }

                if (cb_uretilenleri_gosterme.IsChecked == true)
                {
                    variables.Query = variables.Query + " and KumasKalan > 0 and KonfeksiyonKalan > 0 ";
                }

                if (txt_siparis_no.Text != "")
                {
                    variables.Query = variables.Query + " and fisno like '%" + txt_siparis_no.Text + "%'";
                }
                if (txt_siparis_satir.Text != "")
                {
                    variables.Query = variables.Query + " and stra_sipkont like '%" + txt_siparis_satir.Text + "%'";
                }
                if (txt_stok_kodu.Text != "")
                {
                    variables.Query = variables.Query + " and UrunKodu like '%" + txt_stok_kodu.Text + "%'";
                }
                if (txt_stok_adi.Text != "")
                {
                    variables.Query = variables.Query + " and UrunAdi like '%" + txt_stok_adi.Text + "%'";
                }
                if (txt_takip_no.Text != "")
                {
                    variables.Query = variables.Query + " and TakipNo like '%" + txt_takip_no.Text + "%'";
                }

                DataTable dataTable = SelectStatement.GetDataTable(variables.Query, variables.Yil);

                dg_genel_durum.ItemsSource = dataTable.DefaultView;

                Mouse.OverrideCursor = null;

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                MessageBox.Show(ex.Message);
                // Close the modal dialog in case of an error

                Mouse.OverrideCursor = null;

            }
        }
        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ToggleColumnVisibility(sender as CheckBox, Visibility.Visible);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleColumnVisibility(sender as CheckBox, Visibility.Collapsed);
        }

        private void ToggleColumnVisibility(CheckBox checkBox, Visibility visibility)
        {
            
            if (checkBox == cb_siparis) // Replace with appropriate CheckBox names
            {
                if (dg_genel_durum != null)
                {
                    DataGridColumn columnToToggle = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Sip No");
                    DataGridColumn columnToToggle2 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Sip Sira");
                    DataGridColumn columnToToggle3 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Sip Durum");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                        columnToToggle3.Visibility = visibility;
                    }
                }
            }
            
            if (checkBox == cb_planlama) // Replace with appropriate CheckBox names
            {
                if (dg_genel_durum != null)
                {
                    DataGridColumn columnToToggle = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "İşemri No");
                    DataGridColumn columnToToggle2 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Takip No");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                    }
                }
            }
            if (checkBox == cb_iskelet) // Replace with appropriate CheckBox names
            {
                if (dg_genel_durum != null)
                {
                    DataGridColumn columnToToggle = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "İskelet Toplam");
                    DataGridColumn columnToToggle2 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "İskelet Kalan");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                    }
                }
            }
            if (checkBox == cb_kumas) // Replace with appropriate CheckBox names
            {
                if (dg_genel_durum != null)
                {
                    DataGridColumn columnToToggle = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Kumaş Toplam");
                    DataGridColumn columnToToggle2 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Kumaş Kalan");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                    }
                }
            }
            if (checkBox == cb_konfeksiyon) // Replace with appropriate CheckBox names
            {
                if (dg_genel_durum != null)
                {
                    DataGridColumn columnToToggle = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Konfeksiyon Toplam");
                    DataGridColumn columnToToggle2 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Konfeksiyon Kalan");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                    }
                }
            }
            if (checkBox == cb_doseme) // Replace with appropriate CheckBox names
            {
                if (dg_genel_durum != null)
                {
                    DataGridColumn columnToToggle = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Döşenmiş Toplam");
                    DataGridColumn columnToToggle2 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Döşenmiş Kalan");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                    }
                }
            }
            if (checkBox == cb_paket) // Replace with appropriate CheckBox names
            {
                if (dg_genel_durum != null)
                {
                    DataGridColumn columnToToggle = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Paket Toplam");
                    DataGridColumn columnToToggle2 = dg_genel_durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Paket Kalan");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                    }
                }
            }
           
        }
    }
}
