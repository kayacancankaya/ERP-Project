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

namespace Layer_UI.Ahsap.Siparis.Popups
{
	/// <summary>
	/// Interaction logic for Popup_Sevk_Takip_Detay.xaml
	/// </summary>
	public partial class Popup_Siparis_Takip_Detay : Window
	{

		public Popup_Siparis_Takip_Detay(ObservableCollection<Cls_Sevk> wholeReport)
		{
			InitializeComponent();

			dg_Detayli_Rapor.ItemsSource = wholeReport;

			Mouse.OverrideCursor = null;
		}

	}
}
