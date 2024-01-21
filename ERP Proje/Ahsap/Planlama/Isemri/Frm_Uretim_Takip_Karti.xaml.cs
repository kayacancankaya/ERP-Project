using Layer_2_Common.Excels;
using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.UserControls;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Layer_UI.Ahsap.Planlama.Isemri
{
    /// <summary>
    /// Interaction logic for Frm_Uretim_Takip_Karti.xaml
    /// </summary>
    public partial class Frm_Uretim_Takip_Karti : Window
    {
        public Frm_Uretim_Takip_Karti()
        {
            InitializeComponent();
        }
        public ObservableCollection<Cls_Isemri> SiparisCollection { get; set; }
        public ObservableCollection<Cls_Isemri> IsemriCollection { get; set; }
        public ObservableCollection<Cls_Isemri> TakipKartiCollection { get; set; }


        Variables variables = new();
        Cls_Isemri isemri = new();
        ObservableCollection<Cls_Isemri> isemriCollection = new();
        ObservableCollection<Cls_Isemri> bildirimCollection = new();
        Dictionary<string, string> restrictionPairs = new Dictionary<string, string>();
        private void btn_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_siparis_no.Text) &&
                    string.IsNullOrWhiteSpace(txt_isemrino.Text) &&
                    string.IsNullOrWhiteSpace(txt_stok_kodu.Text) &&
                    string.IsNullOrWhiteSpace(txt_stok_adi.Text) &&
                    string.IsNullOrWhiteSpace(txt_cari_adi.Text))
                { CRUDmessages.NoInput(); return; }


                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                restrictionPairs.Clear();

                if (!string.IsNullOrWhiteSpace(txt_siparis_no.Text))
                    restrictionPairs.Add("@siparisNo", txt_siparis_no.Text);

                if (!string.IsNullOrWhiteSpace(txt_isemrino.Text))
                    restrictionPairs.Add("@isemrino", txt_siparis_no.Text);


                if (!string.IsNullOrWhiteSpace(txt_stok_kodu.Text))
                    restrictionPairs.Add("@stokKodu", txt_stok_kodu.Text);

                if (!string.IsNullOrWhiteSpace(txt_stok_adi.Text))
                    restrictionPairs.Add("@stokAdi", txt_stok_adi.Text);

                if (!string.IsNullOrWhiteSpace(txt_cari_adi.Text))
                    restrictionPairs.Add("@cariAdi", txt_cari_adi.Text);

                isemriCollection = isemri.PopulateIsemriBildirimList(restrictionPairs);

                if (isemriCollection == null)
                {
                    CRUDmessages.GeneralFailureMessage("İşemri Bildirim Listesi Oluşturulurken"); Mouse.OverrideCursor = null;
                }
                if (!isemriCollection.Any())
                { CRUDmessages.GeneralFailureMessageCustomMessage("Listelenecek İşemri Bulunamadı."); Mouse.OverrideCursor = null; return; }

                dg_SiparisSecim.ItemsSource = isemriCollection;
                stack_panel_takip_karti.Visibility = Visibility.Visible;
                Mouse.OverrideCursor = null;
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("İşemri Bildirim Listesi Oluşturulurken"); Mouse.OverrideCursor = null;
            }

        }
        private async void btn_takip_karti_bas_clicked(object sender, RoutedEventArgs e)
        {
            variables.WarningMessage = "Takip Kartları Açılıyor.\n Lütfen Bekleyiniz.";
            Frm_Wait waitForm = new(variables.WarningMessage);
            try
            {

                IsemriCollection = new();
                variables.Counter = 0;
                foreach (Cls_Isemri item in dg_SiparisSecim.Items)
                {
                    if (item.IsChecked)
                    {
                        if (item.BildirilecekIsemriMiktar > item.KALAN_IE_MIKTAR)
                        { CRUDmessages.GeneralFailureMessageCustomMessage("Bildirilecek İşemri Miktarı Kalan İşemri Miktarından Büyük Olamaz."); return; }

                        if (item.BildirilecekIsemriMiktar == 0)
                        { CRUDmessages.GeneralFailureMessage("Gönderilecek Miktar 0 Olamaz."); return; }

                        IsemriCollection.Add(item);
                        variables.Counter ++;
                    }

                }
                if(variables.Counter == 0)
                { CRUDmessages.NoInput(); return; }

                waitForm.Show();

                bildirimCollection = isemri.GetUretimTakipKartiCollection(IsemriCollection);

                if(bildirimCollection == null)
                { CRUDmessages.GeneralFailureMessage("Alt İşemirlerine Ulaşılırken"); return; }

                variables.ResultInt = await TakipKartiBas(bildirimCollection);

                switch (variables.ResultInt)
                {
                    case 1:
                        CRUDmessages.GeneralSuccessMessage("Excele Aktarım ");
                        break;
                    case 2:
                        CRUDmessages.GeneralFailureMessage("Excel İşlemleri Yapılırken");
                        break;
                    case -1:
                        CRUDmessages.GeneralFailureMessageCustomMessage("Veri Tabanına Kayıt Yapılırken");
                        break;
                }

                waitForm.Close();
                Frm_Uretim_Takip_Karti _frm = new();
                _frm.Show();
                this.Close();
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("İşemirleri Kaydedilirken");
                waitForm.Close();
            }
        }

        private async Task<int> TakipKartiBas(ObservableCollection<Cls_Isemri> IsemriCollection)
        {
            try
            {

                ExcelMethodsEPP excelWorks = new ExcelMethodsEPP();

                var distinctSiparisNo = IsemriCollection.Select(s=>s.SIPARIS_NO).Distinct().ToList();
                var distinctSiparisSatirStok = IsemriCollection.Select(s=> new {SiparisNo = s.SIPARIS_NO, SiparisSira = s.SIPARIS_SIRA, StokKodu = s.STOK_KODU }).Distinct().ToList();
                ExcelPackage existingPackage = null;
                
                //çalışma kitabını ve sayfalarını kaydet
                foreach (var siparis in distinctSiparisNo)
                {

                    variables.FilePath = string.Format("C:\\excel-c\\plan\\{0}_{1}", "Uretim_Takip_Karti", siparis);

                    if (!File.Exists(variables.FilePath + "_1.xlsx"))
                    {
                        variables.Counter = 0;
                        foreach (var item in distinctSiparisSatirStok.Where(s=>s.SiparisNo == siparis))
                        {  
                            if(variables.Counter == 0)
                            {
                                variables.SheetName = string.Format("{0}_{1}_{2}",item.SiparisNo,item.SiparisSira,item.StokKodu);
                                variables.FilePath = excelWorks.CreateExcelFile(variables.FilePath, variables.SheetName);

                                FileInfo fileInfo = new FileInfo(variables.FilePath);
                                existingPackage = new ExcelPackage(fileInfo);

                                excelWorks.SetRowHeight(existingPackage, variables.SheetName, 1, 3);
                                excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 15, 1);
                                excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 26, 2);
                                excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 10, 3);
                                excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 26, 4);
                                excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 9, 5);
                                excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 9, 6);

                                variables.Counter ++;
                                continue;
                            }

                            variables.SheetName = string.Format("{0}_{1}_{2}", item.SiparisNo, item.SiparisSira, item.StokKodu);

                            variables.Result = excelWorks.CreateExcelSheetIfDoesntExists(existingPackage,variables.SheetName, variables.FilePath);
                            
                            if (!variables.Result)
                            { return 2; }

                            excelWorks.SetRowHeight(existingPackage, variables.SheetName, 1, 3);
                            excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 15, 1);
                            excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 26, 2);
                            excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 10, 3);
                            excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 26, 4);
                            excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 9, 5);
                            excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 9, 6);

                            variables.Counter++; 
                        }

                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(variables.FilePath + "_1.xlsx");
                        existingPackage = new ExcelPackage(fileInfo);

                        excelWorks.SetRowHeight(existingPackage, variables.SheetName, 1, 3);
                        excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 15, 1);
                        excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 26, 2);
                        excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 10, 3);
                        excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 26, 4);
                        excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 9, 5);
                        excelWorks.SetColumnWidth(existingPackage, variables.SheetName, 9, 6);

                        foreach (var item in distinctSiparisSatirStok.Where(s => s.SiparisNo == siparis))
                        {
                            variables.SheetName = string.Format("{0}_{1}_{2}", item.SiparisNo, item.SiparisSira, item.StokKodu);

                            variables.Result = excelWorks.CreateExcelSheetIfDoesntExists(existingPackage,  variables.FilePath + "_1.xlsx", variables.SheetName);
                            if(!variables.Result)
                            { return 2; }

                        }
                    }
                }

                //her sipariş satır için üretim takip kartı oluştur               
                foreach(var isemri in IsemriCollection)
                {

                   variables.SheetName = string.Format("{0}_{1}_{2}", isemri.SIPARIS_NO, isemri.SIPARIS_SIRA, isemri.STOK_KODU);

                   int firstBlankRow = excelWorks.GetFirstBlankRow(existingPackage,variables.SheetName);

                    if (firstBlankRow == -1 ||
                        firstBlankRow == 0)
                        firstBlankRow = 2;

                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow,"Sipariş No:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 1,"Sipariş Sıra:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 2,"Cari Adı:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 3,"Ürün Kodu:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 4,"Ürün Adı:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 5,"Yarı Mamul Kodu:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 6,"Yarı Mamul Adı:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 7,"İşemrino:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 8,"İşemri Miktar:","Calibri",11,"#000000",true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "A" + firstBlankRow + 9 + ":A" + firstBlankRow + 13,"Boş:","Calibri",11,"#ffffff",false);

                    excelWorks.RightAlignCells(existingPackage, variables.SheetName, "A" + (firstBlankRow + 1) + ":A" + firstBlankRow + 6);

                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow, isemri.SIPARIS_NO, "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 1, isemri.SIPARIS_SIRA.ToString(), "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 2, isemri.CARI_ADI, "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 3, isemri.URUN_KODU, "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 4, isemri.URUN_ADI, "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 5, isemri.STOK_KODU, "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 6, isemri.STOK_ADI, "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 7, isemri.ISEMRINO, "Calibri", 11, "#000000", true);
                    excelWorks.WriteTextToCell(existingPackage, variables.SheetName, "B" + firstBlankRow + 8, isemri.BildirilecekIsemriMiktar.ToString(), "Calibri", 11, "#000000", true);

                    excelWorks.ShrinkToFit(existingPackage, variables.SheetName,"B" + firstBlankRow + ":B" + firstBlankRow + 8,true);



                    //excelWorks.SetRowHeight(existingPackage, sheetName, 5, 50);
                    //excelWorks.SetColumnWidth(existingPackage, sheetName, 1, 1);
                    //excelWorks.WriteTextToCell(existingPackage, sheetName, "B2", "VitaBianca", "Calibri", 13, "#ffffff", true);
                    //excelWorks.WriteTextToCell(existingPackage, sheetName, "B3", "Sevkiyat", "Calibri", 13, "#ffffff", true);

                    //excelWorks.TextWrap(existingPackage, sheetName, "B6:H" + rowCount + 6, true);

                    //int i = 7;
                    //while (i < rowCount + 7)
                    //{
                    //    excelWorks.SetRowHeight(existingPackage, sheetName, i, 50);
                    //    i++;
                    //}


                }

                return 1;
            }
            catch 
            {
               
                return -1;
            }
        }
        private void ToggleHeaderCheckBox(object sender, RoutedEventArgs e)
        {
            bool headerIsChecked = ((CheckBox)sender).IsChecked ?? false;
            foreach (Cls_Isemri item in dg_SiparisSecim.Items)
            {
                item.IsChecked = headerIsChecked;
            }
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
                            if (dataGrid.Columns[i].Header.ToString() == "Gönderilecek İşemri Miktar") 
                            {
                                miktarColumnIndex = i;
                                break;
                            }
                        }

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
        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Border checkBox)
            {
                if (checkBox.Name == "DGR_Border") return;
                if (checkBox.Child is SelectiveScrollingGrid) return;

                // Get the DataContex associated with the clicked checkbox
                if (checkBox.DataContext is Cls_Isemri item && checkBox.Child is ContentPresenter && checkBox.ActualHeight == 15.098340034484863 && checkBox.ActualWidth == 15.974980354309082)
                {
                    item.IsChecked = !item.IsChecked; // Toggle the IsChecked property
                    e.Handled = true; // Prevent the checkbox click event from bubbling up
                }
            }

        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGridCellInfo cellInfo = dg_SiparisSecim.CurrentCell;

            if (cellInfo != null &&
                cellInfo.Column.DisplayIndex == 6)
            {

                var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);


                if (cellContent is TextBox textBox)
                {
                    // Clear the text inside the cell
                    textBox.Text = string.Empty;
                }
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
