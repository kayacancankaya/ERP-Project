using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Layer_UI.Ahsap.Planlama.Isemri
{
    /// <summary>
    /// Interaction logic for Frm_Isemri_Bildir.xaml
    /// </summary>
    public partial class Frm_Isemri_Bildir : Window
    {
        public Frm_Isemri_Bildir()
        {
            InitializeComponent();
            cb_alt_isemri_bildir.IsChecked = true;
        }

        Variables variables = new();
        Cls_Isemri isemri = new();
        ObservableCollection<Cls_Isemri> isemriCollection = new();
        ObservableCollection<Cls_Isemri> bildirimCollection = new();
        Dictionary<string, string> restrictionPairs = new Dictionary<string, string>();
        string queryRestrictions = string.Empty;

        private void btn_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_siparis_no.Text) &&
                    string.IsNullOrWhiteSpace(txt_isemrino.Text) &&
                    string.IsNullOrWhiteSpace(txt_stok_kodu.Text) &&
                    string.IsNullOrWhiteSpace(txt_stok_adi.Text))
                { CRUDmessages.NoInput();return; }


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

                isemriCollection = isemri.PopulateIsemriBildirimList(restrictionPairs);

                if (!isemriCollection.Any())
                {
                    CRUDmessages.GeneralFailureMessage("İşemri Bildirim Listesi Oluşturulurken"); Mouse.OverrideCursor = null;
                }
                if (!isemriCollection.Any())
                { CRUDmessages.GeneralFailureMessageCustomMessage("Listelenecek İşemri Bulunamadı."); Mouse.OverrideCursor = null; return; }

                dg_IsemriSecim.ItemsSource = isemriCollection;
                Mouse.OverrideCursor = null;
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("İşemri Bildirim Listesi Oluşturulurken"); Mouse.OverrideCursor = null;
            }

        }
        private void btn_isemri_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                foreach (Cls_Isemri isemri in dg_IsemriSecim.Items) 
                { 
                    if(isemri.IsChecked == true)
                        bildirimCollection.Add(isemri);
                }

                if (cb_alt_isemri_bildir.IsChecked == true)
                variables.ResultInt = isemri.InsertIsemri(bildirimCollection,true);
                else
                variables.ResultInt = isemri.InsertIsemri(bildirimCollection, false);

                switch (variables.ResultInt) 
                { 
                    case -1:
                        CRUDmessages.GeneralFailureMessageCustomMessage("Sistemsel Problem İle Karşılaşıldı."); Mouse.OverrideCursor = null;
                        return; 
                    case 2:
                        CRUDmessages.GeneralFailureMessageCustomMessage("İşemri Kaydedilirken");Mouse.OverrideCursor = null; 
                        return;
                    case 1:
                        CRUDmessages.InsertSuccessMessage("İşemri", bildirimCollection.Count); Mouse.OverrideCursor = null;
                        break;
                }
                Frm_Isemri_Bildir _frm = new();
                _frm.Show();
                this.Close();
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("İşemirleri Kaydedilirken");Mouse.OverrideCursor = null;
            }
        }
        private void ToggleHeaderCheckBox(object sender, RoutedEventArgs e)
        {
            bool headerIsChecked = ((CheckBox)sender).IsChecked ?? false;
            foreach (Cls_Isemri item in dg_IsemriSecim.Items)
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
                            if (dataGrid.Columns[i].Header.ToString() == "Bildirilecek Miktar" )
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
        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

    }
}