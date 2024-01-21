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

namespace Layer_UI.Ahsap.Planlama.Popups
{
    /// <summary>
    /// Interaction logic for Popup_Plan_Adi_Detay_Ahsap.xaml
    /// </summary>
    public partial class Popup_Plan_Adi_Detay_Ahsap : Window
    {
        public Popup_Plan_Adi_Detay_Ahsap(ObservableCollection<Cls_Planlama> planDetay)
        {
            InitializeComponent();
            dg_Plan_Adi_Detay.ItemsSource = planDetay;
            Mouse.OverrideCursor = null;
        }
    }
}
