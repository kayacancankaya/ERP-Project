using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using Layer_UI.Planlama_Moduler.Simulasyon.Popups;
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

namespace Layer_UI.Ahsap.Planlama.Popups
{
    /// <summary>
    /// Interaction logic for Popup_Plan_Adi_Goster_Ahsap.xaml
    /// </summary>
    public partial class Popup_Plan_Adi_Goster_Ahsap : Window
    {
        Cls_Planlama plan = new();

        ObservableCollection<Cls_Planlama> planAdiCollection = new();
        string simulasyonTipi = string.Empty;
        public Popup_Plan_Adi_Goster_Ahsap(string simulasyonTip)
        {
            InitializeComponent();
            simulasyonTipi = simulasyonTip;
            planAdiCollection = plan.GetDistinctPlanAdi(simulasyonTipi);
            if (planAdiCollection == null)
            {
                CRUDmessages.QueryIsEmpty("Plan Adı");
                Mouse.OverrideCursor = null;
                return;
            }

            dg_Plan_Adlari.ItemsSource = planAdiCollection;
            Mouse.OverrideCursor = null;

        }

        private void plan_adi_detay_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
                Cls_Planlama dataItem = UIinteractions.GetDataItemFromButton<Cls_Planlama>(sender);

                Cls_Planlama? planItem = new Cls_Planlama
                {
                    PlanAdiSira = dataItem.PlanAdiSira,
                    PlanAdi = dataItem.PlanAdi,
                };

                ObservableCollection<Cls_Planlama> planAdiDetayCollection = plan.GetPlanAdiDetay(planItem, "Ahsap Plan");

                Popup_Plan_Adi_Detay_Ahsap _frm = new(planAdiDetayCollection);
                var result = _frm.ShowDialog();

                if (result == false)
                {

                    planAdiCollection = plan.GetDistinctPlanAdi(simulasyonTipi);
                    if (planAdiCollection == null)
                    {
                        CRUDmessages.QueryIsEmpty("Plan Adı");
                        Mouse.OverrideCursor = null;
                        return;
                    }

                    dg_Plan_Adlari.ItemsSource = planAdiCollection;
                    dg_Plan_Adlari.Items.Refresh();
                    Mouse.OverrideCursor = null;
                }

            }
            catch
            {
                CRUDmessages.GeneralFailureMessageCustomMessage("Plan Adı Detayları Getirilirken");
                Mouse.OverrideCursor = null;
            }
        }

    }
}