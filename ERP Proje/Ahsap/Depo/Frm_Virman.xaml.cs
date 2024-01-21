using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace Layer_UI.Ahsap.Depo
{
    /// <summary>
    /// Interaction logic for Frm_Virman.xaml
    /// </summary>
    public partial class Frm_Virman : Window
    {
        Variables variables = new ();
        Cls_Depo depo = new ();
        ObservableCollection<int> depoNoCollection = new();
        public Frm_Virman()
        {
            InitializeComponent();
            depoNoCollection = depo.GetDistinctDepoKodu();
            cbx_depo_kodu_eski.ItemsSource = depoNoCollection;
            cbx_depo_kodu_yeni.ItemsSource = depoNoCollection;
        }

        private void btn_virman_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                variables.ErrorMessage = string.Empty;
                string dateToRecord = string.Empty;

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                if (string.IsNullOrEmpty(txt_eski_stok_kodu.Text))
                    variables.ErrorMessage += "Eski Stok Kodu Boş Olamaz\n";
                if (string.IsNullOrEmpty(txt_yeni_stok_kodu.Text))
                    variables.ErrorMessage += "Yeni Stok Kodu Boş Olamaz\n";
                if(cbx_depo_kodu_eski.SelectedItem == null)
                    variables.ErrorMessage += "Eski Depo Kodu Boş Olamaz\n";
                if(cbx_depo_kodu_yeni.SelectedItem == null)
                    variables.ErrorMessage += "Yeni Depo Kodu Boş Olamaz\n";
                if(txt_eski_stok_kodu.Text.Length > 19)
                    variables.ErrorMessage += "Eski Stok Koduna 19 Harften Fazla Giriş Yapılamaz\n";
                if(txt_yeni_stok_kodu.Text.Length > 19)
                    variables.ErrorMessage += "Yeni Stok Koduna 19 Harften Fazla Giriş Yapılamaz\n";
                if(txt_eski_stok_kodu.Text.Length < 11)
                    variables.ErrorMessage += "Eski Stok Koduna 11 Rakamdan Az Giriş Yapılamaz\n";
                if(txt_yeni_stok_kodu.Text.Length < 11)
                    variables.ErrorMessage += "Yeni Stok Koduna 11 Rakamdan Az Giriş Yapılamaz\n";
                if(string.IsNullOrEmpty(txt_eski_miktar.Text))
                    variables.ErrorMessage += "Eski Miktar Boş Olamaz\n";
                if(string.IsNullOrEmpty(txt_yeni_miktar.Text))
                    variables.ErrorMessage += "Yeni Miktar Boş Olamaz\n";

                char dot = '.';
                if (txt_eski_miktar.Text.Count(d => d == dot) > 1)
                    variables.ErrorMessage += "Eski Miktar Format Hatalı\n";
                if (txt_yeni_miktar.Text.Count(d => d == dot) > 1)
                    variables.ErrorMessage += "Yeni Miktar Format Hatalı\n";
                if(txt_eski_miktar.Text[txt_eski_miktar.Text.Length - 1] == '.')
                    variables.ErrorMessage += "Eski Miktar Format Hatalı\n";
                if(txt_yeni_miktar.Text[txt_yeni_miktar.Text.Length - 1] == '.')
                    variables.ErrorMessage += "Yeni Miktar Format Hatalı\n";


                if (!string.IsNullOrEmpty(variables.ErrorMessage))
                { CRUDmessages.GeneralFailureMessageCustomMessage(variables.ErrorMessage); Mouse.OverrideCursor = null; return; }

                if (dp_tarih.SelectedDate == null)
                    dateToRecord = DateTime.Now.ToString("yyyy-MM-dd");
                if (string.IsNullOrEmpty(txt_aciklama.Text))
                    txt_aciklama.Text = string.Empty;
                
                Cls_Depo eskiStokKodu = new();
                Cls_Depo yeniStokKodu = new();

                eskiStokKodu.StokKodu = txt_eski_stok_kodu.Text;
                eskiStokKodu.HareketMiktar = Convert.ToDecimal(txt_eski_miktar.Text, CultureInfo.InvariantCulture);
                eskiStokKodu.DepoKodu = Convert.ToInt32(cbx_depo_kodu_eski.Text);
                eskiStokKodu.Ekalan = txt_aciklama.Text;
                eskiStokKodu.HareketAciklama = txt_yeni_stok_kodu.Text;
                eskiStokKodu.HareketTarih = Convert.ToDateTime(dateToRecord);

                yeniStokKodu.StokKodu = txt_yeni_stok_kodu.Text;
                yeniStokKodu.HareketMiktar = Convert.ToDecimal(txt_yeni_miktar.Text, CultureInfo.InvariantCulture);
                yeniStokKodu.DepoKodu = Convert.ToInt32(cbx_depo_kodu_yeni.Text);
                yeniStokKodu.HareketAciklama = txt_eski_stok_kodu.Text;

                variables.ResultInt = depo.InsertVirman(eskiStokKodu,yeniStokKodu);

                if(variables.ResultInt == -1 ||
                   variables.ResultInt == 3)
                { Mouse.OverrideCursor = null; CRUDmessages.GeneralFailureMessage("Virman Kaydedilirken"); return; }
                if(variables.ResultInt == 2)
                { Mouse.OverrideCursor = null; CRUDmessages.GeneralFailureMessage("Virman Bilgileri Alınırken"); return; }
                if (variables.ResultInt == 1)
                { Mouse.OverrideCursor = null; CRUDmessages.InsertSuccessMessage("Stok Hareketi",2); return; }

            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Virman Kaydedilirken");
            }
        }

        private void btn_sifirla_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Button clickedButton = (Button)sender;


                if (clickedButton.Parent is StackPanel stackPanel)
                {
                    foreach (UIElement element in stackPanel.Children)
                    {
                        if (element is ComboBox comboBox)
                        {
                            comboBox.SelectedIndex = -1;
                        }
                        if (element is DatePicker datePicker)
                        {
                            datePicker.SelectedDate = null;
                        }
                    }
                }

            }
            catch
            {
                CRUDmessages.GeneralFailureMessageCustomMessage("Liste Sıfırlanırken");
            }
        }
        private void btn_stok_kodu_getir_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Button clickedButton = (Button)sender;

                string stokKodu = string.Empty;

                Frm_Stok_Karti_Rehberi frm = new();
                var result = frm.ShowDialog();
                if(result == true) 
                {
                    stokKodu = frm.SelectedStokKodu;
                }
                

                if (clickedButton.Parent is StackPanel stackPanel)
                {
                    foreach (UIElement element in stackPanel.Children)
                    {
                        if (element is TextBox textBox)
                        {
                            textBox.Text=stokKodu;
                        }
                    }
                }

            }
            catch
            {
                CRUDmessages.GeneralFailureMessageCustomMessage("Stok Rehberi Açılırken");
            }
        }

        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
