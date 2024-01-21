using Layer_2_Common.Type;
using Layer_Business;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
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

namespace Layer_UI.Arge.Yardimci
{
    /// <summary>
    /// Interaction logic for Frm_Koli_Uyarla.xaml
    /// </summary>
    public partial class Frm_Koli_Uyarla : Window
    {
        public Frm_Koli_Uyarla()
        {
            InitializeComponent();
        }
        Variables variables = new ();
        Cls_Arge arge = new ();

        private void btn_recete_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txt_boy.Text) ||
                    string.IsNullOrEmpty(txt_en.Text) ||
                    string.IsNullOrEmpty(txt_yukseklik.Text))
                { CRUDmessages.GeneralFailureMessage("Eksik Bilgi Girdiniz."); return; }

                if (!int.TryParse(txt_en.Text, out int intEnValue))
                { CRUDmessages.GeneralFailureMessageCustomMessage("En Tam Sayı Olmalı."); return; };
                if (!int.TryParse(txt_boy.Text, out int intBoyValue))
                { CRUDmessages.GeneralFailureMessageCustomMessage("Boy Tam Sayı Olmalı."); return; };
                if (!int.TryParse(txt_yukseklik.Text, out int intYukseklikValue))
                { CRUDmessages.GeneralFailureMessageCustomMessage("Yükseklik Tam Sayı Olmalı."); return; };

                arge.Boy = Convert.ToInt32(txt_boy.Text) + (Convert.ToInt32(txt_yukseklik.Text) * 2);
                arge.En = (Convert.ToInt32(txt_en.Text) * 2) + (Convert.ToInt32(txt_yukseklik.Text) * 2);

                    if (arge.En > 2400)
                        { CRUDmessages.GeneralFailureMessageCustomMessage("En Makine Kapasitesinin Üzerinde!"); return; }

                arge.Yukseklik = Convert.ToInt32(txt_yukseklik.Text);
                arge.M2 = (Convert.ToDecimal(arge.Boy) * Convert.ToDecimal(arge.En)) / 1000000;
                               
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                arge.ArgeCollection = arge.PopulateReceteIcinKoliDonusturListControl(Convert.ToInt32(txt_en.Text),
                                                                                  Convert.ToInt32(txt_boy.Text),
                                                                                  Convert.ToInt32(txt_yukseklik.Text));

                if(arge.ArgeCollection == null)
                { CRUDmessages.GeneralFailureMessage("Listeleme İşlemi Esnasında"); Mouse.OverrideCursor = null; return; }
                
                if (arge.ArgeCollection.Select(a=> a.HamKodu).FirstOrDefault() != "Boş Sonuç" )
                { CRUDmessages.GeneralFailureMessageCustomMessage("Eklemek İstediğiniz Ölçülere Ait Ürün Ağacı Bulunmakta!"); Mouse.OverrideCursor = null;return; }

                if (arge.ArgeCollection.Select(a => a.HamKodu).FirstOrDefault() == "Boş Sonuç")
                {
                    arge.ArgeCollection.Clear();

                    arge.YariMamulAdi = "KESILI KARTON C+B DALGA " + txt_boy.Text.PadLeft(4,'0') + "*" + txt_en.Text.PadLeft(4,'0') + "*" + txt_yukseklik.Text.PadLeft(4, '0');

                    arge.YariMamulKodu = arge.GetKoliYariMamulKodu();


                    if (string.IsNullOrEmpty(arge.YariMamulKodu) ||
                        arge.YariMamulKodu == "STRING HATA")
                    { CRUDmessages.GeneralFailureMessage("Yarı Mamul Kodu Alınırken");Mouse.OverrideCursor = null; return; }


                    if (arge.En < 1100)
                    { arge.HamKodu = "HM000011111"; arge.HamAdi = arge.GetHamAdiFromHamKodu(arge.HamKodu); }
                    else if (arge.En < 1200)
                    { arge.HamKodu = "HM000011085"; arge.HamAdi = arge.GetHamAdiFromHamKodu(arge.HamKodu);}
                    else if (arge.En < 1300)
                    { arge.HamKodu = "HM000011112"; arge.HamAdi = arge.GetHamAdiFromHamKodu(arge.HamKodu);}
                    else if (arge.En < 1400)
                    { arge.HamKodu = "HM000011041"; arge.HamAdi = arge.GetHamAdiFromHamKodu(arge.HamKodu);}
                    else if (arge.En < 1500)
                    { arge.HamKodu = "HM000011113"; arge.HamAdi = arge.GetHamAdiFromHamKodu(arge.HamKodu);}
                    else if (arge.En < 2200)
                    { arge.HamKodu = "HM000011114"; arge.HamAdi = arge.GetHamAdiFromHamKodu(arge.HamKodu);}
                    else if (arge.En < 2400)
                    { arge.HamKodu = "HM000011129"; arge.HamAdi = arge.GetHamAdiFromHamKodu(arge.HamKodu);}
                    else
                    { CRUDmessages.GeneralFailureMessage("Ham Kodu Alınırken"); Mouse.OverrideCursor = null; return; }

                    if (arge.HamAdi == "HAMADI HATA")
                    { CRUDmessages.GeneralFailureMessage("Ham Adı Alınırken"); Mouse.OverrideCursor = null; return; }

                    arge.ArgeCollection.Add(arge);
                    dg_StokKarti.ItemsSource = arge.ArgeCollection;
                    stack_panel_recete_kaydet.Visibility = Visibility.Visible;
                }
                Mouse.OverrideCursor = null;
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Stok Bilgileri Listelenirken"); Mouse.OverrideCursor = null; 
            }
        }


        private void btn_stok_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                variables.Counter = 0;

                foreach (Cls_Arge item in dg_StokKarti.Items)
                {
                    if (item.M2 == 0)
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Miktar 0 olamaz."); return; };
                    if (item.M2 < 0)
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Miktar negatif olamaz."); return; };

                    if(item.YariMamulKodu.Length > 35)
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Stok Kodu Uzunluğu 35'ten Büyük Olamaz."); return; };
                    if(item.HamKodu.Length > 35)
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Ham Kodu Uzunluğu 35'ten Büyük Olamaz."); return; };
                    if(item.YariMamulAdi.Length > 100)
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Stok Adı Uzunluğu 100'den Büyük Olamaz."); return; };

                    if (!int.TryParse(txt_en.Text, out int intEnValue))
                    { CRUDmessages.GeneralFailureMessageCustomMessage("En Tam Sayı Olmalı."); return; };
                    if (!int.TryParse(txt_boy.Text, out int intBoyValue))
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Boy Tam Sayı Olmalı."); return; };
                    if (!int.TryParse(txt_yukseklik.Text, out int intYukseklikValue))
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Yükseklik Tam Sayı Olmalı."); return; };

                    variables.Counter++;
                    if (variables.Counter > 1)
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Birden Fazla Reçete Oluşturulamaz."); return; };

                }

                if (variables.Counter == 0)
                { CRUDmessages.GeneralFailureMessageCustomMessage("Seçim Yapmadınız."); return; };

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                if (arge.ArgeCollection == null)
                { CRUDmessages.GeneralFailureMessage("Yeniden Listeleme Yapınız."); Mouse.OverrideCursor = null; return; }

                if (arge.ArgeCollection.Count > 1)
                { CRUDmessages.GeneralFailureMessageCustomMessage("Birden Fazla Stok Kaydedilemez.\nYeniden Listeleme Yapınız."); Mouse.OverrideCursor = null; return; }

                if (arge.IfStokKoduExists(arge.StokKodu,"Vita"))
                { CRUDmessages.GeneralFailureMessageCustomMessage("Eklemek İstediğiniz Ölçülere Ait Ürün Ağacı Bulunmakta!\n Yeniden Listeleme Yapınız."); Mouse.OverrideCursor = null; return; }

                foreach (Cls_Arge item in arge.ArgeCollection)
                {
                    item.En = Convert.ToInt32(txt_en.Text);
                    item.Boy = Convert.ToInt32(txt_boy.Text);
                    item.Yukseklik = Convert.ToInt32(txt_yukseklik.Text);
                    
                }

                variables.Result = arge.InsertStokKartiveReceteKoli(arge.ArgeCollection);
                if (!variables.Result)
                    { CRUDmessages.GeneralFailureMessage("Reçete Oluşturulurken"); Mouse.OverrideCursor = null; return; }

                CRUDmessages.InsertSuccessMessage("Stok Kartı ve Reçete", 1);
                Mouse.OverrideCursor = null;
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Reçete Oluşturulurken");Mouse.OverrideCursor = null;
            }
        }
        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        bool selectMiktarColumn = false;
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

    }
}
