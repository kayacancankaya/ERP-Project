using Layer_2_Common.Type;
using Layer_Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Layer_Business
{
    public class Cls_Depo : Cls_Base, INotifyPropertyChanged
    {
        public string StokKodu { get; set; } = string.Empty;
        public string StokAdi { get; set; } = string.Empty;
        public decimal StokMiktar { get; set; } = 0;
        public int DepoKodu { get; set; } = 0;

        public int HareketID { get; set; } = 0;

        public string FisNo { get; set; } = string.Empty;

        public decimal HareketMiktar { get; set; } = 0;

        public string GirisCikisKod { get; set; } = string.Empty;

        public DateTime HareketTarih { get; set; }

        public string HareketAciklama { get; set; } = string.Empty;
        public string SiparisNumarasi { get; set; } = string.Empty;
        public int SiparisSira { get; set; } = 0;
        public string Ekalan { get; set; } = string.Empty;
        public string Kod1 { get; set; } = string.Empty;

        public decimal NetFiyat {  get; set; } = 0;
        public decimal BrütFiyat { get; set; } = 0;

        public decimal DovizFiyat { get; set; } = 0;

        public string HareketTuru { get; set; } = string.Empty;
        public string BelgeTipi { get; set; } = string.Empty;
        public string BelgeTipiKodu { get; set; } = string.Empty;
        public string FaturaTipi { get; set; } = string.Empty;
        public string FaturaTipiKodu { get; set; } = string.Empty;
        public string HareketTipi { get; set; } = string.Empty;
        public string HareketTipiKodu { get; set; } = string.Empty;
        public string IsemriNo { get; set; } = string.Empty;
        public string TakipNo { get; set; } = string.Empty;
        public string Kod2 { get; set; } = string.Empty;
        public string Kod3 { get; set; } = string.Empty;
        public string Kod4 { get; set; } = string.Empty;
        public string Kod5 { get; set; } = string.Empty;
        public int CikisDepoKodu { get; set; }
        public int GirisDepoKodu { get; set; }
        public decimal CikisDepoBakiye { get; set; }
        public decimal GirisDepoBakiye { get; set; }
        public decimal ToplamDATIhtiyacMiktar { get; set; }
        public decimal GonderilenDATMiktar { get; set; }
        public decimal KalanDATMiktar { get; set; }
        public decimal GonderilecekDATMiktar { get; set; }
        //eksi bakiye
        public decimal BakiyeAranan { get; set; }
        public decimal Bakiye10 { get; set; }
        public decimal Bakiye15 { get; set; }
        public decimal Bakiye30 { get; set; }
        public decimal Bakiye35 { get; set; }
        public decimal Bakiye40 { get; set; }
        public decimal Bakiye45 { get; set; }
        // eksi bakiye ends
        private bool _isChecked = true;

        public bool IsChecked 
        {
            get { return _isChecked; }
            set { _isChecked = value; 
                    OnPropertyChanged(nameof(IsChecked));
                }
        }


        Variables variables = new();
        DataLayer data = new();
        LoginLogic login = new();
        ObservableCollection<Cls_Depo> temp_depo_coll = new();
        ObservableCollection<Cls_Depo> temp_stok_hareket_coll = new();
        ObservableCollection<Cls_Depo> DepoCollection = new();
        ObservableCollection<Cls_Depo> StokHareketCollection = new();

        public Cls_Depo()
        {
            variables.Fabrika = login.GetFabrika();

        }
        public ObservableCollection<Cls_Depo> PopulateDepoDurumList(Dictionary<string,string> kisitPairs, string fabrika)
        {
            try
            {
                variables.Query = "SELECT ph.STOK_KODU,sabit.STOK_ADI,ph.depo_kodu,cast(isnull(TOP_GIRIS_MIK,0)-isnull(TOP_CIKIS_MIK,0) as float) AS bakiye " +
                                    " FROM TBLSTOKPH ph (nolock) left join tblstsabit sabit (nolock) on ph.stok_kodu=sabit.stok_kodu where ph.DEPO_KODU<>0 " +
                                    " and ph.stok_kodu = @stokKodu";

                SqlParameter[] parameter = new SqlParameter[1];
        
                if (kisitPairs["stokKodu"] != null)
                {
                    parameter[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parameter[0].Value = kisitPairs["stokKodu"];
                }

                temp_depo_coll.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter,fabrika))
                {
                    if (reader != null )
                    {
                        while (reader.Read())
                        {
                            Cls_Depo depoItem = new Cls_Depo
                            {
                                StokKodu = reader["STOK_KODU"].ToString(),
                                StokAdi = reader["STOK_ADI"].ToString(),
                                DepoKodu = Convert.ToInt32(reader["depo_kodu"]),
                                StokMiktar = Convert.ToInt32(reader["bakiye"]),
                            };
                            temp_depo_coll.Add(depoItem);
                        }
                    }

                    if (reader == null ) 
                    {
                        Cls_Depo depoItem = new Cls_Depo
                        {
                            StokKodu = "BOS",
                        };
                        temp_depo_coll.Add(depoItem);
                    }
                }

                DepoCollection = temp_depo_coll;
                return DepoCollection;

            }
            catch { return null; }
        }
        public ObservableCollection<Cls_Depo> PopulateStokHareketList(Dictionary<string, string> kisitPairs,string fabrika)
        {
            try
            {

                variables.Query = "SELECT a.inckeyno,a.STOK_KODU,FISNO,CAST(STHAR_GCMIK AS FLOAT) AS GC_MIK, STHAR_GCKOD AS GCKOD, a.DEPO_KODU,STHAR_TARIH AS TARIH," +
                                  " STHAR_ACIKLAMA AS ACIK,STHAR_SIPNUM AS SIPNUM,STRA_SIPKONT,kod_1,stok_adi,ekalan,STHAR_NF AS NF,STHAR_BF AS BF,STHAR_IAF AS IAF," +
                                  "STHAR_HTUR,STHAR_BGTIP,STHAR_FTIRSIP,kt_takipnum FROM TBLSTHAR a (NOLOCK) " +
                                   "left join tblstsabit b (NOLOCK) on a.stok_kodu = b.stok_kodu left join tblisemriek c (NOLOCK) on a.sthar_sipnum = c.isemri WHERE 1 = 1 ";

              
                variables.Counter = 0;

                if (kisitPairs["stokKodu"] != null)
                {
                    variables.Query = variables.Query + "and a.stok_kodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }


                if (kisitPairs["stokAdi"] != null)
                {
                    variables.Query = variables.Query + "and stok_adi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }
                if (kisitPairs["fisNo"] != null)
                {
                    variables.Query = variables.Query + "and fisno like '%' + @fisNo + '%' ";
                    variables.Counter++;
                }
                if (kisitPairs["aciklama"] != null)
                {
                    variables.Query = variables.Query + "and sthar_aciklama like '%' + @aciklama + '%' ";
                    variables.Counter++;
                }
                if (kisitPairs["ekalan"] != null)
                {
                    variables.Query = variables.Query + "and ekalan like '%' + @ekalan + '%' ";
                    variables.Counter++;
                }
                if (kisitPairs["depoKodu1"] != null && kisitPairs["depoKodu2"] == null)
                {
                    variables.Query = variables.Query + "and a.depo_kodu =@depoKodu1 ";
                    variables.Counter++;
                }

                if (kisitPairs["depoKodu1"] != null && kisitPairs["depoKodu2"] != null)
                {
                    variables.Query = variables.Query + "and (a.depo_kodu =@depoKodu1 or a.depo_kodu =@depoKodu2) ";
                    variables.Counter += 2;
                }
                if (kisitPairs["hareketTipi"] != null)
                {
                    variables.Query = variables.Query + GetHareketTipiQuery(kisitPairs["hareketTipi"]);
                }
                if (kisitPairs["kod1"] != null)
                {
                    variables.Query = variables.Query + "and kod_1 = @kod1 ";
                    variables.Counter++;
                }
                if (kisitPairs["baslangicTarih"] != null)
                {
                    variables.Query = variables.Query + "and sthar_tarih>=@baslangicTarih ";
                    variables.Counter++;
                }
                if (kisitPairs["bitisTarih"] != null)
                {
                    variables.Query = variables.Query + "and sthar_tarih<=@bitisTarih ";
                    variables.Counter++;
                }


                SqlParameter[] parameters = new SqlParameter[variables.Counter];
                variables.Counter = 0;

                if (kisitPairs["stokKodu"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = kisitPairs["stokKodu"];
                    variables.Counter++;
                }

                if (kisitPairs["stokAdi"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@stokAdi", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["stokAdi"];
                    variables.Counter++;
                }
                if (kisitPairs["fisNo"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@fisNo", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = kisitPairs["fisNo"];
                    variables.Counter++;
                }
                if (kisitPairs["aciklama"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@aciklama", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["aciklama"];
                    variables.Counter++;
                }
                if (kisitPairs["ekalan"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@ekalan", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["ekalan"];
                    variables.Counter++;
                }
                if (kisitPairs["depoKodu1"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@depoKodu1", SqlDbType.Int);
                    parameters[variables.Counter].Value = kisitPairs["depoKodu1"];
                    variables.Counter++;
                }
                if (kisitPairs["depoKodu2"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@depoKodu2", SqlDbType.Int);
                    parameters[variables.Counter].Value = kisitPairs["depoKodu2"];
                    variables.Counter++;
                }
                if (kisitPairs["kod1"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@kod1", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["kod1"];
                    variables.Counter++;
                }
                if (kisitPairs["baslangicTarih"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@baslangicTarih", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = kisitPairs["baslangicTarih"];
                    variables.Counter++;
                }
                if (kisitPairs["bitisTarih"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@bitisTarih", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = kisitPairs["bitisTarih"];
                    variables.Counter++;
                }

                temp_stok_hareket_coll.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,fabrika))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Cls_Depo depoItem = new Cls_Depo
                            {
                                HareketID = Convert.ToInt32(reader["inckeyno"]),
                                StokKodu = reader["STOK_KODU"].ToString(),
                                StokAdi = reader["stok_adi"].ToString(),
                                FisNo = reader["FISNO"].ToString(),
                                HareketMiktar = Convert.ToDecimal(reader["GC_MIK"]),
                                GirisCikisKod = reader["GCKOD"].ToString(),
                                DepoKodu = Convert.ToInt32(reader["DEPO_KODU"]),
                                HareketTarih =Convert.ToDateTime(reader["TARIH"]),
                                HareketAciklama = reader["ACIK"].ToString(),
                                Ekalan = reader["ekalan"].ToString(),
                                TakipNo = reader["kt_takipnum"].ToString(),
                                SiparisNumarasi = reader["SIPNUM"].ToString(),
                                SiparisSira = Convert.ToInt32(reader["STRA_SIPKONT"]),
                                Kod1 = reader["kod_1"].ToString(),
                                HareketTipiKodu = reader["STHAR_HTUR"].ToString(),
                                BelgeTipiKodu = reader["STHAR_BGTIP"].ToString(),
                                FaturaTipiKodu = reader["STHAR_FTIRSIP"].ToString(),

                            };
                            temp_stok_hareket_coll.Add(depoItem);
                        }
                    }

                    else
                    {
                        Cls_Depo depoItem = new Cls_Depo
                        {
                            StokKodu = "BOS",

                        };
                        temp_stok_hareket_coll.Add(depoItem);
                    }
                }

                StokHareketCollection = temp_stok_hareket_coll;
                return StokHareketCollection;

            }
            catch { return null; }
        }
        public ObservableCollection<Cls_Depo> PopulateDATKaydedilecekListesi(Dictionary<string, string> kisitPairs, string fabrika)
        {
            try
            {
                variables.Query = "select * from vbvDATListe where hamKodu like 'hm%' and takipNo is not null and takipNo<>'' ";


                variables.Counter = 0;

                if (kisitPairs["takipNo"] != null)
                {
                    variables.Query = variables.Query + "and takipNo like '%' + @takipNo + '%' ";
                    variables.Counter++;
                }
                if (kisitPairs["hamKodu"] != null)
                {
                    variables.Query = variables.Query + "and hamKodu like '%' + @hamKodu + '%' ";
                    variables.Counter++;
                }
                if (kisitPairs["hamAdi"] != null)
                {
                    variables.Query = variables.Query + "and hamAdi like '%' + @hamAdi + '%' ";
                    variables.Counter++;
                }
                if (kisitPairs["kod1"] != null)
                {
                    variables.Query = variables.Query + "and kod1 = @kod1 ";
                    variables.Counter++;
                }
                if (kisitPairs["kod5"] != null)
                {
                    variables.Query = variables.Query + "and kod5 = @kod5 ";
                    variables.Counter++;
                }
               
                SqlParameter[] parameters = new SqlParameter[variables.Counter];
                variables.Counter = 0;

                if (kisitPairs["takipNo"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@takipNo", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = kisitPairs["takipNo"];
                    variables.Counter++;
                }

                if (kisitPairs["hamKodu"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@hamKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = kisitPairs["hamKodu"];
                    variables.Counter++;
                }
                if (kisitPairs["hamAdi"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@hamAdi", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["hamAdi"];
                    variables.Counter++;
                }
                if (kisitPairs["kod1"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@kod1", SqlDbType.NVarChar, 100);
                    parameters[variables.Counter].Value = kisitPairs["kod1"];
                    variables.Counter++;
                }
                if (kisitPairs["kod5"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@kod5", SqlDbType.NVarChar, 100);
                    parameters[variables.Counter].Value = kisitPairs["kod5"];
                    variables.Counter++;
                }

                temp_depo_coll.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters, fabrika))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Cls_Depo depoItem = new Cls_Depo
                            {
                                TakipNo = reader["takipNo"].ToString(),
                                StokKodu = reader["hamKodu"].ToString(),
                                StokAdi = reader["hamAdi"].ToString(),
                                Kod5 = reader["kod5"].ToString(),
                                Kod1 = reader["kod1"].ToString(),
                                CikisDepoKodu =Convert.ToInt32(reader["cikisDepo"]),
                                GirisDepoKodu = Convert.ToInt32(reader["girisDepo"]),
                                CikisDepoBakiye = Convert.ToDecimal(reader["cikisDepoBakiye"]),
                                GirisDepoBakiye = Convert.ToDecimal(reader["girisDepoBakiye"]),
                                ToplamDATIhtiyacMiktar = Convert.ToDecimal(reader["toplamDATIhtiyac"]),
                                GonderilenDATMiktar = Convert.ToDecimal(reader["gonderilenDATMiktar"]),
                                KalanDATMiktar = Convert.ToDecimal(reader["kalanDATMiktar"]),
                                GonderilecekDATMiktar = Convert.ToDecimal(reader["kalanDATMiktar"]),
                                IsChecked = true,
                            };
                            temp_depo_coll.Add(depoItem);
                        }
                    }

                    else
                    {
                        Cls_Depo depoItem = new Cls_Depo
                        {
                            StokKodu = "BOS",

                        };
                        temp_depo_coll.Add(depoItem);
                    }
                }

                DepoCollection = temp_depo_coll;
                return DepoCollection;

            }
            catch 
            {
                return null;
            }
        }
        public ObservableCollection<Cls_Depo> PopulateEksiBakiyeListesi(int depoKodu)
        {
            try
            {

                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@depoKodu", SqlDbType.SmallInt);
                parameter[0].Value = depoKodu;

                temp_depo_coll.Clear();
                using (DataTable dataTable = data.Stored_Proc_With_Parameters_Returns_Table("vbpEksiBakiyeListe", variables.Yil, parameter)) 
                {
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {

                            Cls_Depo depoItem = new Cls_Depo
                            {
                                StokKodu = dr["STOK_KODU"].ToString(),
                                StokAdi = dr["STOK_ADI"].ToString(),
                                Kod1 = dr["KOD_1"].ToString(),
                                BakiyeAranan = Convert.ToDecimal(dr["BAKIYE"]),
                                Bakiye10 = Convert.ToDecimal(dr["BAKIYE10"]),
                                Bakiye15 = Convert.ToDecimal(dr["BAKIYE15"]),
                                Bakiye30 = Convert.ToDecimal(dr["BAKIYE30"]),
                                Bakiye35 = Convert.ToDecimal(dr["BAKIYE35"]),
                                Bakiye40 = Convert.ToDecimal(dr["BAKIYE40"]),
                                Bakiye45 = Convert.ToDecimal(dr["BAKIYE45"]),
                            };
                            temp_depo_coll.Add(depoItem);
                        }
                    }
                }

                DepoCollection = temp_depo_coll;
                return DepoCollection;

            }
            catch 
            {
                return null;
            }
        }
        public ObservableCollection<int> GetDistinctDepoKodu()
        {
            try
            {
                variables.Query = "Select distinct depo_kodu from tblstsabit (nolock) where depo_kodu <> 0 order by depo_kodu asc";
                int depoKodu = 0;
                ObservableCollection<int> depoKoduCollection = new();
                if(DepoCollection != null)
                    DepoCollection.Clear();

                using(SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query,variables.Yil,variables.Fabrika))
                {
                    if (reader == null)
                        return null;

                    while (reader.Read()) 
                    {

                        depoKodu = Convert.ToInt32(reader[0]);

                        depoKoduCollection.Add(depoKodu);
                    }
                }
                return depoKoduCollection;
            }
            catch 
            {
                return null;
            }
        }
        public ObservableCollection<string> GetDistinctKod1()
        {
            try
            {
                variables.Query = "Select distinct kod_1 from tblstsabit (nolock) order by kod_1 asc";
                string kod1 = string.Empty;
                ObservableCollection<string> kod1Collection = new();

                using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil,variables.Fabrika))
                {
                    if (reader == null)
                        return null;

                    while (reader.Read())
                    {
                        kod1 = reader[0].ToString();

                        kod1Collection.Add(kod1);
                    }
                }
                return kod1Collection;
            }
            catch
            {
                return null;
            }
        }
        public ObservableCollection<string> GetDistinctKod5()
        {
            try
            {
                variables.Query = "Select distinct kod_5 from tblstsabit (nolock) order by kod_5 asc";
                string kod5 = string.Empty;
                ObservableCollection<string> kod5Collection = new();
                
                using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil, variables.Fabrika))
                {
                    if (reader == null)
                        return null;

                    while (reader.Read())
                    {
                        kod5 = reader[0].ToString();

                        kod5Collection.Add(kod5);
                    }
                }
                return kod5Collection;
            }
            catch
            {
                return null;
            }
        }
        private string GetHareketTipiQuery(string hareketTipi)
        {
            try
            {

                string query = string.Empty;
                switch (hareketTipi)
                {
                    case "DAT":
                        query = "and (sthar_ftirsip ='9' or sthar_ftirsip='8') and sthar_htur='B' and sthar_bgtip='I' ";
                        break;
                    case "FATURA":
                        query = "and sthar_ftirsip ='2' and sthar_htur='J' and sthar_bgtip='F' ";
                        break;
                    case "İRSALİYE":
                        query = "and sthar_ftirsip ='4' and sthar_htur='H' and sthar_bgtip='I' ";
                        break;
                    case "SEVK":
                        query = "and sthar_ftirsip ='1' and sthar_htur='J' and sthar_bgtip='F' ";
                        break;
                    case "ÜRETİM":
                        query = "and sthar_htur='C' and (sthar_bgtip='U' or sthar_bgtip='V') ";
                        break;
                    case "VİRMAN":
                        query = "and (sthar_ftirsip ='9' or sthar_ftirsip='8') and sthar_htur='A' and sthar_bgtip='I' ";
                        break;
                    default:
                        query = string.Empty;
                        break;
            }
            return query;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public ObservableCollection<Cls_Depo> GetStokKarti(Dictionary<string,string> constraintPairs)
        {
            ObservableCollection<Cls_Depo> stokKartiCollection = new();
            try
            {
                
                if(constraintPairs == null || constraintPairs.Count == 0)
                {
                    Cls_Depo depoHata = new Cls_Depo();
                    depoHata.StokAdi = "Stok Kodu, Stok Adı Seçimleri Bulunamadı";
                    stokKartiCollection.Add(depoHata);
                    return stokKartiCollection;
                }

                variables.Query = "Select stok_kodu,stok_adi,isnull(kod_1,'') as kod1,isnull(kod_2,'') as kod2,isnull(kod_3,'') as kod3,isnull(kod_4,'') as kod4,isnull(kod_5,'') as kod5 from tblstsabit (nolock) where 1=1 ";

                variables.Counter = 0;

                if (constraintPairs["stokKodu"] != null)
                {
                    variables.Query = variables.Query + "and stok_kodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }

                if (constraintPairs["stokAdi"] != null)
                {
                    variables.Query = variables.Query + "and stok_adi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];
                variables.Counter = 0;

                if (constraintPairs["stokKodu"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = constraintPairs["stokKodu"];
                    variables.Counter++;
                }

                if (constraintPairs["stokAdi"] != null)
                {
                    parameters[variables.Counter] = new SqlParameter("@stokAdi", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = constraintPairs["stokAdi"];
                    variables.Counter++;
                }
                if(temp_depo_coll.Count > 0) 
                    temp_depo_coll.Clear();
                
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameters, variables.Fabrika))
                {
                    if (reader == null)
                    {
                        Cls_Depo depoHata = new Cls_Depo();
                        depoHata.StokAdi = "Sorgu Boş Sonuç Döndürdü";
                        stokKartiCollection.Add(depoHata);
                        return stokKartiCollection;
                    }
                    while (reader.Read())
                    {
                        Cls_Depo stokKarti = new Cls_Depo
                        {
                            StokKodu = reader[0].ToString(),
                            StokAdi = reader[1].ToString(),
                            Kod1 = reader[2].ToString(),
                            Kod2 = reader[3].ToString(),
                            Kod3 = reader[4].ToString(),
                            Kod4 = reader[5].ToString(),
                            Kod5 = reader[6].ToString(),
                        };
                        temp_depo_coll.Add(stokKarti);
                    }

                    stokKartiCollection = temp_depo_coll;
                }
                return stokKartiCollection;
            }
            catch
            {
                Cls_Depo depoHata = new Cls_Depo();
                depoHata.StokAdi = "Veri Tabanına Bağlanırken Hata İle Karşılaşıldı.";
                stokKartiCollection.Add(depoHata);
                return stokKartiCollection;
            }
        }
        public string GetFisnoForDAT()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@prefix", SqlDbType.NVarChar, 4);
                parameters[0].Value = "DPIH";
                parameters[1] = new SqlParameter("@tableName", SqlDbType.NVarChar, 128);
                parameters[1].Value = "TBLSTHAR";
                parameters[2] = new SqlParameter("@columnName", SqlDbType.NVarChar, 128);
                parameters[2].Value = "FISNO";
                FisNo = data.Get_One_String_Result_Stored_Proc_With_Parameters("vbpGetFisno", variables.Yil, parameters);

                return FisNo;
            }
            catch 
            {
                return string.Empty;
            }

        }

        public int InsertDAT(ObservableCollection<Cls_Depo> datCollection, string fisno)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@portalID", SqlDbType.SmallInt);
                parameters[0].Value = login.GetPortalID();
                parameters[1] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                parameters[1].Value = fisno;

                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertDATMas", variables.Yil, parameters);
                if(!variables.Result)
                    return 2;

                variables.Counter = 1;
                foreach(Cls_Depo item in datCollection)
                {
                    SqlParameter[] parametersSatir = new SqlParameter[7];
                    parametersSatir[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar,35);
                    parametersSatir[0].Value = item.StokKodu;
                    parametersSatir[1] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                    parametersSatir[1].Value = fisno;
                    parametersSatir[2] = new SqlParameter("@miktar", SqlDbType.Decimal);
                    parametersSatir[2].Value = item.GonderilecekDATMiktar;
                    parametersSatir[3] = new SqlParameter("@cikisDepoKodu", SqlDbType.Int);
                    parametersSatir[3].Value = item.CikisDepoKodu;
                    parametersSatir[4] = new SqlParameter("@girisDepoKodu", SqlDbType.Int);
                    parametersSatir[4].Value = item.GirisDepoKodu;
                    parametersSatir[5] = new SqlParameter("@takipNo", SqlDbType.NVarChar, 15);
                    parametersSatir[5].Value = item.TakipNo;
                    parametersSatir[6] = new SqlParameter("@sira", SqlDbType.SmallInt);
                    parametersSatir[6].Value = variables.Counter;

                    variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertDATSatir",variables.Yil, parametersSatir);

                    if (!variables.Result)
                        return 3;

                    variables.Counter++;
                }
                return 1;
            }
            catch (Exception ex)
            {
                var exm = ex.Message.ToString(); return -1;
            }
        }
        public Int16 InsertVirman(Cls_Depo eskiDepoKodu, Cls_Depo yeniDepoKodu)
        {
            try
            {
                if(eskiDepoKodu is null ||
                    yeniDepoKodu is null)
                { return 2; }

                SqlParameter[] parameters = new SqlParameter[9];
                parameters[0] = new SqlParameter("@eskiStokKodu", SqlDbType.NVarChar,35);
                parameters[0].Value = eskiDepoKodu.StokKodu;
                parameters[1] = new SqlParameter("@eskiMiktar", SqlDbType.Float);
                parameters[1].Value = eskiDepoKodu.HareketMiktar;
                parameters[2] = new SqlParameter("@eskiDepoKodu", SqlDbType.Int);
                parameters[2].Value = eskiDepoKodu.DepoKodu;
                parameters[3] = new SqlParameter("@aciklama", SqlDbType.NVarChar, 200);
                parameters[3].Value = eskiDepoKodu.Ekalan;
                parameters[4] = new SqlParameter("@yeniStokKodu", SqlDbType.NVarChar, 35);
                parameters[4].Value = yeniDepoKodu.StokKodu;
                parameters[5] = new SqlParameter("@yeniMiktar", SqlDbType.Float);
                parameters[5].Value = yeniDepoKodu.HareketMiktar;
                parameters[6] = new SqlParameter("@yeniDepoKodu", SqlDbType.Int);
                parameters[6].Value = yeniDepoKodu.DepoKodu;
                parameters[7] = new SqlParameter("@tarih", SqlDbType.DateTime);
                parameters[7].Value = eskiDepoKodu.HareketTarih;
                parameters[8] = new SqlParameter("@userName", SqlDbType.NVarChar,200);
                parameters[8].Value = login.GetUserName();

                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertVirman", variables.Yil, parameters,variables.Fabrika);
                if(!variables.Result)
                    return 3;

                return 1;
            }
            catch 
            {
                return -1;
            }
        }

        public bool DATGeriAl(string fisno)
        {
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
                parameter[0].Value = fisno;

                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpDATGeriAl",variables.Yil, parameter);

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
