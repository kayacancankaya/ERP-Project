using System;
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

namespace Layer_UI.Bilgi_Islem
{
    /// <summary>
    /// Interaction logic for Frm_Bilgi_Islem.xaml
    /// </summary>
    public partial class Frm_Bilgi_Islem : Window
    {
        public Frm_Bilgi_Islem()
        {

            InitializeComponent();

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
