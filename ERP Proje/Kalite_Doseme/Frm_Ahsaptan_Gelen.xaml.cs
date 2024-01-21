using Layer_2_Common.Type;
using Layer_Data;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Layer_UI.Kalite_Doseme
{
    /// <summary>
    /// Interaction logic for Frm_Ahsaptan_Gelen.xaml
    /// </summary>
    public partial class Frm_Ahsaptan_Gelen : Window
    {
        public Frm_Ahsaptan_Gelen()
        {
            InitializeComponent();
        }
        Variables variables = new();
        public void listele_click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            try
            {
                variables.Query = "select * from vbvAhsaptanGelen where 1=1 ";

                if(string.IsNullOrEmpty(txt_urun_kodu.Text) == false )
                {
                    variables.Query = variables.Query + $"and Urun_Kodu like '%{txt_urun_kodu.Text}%' ";
                }
                
                if (string.IsNullOrEmpty(txt_urun_adi.Text) == false)
                {
                    variables.Query = variables.Query + $"and Urun_Adi like '%{txt_urun_adi.Text}%' ";
                }
                
                if (string.IsNullOrEmpty(txt_ham_kodu.Text) == false)
                {
                    variables.Query = variables.Query + $"and Ham_Kodu like '%{txt_ham_kodu.Text}%' ";
                }
                
                if (string.IsNullOrEmpty(txt_ham_adi.Text) == false)
                {
                    variables.Query = variables.Query + $"and Ham_Adi like '%{txt_ham_adi.Text}%' ";
                }

                DataTable dataTable = SelectStatement.GetDataTable(variables.Query, variables.Yil);
                if (dataTable.Rows.Count == 0)
                {
                    lbl_uyari.Visibility = Visibility.Visible;
                }
                else
                {
                    dg_genel_durum.ItemsSource = dataTable.DefaultView;
                }

                Mouse.OverrideCursor = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
             
                Mouse.OverrideCursor = null;
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
