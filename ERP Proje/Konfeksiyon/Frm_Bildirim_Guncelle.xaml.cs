using Layer_2_Common.Type;
using Layer_Business;
using Layer_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using static Layer_Business.Cls_Base;

namespace Layer_UI.Konfeksiyon
{
    /// <summary>
    /// Interaction logic for Frm_Bildirim_Guncelle.xaml
    /// </summary>
    public partial class Frm_Bildirim_Guncelle : Window
    {
        public Frm_Bildirim_Guncelle()
        {
            InitializeComponent();
            lbl_bos_liste.Visibility = Visibility.Collapsed;
        }

        Cls_Isemri cls_isemri = new Cls_Isemri();
        Variables variables = new Variables();
        Dictionary<string, string> kisitlar = new Dictionary<string, string>
        {
        };
        private void listele_click(object sender, RoutedEventArgs e)
        {

            lbl_bos_liste.Visibility = Visibility.Collapsed;

            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

            try
            {
                variables.Query = "select * from vbvKumasBildirimGuncelle";

                kisitlar.Clear();

                if (string.IsNullOrWhiteSpace(txt_takip_no.Text) ||
					string.IsNullOrWhiteSpace(txt_ham_adi.Text) )
                {
                    MessageBox.Show("Lütfen Kısıt Giriniz.");
                    Mouse.OverrideCursor = null;
                    return;
                }

                if (!string.IsNullOrEmpty(txt_ham_adi.Text))
                {
                    variables.Query = variables.Query + " where ham_adi like '%' + @hamAdi + '%'";
                    kisitlar.Add("@hamAdi",txt_ham_adi.Text);
                }

                if (!string.IsNullOrEmpty(txt_takip_no.Text))
                {

                    if(kisitlar.Any())
                        variables.Query = variables.Query + " and takip_no like '%' + @takipNo + '%'";
                    else
                        variables.Query = variables.Query + " where takip_no like '%' + @takipNo + '%'";

                    kisitlar.Add("@takipNo",txt_takip_no.Text);
                }

                if (!string.IsNullOrEmpty(txt_urun_kodu.Text))
                { 
                    if (kisitlar.Any())
                        variables.Query = variables.Query + " and urun_kodu like '%' + @urunKodu + '%'";
                    else
                        variables.Query = variables.Query + " where urun_kodu like '%' + @urunKodu + '%'";

                    kisitlar.Add("@urunKodu",txt_urun_kodu.Text);
                }

                gv_bildirim.ItemsSource = null;
                gv_bildirim.Items.Clear();

                cls_isemri.IsemirleriCollection = cls_isemri.PopulateGridViewWithBildirilenIsemirleriCollection(variables.Query, variables.Yil, kisitlar);

                gv_bildirim.ItemsSource = cls_isemri.IsemirleriCollection;

                Mouse.OverrideCursor = null;

                if (gv_bildirim.Items.Count > 0)
                {
                    btn_bildir.Visibility = Visibility.Visible;
                }
                else
                {
                    lbl_bos_liste.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                MessageBox.Show(ex.Message);
                // Close the modal dialog in case of an error

                Mouse.OverrideCursor = null;

            }
        }
        ObservableCollection<String> hamKoduControlCollection = new();
        private void btn_bildir_click(object sender, RoutedEventArgs e)
        {
            try
            {

            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

				if (string.IsNullOrEmpty(txt_metre.Text)) { MessageBox.Show("Metre Bilgisi Giriniz."); Mouse.OverrideCursor = null; return; }
				if (!EntryControls.IsValidDecimal(txt_metre.Text)) { MessageBox.Show("Metre Formatı Hatalı."); Mouse.OverrideCursor = null; return; }

				variables.Counter = 0;

				if (hamKoduControlCollection.Any())
                    hamKoduControlCollection.Clear();

				foreach (Cls_Isemri item in gv_bildirim.Items)
				{
					if (item.IsChecked == true)
                    {
                        
						if (!hamKoduControlCollection.Contains(item.HAM_KODU))
                        {
							hamKoduControlCollection.Add(item.HAM_KODU);
                            variables.Counter++;
                        }
					}
                   
                }
				if (variables.Counter > 1)
				{ CRUDmessages.GeneralFailureMessageCustomMessage("Bir Adet Farklı Hammadde Bildirimi Yapılabilir."); Mouse.OverrideCursor = null; return; }
				if (variables.Counter == 0)
				{ CRUDmessages.GeneralFailureMessageCustomMessage("Hiç Seçim Yapmadınız."); Mouse.OverrideCursor = null; return; }
				int kullanici_kodu = 78;
            
            variables.Query = string.Empty;

            decimal kumulatifToplamIhtiyac = 0;

            foreach (Cls_Isemri item in gv_bildirim.Items)
            {

                if (item.IsChecked == true)
                {
                    decimal birimToplamIhtiyac = item.BILDIRILEN_MIKTAR * item.BIRIM_HAM_MIKTAR;
                    kumulatifToplamIhtiyac = kumulatifToplamIhtiyac + birimToplamIhtiyac;
                }

            }
			
			string metre = txt_metre.Text.Replace(".", ",");

            decimal katSayi = Convert.ToDecimal(metre) / kumulatifToplamIhtiyac;


            variables.Counter = 0;
            foreach (Cls_Isemri item in gv_bildirim.Items)
            {

                if (item.IsChecked == true)
                {
                    if (item.BILDIRILEN_MIKTAR <= 0 ) { MessageBox.Show("Bildirilecek Miktar 0 Olamaz."); return; };

                    decimal receteDuzeltilmisMiktar = katSayi * item.BIRIM_HAM_MIKTAR;
                    int decimalPlaces = 5; // Number of decimal places you want
                    
                    receteDuzeltilmisMiktar = Math.Round(receteDuzeltilmisMiktar, decimalPlaces);

                        decimal bildirilecekMiktar = receteDuzeltilmisMiktar * item.BILDIRILEN_MIKTAR;

					string receteDuzeltilmisMiktarString = receteDuzeltilmisMiktar.ToString();
                    receteDuzeltilmisMiktarString = receteDuzeltilmisMiktarString.Replace(",", ".");
                    string bildirilecekMiktarString = bildirilecekMiktar.ToString().Replace(",", ".");

                    variables.Query = variables.Query + $" UPDATE TBLISEMRIREC SET MIKTAR='{receteDuzeltilmisMiktarString}' where inckeyno='{item.ReceteID}' " + 
                                                         $" UPDATE TBLSTHAR SET STHAR_GCMIK='{bildirilecekMiktarString}' where sthar_sipnum='{item.ISEMRINO}' and stok_kodu = '{item.HAM_KODU}' ";
					variables.Counter++;
                }
            }

            DataLayer dataLayer = new DataLayer();
            dataLayer.Update_Statement(variables.Query, variables.Yil, "Isemri", variables.Counter);
            Mouse.OverrideCursor = null;
			}
			catch (Exception)
			{
                CRUDmessages.GeneralFailureMessage("Bildirim Yapılırken"); Mouse.OverrideCursor = null;
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
            foreach (Cls_Isemri item in gv_bildirim.Items)
            {
                item.IsChecked = headerIsChecked;
            }
        }
        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Border checkBox)
            {
                if (checkBox.Name == "DGR_Border") return;
                if (checkBox.Child is SelectiveScrollingGrid) return;
              
                // Get the DataContext (Cls_Isemri) associated with the clicked checkbox
                if (checkBox.DataContext is Cls_Isemri item && checkBox.Child is ContentPresenter && checkBox.ActualHeight == 15.098340034484863 && checkBox.ActualWidth == 15.974980354309082)
                {

                    item.IsChecked = !item.IsChecked; // Toggle the IsChecked property
                    e.Handled = true; // Prevent the checkbox click event from bubbling up

                }
            }

        }
    }
}
