using Layer_2_Common.Type;
using Layer_Data;
using Microsoft.VisualBasic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Layer_Business
{
    public class Cls_Siparis : Cls_Base,INotifyPropertyChanged
    {
        private string _stokKodu = "";

        public string StokKodu
        {
            get { return _stokKodu; }
            set { _stokKodu = value;
                OnPropertyChanged(nameof(StokKodu));
            }
        }
        private string _stokAdi = "";

        public string StokAdi
        {
            get { return _stokAdi; }
            set {
                _stokAdi = value;
                OnPropertyChanged(nameof(StokAdi));
            }
        }

        private string _fisno = "";

        public string Fisno
        {
            get { return _fisno; }
            set { _fisno = value;
                OnPropertyChanged(nameof(Fisno));
            }
        }
        private int _fisSira;

        public int FisSira
        {
            get { return _fisSira; }
            set {
                _fisSira = value;
                OnPropertyChanged(nameof(FisSira));
            }
        }


        private int _siparisMiktar;
        public int SiparisMiktar
        {
            get { return _siparisMiktar; }
            set {
                _siparisMiktar = value;
                OnPropertyChanged(nameof(SiparisMiktar));
            }
        }

        private int _siparisTeslimMiktar;

        public int SiparisTeslimMiktar
        {
            get { return _siparisTeslimMiktar; }
            set {
                _siparisTeslimMiktar = value;
                OnPropertyChanged(nameof(SiparisTeslimMiktar));
            }
        }
        private int _siparisBakiye;

        //satış irsaliyesi giriş için kullanılan miktar
        public int SatisSiparisGirisMiktar { get; set; }
        public int SiparisBakiye
        {
            get { return _siparisBakiye; }
            set {
                _siparisBakiye = value;
                OnPropertyChanged(nameof(SiparisBakiye));
            }
        }
       
        private decimal _dovizFiyat = 0;

        public decimal DovizFiyat
        {
            get { return _dovizFiyat; }
            set {
				_dovizFiyat = value;
                OnPropertyChanged(nameof(DovizFiyat));
            }
        }
		private decimal _siparisFiyat = 0;
		public decimal SiparisFiyat
        {
            get { return _siparisFiyat; }
            set {
				_siparisFiyat = value;
                OnPropertyChanged(nameof(SiparisFiyat));
            }
        }

        private double _siparisToplamTutar;

        public double SiparisToplamTutar
        {
            get { return _siparisToplamTutar; }
            set { _siparisToplamTutar = value; }
        }

        public double ToplamSiparisKalemi { get; set; }

        public double SiparisToplamKDV { get; set; }


        private string _siparisTarih; 

        public string SiparisTarih
        {
            get { return _siparisTarih; }
            set { _siparisTarih = value; }
        }

        private string _siparisDurum = "H";

        public string SiparisDurum 
        {
            get { return _siparisDurum; }
            set { _siparisDurum = value;
            OnPropertyChanged(nameof(SiparisDurum));
            }
        }


		private DateTime _terminTarih = DateTime.Now.AddMonths(2);

		public DateTime TerminTarih
        {
            get { return _terminTarih; }
            set { _terminTarih = value; }
        }

        private DateTime _talepTarih = DateTime.Now;

        public DateTime TalepTarih
        {
            get { return _talepTarih; }
            set { _talepTarih = value; }
        }


        private string _destinasyon = "";

        public string Destinasyon
        {
            get { return _destinasyon; }
            set {
                _destinasyon = value;
                OnPropertyChanged(nameof(Destinasyon));
            }
        }

        private string _siparisAciklama = "";

        public string SiparisAciklama
        {
            get { return _siparisAciklama; }
            set {
                _siparisAciklama = value;
                OnPropertyChanged(nameof(SiparisAciklama));
            }
        }
        private string _siparisSatirAciklama = "";

        public string SiparisSatirAciklama
        {
            get { return _siparisSatirAciklama; }
            set {
                _siparisSatirAciklama = value;
                OnPropertyChanged(nameof(SiparisSatirAciklama));
            }
        }
        private string _POnumarasi = "";

        public string POnumarasi
        {
            get { return _POnumarasi; }
            set {
                _POnumarasi = value;
                OnPropertyChanged(nameof(POnumarasi));
            }
        }


        private string _dovizTipi;

        public string DovizTipi
        {
            get { return _dovizTipi; }
            set
            {
                _dovizTipi = value;
                OnPropertyChanged(nameof(DovizTipi));
            }
        }


		private string _siparisTipi = "";//yurt içi yurt dışı

        public string SiparisTipi
        {
            get { return _siparisTipi; }
            set
            {
                _siparisTipi = value;
                OnPropertyChanged(nameof(SiparisTipi));
            }
        }


        private decimal _stokKDV ;

        public decimal StokKDV
        {
            get { return _stokKDV; }
            set
            {
                _stokKDV = value;
                OnPropertyChanged(nameof(StokKDV));
            }
        }
        public int UserID { get; set; }

        public string UserName { get; set; }

        private string _varyantVarMi = "";

        public string VaryantVarMi
        {
            get { return _varyantVarMi; }
            set
            {
                _varyantVarMi = value;
                OnPropertyChanged(nameof(VaryantVarMi));
            }
        }


        public decimal TedarikSiparisMiktar { get; set; }
        public decimal TedarikTeslimMiktar { get; set; }
        public decimal TedarikSiparisBakiye { get; set; }
        public decimal TedarikGirisMiktar { get; set; }

        public int Vade { get; set; }

        public int DepoKodu { get; set; }

        private Cls_Cari _associatedCari = new Cls_Cari();

        public Cls_Cari AssociatedCari
        {
            get { return _associatedCari; }
            set
            {
                if (value != null)
                {
                    _associatedCari = value;
                    OnPropertyChanged(nameof(AssociatedCari));
                }
                else
                {
                    throw new ArgumentException("Sipariş için Cari Oluşturulurken Hata İle Karşılaşıldı.");
                }
            }
        }


        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; 
                OnPropertyChanged(nameof(IsChecked));
                }
        }


        private ObservableCollection<Cls_Siparis> _siparisCollection = new ();
        private ObservableCollection<Cls_Siparis> temp_coll_sip = new ();
        public ObservableCollection<Cls_Siparis> SiparisDetayCollection = new ();

        public ObservableCollection<Cls_Siparis> SiparisCollection
        {
            get { return _siparisCollection; }
            set
            {
                if (value != null)
                {
                    _siparisCollection = value;
                    OnPropertyChanged(nameof(SiparisCollection));
                }
                else
                {
                    throw new ArgumentException("Sipariş Oluşturulurken Hata İle Karşılaşıldı.");
                }
            }
        }

        DataLayer dataLayer = new ();
        DataTable dataTable = new ();
        Variables variables = new ();
        LoginLogic login = new();

        public Cls_Siparis()
        {
            variables.Fabrika = login.GetFabrika();
        }

        //vita için fişno üret P0S
        public string SatisSiparisiIcinFisNoUret()
        {
            try
            {
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader("select dbo.vbfSatisSiparisiIcinFisNoUret()", variables.Yil))
                {
                    if (reader != null && reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            Fisno = reader.GetString(0);
                        else
                            Fisno = "P0S000000000001";
                    }
                }
                return Fisno;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return null; }
        }
        //Ahşap için fiş no üret PAS
        public string SatisSiparisiIcinFisNoUret(string fabrika)
        {
            try
            {
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader("select dbo.vbfSatisSiparisiIcinFisNoUret()", variables.Yil,fabrika))
                {
                    if (reader != null && reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            Fisno = reader.GetString(0);
                        else
                            Fisno = "PAS000000000001";
                    }
                }
                return Fisno;
            }
            catch { 
                return string.Empty; }
        }

        public bool InsertSiparisGenel(string SatisCariKodu, int DovizTipi, string TeslimCariKodu, string aciklama,
                                       int satisIhracatMi, decimal brutTutar, decimal toplamKdv, string destinasyon, 
                                       int toplamSiparisKalemi,int portalID, string POno, string fisno)
        {
            try
            {
                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)>
                {
                    { "@satisCariKodu", (SqlDbType.NVarChar, 35, 0, SatisCariKodu) },
                    { "@dovtip", (SqlDbType.TinyInt, 0, 0, DovizTipi) },
                    { "@teslimCariKod", (SqlDbType.NVarChar, 15, 0, TeslimCariKodu) },
                    { "@aciklama", (SqlDbType.NVarChar, 35, 0, aciklama) },
                    { "@satisIhracatMi", (SqlDbType.TinyInt, 0, 0, satisIhracatMi) },
                    { "@brutTutar", (SqlDbType.Decimal, 28, 8, brutTutar) },
                    { "@toplamKdv", (SqlDbType.Decimal, 28, 8, toplamKdv) },
                    { "@destinasyon", (SqlDbType.NVarChar, 20, 0, destinasyon) },
                    { "@toplamSiparisKalemi", (SqlDbType.SmallInt, 0, 0, toplamSiparisKalemi) },
                    { "@portalID", (SqlDbType.SmallInt, 0, 0, portalID) },
                    { "@POno", (SqlDbType.NVarChar, 100, 0, POno) },
                    { "@fisno", (SqlDbType.NVarChar, 15, 0, fisno) },
                };

                variables.IsTrue = dataLayer.ExecuteStoredProcedureWithParameters("vbpInsertSiparisOnayaGonderMas", variables.Yil, parameters);
                return variables.IsTrue;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return false; }
        }

        public bool InsertSiparisSatir(string StokKodu, int SiparisMiktar, decimal SiparisFiyat, string TerminTarih, 
                                       string SatisCariKodu,int sira, string TeslimCariKodu, int DovizTipi, string fisno)
        {
           try
            {

                StokKDV = GetKdvOrani(fisno, StokKodu);

                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)>
                    {
                        { "@stokKodu", (SqlDbType.NVarChar, 35, 0, StokKodu) },
                        { "@siparisMiktar", (SqlDbType.Int, 0, 0, SiparisMiktar) },
                        { "@siparisFiyat", (SqlDbType.Decimal, 18, 2, SiparisFiyat) },
                        { "@satisCariKodu", (SqlDbType.NVarChar, 35, 0, SatisCariKodu) },
                        { "@dovtip", (SqlDbType.TinyInt, 0, 0, DovizTipi) },
                        { "@teslimCariKod", (SqlDbType.NVarChar, 35, 0, TeslimCariKodu) },
                        { "@sira", (SqlDbType.SmallInt, 0, 0, sira) },
                        { "@teslimTarih", (SqlDbType.DateTime, 0, 0, TerminTarih) },
                        { "@kdv", (SqlDbType.Decimal, 18, 2, StokKDV) },
                        { "@fisno", (SqlDbType.NVarChar, 15, 0, fisno) },
                    };

                variables.IsTrue = dataLayer.ExecuteStoredProcedureWithParameters("vbpInsertSiparisOnayaGonder", variables.Yil, parameters);
                return variables.IsTrue;
            }
            catch (Exception ex) {MessageBox.Show(ex.Message.ToString());return false;}
        }


        public bool InsertSatisIrsaliyesiGenel(string fisno, string teslimCariKodu, decimal brutTutar, decimal toplamKdv, int toplamSiparisKalemi,string siparisNumarasi, string fabrika)
         {
            try
            {

                login.User = login.GetUserName().Substring(0, Math.Min(12, login.GetUserName().Length));

                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)> { 
                    { "@cariKod", (SqlDbType.NVarChar, 15, 0, teslimCariKodu) },
                    { "@brutTutar", (SqlDbType.Decimal, 28, 8, brutTutar) },
                    { "@toplamKdv", (SqlDbType.Decimal, 28, 8, toplamKdv) },
                    { "@toplamSiparisKalemi", (SqlDbType.SmallInt, 0, 0, toplamSiparisKalemi) },
                    { "@userName", (SqlDbType.NVarChar, 12, 0, login.User) },
                    { "@fisno", (SqlDbType.NVarChar, 15, 0, fisno) },
                    { "@siparisNo", (SqlDbType.NVarChar, 15, 0, siparisNumarasi) },
                };

                variables.IsTrue = dataLayer.ExecuteStoredProcedureWithParameters("vbpInsertSatisIrsaliyesiMas", variables.Yil, parameters,fabrika);
                return variables.IsTrue;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return false; }
        }
        public bool InsertSatisIrsaliyesiSatir(string stokKodu, decimal siparisMiktar, decimal siparisFiyat,
                                              string cariKodu, int sira, string fisno, string siparisNumarasi,
                                              int siparisSira, int depoKodu, string fabrika)
        {
            try
            {

                StokKDV = GetKdvOrani(siparisNumarasi, StokKodu, fabrika);

                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)>
                    {
                        { "@stokKodu", (SqlDbType.NVarChar, 35, 0, stokKodu) },
                        { "@siparisMiktar", (SqlDbType.Decimal, 28, 8, siparisMiktar) },
                        { "@siparisFiyat", (SqlDbType.Decimal, 18, 2, siparisFiyat) },
                        { "@cariKodu", (SqlDbType.NVarChar, 15, 0, cariKodu) },
                        { "@sira", (SqlDbType.SmallInt, 0, 0, sira) },
                        { "@kdv", (SqlDbType.Decimal, 18, 2, StokKDV) },
                        { "@fisno", (SqlDbType.NVarChar, 15, 0, fisno) },
                        { "@siparisNo", (SqlDbType.NVarChar, 15, 0, siparisNumarasi) },
                        { "@siparisSira", (SqlDbType.Int, 0, 0, siparisSira) },
                        { "@depoKodu", (SqlDbType.Int, 0, 0, depoKodu) },
                    };

                variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vbpInsertSatisIrsaliyesiSatir", variables.Yil, parameters, fabrika);

                return variables.Result;
            }
            catch { return false; }
        }
        public bool InsertTedarikSiparisGenel(string irsaliyeNumarasi, string tedarikciCariKodu, decimal brutTutar, decimal toplamKdv, int toplamSiparisKalemi, string siparisNumarasi, string fabrika)
         {
            try
            {

                login.User = login.GetUserName().Substring(0, Math.Min(12, login.GetUserName().Length));

                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)> { 
                    { "@cariKod", (SqlDbType.NVarChar, 15, 0, tedarikciCariKodu) },
                    { "@brutTutar", (SqlDbType.Decimal, 28, 8, brutTutar) },
                    { "@toplamKdv", (SqlDbType.Decimal, 28, 8, toplamKdv) },
                    { "@toplamSiparisKalemi", (SqlDbType.SmallInt, 0, 0, toplamSiparisKalemi) },
                    { "@userName", (SqlDbType.NVarChar, 12, 0, login.User) },
                    { "@fisno", (SqlDbType.NVarChar, 15, 0, irsaliyeNumarasi) },
                    { "@siparisNo", (SqlDbType.NVarChar, 15, 0, siparisNumarasi) },
                };

                variables.IsTrue = dataLayer.ExecuteStoredProcedureWithParameters("vbpInsertSatisIrsaliyesiSatir", variables.Yil, parameters,fabrika);
                return variables.IsTrue;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return false; }
        }

        public int InsertTedarikSiparisSatir(string stokKodu, decimal siparisMiktar, decimal siparisFiyat,
                                              string cariKodu, int sira, string irsaliyeNumarasi, string siparisNumarasi,
                                              int siparisSira, int depoKodu, string fabrika)
        {
            try
            {

                StokKDV = GetKdvOrani(siparisNumarasi, StokKodu, fabrika);

                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)>
                    {
                        { "@stokKodu", (SqlDbType.NVarChar, 35, 0, stokKodu) },
                        { "@siparisMiktar", (SqlDbType.Decimal, 28, 8, siparisMiktar) },
                        { "@siparisFiyat", (SqlDbType.Decimal, 18, 2, siparisFiyat) },
                        { "@cariKodu", (SqlDbType.NVarChar, 15, 0, cariKodu) },
                        { "@sira", (SqlDbType.SmallInt, 0, 0, sira) },
                        { "@kdv", (SqlDbType.Decimal, 18, 2, StokKDV) },
                        { "@fisno", (SqlDbType.NVarChar, 15, 0, irsaliyeNumarasi) },
                        { "@siparisNo", (SqlDbType.NVarChar, 15, 0, siparisNumarasi) },
                        { "@siparisSira", (SqlDbType.Int, 0, 0, siparisSira) },
                        { "@depoKodu", (SqlDbType.Int, 0, 0, depoKodu) },
                    };

                variables.ResultInt = dataLayer.Get_One_Int_Result_Stored_Proc_With_Parameters("vbpInsertIrsaliyeSatir", variables.Yil, parameters,fabrika);

                return variables.ResultInt;
            }
            catch  { return -1; }
        }
        public bool InsertTedarikSiparisGenel(string irsaliyeNumarasi, string tedarikciCariKodu, decimal brutTutar, decimal toplamKdv, int toplamSiparisKalemi, string siparisNumarasi)
         {
            try
            {

                login.PortalID = login.GetPortalID();



                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)> { 
                    { "@cariKod", (SqlDbType.NVarChar, 15, 0, tedarikciCariKodu) },
                    { "@brutTutar", (SqlDbType.Decimal, 28, 8, brutTutar) },
                    { "@toplamKdv", (SqlDbType.Decimal, 28, 8, toplamKdv) },
                    { "@toplamSiparisKalemi", (SqlDbType.SmallInt, 0, 0, toplamSiparisKalemi) },
                    { "@portalID", (SqlDbType.SmallInt, 0, 0, login.PortalID) },
                    { "@fisno", (SqlDbType.NVarChar, 15, 0, irsaliyeNumarasi) },
                    { "@siparisNo", (SqlDbType.NVarChar, 15, 0, siparisNumarasi) },
                };

                variables.IsTrue = dataLayer.ExecuteStoredProcedureWithParameters("vbpInsertIrsaliyeMas", variables.Yil, parameters);
                return variables.IsTrue;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return false; }
        }

        public int InsertTedarikSiparisSatir(string stokKodu, decimal siparisMiktar, decimal siparisFiyat,
                                              string cariKodu, int sira, string irsaliyeNumarasi, string siparisNumarasi,
                                              int siparisSira, int depoKodu)
        {
            try
            {

                StokKDV = GetKdvOrani(siparisNumarasi, StokKodu);

                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)>
                    {
                        { "@stokKodu", (SqlDbType.NVarChar, 35, 0, stokKodu) },
                        { "@siparisMiktar", (SqlDbType.Decimal, 28, 8, siparisMiktar) },
                        { "@siparisFiyat", (SqlDbType.Decimal, 18, 2, siparisFiyat) },
                        { "@cariKodu", (SqlDbType.NVarChar, 15, 0, cariKodu) },
                        { "@sira", (SqlDbType.SmallInt, 0, 0, sira) },
                        { "@kdv", (SqlDbType.Decimal, 18, 2, StokKDV) },
                        { "@fisno", (SqlDbType.NVarChar, 15, 0, irsaliyeNumarasi) },
                        { "@siparisNo", (SqlDbType.NVarChar, 15, 0, siparisNumarasi) },
                        { "@siparisSira", (SqlDbType.Int, 0, 0, siparisSira) },
                        { "@depoKodu", (SqlDbType.Int, 0, 0, depoKodu) },
                    };

                variables.ResultInt = dataLayer.Get_One_Int_Result_Stored_Proc_With_Parameters("vbpInsertIrsaliyeSatir", variables.Yil, parameters);

                return variables.ResultInt;
            }
            catch  { return -1; }
        }
        public bool InsertSiparisKaydetGenel(Cls_Siparis siparisGenel, string fabrika)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@cariKodu", SqlDbType.NVarChar, 15);
                    parameters[0].Value = siparisGenel.AssociatedCari.TeslimCariKodu;
                    parameters[1] = new SqlParameter("@genelAciklama", SqlDbType.NVarChar, 20);
                    parameters[1].Value = siparisGenel.SiparisAciklama;
                    parameters[2] = new SqlParameter("@brutTutar", SqlDbType.Float);
                    parameters[2].Value = Convert.ToDouble(siparisGenel.SiparisToplamTutar);
                    parameters[3] = new SqlParameter("@toplamKdv", SqlDbType.Float);
                    parameters[3].Value = siparisGenel.SiparisToplamKDV;
                    parameters[4] = new SqlParameter("@toplamSiparisKalemi", SqlDbType.SmallInt);
                    parameters[4].Value = siparisGenel.ToplamSiparisKalemi;
                    parameters[5] = new SqlParameter("@kullaniciAdi", SqlDbType.NVarChar,12);
                    parameters[5].Value = login.GetUserName().Substring(0, Math.Min(12, login.GetUserName().Length));
                    parameters[6] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                    parameters[6].Value = siparisGenel.Fisno;
            
                variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vapInsertSiparisMas",variables.Yil,parameters,fabrika);
                    if (!variables.Result)
                        return false;
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertSiparisKaydetSatir(ObservableCollection<Cls_Siparis> siparisCollection, string fisno, string fabrika)
        {
            try
            {
                variables.Counter = 1;
                foreach (Cls_Siparis item in siparisCollection) 
                { 
                    SqlParameter[] parameters = new SqlParameter[10];
                    parameters[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[0].Value = item.StokKodu;
                    parameters[1] = new SqlParameter("@siparisMiktar", SqlDbType.Int);
                    parameters[1].Value = item.SiparisMiktar;
                    parameters[2] = new SqlParameter("@siparisFiyat", SqlDbType.Decimal);
                    parameters[2].Value = item.SiparisFiyat;
                    parameters[3] = new SqlParameter("@satirAciklama", SqlDbType.NVarChar,300);
                    parameters[3].Value = item.SiparisSatirAciklama;
                    parameters[4] = new SqlParameter("@cariKodu", SqlDbType.NVarChar,15);
                    parameters[4].Value = item.AssociatedCari.TeslimCariKodu;
                    parameters[5] = new SqlParameter("@sira", SqlDbType.SmallInt);
                    parameters[5].Value = variables.Counter;
                    parameters[6] = new SqlParameter("@kdv", SqlDbType.Int);
                    parameters[6].Value = item.StokKDV;
                    parameters[7] = new SqlParameter("@depoKodu", SqlDbType.Int);
                    parameters[7].Value = item.DepoKodu;
                    parameters[8] = new SqlParameter("@vade", SqlDbType.Int);
                    parameters[8].Value = item.Vade;
                    parameters[9] = new SqlParameter("@fisno", SqlDbType.NVarChar,15);
                    parameters[9].Value = fisno;

                    variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vapInsertSiparisSatir", variables.Yil, parameters, fabrika);

                    if (!variables.Result)
                        return false;

                    variables.Counter ++;
                }

                return true;
            }
            catch 
            {
                return false;
            }
        }
        public bool DeleteSiparisGeriAl(string fisno, string fabrika)
        {
            try
            {
                variables.Query = "delete from tblsipamas where fisno = @fisno delete from tblsipatra where fisno = @fisno delete from tblfatuek where fisno=@fisno";

                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@fisno",SqlDbType.NVarChar,15);
                parameter[0].Value = fisno;

                variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameter, fabrika);
                if(!variables.Result) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
        public ObservableCollection<Cls_Siparis> GetSiparisGenelInfo(string fisno)
		{
			try
            {
				variables.Query = "SELECT * FROM vbvSiparisGenelBilgileri WHERE FATIRS_NO LIKE '%' + @fisno + '%'";


				SqlParameter[] parameterArray = new SqlParameter[1]; 

                parameterArray[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 35);
                parameterArray[0].Value = fisno;

                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameterArray))
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {

							login.UserID = login.GetUserIDFromPortalID(Convert.ToInt32(reader["kullaniciKodu"]));
							login.User = login.GetUserNameFromUserID(login.UserID);

							Cls_Siparis cls_Siparis = new Cls_Siparis
                            {
                                Fisno = reader[0].ToString(),
                                UserName = login.User,
								
							};

                            // Create an instance of AssociatedCari and populate its properties
                            cls_Siparis.AssociatedCari = new Cls_Cari
                            {
                                SatisCariKodu = reader[1].ToString(),
                                SatisCariAdi = reader[2].ToString(),
                                TeslimCariKodu = reader[3].ToString(),
                                TeslimCariAdi = reader[4].ToString(),
                            };

							cls_Siparis.SiparisTarih = !string.IsNullOrEmpty(reader["TARIH"].ToString()) ?
                                                        Convert.ToDateTime(reader["TARIH"]).ToString("dd.MM.yyyy")
                                                        : string.Empty;

                            cls_Siparis.AssociatedCari.SiparisTipi = (Cls_Base.SiparisTipi)Convert.ToInt32(reader[6]);
                            cls_Siparis.Destinasyon = reader[7].ToString();
                            cls_Siparis.SiparisDurum = reader["siparisDurum"].ToString();

                            _siparisCollection.Add(cls_Siparis);
                        }
                    }
                }

                SiparisCollection = _siparisCollection;
                return SiparisCollection;

            }
            catch { return null; }

        }
        public ObservableCollection<Cls_Siparis> GetOnayBekleyenSiparisler()
        {
            try
            {

                variables.Query = "select * from vbvOnayBekleyenSiparisler";

                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil);

                temp_coll_sip.Clear();

                foreach (DataRow row in dataTable.Rows)
                {

                    Cls_Cari cari = new Cls_Cari()
                    {
                        SatisCariAdi = row["satisCariIsim"].ToString(),
                        TeslimCariAdi = row["teslimCariIsim"].ToString(),
                        IlIlce = row["cariIlIlce"].ToString(),
                    };

                    login.UserID = login.GetUserIDFromPortalID(Convert.ToInt32(row["kullaniciKodu"]));
                    login.User = login.GetUserNameFromUserID(login.UserID);
                    Cls_Siparis cls_Siparis = new Cls_Siparis()
                    {
                        Fisno = row["fisno"].ToString(),
						SiparisTarih = !string.IsNullOrEmpty(row["tarih"].ToString()) ? 
                                        Convert.ToDateTime(row["tarih"]).ToString("dd.MM.yyyy")
					                    : string.Empty,
                        UserName = login.User,
						SiparisToplamTutar = Convert.ToDouble(row["brutTutar"]),
                        AssociatedCari = cari,
                        DovizTipi = row["dovizTipi"].ToString(),
                        DovizFiyat = Convert.ToDecimal(row["dovizTutar"]),
                    };
                    temp_coll_sip.Add(cls_Siparis);
                }

                SiparisCollection = temp_coll_sip;
                return SiparisCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Cls_Siparis> GetSiparisSatirInfo(string fisno, string talepEden)
        {
            try
            { 

                if (talepEden == "Onay")
                { 
                    variables.Query = $"select a.stra_sipkont, a.stok_kodu, b.stok_adi, a.sthar_gcmik, a.sthar_testar,a.STHAR_NF from werpsipatra a left join tblstsabit b on a.stok_kodu=b.stok_kodu where fisno='{fisno}'";
                }

                if (talepEden == "Asil")
                { 
                    variables.Query = $"select a.fisno, a.stra_sipkont, a.stok_kodu, b.stok_adi, a.sthar_gcmik, a.sthar_testar,a.sthar_htur,a.STHAR_NF from tblsipatra a left join tblstsabit b on a.stok_kodu=b.stok_kodu where fisno='{fisno}'";
                }

                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil);

                temp_coll_sip.Clear();

                foreach (DataRow row in dataTable.Rows)
                {

                    if (talepEden == "Onay")
                    {
                        Cls_Siparis cls_Siparis = new Cls_Siparis()
                        {
                            FisSira = Convert.ToInt32(row["stra_sipkont"]),
                            StokKodu = row["stok_kodu"].ToString(),
                            StokAdi = row["stok_adi"].ToString(),
                            SiparisMiktar = Convert.ToInt32(row["sthar_gcmik"]),
                            TerminTarih = Convert.ToDateTime(row["sthar_testar"]),
                            SiparisFiyat = Convert.ToDecimal(row["STHAR_NF"]),
                        };
						temp_coll_sip.Add(cls_Siparis);
					}
                
                    if(talepEden == "Asil")
                    { 
                        Cls_Siparis cls_Siparis = new Cls_Siparis()
                        {
                            Fisno = row["fisno"].ToString(),
                            FisSira = Convert.ToInt32(row["stra_sipkont"]),
                            StokKodu = row["stok_kodu"].ToString(),
                            StokAdi = row["stok_adi"].ToString(),
                            SiparisMiktar = Convert.ToInt32(row["sthar_gcmik"]),
                            TerminTarih = Convert.ToDateTime(row["sthar_testar"]),
                            SiparisDurum = row["sthar_htur"].ToString(),
                        };
						temp_coll_sip.Add(cls_Siparis);
					}
                }

                SiparisDetayCollection = temp_coll_sip;
                return SiparisDetayCollection;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }
        public ObservableCollection<Cls_Siparis> GetDuzenleOnayBekleyenSiparisler(string fisno)
        {
            try
            {

                variables.Query = $"select * from vbvOnayBekleyenSiparisler where fisno='{fisno}'";

                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil);

                temp_coll_sip.Clear();

                int DovizTipiInt = 0 , SatisTipiInt = 0;

                foreach (DataRow row in dataTable.Rows)
                {

                    Cls_Cari cari = new Cls_Cari()
                    {
                        SatisCariKodu = row["satisCariKod"].ToString(),
                        TeslimCariKodu = row["teslimCariKod"].ToString(),
                        SatisCariAdi = row["satisCariIsim"].ToString(),
                        TeslimCariAdi = row["teslimCariIsim"].ToString(),
                        SiparisTipi = (Cls_Base.SiparisTipi) Convert.ToInt32(row["siparisTipi"]),
                        DovizTipi = (Cls_Base.DovizTipi) Convert.ToInt32(row["dovizTipi"]),
                    };

                    Cls_Siparis cls_Siparis = new Cls_Siparis()
                    {
                        Fisno = row["fisno"].ToString(),
                        Destinasyon = row["destinasyon"].ToString(),
                        POnumarasi = row["POno"].ToString(),
                        SiparisAciklama = row["Aciklama"].ToString(),
                        AssociatedCari = cari,
                    };

                    temp_coll_sip.Add(cls_Siparis);
                }

                SiparisCollection = temp_coll_sip;
                return SiparisCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Cls_Siparis> GetDuzenleSiparisSatirInfo(string fisno)
        {
            try
            {
                variables.Query = $"select a.stra_sipkont, a.stok_kodu, b.stok_adi, a.sthar_gcmik, a.sthar_testar from werpsipatra a left join tblstsabit b on a.stok_kodu=b.stok_kodu where fisno='{fisno}'";
                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil);

                temp_coll_sip.Clear();

                foreach (DataRow row in dataTable.Rows)
                {


                    Cls_Siparis cls_Siparis = new Cls_Siparis()
                    {
                        FisSira = Convert.ToInt32(row["stra_sipkont"]),
                        StokKodu = row["stok_kodu"].ToString(),
                        StokAdi = row["stok_adi"].ToString(),
                        SiparisMiktar = Convert.ToInt32(row["sthar_gcmik"]),
                        TerminTarih = Convert.ToDateTime(row["sthar_testar"]),
                    };


                    temp_coll_sip.Add(cls_Siparis);
                }

                SiparisDetayCollection = temp_coll_sip;
                return SiparisDetayCollection;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }
        public string GetNextFisnoForSatisIrsaliyesi(string fabrika)
        {
            try
            {
                variables.Query = "select top 1 cast(FATIRS_NO as int) + 1 from TBLFATUIRS where FTIRSIP='3' or FTIRSIP='A' and LEFT(fatirs_no,3)='000' order by FATIRS_NO desc";

                int fisSayisi = dataLayer.Get_One_Int_Result_Command(variables.Query, variables.Yil, fabrika);
                if (fisSayisi == -1)
                    return string.Empty;

                Fisno = fisSayisi.ToString();
                Fisno = Fisno.PadLeft(15, '0');
                return Fisno;
            }
            catch 
            {
                return string.Empty;
            }
        }
        public decimal GetKdvOrani(string fisno, string stokKodu, string fabrika)  
        {
            try
            { 
                variables.Query = $"select top 1 isnull(sthar_kdv,0) from tblsipatra (nolock) where stok_kodu=@stokKodu and fisno=@fisno";

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                parameters[0].Value = stokKodu;
                parameters[1] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                parameters[1].Value = fisno;

                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameters, fabrika))
                {
                    if (reader != null && reader.HasRows)
                    {
                        reader.Read();
                        StokKDV = reader.GetDecimal(0);
                    }
                }
                return StokKDV;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return 0; }

        }
        public decimal GetKdvOrani(string fisno, string stokKodu)  
        {
            try
            { 
                variables.Query = $"select top 1 kdv_orani from tblsipatra a left join tblstsabit b on a.stok_kodu=b.stok_kodu where a.stok_kodu=@stokKodu and a.fisno=@fisno";

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                parameters[0].Value = stokKodu;
                parameters[1] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                parameters[1].Value = fisno;

                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameters))
                {
                    if (reader != null && reader.HasRows)
                    {
                        reader.Read();
                        StokKDV = reader.GetDecimal(0);
                    }
                }
                return StokKDV;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return 0; }

        }
        public string GetPrefixForSiparisStokAdi(string stokKodu, string fisNo, int fisSira, string fabrika)
        {
            try
            {
                variables.Query = "select ekalan from tblsipatra where stok_kodu=@stokKodu and fisno=@fisno and stra_sipkont=@fisSira";

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                parameters[1] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                parameters[2] = new SqlParameter("@fisSira", SqlDbType.Int);
                parameters[0].Value = stokKodu;
                parameters[1].Value = fisNo;
                parameters[2].Value = fisSira;

                string prefix = dataLayer.Get_One_String_Result_Command_With_Parameters(variables.Query,variables.Yil,parameters,fabrika);

                

                return prefix;
                    
            }
            catch
            {
                return string.Empty;
            }
        }
        public bool DeleteOnayBekleyenSiparis(string fisno)
        {

            if (string.IsNullOrEmpty(fisno)) { MessageBox.Show("Fiş No Boş Olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Stop); return false; }
            
            Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters = new Dictionary<string, (SqlDbType, int, int, object)>
            {
            
                { "@fisNo", (SqlDbType.NVarChar, 15, 0, fisno) },
            
            };


            try
            {
                variables.IsTrue = dataLayer.ExecuteStoredProcedureWithParameters("vbpOnayBekleyenSiparisSil", variables.Yil, parameters);

                return variables.IsTrue;
            }

            catch { return false; }
        }
        public bool SiparisKapatAcSatir(string fisno, int sira, bool kapat_)
        {
            if (kapat_)
            {
                variables.Query = "update tblsipatra set sthar_htur='K' where fisno=@fisno and stra_sipkont=@sira ";
            }
            if (!kapat_)
            {
                variables.Query = "update tblsipatra set sthar_htur='H' where fisno=@fisno and stra_sipkont=@sira ";
            }

            SqlParameter[] parameter = new SqlParameter[2];

            parameter[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
            parameter[0].Value = fisno;
            parameter[1] = new SqlParameter("@sira", SqlDbType.Int);
            parameter[1].Value = sira;

            try
            {
                variables.IsTrue = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil,parameter);

                return variables.IsTrue;
            }

            catch { return false; }
        }
        public bool SiparisKapatAc(string fisno,bool kapat_)
        {
            if (kapat_)
            {
                variables.Query = "update tblsipatra set sthar_htur='K' where fisno=@fisno ";
            }
            if (!kapat_)
            {
                variables.Query = "update tblsipatra set sthar_htur='H' where fisno=@fisno ";
            }

            SqlParameter[] parameter = new SqlParameter[1];

            parameter[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
            parameter[0].Value = fisno;

            try
            {
                variables.IsTrue = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil,parameter);

                return variables.IsTrue;
            }

            catch { return false; }
        }
        public bool UpdateOnayBekleyenSiparis(ObservableCollection<Cls_Siparis> siparisMas, ObservableCollection<Cls_Siparis> siparisSatir)
        { 
            
            try
            {

                variables.Counter = 0;
                decimal kumulatifSatisToplam = 0;
                decimal kdv = 0;
                decimal qumulativeKdv = 0;

                foreach(Cls_Siparis siparisItem in siparisSatir)
                {
                    kumulatifSatisToplam = kumulatifSatisToplam + (siparisItem.SiparisMiktar * siparisItem.SiparisFiyat);
                    kdv = GetKdvOrani(siparisItem.Fisno, siparisItem.StokKodu);
                    qumulativeKdv = qumulativeKdv + (siparisItem.SiparisFiyat * (kdv / 100) * siparisItem.SiparisMiktar);
                    variables.Counter++;
                }


                string satisCariKodu = siparisMas.First().AssociatedCari.SatisCariKodu;
                string teslimCariKodu = siparisMas.First().AssociatedCari.TeslimCariKodu;
                int dovizTipi = Convert.ToInt32(siparisMas.First().AssociatedCari.DovizTipi);
                int siparisTipi = Convert.ToInt32(siparisMas.First().AssociatedCari.SiparisTipi);
                string aciklama = siparisMas.First().SiparisAciklama;
                string destinasyon = siparisMas.First().Destinasyon;
                string POno = siparisMas.First().POnumarasi;
                string fisno = siparisMas.First().Fisno;
                int portalID = login.GetPortalID();
                
                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parametersMas = new Dictionary<string, (SqlDbType, int, int, object)>
                {
                    { "@satisCariKodu", (SqlDbType.NVarChar, 15, 0,  satisCariKodu)},
                    { "@dovtip", (SqlDbType.TinyInt, 0, 0, dovizTipi) },
                    { "@teslimCariKod", (SqlDbType.NVarChar, 15, 0, teslimCariKodu) },
                    { "@aciklama", (SqlDbType.NVarChar, 35, 0, aciklama) },
                    { "@satisIhracatMi", (SqlDbType.TinyInt, 0, 0, siparisTipi) },
                    { "@brutTutar", (SqlDbType.Decimal, 28, 8, kumulatifSatisToplam) },
                    { "@toplamKdv", (SqlDbType.Decimal, 28, 8, qumulativeKdv) },
                    { "@destinasyon", (SqlDbType.NVarChar, 20, 0, destinasyon) },
                    { "@toplamSiparisKalemi", (SqlDbType.SmallInt, 0, 0, variables.Counter) },
                    { "@portalID", (SqlDbType.SmallInt, 0, 0, portalID) },
                    { "@POno", (SqlDbType.NVarChar, 100, 0, POno) },
                    { "@fisno", (SqlDbType.NVarChar, 15, 0, fisno) },
                };

                    variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vbpUpdateOnayBekleyenSiparisGenel", variables.Yil, parametersMas);

                    if (!variables.Result)
                        return variables.Result;


                int maxSira = GetMaxSiraNoFromOnayBekleyen(fisno);
                    if(maxSira == 0)
                        return false;
                    
                    if(maxSira > variables.Counter)
                        variables.Result = DeleteExtraSiparisSatir(fisno,variables.Counter);

                    if (!variables.Result)
                    return variables.Result;

                variables.Counter = 0;

                foreach (Cls_Siparis siparisItem in siparisSatir)
                { 

                siparisItem.StokKDV = GetKdvOrani(siparisItem.Fisno, siparisItem.StokKodu);
                variables.Counter++;

                Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parametersSatir = new Dictionary<string, (SqlDbType, int, int, object)>
                    {
                        { "@stokKodu", (SqlDbType.NVarChar, 35, 0, siparisItem.StokKodu) },
                        { "@siparisMiktar", (SqlDbType.Int, 0, 0, siparisItem.SiparisMiktar) },
                        { "@siparisFiyat", (SqlDbType.Decimal, 18, 2, siparisItem.SiparisFiyat) },
                        { "@satisCariKodu", (SqlDbType.NVarChar, 35, 0, satisCariKodu) },
                        { "@dovtip", (SqlDbType.TinyInt, 0, 0, dovizTipi) },
                        { "@teslimCariKod", (SqlDbType.NVarChar, 15, 0, teslimCariKodu) },
                        { "@sira", (SqlDbType.SmallInt, 0, 0, variables.Counter) },
                        { "@teslimTarih", (SqlDbType.DateTime, 0, 0, string.Format(siparisItem.TerminTarih.ToString(), "yyyy-MM-dd")) },
                        { "@kdv", (SqlDbType.Decimal, 18, 2, siparisItem.StokKDV) },
                        { "@fisno", (SqlDbType.NVarChar, 15, 0, fisno) },
                    };

                        variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vbpUpdateOnayBekleyenSiparisSatir", variables.Yil, parametersSatir);

                        if (!variables.Result)
                        return variables.Result;

                }

                return variables.Result;
            }
            catch 
            {
                return false;
            }
        }
        private int GetMaxSiraNoFromOnayBekleyen(string fisno)
        {
            try
            {
                int maxSira = 0;
                variables.Query = "select max(stra_sipkont) from WERPSIPATRA where FISNO=@fisno";

                SqlParameter[] parameterArray = new SqlParameter[1]; // Initialize the array with one element

                parameterArray[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                parameterArray[0].Value = fisno;


                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameterArray))
                {
                    if (reader != null && reader.HasRows)
                    {
                        reader.Read();
                        maxSira = reader.GetInt16(0);
                    }
                }
                return maxSira;

            }
            catch
            { return 0; }
        }
        private bool DeleteExtraSiparisSatir(string fisno, int maxSira)
        {
            try
            {

                variables.Query = "delete from WERPSIPATRA where FISNO=@fisno and stra_sipkont>@maxSira";

                SqlParameter[] parameterArray = new SqlParameter[2]; // Initialize the array with two elements

                parameterArray[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                parameterArray[0].Value = fisno;
                parameterArray[1] = new SqlParameter("@maxSira", SqlDbType.Int);
                parameterArray[1].Value = maxSira;

                variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameterArray);

                if (!variables.Result)
                    return false;

                return true;

            }
            catch
            { return false; }
        }

        //irsaliye kayıt için sipariş listele
        public ObservableCollection<Cls_Siparis> PopulateSatisIrsaliyesiSiparisListele(Dictionary<string, string> constraints, string fabrika)
        {
            try
            {
                variables.Query = "SELECT * FROM vbvSatisIrsaliyesiSiparisListele where 1=1 ";

                variables.Counter = 0;


                if (!string.IsNullOrWhiteSpace(constraints["cariIsim"]))
                {
                    variables.Query = variables.Query + "and cariIsım like '%' + @cariIsim + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisNumarasi"]))
                {
                    if (constraints["siparisNumarasi"].Length > 15)
                    {
                        constraints["siparisNumarasi"] = constraints["siparisNumarasi"].Substring(0, 15);
                    }
                    variables.Query = variables.Query + "and siparisNumarasi like '%' + @siparisNumarasi + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisSira"]))
                {
                    variables.Query = variables.Query + "and siparisSira = @siparisSira ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokKodu"]))
                {
                    variables.Query = variables.Query + "and stokKodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokAdi"]))
                {
                    variables.Query = variables.Query + "and stokAdi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];

                variables.Counter = 0;

                if (!string.IsNullOrWhiteSpace(constraints["cariIsim"]))
                {
                    parameters[variables.Counter] = new("@cariIsim", SqlDbType.NVarChar, 500);
                    parameters[variables.Counter].Value = constraints["cariIsim"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisNumarasi"]))
                {
                    parameters[variables.Counter] = new("@siparisNumarasi", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = constraints["siparisNumarasi"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisSira"]))
                {
                    parameters[variables.Counter] = new("@siparisSira", SqlDbType.Int);
                    parameters[variables.Counter].Value = constraints["siparisSira"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokKodu"]))
                {
                    parameters[variables.Counter] = new("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = constraints["stokKodu"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokAdi"]))
                {
                    parameters[variables.Counter] = new("@stokAdi", SqlDbType.NVarChar, 500);
                    parameters[variables.Counter].Value = constraints["stokAdi"];

                    variables.Counter++;
                }
                string cariKodu = string.Empty;
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,fabrika))

                {
                    if (!reader.HasRows) { return null; }
                    temp_coll_sip.Clear();

                    while (reader.Read())
                    {
                        // Create an instance of the ViewModel and populate it from the DataRow
                        Cls_Siparis siparis = new Cls_Siparis
                        {
                            Fisno = reader["siparisNumarasi"].ToString(),
                            FisSira = Convert.ToInt32(reader["siparisSira"]),
                            StokKodu = reader["stokKodu"].ToString(),
                            StokAdi = reader["stokAdi"].ToString(),
                            SiparisMiktar = Convert.ToInt32(reader["siparisMiktar"]),
                            SiparisTeslimMiktar = Convert.ToInt32(reader["teslimMiktar"]),
                            SiparisBakiye = Convert.ToInt32(reader["bakiye"]),
                            DepoKodu = Convert.ToInt32(reader["depoKodu"]),
                            SiparisTarih = reader["siparisTarih"].ToString().Substring(0,11),
                            SiparisFiyat = Convert.ToDecimal(reader["siparisFiyat"]),
                        };
                        siparis.AssociatedCari.TeslimCariKodu = reader["cariKodu"].ToString();
                        siparis.AssociatedCari.TeslimCariAdi = reader["cariIsim"].ToString();
                        temp_coll_sip.Add(siparis);
                    }
                }
                SiparisCollection = temp_coll_sip;
                return SiparisCollection;
            }
            catch { return null; }

        }
        public ObservableCollection<Cls_Siparis> PopulateTedarikciSiparisListele(Dictionary<string, string> constraints, string fabrika)
        {
            try
            {
                variables.Query = "SELECT * FROM vbvTedarikciSiparisListele where 1=1 ";

                variables.Counter = 0;


                if (!string.IsNullOrWhiteSpace(constraints["cariIsim"]))
                {
                    variables.Query = variables.Query + "and cariIsim like '%' + @cariIsim + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisNumarasi"]))
                {
                    if (constraints["siparisNumarasi"].Length > 15)
                    {
                        constraints["siparisNumarasi"] = constraints["siparisNumarasi"].Substring(0, 15);
                    }
                    variables.Query = variables.Query + "and siparisNumarasi like '%' + @siparisNumarasi + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisSira"]))
                {
                    variables.Query = variables.Query + "and siparisSira = @siparisSira ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokKodu"]))
                {
                    variables.Query = variables.Query + "and stokKodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokAdi"]))
                {
                    variables.Query = variables.Query + "and stokAdi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];

                variables.Counter = 0;

                if (!string.IsNullOrWhiteSpace(constraints["cariIsim"]))
                {
                    parameters[variables.Counter] = new("@cariIsim", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = constraints["cariIsim"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisNumarasi"]))
                {
                    parameters[variables.Counter] = new("@siparisNumarasi", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = constraints["siparisNumarasi"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisSira"]))
                {
                    parameters[variables.Counter] = new("@siparisSira", SqlDbType.Int);
                    parameters[variables.Counter].Value = constraints["siparisSira"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokKodu"]))
                {
                    parameters[variables.Counter] = new("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = constraints["stokKodu"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokAdi"]))
                {
                    parameters[variables.Counter] = new("@stokAdi", SqlDbType.NVarChar, 500);
                    parameters[variables.Counter].Value = constraints["stokAdi"];

                    variables.Counter++;
                }
                string cariKodu = string.Empty;
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,fabrika))

                {
                    if (!reader.HasRows) { return null; }
                    temp_coll_sip.Clear();

                    while (reader.Read())
                    {
                        // Create an instance of the ViewModel and populate it from the DataRow
                        Cls_Siparis siparis = new Cls_Siparis
                        {
                            Fisno = reader["siparisNumarasi"].ToString(),
                            FisSira = Convert.ToInt32(reader["siparisSira"]),
                            StokKodu = reader["stokKodu"].ToString(),
                            StokAdi = reader["stokAdi"].ToString(),
                            TedarikSiparisMiktar = Convert.ToDecimal(reader["siparisMiktar"]),
                            TedarikTeslimMiktar = Convert.ToDecimal(reader["teslimMiktar"]),
                            TedarikSiparisBakiye = Convert.ToDecimal(reader["bakiye"]),
                            DepoKodu = Convert.ToInt32(reader["depoKodu"]),
                            SiparisTarih = reader["siparisTarih"].ToString().Substring(0,11),
                            SiparisFiyat = Convert.ToDecimal(reader["siparisFiyat"]),
                        };
                        cariKodu = reader["cariKodu"].ToString();
                        siparis.AssociatedCari.TedarikciCariKodu = cariKodu;
                        temp_coll_sip.Add(siparis);
                    }
                }
                SiparisCollection = temp_coll_sip;
                return SiparisCollection;
            }
            catch { return null; }

        }
        public ObservableCollection<Cls_Siparis> PopulateTedarikciSiparisListele(Dictionary<string, string> constraints)
        {
            try
            {
                variables.Query = "SELECT * FROM vbvTedarikciSiparisListele where 1=1 ";

                variables.Counter = 0;


                if (!string.IsNullOrWhiteSpace(constraints["cariKodu"]))
                {
                    variables.Query = variables.Query + "and cariKodu like '%' + @cariKodu + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisNumarasi"]))
                {
                    if (constraints["siparisNumarasi"].Length > 15)
                    {
                        constraints["siparisNumarasi"] = constraints["siparisNumarasi"].Substring(0, 15);
                    }
                    variables.Query = variables.Query + "and siparisNumarasi like '%' + @siparisNumarasi + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisSira"]))
                {
                    variables.Query = variables.Query + "and siparisSira = @siparisSira ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokKodu"]))
                {
                    variables.Query = variables.Query + "and stokKodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokAdi"]))
                {
                    variables.Query = variables.Query + "and stokAdi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];

                variables.Counter = 0;

                if (!string.IsNullOrWhiteSpace(constraints["cariKodu"]))
                {
                    parameters[variables.Counter] = new("@cariKodu", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = constraints["cariKodu"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisNumarasi"]))
                {
                    parameters[variables.Counter] = new("@siparisNumarasi", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = constraints["siparisNumarasi"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["siparisSira"]))
                {
                    parameters[variables.Counter] = new("@siparisSira", SqlDbType.Int);
                    parameters[variables.Counter].Value = constraints["siparisSira"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokKodu"]))
                {
                    parameters[variables.Counter] = new("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = constraints["stokKodu"];

                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["stokAdi"]))
                {
                    parameters[variables.Counter] = new("@stokAdi", SqlDbType.NVarChar, 500);
                    parameters[variables.Counter].Value = constraints["stokAdi"];

                    variables.Counter++;
                }
                string cariKodu = string.Empty;
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))

                {
                    if (!reader.HasRows) { return null; }
                    temp_coll_sip.Clear();

                    while (reader.Read())
                    {
                        // Create an instance of the ViewModel and populate it from the DataRow
                        Cls_Siparis siparis = new Cls_Siparis
                        {
                            Fisno = reader["siparisNumarasi"].ToString(),
                            FisSira = Convert.ToInt32(reader["siparisSira"]),
                            StokKodu = reader["stokKodu"].ToString(),
                            StokAdi = reader["stokAdi"].ToString(),
                            TedarikSiparisMiktar = Convert.ToDecimal(reader["siparisMiktar"]),
                            TedarikTeslimMiktar = Convert.ToDecimal(reader["teslimMiktar"]),
                            TedarikSiparisBakiye = Convert.ToDecimal(reader["bakiye"]),
                            DepoKodu = Convert.ToInt32(reader["depoKodu"]),
                            SiparisTarih = reader["siparisTarih"].ToString().Substring(0,11),
                            SiparisFiyat = Convert.ToDecimal(reader["siparisFiyat"]),
                            TedarikGirisMiktar = Convert.ToDecimal(reader["bakiye"]),
                        };
                        cariKodu = reader["cariKodu"].ToString();
                        siparis.AssociatedCari.TedarikciCariKodu = cariKodu;
                        temp_coll_sip.Add(siparis);
                    }
                }
                SiparisCollection = temp_coll_sip;
                return SiparisCollection;
            }
            catch { return null; }

        }

        public ObservableCollection<Cls_Siparis> PopulateUrunAgaciOlmayanSiparisler()
        { 
           
            try 
	        {
                ObservableCollection<Cls_Siparis> UrunAgaciOlmayanSiparislerCollection = new();
                variables.Query = "Select * from vbvUrunAgaciOlmayanSiparisler";
                string cariKodu = string.Empty;
                string cariAdi = string.Empty;

                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.Yil, variables.Fabrika))

                {
                    if (!reader.HasRows) { return null; }
                    temp_coll_sip.Clear();

                    while (reader.Read())
                    {
                        // Create an instance of the ViewModel and populate it from the DataRow
                        Cls_Siparis siparis = new Cls_Siparis
                        {
                            Fisno = reader["siparisNumarasi"].ToString(),
                            FisSira = Convert.ToInt32(reader["siparisSira"]),
                            StokKodu = reader["stokKodu"].ToString(),
                            StokAdi = reader["stokAdi"].ToString(),
                            SiparisTarih = reader["siparisTarih"].ToString().Substring(0, 11),
                            TerminTarih = Convert.ToDateTime(reader["teslimTarih"]),
                        };
                        cariKodu = reader["cariKodu"].ToString();
                        cariAdi = reader["cariAdi"].ToString();
                        siparis.AssociatedCari.TeslimCariKodu = cariKodu;
                        siparis.AssociatedCari.TeslimCariAdi = cariAdi;
                        temp_coll_sip.Add(siparis);
                    }
                }
                UrunAgaciOlmayanSiparislerCollection = temp_coll_sip;
                return UrunAgaciOlmayanSiparislerCollection;
            }
            catch 
	        {
                return null;
            }
        }
        public int CheckIfIrsaliyeNoExists(string irsaliyeNumarasi,string fabrika)
        {
            try
            {
                variables.Query = "select count(*) from tblsthar where fisno = @irsaliyeNumarasi";

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@irsaliyeNumarasi", SqlDbType.NVarChar, 15);
                parameters[0].Value = irsaliyeNumarasi;
                
                variables.ResultInt = dataLayer.Get_One_Int_Result_Command_With_Parameters(variables.Query, variables.Yil, parameters, fabrika);
                return variables.ResultInt;


            }
            catch
            {
                return -1;
            }
        }
        public bool GetFarkliSiparislerIcinVadeVeDovizTipi(string itemFisno,string collectionFisno,string fabrika)
        {
            try
            {
                variables.Query =   "with cte as (" +
                                    "select odemegunu,DOVIZTIP,tipi from  tblsipamas (nolock) where fatirs_no =@itemFisno " +
                                    "union " +
                                    "select odemegunu, doviztip,tipi from tblsipamas (nolock) where fatirs_no =@collectionFisno) " +
                                    "select count(*) from cte";

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@itemFisno", SqlDbType.NVarChar, 15);
                parameters[0].Value = itemFisno;
                parameters[1] = new SqlParameter("@collectionFisno", SqlDbType.NVarChar, 15);
                parameters[1].Value = collectionFisno;
                variables.ResultInt = dataLayer.Get_One_Int_Result_Command_With_Parameters(variables.Query, variables.Yil, parameters, fabrika);

                if (variables.ResultInt != 1) 
                    return false;
                
                return true;

            }       
            catch 
            {
                return false;
            }
        }
        public bool IrsaliyeGeriAl(string irsaliyeNumarasi, string fabrika)
        {
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@irsaliyeNumarasi", SqlDbType.NVarChar, 15);
                parameter[0].Value = irsaliyeNumarasi;

                variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vbpIrsaliyeGeriAl",variables.Yil,parameter,fabrika);
                return variables.Result;

            }
            catch
            {
                return false;
            }
        }
        public bool SatisIrsaliyesiGeriAl(string irsaliyeNumarasi, string fabrika)
        {
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@irsaliyeNumarasi", SqlDbType.NVarChar, 15);
                parameter[0].Value = irsaliyeNumarasi;

                variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vbpSatisIrsaliyesiGeriAl", variables.Yil,parameter,fabrika);
                return variables.Result;

            }
            catch
            {
                return false;
            }
        }
        public bool IrsaliyeGeriAl(string irsaliyeNumarasi)
        {
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@irsaliyeNumarasi", SqlDbType.NVarChar, 15);
                parameter[0].Value = irsaliyeNumarasi;

                variables.Result = dataLayer.ExecuteStoredProcedureWithParameters("vbpIrsaliyeGeriAl",variables.Yil,parameter);
                return variables.Result;

            }
            catch
            {
                return false;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string getStr)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(getStr));
        }

    }
}
