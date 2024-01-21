using Layer_2_Common.Excels;
using Layer_2_Common.Type;
using Layer_Business;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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

namespace Layer_UI.Ahsap.Planlama
{
    /// <summary>
    /// Interaction logic for Frm_Genel_Plan.xaml
    /// </summary>
    public partial class Frm_Genel_Plan : Window
    {
        PlanDurum planDurum = new();

        DateValidation dateValidation = new();

        Variables variables = new();

        LoginLogic login = new();

        string updateQuery = string.Empty, strMsg = string.Empty;
        int counter = 0, SevkMiktar = 0;
        bool dateCheck;
        MessageBoxResult resultMsg;

        string queryCount = string.Empty, query = string.Empty;
        public Frm_Genel_Plan()
        {
            InitializeComponent();
            DataContext = planDurum;

            login.Departman = login.GetDepartment();

            try
            {

                dg_Plan_Durum.ItemsSource = null;
                dg_Plan_Durum.Items.Clear();

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                query = "select * from vbvPlanDurum  order by CONVERT(DATE, SiparisTarih, 104) DESC offset 0 rows fetch next 25 rows only";
                queryCount = "select count(*) from vbvPlanDurum";

                planDurum.PopulateMainWindow(query);
                dg_Plan_Durum.ItemsSource = planDurum.PlanDurumCollection;

                planDurum.UpdatePageNumber(queryCount);
                grd_page_numbers.DataContext = planDurum;
                grd_selected_number.DataContext = planDurum;
                grd_Total_Numbers_Count.DataContext = planDurum;
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

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                ContextMenu contextMenu = dataGrid.Resources["dgr_satis"] as ContextMenu;
                if (contextMenu != null &&
                    (login.Departman == "Satis" ||
                     login.Departman == "Bilgi Islem"))
                {
                    dataGrid.ContextMenu = contextMenu;
                }
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
                if (dg_Plan_Durum != null)
                {
                    DataGridColumn columnToToggle = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Sip No");
                    DataGridColumn columnToToggle2 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Sip Sıra");
                    DataGridColumn columnToToggle3 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Sip Durum");
                    DataGridColumn columnToToggle4 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Sip Tarih");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                        columnToToggle3.Visibility = visibility;
                        columnToToggle4.Visibility = visibility;
                    }
                }
            }
            
            if (checkBox == cb_cari) // Replace with appropriate CheckBox names
            {
                if (dg_Plan_Durum != null)
                {
                    DataGridColumn columnToToggle = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Cari Kod");
                    DataGridColumn columnToToggle2 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Cari İsim");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                    }
                }
            }
            if (checkBox == cb_planlama) // Replace with appropriate CheckBox names
            {
                if (dg_Plan_Durum != null)
                {
                    DataGridColumn columnToToggle = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Ref İe");

                    if (columnToToggle != null)
                    {
                        columnToToggle.Visibility = visibility;
                    }
                }
            }
            if (checkBox == cb_urun) // Replace with appropriate CheckBox names
            {
                if (dg_Plan_Durum != null)
                {
                    DataGridColumn columnToToggle1 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Ürün Kodu");
                    DataGridColumn columnToToggle2 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Durum");
                    DataGridColumn columnToToggle3 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Termin Tarih");
                    DataGridColumn columnToToggle4 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Planlanan Tarih");
                    DataGridColumn columnToToggle5 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Aciklama");
                    DataGridColumn columnToToggle6 = dg_Plan_Durum.Columns.FirstOrDefault(c => c.Header.ToString() == "Urun Miktar");

                        columnToToggle1.Visibility = visibility;
                        columnToToggle2.Visibility = visibility;
                        columnToToggle3.Visibility = visibility;
                        columnToToggle4.Visibility = visibility;
                        columnToToggle5.Visibility = visibility;
                        columnToToggle6.Visibility = visibility;
                    
                }
            }
        }

        private void cg_dg_header_checked(object sender, RoutedEventArgs e)
        {
            bool isChecked = ((CheckBox)sender).IsChecked ?? false;

            foreach (var item in dg_Plan_Durum.Items)
            {
                var row = dg_Plan_Durum.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(row);
                    if (checkBox != null)
                    {
                        checkBox.IsChecked = isChecked;
                    }
                }
            }

        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;

                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        private void cmb_selected_record_changed(object sender, SelectionChangedEventArgs e)
        {

            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            dg_Plan_Durum.ItemsSource = null;
            dg_Plan_Durum.Items.Clear();


            if (((ComboBox)sender).SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
            {
                planDurum.SelectedRecord = Convert.ToInt32(selectedItem.Content);
            }
            planDurum.DisplayedRowSelectionChanged();
            dg_Plan_Durum.ItemsSource = planDurum.PlanDurumCollection;


            Mouse.OverrideCursor = null;
        }

        private void btn_paging_clicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            string buttonName = button.Name;

            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            dg_Plan_Durum.ItemsSource = null;
            dg_Plan_Durum.Items.Clear();

            planDurum.PlanDurumCollection = planDurum.PagingButtonsClicked(buttonName);

            grd_page_numbers.DataContext = planDurum;
            grd_next_buttons.DataContext = planDurum;
            grd_prev_buttons.DataContext = planDurum;

            dg_Plan_Durum.ItemsSource = planDurum.PlanDurumCollection;

            Mouse.OverrideCursor = null;

        }

        private void btn_search_clicked(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            dg_Plan_Durum.ItemsSource = null;
            dg_Plan_Durum.Items.Clear();

            string searchBox1 = txt_search_box1.Text;
            string searchBox2 = txt_search_box2.Text;
            string searchBox3 = txt_search_box3.Text;
            string searchBox4 = txt_search_box4.Text;

            planDurum.SearchBoxChanged(searchBox1, searchBox2, searchBox3, searchBox4);
            dg_Plan_Durum.ItemsSource = planDurum.PlanDurumCollection;
            grd_page_numbers.DataContext = planDurum;
            grd_next_buttons.DataContext = planDurum;
            grd_prev_buttons.DataContext = planDurum;


            Mouse.OverrideCursor = null;
        }

        private void btn_download_excel(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                ExcelMethodsEPP excelWorks = new ExcelMethodsEPP();
                string filePath = string.Format("C:\\excel-c\\plan\\{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string imagePath = "\\\\192.168.1.11\\Netsis\\Images\\vb.png";
                string sheetName = "Plan";

                filePath = excelWorks.CreateExcelFile(filePath, sheetName);

                FileInfo fileInfo = new FileInfo(filePath);

                var existingPackage = new ExcelPackage(fileInfo);

                //şablon header kısmı
                excelWorks.SetRowHeight(existingPackage, sheetName, 1, 6);
                excelWorks.SetRowHeight(existingPackage, sheetName, 2, 25);
                excelWorks.SetRowHeight(existingPackage, sheetName, 3, 19);
                excelWorks.SetRowHeight(existingPackage, sheetName, 4, 6);
                excelWorks.SetRowHeight(existingPackage, sheetName, 5, 14);

                excelWorks.SetColumnWidth(existingPackage, sheetName, 1, 1);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 39, 1);

                excelWorks.SetColumnWidth(existingPackage, sheetName, 2, 18);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 4, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 5, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 6, 14);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 7, 14);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 8, 14);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 9, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 10, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 11, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 12, 9);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 13, 9);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 14, 18);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 15, 18);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 17, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 18, 10);
                excelWorks.SetColumnWidth(existingPackage, sheetName, 19, 10);

                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "A1:XFD1000", "#E6E6E7");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "B2:U2", "#333F4F");
                excelWorks.SetCellBackgroundColor(existingPackage, sheetName, "B3:U3", "#3B495B");

                excelWorks.WriteTextToCell(existingPackage, sheetName, "B2", "VitaBianca Ahşap", "Calibri", 13, "#ffffff", true);
                excelWorks.WriteTextToCell(existingPackage, sheetName, "B3", "Plan", "Calibri", 13, "#ffffff", true);

                int rowCount = dg_Plan_Durum.Items.Count;
                int columnCount = dg_Plan_Durum.Columns.Count;
                excelWorks.SetRowHeight(existingPackage, sheetName, 5, 44);


                excelWorks.SetRowHeight(existingPackage, sheetName, 5, 44);
                int i = 6;
                while (i < rowCount + 6)
                {
                    excelWorks.SetRowHeight(existingPackage, sheetName, i, 24);
                    i++;
                }
                excelWorks.TextWrap(existingPackage, sheetName, "B5:U5", true);
                DataTable dataToExport = GetDataFromDataGrid(dg_Plan_Durum);

                excelWorks.ExportDataToExcel(dataToExport, existingPackage, sheetName, 5, 2);
                excelWorks.TextWrap(existingPackage, sheetName, "B5:U5", true);

                excelWorks.CreateStyledTable(existingPackage, sheetName, "B5:U5", "#333F4F", rowCount, 5, columnCount, 2, "#D9D9D9", "#ffffff", "Plan");

                Mouse.OverrideCursor = null;

                MessageBox.Show("Plan Excele Aktarıldı.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString()); Mouse.OverrideCursor = null;
            }

        }

        private static DataTable GetDataFromDataGrid(DataGrid dataGrid)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Sip No");
            dataTable.Columns.Add("Sip Sıra");
            dataTable.Columns.Add("Sip Tarih");
            dataTable.Columns.Add("Sip Durum");
            dataTable.Columns.Add("Sat C Kod");
            dataTable.Columns.Add("Sat C İsim");
            dataTable.Columns.Add("Tes C Kod");
            dataTable.Columns.Add("Tes C İsim");
            dataTable.Columns.Add("Destinasyon");
            dataTable.Columns.Add("PO");
            dataTable.Columns.Add("Plan No");
            dataTable.Columns.Add("Takip No");
            dataTable.Columns.Add("Ref İe");
            dataTable.Columns.Add("Ürün Kodu");
            dataTable.Columns.Add("Durum");
            dataTable.Columns.Add("Termin Tarih");
            dataTable.Columns.Add("Planlanan Tarih");
            dataTable.Columns.Add("Aciklama");
            dataTable.Columns.Add("Urun Miktar");
            dataTable.Columns.Add("Mamul Stok");

            foreach (var item in dataGrid.Items)
            {
                var planItem = item as PlanDurum;
                if (planItem != null)
                {
                    var dataRow = dataTable.NewRow();

                    dataRow["Sip No"] = planItem.SiparisNo;
                    dataRow["Sip Sıra"] = planItem.SiparisSira;
                    dataRow["Sip Tarih"] = planItem.SiparisTarih;
                    dataRow["Sip Durum"] = planItem.SiparisDurum;
                    dataRow["Sat C Kod"] = planItem.SatisCariKod;
                    dataRow["Sat C İsim"] = planItem.SatisCariIsim;
                    dataRow["Tes C Kod"] = planItem.CariKod;
                    dataRow["Tes C İsim"] = planItem.CariIsim;
                    dataRow["Destinasyon"] = planItem.Destinasyon;
                    dataRow["PO"] = planItem.PO;
                    dataRow["Plan No"] = planItem.PlanNo;
                    dataRow["Takip No"] = planItem.TakipNo;
                    dataRow["Ref İe"] = planItem.ReferansIsemri;
                    dataRow["Ürün Kodu"] = planItem.UrunKodu;
                    dataRow["Durum"] = planItem.Durum;
                    dataRow["Termin Tarih"] = planItem.IhtiyacDuyulanTarih;
                    dataRow["Planlanan Tarih"] = planItem.PlanlananTarih;
                    dataRow["Aciklama"] = planItem.PlanlamaAciklama;
                    dataRow["Urun Miktar"] = planItem.UrunMiktar;
                    dataRow["Mamul Stok"] = planItem.MamulStok;

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

    }
}
