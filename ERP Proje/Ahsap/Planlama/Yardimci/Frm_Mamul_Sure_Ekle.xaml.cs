using Layer_2_Common.Type;
using Layer_Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
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

namespace Layer_UI.Ahsap.Planlama.Yardimci
{
    /// <summary>
    /// Interaction logic for Frm_Mamul_Sure_Ekle.xaml
    /// </summary>
    public partial class Frm_Mamul_Sure_Ekle : Window
    {
        Variables variables = new ();
        Cls_Uretim uretim = new ();
        public ObservableCollection<Cls_Uretim> sureCollection = new();
        public List<string> kod1Collection = new();
        public List<string> kod2Collection = new();
        public List<string> kod3Collection = new();
        public Frm_Mamul_Sure_Ekle()
        {
            InitializeComponent();
            
            kod1Collection = uretim.GetKod1();
            cbx_kod1.ItemsSource = kod1Collection;
            kod2Collection = uretim.GetKod2();
            cbx_kod2.ItemsSource = kod2Collection;
            kod3Collection = uretim.GetKod3();
            cbx_kod3.ItemsSource = kod3Collection;
  
        }

        private void btn_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;

                //if(string.IsNullOrEmpty(txt_stok_adi.Text) &&
                //    string.IsNullOrEmpty(txt_stok_kodu.Text) &&
                //    string.IsNullOrEmpty(txt_kod1.Text) &&
                //    string.IsNullOrEmpty(txt_kod2.Text) &&
                //    string.IsNullOrEmpty(txt_kod3.Text) )
                //{
                //    CRUDmessages.NoInput();return;
                //}
                
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                Dictionary<string, string> kisitPairs = new Dictionary<string, string>();

                kisitPairs.Add("urunKodu", txt_stok_kodu.Text);
                kisitPairs.Add("urunAdi", txt_stok_adi.Text);
                if(cbx_kod1.SelectedIndex != -1)
                    kisitPairs.Add("kod1", cbx_kod1.SelectedItem.ToString());
                else
                    kisitPairs.Add("kod1",string.Empty);
                if (cbx_kod2.SelectedIndex != -1)
                    kisitPairs.Add("kod2", cbx_kod2.SelectedItem.ToString());
                else
                    kisitPairs.Add("kod2", string.Empty);
                if (cbx_kod3.SelectedIndex != -1)
                    kisitPairs.Add("kod3", cbx_kod3.SelectedItem.ToString());
                else
                    kisitPairs.Add("kod3", string.Empty);

                sureCollection = uretim.PopulateSureEkle(kisitPairs);

                if (!sureCollection.Any())
                { CRUDmessages.QueryIsEmpty("Süre"); Mouse.OverrideCursor = null; return; }

                dg_Mamul_Sure_Ekle.ItemsSource = sureCollection;

                dg_Mamul_Sure_Ekle.Visibility = Visibility.Visible;
                stack_panel_sure_kaydet.Visibility = Visibility.Visible;

                Mouse.OverrideCursor = null;
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Listeleme İşlemi Yapılırken");
                Mouse.OverrideCursor = null;
            }
        }
        private void btn_sure_kaydet_clicked(object sender, RoutedEventArgs e)
        {

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

        private bool selectMiktarColumn=false;
        private void DataGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectMiktarColumn)
            {
                if (sender is DataGrid dataGrid)
                {
                    DataGridRow? row = ItemsControl.ContainerFromElement(dataGrid, e.OriginalSource as DependencyObject) as DataGridRow;
                    if (row != null)
                    {
                        int miktarColumnIndex = -1;
                        for (int i = 0; i < dataGrid.Columns.Count; i++)
                        {
                            if (dataGrid.Columns[i].Header.ToString() == "Cila Süre" ||
                                dataGrid.Columns[i].Header.ToString() == "İskelet Süre" ||
                                dataGrid.Columns[i].Header.ToString() == "Montaj Süre" ||
                                dataGrid.Columns[i].Header.ToString() == "Paket Süre" )
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
        private void ToggleHeaderCheckBox(object sender, RoutedEventArgs e)
        {
            bool headerIsChecked = ((CheckBox)sender).IsChecked ?? false;
            foreach (Cls_Uretim item in dg_Mamul_Sure_Ekle.Items)
            {
                item.IsChecked = headerIsChecked;
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
