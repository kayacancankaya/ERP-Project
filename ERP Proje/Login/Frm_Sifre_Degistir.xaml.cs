using System.Windows;
using System.Windows.Input;
using Layer_Business;
using Layer_2_Common.Type;

namespace Layer_UI.Login
{

    public partial class Frm_Sifre_Degistir : Window
    {
        LoginLogic login = new LoginLogic();
        Variables variables = new Variables();
        public Frm_Sifre_Degistir()
        {

            InitializeComponent();
            variables.IsTrue = login.CheckIfPcAddressExists();

            if( variables.IsTrue == false) { MessageBox.Show("Bilgisayar Sisteme Kayıtlı Değil.\n Yeni Kullanıcı Kaydı Oluşturunuz."); return; }
            
            else
            {
                login.Address = login.GetAddress();
                txt_user_name.Text = login.GetUserName(login.Address);
            }
        }


        private void mousedown_Window(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btn_minimize_click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btn_close_click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

        private void btn_sifre_degistir_clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_user_name.Text)) { MessageBox.Show("Kullanıcı Bilgisi Eksik.\n Yeni Kullanıcı Kaydı Oluşturunuz."); return;  }
            if (string.IsNullOrEmpty(txt_password.Password.ToString())) { MessageBox.Show("Şifre Giriniz."); return;  }

            variables.IsTrue = login.SavePaswordChanged(txt_user_name.Text, txt_password.Password.ToString());

            if(variables.IsTrue == false) { MessageBox.Show("Hata ile Karşılaşıldı.\n Yeni Kullanıcı Kaydı Oluşturunuz."); return; }

            else
            {
                MessageBox.Show("Değişiklik Kaydedildi.");
                Frm_Login frm_ = new Frm_Login();
                this.Close();
                frm_.Show();
            }
        }

        private void btn_giris_sayfasi_clicked(object sender, RoutedEventArgs e)
        {
            Frm_Login frm_  = new Frm_Login();
            this.Close();   
            frm_.Show();    
        }
    }
}
