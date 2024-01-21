using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Layer_2_Common.Type;
using Layer_Business;

namespace Layer_UI.Login
{
    /// <summary>
    /// Interaction logic for Frm_Ilk_Kayit.xaml
    /// </summary>
    public partial class Frm_Ilk_Kayit : Window
    {
        public Frm_Ilk_Kayit()
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

        private void btn_minimize_click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btn_close_click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        private void btn_kaydet_click(object sender, RoutedEventArgs e)
        {
            Variables variables = new Variables();
            variables.ErrorMessage = string.Empty;
            ComboBoxItem selectedComboBoxItem = txt_departman.SelectedItem as ComboBoxItem;

            if (string.IsNullOrEmpty(txt_user_name.Text)) { variables.ErrorMessage = variables.ErrorMessage + "Kullanici Adini Giriniz.\n"; }
            if (string.IsNullOrEmpty(txt_password.Password.ToString())) { variables.ErrorMessage = variables.ErrorMessage + "Şifre Giriniz.\n"; }
            if (string.IsNullOrEmpty(selectedComboBoxItem.Content.ToString())) { variables.ErrorMessage = variables.ErrorMessage + "Departman Giriniz\n"; }
            if (string.IsNullOrEmpty(txt_email.Text)) { variables.ErrorMessage = variables.ErrorMessage + "Email Giriniz.\n"; }
            
            if (string.IsNullOrEmpty(variables.ErrorMessage)==false) { MessageBox.Show(variables.ErrorMessage);return; }

            LoginLogic login = new LoginLogic();

            variables.IsTrue = login.CheckIfUserExists(txt_user_name.Text);
            if (variables.IsTrue) { variables.ErrorMessage = variables.ErrorMessage + "Kullanici Adi Mevcut.\n"; }
            variables.IsTrue = false;

            variables.IsTrue = login.CheckIfPcAddressExists();
            if (variables.IsTrue) { variables.ErrorMessage = variables.ErrorMessage + "Bilgisayar Sistemde Kayıtlı.\n"; }
            variables.IsTrue = false;

            if (string.IsNullOrEmpty(variables.ErrorMessage) == false) { MessageBox.Show(variables.ErrorMessage); return; }

            variables.IsTrue = login.SaveNewUser(txt_user_name.Text,txt_password.Password.ToString(), selectedComboBoxItem.Content.ToString(), txt_email.Text);
            if(variables.IsTrue) { MessageBox.Show("Yeni Kullanıcı Sisteme Kaydedildi."); }

            Cls_Login_Ui login_Ui = new Cls_Login_Ui();
            Window window = new Window();
            window=login_Ui.GetDepartmentForMainPage(txt_user_name.Text, txt_password.Password.ToString());
            this.Close();
            window.Show();


        }

        private void btn_giris_sayfasi_clicked(object sender, RoutedEventArgs e)
        {
            Frm_Login frm_ = new Frm_Login();
            this.Close();
            frm_.Show();
        }
    }
}
