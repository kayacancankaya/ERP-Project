using Layer_2_Common.Excels;
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
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Transactions;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using static Layer_Business.Cls_Base;

namespace Layer_Business

{
    public class Cls_Sevk : Cls_Base, INotifyPropertyChanged
    {


        public string SevkIrsaliyeNo { get; set; }

        public string SevkEmriNo { get; set; }

        public DateTime SevkTarihi { get; set; } = DateTime.Now;

        public DateTime SevkEmriTarihi { get; set; } = DateTime.Now;


        private DateTime _sevkEmriTarihiBaslangicFiltre = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DateTime SevkEmriTarihiBaslangicFiltre
		{
            get { return _sevkEmriTarihiBaslangicFiltre; }
            set { _sevkEmriTarihiBaslangicFiltre = value;
                OnPropertyChanged(nameof(SevkEmriTarihiBaslangicFiltre));
            }
        }

        public int SevkToplamUrun { get; set; }

        private DateTime _sevkEmriTarihiBitisFiltre = new DateTime(DateTime.Now.Year, 12, 31);

        public DateTime SevkEmriTarihiBitisFiltre
		{
            get { return _sevkEmriTarihiBitisFiltre; }
            set { _sevkEmriTarihiBitisFiltre = value;
				OnPropertyChanged(nameof(SevkEmriTarihiBitisFiltre));
			}
        }


        public int SevkSira { get; set; }

        public string SevkAciklama { get; set; } = string.Empty;

        public int SevkMiktar { get; set; }
        public int AcikSevkMiktar { get; set; }

        public string YuklemeEmriNo { get; set; }

        public DateTime YuklemeTarih { get; set; }
        public string SiparisIsemrino { get; set; }

        public string UrunKodu { get; set; }
        
        public string IngilizceIsim { get; set; }

        public string UrunAdi { get; set; }
		public string GTIPNo { get; set; }

		public string SatisCariKodu { get; set; }
		public string SatisCariAdi { get; set; }
        public string CariKodu { get; set; }

        public string CariAdi { get; set; }
        public string CariAdres { get; set; }
        public string CariTel { get; set; }
        public string CariEmail { get; set; }

        public string PaketKodu { get; set; }

        public string PaketAdi { get; set; }

        public string SiparisKodu { get; set; }

        public int SiparisSira { get; set; }

        public string SiparisTarih { get; set; }
		public string SiparisTalepTarih { get; set; }
		public string TeslimTarih { get; set; }

        public string SiparisDurum { get; set; }

        public int PaketMiktar { get; set; }

        public int OkutulanMiktar { get; set; }

        public int UretimMiktar { get; set; }
        public int SiparisMiktar { get; set; }
        public int DepoMiktar { get; set; }
        public int TeslimMiktar { get; set; }

        public int IrsaliyeMiktar { get; set; }

        public string OkutmaTarihi { get; set; }

        public float UrunAgirlik { get; set; }
        public float UrunHacim { get; set; }
        public float SevkAgirlik { get; set; }

        public float SevkHacim { get; set; }

        public string SoforIsim { get; set; } = string.Empty;

        public string PlakaNo { get; set; } = string.Empty;

        public float SiparisFiyatTlUrun { get; set; }
        public float SiparisFiyatDovizUrun { get; set; }
        public float TlTutarToplamSiparis { get; set; }
        public float DovizTutarToplamSiparis { get; set; }
        public string Destinasyon { get; set; }
        public string PONo { get; set; }
        public string SiparisOnEk { get; set; }
        public string UserName { get; set; }

        public Cls_Base.DovizTipi DovizTipi { get; set; }
        public Cls_Base.UretimDurum UretimDurum { get; set; }

        private ObservableCollection<Cls_Sevk> _sevkCollection = new();
        public ObservableCollection<Cls_Sevk> SevkCollection
        {
            get { return _sevkCollection; }
            set
            {
                if (value != null)
                {
                    _sevkCollection = value;
                    OnPropertyChanged(nameof(SevkCollection));
                }
                else
                {
                    throw new ArgumentException("Sevk Bilgileri Oluşturulurken Hata İle Karşılaşıldı.");
                }
            }
        }
        ObservableCollection<Cls_Sevk> AgirlikHacimCollection = new();
        public ObservableCollection<Cls_Sevk> ReOrderedSevkSira = new();
        public ObservableCollection<Cls_Sevk> SevkCollectionCariReport = new();
        public ObservableCollection<Cls_Sevk> SevkCollectionSiparisReport = new();
        public ObservableCollection<Cls_Sevk> WholeReportCollection = new();
        public ObservableCollection<Cls_Sevk> InvoiceCollection = new();

        private ObservableCollection<Cls_Sevk> temp_coll_sevk = new();



        //public List<Cls_Sevk> list_collection_sevk = new List<Cls_Sevk>(); 

        //readonly DataLayer dataLayer = new DataLayer();
        SqlDataReader reader;

        Variables variables = new();
        DataLayer data = new();
        LoginLogic login = new();
        string sevkEmriTarihi = string.Empty;

        public Cls_Sevk()
        {
		}
        public ObservableCollection<Cls_Sevk> PopulateSevkEmriOlusturulacakList_SSH(Dictionary<string, string> kisitPairs)
        {
            try
            {

                ObservableCollection<Cls_Sevk> OrdersCollection = new();
                variables.Query = "select * from vbvSevkEmriOlusturulacakSiparisler where stokKodu like 'SSH%' ";
                variables.Counter = 0;


                if (!string.IsNullOrEmpty(kisitPairs["siparisNo"]))
                {
                    variables.Query = variables.Query + "and siparisNo like '%' + @siparisNo + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["stokKodu"]))
                {
                    variables.Query = variables.Query + "and stokKodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }


                if (!string.IsNullOrEmpty(kisitPairs["stokAdi"]))
                {
                    variables.Query = variables.Query + "and stokAdi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["cariAdi"]))
                {
                    variables.Query = variables.Query + "and cariAdi like '%' + @cariAdi + '%' ";
                    variables.Counter++;
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];
                variables.Counter = 0;

                if (!string.IsNullOrEmpty(kisitPairs["siparisNo"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = kisitPairs["siparisNo"];
                    variables.Counter++;
                }

                if (!string.IsNullOrEmpty(kisitPairs["stokKodu"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = kisitPairs["stokKodu"];
                    variables.Counter++;
                }


                if (!string.IsNullOrEmpty(kisitPairs["stokAdi"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@stokAdi", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["stokAdi"];
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["cariAdi"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@cariAdi", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["cariAdi"];
                    variables.Counter++;
                }

                temp_coll_sevk.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Cls_Sevk sevkItem = new Cls_Sevk
                            {
                                SiparisKodu = reader[0].ToString(),
                                SiparisSira = Convert.ToInt32(reader[1]),
                                UrunKodu = reader[2].ToString(),
                                UrunAdi = reader[3].ToString(),
                                SiparisTarih = reader[4].ToString(),
                                TeslimTarih = reader[5].ToString(),
                                SiparisMiktar = Convert.ToInt32(reader[6]),
                                TeslimMiktar = Convert.ToInt32(reader[7]),
                                AcikSevkMiktar = Convert.ToInt32(reader[8]),
                                DepoMiktar = Convert.ToInt32(reader[9]),
                                UretimDurum = (Cls_Base.UretimDurum)Convert.ToInt32(reader[10]),
                                CariAdi = reader[11].ToString(),
                                CariKodu = reader[12].ToString(),
                            };
                            temp_coll_sevk.Add(sevkItem);
                        }
                    }
                }

                OrdersCollection = temp_coll_sevk;
                return OrdersCollection;

            }
            catch { return null; }
        }
        public ObservableCollection<Cls_Sevk> PopulateSevkEmriOlusturulacakList(Dictionary<string, string> kisitPairs)
        {
            try
            {

                ObservableCollection<Cls_Sevk> OrdersCollection = new();
                variables.Query = "select * from vbvSevkEmriOlusturulacakSiparisler where 1=1 ";
                variables.Counter = 0;


                if (!string.IsNullOrEmpty(kisitPairs["siparisNo"]))
                {
                    variables.Query = variables.Query + "and siparisNo like '%' + @siparisNo + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["stokKodu"]))
                {
                    variables.Query = variables.Query + "and stokKodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }


                if (!string.IsNullOrEmpty(kisitPairs["stokAdi"]))
                {
                    variables.Query = variables.Query + "and stokAdi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["cariAdi"]))
                {
                    variables.Query = variables.Query + "and cariAdi like '%' + @cariAdi + '%' ";
                    variables.Counter++;
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];
                variables.Counter = 0;

                if (!string.IsNullOrEmpty(kisitPairs["siparisNo"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = kisitPairs["siparisNo"];
                    variables.Counter++;
                }

                if (!string.IsNullOrEmpty(kisitPairs["stokKodu"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parameters[variables.Counter].Value = kisitPairs["stokKodu"];
                    variables.Counter++;
                }


                if (!string.IsNullOrEmpty(kisitPairs["stokAdi"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@stokAdi", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["stokAdi"];
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["cariAdi"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@cariAdi", SqlDbType.NVarChar, 400);
                    parameters[variables.Counter].Value = kisitPairs["cariAdi"];
                    variables.Counter++;
                }

					temp_coll_sevk.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Cls_Sevk sevkItem = new Cls_Sevk
                            {
                                SiparisKodu = reader[0].ToString(),
                                SiparisSira = Convert.ToInt32(reader[1]),
                                UrunKodu = reader[2].ToString(),
                                UrunAdi = reader[3].ToString(),
                                SiparisTarih = reader[4].ToString(),
                                TeslimTarih = reader[5].ToString(),
                                SiparisMiktar = Convert.ToInt32(reader[6]),
                                TeslimMiktar = Convert.ToInt32(reader[7]),
                                AcikSevkMiktar = Convert.ToInt32(reader[8]),
                                DepoMiktar = Convert.ToInt32(reader[9]),
                                UretimDurum = (Cls_Base.UretimDurum)Convert.ToInt32(reader[10]),
                                CariAdi = reader[11].ToString(),
                                CariKodu = reader[12].ToString(),
                            };
                            temp_coll_sevk.Add(sevkItem);
                        }
                    }
                }

                OrdersCollection = temp_coll_sevk;
                return OrdersCollection;

            }
            catch { return null; }
        }
        public ObservableCollection<Cls_Sevk> PopulateSevkGuncelleList()
        {
            try
            {
                variables.Query = "select * from vbvSevkGuncelleMas";
                string formattedDate = string.Empty;
                reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil);

                DateTime dateTime;
                

				while (reader.Read())
                {
                    formattedDate = reader[1].ToString().Substring(0,10);
					DateTime.TryParse(formattedDate, out dateTime);
					Cls_Sevk sevkItem = new Cls_Sevk
                    {
                        
                        SevkEmriNo = reader[0].ToString(),
                        SevkEmriTarihi = dateTime,
						SevkAciklama = reader[2].ToString(),

                    };
                    temp_coll_sevk.Add(sevkItem);
                }
                SevkCollection = temp_coll_sevk;
                reader.Close();
                return SevkCollection;
            }

            catch
            {
                return null;
            }
        }
        public ObservableCollection<Cls_Sevk> PopulateSevkGuncelleListSatir(string sevkEmriNo)
        {
            try
            {
				SqlParameter[] parameter = new SqlParameter[1];

				parameter[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
				parameter[0].Value = sevkEmriNo;


				variables.Query = "select * from vbvSevkGuncelleSatir where belgeno=@sevkEmriNo";

                reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter);

                while (reader.Read())
                {
                    Cls_Sevk sevkItem = new Cls_Sevk
                    {
                        SevkSira = Convert.ToInt32(reader[0]),
                        SevkEmriNo = reader[1].ToString(),
                        SiparisKodu = reader[2].ToString(),
                        SiparisSira = Convert.ToInt32(reader[3]),
                        CariKodu = reader[4].ToString(),
                        CariAdi = reader[5].ToString(),
                        UrunKodu = reader[6].ToString(),
                        UrunAdi = reader[7].ToString(),
                        SiparisMiktar = Convert.ToInt32(reader[8]),
                        AcikSevkMiktar = Convert.ToInt32(reader[9]),
                        TeslimMiktar = Convert.ToInt32(reader[10]),
                        SevkMiktar = Convert.ToInt32(reader[11]),
                        DepoMiktar = Convert.ToInt32(reader[12]),
                    };
                    temp_coll_sevk.Add(sevkItem);
                }
                SevkCollection = temp_coll_sevk;
                reader.Close();
                return SevkCollection;
            }

            catch
            {
                return null;
            }
        }
        public ObservableCollection<Cls_Sevk> PopulatePopupToBeUpdatedSevkEmri(string sevkEmriNo)
        {
            try
            {
                variables.Query = "select * from vbvYuklenmemisSevkSatirEkle where belgeno = @belgeno";

                SqlParameter[] parameter = new SqlParameter[1];

                parameter[0] = new SqlParameter("@belgeno", SqlDbType.NVarChar, 15);
                parameter[0].Value = sevkEmriNo;


                reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter);

                while (reader.Read())
                {
                    
					string formattedDate = reader[15].ToString().Length > 9 ? reader[15].ToString().Substring(0, 10) : DateTime.Now.ToString().Substring(0,10);
                    DateTime dateTime;
					DateTime.TryParse(formattedDate, out dateTime);

					Cls_Sevk sevkItem = new Cls_Sevk
                    {

                        SevkSira = Convert.ToInt32(reader[0]),
                        SevkEmriNo = reader[1].ToString(),
                        CariKodu = reader[2].ToString(),
                        CariAdi = reader[3].ToString(),
                        SiparisKodu = reader[4].ToString(),
                        SiparisSira = Convert.ToInt32(reader[5]),
                        UrunKodu = reader[6].ToString(),
                        UrunAdi = reader[7].ToString(),
                        SevkMiktar = Convert.ToInt32(reader[8]),
                        UrunHacim = Convert.ToInt32(reader[9]),
                        UrunAgirlik = Convert.ToInt32(reader[10]),
                        SevkHacim = Convert.ToInt32(reader[11]),
                        SevkAgirlik = Convert.ToInt32(reader[12]),
                        PlakaNo = reader[13].ToString(),
						SoforIsim = reader[14].ToString(),
						SevkEmriTarihi = dateTime,
						SevkAciklama = reader[16].ToString(),

					};
                    temp_coll_sevk.Add(sevkItem);
                }
                SevkCollection = temp_coll_sevk;
                reader.Close();
                return SevkCollection;

            }
            catch
            {
                return null;
            }
        }
        public ObservableCollection<Cls_Sevk> PopulateCariReportCollectionForShipment (Dictionary<string,string> restrictionPairs)
        { 
        try 
	    {
                variables.Query = "select sevkEmrino,teslimCariKodu,teslimCari,sevkAciklama,kayitTarihi,kullaniciAdi,sum(sevkMiktar) as toplamMiktar " +
								   " from vbvSevkDurumRapor (nolock)  where kayitTarihi >= @baslangicTarih and kayitTarihi<= @bitisTarih ";


				variables.Counter = 2;


                if (restrictionPairs.ContainsKey("@sevkNo"))
                {
                    variables.Query += " and sevkEmriNo like '%' + @sevkNo + '%'";
                    variables.Counter++;
                }

                if (restrictionPairs.ContainsKey("@siparisNo"))
                {
                    variables.Query += " and siparisNo like '%' + @siparisNo + '%'";
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
                    variables.Query += " and teslimCari like '%' + @cariAdi + '%'";
                    variables.Counter++;
                }


                SqlParameter[] parameters = new SqlParameter[variables.Counter];

				parameters[0] = new("@baslangicTarih", SqlDbType.NVarChar, 10);
				parameters[0].Value = restrictionPairs["@baslangicTarih"];
				parameters[1] = new("@bitisTarih", SqlDbType.NVarChar, 10);
				parameters[1].Value = restrictionPairs["@bitisTarih"];

				variables.Counter = 2;
				if (restrictionPairs.ContainsKey("@sevkNo"))
				{
                    parameters[variables.Counter] = new("@sevkNo", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = restrictionPairs["@sevkNo"];
                    variables.Counter++;
				}
				if (restrictionPairs.ContainsKey("@siparisNo"))
				{
                    parameters[variables.Counter] = new("@siparisNo", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = restrictionPairs["@siparisNo"];
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

                temp_coll_sevk.Clear();
                SevkCollectionCariReport.Clear();

                variables.Query += " group by sevkEmrino,teslimCariKodu,teslimCari,sevkAciklama,kayitTarihi,kullaniciAdi";


				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
				{

                    while (reader.Read())
                    {
						login.UserID = login.GetUserIDFromPortalID(Convert.ToInt32(reader["kullaniciAdi"]));
						login.User = login.GetUserNameFromUserID(login.UserID);

						sevkEmriTarihi = !string.IsNullOrEmpty(reader["kayitTarihi"].ToString()) ?
													Convert.ToDateTime(reader["kayitTarihi"]).ToString("dd.MM.yyyy")
													: string.Empty;


						Cls_Sevk sevkItem = new Cls_Sevk
                        {
                            SevkEmriNo = reader["sevkEmrino"].ToString(),
                            CariKodu = reader["teslimCariKodu"].ToString(),
                            CariAdi = reader["teslimCari"].ToString(),
                            SevkAciklama = reader["sevkAciklama"].ToString(),
                            SevkEmriTarihi = Convert.ToDateTime(sevkEmriTarihi),
                            SevkToplamUrun = Convert.ToInt32(reader["toplamMiktar"]),
                            UserName = login.User,
                        };
                        temp_coll_sevk.Add(sevkItem);
                    }
                    reader.Close();
				}
		    SevkCollectionCariReport = temp_coll_sevk;
			return SevkCollectionCariReport;
		}
	    catch 
	    {
            return null;
	    }
		}
		public ObservableCollection<Cls_Sevk> PopulateSiparisReportCollectionForShipment(Dictionary<string, string> restrictionPairs, 
															                            string sevkEmriNo)
		{
			try
			{

                variables.Query = "select siparisNo,siparisSira,stokKodu,stokAdi,paketKodu,paketAdi,siparisMiktar," +
                    "depoMiktar,uretimMiktar from vbvSevkDurumRapor (nolock) where  " +
					" kayitTarihi >= @baslangicTarih and kayitTarihi <= @bitisTarih and sevkEmrino = @sevkEmriNo ";

				variables.Counter = 3;

				if (restrictionPairs.ContainsKey("@sevkNo"))
				{
					variables.Query += " and sevkEmriNo like '%' + @sevkNo + '%'";
					variables.Counter++;
				}

				if (restrictionPairs.ContainsKey("@siparisNo"))
				{
					variables.Query += " and siparisNo like '%' + @siparisNo + '%'";
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
					variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
					variables.Counter++;
				}


				SqlParameter[] parameters = new SqlParameter[variables.Counter];

				parameters[0] = new("@baslangicTarih", SqlDbType.NVarChar, 10);
				parameters[0].Value = restrictionPairs["@baslangicTarih"];
				parameters[1] = new("@bitisTarih", SqlDbType.NVarChar, 10);
				parameters[1].Value = restrictionPairs["@bitisTarih"];
				parameters[2] = new("@sevkEmriNo", SqlDbType.NVarChar, 15);
				parameters[2].Value = sevkEmriNo;

				variables.Counter = 3;

				if (restrictionPairs.ContainsKey("@sevkNo"))
				{
					parameters[variables.Counter] = new("@sevkNo", SqlDbType.NVarChar, 15);
					parameters[variables.Counter].Value = restrictionPairs["@sevkNo"];
					variables.Counter++;
				}

				if (restrictionPairs.ContainsKey("@siparisNo"))
				{
					parameters[variables.Counter] = new("@siparisNo", SqlDbType.NVarChar, 15);
					parameters[variables.Counter].Value = restrictionPairs["@siparisNo"];
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


				temp_coll_sevk.Clear();
				SevkCollectionSiparisReport.Clear();

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
				{
					while (reader.Read())
					{
						Cls_Sevk sevkItem = new Cls_Sevk
						{

							SiparisKodu = reader["siparisNo"].ToString(),
							SiparisSira = Convert.ToInt32(reader["siparisSira"]),
							UrunKodu = reader["stokKodu"].ToString(),
							UrunAdi = reader["stokAdi"].ToString(),
							PaketKodu = reader["paketKodu"].ToString(),
							PaketAdi = reader["paketAdi"].ToString(),
							SiparisMiktar = Convert.ToInt32(reader["siparisMiktar"]),
							DepoMiktar = Convert.ToInt32(reader["depoMiktar"]),
							UretimMiktar = Convert.ToInt32(reader["uretimMiktar"]),
						};
						temp_coll_sevk.Add(sevkItem);
					}
					reader.Close();
				}
				SevkCollectionSiparisReport = temp_coll_sevk;
				return SevkCollectionSiparisReport;
			}
			catch
			{
				return null;
			}
		}

		public ObservableCollection<Cls_Sevk> PopulateYuklemeReportCollectionForShipment(Dictionary<string, string> restrictionPairs,string siparisNo
                                                                                       ,int siparisSira)
		{
			try
			{
				variables.Query = "select yuklemeEmriNo,yukKayitTarihi,yuklemeMiktar from vbvSevkDurumRapor where " +
									" kayitTarihi >= @baslangicTarih and kayitTarihi <= @bitisTarih and siparisNo=@sipNo and siparisSira=@sipSatir ";
				variables.Counter = 4;

				if (restrictionPairs.ContainsKey("@sevkNo"))
				{
					variables.Query += " and sevkEmriNo like '%' + @sevkNo + '%'";
					variables.Counter++;
				}

				if (restrictionPairs.ContainsKey("@siparisNo"))
				{
					variables.Query += " and siparisNo like '%' + siparisNo + '%'";
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
					variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
					variables.Counter++;
				}


				SqlParameter[] parameters = new SqlParameter[variables.Counter];

				variables.Counter = 4;

				parameters[0] = new("@baslangicTarih", SqlDbType.NVarChar, 10);
				parameters[0].Value = restrictionPairs["@baslangicTarih"];
				parameters[1] = new("@bitisTarih", SqlDbType.NVarChar, 10);
				parameters[1].Value = restrictionPairs["@bitisTarih"];
				parameters[2] = new("@sipNo", SqlDbType.NVarChar, 15);
				parameters[2].Value = siparisNo;
				parameters[3] = new("@sipSatir", SqlDbType.Int);
				parameters[3].Value = siparisSira;

				if (restrictionPairs.ContainsKey("@sevkNo"))
				{
					parameters[variables.Counter] = new("@sevkNo", SqlDbType.NVarChar, 15);
					parameters[variables.Counter].Value = restrictionPairs["@sevkNo"];
					variables.Counter++;
				}
				if (restrictionPairs.ContainsKey("@siparisNo"))
				{
					parameters[variables.Counter] = new("@siparisNo", SqlDbType.NVarChar, 15);
					parameters[variables.Counter].Value = restrictionPairs["@siparisNo"];
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

				temp_coll_sevk.Clear();
				WholeReportCollection.Clear();

				variables.Query += " and siparisNo = '" + siparisNo + "'";

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
				{

					while (reader.Read())
					{
						Cls_Sevk sevkItem = new Cls_Sevk
						{
							YuklemeEmriNo = reader[0].ToString(),
							YuklemeTarih = Convert.ToDateTime(reader[1]),
							OkutulanMiktar = Convert.ToInt32(reader[2]),
						};
						temp_coll_sevk.Add(sevkItem);
					}
					reader.Close();
				}
				WholeReportCollection = temp_coll_sevk;
				return WholeReportCollection;
			}
			catch
			{
				return null;
			}
		}

		public ObservableCollection<Cls_Sevk> PopulateCariReportCollection (Dictionary<string,string> restrictionPairs, string restrictionQueries, string fabrika)
        { 
        try 
	    {
				if (restrictionPairs != null)
				{
					variables.Query = "select satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,SUM(tlTutar) as tlTutar,sum(dovizTutar) as dovizTutar, " +
								   " dovizTipi from vbvSevkRapor (nolock)  where 1 = 1 " + restrictionQueries;
                    variables.Counter = 0;

                

                    if (restrictionPairs.ContainsKey("@siparisNo"))
                    {
                        variables.Query += " and siparisNo like '%' + @siparisNo + '%'";
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
                        variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
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

                    temp_coll_sevk.Clear();
                    SevkCollectionCariReport.Clear();

                    variables.Query += " group by satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,dovizTipi";

				    using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,"Ahşap"))
				    {

                        while (reader.Read())
                        {
                            Cls_Sevk sevkItem = new Cls_Sevk
                            {
                                SatisCariKodu = reader["satisCariKodu"].ToString(),
                                SatisCariAdi = reader["satisCariIsmi"].ToString(),
                                CariKodu = reader["teslimCariKodu"].ToString(),
                                CariAdi = reader["teslimCariIsmi"].ToString(),
                                TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
                                DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
                                DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
                            };
                            temp_coll_sevk.Add(sevkItem);
                        }
                        reader.Close();
				    }
				}
                else
                {
                    variables.Query = "select satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,SUM(tlTutar) as tlTutar,sum(dovizTutar) as dovizTutar, " +
                                   " dovizTipi from vbvSevkRapor (nolock) group by satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,dovizTipi";


					temp_coll_sevk.Clear();
					SevkCollectionCariReport.Clear();
					using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil,"Ahşap"))
					{

						while (reader.Read())
						{
							Cls_Sevk sevkItem = new Cls_Sevk
							{
								SatisCariKodu = reader["satisCariKodu"].ToString(),
								SatisCariAdi = reader["satisCariIsmi"].ToString(),
								CariKodu = reader["teslimCariKodu"].ToString(),
								CariAdi = reader["teslimCariIsmi"].ToString(),
								TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
								DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
								DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
							};
							temp_coll_sevk.Add(sevkItem);
						}
						reader.Close();
					}
				}

			SevkCollectionCariReport = temp_coll_sevk;
			return SevkCollectionCariReport;
		}
	    catch 
	    {
            return null;
	    }
		}
		public ObservableCollection<Cls_Sevk> PopulateCariReportCollection (Dictionary<string,string> restrictionPairs, string restrictionQueries)
        { 
        try 
	    {
				if (restrictionPairs != null)
				{
					variables.Query = "select satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,SUM(tlTutar) as tlTutar,sum(dovizTutar) as dovizTutar, " +
								   " dovizTipi from vbvSevkRapor (nolock)  where 1 = 1 " + restrictionQueries;
                    variables.Counter = 0;

                

                    if (restrictionPairs.ContainsKey("@siparisNo"))
                    {
                        variables.Query += " and siparisNo like '%' + @siparisNo + '%'";
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
                        variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
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

                    temp_coll_sevk.Clear();
                    SevkCollectionCariReport.Clear();

                    variables.Query += " group by satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,dovizTipi";

				    using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
				    {

                        while (reader.Read())
                        {
                            Cls_Sevk sevkItem = new Cls_Sevk
                            {
                                SatisCariKodu = reader["satisCariKodu"].ToString(),
                                SatisCariAdi = reader["satisCariIsmi"].ToString(),
                                CariKodu = reader["teslimCariKodu"].ToString(),
                                CariAdi = reader["teslimCariIsmi"].ToString(),
                                TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
                                DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
                                DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
                            };
                            temp_coll_sevk.Add(sevkItem);
                        }
                        reader.Close();
				    }
				}
                else
                {
                    variables.Query = "select satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,SUM(tlTutar) as tlTutar,sum(dovizTutar) as dovizTutar, " +
                                   " dovizTipi from vbvSevkRapor (nolock) group by satisCariKodu,satisCariIsmi,teslimCariKodu,teslimCariIsmi,dovizTipi";


					temp_coll_sevk.Clear();
					SevkCollectionCariReport.Clear();
					using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil))
					{

						while (reader.Read())
						{
							Cls_Sevk sevkItem = new Cls_Sevk
							{
								SatisCariKodu = reader["satisCariKodu"].ToString(),
								SatisCariAdi = reader["satisCariIsmi"].ToString(),
								CariKodu = reader["teslimCariKodu"].ToString(),
								CariAdi = reader["teslimCariIsmi"].ToString(),
								TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
								DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
								DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
							};
							temp_coll_sevk.Add(sevkItem);
						}
						reader.Close();
					}
				}

			SevkCollectionCariReport = temp_coll_sevk;
			return SevkCollectionCariReport;
		}
	    catch 
	    {
            return null;
	    }
		}
		public ObservableCollection<Cls_Sevk> PopulateSiparisReportCollection (Dictionary<string,string> restrictionPairs, string restrictionQueries,
		                                                         string satisCariKodu, string satisCariAdi,string teslimCariKodu,string teslimCariAdi, string fabrika)
		{
			try 
	    {

                variables.Query = "select siparisNo, SUM(tlTutar) as tlTutar,sum(dovizTutar) as dovizTutar,  dovizTipi, destinasyon,poNo " +
                                   " from vbvSevkRapor(nolock)  where 1 = 1 " + restrictionQueries + " and satisCariKodu = '" + satisCariKodu + "' and " +
                                   " satisCariIsmi = '" + satisCariAdi + "' and teslimCariKodu = '" + teslimCariKodu + "' and teslimCariIsmi ='" + teslimCariAdi + "' ";

				variables.Counter = 0;

                if (restrictionPairs.ContainsKey("@siparisNo"))
                {
                    variables.Query += " and siparisNo like '%' + @siparisNo + '%'";
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
                    variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
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

                temp_coll_sevk.Clear();
                SevkCollectionSiparisReport.Clear();

                variables.Query += " group by siparisNo, destinasyon, poNo, dovizTipi";

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,fabrika))
				{

                    while (reader.Read())
                    {
                        Cls_Sevk sevkItem = new Cls_Sevk
                        {
                            SiparisKodu = reader["siparisNo"].ToString(),
                            PONo = reader["poNo"].ToString(),
                            Destinasyon = reader["destinasyon"].ToString(),
                            TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
                            DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
                            DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
                        };
                        temp_coll_sevk.Add(sevkItem);
                    }
                    reader.Close();
				}
		    SevkCollectionSiparisReport = temp_coll_sevk;
			return SevkCollectionSiparisReport;
		}
	    catch 
	    {
            return null;
	    }
		}
		public ObservableCollection<Cls_Sevk> PopulateSiparisReportCollection (Dictionary<string,string> restrictionPairs, string restrictionQueries,
		                                                         string satisCariKodu, string satisCariAdi,string teslimCariKodu,string teslimCariAdi)
		{
			try 
	    {

                variables.Query = "select siparisNo, SUM(tlTutar) as tlTutar,sum(dovizTutar) as dovizTutar,  dovizTipi, destinasyon,poNo " +
                                   " from vbvSevkRapor(nolock)  where 1 = 1 " + restrictionQueries + " and satisCariKodu = '" + satisCariKodu + "' and " +
                                   " satisCariIsmi = '" + satisCariAdi + "' and teslimCariKodu = '" + teslimCariKodu + "' and teslimCariIsmi ='" + teslimCariAdi + "' ";

				variables.Counter = 0;

                if (restrictionPairs.ContainsKey("@siparisNo"))
                {
                    variables.Query += " and siparisNo like '%' + @siparisNo + '%'";
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
                    variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
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

                temp_coll_sevk.Clear();
                SevkCollectionSiparisReport.Clear();

                variables.Query += " group by siparisNo, destinasyon, poNo, dovizTipi";

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
				{

                    while (reader.Read())
                    {
                        Cls_Sevk sevkItem = new Cls_Sevk
                        {
                            SiparisKodu = reader["siparisNo"].ToString(),
                            PONo = reader["poNo"].ToString(),
                            Destinasyon = reader["destinasyon"].ToString(),
                            TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
                            DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
                            DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
                        };
                        temp_coll_sevk.Add(sevkItem);
                    }
                    reader.Close();
				}
		    SevkCollectionSiparisReport = temp_coll_sevk;
			return SevkCollectionSiparisReport;
		}
	    catch 
	    {
            return null;
	    }
		}
		
		public ObservableCollection<Cls_Sevk> PopulateWholeReportCollection(Dictionary<string, string> restrictionPairs, string restrictionQueries,
                                                                             string siparisNo,string fabrika)
		{
			try
			{
				variables.Query = "select * from vbvSevkRapor where 1 = 1" + restrictionQueries;
				variables.Counter = 0;

				if (restrictionPairs.ContainsKey("@siparisNo"))
				{
					variables.Query += " and siparisNo like '%' + siparisNo + '%'";
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
					variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
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

				temp_coll_sevk.Clear();
				WholeReportCollection.Clear();

                variables.Query += " and siparisNo = '" + siparisNo + "'";

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,fabrika))
				{

					while (reader.Read())
					{
						Cls_Sevk sevkItem = new Cls_Sevk
						{
							SiparisKodu = reader[0].ToString(),
							SiparisSira = Convert.ToInt32(reader[1]),
							SiparisDurum = reader["siparisDurum"].ToString(),
							UrunKodu = reader["stokKodu"].ToString(),
							UrunAdi = reader["stokAdi"].ToString(),
							SiparisMiktar = Convert.ToInt32(reader["siparisMiktar"]),
							TeslimMiktar = Convert.ToInt32(reader["teslimMiktar"]),
							DepoMiktar = Convert.ToInt32(reader["depoMiktar"]),
							SiparisTarih = reader["siparisTarih"].ToString(),
							SiparisTalepTarih = reader["talepTarih"].ToString(),
							TeslimTarih = reader["teslimTarih"].ToString(),
							SatisCariKodu = reader["satisCariKodu"].ToString(),
							SatisCariAdi = reader["satisCariIsmi"].ToString(),
							CariKodu = reader["teslimCariKodu"].ToString(),
							CariAdi = reader["teslimCariIsmi"].ToString(),
							SiparisFiyatTlUrun = Convert.ToSingle(reader["tlTutarUrun"]),
							SiparisFiyatDovizUrun = Convert.ToSingle(reader["dovizTutarUrun"]),
							TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
							DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
							DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
							Destinasyon = reader["destinasyon"].ToString(),
							PONo = reader["poNo"].ToString(),
                            SiparisIsemrino = reader["isemrino"].ToString(),
                            SiparisOnEk = reader["onEk"].ToString(),
						};
						temp_coll_sevk.Add(sevkItem);
					}
					reader.Close();
				}
				WholeReportCollection = temp_coll_sevk;
				return WholeReportCollection;
			}
			catch
			{
				return null;
			}
		}
		public ObservableCollection<Cls_Sevk> PopulateWholeReportCollection(Dictionary<string, string> restrictionPairs, string restrictionQueries,
                                                                             string siparisNo)
		{
			try
			{
				variables.Query = "select * from vbvSevkRapor where 1 = 1" + restrictionQueries;
				variables.Counter = 0;

				if (restrictionPairs.ContainsKey("@siparisNo"))
				{
					variables.Query += " and siparisNo like '%' + siparisNo + '%'";
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
					variables.Query += " and teslimCariIsmi like '%' + @cariAdi + '%'";
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

				temp_coll_sevk.Clear();
				WholeReportCollection.Clear();

                variables.Query += " and siparisNo = '" + siparisNo + "'";

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
				{

					while (reader.Read())
					{
						Cls_Sevk sevkItem = new Cls_Sevk
						{
							SiparisKodu = reader[0].ToString(),
							SiparisSira = Convert.ToInt32(reader[1]),
							SiparisDurum = reader["siparisDurum"].ToString(),
							UrunKodu = reader["stokKodu"].ToString(),
							UrunAdi = reader["stokAdi"].ToString(),
							SiparisMiktar = Convert.ToInt32(reader["siparisMiktar"]),
							TeslimMiktar = Convert.ToInt32(reader["teslimMiktar"]),
							DepoMiktar = Convert.ToInt32(reader["depoMiktar"]),
							SiparisTarih = reader["siparisTarih"].ToString(),
							SiparisTalepTarih = reader["talepTarih"].ToString(),
							TeslimTarih = reader["teslimTarih"].ToString(),
							SatisCariKodu = reader["satisCariKodu"].ToString(),
							SatisCariAdi = reader["satisCariIsmi"].ToString(),
							CariKodu = reader["teslimCariKodu"].ToString(),
							CariAdi = reader["teslimCariIsmi"].ToString(),
							SiparisFiyatTlUrun = Convert.ToSingle(reader["tlTutarUrun"]),
							SiparisFiyatDovizUrun = Convert.ToSingle(reader["dovizTutarUrun"]),
							TlTutarToplamSiparis = Convert.ToSingle(reader["tlTutar"]),
							DovizTutarToplamSiparis = Convert.ToSingle(reader["dovizTutar"]),
							DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
							Destinasyon = reader["destinasyon"].ToString(),
							PONo = reader["poNo"].ToString(),
                            SiparisIsemrino = reader["isemrino"].ToString(),
						};
						temp_coll_sevk.Add(sevkItem);
					}
					reader.Close();
				}
				WholeReportCollection = temp_coll_sevk;
				return WholeReportCollection;
			}
			catch
			{
				return null;
			}
		}
        
		public ObservableCollection<Cls_Sevk> PopulateInvoiceCollection(string siparisNo,string fabrika)
		{
			try
			{
				variables.Query = "select * from vbvCariRaporExcelAktar where siparisNo = @siparisNo";
				
				SqlParameter[] parameters = new SqlParameter[1];

					parameters[0] = new("@siparisNo", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = siparisNo;

				temp_coll_sevk.Clear();
				InvoiceCollection.Clear();

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,fabrika))
				{

					while (reader.Read())
					{
						Cls_Sevk sevkItem = new Cls_Sevk
						{
							SiparisKodu = reader["siparisNo"].ToString(),
							SiparisSira = Convert.ToInt32(reader["siparisSira"]),
							SiparisTarih = reader["siparisTarih"].ToString(),
							CariKodu = reader["cariKod"].ToString(),
							CariAdi = reader["cariIsim"].ToString(),
							CariAdres = reader["adres"].ToString(),
							CariTel = reader["tel"].ToString(),
							CariEmail = reader["email"].ToString(),
							UrunKodu = reader["urunKodu"].ToString(),
							UrunAdi = reader["urunAdi"].ToString(),
							GTIPNo = reader["GTIPNo"].ToString(),
							SiparisMiktar = Convert.ToInt32(reader["siparisMiktar"]),
							SiparisFiyatDovizUrun = Convert.ToSingle(reader["birimFiyat"]),
							DovizTutarToplamSiparis = Convert.ToSingle(reader["toplamFiyat"]),
							DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
							
						};
						temp_coll_sevk.Add(sevkItem);
					}
					reader.Close();
				}
				InvoiceCollection = temp_coll_sevk;
				return InvoiceCollection;
			}
			catch
			{
				return null;
			}
		}
		public ObservableCollection<Cls_Sevk> PopulateInvoiceCollection(string siparisNo)
		{
			try
			{
				variables.Query = "select * from vbvCariRaporExcelAktar where siparisNo = @siparisNo";
				
				SqlParameter[] parameters = new SqlParameter[1];

					parameters[0] = new("@siparisNo", SqlDbType.NVarChar, 15);
                    parameters[variables.Counter].Value = siparisNo;

				temp_coll_sevk.Clear();
				InvoiceCollection.Clear();

				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
				{

					while (reader.Read())
					{
						Cls_Sevk sevkItem = new Cls_Sevk
						{
							SiparisKodu = reader["siparisNo"].ToString(),
							SiparisSira = Convert.ToInt32(reader["siparisSira"]),
							SiparisTarih = reader["siparisTarih"].ToString(),
							CariKodu = reader["cariKod"].ToString(),
							CariAdi = reader["cariIsim"].ToString(),
							CariAdres = reader["adres"].ToString(),
							CariTel = reader["tel"].ToString(),
							CariEmail = reader["email"].ToString(),
							UrunKodu = reader["urunKodu"].ToString(),
							UrunAdi = reader["urunAdi"].ToString(),
							GTIPNo = reader["GTIPNo"].ToString(),
							SiparisMiktar = Convert.ToInt32(reader["siparisMiktar"]),
							SiparisFiyatDovizUrun = Convert.ToSingle(reader["birimFiyat"]),
							DovizTutarToplamSiparis = Convert.ToSingle(reader["toplamFiyat"]),
							DovizTipi = (Cls_Base.DovizTipi)Convert.ToInt32(reader["dovizTipi"]),
							
						};
						temp_coll_sevk.Add(sevkItem);
					}
					reader.Close();
				}
				InvoiceCollection = temp_coll_sevk;
				return InvoiceCollection;
			}
			catch
			{
				return null;
			}
		}

		public ObservableCollection<Cls_Sevk> GetProductVolumeAndWeight(string productCode, string fabrika)
        {
            try
            {



                variables.Query = "select * from vbvUrunHacimAgirlikBilgisiGetir where stok_kodu=@urunKodu";
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@urunKodu", SqlDbType.NVarChar, 35);
                parameters[0].Value = productCode;

                temp_coll_sevk.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,fabrika))
                {
                    if (reader.Read())
                    {
                        Cls_Sevk sevkItem = new Cls_Sevk
                        {
                            UrunHacim = Convert.ToSingle(reader[1]),
                            UrunAgirlik = Convert.ToSingle(reader[2]),
                        };
                        temp_coll_sevk.Add(sevkItem);
                    }

                }

                AgirlikHacimCollection = temp_coll_sevk;
                return AgirlikHacimCollection;

            }

            catch { return null; }
        }
		public ObservableCollection<Cls_Sevk> GetProductVolumeAndWeight(string productCode)
        {
            try
            {



                variables.Query = "select * from vbvUrunHacimAgirlikBilgisiGetir where stok_kodu=@urunKodu";
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@urunKodu", SqlDbType.NVarChar, 35);
                parameters[0].Value = productCode;

                temp_coll_sevk.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
                {
                    if (reader.Read())
                    {
                        Cls_Sevk sevkItem = new Cls_Sevk
                        {
                            UrunHacim = Convert.ToSingle(reader[1]),
                            UrunAgirlik = Convert.ToSingle(reader[2]),
                        };
                        temp_coll_sevk.Add(sevkItem);
                    }

                }

                AgirlikHacimCollection = temp_coll_sevk;
                return AgirlikHacimCollection;

            }

            catch { return null; }
        }

        public ObservableCollection<Cls_Sevk> GetExcelReportCollectionForUrunToplama(string sevkEmriNo)
        {
            try
            {
                variables.Query = "select distinct sevkEmriNo, kayitTarihi,stokKodu,stokAdi,paketKodu,paketAdi,sevkAciklama,sevkMiktar,depoMiktar" +
								  " ingilizceIsim, teslimCariKodu,teslimCari from vbvSevkDurumRapor (nolock) where sevkEmriNo=@sevkEmriNo";

                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@sevkEmriNo",SqlDbType.NVarChar,15);
                parameter[0].Value = sevkEmriNo;
                temp_coll_sevk.Clear() ;
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter))
                {
                    while (reader.Read())
                    {
                        Cls_Sevk item = new Cls_Sevk
                        {
                            SevkEmriNo = reader[0].ToString(),
                            SevkTarihi = Convert.ToDateTime(reader[1]),
                            UrunKodu = reader[2].ToString(),
                            UrunAdi = reader[3].ToString(),
                            PaketKodu = reader[4].ToString(),
                            PaketAdi = reader[5].ToString(),
                            SevkAciklama = reader[6].ToString(),
                            SevkMiktar = Convert.ToInt32(reader[7]),
                            DepoMiktar = Convert.ToInt32(reader[8]),
                            IngilizceIsim = reader["ingilizceIsim"].ToString(),
                            CariKodu = reader["teslimCariKodu"].ToString(),
                            CariAdi = reader["teslimCari"].ToString(),
                        };
                        temp_coll_sevk.Add(item);
                    }
                };
				ObservableCollection<Cls_Sevk> excelCollection = temp_coll_sevk ;

                return excelCollection;
			}
            catch 
            {
                return null;
            }
        }

		public string InsertShipmentsSSH(ObservableCollection<Cls_Sevk> shipments)
		{
			try
			{
                //sevk emri no al
				SqlParameter[] parameter = new SqlParameter[3];

				parameter[0] = new SqlParameter("@prefix", SqlDbType.NVarChar, 3);
				parameter[0].Value = "SE";
				parameter[1] = new SqlParameter("@tableName", SqlDbType.NVarChar, 128);
				parameter[1].Value = "TBLSEVKTRA";
				parameter[2] = new SqlParameter("@columnName", SqlDbType.NVarChar, 128);
				parameter[2].Value = "BELGENO";

				SevkEmriNo = data.Get_One_String_Result_Stored_Proc_With_Parameters("vbpGetFisno", variables.Yil, parameter);
                if (string.IsNullOrEmpty(SevkEmriNo))
                    return "Sevk Bilgileri Alınırken";

				//sevk irsaliye no al
				SevkIrsaliyeNo = data.Get_One_String_Result_Stored_Proc("vbpGetSevkIrsaliyeNo", variables.Yil);
                if (string.IsNullOrEmpty(SevkIrsaliyeNo))
                   return "Sevk Bilgileri Alınırken";

				SiparisKodu = shipments.Select(x => x.SiparisKodu).Any() ? shipments.Select(x => x.SiparisKodu).FirstOrDefault() : string.Empty;
				SiparisSira = shipments.Select(x => x.SiparisSira).Any() ? shipments.Select(x => x.SiparisSira).FirstOrDefault() : 0;
				UrunKodu = shipments.Select(x => x.UrunKodu).Any() ? shipments.Select(x => x.UrunKodu).FirstOrDefault() : string.Empty;
				CariKodu = shipments.Select(x => x.CariKodu).Any() ? shipments.Select(x => x.CariKodu).FirstOrDefault() : string.Empty;

				if (string.IsNullOrEmpty(SiparisKodu) ||
					SiparisSira == 0 ||
					string.IsNullOrEmpty(UrunKodu) ||
                    string.IsNullOrEmpty(CariKodu))
					return "Sevk Bilgileri Alınırken";

				//satış cari kodu al
				variables.Query = "Select STHAR_ACIKLAMA from tblsipatra where fisno=@fisno and stra_sipkont=@sira and stok_kodu=@stokKodu";
				SqlParameter[] parametersForSatisCari = new SqlParameter[3];

				parametersForSatisCari[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
				parametersForSatisCari[0].Value = SiparisKodu;
				parametersForSatisCari[1] = new SqlParameter("@sira", SqlDbType.TinyInt);
				parametersForSatisCari[1].Value = SiparisSira;
				parametersForSatisCari[2] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
				parametersForSatisCari[2].Value = UrunKodu;

				reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parametersForSatisCari);
                SatisCariKodu = string.Empty;
                while (reader.Read())
				{
					if (reader.HasRows)
                    { 
						SatisCariKodu = reader[0].ToString();
                        if(string.IsNullOrEmpty(SatisCariKodu)) return "Sevk Bilgileri Alınırken";
					}

					else
					{
						variables.Result = false;
						return "Sevk Bilgileri Alınırken";
					}
				}
                if (string.IsNullOrEmpty(SatisCariKodu))
                    return "Sevk Bilgileri Alınırken";
				reader.Close();

                //sevk emri mas
                SqlParameter[] parameters = new SqlParameter[8];

                SevkEmriTarihi = shipments.Select(x => x.SevkEmriTarihi).FirstOrDefault();
                SevkAciklama = shipments.Select(x => x.SevkAciklama).Any() ? shipments.Select(x => x.SevkAciklama).FirstOrDefault() : string.Empty;
                SoforIsim = shipments.Select(x => x.SoforIsim).Any() ? shipments.Select(x => x.SoforIsim).FirstOrDefault() : string.Empty;
                PlakaNo = shipments.Select(x => x.PlakaNo).Any() ? shipments.Select(x => x.PlakaNo).FirstOrDefault() : string.Empty;
                SevkHacim = shipments.Select(x => x.SevkHacim).Any() ? shipments.Select(x => x.SevkHacim).FirstOrDefault() : 0;
                SevkAgirlik = shipments.Select(x => x.SevkAgirlik).Any() ? shipments.Select(x => x.SevkAgirlik).FirstOrDefault() : 0;


                string user = login.GetPortalID().ToString();

                parameters[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                parameters[0].Value = SevkEmriNo;
                parameters[1] = new SqlParameter("@sevkEmriTarihi", SqlDbType.DateTime);
                parameters[1].Value = SevkEmriTarihi;
                parameters[2] = new SqlParameter("@userID", SqlDbType.NVarChar, 3);
                parameters[2].Value = user;
                parameters[3] = new SqlParameter("@sevkAciklama", SqlDbType.NVarChar, 500);
                parameters[3].Value = SevkAciklama;
                parameters[4] = new SqlParameter("@soforIsim", SqlDbType.NVarChar, 500);
                parameters[4].Value = SoforIsim;
                parameters[5] = new SqlParameter("@plakaNo", SqlDbType.NVarChar, 30);
                parameters[5].Value = PlakaNo;
                parameters[6] = new SqlParameter("@sevkHacim", SqlDbType.Decimal);
                parameters[6].Value = SevkHacim;
                parameters[7] = new SqlParameter("@sevkAgirlik", SqlDbType.Decimal);
                parameters[7].Value = SevkAgirlik;

                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertSevkEmriGenel", variables.Yil, parameters);

                if (!variables.Result)
                    return "Sevk Genel Bilgileri Kaydedilirken";
                //sevk emri satır
                foreach (Cls_Sevk item in shipments)
                {
                    SqlParameter[] parametersForRows = new SqlParameter[10];

                    parametersForRows[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                    parametersForRows[0].Value = SevkEmriNo;
                    parametersForRows[1] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
                    parametersForRows[1].Value = item.SiparisKodu;
                    parametersForRows[2] = new SqlParameter("@siparisSira", SqlDbType.TinyInt);
                    parametersForRows[2].Value = item.SiparisSira;
                    parametersForRows[3] = new SqlParameter("@teslimCari", SqlDbType.NVarChar, 15);
                    parametersForRows[3].Value = item.CariKodu;
                    parametersForRows[4] = new SqlParameter("@sira", SqlDbType.TinyInt);
                    parametersForRows[4].Value = item.SevkSira;
                    parametersForRows[5] = new SqlParameter("@miktar", SqlDbType.Int);
                    parametersForRows[5].Value = item.SevkMiktar;
                    parametersForRows[6] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parametersForRows[6].Value = item.UrunKodu;
                    parametersForRows[7] = new SqlParameter("@userId", SqlDbType.NVarChar, 5);
                    parametersForRows[7].Value = user;
                    parametersForRows[8] = new SqlParameter("@urunHacim", SqlDbType.Float);
                    parametersForRows[8].Value = item.UrunHacim;
                    parametersForRows[9] = new SqlParameter("@urunAgirlik", SqlDbType.Float);
                    parametersForRows[9].Value = item.UrunAgirlik;
                    variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertSevkEmriSatir", variables.Yil, parametersForRows);

                    if (!variables.Result)
                        return "Sevk Satır Bilgileri Kaydedilirken";
                }
                int userID = login.GetPortalID();
                //yükleme mas
                SqlParameter[] parametersYuklemeMas = new SqlParameter[2];
				parametersYuklemeMas[0] = new SqlParameter("@userID", SqlDbType.SmallInt);
				parametersYuklemeMas[0].Value = userID;
				parametersYuklemeMas[1] = new SqlParameter("@belgeNo", SqlDbType.NVarChar, 15);
				parametersYuklemeMas[1].Value = SevkEmriNo;
				variables.Result = data.ExecuteStoredProcedureWithParameters("vbpYuklemeKayitMas", variables.Yil, parametersYuklemeMas);
				    if (!variables.Result)
					    return "Yükleme Genel Bilgileri Kaydedilirken";

                //yükleme satır
                foreach (Cls_Sevk item in shipments)
                {
                    SqlParameter[] parametersYuklemeSatir = new SqlParameter[9];

                    parametersYuklemeSatir[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                    parametersYuklemeSatir[0].Value = SevkEmriNo;
                    parametersYuklemeSatir[1] = new SqlParameter("@sipNo", SqlDbType.NVarChar, 15);
                    parametersYuklemeSatir[1].Value = item.SiparisKodu;
                    parametersYuklemeSatir[2] = new SqlParameter("@sipKont", SqlDbType.TinyInt);
                    parametersYuklemeSatir[2].Value = item.SiparisSira;
                    parametersYuklemeSatir[3] = new SqlParameter("@teslimCari", SqlDbType.NVarChar, 15);
                    parametersYuklemeSatir[3].Value = item.CariKodu;
                    parametersYuklemeSatir[4] = new SqlParameter("@sira", SqlDbType.TinyInt);
                    parametersYuklemeSatir[4].Value = item.SevkSira;
                    parametersYuklemeSatir[5] = new SqlParameter("@miktar", SqlDbType.Int);
                    parametersYuklemeSatir[5].Value = item.SevkMiktar;
                    parametersYuklemeSatir[6] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parametersYuklemeSatir[6].Value = item.UrunKodu;
                    parametersYuklemeSatir[7] = new SqlParameter("@userId", SqlDbType.NVarChar, 5);
                    parametersYuklemeSatir[7].Value = user;
                    parametersYuklemeSatir[8] = new SqlParameter("@sevkIrsaliyeNo", SqlDbType.NVarChar, 15);
                    parametersYuklemeSatir[8].Value = SevkIrsaliyeNo;

                    variables.Result = data.ExecuteStoredProcedureWithParameters("vbpYuklemeKayitSatir", variables.Yil, parametersYuklemeSatir);
                    if (!variables.Result)
                        return "Yükleme Satır Bilgileri Kaydedilirken";
                }
				//İrsaliye Kaydet
				SqlParameter[] parametersIrsaliye = new SqlParameter[4];
				parametersIrsaliye[0] = new SqlParameter("@satisCariKodu", SqlDbType.NVarChar, 15);
				parametersIrsaliye[0].Value = SatisCariKodu;
				parametersIrsaliye[1] = new SqlParameter("@cariKodu", SqlDbType.NVarChar, 15);
				parametersIrsaliye[1].Value = CariKodu;
				parametersIrsaliye[2] = new SqlParameter("@kullaniciKodu", SqlDbType.SmallInt);
				parametersIrsaliye[2].Value = userID;
				parametersIrsaliye[3] = new SqlParameter("@sevkIrsaliyesiNo", SqlDbType.NVarChar, 15);
				parametersIrsaliye[3].Value = SevkIrsaliyeNo;
				variables.Result = data.ExecuteStoredProcedureWithParameters("vbpElTerminaliIrsaliyeKaydet", variables.Yil, parametersIrsaliye);
				if (!variables.Result)
					return "Yükleme İrsaliye Bilgileri Kaydedilirken";

				return "Başarı";
			}
			catch { return string.Empty; }
		}

		public bool InsertShipments(ObservableCollection<Cls_Sevk> shipments)
        {
            try
            {
				SqlParameter[] parameter = new SqlParameter[3];

				parameter[0] = new SqlParameter("@prefix", SqlDbType.NVarChar, 3);
				parameter[0].Value = "SE";
				parameter[1] = new SqlParameter("@tableName", SqlDbType.NVarChar, 128);
				parameter[1].Value = "TBLSEVKTRA";
				parameter[2] = new SqlParameter("@columnName", SqlDbType.NVarChar, 128);
				parameter[2].Value = "BELGENO";

				SevkEmriNo = data.Get_One_String_Result_Stored_Proc_With_Parameters("vbpGetFisno", variables.Yil, parameter);

				SqlParameter[] parameters = new SqlParameter[8];

                SevkEmriTarihi = shipments.Select(x => x.SevkEmriTarihi).FirstOrDefault();
                SevkAciklama = shipments.Select(x => x.SevkAciklama).Any() ? shipments.Select(x => x.SevkAciklama).FirstOrDefault() : string.Empty;
                SoforIsim = shipments.Select(x => x.SoforIsim).Any() ? shipments.Select(x => x.SoforIsim).FirstOrDefault() : string.Empty;
                PlakaNo = shipments.Select(x => x.PlakaNo).Any() ? shipments.Select(x => x.PlakaNo).FirstOrDefault() : string.Empty;
                SevkHacim = shipments.Select(x => x.SevkHacim).Any() ? shipments.Select(x => x.SevkHacim).FirstOrDefault() : 0;
                SevkAgirlik = shipments.Select(x => x.SevkAgirlik).Any() ? shipments.Select(x => x.SevkAgirlik).FirstOrDefault() : 0;

                string user = login.GetPortalID().ToString();

                parameters[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                parameters[0].Value = SevkEmriNo;
                parameters[1] = new SqlParameter("@sevkEmriTarihi", SqlDbType.DateTime);
                parameters[1].Value = SevkEmriTarihi;
                parameters[2] = new SqlParameter("@userID", SqlDbType.NVarChar, 3);
                parameters[2].Value = user;
                parameters[3] = new SqlParameter("@sevkAciklama", SqlDbType.NVarChar, 500);
                parameters[3].Value = SevkAciklama;
                parameters[4] = new SqlParameter("@soforIsim", SqlDbType.NVarChar, 500);
                parameters[4].Value = SoforIsim;
                parameters[5] = new SqlParameter("@plakaNo", SqlDbType.NVarChar, 30);
                parameters[5].Value = PlakaNo;
                parameters[6] = new SqlParameter("@sevkHacim", SqlDbType.Decimal);
                parameters[6].Value = SevkHacim;
                parameters[7] = new SqlParameter("@sevkAgirlik", SqlDbType.Decimal);
                parameters[7].Value = SevkAgirlik;

                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertSevkEmriGenel", variables.Yil, parameters);

                if (!variables.Result)
                    return variables.Result;

                foreach (Cls_Sevk item in shipments)
                {
                    SqlParameter[] parametersForRows = new SqlParameter[10];

                    parametersForRows[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                    parametersForRows[0].Value = SevkEmriNo;
                    parametersForRows[1] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
                    parametersForRows[1].Value = item.SiparisKodu;
                    parametersForRows[2] = new SqlParameter("@siparisSira", SqlDbType.TinyInt);
                    parametersForRows[2].Value = item.SiparisSira;
                    parametersForRows[3] = new SqlParameter("@teslimCari", SqlDbType.NVarChar, 15);
                    parametersForRows[3].Value = item.CariKodu;
                    parametersForRows[4] = new SqlParameter("@sira", SqlDbType.TinyInt);
                    parametersForRows[4].Value = item.SevkSira;
                    parametersForRows[5] = new SqlParameter("@miktar", SqlDbType.Int);
                    parametersForRows[5].Value = item.SevkMiktar;
                    parametersForRows[6] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                    parametersForRows[6].Value = item.UrunKodu;
                    parametersForRows[7] = new SqlParameter("@userId", SqlDbType.NVarChar, 5);
                    parametersForRows[7].Value = user;
                    parametersForRows[8] = new SqlParameter("@urunHacim", SqlDbType.Float);
                    parametersForRows[8].Value = item.UrunHacim;
                    parametersForRows[9] = new SqlParameter("@urunAgirlik", SqlDbType.Float);
                    parametersForRows[9].Value = item.UrunAgirlik;
                    variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertSevkEmriSatir", variables.Yil, parametersForRows);

                    if (!variables.Result)
                        return variables.Result;
                }

                return variables.Result;
            }
            catch { return false; }
        }
		public bool InsertShipmentsToExistingOrder(ObservableCollection<Cls_Sevk> shipments)
		{
			try
			{
                variables.Result = UpdateSevkMas(shipments);

			    var sevkEmrino = shipments.Select(shipment => shipment.SevkEmriNo).FirstOrDefault();
                variables.Counter = CountSevkSira(sevkEmrino);

                if (variables.Counter == -1)
                    return false;


				foreach (Cls_Sevk item in shipments)
				{
					int returnedNumber = CheckIfSevkSatirExists(sevkEmrino, item.SiparisKodu, item.SiparisSira);

					if (returnedNumber == 1)
						continue;
					if (returnedNumber == 3)
						return false;

					SqlParameter[] parametersForRows = new SqlParameter[10];

					parametersForRows[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
					parametersForRows[0].Value = sevkEmrino;
					parametersForRows[1] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
					parametersForRows[1].Value = item.SiparisKodu;
					parametersForRows[2] = new SqlParameter("@siparisSira", SqlDbType.TinyInt);
					parametersForRows[2].Value = item.SiparisSira;
					parametersForRows[3] = new SqlParameter("@teslimCari", SqlDbType.NVarChar, 15);
					parametersForRows[3].Value = item.CariKodu;
					parametersForRows[4] = new SqlParameter("@sira", SqlDbType.TinyInt);
					parametersForRows[4].Value = variables.Counter;
					parametersForRows[5] = new SqlParameter("@miktar", SqlDbType.Int);
					parametersForRows[5].Value = item.SevkMiktar;
					parametersForRows[6] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
					parametersForRows[6].Value = item.UrunKodu;
					parametersForRows[7] = new SqlParameter("@userId", SqlDbType.NVarChar, 5);
					parametersForRows[7].Value = login.GetPortalID();
					parametersForRows[8] = new SqlParameter("@urunHacim", SqlDbType.Decimal);
					parametersForRows[8].Value = item.UrunHacim;
					parametersForRows[9] = new SqlParameter("@urunAgirlik", SqlDbType.Decimal);
					parametersForRows[9].Value = item.UrunAgirlik;
					variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertSevkEmriSatir", variables.Yil, parametersForRows);

					if (!variables.Result)
						return variables.Result;
                    
                    variables.Counter++;
				}

				SqlParameter[] parametersSiraOrder = new SqlParameter[1];
				parametersSiraOrder[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar,15);
				parametersSiraOrder[0].Value = sevkEmrino;
                variables.Query = "select sira,sipno,sipkont,belgeno from tblsevktra where belgeno=@sevkEmriNo order by inckeyno asc";
                reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query,variables.Yil,parametersSiraOrder);

                variables.Counter = 0;
                while(reader.Read())
                {
					SqlParameter[] parametersUpdate = new SqlParameter[4];
					parametersUpdate[0] = new SqlParameter("@sira", SqlDbType.Int);
					parametersUpdate[0].Value = variables.Counter + 1;
					parametersUpdate[1] = new SqlParameter("@sipno", SqlDbType.NVarChar, 15);
					parametersUpdate[1].Value = reader[1].ToString();
					parametersUpdate[2] = new SqlParameter("@sipkont", SqlDbType.Int);
					parametersUpdate[2].Value = Convert.ToInt32(reader[2]);
					parametersUpdate[3] = new SqlParameter("@belgeno", SqlDbType.NVarChar, 15);
					parametersUpdate[3].Value = reader[3].ToString();

					variables.Query = "UPDATE tblsevktra SET sira = @sira WHERE sipno = @sipno AND sipkont = @sipkont AND belgeno = @belgeno";

					variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parametersUpdate);

                    variables.Counter++;

                    if (!variables.Result)
                        return false;
				}
                
				return variables.Result;
			}
			catch { return false; }
		}
		public bool UpdateYuklenmemisSevkMiktar(string sevkEmriNo, string siparisNo, int siparisSira, int sevkMiktar)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@sevkMiktar", SqlDbType.Int);
                parameters[0].Value = sevkMiktar;
                parameters[1] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                parameters[1].Value = sevkEmriNo;
                parameters[2] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
                parameters[2].Value = siparisNo;
                parameters[3] = new SqlParameter("@siparisSira", SqlDbType.SmallInt);
                parameters[3].Value = siparisSira;

                variables.Query = "update TBLSEVKTRA SET MIKTAR=@sevkMiktar where BELGENO=@sevkEmriNo and SIPNO=@siparisNo and sipkont=@siparisSira";

                variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);

                if (!variables.Result) return false;

				SqlParameter[] parameter = new SqlParameter[1];
				parameter[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
				parameter[0].Value = sevkEmriNo;

                variables.Query = "select f_yedek1,f_yedek2,miktar from tblsevktra where belgeno=@sevkEmriNo";

                reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query,variables.Yil, parameter);
                
                float toplamHacim = 0, toplamAgirlik = 0;
                
                while(reader.Read())
                {
                    toplamHacim += Convert.ToSingle(reader[0]) * Convert.ToInt32(reader[2]);
                    toplamAgirlik += Convert.ToSingle(reader[1]) * Convert.ToInt32(reader[2]);
                }

				SqlParameter[] parametermas = new SqlParameter[3];
				parametermas[0] = new SqlParameter("@toplamHacim", SqlDbType.Float);
				parametermas[0].Value = toplamHacim;
				parametermas[1] = new SqlParameter("@toplamAgirlik", SqlDbType.Float);
				parametermas[1].Value = toplamAgirlik;
				parametermas[2] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
				parametermas[2].Value = sevkEmriNo;
                variables.Query = "update tblsevkmas set f_yedek1 = @toplamHacim, f_yedek2 = @toplamAgirlik where belgeno=@sevkEmriNo";

				variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parametermas);
				if (!variables.Result) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateSevkMas(ObservableCollection<Cls_Sevk> shipments)
        {
            try
            {
				SqlParameter[] parameters = new SqlParameter[8];

				SevkEmriNo = shipments.Select(x => x.SevkEmriNo).Any() ? shipments.Select(x => x.SevkEmriNo).FirstOrDefault() : string.Empty;
				SevkEmriTarihi = shipments.Select(x => x.SevkEmriTarihi).FirstOrDefault();
				SevkAciklama = shipments.Select(x => x.SevkAciklama).Any() ? shipments.Select(x => x.SevkAciklama).FirstOrDefault() : string.Empty;
				SoforIsim = shipments.Select(x => x.SoforIsim).Any() ? shipments.Select(x => x.SoforIsim).FirstOrDefault() : string.Empty;
				PlakaNo = shipments.Select(x => x.PlakaNo).Any() ? shipments.Select(x => x.PlakaNo).FirstOrDefault() : string.Empty;
				SevkHacim = shipments.Select(x => x.SevkHacim).Any() ? shipments.Select(x => x.SevkHacim).FirstOrDefault() : 0;
				SevkAgirlik = shipments.Select(x => x.SevkAgirlik).Any() ? shipments.Select(x => x.SevkAgirlik).FirstOrDefault() : 0;

				string user = login.GetPortalID().ToString();

				if (string.IsNullOrEmpty(user) ||
					string.IsNullOrEmpty(SevkEmriNo))
					return false;

				parameters[0] = new SqlParameter("@sevkEmriTarihi", SqlDbType.DateTime);
				parameters[0].Value = SevkEmriTarihi;
				parameters[1] = new SqlParameter("@userID", SqlDbType.NVarChar, 3);
				parameters[1].Value = user;
				parameters[2] = new SqlParameter("@sevkAciklama", SqlDbType.NVarChar, 500);
				parameters[2].Value = SevkAciklama;
				parameters[3] = new SqlParameter("@soforIsim", SqlDbType.NVarChar, 500);
				parameters[3].Value = SoforIsim;
				parameters[4] = new SqlParameter("@plakaNo", SqlDbType.NVarChar, 30);
				parameters[4].Value = PlakaNo;
				parameters[5] = new SqlParameter("@sevkHacim", SqlDbType.Decimal);
				parameters[5].Value = SevkHacim;
				parameters[6] = new SqlParameter("@sevkAgirlik", SqlDbType.Decimal);
				parameters[6].Value = SevkAgirlik;
				parameters[7] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
				parameters[7].Value = SevkEmriNo;

				variables.Query = "update tblsevkmas set tarih=@sevkEmriTarihi, acik1=@SevkAciklama,kamyonno=@plakaNo,duzeltmeyapankul=@userID, " +
					"duzeltmetarihi=getdate(),f_yedek1=@sevkHacim, f_yedek2=@sevkAgirlik, soforIsim=@soforIsim where tip=1 and belgeno=@sevkEmriNo";

				variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);

				if (!variables.Result)
					return variables.Result;

                return variables.Result;
			}
            catch (Exception)
            {
                return false;
            }
        }
        public ObservableCollection<Cls_Sevk> UpdateSevkSira(ObservableCollection<Cls_Sevk> reOrderedSevkSira)
        {
            try
            {
                if (!reOrderedSevkSira.Any())
                    return null;

                foreach (Cls_Sevk item in reOrderedSevkSira)
                {
                    SqlParameter[] parameter = new SqlParameter[4];
                    parameter[0] = new SqlParameter("@sevkEmrino", SqlDbType.NVarChar, 15);
                    parameter[0].Value = item.SevkEmriNo;
                    parameter[1] = new SqlParameter("@siparisKodu", SqlDbType.NVarChar, 15);
                    parameter[1].Value = item.SiparisKodu;
                    parameter[2] = new SqlParameter("@siparisSira", SqlDbType.Int);
                    parameter[2].Value = item.SiparisSira;
                    parameter[3] = new SqlParameter("@sevkSira", SqlDbType.Int);
                    parameter[3].Value = item.SevkSira;

                    variables.Query = "update tblsevktra set sira = @sevkSira where belgeno = @sevkEmriNo " +
                                      "and sipno = @siparisKodu and sipkont = @siparisSira";

                    variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameter);

                    if (!variables.Result)
                        return null;
                }

                ReOrderedSevkSira = GetReorderedSevkSira(reOrderedSevkSira);

                return ReOrderedSevkSira;
            }
            catch
            {
                return null;
            }
        }

		public ObservableCollection<Cls_Sevk> ReOrderSevkSira(ObservableCollection<Cls_Sevk> toBereOrderedSevkSira)
		{
			try
			{
				if (!toBereOrderedSevkSira.Any())
					return null;

                variables.Counter = 0;
				foreach (Cls_Sevk item in toBereOrderedSevkSira)
				{
					SqlParameter[] parameter = new SqlParameter[4];
					parameter[0] = new SqlParameter("@sevkEmrino", SqlDbType.NVarChar, 15);
					parameter[0].Value = item.SevkEmriNo;
					parameter[1] = new SqlParameter("@siparisKodu", SqlDbType.NVarChar, 15);
					parameter[1].Value = item.SiparisKodu;
					parameter[2] = new SqlParameter("@siparisSira", SqlDbType.Int);
					parameter[2].Value = item.SiparisSira;
					parameter[3] = new SqlParameter("@sevkSira", SqlDbType.Int);
					parameter[3].Value = variables.Counter;

					variables.Query = "update tblsevktra set sira = @sevkSira where belgeno = @sevkEmriNo " +
									  "and sipno = @siparisKodu and sipkont = @siparisSira";

					variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameter);

					if (!variables.Result)
						return null;
                    variables.Counter++;
				}

				ReOrderedSevkSira = GetReorderedSevkSira(toBereOrderedSevkSira);

				return ReOrderedSevkSira;
			}
			catch
			{
				return null;
			}
		}

		private ObservableCollection<Cls_Sevk> GetReorderedSevkSira(ObservableCollection<Cls_Sevk> reorderedSevkSira)
		{
			try
			{
				var sevkEmriNo = reorderedSevkSira.Select(item => item.SevkEmriNo).FirstOrDefault();
				SqlParameter[] parameter = new SqlParameter[1];

				parameter[0] = new("@sevkEmriNo", SqlDbType.NVarChar, 15);
				parameter[0].Value = sevkEmriNo;

				variables.Query = "select * from vbvSevkSiraDuzenle where belgeno=@sevkEmriNo";

				reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameter);

				if (reader == null)
					return null;

				reorderedSevkSira.Clear();

				while (reader.Read())
				{
					foreach (Cls_Sevk item in reader)
					{
						item.SevkSira = Convert.ToInt32(reader[0]);
						item.SevkEmriNo = reader[1].ToString();
						item.CariAdi = reader[2].ToString();
						item.SiparisKodu = reader[3].ToString();
						item.SiparisSira = Convert.ToInt32(reader[4]);
						item.UrunAdi = reader[5].ToString();
						item.SevkMiktar = Convert.ToInt32(reader[6]);
						item.UrunKodu = reader[7].ToString();
						AgirlikHacimCollection = GetProductVolumeAndWeight(item.UrunKodu);
						item.UrunHacim = AgirlikHacimCollection.Select(item => item.UrunHacim).FirstOrDefault();
						item.UrunAgirlik = AgirlikHacimCollection.Select(item => item.UrunAgirlik).FirstOrDefault();

						reorderedSevkSira.Add(item);
					}
				}
				reader.Close();
				return reorderedSevkSira;

			}
			catch
			{
				return null;
			}
		}

        private int CountSevkSira(string sevkEmriNo)
        {
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new("@sevkEmriNo", SqlDbType.NVarChar, 15);
                parameter[0].Value = sevkEmriNo;

                variables.Query = "select count(*) from tblsevktra where belgeno=@sevkEmriNo";

                reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query,variables.Yil, parameter);

                while(reader.Read())
                {
                    variables.Counter = Convert.ToInt32(reader[0]);
                }

                reader.Close();

                if (variables.Counter == 0)
                    return -1;

                return variables.Counter;
            }
            catch 
            {
                return -1;
            }
        }
		public bool DeleteYuklenmemisSevkEmriMas(string sevkEmriNo)
        {
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                parameter[0].Value = sevkEmriNo;

                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpDeleteYuklenmemisSevkEmriMas", variables.Yil, parameter);
                if (!variables.Result) return false;

                return variables.Result;

            }
            catch
            {
                return false;
            }
        }

        public bool DeleteYuklenmemisSevkSatir(string sevkEmriNo, string siparisNo, int siparisSira)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@sevkEmriNo", SqlDbType.NVarChar, 15);
                parameters[0].Value = sevkEmriNo;
                parameters[1] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
                parameters[1].Value = siparisNo;
                parameters[2] = new SqlParameter("@siparisSira", SqlDbType.Int);
                parameters[2].Value = siparisSira;

                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpDeleteYuklenmemisSevkEmriSatir", variables.Yil, parameters);

                if (!variables.Result) return false;
                return variables.Result;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteYuklemeKaydiGeriAlSSH(ObservableCollection<Cls_Sevk> shipmentCollection, string result)
        {
            try 
            {
				SevkEmriNo = shipmentCollection.Select(x => x.SevkEmriNo).FirstOrDefault();

				if (result == "Sevk Bilgileri Alınırken")
                    return true;
                  
                if(result == "Sevk Genel Bilgileri Kaydedilirken")
                { 
                    variables.Query = "delete from tblsevkmas where tip=1 and belgeno=@belgeno";
                    SqlParameter[] parameter = new SqlParameter[1];
                    parameter[0] = new SqlParameter("@belgeno", SqlDbType.NVarChar, 15);
                    parameter[0].Value = SevkEmriNo;
                    variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameter);

                    return variables.Result;
				} 
                if(result == "Sevk Satır Bilgileri Kaydedilirken")
                { 
                    variables.Query = "delete from tblsevkmas where tip=1 and belgeno=@belgeno delete from tblsevktra where tip=1 and belgeno=@belgeno";
                    SqlParameter[] parameter = new SqlParameter[1];
                    parameter[0] = new SqlParameter("@belgeno", SqlDbType.NVarChar, 15);
                    parameter[0].Value = SevkEmriNo;
                    variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameter);

                    return variables.Result;
				}
                if(result == "Yukleme Genel Bilgileri Kaydedilirken")
                { 
                    variables.Query = "delete from tblsevkmas where tip=1 and belgeno=@belgeno delete from tblsevktra where tip=1 and belgeno=@belgeno " +
                                      " delete from tblsevkmas where tip=3 and acik10=@belgeno delete from tblsevktra where tip=3 and sipno=@belgeno";
                    SevkEmriNo = shipmentCollection.Select(x => x.SevkEmriNo).FirstOrDefault();
                    SqlParameter[] parameter = new SqlParameter[1];
                    parameter[0] = new SqlParameter("@belgeno", SqlDbType.NVarChar, 15);
                    parameter[0].Value = SevkEmriNo;
                    variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameter);

                    return variables.Result;
				}
                if(result == "Yukleme Satır Bilgileri Kaydedilirken" || 
                    result == "Sevk Genel Bilgileri Kaydedilirken")
                {
					variables.Query = "delete from tblsevkmas where tip=1 and belgeno=@belgeno delete from tblsevktra where tip=1 and belgeno=@belgeno " +
									 " delete from tblsevkmas where tip=3 and acik10=@belgeno delete from tblsevktra where tip=3 and sipno=@belgeno";
					SqlParameter[] parameter = new SqlParameter[1];
					parameter[0] = new SqlParameter("@belgeno", SqlDbType.NVarChar, 15);
					parameter[0].Value = SevkEmriNo;
					variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameter);
                    if (!variables.Result)
                        return false;

					foreach (Cls_Sevk item in shipmentCollection)
					{
						SiparisKodu = item.SiparisKodu;
						SiparisSira = item.SiparisSira;
						UrunKodu = item.UrunKodu;
                        //sevk irsaliye no al
						variables.Query = "Select fisno from tblsthar where stok_kodu=@stokKodu and sthar_sipnum=@fisno and stra_sipkont=@stra_sipkont";

						SqlParameter[] parameterSthar = new SqlParameter[3];
						parameterSthar[0] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
						parameterSthar[0].Value = SiparisKodu;
						parameterSthar[1] = new SqlParameter("@sira", SqlDbType.Int);
						parameterSthar[1].Value = SiparisSira;
						parameterSthar[2] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 15);
						parameterSthar[2].Value = UrunKodu;
						SevkIrsaliyeNo = data.Get_One_String_Result_Command_With_Parameters(variables.Query, variables.Yil, parameterSthar);

                        if (string.IsNullOrEmpty(SevkIrsaliyeNo))
                            return true;

                        //sthar sil
                        variables.Query = " delete from tblsthar where stok_kodu=@stokKodu and sthar_sipnum=@siparisNo and stra_sipkont=@stra_sipkont " +
                                          " fisno =@fisno"  ;

						SqlParameter[] parameterForDeleteSthar = new SqlParameter[3];
						parameterForDeleteSthar[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
						parameterForDeleteSthar[0].Value = UrunKodu;
						parameterForDeleteSthar[1] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
						parameterForDeleteSthar[1].Value = SiparisKodu;
						parameterForDeleteSthar[2] = new SqlParameter("@sira", SqlDbType.Int);
						parameterForDeleteSthar[2].Value = SiparisSira;
						parameterForDeleteSthar[3] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
						parameterForDeleteSthar[3].Value = SevkIrsaliyeNo;
						variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameterForDeleteSthar);

                        if (!variables.Result)
                            return false;

                        //kalem silmek için inckey al
						int inckey = 0;
                        string inckeyno = string.Empty;
						variables.Query = "Select inckeyno from tblsthar where stok_kodu=@stokKodu and sthar_sipnum=@sipNo and stra_sipkont=@stra_sipkont" +
										  " fisno =@fisno";
						SqlParameter[] parameterInckey = new SqlParameter[3];
						parameterInckey[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 15);
						parameterInckey[0].Value = UrunKodu;
						parameterInckey[1] = new SqlParameter("@sipNo", SqlDbType.NVarChar, 15);
						parameterInckey[1].Value = SiparisKodu;
						parameterInckey[2]= new SqlParameter("@sira", SqlDbType.Int);
						parameterInckey[2].Value = SiparisSira;
						parameterInckey[3] = new SqlParameter("@fisno", SqlDbType.NVarChar, 15);
						parameterInckey[3].Value = SevkIrsaliyeNo;
						inckeyno = data.Get_One_String_Result_Command_With_Parameters(variables.Query, variables.Yil, parameterInckey);

                        if (inckeyno == string.Empty)
                            return true;

                        inckey = Convert.ToInt32(inckeyno);

						//kalem sil
						variables.Query = " delete from TBLKALEMDETAY where refinckeyno=@inckey";

						SqlParameter[] parameterForDeleteKalem = new SqlParameter[1];
						parameterForDeleteKalem[0] = new SqlParameter("@inckey", SqlDbType.Int);
						parameterForDeleteKalem[0].Value = inckey;
						variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameterForDeleteKalem);

						if (!variables.Result)
							return false;
                        //yukleme isemri no al
                        variables.Query = "select isemrino from tblisemri where siparis_no=@sipno and sipkont=@sipkont and stok_kodu=@stokKodu and isemrino like 'YUK%'";
						SqlParameter[] parameterIsemriNo = new SqlParameter[3];
						parameterIsemriNo[0] = new SqlParameter("@sipno", SqlDbType.NVarChar, 15);
						parameterIsemriNo[0].Value = SiparisKodu;
						parameterIsemriNo[1] = new SqlParameter("@sipkont", SqlDbType.Int);
						parameterIsemriNo[1].Value = SiparisSira;
						parameterIsemriNo[2] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 15);
						parameterIsemriNo[2].Value = UrunKodu;
						YuklemeEmriNo = data.Get_One_String_Result_Command_With_Parameters(variables.Query, variables.Yil, parameterInckey);
                        if (string.IsNullOrEmpty(YuklemeEmriNo))
                            return true;

						//yukleme satir sil
						variables.Query = "delete from sbpturetimsonu where isEmriNo=@isemrino and stokKodu=@stokKodu " +
                                        "delete from tblsthar where sthar_sipnum=@isemrino" +
                                        "delete from tblstokurs where uretson_sipno=@isemrino " +
                                        "delete from tblisemri where isemrino=@isemrino " +
                                        "delete from tblfatuek where fatirs_no = @sevkIrsaliyeNo";

						SqlParameter[] parameterForDeleteYukSatir= new SqlParameter[3];
						parameterForDeleteYukSatir[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
						parameterForDeleteYukSatir[0].Value = UrunKodu;
						parameterForDeleteYukSatir[1] = new SqlParameter("@isemrino", SqlDbType.NVarChar, 15);
						parameterForDeleteYukSatir[1].Value = YuklemeEmriNo;
						parameterForDeleteYukSatir[3] = new SqlParameter("@sevkIrsaliyeNo", SqlDbType.NVarChar, 15);
						parameterForDeleteYukSatir[3].Value = SevkIrsaliyeNo;
						variables.Result = data.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameterForDeleteYukSatir);

						if (!variables.Result)
							return false;

					}
					return variables.Result;
				}

				return variables.Result;
			}

            catch { return false; }
        }

		private int CheckIfSevkSatirExists(string sevkEmriNo, string siparisNo, int siparisSatir)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];

                parameters[0] = new("@sevkEmriNo", SqlDbType.NVarChar, 15);
                parameters[0].Value = sevkEmriNo;
                parameters[1] = new("@siparisNo", SqlDbType.NVarChar, 15);
                parameters[1].Value = siparisNo;
                parameters[2] = new("@siparisSatir", SqlDbType.Int);
                parameters[2].Value = siparisSatir;

                variables.Query = "Select count(*) from tblsevktra where belgeno=@sevkEmriNo and sipno=@siparisNo and sipkont=@siparisSatir";
                reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameters);

                while (reader.Read()) 
                {
                    variables.Counter = Convert.ToInt32(reader[0]);
                }
                reader.Close();

                if (variables.Counter == 1 ) 
                   return 1;

                else if (variables.Counter == 0 ) 
                   return 2;

                else 
                    return 3;
            }
            catch 
            {
                return 3;   
            }
        }

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(string getStr)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(getStr));
		}
	}
}