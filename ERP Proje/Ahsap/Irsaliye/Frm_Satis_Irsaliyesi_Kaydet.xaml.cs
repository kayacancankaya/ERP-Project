using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Layer_UI.Ahsap.Irsaliye
{
    /// <summary>
    /// Interaction logic for Frm_Irsaliye_Kaydet.xaml
    /// </summary>
    public partial class Frm_Satis_Irsaliyesi_Kaydet : Window
    {   
        private Cls_Siparis cls_siparis = new();
        private Cls_Cari cari = new();
        private Cls_Arge arge = new();
        private ObservableCollection<Cls_Siparis> irsaliyeCollection = new();
        public Frm_Satis_Irsaliyesi_Kaydet()
        {
            InitializeComponent();
            
            cls_siparis = new ();
            cls_siparis.SiparisCollection = new ObservableCollection<Cls_Siparis>();
        }
      
        private void btn_siparis_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                dg_siparis_secim.ItemsSource = null;
                dg_siparis_secim.Items.Clear();
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

              
                Dictionary<string, string> constraints = new Dictionary<string, string>();

                constraints.Add("cariIsim", txt_cari_adi.Text);
                constraints.Add("siparisNumarasi", txt_siparis_numarasi.Text);
                constraints.Add("siparisSira", txt_siparis_sira.Text);
                constraints.Add("stokAdi", txt_stok_adi.Text);
                constraints.Add("stokKodu", txt_stok_kodu.Text);
                
                cls_siparis.SiparisCollection = cls_siparis.PopulateSatisIrsaliyesiSiparisListele(constraints, "Ahşap");

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

                variables.ErrorMessage = dataItem.SiparisBakiye == 0 ? "Miktar 0 Olamaz." : variables.ErrorMessage;
                variables.ErrorMessage = (dataItem.SiparisMiktar - dataItem.SiparisTeslimMiktar) < dataItem.SiparisBakiye ? "Giriş Miktarı Kalan Bakiyeden Büyük Olamaz." : variables.ErrorMessage;
                if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); return; };

                    Cls_Siparis? siparis = new Cls_Siparis
                    {
                        StokKodu = dataItem.StokKodu,
                        StokAdi = dataItem.StokAdi,
                        Fisno = dataItem.Fisno,
                        FisSira = dataItem.FisSira,
                        SiparisBakiye = dataItem.SiparisBakiye,
                        SiparisMiktar = dataItem.SiparisMiktar,
                        SiparisTeslimMiktar = dataItem.SiparisTeslimMiktar,
                        DepoKodu = dataItem.DepoKodu,
                        SiparisTarih = dataItem.SiparisTarih.Substring(0,11),
                        SiparisFiyat = dataItem.SiparisFiyat,
                    
                    };
                    siparis.AssociatedCari.TeslimCariKodu = dataItem.AssociatedCari.TeslimCariKodu;

                    variables.ResultInt = EklemeKontrol(siparis, irsaliyeCollection);
                    if(variables.ResultInt == -1)
                    { CRUDmessages.GeneralFailureMessage("Ekleme İşlemi Kontrol Edilirken");Mouse.OverrideCursor = null;return; }
                    if (variables.ResultInt == 2)
                    { CRUDmessages.GeneralFailureMessage("Farklı Cari İle Kayıt Yapılamaz."); Mouse.OverrideCursor = null; return; }
                    if (variables.ResultInt == 3)
                    { CRUDmessages.GeneralFailureMessage("Aynı Sipariş Birden Fazla Eklenemez."); Mouse.OverrideCursor = null; return; }
                    if (variables.ResultInt == 4)
                    { CRUDmessages.GeneralFailureMessage("Stok Kodu Bulunamadı."); Mouse.OverrideCursor = null; return; }
                    if (variables.ResultInt == 5)
                    { CRUDmessages.GeneralFailureMessage("Siparişlerin Vade, Döviz Tipi ve İhracat Olup Olmadığı Bilgileri Aynı Olmalı."); Mouse.OverrideCursor = null; return; }

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

            variables.QumulativeSum = 0;
            variables.Counter = 0;
            decimal kdv = 0;
            decimal qumulativeKdv = 0;


            foreach (Cls_Siparis item in dg_SiparisEkle.Items)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.StokKodu)) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Stok Kodu Boş Olamaz.\n", variables.Counter + 1);

                    string stokAdi = cls_siparis.GetPrefixForSiparisStokAdi(item.StokKodu, item.Fisno, item.FisSira, "Ahşap") + " " + item.StokAdi;

                    if(string.IsNullOrEmpty(stokAdi))
                        variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Stok Adı Boş Olamaz.\n", variables.Counter + 1);
                    if(stokAdi.Length > 100)
                        variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Stok Adı 100 Karakterden Büyük Olamaz.\n Sipariş Genel Açıklamasının Değiştirilmesi Gerekli\n", variables.Counter + 1);

                    if (string.IsNullOrEmpty(item.Fisno)) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Fiş Numarası Boş Olamaz.\n", variables.Counter + 1);
                    if (item.FisSira is 0) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Sipariş Sıra 0 Olamaz.\n", variables.Counter + 1);

                    variables.ErrorMessage = item.SiparisMiktar - item.SiparisTeslimMiktar > item.SiparisBakiye ? variables.ErrorMessage + "Kayıt Miktarı Bakiyeden Büyük Olamaz." : variables.ErrorMessage;

                    if (item.SiparisBakiye is 0) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Miktar 0 Olamaz.\n", variables.Counter + 1);
                    if (item.SiparisBakiye < 0) variables.ErrorMessage = variables.ErrorMessage + string.Format("{0}. Satır Miktar negatif Olamaz.\n", variables.Counter + 1);

                    if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); variables.ErrorMessage = string.Empty; Mouse.OverrideCursor = null; return; }


                    variables.QumulativeSum = variables.QumulativeSum + (item.SiparisFiyat * item.SiparisBakiye);
                    kdv = cls_siparis.GetKdvOrani(item.Fisno, item.StokKodu, "Ahşap");
                    qumulativeKdv = qumulativeKdv + (item.SiparisFiyat * (kdv / 100) * item.SiparisBakiye);
                    variables.Counter++;

                }

                catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); Mouse.OverrideCursor = null; return; }

            }
                if(variables.Counter == 0) { CRUDmessages.NoInput();Mouse.OverrideCursor = null;return; }

                string fisno = cls_siparis.GetNextFisnoForSatisIrsaliyesi("Ahşap");
                if (string.IsNullOrEmpty(fisno) || fisno.Length != 15)
                { CRUDmessages.GeneralFailureMessage("İrsaliye için Fiş Numarası Alınırken"); Mouse.OverrideCursor = null; return; }

                variables.IsTrue = cls_siparis.InsertSatisIrsaliyesiGenel(fisno, irsaliyeCollection.Select(c=>c.AssociatedCari.TeslimCariKodu).FirstOrDefault(), variables.QumulativeSum,
                                                                         qumulativeKdv, variables.Counter, irsaliyeCollection.Select(s => s.Fisno).FirstOrDefault(),"Ahşap");                
                if(!variables.IsTrue)
                { 
                    KayitGeriAl(fisno, "Ahşap"); 
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
                    
                        //siparişe istinaden stok adını geçici değiştir
                        string prefix = cls_siparis.GetPrefixForSiparisStokAdi(item.StokKodu, item.Fisno, item.FisSira,"Ahşap");
                        if (prefix == null || prefix == "STRING HATA")
                        { CRUDmessages.GeneralFailureMessage("Stok Adı Ön Eki Alınırken"); Mouse.OverrideCursor = null; return; }
                        string stokAdi = arge.GetStokAdi(item.StokKodu, "Ahşap");
                        if (stokAdi == null || stokAdi == "STRING HATA")
                        { CRUDmessages.GeneralFailureMessage("Stok Adı Alınırken"); Mouse.OverrideCursor = null; return; }
                        cls_siparis.StokAdi = prefix + " " + stokAdi;

                        variables.Result = arge.UpdateStokAdi(item.StokKodu, cls_siparis.StokAdi, "Ahşap");
                        if(!variables.Result) 
                        {
                            KayitGeriAl(fisno, "Ahşap");
                            CRUDmessages.GeneralFailureMessage("İrsaliye Genel Bilgileri Kaydedilirken");
                            Mouse.OverrideCursor = null;
                            return;
                        }


                        
                        variables.Result = cls_siparis.InsertSatisIrsaliyesiSatir(item.StokKodu, item.SiparisBakiye, item.SiparisFiyat,
                                                                                    irsaliyeCollection.Select(c => c.AssociatedCari.TeslimCariKodu).FirstOrDefault(), 
                                                                                    variables.Counter, fisno,
                                                                                    item.Fisno, item.FisSira, item.DepoKodu,"Ahşap");
                        if (!variables.Result)
                        {
                            KayitGeriAl(fisno,"Ahşap");
                            arge.UpdateStokAdi(item.StokKodu, stokAdi, "Ahşap");
                            CRUDmessages.GeneralFailureMessage("İrsaliye Satır Bilgileri Kaydedilirken");
                            Mouse.OverrideCursor = null;
                            return;
                        }


                        variables.Result = arge.UpdateStokAdi(item.StokKodu, stokAdi, "Ahşap");
                        if (!variables.Result)
                        {
                            KayitGeriAl(fisno, "Ahşap");
                            CRUDmessages.GeneralFailureMessage("İrsaliye Satır Bilgileri Kaydedilirken");
                            Mouse.OverrideCursor = null;
                            return;
                        }
                   
                    }
                    catch 
                    {
                        KayitGeriAl(fisno,"Ahşap");
                        CRUDmessages.GeneralFailureMessage("İrsaliye Satır Bilgileri Kaydedilirken");
                        Mouse.OverrideCursor = null;
                        return;
                    }
    
                }

                if (variables.Result)
                {
                    variables.SuccessMessage = variables.SuccessMessage + "İrsaliye Satırları Kaydedildi.";
                    MessageBox.Show(variables.SuccessMessage, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information);
                    Mouse.OverrideCursor = null;
                    Frm_Satis_Irsaliyesi_Kaydet frm = new Frm_Satis_Irsaliyesi_Kaydet();
                    frm.Show();
                    this.Close();
                }
    
                else
                {
                    KayitGeriAl(fisno,"Ahşap");
                    CRUDmessages.GeneralFailureMessage("İrsaliye Satır Bilgileri Kaydedilirken");
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
                    
                    if (siparisItem.AssociatedCari.TeslimCariKodu != siparisCollection.Select(x => x.AssociatedCari.TeslimCariKodu).FirstOrDefault())
                    {
                        return 2;
                    };
                    if (siparisItem.Fisno != siparisCollection.Select(f => f.Fisno).LastOrDefault())
                    {
                        variables.Result = cls_siparis.GetFarkliSiparislerIcinVadeVeDovizTipi(siparisItem.Fisno, siparisCollection.Select(f => f.Fisno).LastOrDefault(), "Ahşap");

                        if (!variables.Result)
                            return 5;
                    }
                }

                foreach (Cls_Siparis item in siparisCollection)
                {
                    if (item.StokKodu == siparisItem.StokKodu &&
                        item.StokAdi == siparisItem.StokAdi &&
                        item.Fisno == siparisItem.Fisno &&
                        item.FisSira == siparisItem.FisSira)
                        return 3;

                    variables.Result = arge.IfStokKoduExists(item.StokKodu,"Ahşap");
                    if (!variables.Result)
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

                variables.Result = cls_siparis.SatisIrsaliyesiGeriAl(fisno, fabrika);
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
