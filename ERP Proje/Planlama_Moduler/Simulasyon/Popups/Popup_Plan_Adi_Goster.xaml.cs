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
using static Layer_Business.Cls_Base;

namespace Layer_UI.Planlama_Moduler.Simulasyon.Popups
{
    /// <summary>
    /// Popup_Plan_Adi_Goster.xaml etkileşim mantığı
    /// </summary>
    public partial class Popup_Plan_Adi_Goster : Window
    {
        Cls_Planlama plan = new();
        public Popup_Plan_Adi_Goster(string simulasyonTip)
        {
            InitializeComponent();

            ObservableCollection<Cls_Planlama> planAdiCollection = new();
            planAdiCollection = plan.GetDistinctPlanAdi(simulasyonTip);
            if(planAdiCollection == null)
            {
                CRUDmessages.QueryIsEmpty("Plan Adı");
                Mouse.OverrideCursor = null;
                return;
            }

            dg_Plan_Adlari.ItemsSource = planAdiCollection;
            Mouse.OverrideCursor = null;

        }

        private void plan_adi_detay_clicked (object sender, RoutedEventArgs e)
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

                ObservableCollection<Cls_Planlama> planAdiDetayCollection = plan.GetPlanAdiDetay(planItem, "Simülasyon");

                Popup_Plan_Adi_Detay _frm = new(planAdiDetayCollection);
                _frm.ShowDialog();
                
            }
            catch 
            {
                CRUDmessages.GeneralFailureMessageCustomMessage("Plan Adı Detayları Getirilirken");
                Mouse.OverrideCursor = null;
            }
        }

    }
}
