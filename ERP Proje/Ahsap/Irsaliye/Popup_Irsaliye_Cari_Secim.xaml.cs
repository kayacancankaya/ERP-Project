using Layer_2_Common.Type;
using Layer_Business;
using System;
using System.Linq;
using System.Windows;

namespace Layer_UI.Ahsap.Irsaliye
{
    /// <summary>
    /// Interaction logic for Popup_Irsaliye_Cari_Secim.xaml
    /// </summary>
    public partial class Popup_Irsaliye_Cari_Secim : Window
    {
        private Frm_Irsaliye_Kaydet mainFormInstance;

       
        Variables variables = new();
        string irsaliyeNo = string.Empty;
        public Popup_Irsaliye_Cari_Secim(string irsaliyeNumarasi)
        {

            InitializeComponent();

            DataContext = Resources["cls_cari"];

            irsaliyeNo = irsaliyeNumarasi;

        }

        private void btn_tedarik_cari_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Cls_Cari cls_cari_tedarik_cari = new();

                if (string.IsNullOrEmpty(txt_tedarik_cari_kodu.Text) && string.IsNullOrEmpty(txt_tedarik_cari_adi.Text))
                {
                    MessageBox.Show("Lütfen Cari Bilgisi Giriniz.");
                    return;
                }


                dg_SipariseCariBaglaTedarikCari.ItemsSource = null;
                dg_SipariseCariBaglaTedarikCari.Items.Clear();

                cls_cari_tedarik_cari.SipariseCariBaglaCollection = cls_cari_tedarik_cari.PopulateSipariseCariBaglaTeslimCari(txt_tedarik_cari_kodu.Text, txt_tedarik_cari_adi.Text, "Ahşap");
                dg_SipariseCariBaglaTedarikCari.ItemsSource = cls_cari_tedarik_cari.SipariseCariBaglaCollection;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_tedarikci_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Cls_Cari cls_cari = new();
                variables.Counter = 0;

                // Assuming your DataGrid is bound to a collection of objects of type MyDataGridItem
                foreach (Cls_Cari item in dg_SipariseCariBaglaTedarikCari.Items)
                {
                    // Assuming you have a property in MyDataGridItem that represents the checkbox state
                    if (item.IsChecked)
                    {
                        cls_cari.TedarikciCariKodu = item.TeslimCariKodu;
                        cls_cari.TedarikciCariAdi = item.TeslimCariAdi;
                        variables.Counter++;
                    }
                }

                if (variables.Counter == 0)
                {
                    CRUDmessages.GeneralFailureMessageNoInput(); return;
                }

                if (variables.Counter > 1)
                {
                    CRUDmessages.GeneralFailureMessageCustomMessage("Birden Fazla Cari Seçilemez.");
                    return;
                }

                var openWindows = Application.Current.Windows.OfType<Window>().ToList();

                openWindows[0].Close();//açık sipariş formunu kapatıp yeni instanceı aç.

                Frm_Irsaliye_Kaydet frm = new(irsaliyeNo, cls_cari.TedarikciCariKodu, cls_cari.TedarikciCariAdi);

                frm.Show();
                //frm_Musteri_Secim.Show();
                //openWindows[2].Close();//açık sipariş formunu kapatıp yeni instanceı aç.
                //openWindows[4].Close();//açık sipariş formunu kapatıp yeni instanceı aç.
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
    }
}
