using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Depo.DAT;
using Layer_UI.Methods;
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

namespace Layer_UI.Arge.Mamul_Turetme
{
    /// <summary>
    /// Interaction logic for Frm_Ozellik_Ekle.xaml
    /// </summary>
    public partial class Frm_Ozellik_Ekle : Window
    {

        Cls_Urun urun = new();
        private ObservableCollection<Cls_Urun> urunCollection = new();
        string secilenUrunOzellikTipi = string.Empty;
        Variables variables = new Variables();
        public Frm_Ozellik_Ekle()
        {
            InitializeComponent();
        }
        public Frm_Ozellik_Ekle(string eskiFormdanGelenSecilenOzellikTipi)
        {
            InitializeComponent();

            dg_urun_grubu_liste.ItemsSource = null;
            dg_urun_grubu_liste.Items.Clear();
            dg_model_liste.ItemsSource = null;
            dg_model_liste.Items.Clear();
            dg_satis_sekil_liste.ItemsSource = null;
            dg_satis_sekil_liste.Items.Clear();

            secilenUrunOzellikTipi = eskiFormdanGelenSecilenOzellikTipi;

            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

           foreach(ComboBoxItem item in cbx_ozellik_secim.Items) 
            { 
                if (item.Content.ToString() == secilenUrunOzellikTipi.ToString())
                { cbx_ozellik_secim.SelectedItem = item;break; }
            }

            urunCollection = urun.PopulateOzellikListe(secilenUrunOzellikTipi);

            if (urunCollection == null)
            { CRUDmessages.GeneralFailureMessage("Ürün Kayıtları Listelenirken"); Mouse.OverrideCursor = null; return; }
            if (urunCollection.Count == 0)
            { CRUDmessages.QueryIsEmpty("Ürün Kayıtları"); Mouse.OverrideCursor = null; return; }

            if (secilenUrunOzellikTipi == "Ürün Grup")
            {
                dg_urun_grubu_liste.ItemsSource = urunCollection;
                dg_urun_grubu_liste.Visibility = Visibility.Visible;
                dg_model_liste.Visibility = Visibility.Collapsed;
                dg_satis_sekil_liste.Visibility = Visibility.Collapsed;

            }
            if (secilenUrunOzellikTipi == "Model")
            {
                dg_model_liste.ItemsSource = urunCollection;
                dg_model_liste.Visibility = Visibility.Visible;
                dg_urun_grubu_liste.Visibility = Visibility.Collapsed;
                dg_satis_sekil_liste.Visibility = Visibility.Collapsed;
            }
            if (secilenUrunOzellikTipi == "Satış Şekil")
            {
                dg_satis_sekil_liste.ItemsSource = urunCollection;
                dg_satis_sekil_liste.Visibility = Visibility.Visible;
                dg_urun_grubu_liste.Visibility = Visibility.Collapsed;
                dg_model_liste.Visibility = Visibility.Collapsed;
            }

            Mouse.OverrideCursor = null;
        }
        private void btn_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;

                if (cbx_ozellik_secim.SelectedItem == null)
                { CRUDmessages.NoInput(); return; }

                dg_urun_grubu_liste.ItemsSource = null;
                dg_urun_grubu_liste.Items.Clear();
                dg_model_liste.ItemsSource = null;
                dg_model_liste.Items.Clear();
                dg_satis_sekil_liste.ItemsSource = null;
                dg_satis_sekil_liste.Items.Clear();

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                ComboBoxItem selectedItem = cbx_ozellik_secim.SelectedItem as ComboBoxItem;

                secilenUrunOzellikTipi = selectedItem.Content.ToString();

                urunCollection = urun.PopulateOzellikListe(secilenUrunOzellikTipi);

                if (urunCollection == null)
                { CRUDmessages.GeneralFailureMessage("Ürün Kayıtları Listelenirken"); Mouse.OverrideCursor = null; return; }
                if (urunCollection.Count == 0)
                { CRUDmessages.QueryIsEmpty("Ürün Kayıtları"); Mouse.OverrideCursor = null; return; }

                if(secilenUrunOzellikTipi == "Ürün Grup")
                { 
                    dg_urun_grubu_liste.ItemsSource = urunCollection; 
                    dg_urun_grubu_liste.Visibility = Visibility.Visible; 
                    dg_model_liste.Visibility= Visibility.Collapsed;
                    dg_satis_sekil_liste.Visibility =Visibility.Collapsed;
                
                }
                if(secilenUrunOzellikTipi == "Model")
                {
                    dg_model_liste.ItemsSource = urunCollection;
                    dg_model_liste.Visibility = Visibility.Visible;
                    dg_urun_grubu_liste.Visibility = Visibility.Collapsed;
                    dg_satis_sekil_liste.Visibility = Visibility.Collapsed;
                }
                if(secilenUrunOzellikTipi == "Satış Şekil")
                {
                    dg_satis_sekil_liste.ItemsSource = urunCollection;
                    dg_satis_sekil_liste.Visibility = Visibility.Visible;
                    dg_urun_grubu_liste.Visibility = Visibility.Collapsed;
                    dg_model_liste.Visibility = Visibility.Collapsed;
                }

                Mouse.OverrideCursor = null;
            }

            catch { CRUDmessages.GeneralFailureMessage("Özellik Kayıtları Listelenirken"); Mouse.OverrideCursor = null; }
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
        private void btn_yeni_ekle_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(secilenUrunOzellikTipi))
                    { CRUDmessages.GeneralFailureMessageCustomMessage("Tip Seçiniz"); return; }
                
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor); 

                Popup_Ozellik_Ekle popup = new Popup_Ozellik_Ekle(secilenUrunOzellikTipi);
                popup.ShowDialog();


            }
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Ekleme İşlemi Esnasında");
            }
        }
        private void btn_guncelle(object sender, RoutedEventArgs e)
        {
            try
            {
                Cls_Urun dataItem = UIinteractions.GetDataItemFromButton<Cls_Urun>(sender);

                if (dataItem == null)
                { CRUDmessages.GeneralFailureMessage("Ürün Özelliği Seçilirken");return; }

                if (secilenUrunOzellikTipi == "Ürün Grup")
                {
                    if (string.IsNullOrEmpty(dataItem.UrunGrubuIsim))
                        { CRUDmessages.GeneralFailureMessage("İsim Boş Olamaz"); return; }
                    
                }
                if (secilenUrunOzellikTipi == "Model")
                {
                    if (string.IsNullOrEmpty(dataItem.ModelIsim))
                        { CRUDmessages.GeneralFailureMessage("İsim Boş Olamaz"); return; }
                    
                }
                if (secilenUrunOzellikTipi == "Satış Şekil")
                {
                    if (string.IsNullOrEmpty(dataItem.SatisSekilIsim))
                        { CRUDmessages.GeneralFailureMessage("İsim Boş Olamaz"); return; }
                    
                }

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                variables.Result = urun.UpdateUrunOzellik(dataItem, secilenUrunOzellikTipi);
                if(!variables.Result)
                { CRUDmessages.GeneralFailureMessage("Güncelleme Yapılırken"); Mouse.OverrideCursor = null; return; }

                CRUDmessages.UpdateSuccessMessage("Ürün", 1);
                Frm_Ozellik_Ekle frm = new(secilenUrunOzellikTipi);
                frm.Show();
                this.Close();
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Güncelleme Yapılırken"); Mouse.OverrideCursor = null;
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
                            if (dataGrid.Columns[i].Header.ToString() == "Gönderilecek Mik")
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

        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void NewOzellikSelected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBoxSelectionHasChanged = sender as ComboBox;

            ComboBoxItem selectedItem = comboBoxSelectionHasChanged.SelectedItem as ComboBoxItem;

            secilenUrunOzellikTipi = selectedItem.Content.ToString();
        }
    }
}
