using Layer_2_Common.Type;
using Layer_Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Layer_UI.Konfeksiyon
{
    public partial class Frm_Uretim_Durumu : Window
    {
        public Frm_Uretim_Durumu()
        {
            InitializeComponent();
        }
        Variables variables = new Variables();
        public void listele_click(object sender, RoutedEventArgs e)
        {

            
            Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);
            try
            {
                variables.Query = "select * from vbvKonfeksiyonUretimDurum where 1=1";

                if (txt_stok_adi.Text != "")
                {
                    variables.Query = variables.Query + " and mamulAdi like '%" + txt_stok_adi.Text + "%'";
                }

                if (txt_kumas_kod.Text != "")
                {
                    variables.Query = variables.Query + " and kumasKodu like '%" + txt_kumas_kod.Text + "%'";
                }

                DataTable dataTable = SelectStatement.GetDataTable(variables.Query, variables.Yil);

                dg_genel_durum.ItemsSource = dataTable.DefaultView;

                Mouse.OverrideCursor = null;

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                MessageBox.Show(ex.Message);
                // Close the modal dialog in case of an error

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
