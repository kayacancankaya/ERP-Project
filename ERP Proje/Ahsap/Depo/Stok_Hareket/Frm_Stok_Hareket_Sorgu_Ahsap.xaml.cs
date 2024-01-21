using Layer_2_Common.Excels;
using Layer_2_Common.Type;
using Layer_Business;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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

namespace Layer_UI.Ahsap.Depo.Stok_Hareket
{
    /// <summary>
    /// Interaction logic for Frm_Stok_Hareket_Sorgu_Ahsap.xaml
    /// </summary>
    public partial class Frm_Stok_Hareket_Sorgu_Ahsap : Window
    {
        Cls_Depo depo = new();
        Variables variables = new();
        private ObservableCollection<int> depoNoCollection = new();
        private ObservableCollection<string> kod1Collection = new();
        private ObservableCollection<Cls_Depo> depoCollection = new();
        private ObservableCollection<Cls_Depo> stokHareketCollection = new();
        public Frm_Stok_Hareket_Sorgu_Ahsap()
        {
            InitializeComponent();
            depoNoCollection = depo.GetDistinctDepoKodu();
            kod1Collection = depo.GetDistinctKod1();
            cbx_depo_kodu1.ItemsSource = depoNoCollection;
            cbx_depo_kodu2.ItemsSource = depoNoCollection;
            cbx_kod1.ItemsSource = kod1Collection;
        }
        private void btn_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;

                if (
                   string.IsNullOrWhiteSpace(txt_stok_kodu.Text) &&
                   string.IsNullOrWhiteSpace(txt_stok_adi.Text) &&
                    string.IsNullOrWhiteSpace(txt_fisno.Text) &&
                    string.IsNullOrWhiteSpace(txt_aciklama.Text) &&
                    string.IsNullOrWhiteSpace(txt_ekalan.Text)
                    )
                {
                    CRUDmessages.NoInput();
                    return;
                }

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);



                Dictionary<string, string> kisitPairs = new();


                if (!string.IsNullOrEmpty(txt_stok_kodu.Text)) { kisitPairs.Add("stokKodu", txt_stok_kodu.Text); }
                else kisitPairs.Add("stokKodu", null);
                if (!string.IsNullOrEmpty(txt_stok_adi.Text)) { kisitPairs.Add("stokAdi", txt_stok_adi.Text); }
                else kisitPairs.Add("stokAdi", null);
                if (!string.IsNullOrEmpty(txt_fisno.Text)) { kisitPairs.Add("fisNo", txt_fisno.Text); }
                else kisitPairs.Add("fisNo", null);
                if (!string.IsNullOrEmpty(txt_aciklama.Text)) { kisitPairs.Add("aciklama", txt_aciklama.Text); }
                else kisitPairs.Add("aciklama", null);
                if (!string.IsNullOrEmpty(txt_ekalan.Text)) { kisitPairs.Add("ekalan", txt_ekalan.Text); }
                else kisitPairs.Add("ekalan", null);



                if (cbx_depo_kodu1.SelectedItem != null)
                {
                    kisitPairs.Add("depoKodu1", cbx_depo_kodu1.SelectedItem.ToString());
                }
                else kisitPairs.Add("depoKodu1", null);

                if (cbx_depo_kodu2.SelectedItem != null)
                {
                    kisitPairs.Add("depoKodu2", cbx_depo_kodu2.SelectedItem.ToString());
                }
                else kisitPairs.Add("depoKodu2", null);

                if (cbx_hareket_tipi.SelectedItem != null)
                {
                    ComboBoxItem item = cbx_hareket_tipi.SelectedItem as ComboBoxItem;
                    kisitPairs.Add("hareketTipi", item.Content.ToString());
                }
                else kisitPairs.Add("hareketTipi", null);

                if (cbx_kod1.SelectedItem != null)
                {
                    kisitPairs.Add("kod1", cbx_kod1.SelectedItem.ToString());
                }
                else kisitPairs.Add("kod1", null);

                if (dp_baslangic_tarih.SelectedDate != null)
                    kisitPairs.Add("baslangicTarih", dp_baslangic_tarih.SelectedDate?.ToString("yyyy-MM-dd"));
                else kisitPairs.Add("baslangicTarih", null);

                if (dp_bitis_tarih.SelectedDate != null)
                    kisitPairs.Add("bitisTarih", dp_bitis_tarih.SelectedDate?.ToString("yyyy-MM-dd"));
                else kisitPairs.Add("bitisTarih", null);

                if (txt_stok_kodu.Text.Length >= 11)
                {
                    depoCollection = depo.PopulateDepoDurumList(kisitPairs, "Ahşap");
                    if (depoCollection == null)
                    {
                        CRUDmessages.GeneralFailureMessage("Depo Stoğu Listelenirken");
                        Mouse.OverrideCursor = null; return;
                    }
                    if (depoCollection.Select(s => s.StokKodu).FirstOrDefault() == "BOS")
                    {
                        CRUDmessages.QueryIsEmpty("Depo Stoğu");
                        dg_depo_stok_durum.Visibility = Visibility.Hidden;
                        Mouse.OverrideCursor = null; return;
                    }
                }

                stokHareketCollection = depo.PopulateStokHareketList(kisitPairs, "Ahşap");

                if (stokHareketCollection == null &&
                   depoCollection == null)
                {
                    CRUDmessages.QueryIsEmpty("Stok Hareket Kaydı");
                    dg_depo_stok_durum.Visibility = Visibility.Hidden;
                    dg_stok_hareket.Visibility = Visibility.Hidden;
                    btn_excele_aktar.Visibility = Visibility.Hidden;
                    Mouse.OverrideCursor = null; return;
                }

                if (stokHareketCollection == null)
                {
                    CRUDmessages.GeneralFailureMessage("Stok Hareketleri Listelenirken");
                    dg_depo_stok_durum.Visibility = Visibility.Hidden;
                    dg_stok_hareket.Visibility = Visibility.Hidden;
                    btn_excele_aktar.Visibility = Visibility.Hidden;
                    Mouse.OverrideCursor = null; return;
                }
                if (stokHareketCollection.Select(s => s.StokKodu).FirstOrDefault() == "BOS")
                {
                    CRUDmessages.QueryIsEmpty("Stok Hareket Kaydı");
                    dg_depo_stok_durum.Visibility = Visibility.Hidden;
                    dg_stok_hareket.Visibility = Visibility.Hidden;
                    btn_excele_aktar.Visibility = Visibility.Hidden;
                    Mouse.OverrideCursor = null; return;
                }
                if (depoCollection.Count > 0)
                {
                    dg_depo_stok_durum.ItemsSource = depoCollection;
                    dg_depo_stok_durum.Visibility = Visibility.Visible;
                }

                if (stokHareketCollection.Count > 0)
                {
                    dg_stok_hareket.ItemsSource = stokHareketCollection;
                    dg_stok_hareket.Visibility = Visibility.Visible;
                    btn_excele_aktar.Visibility = Visibility.Visible;
                    Mouse.OverrideCursor = null; return;
                }

                Mouse.OverrideCursor = null;
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Stok Hareketleri Listelenirken");
                Mouse.OverrideCursor = null;
            }
        }
        private void btn_excele_aktar_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                ExcelMethodsEPP excelWorks = new ExcelMethodsEPP();
                string filePath = string.Format("C:\\excel-c\\depo\\{0}_{1}", "Stok_Hareket", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string imagePath = "\\\\192.168.1.11\\Netsis\\Images\\vb.png";
                string sheetName = "Stok_Hareket";

                filePath = excelWorks.CreateExcelFile(filePath, sheetName);

                FileInfo fileInfo = new FileInfo(filePath);

                var existingPackage = new ExcelPackage(fileInfo);

                //şablon header kısmı
                excelWorks.SetRowHeight(existingPackage, sheetName, 1, 6);
                excelWorks.SetRowHeight(existingPackage, sheetName, 2, 25);
                excelWorks.SetRowHeight(existingPackage, sheetName, 3, 19);
                excelWorks.SetRowHeight(existingPackage, sheetName, 4, 6);
                excelWorks.SetRowHeight(existingPackage, sheetName, 5, 25);

                excelWorks.SetColumnWidth(existingPackage, sheetName, 1, 1);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 19, 1);

                excelWorks.SetColumnWidth(existingPackage, sheetName, 2, 7);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 3, 25);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 4, 35);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 5, 16);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 6, 6);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 7, 6);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 8, 7);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 9, 11);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 10, 11);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 11, 21);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 12, 9);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 13, 16);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 14, 5);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 15, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 16, 5);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 17, 5);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 18, 5);

                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "A1:XFD1000", "#E6E6E7");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "A1:A22", "#FFFFFF");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "A1:S1", "#FFFFFF");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "S1:S22", "#FFFFFF");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "B2:R2", "#333F4F");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "B3:R3", "#3B495B");

                excelWorks.WriteTextToCell(existingPackage, sheetName, "B2", "VitaBianca Ahşap", "Calibri", 13, "#FFFFFF", true);
                excelWorks.WriteTextToCell(existingPackage, sheetName, "B3", "Depo-Stok Hareket Rapor", "Calibri", 13, "#FFFFFF", true);

                excelWorks.InsertImage(existingPackage, sheetName, imagePath, "logo", 17, 2, 57, 57);

                DataTable dataTable = GetDataFromCollection(stokHareketCollection);
                int rowCount = dataTable.Rows.Count;
                int columnCount = dataTable.Columns.Count;

                excelWorks.ExportDataToExcel(dataTable, existingPackage, sheetName, 6, 2);

                excelWorks.SetRowHeight(existingPackage, sheetName, 6, 38);
                excelWorks.TextWrap(existingPackage, sheetName, "B6:R" + rowCount + 6, true);

                int i = 7;
                while (i < rowCount + 7)
                {
                    excelWorks.SetRowHeight(existingPackage, sheetName, i, 50);
                    i++;
                }

                excelWorks.CreateStyledTable(existingPackage, sheetName, "B6:R6", "#333F4F", rowCount + 1, 6, columnCount + 1, 2, "#D9D9D9", "#FFFFFF", "Stok_Hareket");

                Mouse.OverrideCursor = null;

                MessageBox.Show("Stok Hareket Tablosu Excele Aktarıldı.");
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Excele Aktarılırken");
                Mouse.OverrideCursor = null;
            }
        }
        private static DataTable GetDataFromCollection(ObservableCollection<Cls_Depo> stokHareketCollection)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Stok Kodu");
            dataTable.Columns.Add("Stok Adı");
            dataTable.Columns.Add("Fiş No");
            dataTable.Columns.Add("G-C Mik");
            dataTable.Columns.Add("G-C Kod");
            dataTable.Columns.Add("Depo Kodu");
            dataTable.Columns.Add("Tarih");
            dataTable.Columns.Add("Açıklama");
            dataTable.Columns.Add("Ekalan");
            dataTable.Columns.Add("Takip No");
            dataTable.Columns.Add("Sipariş Numarası");
            dataTable.Columns.Add("Sip Sıra");
            dataTable.Columns.Add("Kod1");
            dataTable.Columns.Add("Har Tür");
            dataTable.Columns.Add("Bel Tip");
            dataTable.Columns.Add("Fat Tip");

            foreach (Cls_Depo item in stokHareketCollection)
            {

                if (item != null)
                {

                    var dataRow = dataTable.NewRow();

                    // Map the properties of cls_Irsaliye to the DataTable columns
                    dataRow["ID"] = item.HareketID;
                    dataRow["Stok Kodu"] = item.StokKodu;
                    dataRow["Stok Adı"] = item.StokAdi;
                    dataRow["Fiş No"] = item.FisNo;
                    dataRow["G-C Mik"] = item.HareketMiktar;
                    dataRow["G-C Kod"] = item.GirisCikisKod;
                    dataRow["Depo Kodu"] = item.DepoKodu;
                    dataRow["Tarih"] = item.HareketTarih;
                    dataRow["Açıklama"] = item.HareketAciklama;
                    dataRow["Ekalan"] = item.Ekalan;
                    dataRow["Takip No"] = item.TakipNo;
                    dataRow["Sipariş Numarası"] = item.SiparisNumarasi;
                    dataRow["Sip Sıra"] = item.SiparisSira;
                    dataRow["Kod1"] = item.Kod1;
                    dataRow["Har Tür"] = item.HareketTipiKodu;
                    dataRow["Bel Tip"] = item.BelgeTipiKodu;
                    dataRow["Fat Tip"] = item.FaturaTipiKodu;

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
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
        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
