using Layer_2_Common.Type;
using Layer_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;

namespace Layer_Business
{
    public class Cls_Cari : Cls_Base
    {
        private string _satisCariKodu = "120A100104";

        public string SatisCariKodu 
        {
            get { return _satisCariKodu; }
            set
            {
                _satisCariKodu = value;
                OnPropertyChanged(nameof(SatisCariKodu));
            }
        }

        private string _satisCariAdi = "VITA BIANCA MOB.TEKS.INS.ITH.IHR.PAZ.SAN.VE TIC.LTD.STI.";

        public string SatisCariAdi
        {
            get { return _satisCariAdi; }
            set
            {
                _satisCariAdi = value;
                OnPropertyChanged(nameof(SatisCariAdi));
            }
        }

        private string _teslimCariKodu = string.Empty;

        public string TeslimCariKodu
        {
            get => _teslimCariKodu;
            set
            {
                _teslimCariKodu = value;
                OnPropertyChanged(nameof(TeslimCariKodu));
            }
        }

        private string _teslimCariAdi = string.Empty;
        public string TeslimCariAdi
        {
            get => _teslimCariAdi;
            set
            {
                _teslimCariAdi = value;
                OnPropertyChanged(nameof(TeslimCariAdi));
            }
        }

        private string _tedarikciCariKodu = string.Empty;

        public string TedarikciCariKodu
        {
            get => _tedarikciCariKodu;
            set
            {
                _tedarikciCariKodu = value;
                OnPropertyChanged(nameof(TedarikciCariKodu));
            }
        }

        private string _tedarikciCariAdi = string.Empty;
        public string TedarikciCariAdi
        {
            get => _tedarikciCariAdi;
            set
            {
                _tedarikciCariAdi = value;
                OnPropertyChanged(nameof(TedarikciCariAdi));
            }
        }

        public string IrsaliyeNumarasi { get; set; }

        private DovizTipi _dovizTipi = DovizTipi.USD;

        public new DovizTipi DovizTipi
        {
            get { return _dovizTipi; }
            set
            {
                _dovizTipi = value;
                OnPropertyChanged(nameof(DovizTipi));
            }
        }

        private SiparisTipi _siparisTipi = SiparisTipi.Yurtdisi;

        public new SiparisTipi SiparisTipi
        {
            get { return _siparisTipi; }
            set
            {
                _siparisTipi = value;
                OnPropertyChanged(nameof(SiparisTipi));
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        private string _ulkeKodu;

        public string UlkeKodu
        {
            get { return _ulkeKodu; }
            set
            {
                _ulkeKodu = value;
                OnPropertyChanged(nameof(UlkeKodu));
            }
        }
        private string _ilIlce;

        public string IlIlce
        {
            get { return _ilIlce; }
            set
            {
                _ilIlce = value;
                OnPropertyChanged(nameof(IlIlce));
            }
        }

        private ObservableCollection<Cls_Cari> _sipariseCariBaglaCollection;
        public ObservableCollection<Cls_Cari> SipariseCariBaglaCollection
        {
            get { return _sipariseCariBaglaCollection; }
            set { _sipariseCariBaglaCollection = value; }
        }
        ObservableCollection<Cls_Cari> coll_cari = new ObservableCollection<Cls_Cari>();
        DataLayer dataLayer = new DataLayer();
        DataTable dataTable = new DataTable();
        
        Variables variables = new Variables();
        public ObservableCollection<Cls_Cari> PopulateSipariseCariBaglaTeslimCari(string teslimCariKodu, string teslimCariAdi, string fabrika)
        {
            try
            {
                TeslimCariKodu = teslimCariKodu;
                TeslimCariAdi = teslimCariAdi;

                variables.Query = "select CARI_KOD,CARI_ISIM,ULKE_KODU FROM TBLCASABIT WHERE 1=1 ";

                if (string.IsNullOrEmpty(TeslimCariKodu) == false) { variables.Query = variables.Query + "AND CARI_KOD like '%" + TeslimCariKodu + "%' "; }

                if (string.IsNullOrEmpty(TeslimCariAdi) == false) { variables.Query = variables.Query + "AND CARI_ISIM like '%" + TeslimCariAdi + "%' "; }

                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil,fabrika);
             
                coll_cari.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Cari cls_cari= new Cls_Cari
                    {
                        IsChecked = false,
                        TeslimCariKodu = row["CARI_KOD"].ToString(),
                        TeslimCariAdi = row["CARI_ISIM"].ToString(),
                        UlkeKodu = row["ULKE_KODU"].ToString()
                    };
                    coll_cari.Add(cls_cari);
                }

                SipariseCariBaglaCollection = coll_cari;

                return SipariseCariBaglaCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Cls_Cari> PopulateSipariseCariBaglaTeslimCari(string teslimCariKodu, string teslimCariAdi)
        {
            try
            {
                TeslimCariKodu = teslimCariKodu;
                TeslimCariAdi = teslimCariAdi;

                variables.Query = "select CARI_KOD,CARI_ISIM,ULKE_KODU FROM TBLCASABIT WHERE 1=1 ";

                if (string.IsNullOrEmpty(TeslimCariKodu) == false) { variables.Query = variables.Query + "AND CARI_KOD like '%" + TeslimCariKodu + "%' "; }

                if (string.IsNullOrEmpty(TeslimCariAdi) == false) { variables.Query = variables.Query + "AND CARI_ISIM like '%" + TeslimCariAdi + "%' "; }

                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil);
             
                coll_cari.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Cari cls_cari= new Cls_Cari
                    {
                        IsChecked = false,
                        TeslimCariKodu = row["CARI_KOD"].ToString(),
                        TeslimCariAdi = row["CARI_ISIM"].ToString(),
                        UlkeKodu = row["ULKE_KODU"].ToString()
                    };
                    coll_cari.Add(cls_cari);
                }

                SipariseCariBaglaCollection = coll_cari;

                return SipariseCariBaglaCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Cls_Cari> PopulateSipariseCariBaglaSatisCari(string satisCariKodu, string satisCariAdi)
        {
            try
            {
                SatisCariKodu = satisCariKodu;
                SatisCariAdi = satisCariAdi;

                variables.Query = "select CARI_KOD,CARI_ISIM,ULKE_KODU FROM TBLCASABIT WHERE 1=1 ";

                if (string.IsNullOrEmpty(SatisCariKodu) == false) { variables.Query = variables.Query + "AND CARI_KOD like '%" + SatisCariKodu + "%' "; }

                if (string.IsNullOrEmpty(SatisCariAdi) == false) { variables.Query = variables.Query + "AND CARI_ISIM like '%" + SatisCariAdi + "%' "; }

                
                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil);
              
                coll_cari.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Cari cls_cari= new Cls_Cari
                    {
                        IsChecked = false,
                        SatisCariKodu = row["CARI_KOD"].ToString(),
                        SatisCariAdi = row["CARI_ISIM"].ToString(),
                        UlkeKodu = row["ULKE_KODU"].ToString()
                    };
                    coll_cari.Add(cls_cari);
                }

                SipariseCariBaglaCollection = coll_cari;

                return SipariseCariBaglaCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Cls_Cari> PopulateCariBilgileriGuncelle()
        {
            try
            {

                variables.Query = "select CARI_KOD,CARI_ISIM FROM TBLCASABIT WHERE 1=1 ";

                if (string.IsNullOrEmpty(SatisCariKodu) == false) { variables.Query = variables.Query + "AND CARI_KOD like '%" + SatisCariKodu + "%' "; }

                if (string.IsNullOrEmpty(SatisCariAdi) == false) { variables.Query = variables.Query + "AND CARI_ISIM like '%" + SatisCariAdi + "%' "; }


                dataTable = dataLayer.Select_Command(variables.Query, variables.Yil);

                coll_cari.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Cari cls_cari = new Cls_Cari
                    {
                        IsChecked = false,
                        SatisCariKodu = row["CARI_KOD"].ToString(),
                        SatisCariAdi = row["CARI_ISIM"].ToString(),
                        TeslimCariKodu = row["CARI_KOD"].ToString(),
                        TeslimCariAdi = row["CARI_ISIM"].ToString(),
                    };
                    coll_cari.Add(cls_cari);
                }

                SipariseCariBaglaCollection = coll_cari;

                return SipariseCariBaglaCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Cls_Cari> GetTedarikciCariInfo(string irsaliyeNumarasi, string fabrika)
        {
            try
            {
                variables.Query = "select top 1 a.cari_kodu,cari_isim,fatirs_no from tblfatuirs a (nolock) " +
                                  " left join tblcasabit b (nolock) on a.cari_kodu = b.cari_kod " +
                                  " where a.fatirs_no like '%' + @irsaliyeNumarasi + '%' AND FTIRSIP='4' " +
                                  " order by TARIH desc";
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@irsaliyeNumarasi", SqlDbType.NVarChar, 15);
                parameter[0].Value = irsaliyeNumarasi.Substring(0,3);
                ObservableCollection<Cls_Cari> coll_temp = new();
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter, fabrika))
                {
                    if (reader == null)
                    {
                        TedarikciCariKodu = "Kayıt Yok";
                        TedarikciCariAdi = "Kayıt Yok";
                        IrsaliyeNumarasi = "";
                    }
                    while (reader.Read())
                    {
                        Cls_Cari cari = new Cls_Cari()
                        {
                            TedarikciCariKodu = reader[0].ToString(),
                            TedarikciCariAdi = reader[1].ToString(),
                            IrsaliyeNumarasi = reader[2].ToString(),
                        };
                        coll_temp.Add(cari);
                    }
                }
                return coll_temp;
            }
            catch 
            {
                return null;
            }
        }
        public ObservableCollection<Cls_Cari> GetTedarikciCariInfo(string irsaliyeNumarasi)
        {
            try
            {
                variables.Query = "select top 1 a.cari_kodu,cari_isim,fatirs_no from tblfatuirs a (nolock) " +
                                  " left join tblcasabit b (nolock) on a.cari_kodu = b.cari_kod " +
                                  " where a.fatirs_no like '%' + @irsaliyeNumarasi + '%' AND FTIRSIP='4' " +
                                  " order by TARIH desc";
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@irsaliyeNumarasi", SqlDbType.NVarChar, 15);
                parameter[0].Value = irsaliyeNumarasi.Substring(0,3);
                ObservableCollection<Cls_Cari> coll_temp = new();
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter))
                {
                    if (reader == null)
                    {
                        TedarikciCariKodu = "Kayıt Yok";
                        TedarikciCariAdi = "Kayıt Yok";
                        IrsaliyeNumarasi = "";
                    }
                    while (reader.Read())
                    {
                        Cls_Cari cari = new Cls_Cari()
                        {
                            TedarikciCariKodu = reader[0].ToString(),
                            TedarikciCariAdi = reader[1].ToString(),
                            IrsaliyeNumarasi = reader[2].ToString(),
                        };
                        coll_temp.Add(cari);
                    }
                }
                return coll_temp;
            }
            catch 
            {
                return null;
            }
        }
        public static DovizTipi GetDovizTipi(string dovizTipi)
        {
            switch (dovizTipi) 
            {
                case "USD":
                    return DovizTipi.USD;
                case "TRY":
                    return DovizTipi.TL;
                case "EUR":
                    return DovizTipi.EUR;
                case "GBP":
                    return DovizTipi.GBP;
                default:
                    return DovizTipi.USD;

            }
        }
        public static SiparisTipi GetSiparisTipi(string siparisTipi)
        {
            switch (siparisTipi) 
            {
                case "Yurt Dışı":
                    return SiparisTipi.Yurtdisi;
                case "Yurt İçi":
                    return SiparisTipi.Yurtici;
                default:
                    return SiparisTipi.Yurtdisi;

            }
        }

        public string GetIrsaliyeNoFromCari(string cariKodu)
        {
            try
            {
                variables.Query = "Select top 1 fatirs_no from tblfatuirs where cari_kodu=@cariKodu and FTIRSIP='4' order by tarih desc";

                SqlParameter[] parameter = new SqlParameter[1];

                parameter[0] = new SqlParameter("@cariKodu", SqlDbType.NVarChar, 15);
                parameter[0].Value = cariKodu;

                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query,variables.Yil,parameter)) 
                {
                    if (reader == null)
                        return null;
                    while (reader.Read()) 
                    {
                        IrsaliyeNumarasi = reader[0].ToString();
                    }
                }
                return IrsaliyeNumarasi;

            }
            catch 
            {
                return null;
            }
        }
        public string GetIrsaliyeNoFromCari(string cariKodu, string fabrika)
        {
            try
            {
                variables.Query = "Select top 1 fatirs_no from tblfatuirs where cari_kodu=@cariKodu and FTIRSIP='4' order by tarih desc";

                SqlParameter[] parameter = new SqlParameter[1];

                parameter[0] = new SqlParameter("@cariKodu", SqlDbType.NVarChar, 15);
                parameter[0].Value = cariKodu;

                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query,variables.Yil,parameter,fabrika)) 
                {
                    if (reader == null)
                        return null;
                    while (reader.Read()) 
                    {
                        IrsaliyeNumarasi = reader[0].ToString();
                    }
                }
                return IrsaliyeNumarasi;

            }
            catch 
            {
                return null;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string getStr)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(getStr));
        }
    }
}
