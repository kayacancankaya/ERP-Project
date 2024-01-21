using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using Layer_UI.Planlama_Moduler.Simulasyon.Popups;
using Layer_UI.Planlama_Moduler.Simulasyon.Wait;
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

namespace Layer_UI.Planlama_Moduler.Simulasyon
{
    /// <summary>
    /// Interaction logic for Frm_Simulasyon_Sunta.xaml
    /// </summary>
    public partial class Frm_Simulasyon_Sunta : Window
    {
        public Frm_Simulasyon_Sunta()
        {
            InitializeComponent();
            SiparisCollection = new();
            PlanAdiCollection = new();
            ObservableCollection<Cls_Planlama> kayitliPlanAdlariCollection = new();
            dg_SiparisSecim.ItemsSource = SiparisCollection;
            dg_SiparisEkle.ItemsSource = PlanAdiCollection;
            
            kayitliPlanAdlariCollection = plan.GetKayitliPlanAdlari("Simülasyon");
            
            if (kayitliPlanAdlariCollection != null)
            {
                foreach (Cls_Planlama planItem in kayitliPlanAdlariCollection)
                {
            
                    ComboBoxItem comboBoxItem = new ComboBoxItem
                    {
                        Content = planItem.PlanAdi,
                    };
                    cx_plan_adi.Items.Add(comboBoxItem);
                }
            }
        }

        public ObservableCollection<Cls_Planlama> SiparisCollection { get; set; }
        public ObservableCollection<Cls_Planlama> PlanAdiCollection { get; set; }

        Variables variables = new();
        Cls_Planlama plan = new();
        private void btn_listele_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                Dictionary<string, string> kisitPairs = new Dictionary<string, string>();

                kisitPairs.Add("urunGrup", txt_urun_grup.Text);
                kisitPairs.Add("model", txt_model.Text);
                kisitPairs.Add("satisSekil", txt_satis_sekil.Text);
                kisitPairs.Add("stokKodu", txt_stok_kodu.Text);
                kisitPairs.Add("stokAdi", txt_stok_adi.Text);

                if(cb_eksik_stok.IsChecked == true)
                    kisitPairs.Add("eksikStokGosterme", "true");
                if(cb_eksik_stok.IsChecked == false)
                    kisitPairs.Add("eksikStokGosterme", "false");

                SiparisCollection = new();

                var newOrders = plan.PopulatePlakaSimulasyonList(kisitPairs);
                if (newOrders == null)
                { CRUDmessages.QueryIsEmpty("Ürün"); Mouse.OverrideCursor = null; return; }

                foreach (var order in newOrders)
                {
                    SiparisCollection.Add(order);
                }

                if (!SiparisCollection.Any())
                {
                    CRUDmessages.QueryIsEmpty("Ürün");
                    Mouse.OverrideCursor = null;
                    return;
                }

                dg_SiparisSecim.ItemsSource = SiparisCollection;

                cx_plan_adi.Items.Clear();


                ObservableCollection<Cls_Planlama> kayitliPlanAdlariCollectionRefersh = new();

                kayitliPlanAdlariCollectionRefersh = plan.GetKayitliPlanAdlari("Simülasyon Sunta");

                if (kayitliPlanAdlariCollectionRefersh != null)
                {
                    foreach (Cls_Planlama planItem in kayitliPlanAdlariCollectionRefersh)
                    {

                        ComboBoxItem comboBoxItem = new ComboBoxItem
                        {
                            Content = planItem.PlanAdi,
                        };
                        cx_plan_adi.Items.Add(comboBoxItem);
                    }
                }

                Mouse.OverrideCursor = null;
            }
            catch (Exception)
            {
                CRUDmessages.GeneralFailureMessage("Listeleme İşlemi Yapılırken");
                Mouse.OverrideCursor = null;
            }
        }
        private void btn_secilenleri_ekle_clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                variables.ErrorMessage = string.Empty;
                
                foreach (Cls_Planlama dataItem in dg_SiparisSecim.ItemsSource)
                {
                    if (dataItem.IsChecked)
                    {
                       
                        if(dataItem.SiparisMiktar == 0)
                        {
                            CRUDmessages.GeneralFailureMessageCustomMessage(dataItem.UrunKodu + " Miktar 0 Olamaz.");
                            return;
                        }

                        int planSira = PlanAdiCollection.Any() ? PlanAdiCollection.Select(item => item.PlanAdiSira).Max() + 1 : 1;

                        Cls_Planlama? planItem = new Cls_Planlama
                        {
                            UrunGrup = dataItem.UrunGrup,
                            Model = dataItem.Model,
                            SatisSekil = dataItem.SatisSekil,
                            UrunAdi = dataItem.UrunAdi,
                            UrunKodu = dataItem.UrunKodu,
                            MakSiparisMiktar = dataItem.MakSiparisMiktar,
                            SiparisMiktar = dataItem.SiparisMiktar,
                            PlanAdiSira = planSira,
                        };

                        if (planSira > 1)
                        {
                            variables.ResultInt = EklemeKontrol(planItem, PlanAdiCollection);

                            if (variables.ResultInt == 1)
                                PlanAdiCollection.Add(planItem); 
                                dg_SiparisEkle.Visibility = Visibility.Visible;
                                stack_panel_sevk_kaydet.Visibility = Visibility.Visible;
                            if (variables.ResultInt == 2)
                            { CRUDmessages.GeneralFailureMessageCustomMessage("Birden Fazla Aynı Ürün Eklenemez."); return; }
                            if (variables.ResultInt == 3)
                            { CRUDmessages.GeneralFailureMessage("Sipariş Eklenmek Üzere Kontrol Edilirken"); return; }

                        }

                        if (planSira == 1)
                        {
                            PlanAdiCollection.Add(planItem);

                            dg_SiparisEkle.Visibility = Visibility.Visible;
                            stack_panel_sevk_kaydet.Visibility = Visibility.Visible;
                        }
                    }

                }

            }
            catch { CRUDmessages.GeneralFailureMessage("Seçilen Sipariş Eklenirken"); }
        }
        private void btn_siparis_sil(object sender, RoutedEventArgs e)
        {
            variables.Result = CRUDmessages.DeleteOnayMessage();

            try
            {
                if (variables.Result)
                {
                    variables.ErrorMessage = string.Empty;
                    Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                    Button? button = sender as Button;
                    if (button == null) { CRUDmessages.GeneralFailureMessage("Ürün Bilgileri Alınırken"); return; }
                    DataGridRow? row = UIinteractions.FindVisualParent<DataGridRow>(button);

                    if (row == null) { CRUDmessages.GeneralFailureMessage("Ürün Bilgileri Alınırken"); return; }

                    Cls_Planlama dataItem = row.Item as Cls_Planlama;

                    PlanAdiCollection.Remove(dataItem);

                    dg_SiparisEkle.ItemsSource = PlanAdiCollection;

                    dg_SiparisEkle.Items.Refresh();

                    Mouse.OverrideCursor = null;
                }
                else
                {
                    Mouse.OverrideCursor = null; return;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); Mouse.OverrideCursor = null; }

        }
        private void btn_plan_adi_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                variables.ErrorMessage = string.Empty;
                variables.WarningMessage = string.Empty;
                variables.Continue = true;
                bool isNew = false;
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                if (!PlanAdiCollection.Any())
                    variables.ErrorMessage = "Eklenecek Ürün Bulunamadı.\n";

                foreach(Cls_Planlama item in PlanAdiCollection)
                {
                    if (item.SiparisMiktar == 0)
                        variables.ErrorMessage = item.UrunKodu + " Miktar 0 Olamaz.";
                }


                if (!string.IsNullOrEmpty(variables.ErrorMessage))
                {
                    CRUDmessages.GeneralFailureMessageCustomMessage(variables.ErrorMessage);
                    Mouse.OverrideCursor = null;
                    return;
                }

                if (cx_plan_adi.SelectedItem != null)
                {
                    ComboBoxItem? selectedItem = cx_plan_adi.SelectedItem as ComboBoxItem;
                    plan.PlanAdi = selectedItem.Content.ToString();
                    variables.WarningMessage = "Kayıtlı Plan Adı Kullanılacak\n";
                    isNew = false;
                }
                else if (!string.IsNullOrWhiteSpace(txt_plan_adi.Text))
                {
                    if (UIinteractions.IsNumeric(txt_plan_adi.Text[0]))
                    {
                        CRUDmessages.GeneralFailureMessage("Plan Adı Rakam İle Başlayamaz.");
                        Mouse.OverrideCursor = null;
                        return;
                    }
                    plan.PlanAdi = txt_plan_adi.Text;
                    variables.WarningMessage = "Yeni Plan Adı Kullanılacak\n";
                    isNew = true;
                }
                else
                    variables.ErrorMessage = "Plan Adı Seçiniz.";

                if (!string.IsNullOrEmpty(variables.ErrorMessage))
                {
                    CRUDmessages.GeneralFailureMessageCustomMessage(variables.ErrorMessage);
                    Mouse.OverrideCursor = null;
                    return;
                }
                if (!string.IsNullOrEmpty(variables.WarningMessage))
                    variables.Continue = CRUDmessages.DoYouWishToContinue(variables.WarningMessage);


                if (variables.Continue)
                    variables.ResultInt = plan.InsertPlanAdi(PlanAdiCollection, plan.PlanAdi, isNew, "Simülasyon Sunta");
                else
                { Mouse.OverrideCursor = null; return; }

                switch (variables.ResultInt)
                {
                    case 1:
                        CRUDmessages.InsertSuccessMessage("Simülasyon");
                        break;
                    case 2:
                        CRUDmessages.GeneralFailureMessage("Plan Sırası Alınırken");
                        break;
                    case 3:
                        CRUDmessages.GeneralFailureMessageCustomMessage("Plan Adı Listede Mevcut Olduğundan \n" +
                                                                            "İşlem Yapılamadı");
                        break;
                    case 4:
                        CRUDmessages.GeneralFailureMessage("Plan Kaydedilirken");
                        break;
                    case 5:
                        CRUDmessages.GeneralFailureMessageCustomMessage("Veri Tabanına Kayıt Yapılırken");
                        break;
                }

                Mouse.OverrideCursor = null;
                Frm_Simulasyon_Moduler _frm = new();
                _frm.Show();
                this.Close();
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Plan Bilgileri Kaydedilirken");
                Mouse.OverrideCursor = null;

            }


        }
        private int EklemeKontrol(Cls_Planlama planItem, ObservableCollection<Cls_Planlama> addedCollection)
        {
            try
            {
                if (addedCollection.Any())
                {
                    foreach (Cls_Planlama item in addedCollection)
                    {
                        if (item.UrunKodu == planItem.UrunKodu )
                            return 2;

                    }

                }

                return 1;
            }
            catch
            {
                return 3;
            }
        }
        private void btn_tum_plan_adlari_sil_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                variables.Result = CRUDmessages.DeleteOnayMessage();
                if (!variables.Result)
                    return;

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
                variables.Result = plan.TruncatePlanAdlari("Simülasyon Sunta");

                if (!variables.Result)
                { CRUDmessages.DeleteFailureMessage("Plan Adları"); Mouse.OverrideCursor = null; return; }

                CRUDmessages.DeleteSuccessMessage("Plan Adları");

                Mouse.OverrideCursor = null;
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Plan Adları Silinirken");
                Mouse.OverrideCursor = null;
            }
        }
        private void btn_plan_adi_goster_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Popup_Plan_Adi_Goster frm = new("Simülasyon Sunta");
                frm.ShowDialog();
            }

            catch
            {
                CRUDmessages.GeneralFailureMessage("Yeni Form Açılırken");
            }
        }
        private void btn_simulasyon(object sender, RoutedEventArgs e)
        {
            try
            {
                Wait_Simulasyon_Calculating toBeClosedForm;

                do
                {
                    toBeClosedForm = UIinteractions.FindSpecificForm<Wait_Simulasyon_Calculating>();

                    if (toBeClosedForm != null)
                    {
                        toBeClosedForm.Close();
                    }
                }
                while (toBeClosedForm != null);


                Wait_Simulasyon_Calculating frm = new Wait_Simulasyon_Calculating("Simülasyon Sunta");
                frm.Show();
            }
            catch
            {
                CRUDmessages.GeneralFailureMessage("Simülasyon Hesaplanırken");
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
                            if (dataGrid.Columns[i].Header.ToString() == "Sevk Miktar" ||
                                dataGrid.Columns[i].Header.ToString() == "Hacim" ||
                                dataGrid.Columns[i].Header.ToString() == "Ağırlık")
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
        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

    }
}

