using Layer_Business;
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

namespace Layer_UI.Planlama_Moduler.Simulasyon.Popups
{
    /// <summary>
    /// Popup_Plan_Adi_Detay.xaml etkileşim mantığı
    /// </summary>
    public partial class Popup_Plan_Adi_Detay : Window
    {
        public Popup_Plan_Adi_Detay(ObservableCollection<Cls_Planlama> planDetay)
        {
            InitializeComponent();

            dg_Plan_Adlari.ItemsSource = planDetay;
            Mouse.OverrideCursor = null;
        }
    }
}
