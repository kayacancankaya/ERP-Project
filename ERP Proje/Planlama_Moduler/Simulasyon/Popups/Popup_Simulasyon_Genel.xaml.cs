using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using Layer_UI.Planlama_Moduler.Simulasyon.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
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

namespace Layer_UI.Planlama_Moduler.Simulasyon.Popups
{
    /// <summary>
    /// Interaction logic for Popup_Simulasyon_Genel.xaml
    /// </summary>
    public partial class Popup_Simulasyon_Genel : Window
    {
        public Popup_Simulasyon_Genel()
        {
            try
            {
                InitializeComponent();

                ObservableCollection<Cls_Planlama> planAdiCollection = new();
                planAdiCollection = plan.GetDistinctPlanAdiForSimulation("Simülasyon");
                if (planAdiCollection == null)
                {
                    CRUDmessages.QueryIsEmpty("Plan Adı");
                    return;
                }

                dg_Plan_Adlari.ItemsSource = planAdiCollection;
                dg_Plan_Adlari.Items.Refresh();
            }
            catch (Exception)
            {
                CRUDmessages.GeneralFailureMessage("Simülasyon Sonuç Formu Açılırken");
            }

        }
        Cls_Planlama plan = new();
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

                ObservableCollection<Cls_Planlama> planAdiDetayCollection = plan.GetPlanAdiDetay(planItem,"Simülasyon");

                Popup_Plan_Adi_Detay _frm = new(planAdiDetayCollection);
                _frm.ShowDialog();

                ObservableCollection<Cls_Planlama> planAdiCollection = new();
                planAdiCollection = plan.GetDistinctPlanAdiForSimulation("Simülasyon");
                if (planAdiCollection == null)
                {
                    CRUDmessages.QueryIsEmpty("Plan Adı");
                    return;
                }

                dg_Plan_Adlari.ItemsSource = planAdiCollection;

            }
            catch
            {
                CRUDmessages.GeneralFailureMessageCustomMessage("Plan Adı Detayları Getirilirken");
                Mouse.OverrideCursor = null;
            }
        }

    }
}
