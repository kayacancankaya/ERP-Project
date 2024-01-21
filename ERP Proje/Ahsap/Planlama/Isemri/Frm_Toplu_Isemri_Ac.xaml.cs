using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using Layer_UI.Planlama_Moduler.Simulasyon.Popups;
using Layer_UI.Planlama_Moduler.Simulasyon.Wait;
using Layer_UI.Planlama_Moduler.Simulasyon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Layer_UI.UserControls;

namespace Layer_UI.Ahsap.Planlama.Isemri
{
    /// <summary>
    /// Interaction logic for Frm_Toplu_Isemri_Ac.xaml
    /// </summary>
    public partial class Frm_Toplu_Isemri_Ac : Window
    {
        public Frm_Toplu_Isemri_Ac()
        {
            InitializeComponent();
        }
        public ObservableCollection<Cls_Planlama> SiparisCollection { get; set; }
        public ObservableCollection<Cls_Planlama> IsemriCollection { get; set; }

        Variables variables = new();
        Cls_Planlama plan = new();
        private void btn_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                variables.ErrorMessage = string.Empty;

                Dictionary<string, string> kisitPairs = new Dictionary<string, string>();

                kisitPairs.Add("siparisNo", txt_siparis_no.Text);
                kisitPairs.Add("stokKodu", txt_stok_kodu.Text);
                kisitPairs.Add("stokAdi", txt_stok_adi.Text);
                kisitPairs.Add("cariAdi", txt_cari_adi.Text);

                SiparisCollection = new();

                SiparisCollection = plan.PopulateTopluIsemriAcList(kisitPairs);
                if (!SiparisCollection.Any())
                { CRUDmessages.QueryIsEmpty("Sipariş"); Mouse.OverrideCursor = null; return; }

                dg_SiparisSecim.ItemsSource = SiparisCollection;

                dg_SiparisSecim.Visibility = Visibility.Visible;
                stack_panel_isemri_kaydet.Visibility = Visibility.Visible;

                Mouse.OverrideCursor = null;
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Listeleme İşlemi Yapılırken");
                Mouse.OverrideCursor = null;
            }
        }
        private async void btn_isemri_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            variables.WarningMessage = "İşemirleri Açılıyor.\n Lütfen Bekleyiniz.";
            Frm_Wait waitForm = new(variables.WarningMessage);
            try
            {
                
                IsemriCollection = new();
                foreach (Cls_Planlama item in dg_SiparisSecim.Items) 
                { 
                    if(item.IsChecked)
                    {
                        if (item.GonderilecekIsemriMiktar > item.KalanIsemriMiktar)
                        { CRUDmessages.GeneralFailureMessageCustomMessage("Bildirilecek İşemri Miktarı Kalan İşemri Miktarından Büyük Olamaz.");return; }

                        if(item.GonderilecekIsemriMiktar == 0)
                        {   CRUDmessages.GeneralFailureMessage("Gönderilecek Miktar 0 Olamaz."); return; }

                        if (item.IsemriAciklama == "Lütfen Açıklama Giriniz...")
                            item.IsemriAciklama = string.Empty;

                        IsemriCollection.Add(item);
                    }
                
                }

                btn_isemri.IsEnabled = false;
                
                waitForm.Show();

                variables.ResultInt = await plan.InsertTopluIsemri(IsemriCollection);
                
                switch (variables.ResultInt)
                {
                    case 1:
                        CRUDmessages.InsertSuccessMessage("İşemri",IsemriCollection.Count);
                        variables.Result = true;
                        break;
                    case 2:
                        CRUDmessages.GeneralFailureMessage("İş Emirleri Bildirilirken");
                        variables.Result = plan.DeleteIsemri(IsemriCollection,"Ahşap");
                        break;
                    case -1:
                        CRUDmessages.GeneralFailureMessageCustomMessage("Veri Tabanına Kayıt Yapılırken");
                        variables.Result = plan.DeleteIsemri(IsemriCollection, "Ahşap");
                        break;
                }

                if (!variables.Result)
                 CRUDmessages.GeneralFailureMessageCustomMessage("Kayıt Geri Alma İşlemi Esnasında Hata ile Karşılaşıldı.\n" +
                                                                    "Veri Bütünlüğü Bozulmuş Olabilir.\n Bilgi İşlem Personeline Haber Veriniz.");
                
                waitForm.Close();
                Frm_Toplu_Isemri_Ac _frm = new();
                _frm.Show();
                this.Close();
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("İşemirleri Kaydedilirken");
                waitForm.Close();
            }
        }
        private void ToggleHeaderCheckBox(object sender, RoutedEventArgs e)
        {
            bool headerIsChecked = ((CheckBox)sender).IsChecked ?? false;
            foreach (Cls_Planlama item in dg_SiparisSecim.Items)
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
                            if (dataGrid.Columns[i].Header.ToString() == "Gönderilecek İşemri Miktar" ||
                                dataGrid.Columns[i].Header.ToString() == "İşemri Açıklama" )
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
