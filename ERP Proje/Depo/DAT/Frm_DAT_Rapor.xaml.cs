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

namespace Layer_UI.Depo.DAT
{
    public partial class Frm_DAT_Rapor : Window
    {
        public Frm_DAT_Rapor()
        {
            InitializeComponent();

            cbx_kod1.ItemsSource = depo.GetDistinctKod1();
            cbx_kod5.ItemsSource = depo.GetDistinctKod5();
        }

        Cls_Depo depo = new();
        Variables variables = new();
        private ObservableCollection<Cls_Depo> depoCollection = new();
        private void btn_dat_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(txt_takip_no.Text) &&
                    (string.IsNullOrEmpty(txt_ham_kodu.Text) &&
                    string.IsNullOrEmpty(txt_ham_adi.Text)))
                { CRUDmessages.NoInput(); return; }


                dg_dat_liste.ItemsSource = null;
                dg_dat_liste.Items.Clear();
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);


                Dictionary<string, string> constraints = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(txt_takip_no.Text))
                    constraints.Add("takipNo", txt_takip_no.Text);
                else
                    constraints.Add("takipNo", null);

                if (!string.IsNullOrWhiteSpace(txt_ham_kodu.Text))
                    constraints.Add("hamKodu", txt_ham_kodu.Text);
                else
                    constraints.Add("hamKodu", null);

                if (!string.IsNullOrWhiteSpace(txt_ham_adi.Text))
                    constraints.Add("hamAdi", txt_ham_adi.Text);
                else
                    constraints.Add("hamAdi", null);

                if (cbx_kod1.SelectedItem != null)
                    constraints.Add("kod1", cbx_kod1.SelectedItem.ToString());
                else
                    constraints.Add("kod1", null);

                if (cbx_kod5.SelectedItem != null)
                    constraints.Add("kod5", cbx_kod5.SelectedItem.ToString());
                else
                    constraints.Add("kod5", null);

                depoCollection = depo.PopulateDATKaydedilecekListesi(constraints, "Vita");

                if (depoCollection == null)
                { CRUDmessages.GeneralFailureMessage("DAT Kayıtları Listelenirken"); Mouse.OverrideCursor = null; return; }
                if (depoCollection.Count == 0)
                { CRUDmessages.QueryIsEmpty("DAT Kayıtları"); Mouse.OverrideCursor = null; return; }

                dg_dat_liste.ItemsSource = depoCollection;

                Mouse.OverrideCursor = null;
            }

            catch { CRUDmessages.GeneralFailureMessage("DAT Kayıtları Listelenirken"); Mouse.OverrideCursor = null; }
        }
        private void btn_excele_aktar_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                ExcelMethodsEPP excelWorks = new ExcelMethodsEPP();
                string filePath = string.Format("C:\\excel-c\\depo\\{0}_{1}", "DAT_Rapor", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string imagePath = "\\\\192.168.1.11\\Netsis\\Images\\vb.png";
                string sheetName = "Dat_Rapor";

                filePath = excelWorks.CreateExcelFile(filePath, sheetName);

                FileInfo fileInfo = new FileInfo(filePath);

                var existingPackage = new ExcelPackage(fileInfo);

                //şablon header kısmı
                excelWorks.SetRowHeight(existingPackage, sheetName, 1, 6);
                excelWorks.SetRowHeight(existingPackage, sheetName, 2, 25);
                excelWorks.SetRowHeight(existingPackage, sheetName, 3, 19);
                excelWorks.SetRowHeight(existingPackage, sheetName, 4, 6);
                excelWorks.SetRowHeight(existingPackage, sheetName, 5, 25);
                //şablon sağdan soldan 1px
                excelWorks.SetColumnWidth(existingPackage, sheetName, 1, 1);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 14, 1);
                //sütun genişlikleri
                excelWorks.SetColumnWidth(existingPackage, sheetName, 2, 12);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 3, 19);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 4, 44);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 5, 12);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 6, 12);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 7, 8);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 8, 8);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 9, 11);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 10, 11);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 11, 11);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 12, 11);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 13, 11);

                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "A1:XFD1000", "#E6E6E7");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "A1:A22", "#FFFFFF");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "A1:N1", "#FFFFFF");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "S1:N22", "#FFFFFF");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "B2:M2", "#333F4F");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "B3:M3", "#3B495B");

                excelWorks.WriteTextToCell(existingPackage, sheetName, "B2", "VitaBianca", "Calibri", 13, "#FFFFFF", true);
                excelWorks.WriteTextToCell(existingPackage, sheetName, "B3", "Depo-DAT Rapor", "Calibri", 13, "#FFFFFF", true);

                excelWorks.InsertImage(existingPackage, sheetName, imagePath, "logo", 13, 2, 57, 57);

                DataTable dataTable = GetDataFromCollection(depoCollection);
                int rowCount = dataTable.Rows.Count;
                int columnCount = dataTable.Columns.Count;

                excelWorks.ExportDataToExcel(dataTable, existingPackage, sheetName, 6, 2);

                excelWorks.SetRowHeight(existingPackage, sheetName, 6, 38);
                excelWorks.TextWrap(existingPackage, sheetName, "B6:M" + rowCount + 6, true);

                int i = 7;
                while (i < rowCount + 7)
                {
                    excelWorks.SetRowHeight(existingPackage, sheetName, i, 50);
                    i++;
                }

                excelWorks.CreateStyledTable(existingPackage, sheetName, "B6:M6", "#333F4F", rowCount + 1, 6, columnCount + 1, 2, "#D9D9D9", "#FFFFFF", "Dat_Rapor");

                Mouse.OverrideCursor = null;

                MessageBox.Show("DAT Tablosu Excele Aktarıldı.");
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

            dataTable.Columns.Add("Takip No");
            dataTable.Columns.Add("Stok Kodu");
            dataTable.Columns.Add("Stok Adı");
            dataTable.Columns.Add("Kod 5");
            dataTable.Columns.Add("Kod 1");
            dataTable.Columns.Add("Çıkış Depo");
            dataTable.Columns.Add("Giriş Depo");
            dataTable.Columns.Add("Çıkış Depo Bakiye");
            dataTable.Columns.Add("Giriş Depo Bakiye");
            dataTable.Columns.Add("Toplam İht Mik");
            dataTable.Columns.Add("Gönd Mik");
            dataTable.Columns.Add("Kalan Miktar");

            foreach (Cls_Depo item in stokHareketCollection)
            {

                if (item != null)
                {

                    var dataRow = dataTable.NewRow();

                    // Map the properties of cls_Irsaliye to the DataTable columns
                    dataRow["Takip No"] = item.TakipNo;
                    dataRow["Stok Kodu"] = item.StokKodu;
                    dataRow["Stok Adı"] = item.StokAdi;
                    dataRow["Kod 5"] = item.Kod5;
                    dataRow["Kod 1"] = item.Kod1;
                    dataRow["Çıkış Depo"] = item.CikisDepoKodu;
                    dataRow["Giriş Depo"] = item.GirisDepoKodu;
                    dataRow["Çıkış Depo Bakiye"] = item.CikisDepoBakiye;
                    dataRow["Giriş Depo Bakiye"] = item.GirisDepoBakiye;
                    dataRow["Toplam İht Mik"] = item.ToplamDATIhtiyacMiktar;
                    dataRow["Gönd Mik"] = item.GonderilenDATMiktar;
                    dataRow["Kalan Miktar"] = item.GonderilecekDATMiktar;

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
