using Layer_Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Input;
using Layer_2_Common.Type;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;

namespace Layer_Business
{
    public class Cls_Isemri : INotifyPropertyChanged
    {
        public string SIPARIS_NO { get; set; }

        public int SIPARIS_SIRA { get; set; }

        public string CARI_KODU { get; set; }
        public string CARI_ADI { get; set; }
        public string TAKIP_NO { get; set; }
        public string URUN_KODU { get; set; }
        public string URUN_ADI { get; set; }
        public string STOK_KODU { get; set; }
        public string STOK_ADI { get; set; }
        public int DEPO_KODU { get; set; }
        public int CIKIS_DEPO_KODU { get; set; }
        public string REFISEMRINO { get; set; }
        public string ISEMRINO { get; set; }
        public int IE_MIKTAR { get; set; }
        public int KALAN_IE_MIKTAR { get; set; }
        public string HAM_KODU { get; set; }
        public string HAM_ADI { get; set; }
        public decimal BILDIRILEN_MIKTAR { get; set; }
        public decimal BIRIM_HAM_MIKTAR { get; set; }
        public decimal TUKETILEN_MIKTAR { get; set; } //URETSON_MIKTAR*RECETE MIKTAR
        public int BildirilecekIsemriMiktar { get; set; }
        public string ID { get; set; }
        public int ReceteID { get; set; }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }


        private decimal _metre;

        public decimal Metre
        {
            get { return _metre; }
            set { _metre = value; }
        }

        private decimal _kumulatifIhtiyac = 0;

        public decimal KumulatifIhtiyac
        {
            get { return _kumulatifIhtiyac; }
            set { _kumulatifIhtiyac = value; }
        }
        private decimal _birimIhtiyac = 0;

        public decimal BirimIhtiyac
        {
            get { return _birimIhtiyac; }
            set { _birimIhtiyac = value; }
        }
        private decimal _katSayi = 0;

        public decimal KatSayi
        {
            get { return _katSayi; }
            set { _katSayi = value; }
        }



        private ObservableCollection<Cls_Isemri> _isemirleriCollection;
        public ObservableCollection<Cls_Isemri> IsemirleriCollection
        {
            get { return _isemirleriCollection; }
            set { _isemirleriCollection = value; }
        }


        readonly ObservableCollection<Cls_Isemri> coll_isemri = new ObservableCollection<Cls_Isemri>();

        readonly DataLayer dataLayer = new DataLayer();
        DataTable dataTable = new DataTable();
        private SqlDataReader reader;
        Variables variables = new();
        LoginLogic login = new();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyChanged)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
        }

        public Cls_Isemri()
        {
            variables.Fabrika = login.GetFabrika();
        }

        public ObservableCollection<Cls_Isemri> PopulateIsemriBildirimList(Dictionary<string, string> restrictionPairs)
        {
            try
            {
                if (restrictionPairs != null)
                {
                    variables.Query = "select * from vbvBildirilecekIsemirleriListele where isemrino=refisemrino ";
                    variables.Counter = 0;

                    if (restrictionPairs.ContainsKey("@siparisNo"))
                    {
                        variables.Query += " and siparisNo like '%' + @siparisNo + '%'";
                        variables.Counter++;
                    }
                    if (restrictionPairs.ContainsKey("@isemrino"))
                    {
                        variables.Query += " and isemrino like '%' + @isemrino + '%'";
                        variables.Counter++;
                    }

                    if (restrictionPairs.ContainsKey("@stokKodu"))
                    {
                        variables.Query += " and stokKodu like '%' + @stokKodu + '%'";
                        variables.Counter++;
                    }

                    if (restrictionPairs.ContainsKey("@stokAdi"))
                    {
                        variables.Query += " and stokAdi like  '%' + @stokAdi + '%'";
                        variables.Counter++;
                    }
                    if (restrictionPairs.ContainsKey("@cariAdi"))
                    {
                        variables.Query += " and cariAdi like  '%' + @cariAdi + '%'";
                        variables.Counter++;
                    }

                    SqlParameter[] parameters = new SqlParameter[variables.Counter];

                    variables.Counter = 0;
                    if (restrictionPairs.ContainsKey("@siparisNo"))
                    {
                        parameters[variables.Counter] = new("@siparisNo", SqlDbType.NVarChar, 15);
                        parameters[variables.Counter].Value = restrictionPairs["@siparisNo"];
                        variables.Counter++;
                    }

                    if (restrictionPairs.ContainsKey("@isemrino"))
                    {
                        parameters[variables.Counter] = new("@isemrino", SqlDbType.NVarChar, 35);
                        parameters[variables.Counter].Value = restrictionPairs["@isemrino"];
                        variables.Counter++;
                    }
                    if (restrictionPairs.ContainsKey("@stokKodu"))
                    {
                        parameters[variables.Counter] = new("@stokKodu", SqlDbType.NVarChar, 35);
                        parameters[variables.Counter].Value = restrictionPairs["@stokKodu"];
                        variables.Counter++;
                    }

                    if (restrictionPairs.ContainsKey("@stokAdi"))
                    {
                        parameters[variables.Counter] = new("@stokAdi", SqlDbType.NVarChar, 435);
                        parameters[variables.Counter].Value = restrictionPairs["@stokAdi"];
                        variables.Counter++;
                    }
                    if (restrictionPairs.ContainsKey("@cariAdi"))
                    {
                        parameters[variables.Counter] = new("@cariAdi", SqlDbType.NVarChar, 435);
                        parameters[variables.Counter].Value = restrictionPairs["@cariAdi"];
                        variables.Counter++;
                    }

                    coll_isemri.Clear();
                    if (IsemirleriCollection != null)
                        IsemirleriCollection.Clear();


                    using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters, variables.Fabrika))
                    {

                        while (reader.Read())
                        {
                            Cls_Isemri isemri = new Cls_Isemri
                            {
                                SIPARIS_NO = reader["siparisNo"].ToString(),
                                SIPARIS_SIRA = Convert.ToInt32(reader["siparisSira"]),
                                CARI_KODU = reader["cariKod"].ToString(),
                                CARI_ADI = reader["cariAdi"].ToString(),
                                ISEMRINO = reader["isemrino"].ToString(),
                                URUN_KODU = reader["urunKodu"].ToString(),
                                URUN_ADI = reader["urunAdi"].ToString(),
                                STOK_KODU = reader["stokKodu"].ToString(),
                                STOK_ADI = reader["stokAdi"].ToString(),
                                IE_MIKTAR = Convert.ToInt32(reader["isemriMiktar"]),
                                BILDIRILEN_MIKTAR = Convert.ToInt32(reader["bildirilenIeMiktar"]),
                                KALAN_IE_MIKTAR = Convert.ToInt32(reader["kalanIeMiktar"]),
                                BildirilecekIsemriMiktar = Convert.ToInt32(reader["kalanIeMiktar"]),
                            };
                            coll_isemri.Add(isemri);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    variables.Query = "select * from vbvBildirilecekIsemirleriListele where refisemrino=isemrino and ";

                    coll_isemri.Clear();
                    IsemirleriCollection.Clear();


                    using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.Yil, variables.Fabrika))
                    {

                        while (reader.Read())
                        {
                            Cls_Isemri isemri = new Cls_Isemri
                            {
                                SIPARIS_NO = reader["siparisNo"].ToString(),
                                SIPARIS_SIRA = Convert.ToInt32(reader["siparisSira"]),
                                ISEMRINO = reader["isemrino"].ToString(),
                                URUN_KODU = reader["urunKodu"].ToString(),
                                URUN_ADI = reader["urunAdi"].ToString(),
                                STOK_KODU = reader["stokKodu"].ToString(),
                                STOK_ADI = reader["stokAdi"].ToString(),
                                IE_MIKTAR = Convert.ToInt32(reader["isemriMiktar"]),
                                BILDIRILEN_MIKTAR = Convert.ToInt32(reader["bildirilenIeMiktar"]),
                                KALAN_IE_MIKTAR = Convert.ToInt32(reader["kalanIeMiktar"]),
                                BildirilecekIsemriMiktar = Convert.ToInt32(reader["kalanIeMiktar"]),
                            };
                            coll_isemri.Add(isemri);
                        }
                        reader.Close();
                    }
                }

                IsemirleriCollection = coll_isemri;
                return IsemirleriCollection;
            }
            catch
            {
                return null;
            }
        }
        public ObservableCollection<Cls_Isemri> PopulateGridViewWithIsemirleriCollection(string query, int yil)
        {
            try
            {
                dataTable = dataLayer.Select_Command(query, yil);

                coll_isemri.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Isemri cls_isemri = new Cls_Isemri
                    {
                        TAKIP_NO = row["TAKIP_NO"].ToString(),
                        URUN_KODU = row["URUN_KODU"].ToString(),
                        URUN_ADI = row["URUN_ADI"].ToString(),
                        STOK_KODU = row["STOK_KODU"].ToString(),
                        STOK_ADI = row["STOK_ADI"].ToString(),
                        DEPO_KODU = Convert.ToInt32(row["DEPO_KODU"]),
                        ISEMRINO = row["ISEMRINO"].ToString(),
                        IE_MIKTAR = Convert.ToInt32(row["IE_MIKTAR"]),
                        KALAN_IE_MIKTAR = Convert.ToInt32(row["KALAN_IE_MIKTAR"]),
                        HAM_KODU = row["HAM_KODU"].ToString(),
                        HAM_ADI = row["HAM_ADI"].ToString(),
                        BIRIM_HAM_MIKTAR = Convert.ToDecimal(row["BIRIM_HAM_MIKTAR"]),
                        ID = row["INCKEYNO"].ToString(),
                        BildirilecekIsemriMiktar = Convert.ToInt32(row["KALAN_IE_MIKTAR"]),
                        IsChecked = false
                    };

                    coll_isemri.Add(cls_isemri);
                }

                IsemirleriCollection = coll_isemri;
                return IsemirleriCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Cls_Isemri> PopulateGridViewWithBildirilenIsemirleriCollection(string query, int yil, Dictionary<string, string> kisitlar)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (kisitlar.ContainsKey("@hamAdi"))
                {
                    parameters.Add(new SqlParameter("@hamAdi", SqlDbType.NVarChar, 500) { Value = kisitlar["@hamAdi"] });
                }

                if (kisitlar.ContainsKey("@takipNo"))
                {
                    parameters.Add(new SqlParameter("@takipNo", SqlDbType.NVarChar, 15) { Value = kisitlar["@takipNo"] });
                }
                if (kisitlar.ContainsKey("@urunKodu"))
                {
                    parameters.Add(new SqlParameter("@urunKodu", SqlDbType.NVarChar, 35) { Value = kisitlar["@urunKodu"] });
                }

                // Convert the list to an array
                SqlParameter[] parameterArray = parameters.ToArray();


                reader = dataLayer.Select_Command_Data_Reader_With_Parameters(query, yil, parameterArray);

                coll_isemri.Clear();

                while (reader.Read())
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Isemri cls_isemri = new Cls_Isemri
                    {
                        ReceteID = Convert.ToInt32(reader["recID"]),
                        TAKIP_NO = reader["takip_no"].ToString(),
                        URUN_KODU = reader["urun_kodu"].ToString(),
                        URUN_ADI = reader["urun_adi"].ToString(),
                        STOK_KODU = reader["yari_mamul_kodu"].ToString(),
                        REFISEMRINO = reader["ref_ie"].ToString(),
                        ISEMRINO = reader["isemri_no"].ToString(),
                        HAM_KODU = reader["ham_kodu"].ToString(),
                        HAM_ADI = reader["ham_adi"].ToString(),
                        BIRIM_HAM_MIKTAR = Convert.ToDecimal(reader["birim_miktar"]),
                        TUKETILEN_MIKTAR = Convert.ToDecimal(reader["tuketilen_miktar"]),
                        BILDIRILEN_MIKTAR = Convert.ToDecimal(reader["bildirilenMiktar"]),
                        IsChecked = false
                    };

                    coll_isemri.Add(cls_isemri);
                }

                IsemirleriCollection = coll_isemri;
                return IsemirleriCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public int InsertIsemri(ObservableCollection<Cls_Isemri> bildirimCollection, bool altBildir)
        { 
            try 
	        {
                variables.Query = "vbpInsertUretimSonu";
                if (altBildir)
                    variables.Query = "vbpInsertUretimSonuveAltIsemirleriDahil";
                
                foreach(Cls_Isemri isemri in  bildirimCollection)
                { 
                    SqlParameter[] parameters = new SqlParameter[6];
                    parameters[0] = new SqlParameter("@isemrino", SqlDbType.NVarChar, 15);
                    parameters[1] = new SqlParameter("@stokKodu",SqlDbType.NVarChar, 15);
                    parameters[2] = new SqlParameter("@depoKodu", SqlDbType.Int);
                    parameters[3] = new SqlParameter("@cikisDepoKodu", SqlDbType.Int);
                    parameters[4] = new SqlParameter("@miktar", SqlDbType.Int);
                    parameters[5] = new SqlParameter("@user", SqlDbType.NVarChar,12);
                    parameters[0].Value = isemri.ISEMRINO;
                    parameters[1].Value = isemri.STOK_KODU;
                    parameters[2].Value = isemri.DEPO_KODU;
                    parameters[3].Value = isemri.CIKIS_DEPO_KODU;
                    parameters[4].Value = isemri.BildirilecekIsemriMiktar;
                    parameters[5].Value = login.GetUserName();

                    variables.Result = dataLayer.ExecuteStoredProcedureWithParameters(variables.Query,variables.Yil,parameters,variables.Fabrika);

                    if(variables.Result == false)
                        return -2;

                }
                return 1;

            }
	        catch 
	        {
            return -1;
	        }
        }
        public decimal IsemirleriToplamIhtiyacHesapla(string query, int yil)
        {
            try
            {
                dataTable = dataLayer.Select_Command(query, yil);
                KumulatifIhtiyac = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                   
                    try { Convert.ToInt32(row["KALAN_IE_MIKTAR"]); } catch { ConversionErrors conversionErrors = new ConversionErrors(); MessageBox.Show(conversionErrors.ConversionError("İşemri Miktar")); Mouse.OverrideCursor = null; };

                    try{ Convert.ToDecimal(row["BIRIM_HAM_MIKTAR"]); } catch { ConversionErrors conversionErrors = new ConversionErrors(); MessageBox.Show(conversionErrors.ConversionError("Ham Madde İhtiyaç Miktar")); Mouse.OverrideCursor = null; };

                    int ieMiktar = Convert.ToInt32(row["KALAN_IE_MIKTAR"]);
                    decimal birimHamMiktar = Convert.ToDecimal(row["BIRIM_HAM_MIKTAR"]);

                    KumulatifIhtiyac += (ieMiktar * birimHamMiktar);
                }

                return KumulatifIhtiyac;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
        public decimal IsemirleriBirimIhtiyacHesapla(int isemriMiktar, decimal birimHamMiktar)
        {
            BirimIhtiyac = 0;
            BirimIhtiyac = isemriMiktar * birimHamMiktar;
            return BirimIhtiyac;
        }

        public ObservableCollection<Cls_Isemri> GetUretimTakipKartiCollection(ObservableCollection<Cls_Isemri> referansIsemirleri)
        {
            try
            {

            ObservableCollection<Cls_Isemri> UretimTakipColl = new();
            coll_isemri.Clear();
            
            foreach (Cls_Isemri referansIsemri in referansIsemirleri) 
            {
                variables.Query = "Select * from vbvBildirilecekIsemirleriListele where refisemrino=@refisemrino";
                SqlParameter[] parameter = new SqlParameter[1]; 
                parameter[0] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
                parameter[0].Value = referansIsemri;

                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter, variables.Fabrika))
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Cls_Isemri isemri = new Cls_Isemri
                            {
                                SIPARIS_NO = reader["siparisNo"].ToString(),
                                SIPARIS_SIRA = Convert.ToInt32(reader["siparisSira"]),
                                CARI_KODU = reader["cariKod"].ToString(),
                                CARI_ADI = reader["cariAdi"].ToString(),
                                ISEMRINO = reader["isemrino"].ToString(),
                                STOK_KODU = reader["stokKodu"].ToString(),
                                STOK_ADI = reader["stokAdi"].ToString(),
                                IE_MIKTAR = Convert.ToInt32(reader["isemriMiktar"]),
                                BILDIRILEN_MIKTAR = Convert.ToInt32(reader["bildirilenIeMiktar"]),
                                KALAN_IE_MIKTAR = Convert.ToInt32(reader["kalanIeMiktar"]),
                                BildirilecekIsemriMiktar = Convert.ToInt32(reader["kalanIeMiktar"]),
                            };
                            coll_isemri.Add(isemri);
                        }
                        reader.Close();
                    }
                }
            }
                UretimTakipColl = coll_isemri;
                return UretimTakipColl;
            }
            catch 
            {
                return null;
            }

        }


        public decimal KatSayiHesapla(decimal metre, decimal kumulatifIhtiyac)
        {
            KatSayi = 0;

            if (kumulatifIhtiyac == 0) { MessageBox.Show("Toplam İhtiyaç 0 olamaz"); return 0; }

            KatSayi = metre/kumulatifIhtiyac;

            return KatSayi;
        }
    }
}

