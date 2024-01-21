using Layer_2_Common.Type;
using Layer_Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;

namespace Layer_Business
{
    public class cls_Irsaliye :INotifyPropertyChanged
    {
        public int SiraNo { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string MalinCinsiRengi { get; set; }
        public int Miktar { get; set; }
        public string Birim { get; set; }

        private string _isChecked;

        private string faturaNo;

        public string FaturNo
        {
            get { return faturaNo; }
            set { faturaNo = value; 
                OnPropertyChanged(nameof(FaturNo));
                }
        }

        private string tarihFatura;

        public string TarihFatura
        {
            get { return tarihFatura; }
            set {
                tarihFatura = value; 
                OnPropertyChanged(nameof(TarihFatura));
                }
        }


        private string irsaliyeNo;

        public string IrsaliyeNo
        {
            get { return irsaliyeNo; }
            set
            {
                irsaliyeNo = value;
                OnPropertyChanged(nameof(IrsaliyeNo));
            }
        }

        public string IsChecked
        {
            get { return _isChecked; }
            set { 
                _isChecked = value; 
                OnPropertyChanged(nameof(IsChecked));
                }
        }

        private string _sirketAdi;

        public string SirketAdi
        {
            get { return _sirketAdi; }
            set
            {
                _sirketAdi = value;
                OnPropertyChanged(nameof(SirketAdi));
            }
        }

        private string _cariKodu;

        public string CariKodu
        {
            get { return _cariKodu; }
            set {
                _cariKodu = value;
                OnPropertyChanged(nameof(CariKodu));
                }
        }

        private string _adres;
        public string Adres
        {
            get { return _adres; }
            set
            {
                _adres = value;
                OnPropertyChanged(nameof(Adres));
            }
        }
        private string _adres2;
        public string Adres2
        {
            get { return _adres2; }
            set
            {
                _adres = value;
                OnPropertyChanged(nameof(Adres2));
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string tel;
        public string Tel
        {
            get { return tel; }
            set
            {
                tel = value;
                OnPropertyChanged(nameof(Tel));
            }
        }

        private string vergiNo;
        public string VergiNo
        {
            get { return vergiNo; }
            set
            {
                vergiNo = value;
                OnPropertyChanged(nameof(VergiNo));
            }
        }

        public string SeriNo { get; set; }
        public string SevkIrsaliyesi { get; set; }
        public string TanzimTarihi { get; set; }
        public string FiiliSevkTarihi { get; set; }

        private string sevkUlkesi;
        public string SevkUlkesi
        {
            get { return sevkUlkesi; }
            set
            {
                sevkUlkesi = value;
                OnPropertyChanged(nameof(SevkUlkesi));
            }
        }

        public string SoforIsım { get; set; }
        public string AracPlaka { get; set; }   
        public string AracPlaka2 { get; set; }

        public string SoforTel { get; set; }
        public string SoforTC { get; set; }
        public string KonteynerNo { get; set; }
        public string Muhur { get; set; }
        public string TeslimatLiman { get; set; }
        public string GemiAdiSeferi { get; set; }
        public string YuklemeLimani { get; set; }



        private ObservableCollection<cls_Irsaliye> _ırsaliyeCollection;
        public ObservableCollection<cls_Irsaliye> IrsaliyeCollection
        {
            get { return _ırsaliyeCollection; }
            set { _ırsaliyeCollection = value; }
        }
        ObservableCollection<cls_Irsaliye> coll_irsaliye = new ObservableCollection<cls_Irsaliye>();
        DataLayer dataLayer = new ();
        DataTable dataTable = new ();
        Variables variables = new ();
        DataRow dataRow { get; set; }
        int counter = 0;

        public event PropertyChangedEventHandler PropertyChanged;


        public ObservableCollection<cls_Irsaliye> populateIrsaliyeView(string irsaliyeNo) 
        {
            try
            { 
            dataTable = dataLayer.Select_Stored_Proc_LTD("vbpIrsaliyeDuzenle","@yil","@irsaliyeNo", variables.Yil, irsaliyeNo,variables.Yil);
            DataRow CariRow = dataTable.Rows[0];
            CariKodu = CariRow["cari_kodu"].ToString();
            foreach (DataRow row in dataTable.Rows)
            {
                counter++;
                // Create an instance of the ViewModel and populate it from the DataRow
                cls_Irsaliye cls_irsaliye = new cls_Irsaliye
                {
                    SiraNo = counter,
                    UrunKodu = row["urun_kodu"].ToString(),
                    UrunAdi = row["model_isim"].ToString(),
                    MalinCinsiRengi = row["stok_adi"].ToString(),
                    Miktar = Convert.ToInt32(row["miktar"]),
                    Birim = row["dovtip"].ToString()
                };
           
                coll_irsaliye.Add(cls_irsaliye);
            }

           

            IrsaliyeCollection = coll_irsaliye;
            OnPropertyChanged(nameof(IrsaliyeCollection));
            OnPropertyChanged(nameof(CariKodu));
            return IrsaliyeCollection;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return null; }
        }
        public void getCariInfo() 
        {
            variables.Query = $"select top 1 * from vbvIrsaliyeGiris where CARI_KOD='{CariKodu}'";
            dataTable = dataLayer.Select_CommandLtd(variables.Query, variables.Yil);
            if (dataTable.Rows.Count > 0)
            {
            DataRow row = dataTable.Rows[0];
                // Create an instance of the ViewModel and populate it from the DataRow

                SirketAdi = row[0].ToString();
                Adres = row[1].ToString();
                Adres2 = row[2].ToString();
                Email = row[3].ToString();
                Tel = row[4].ToString();
                VergiNo = row[5].ToString();
                SevkUlkesi = row[6].ToString();
                

                OnPropertyChanged(nameof(SirketAdi));
                OnPropertyChanged(nameof(Adres));
                OnPropertyChanged(nameof(Adres2));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Tel));
                OnPropertyChanged(nameof(VergiNo));
                OnPropertyChanged(nameof(SevkUlkesi));
            }
            else {
                MessageBox.Show("Cari Bilgisi Sistemde Mevcut Değil.");
                 }
            
        }

        protected void OnPropertyChanged(string getStr)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(getStr));
        }
    }
}
