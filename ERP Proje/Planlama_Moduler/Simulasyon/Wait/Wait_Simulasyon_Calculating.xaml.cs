using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Ahsap.Planlama.Popups;
using Layer_UI.Planlama_Moduler.Simulasyon.Popups;
using Layer_UI.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace Layer_UI.Planlama_Moduler.Simulasyon.Wait
{
    /// <summary>
    /// Interaction logic for Wait_Simulasyon_Calculating.xaml
    /// </summary>
    public partial class Wait_Simulasyon_Calculating : Window
    {
        Variables variables = new ();
        Cls_Planlama plan = new();
        UserControl_Wait uc = new();
        string simulasyonTip = string.Empty;
        public Wait_Simulasyon_Calculating(string simulasyonTipi)
        {
            InitializeComponent();
            simulasyonTip = simulasyonTipi;

        }
        private async void btn_hesaplat_clicked(object sender, RoutedEventArgs e) 
        {
            try
            {
                btn_hesaplat.Visibility = Visibility.Collapsed;
                txt_wait.Visibility = Visibility.Visible;

                variables.ResultInt = await plan.Simulasyon(simulasyonTip);

                if (variables.ResultInt == -1)
                { CRUDmessages.GeneralFailureMessage("Simülasyon Hesaplanırken"); this.Close(); return; }
                if (variables.ResultInt == 2)
                { CRUDmessages.GeneralFailureMessage("Genel İhtiyaç Hesaplanırken");this.Close(); return; }
                if (variables.ResultInt == 3)
                { CRUDmessages.GeneralFailureMessage("Plan Adları Alınırken"); this.Close(); return; }
                if (variables.ResultInt == 4)
                { CRUDmessages.GeneralFailureMessage("Plan İhtiyaçları Hesaplanırken"); this.Close(); return; }

                CRUDmessages.UpdateSuccessMessage("Simülasyon");

                Mouse.OverrideCursor = null;
                if(simulasyonTip == "Simülasyon")
                { 
                    Popup_Simulasyon_Genel frm = new();
                    frm.Show();
                    this.Close();
                }
                if (simulasyonTip == "Simülasyon Sunta")
                {
                    Popup_Simulasyon_Sunta frm = new();
                    frm.Show();
                    this.Close();
                }
                if (simulasyonTip == "Ahsap Plan")
                {
                    Popup_Simulasyon_Ahsap_Plan frm = new();
                    frm.Show();
                    this.Close();
                }

            }
            catch (Exception)
            {
                CRUDmessages.GeneralFailureMessage("Hesaplama Yapılırken"); this.Close();
            }
        }
    }
}
