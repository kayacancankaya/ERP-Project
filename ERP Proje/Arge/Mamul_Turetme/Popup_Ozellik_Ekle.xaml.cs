using Layer_2_Common.Type;
using Layer_Business;
using System;
using System.Collections;
using System.Collections.Generic;
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

namespace Layer_UI.Arge.Mamul_Turetme
{
    /// <summary>
    /// Interaction logic for Popup_Ozellik_Ekle.xaml
    /// </summary>
    public partial class Popup_Ozellik_Ekle : Window
    {
        Cls_Urun urun = new();
        Variables variables = new ();
        string ozellikTip = string.Empty;
        public Popup_Ozellik_Ekle(string ozellikTipi)
        {
            InitializeComponent();
            ozellikTip = ozellikTipi;
            if (ozellikTip == null ) 
            { CRUDmessages.GeneralFailureMessageCustomMessage("Özellik Tipi Boş Olamaz"); Mouse.OverrideCursor = null;return; }
            
            cbx_kilit.SelectedIndex = 0;
            txt_kod.Text = urun.GetKod(ozellikTip);
            if (ozellikTip == "Ürün Grup")
                stack_panel_collapsed.Visibility = Visibility.Visible;
             
            Mouse.OverrideCursor = null;
        }

        private void btn_kaydet_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_kod.Text) ||
                    string.IsNullOrEmpty(txt_isim.Text))
                { CRUDmessages.GeneralFailureMessageCustomMessage("Kod ve İsim Boş Olamaz");  return; }

                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

                ComboBoxItem selectedItem = new ComboBoxItem();
                selectedItem = cbx_kilit.SelectedItem as ComboBoxItem;
                if (ozellikTip == "Ürün Grup")
                {

                    Cls_Urun urun = new Cls_Urun
                    {
                        UrunGrubuKodu = txt_kod.Text,
                        UrunGrubuIsim = txt_isim.Text,
                        UrunGrubuSira = string.IsNullOrEmpty(txt_sira.Text) ? 0 : Convert.ToInt32(txt_sira.Text),
                        Kilit = selectedItem.Content.ToString(),
                        Kod1 = txt_kod1.Text,
                        Kod2 = txt_kod2.Text,
                        Kod3 = txt_kod3.Text,
                        Kod4 = txt_kod4.Text,
                        Kod5 = txt_kod5.Text,
                        Muhdetay = string.IsNullOrEmpty(txt_muhdetay.Text) ? 0 : Convert.ToInt32(txt_muhdetay.Text),
                        TeslimGunu = string.IsNullOrEmpty(txt_teslim_gunu.Text) ? 0 : Convert.ToInt32(txt_teslim_gunu.Text),
                        Kdv = string.IsNullOrEmpty(txt_kdv.Text) ? 0 : Convert.ToInt32(txt_kdv.Text),
                        TakimKodu = string.IsNullOrEmpty(txt_takim_kod.Text) ? 0 : Convert.ToInt32(txt_takim_kod.Text),
                        UniteKod = string.IsNullOrEmpty(txt_unite_kodu.Text) ? 0 : Convert.ToInt32(txt_unite_kodu.Text),
                        MenuGrup = txt_menu_grup.Text,
                        Sayfa = txt_sayfa.Text,
                    };
                    variables.Result = urun.InsertOzellik(urun, ozellikTip);
                }
                if (ozellikTip == "Model")
                {
                    Cls_Urun urun = new Cls_Urun
                    {
                    ModelKodu = txt_kod.Text,
                    ModelIsim = txt_isim.Text,
                    ModelSira = string.IsNullOrEmpty(txt_sira.Text) ? 0 : Convert.ToInt32(txt_sira.Text),
                    ModelKilit = selectedItem.Content.ToString(),
                    ModelKod1 = txt_kod1.Text,
                    ModelKod2 = txt_kod2.Text,
                    ModelKod3 = txt_kod3.Text,
                    ModelKod4 = txt_kod4.Text,
                    ModelKod5 = txt_kod5.Text,
                    };
                    variables.Result = urun.InsertOzellik(urun, ozellikTip);
                }
                if (ozellikTip == "Satış Şekil")
                {
                    Cls_Urun urun = new Cls_Urun
                    {
                        SatisSekilKodu = txt_kod.Text,
                        SatisSekilIsim = txt_isim.Text,
                        SatisSekilSira = string.IsNullOrEmpty(txt_sira.Text) ? 0 : Convert.ToInt32(txt_sira.Text),
                        SatisSekilKilit = selectedItem.Content.ToString(),
                        SatisSekilKod1 = txt_kod1.Text,
                        SatisSekilKod2 = txt_kod2.Text,
                        SatisSekilKod3 = txt_kod3.Text,
                        SatisSekilKod4 = txt_kod4.Text,
                        SatisSekilKod5 = txt_kod5.Text,

                    };

                    variables.Result = urun.InsertOzellik(urun, ozellikTip);
                }
                if (!variables.Result)
                    {
                        CRUDmessages.GeneralFailureMessage("Özellik Kaydedilirken"); Mouse.OverrideCursor = null;return;
                    }

                CRUDmessages.InsertSuccessMessage("Ürün", 1);
                Mouse.OverrideCursor = null;
                Popup_Ozellik_Ekle popup_ = new Popup_Ozellik_Ekle(ozellikTip);
                popup_.Show();
                this.Close();
            }
            
            catch 
            {
                CRUDmessages.GeneralFailureMessage("Özellik Kaydedilirken");Mouse.OverrideCursor = null;
            }
        }
    }
}
