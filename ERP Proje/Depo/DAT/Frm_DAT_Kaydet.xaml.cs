using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Depo.Irsaliye;
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

namespace Layer_UI.Depo.DAT
{
    /// <summary>
    /// Interaction logic for Frm_DAT_Kaydet.xaml
    /// </summary>
    public partial class Frm_DAT_Kaydet : Window
    {
        public Frm_DAT_Kaydet()
        {
            InitializeComponent();

            cbx_kod1.ItemsSource = depo.GetDistinctKod1();
            cbx_kod5.ItemsSource = depo.GetDistinctKod5();
        }

        Cls_Depo depo = new();
        private ObservableCollection<Cls_Depo> depoCollection = new();
        private void btn_dat_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(txt_takip_no.Text) &&
                    string.IsNullOrEmpty(txt_ham_kodu.Text) &&
                    string.IsNullOrEmpty(txt_ham_adi.Text))
                { CRUDmessages.NoInput(); return; }
                    

                dg_dat_liste.ItemsSource = null;
                dg_dat_liste.Items.Clear();
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);


                Dictionary<string, string> constraints = new Dictionary<string, string>();

                if(!string.IsNullOrWhiteSpace(txt_takip_no.Text)) 
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

                depoCollection = depo.PopulateDATKaydedilecekListesi(constraints,"Vita");

                if (depoCollection == null)
                { CRUDmessages.GeneralFailureMessage("DAT Kayıtları Listelenirken"); Mouse.OverrideCursor = null; return; }
                if (depoCollection.Count == 0)
                { CRUDmessages.QueryIsEmpty("DAT Kayıtları"); Mouse.OverrideCursor = null; return; }

                    dg_dat_liste.ItemsSource = depoCollection;
                
                Mouse.OverrideCursor = null;
            }

            catch { CRUDmessages.GeneralFailureMessage("DAT Kayıtları Listelenirken"); Mouse.OverrideCursor = null; }
        }

        Variables variables = new Variables();

        private void btn_dat_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            var result = CRUDmessages.InsertOnayMessage();
            if (!result)
                return;

            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            variables.ErrorMessage = string.Empty;
            variables.WarningMessage = string.Empty;
            variables.SuccessMessage = string.Empty;

            ObservableCollection<Cls_Depo> datCollection = new();
            variables.Counter = 0;
            variables.QumulativeSum = 0;
            foreach (Cls_Depo d in dg_dat_liste.Items)
            {
                if(d.IsChecked)
                {
                    variables.QumulativeSum += d.GonderilecekDATMiktar; 
                    if(string.IsNullOrEmpty(d.TakipNo))
                        variables.ErrorMessage = variables.ErrorMessage +
                             d.StokKodu + "Takip No Boş Olamaz.";
                    if(string.IsNullOrEmpty(d.StokKodu))
                        variables.ErrorMessage = variables.ErrorMessage +
                             d.TakipNo + "Stok Kodu Boş Olamaz.";
                    
                    if (d.CikisDepoBakiye - variables.QumulativeSum < 0) 
                    {
                        datCollection.Clear(); 
                        CRUDmessages.GeneralFailureMessageCustomMessage("Gönderilecek Miktar Depo Bakiyesinden Büyük Olamaz."); 
                        Mouse.OverrideCursor = null; 
                        return;
                       
                    }
             
                    variables.Counter++;
                    datCollection.Add(d);
                }
            }

            if (string.IsNullOrEmpty(variables.ErrorMessage) == false) {  }

            string fisno = depo.GetFisnoForDAT();

            variables.ResultInt = depo.InsertDAT(datCollection,fisno);
            if (variables.ResultInt == 3 ||
                variables.ResultInt == -1)
            {
                KayitGeriAl(fisno);
                CRUDmessages.GeneralFailureMessage("DAT Kaydedilirken");
                Mouse.OverrideCursor = null;
                return;
            }

                CRUDmessages.InsertSuccessMessage("Depo",variables.Counter);
                Mouse.OverrideCursor = null;
                Frm_DAT_Kaydet frm = new Frm_DAT_Kaydet();
                frm.Show();
                this.Close();
            
        }

        private void KayitGeriAl(string fisno)
        {
            try
            {
                if (string.IsNullOrEmpty(fisno))
                    return;

                variables.Result = depo.DATGeriAl(fisno);
                if (variables.Result)
                { CRUDmessages.GeneralFailureMessageCustomMessage("DAT İle Alakalı Kayıtlar Geri Alındı."); return; };
                if (!variables.Result)
                {
                    CRUDmessages.GeneralFailureMessageCustomMessage("DAT İle Alakalı Kayıtlar Geri Alınırken Problem İle Karşılaşıldı.\n" +
                                                                   "Veri Bütünlüğü Bozulmuş Olabilir.\n Bilgi İşlem Personeline Bilgi Veriniz.");
                    return;
                };
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessageCustomMessage("DAT İle Alakalı Kayıtlar Geri Alınırken Problem İle Karşılaşıldı.\n" +
                                                                   "Veri Bütünlüğü Bozulmuş Olabilir.\n Bilgi İşlem Personeline Bilgi Veriniz.");
            }
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

        private void ToggleHeaderCheckBox(object sender, RoutedEventArgs e)
        {
            bool headerIsChecked = ((CheckBox)sender).IsChecked ?? false;
            foreach (Cls_Depo item in dg_dat_liste.Items)
            {
                item.IsChecked = headerIsChecked;
            }
        }
    }
}
