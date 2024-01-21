using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Layer_UI.Ahsap.Irsaliye
{
    /// <summary>
    /// Interaction logic for Frm_Irsaliye_Kaydet.xaml
    /// </summary>
    public partial class Frm_Irsaliye_Kaydet : Window
    {   
        private Cls_Siparis cls_siparis = new();
        private Cls_Cari cari = new();
        private ObservableCollection<Cls_Siparis> irsaliyeCollection = new();
        public Frm_Irsaliye_Kaydet()
        {
            InitializeComponent();
            
            cls_siparis = new ();
            cls_siparis.SiparisCollection = new ObservableCollection<Cls_Siparis>();
        }
        public Frm_Irsaliye_Kaydet(string irsaliyeNumarasi,string cariKodu,string cariAdi)
        {
            InitializeComponent();
           
            cls_siparis = new();
            cls_siparis.SiparisCollection = new ObservableCollection<Cls_Siparis>();
            txt_irsaliye_numarasi.Text = irsaliyeNumarasi;
            txt_tedarikci_cari_kodu.Text = cariKodu;
            txt_tedarikci_cari_adi.Text = cariAdi;  
        }
        private void btn_cari_guncelle_clicked (object sender, EventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
                if(string.IsNullOrWhiteSpace(txt_ornek_irsaliye.Text)) 
                { CRUDmessages.GeneralFailureMessageCustomMessage("Örnek İrsaliye Numarası Giriniz."); return; };

                ObservableCollection<Cls_Cari> tedarikciCariInfo = cari.GetTedarikciCariInfo(txt_ornek_irsaliye.Text, "Ahşap");

                txt_tedarikci_cari_kodu.Text = tedarikciCariInfo.Select(x => x.TedarikciCariKodu).FirstOrDefault() ?? string.Empty;
                txt_tedarikci_cari_adi.Text = tedarikciCariInfo.Select(x => x.TedarikciCariAdi).FirstOrDefault() ?? string.Empty;
                txt_irsaliye_numarasi.Text = tedarikciCariInfo.Select(x => x.IrsaliyeNumarasi).FirstOrDefault() ?? string.Empty;

                if (string.IsNullOrEmpty(txt_tedarikci_cari_kodu.Text))
                    CRUDmessages.GeneralFailureMessageCustomMessage("Cari Bulunamadı.");

                Mouse.OverrideCursor = null;
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Cari Bilgileri Getirilirken");
                Mouse.OverrideCursor = null;
            }
        }
        private void btn_numara_guncelle_clicked(object sender, EventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
                string result = cari.GetIrsaliyeNoFromCari(txt_tedarikci_cari_kodu.Text, "Ahşap");
                if (result == null) 
                {
                    CRUDmessages.GeneralFailureMessage("Numara Getirilirken");
                    Mouse.OverrideCursor = null;
                    return;
                }
                txt_irsaliye_numarasi.Text = result;
                Mouse.OverrideCursor = null;
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Numara Getirilirken");
            }
        }
        private void Show_Tedarikci_Secim_Clicked(object sender, RoutedEventArgs e)
        {
            var cariSecimWindow = new Popup_Irsaliye_Cari_Secim(txt_irsaliye_numarasi.Text);
            // Show the popup window
            cariSecimWindow.ShowDialog();
        }
        private void btn_siparis_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(txt_tedarikci_cari_kodu.Text) || txt_tedarikci_cari_kodu.Text == "Kayıt Yok") variables.ErrorMessage = variables.ErrorMessage + "Teslim Cari Kodu Eksik.\n";
                if (string.IsNullOrEmpty(txt_tedarikci_cari_adi.Text) || txt_tedarikci_cari_adi.Text == "Kayıt Yok") variables.ErrorMessage = variables.ErrorMessage + "Teslim Cari Adı Eksik.\n";
                

                if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); variables.ErrorMessage = string.Empty; Mouse.OverrideCursor = null; return; }

                dg_siparis_secim.ItemsSource = null;
                dg_siparis_secim.Items.Clear();
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

              
                Dictionary<string, string> constraints = new Dictionary<string, string>();

                constraints.Add("cariKodu", txt_tedarikci_cari_kodu.Text);
                constraints.Add("siparisNumarasi", txt_siparis_numarasi.Text);
                constraints.Add("siparisSira", txt_siparis_sira.Text);
                constraints.Add("stokAdi", txt_stok_adi.Text);
                constraints.Add("stokKodu", txt_stok_kodu.Text);
                
                cls_siparis.SiparisCollection = cls_siparis.PopulateTedarikciSiparisListele(constraints, "Ahşap");

                if (cls_siparis.SiparisCollection.Any())
                    dg_siparis_secim.ItemsSource = cls_siparis.SiparisCollection;
                else
                {
                    CRUDmessages.GeneralFailureMessage("Sipariş Bilgileri Listelenirken"); Mouse.OverrideCursor = null;
                    return;
                }
                Mouse.OverrideCursor = null;
            }

            catch { CRUDmessages.GeneralFailureMessage("Sipariş Bilgileri Listelenirken"); Mouse.OverrideCursor = null; }
        }

        Variables variables = new Variables();
        private void add_product_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;

                Button? button = sender as Button;
                if (button == null) { MessageBox.Show("Sipariş Eklerken Hata İle Karşılaşıldı."); return; }

                DataGridRow? row = UIinteractions.FindVisualParent<DataGridRow>(button);

                variables.ErrorMessage = row == null ? "Hata ile Karşılaşıldı" : variables.ErrorMessage;


                // Get the data item associated with the row
                Cls_Siparis? dataItem = row.Item as Cls_Siparis;
               

                variables.ErrorMessage = dataItem == null ? "Hata ile Karşılaşıldı" : variables.ErrorMessage;
                if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); return; };

                variables.ErrorMessage = dataItem.TedarikGirisMiktar == 0 ? "Miktar 0 Olamaz." : variables.ErrorMessage;
                variables.ErrorMessage = dataItem.TedarikGirisMiktar > dataItem.TedarikSiparisBakiye ? "Giriş Miktarı Bakiyeden Büyük Olamaz." : variables.ErrorMessage;
                if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); return; };

                    Cls_Siparis? siparis = new Cls_Siparis
                    {
                        StokKodu = dataItem.StokKodu,
                        StokAdi = dataItem.StokAdi,
                        Fisno = dataItem.Fisno,
                        FisSira = dataItem.FisSira,
                        TedarikGirisMiktar = dataItem.TedarikGirisMiktar,
                        DepoKodu = dataItem.DepoKodu,
                        SiparisTarih = dataItem.SiparisTarih.Substring(0,11),
                        SiparisFiyat = dataItem.SiparisFiyat,
                    
                    };
                    siparis.AssociatedCari.TedarikciCariKodu = dataItem.AssociatedCari.TedarikciCariKodu;
                    variables.ResultInt = EklemeKontrol(siparis, irsaliyeCollection);
                    if(variables.ResultInt == -1)
                    { CRUDmessages.GeneralFailureMessage("Ekleme İşlemi Kontrol Edilirken");Mouse.OverrideCursor = null;return; }
                    if (variables.ResultInt == 2)
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Farklı Sipariş Numarasından Kayıt Yapılamaz."); Mouse.OverrideCursor = null; return; }
                    if (variables.ResultInt == 3)
                    { CRUDmessages.GeneralFailureMessage("Farklı Cari İle Kayıt Yapılamaz."); Mouse.OverrideCursor = null; return; }
                    if (variables.ResultInt == 4)
                    { CRUDmessages.GeneralFailureMessage("Aynı Sipariş Birden Fazla Eklenemez."); Mouse.OverrideCursor = null; return; }

                if (variables.ResultInt == 1)
                        irsaliyeCollection.Add(siparis);
                    
                dg_SiparisEkle.ItemsSource = irsaliyeCollection;

                dg_SiparisEkle.Visibility = Visibility.Visible;
                stack_panel_irsaliye_kaydet.Visibility = Visibility.Visible;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }

        }

        private bool selectMiktarColumn = false;
        private void DataGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectMiktarColumn)
            {
                if (sender is DataGrid dataGrid)
                {
                    DataGridRow? row = ItemsControl.ContainerFromElement(dataGrid, e.OriginalSource as DependencyObject) as DataGridRow;
                    if (row != null)
                    {
                        // Find the index of the "Miktar" column
                        int miktarColumnIndex = -1;
                        for (int i = 0; i < dataGrid.Columns.Count; i++)
                        {
                            if (dataGrid.Columns[i].Header.ToString() == "Miktar")
                            {
                                miktarColumnIndex = i;
                                break;
                            }
                        }

                        // Select the "Miktar" column of the row
                        if (miktarColumnIndex >= 0)
                        {
                            dataGrid.SelectedCells.Clear();
                            DataGridCellInfo cellInfo = new DataGridCellInfo(row.Item, dataGrid.Columns[miktarColumnIndex]);
                            dataGrid.SelectedCells.Add(cellInfo);
                        }
                    }
                }
                selectMiktarColumn = false; // Reset the flag
            }
        }
        private void btn_irsaliye_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            var result = CRUDmessages.InsertOnayMessage();
            if (!result)
                return;

            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            variables.ErrorMessage = string.Empty;
            variables.WarningMessage = string.Empty;
            variables.SuccessMessage = string.Empty;

            if (string.IsNullOrEmpty(txt_irsaliye_numarasi.Text)) variables.ErrorMessage = variables.ErrorMessage + "İrsaliye Numarası Eksik.\n";
            if (string.IsNullOrEmpty(txt_tedarikci_cari_kodu.Text)) variables.ErrorMessage = variables.ErrorMessage + "Cari Kodu Eksik.\n";
          
            if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); variables.ErrorMessage = string.Empty; Mouse.OverrideCursor = null; return; }

            variables.QumulativeSum = 0;
            variables.Counter = 0;
            decimal kdv = 0;
            decimal qumulativeKdv = 0;


            foreach (Cls_Siparis item in dg_SiparisEkle.Items)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.StokKodu)) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Stok Kodu Boş Olamaz.\n", variables.Counter + 1);
                    if (string.IsNullOrEmpty(item.Fisno)) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Fiş Numarası Boş Olamaz.\n", variables.Counter + 1);
                    if (item.FisSira is 0) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Sipariş Sıra 0 olamaz.\n", variables.Counter + 1);
                    if (string.IsNullOrEmpty(item.StokKodu)) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Stok Kodu Eksik.\n", variables.Counter + 1);

                    if (item.TedarikGirisMiktar is 0) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Miktar 0 Olamaz.\n", variables.Counter + 1);

                    if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); variables.ErrorMessage = string.Empty; Mouse.OverrideCursor = null; return; }


                    variables.QumulativeSum = variables.QumulativeSum + (item.SiparisFiyat * item.TedarikGirisMiktar);
                    kdv = cls_siparis.GetKdvOrani(item.Fisno, item.StokKodu, "Ahşap");
                    qumulativeKdv = qumulativeKdv + (item.SiparisFiyat * (kdv / 100) * item.TedarikGirisMiktar);
                    variables.Counter++;

                }

                catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); Mouse.OverrideCursor = null; return; }

            }

                variables.IsTrue = cls_siparis.InsertTedarikSiparisGenel(txt_irsaliye_numarasi.Text, txt_tedarikci_cari_kodu.Text, variables.QumulativeSum,
                                                                         qumulativeKdv, variables.Counter, irsaliyeCollection.OrderBy(x => x.Fisno).Select(x => x.Fisno).FirstOrDefault(),"Ahşap");                
                if(!variables.IsTrue)
                { 
                    KayitGeriAl(irsaliyeCollection.OrderBy(x => x.Fisno).Select(x => x.Fisno).FirstOrDefault(), "Ahşap"); 
                    CRUDmessages.GeneralFailureMessage("İrsaliye Genel Bilgileri Kaydedilirken");
                    Mouse.OverrideCursor = null;
                    return; 
                }
            
                variables.SuccessMessage = "İrsaliye Genel Bilgileri Kaydedildi.\n";
                variables.Counter = 0;
    
                foreach (Cls_Siparis item in dg_SiparisEkle.Items)
                {
                    try
                    {

                        variables.Counter++;

                        if (variables.Counter == 1)
                        { 
                            variables.ResultInt = cls_siparis.InsertTedarikSiparisSatir(item.StokKodu, item.TedarikGirisMiktar, item.SiparisFiyat,
                                                                                 txt_tedarikci_cari_kodu.Text, variables.Counter, txt_irsaliye_numarasi.Text,
                                                                                 item.Fisno, item.FisSira, item.DepoKodu,"Ahşap");
                            if (variables.ResultInt == -1)
                            {
                                KayitGeriAl(irsaliyeCollection.OrderBy(x => x.Fisno).Select(x => x.Fisno).FirstOrDefault(),"Ahşap");
                                CRUDmessages.GeneralFailureMessage("İrsaliye Genel Bilgileri Kaydedilirken");
                                Mouse.OverrideCursor = null;
                                return;
                            }
                        }
                        else
                        { 
                            variables.ResultInt = cls_siparis.InsertTedarikSiparisSatir(item.StokKodu, item.TedarikGirisMiktar, item.SiparisFiyat,
                                                                                 txt_tedarikci_cari_kodu.Text, variables.ResultInt, txt_irsaliye_numarasi.Text,
                                                                                 item.Fisno, item.FisSira, item.DepoKodu);
                            if (variables.ResultInt == -1)
                            {
                                KayitGeriAl(irsaliyeCollection.OrderBy(x => x.Fisno).Select(x => x.Fisno).FirstOrDefault(),"Ahşap");
                                CRUDmessages.GeneralFailureMessage("İrsaliye Genel Bilgileri Kaydedilirken");
                                Mouse.OverrideCursor = null;
                                return;
                            }
                        }
                    }
                catch 
                {
                    KayitGeriAl(irsaliyeCollection.OrderBy(x => x.Fisno).Select(x => x.Fisno).FirstOrDefault(),"Ahşap");
                    CRUDmessages.GeneralFailureMessage("İrsaliye Genel Bilgileri Kaydedilirken");
                    Mouse.OverrideCursor = null;
                    return;
                }
    
                }

                if (variables.ResultInt != -1)
                {
                    variables.SuccessMessage = variables.SuccessMessage + "İrsaliye Satırları Kaydedildi.";
                    MessageBox.Show(variables.SuccessMessage, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information);
                    Mouse.OverrideCursor = null;
                    Frm_Irsaliye_Kaydet frm = new Frm_Irsaliye_Kaydet();
                    frm.Show();
                    this.Close();
                }
    
                else
                {
                    KayitGeriAl(irsaliyeCollection.OrderBy(x => x.Fisno).Select(x => x.Fisno).FirstOrDefault(),"Ahşap");
                    CRUDmessages.GeneralFailureMessage("İrsaliye Genel Bilgileri Kaydedilirken");
                    Mouse.OverrideCursor = null;
                    return;
                }

        }
        private void btn_siparis_sil(object sender, RoutedEventArgs e)
        {
            variables.IsTrue = CRUDmessages.DeleteOnayMessage();

            try
            {
                if (variables.IsTrue)
                {
                    variables.ErrorMessage = string.Empty;
                    Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                    Button? button = sender as Button;
                    if (button == null) { CRUDmessages.GeneralFailureMessage("Sipariş Bilgileri Alınırken"); return; }
                    DataGridRow? row = UIinteractions.FindVisualParent<DataGridRow>(button);

                    if (row == null) { CRUDmessages.GeneralFailureMessage("Sipariş Bilgileri Alınırken"); return; }

                    Cls_Siparis dataItem = row.Item as Cls_Siparis;

                    irsaliyeCollection.Remove(dataItem);

                    dg_SiparisEkle.ItemsSource = irsaliyeCollection; ;

                    dg_SiparisEkle.Items.Refresh();

                    Mouse.OverrideCursor = null;
                }
                else return;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }

        }
        private int EklemeKontrol(Cls_Siparis siparisItem, ObservableCollection<Cls_Siparis> siparisCollection)
        {
            try
            {

                int toplamSiparis = siparisCollection.Count();
                if (toplamSiparis > 0)
                {
                    
                    if (siparisItem.AssociatedCari.TedarikciCariKodu != siparisCollection.Select(x => x.AssociatedCari.TedarikciCariKodu).FirstOrDefault())
                    {
                        return 3;
                    };
                }

                foreach (Cls_Siparis item in siparisCollection)
                {
                    if (item.StokKodu == siparisItem.StokKodu &&
                        item.StokAdi == siparisItem.StokAdi &&
                        item.Fisno == siparisItem.Fisno &&
                        item.FisSira == siparisItem.FisSira)
                        return 4;

                }
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void KayitGeriAl(string fisno,string fabrika)
        {
            try
            {
                if (string.IsNullOrEmpty(fisno))
                    return;

                variables.Result = cls_siparis.IrsaliyeGeriAl(fisno, fabrika);
                if(variables.Result)
                { CRUDmessages.GeneralFailureMessageCustomMessage("İrsaliye İle Alakalı Kayıtlar Geri Alındı."); return; };
                if(!variables.Result)
                {
                    CRUDmessages.GeneralFailureMessageCustomMessage("İrsaliye İle Alakalı Kayıtlar Geri Alınırken Problem İle Karşılaşıldı.\n" +
                                                                   "Veri Bütünlüğü Bozulmuş Olabilir.\n Bilgi İşlem Personeline Bilgi Veriniz.");
                    return;
                };
            }
            catch (Exception ex) 
            {
                CRUDmessages.GeneralFailureMessageCustomMessage(ex.ToString());
            }
        }

    }
}
