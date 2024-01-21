using Layer_2_Common.Type;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System;
using System.Data;
using System.Runtime.InteropServices;
using Layer_2_Common.Excels;
using System.Windows.Controls;
using Layer_Business;
using Layer_UI.Methods;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics.Eventing.Reader;

namespace Layer_UI.Ahsap.Siparis
{
    /// <summary>
    /// Interaction logic for Frm_Siparis_Kaydet.xaml
    /// </summary>
    public partial class Frm_Siparis_Kaydet : Window
    {
        public Frm_Siparis_Kaydet()
        {
            InitializeComponent();
        }

        ExcelMethodsEPP excel = new();
        Variables variables = new ();
        Cls_Siparis siparis = new();
        ObservableCollection<Cls_Siparis> excelCollection = new();
        private void btn_excel_getir_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Dosyaları|*.xlsm;*.xlsx",
                    Title = "Excel Seç"
                };
                
                DataTable dataTable = new DataTable();
                if (openFileDialog.ShowDialog() == true)
                {
                    Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
                    string filePath = openFileDialog.FileName;
                    dataTable = excel.ReadExcelFile(filePath, 0, 2, 10, 5);
                }
                else return;

                if (excelCollection != null)
                    excelCollection.Clear();    

                foreach(DataRow row in dataTable.Rows) 
                {
                    Cls_Siparis siparis = new Cls_Siparis
                    {
                        StokKodu = row["Stok Kodu"].ToString(),
                        SiparisMiktar = Convert.ToInt32(row["Miktar"]),
                        SiparisFiyat = Convert.ToDecimal(row["Fiyat"]),
                        StokKDV = Convert.ToInt32(row["KDV"]),
                        DepoKodu = Convert.ToInt32(row["Depo Kodu"]),
                        Vade = Convert.ToInt32(row["Vade"]),
                        SiparisAciklama = row["Sipariş Genel Açıklama"].ToString(),
                        SiparisSatirAciklama = row["Stok Adı"].ToString(),
                    };
                    siparis.AssociatedCari.TeslimCariKodu = row["Cari Kodu"].ToString();
                    excelCollection.Add(siparis);
                }

                dg_SiparisEkle.ItemsSource = excelCollection;

                txt_pageResult.Text = "Toplam " + dg_SiparisEkle.Items.Count + " adet sipariş listeleniyor.";
                Mouse.OverrideCursor = null;
                CRUDmessages.GeneralSuccessMessage("Aktarım İşlemi");
                
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Excel Listelenirken");
            }
        }
        private void btn_siparis_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                variables.ErrorMessage = string.Empty;
                if (excelCollection.Where(s => s.StokKodu.Length > 35).Any())
                {
                    var hataliStokKodlari = excelCollection.Where(s => s.StokKodu.Length > 35).ToList();
                    
                    foreach(Cls_Siparis item in hataliStokKodlari)
                    {
                        variables.ErrorMessage = variables.ErrorMessage + item + " uzunluğu 35'den büyük olamaz. \n";
                    }
                }
                if (excelCollection.Where(c => c.AssociatedCari.TeslimCariKodu.Length > 15).Any())
                {
                    var hataliCariKodlari = excelCollection.Where(c => c.AssociatedCari.TeslimCariKodu.Length > 15).ToList();
                    
                    foreach(Cls_Siparis item in hataliCariKodlari)
                    {
                        variables.ErrorMessage = variables.ErrorMessage + item + " uzunluğu 15'den büyük olamaz. \n";
                    }
                }
                if (excelCollection.Select(c => c.AssociatedCari.TeslimCariKodu).Distinct().Count() > 1)
                    variables.ErrorMessage = variables.ErrorMessage + " Aynı siparişte birden fazla cari olamaz. \n";
                
                if(!string.IsNullOrEmpty(variables.ErrorMessage)) 
                { CRUDmessages.GeneralFailureMessageCustomMessage(variables.ErrorMessage); return; }

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                string fisno = siparis.SatisSiparisiIcinFisNoUret("Ahşap");

                if (string.IsNullOrEmpty(fisno))
                { CRUDmessages.GeneralFailureMessage("Sipariş Bilgileri Kaydedilirken"); Mouse.OverrideCursor = null; return; }

                variables.Counter = 0;
                variables.QumulativeSum = 0;
                double kumulatifKDV = 0;
                foreach (Cls_Siparis item in excelCollection)
                {
                    variables.QumulativeSum += Convert.ToDecimal(item.SiparisMiktar) * item.SiparisFiyat;
                    kumulatifKDV += (Convert.ToDouble(item.SiparisMiktar) * Convert.ToDouble(item.SiparisFiyat)) / 100 * Convert.ToDouble(item.StokKDV); 
                    variables.Counter++;
                }

                Cls_Siparis siparisGenel = new Cls_Siparis
                {
                    Fisno = fisno,
                    SiparisAciklama = excelCollection.Select(c => c.SiparisAciklama).FirstOrDefault(),
                    SiparisToplamTutar = Convert.ToDouble(variables.QumulativeSum),
                    SiparisToplamKDV = kumulatifKDV,
                    ToplamSiparisKalemi = variables.Counter
                };
                siparisGenel.AssociatedCari.TeslimCariKodu = excelCollection.Select(c => c.AssociatedCari.TeslimCariKodu).FirstOrDefault();

                variables.Result = siparis.InsertSiparisKaydetGenel(siparisGenel, "Ahşap");
                
                if (!variables.Result)
                { 
                    variables.Result = siparis.DeleteSiparisGeriAl(fisno, "Ahşap");

                        if (!variables.Result)
                        {
                            Mouse.OverrideCursor = null;
                            CRUDmessages.GeneralFailureMessage("Sipariş Satır Bilgileri Kaydedilirken \n" +
                                fisno + " sipariş kayıtları geri alınırken hata ile karşılaşıldı. \n Bilgi İşlem Personeline durumu bildiriniz.");
                        return;
                        }

                    Mouse.OverrideCursor = null;
                    CRUDmessages.GeneralFailureMessage("Sipariş Genel Bilgileri Kaydedilirken");
                    return;
                }

                variables.Result = siparis.InsertSiparisKaydetSatir(excelCollection, fisno, "Ahşap");
                    
                if (!variables.Result)
                {
                    variables.Result = siparis.DeleteSiparisGeriAl(fisno, "Ahşap");
                        
                        if(!variables.Result) 
                        {
                            Mouse.OverrideCursor = null;
                            CRUDmessages.GeneralFailureMessage("Sipariş Satır Bilgileri Kaydedilirken \n" +
                                fisno + " sipariş kayıtları geri alınırken hata ile karşılaşıldı. \n Bilgi İşlem Personeline durumu bildiriniz." );
                        }

                    Mouse.OverrideCursor = null;
                    CRUDmessages.GeneralFailureMessage("Sipariş Satır Bilgileri Kaydedilirken");
                }
                Frm_Siparis_Kaydet frm = new();
                frm.Show();
                this.Close();
                Mouse.OverrideCursor = null;
                CRUDmessages.InsertSuccessMessage("Sipariş");
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Sipariş Kaydedilirken"); Mouse.OverrideCursor = null;
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

                    siparis.SiparisCollection.Remove(dataItem);

                    dg_SiparisEkle.ItemsSource = siparis.SiparisCollection;

                    dg_SiparisEkle.Items.Refresh();

                    Mouse.OverrideCursor = null;
                }
                else return;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }

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
