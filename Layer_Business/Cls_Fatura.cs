using Layer_2_Common.Excels;
using Layer_2_Common.Type;
using Layer_Data;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;

namespace Layer_Business
{
    public class Cls_Fatura
    {


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
                _adres2 = value;
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

        private string faturaNo;

        public string FaturaNo
        {
            get { return faturaNo; }
            set
            {
                faturaNo = value;
                OnPropertyChanged(nameof(FaturaNo));
            }
        }

        private string tarihFatura;

        public string TarihFatura
        {
            get { return tarihFatura; }
            set
            {
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

        private int _paketAdet = 0;

        public int PaketAdet
        {
            get { return _paketAdet; }
            set { _paketAdet = value; }
        }
        private int _paketToplam = 0;  // sipariş miktarı ile paket adedin çarpımı tek bir satır için packign listte kullanmak maksatlı

        public int PaketToplam
        {
            get { return _paketToplam; }
            set
            {
                _paketToplam = value;
                OnPropertyChanged(nameof(PaketToplam));
            }
        }


        public string UrunBirim { get; set; } = "PCS";
        public int SiraNo { get; set; }
        public int AdetTakim { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string MalinCinsiRengi { get; set; }

        private string gonderimSekil = "CFR";

        public string GonderimSekil
        {
            get { return gonderimSekil; }
            set
            {
                gonderimSekil = value;
                OnPropertyChanged(nameof(gonderimSekil));

            }
        }

        public double BirimTutar { get; set; }
        public string Birim { get; set; }

        private double toplamTutar = 0;

        public double ToplamTutar
        {
            get { return toplamTutar; }
            set
            {
                toplamTutar = value;
                OnPropertyChanged(nameof(ToplamTutar));
            }
        }

        private string color;

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        private string descriptionOfGoods;

        public string DescriptionOfGoods
        {
            get { return descriptionOfGoods; }
            set
            {
                descriptionOfGoods = value;
                OnPropertyChanged(nameof(DescriptionOfGoods));
            }
        }


        private string _isChecked;
        public string IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }


        private string toplamPaket; // gönderinin toplam kaç paketten oluştuğunu gösterir fatura için

        public string ToplamPaket
        {
            get { return toplamPaket; }
            set
            {
                toplamPaket = value;
                OnPropertyChanged(nameof(ToplamPaket));
            }
        }

        private string brutNet;

        public string BrutNet
        {
            get { return brutNet; }
            set
            {
                brutNet = value;
                OnPropertyChanged(nameof(BrutNet));
            }
        }
        private double brut = 0;

        public double Brut
        {
            get { return brut; }
            set
            {
                brut = value;
                OnPropertyChanged(nameof(Brut));
            }
        }
        private double net = 0;

        public double Net
        {
            get { return net; }
            set
            {
                net = value;
                OnPropertyChanged(nameof(net));
            }
        }

        private string odemeSekil = "CFR";

        public string OdemeSekil
        {
            get { return odemeSekil; }
            set
            {
                odemeSekil = value;
                OnPropertyChanged(nameof(OdemeSekil));
            }
        }

        private string banka = "KUVEYTTÜRK GAZİEMİR ŞB.TR880020500009559509300101 USD";

        public string Banka
        {
            get { return banka; }
            set
            {
                banka = value;
                OnPropertyChanged(nameof(Banka));
            }
        }

        private string imalatci = "VİTA MOBİLYA TEKS.İNŞ.İTH.İHR.PAZ.SAN.VE TİC.A.Ş. - MENDERES V.D. 9250493053";

        public string Imalatci
        {
            get { return imalatci; }
            set
            {
                imalatci = value;
                OnPropertyChanged(nameof(Imalatci));
            }
        }

        private double kapAdet = 0;

        public double KapAdet
        {
            get { return kapAdet; }
            set
            {
                kapAdet = value;
                OnPropertyChanged(nameof(kapAdet));
            }
        }


        private string toplamTutarYalniz;

        public string ToplamTutarYalniz
        {
            get { return toplamTutarYalniz; }
            set
            {
                toplamTutarYalniz = value;
                OnPropertyChanged(nameof(ToplamTutarYalniz));
            }
        }

        private string toplamKapAdetString;

        public string ToplamKapAdetString
        {
            get { return toplamKapAdetString; }
            set
            {
                toplamKapAdetString = value;
                OnPropertyChanged(nameof(toplamKapAdetString));
            }
        }


        private string alici = "ALSSATTA";

        public string Alici
        {
            get { return alici; }
            set
            {
                alici = value;
                OnPropertyChanged(nameof(Alici));
            }
        }
        private string aliciAdres = "Zawia Refaya st. branch from Ahmed Mehdawi St. Benghazi - Libya";

        public string AliciAdres
        {
            get { return aliciAdres; }
            set
            {
                aliciAdres = value;
                OnPropertyChanged(nameof(AliciAdres));
            }
        }
        private string containerNo;

        public string ContainerNO
        {
            get { return containerNo; }
            set
            {
                containerNo = value;
                OnPropertyChanged(nameof(ContainerNO));
            }
        }
        private double navlun = 0;

        public double Navlun
        {
            get { return navlun; }
            set
            {
                navlun = value;
                OnPropertyChanged(nameof(Navlun));
            }
        }

        public string PortofDischarge { get; set; }

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

        private string toplamTutarMetin;

        public string ToplamTutarMetin
        {
            get { return toplamTutarMetin; }
            set
            {
                toplamTutarMetin = value;
                OnPropertyChanged(nameof(ToplamTutarMetin));
            }
        }

        private double grandTotal = 0;

        public double GrandTotal
        {
            get { return grandTotal; }
            set
            {
                grandTotal = value;

                OnPropertyChanged(nameof(GrandTotal));
            }
        }


        private string cariKodu;

        public string CariKodu
        {
            get { return cariKodu; }
            set
            {
                cariKodu = value;
                OnPropertyChanged(nameof(CariKodu));
            }
        }

        private ObservableCollection<Cls_Fatura> _faturaCollection;
        public ObservableCollection<Cls_Fatura> FaturaCollection
        {
            get { return _faturaCollection; }
            set { _faturaCollection = value; }
        }
        ObservableCollection<Cls_Fatura> coll_fatura = new ObservableCollection<Cls_Fatura>();
        DataLayer dataLayer = new DataLayer();
        DataTable dataTable = new DataTable();

        private int kumulatifKapAdet { get; set; } = 0;    //fatura toplam paketleri hesaplamak için


        private int _kumulatifToplamPaket = 0;  //packing list toplam paketleri hesaplamak için

        public int KumulatifToplamPaket
        {
            get { return _kumulatifToplamPaket; }
            set
            {
                _kumulatifToplamPaket = value;
                OnPropertyChanged(nameof(KumulatifToplamPaket));
            }
        }

        private int _kumulatifToplamSet;

        public int KumulatifToplamSet
        {
            get { return _kumulatifToplamSet; }
            set
            {
                _kumulatifToplamSet = value;
                OnPropertyChanged(nameof(KumulatifToplamSet));
            }
        }

        private double _kumulatifToplamAgirlik = 0;

        public double KumulatifToplamAgirlik
        {
            get { return _kumulatifToplamAgirlik; }
            set
            {
                _kumulatifToplamAgirlik = value;
                OnPropertyChanged(nameof(KumulatifToplamAgirlik));
            }
        }



        private double kumulatifBrut { get; set; }

        private decimal kumulatifToplamTutar = 0;

        public decimal KumulatifToplamTutar
        {
            get { return kumulatifToplamTutar; }
            set
            {
                kumulatifToplamTutar = value;
                OnPropertyChanged(nameof(KumulatifToplamTutar));
            }
        }


        private double kumulatifNet { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        Variables variables = new Variables();

        public ObservableCollection<Cls_Fatura> populateFaturaView()
        {
            try
            {
                variables.Counter = 0;
                kumulatifKapAdet = 0;
                kumulatifBrut = 0;
                dataTable = dataLayer.Select_Stored_Proc_LTD("vbpIrsaliyeDuzenle", "@yil", "@irsaliyeNo", variables.Yil, IrsaliyeNo, variables.Yil);
                DataRow FirstRow = dataTable.Rows[0];
                CariKodu = FirstRow["cari_kodu"].ToString();
                Birim = FirstRow["dovtip"].ToString();
                TarihFatura = FirstRow["tarih"].ToString();

                foreach (DataRow row in dataTable.Rows)
                {
                    variables.Counter++;
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Fatura cls_fatura = new Cls_Fatura
                    {
                        SiraNo = variables.Counter,
                        AdetTakim = Convert.ToInt32(row["miktar"]),
                        UrunKodu = row["urun_kodu"].ToString(),
                        UrunAdi = row["model_isim"].ToString(),
                        DescriptionOfGoods = row["description_of_goods"].ToString(),
                        MalinCinsiRengi = row["stok_adi"].ToString(),
                        GonderimSekil = "CFR",
                        BirimTutar = Convert.ToDouble(row["dovfiat"]),
                        Birim = row["unit"].ToString(),
                        Color = row["color"].ToString(),
                        ToplamTutar = Convert.ToDouble(row["toplamFiyat"]),
                        Brut = Convert.ToDouble(row["birim_agirlik"]) * Convert.ToDouble(row["miktar"])
                    };

                    kumulatifKapAdet = kumulatifKapAdet + cls_fatura.AdetTakim;
                    kumulatifToplamTutar = kumulatifToplamTutar + Convert.ToDecimal(cls_fatura.ToplamTutar);
                    kumulatifBrut = kumulatifBrut + cls_fatura.Brut;
                    coll_fatura.Add(cls_fatura);
                }

                KapAdet = kumulatifKapAdet;

                string birim = Birim;
                char delimiter = '/';
                string[] parts = birim.Split(delimiter);
                string currency = parts[0];

                ToplamTutarMetin = string.Format("TOPLAM {0} {1} {2}", GonderimSekil, currency, kumulatifToplamTutar);

                ToplamTutarYalniz = string.Format("Yalnız {0}", ConvertNumbersToTurkishLetters.ConvertToTurkishText(kumulatifToplamTutar));

                ToplamKapAdetString = string.Format("{0} pcs of furnitures", KapAdet);

                BrutNet = string.Format("{0}/{1}", kumulatifBrut, Net);

                FaturaCollection = coll_fatura;
                OnPropertyChanged(nameof(KapAdet));
                OnPropertyChanged(nameof(BrutNet));
                OnPropertyChanged(nameof(ToplamTutarMetin));
                OnPropertyChanged(nameof(ToplamTutarYalniz));
                OnPropertyChanged(nameof(FaturaCollection));
                OnPropertyChanged(nameof(DescriptionOfGoods));
                OnPropertyChanged(nameof(CariKodu));
                OnPropertyChanged(nameof(TarihFatura));
                return FaturaCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<Cls_Fatura> PopulatePackingListView()
        {
            try
            {
                variables.Counter = 0;
                int kumulatifToplamPaket = 0;
                int kumulatifToplamSet = 0;
                double kumulatifToplamAgirlik = 0;
                dataTable = dataLayer.Select_Stored_Proc_LTD("vbpIrsaliyeDuzenle", "@yil", "@irsaliyeNo", variables.Yil, IrsaliyeNo, variables.Yil);
                DataRow FirstRow = dataTable.Rows[0];
                CariKodu = FirstRow["cari_kodu"].ToString();
                Birim = FirstRow["dovtip"].ToString();
                TarihFatura = FirstRow["tarih"].ToString();

                foreach (DataRow row in dataTable.Rows)
                {
                    variables.Counter++;
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Fatura cls_fatura = new Cls_Fatura
                    {
                        SiraNo = variables.Counter,
                        AdetTakim = Convert.ToInt32(row["miktar"]),
                        PaketAdet = Convert.ToInt32(row["paketAdedi"]),
                        UrunKodu = row["urun_kodu"].ToString(),
                        UrunAdi = row["model_isim"].ToString(),
                        DescriptionOfGoods = row["description_of_goods"].ToString(),
                        Color = row["color"].ToString(),
                        Brut = Convert.ToDouble(row["birim_agirlik"])
                    };

                    cls_fatura.UrunBirim = "PCS";
                    cls_fatura.PaketToplam = cls_fatura.AdetTakim * cls_fatura.PaketAdet;
                    cls_fatura.Brut = cls_fatura.Brut * cls_fatura.AdetTakim;

                    kumulatifToplamPaket = kumulatifToplamPaket + cls_fatura.PaketToplam;
                    kumulatifToplamSet = kumulatifToplamSet + cls_fatura.AdetTakim;
                    kumulatifToplamAgirlik = kumulatifToplamAgirlik + cls_fatura.Brut;

                    coll_fatura.Add(cls_fatura);
                }

                KumulatifToplamPaket = kumulatifToplamPaket;
                KumulatifToplamSet = kumulatifToplamSet;
                KumulatifToplamAgirlik = kumulatifToplamAgirlik;
                OnPropertyChanged(nameof(KumulatifToplamPaket));
                OnPropertyChanged(nameof(KumulatifToplamSet));
                OnPropertyChanged(nameof(KumulatifToplamAgirlik));


                FaturaCollection = coll_fatura;

                return FaturaCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
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

            }

            else
            {
                MessageBox.Show("Cari Bilgisi Sistemde Mevcut Değil.");
            }

        }

        protected void OnPropertyChanged(string getStr)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(getStr));
        }
    }
}