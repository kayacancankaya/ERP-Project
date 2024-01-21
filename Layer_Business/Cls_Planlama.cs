using Layer_2_Common.Type;
using Layer_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection.PortableExecutable;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;

namespace Layer_Business
{
	public class Cls_Planlama : Cls_Base, INotifyPropertyChanged
	{

        public string PlanAdi { get; set; }

        public int PlanAdiSira { get; set; }

        public string Renk { get; set; }

        private string _stokKodu = "";

		public string StokKodu
		{
			get { return _stokKodu; }
			set
			{
				_stokKodu = value;
				OnPropertyChanged(nameof(StokKodu));
			}
		}
		private string _stokAdi = "";

		public string StokAdi
		{
			get { return _stokAdi; }
			set
			{
				_stokAdi = value;
				OnPropertyChanged(nameof(StokAdi));
			}
        }

        private string _hamKodu = "";

        public string HamKodu
        {
            get { return _hamKodu; }
            set
            {
                _hamKodu = value;
                OnPropertyChanged(nameof(HamKodu));
            }
        }
        private string _hamAdi = "";

        public string HamAdi
        {
            get { return _hamAdi; }
            set
            {
                _hamAdi = value;
                OnPropertyChanged(nameof(HamAdi));
            }
        }
        private string _urunKodu = "";

		public string UrunKodu
		{
			get { return _urunKodu; }
			set
			{
				_urunKodu = value;
				OnPropertyChanged(nameof(UrunKodu));
			}
		}
		private string _urunAdi = "";

		public string UrunAdi
		{
			get { return _urunAdi; }
			set
			{
				_urunAdi = value;
				OnPropertyChanged(nameof(UrunAdi));
			}
		}

		private string _siparisNumarasi = "";

		public string SiparisNumarasi
		{
			get { return _siparisNumarasi; }
			set
			{
				_siparisNumarasi = value;
				OnPropertyChanged(nameof(SiparisNumarasi));
			}
		}
		private int _siparisSira = 0;

		public int SiparisSira
		{
			get { return _siparisSira; }
			set
			{
				_siparisSira = value;
				OnPropertyChanged(nameof(SiparisSira));
			}
		}

		private int _siparisMiktar;
		public int SiparisMiktar
		{
			get { return _siparisMiktar; }
			set
			{
				_siparisMiktar = value;
				OnPropertyChanged(nameof(SiparisMiktar));
			}
		}
		private decimal _dovizFiyat = 0;

		public decimal DovizFiyat
		{
			get { return _dovizFiyat; }
			set
			{
				_dovizFiyat = value;
				OnPropertyChanged(nameof(DovizFiyat));
			}
		}
		private decimal _siparisFiyat = 0;
		public decimal SiparisFiyat
		{
			get { return _siparisFiyat; }
			set
			{
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
			set
			{
				_siparisDurum = value;
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
		private DateTime _teslimTarih = DateTime.Now.AddMonths(1); // işemrinin teslim tarihi
		public DateTime TeslimTarih
        {
			get { return _teslimTarih; }
			set { _teslimTarih = value; }
		}
		private string _destinasyon = "";
		public string Destinasyon
		{
			get { return _destinasyon; }
			set
			{
				_destinasyon = value;
				OnPropertyChanged(nameof(Destinasyon));
			}
		}
		private string _siparisAciklama = "";
		public string SiparisAciklama
		{
			get { return _siparisAciklama; }
			set
			{
				_siparisAciklama = value;
				OnPropertyChanged(nameof(SiparisAciklama));
			}
		}
		private string _POnumarasi = "";
		public string POnumarasi
		{
			get { return _POnumarasi; }
			set
			{
				_POnumarasi = value;
				OnPropertyChanged(nameof(POnumarasi));
			}
		}
		private string _dovizTipi = "USD";
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

		private decimal _stokKDV = 10;
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
		

		private string _isemriAciklama = "Lütfen Açıklama Giriniz...";

		public string IsemriAciklama
		{
			get { return _isemriAciklama;  }
			set { 
				_isemriAciklama = value; 
				OnPropertyChanged(nameof(IsemriAciklama));
			}
		}

		public int IsemriMiktar { get; set; }
        public int KalanIsemriMiktar { get; set; }
        public int GonderilecekIsemriMiktar { get; set; }
        public int TeslimMiktar { get; set; }
        public int AcikSevkMiktar { get; set; }
        public decimal DepoMiktar { get; set; }
        public decimal TedarikTalepMiktar { get; set; }
        public decimal TedarikSiparisMiktar { get; set; }
		public decimal HamIhtiyacMiktar { get; set; }
		public int SevkMiktar { get; set; }
        public int MinimumPaketUretimAdedi { get; set; }
        public int KalanUretimAdedi { get; set; }
        public string CariKodu { get; set; }
        public string CariAdi { get; set; }
		private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set {
				_isChecked = value; 
				OnPropertyChanged(nameof(IsChecked));
				}
		}
        public string UrunGrup { get; set; }
        public string Model { get; set; }
        public string SatisSekil { get; set; }
        public decimal MakSiparisMiktar { get; set; }

        public Cls_Base.DovizTipi DovizTipi { get; set; }
        public Cls_Base.UretimDurum UretimDurum { get; set; }

        private ObservableCollection<Cls_Planlama> temp_coll_planlama = new();
        private ObservableCollection<Cls_Planlama> temp_coll_renklendir = new();

        public ObservableCollection<Cls_Planlama> PlanlamaCollection = new();
        public ObservableCollection<Cls_Planlama> RenklendirCollection = new();
        DataLayer data = new();

		public Cls_Planlama()
		{
			variables.Fabrika = login.GetFabrika();

        }

        Variables variables = new();
        LoginLogic login = new();

        public ObservableCollection<Cls_Planlama> PopulatePlakaSimulasyonList(Dictionary<string, string> kisitPairs)
        {
            try
            {

                variables.Query = "select * from vbvPlakaSimulasyon where 1=1 ";
                variables.Counter = 0;


                if (!string.IsNullOrEmpty(kisitPairs["urunGrup"]))
                {
                    variables.Query = variables.Query + "and urunGrup like '%' + @urunGrup + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["satisSekil"]))
                {
                    variables.Query = variables.Query + "and satisSekil like '%' + @satisSekil + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["model"]))
                {
                    variables.Query = variables.Query + "and model like '%' + @model + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["stokKodu"]))
                {
                    variables.Query = variables.Query + "and urunKodu like '%' + @stokKodu + '%' ";
                    variables.Counter++;
                }


                if (!string.IsNullOrEmpty(kisitPairs["stokAdi"]))
                {
                    variables.Query = variables.Query + "and urunAdi like '%' + @stokAdi + '%' ";
                    variables.Counter++;
                }

                if (kisitPairs["eksikStokGosterme"] == "true")
                {
                    variables.Query = variables.Query + "and uretilebilecekMiktar > 0 ";
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];
                variables.Counter = 0;

                if (!string.IsNullOrEmpty(kisitPairs["urunGrup"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@urunGrup", SqlDbType.NVarChar, 100);
                    parameters[variables.Counter].Value = kisitPairs["urunGrup"];
                    variables.Counter++;
                }

                if (!string.IsNullOrEmpty(kisitPairs["model"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@model", SqlDbType.NVarChar, 100);
                    parameters[variables.Counter].Value = kisitPairs["model"];
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["satisSekil"]))
                {
                    parameters[variables.Counter] = new SqlParameter("@satisSekil", SqlDbType.NVarChar, 100);
                    parameters[variables.Counter].Value = kisitPairs["satisSekil"];
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
				decimal maksSiparisMiktar = 0;
                temp_coll_planlama.Clear();
                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
							maksSiparisMiktar = Convert.ToDecimal(reader[5]);
                            Cls_Planlama planItem = new Cls_Planlama
                            {
                                UrunGrup = reader[0].ToString(),
                                Model = reader[1].ToString(),
                                SatisSekil = reader[2].ToString(),
                                UrunKodu = reader[3].ToString(),
                                UrunAdi = reader[4].ToString(),
								
                            };


                            if (maksSiparisMiktar >= int.MinValue && maksSiparisMiktar <= int.MaxValue)
                                planItem.MakSiparisMiktar = Math.Floor(maksSiparisMiktar);
                            else
                                planItem.MakSiparisMiktar = maksSiparisMiktar;

                            temp_coll_planlama.Add(planItem);
                        }
                    }
                }

                PlanlamaCollection = temp_coll_planlama;
                return PlanlamaCollection;

            }
            catch (Exception ex){ var stri = ex.Message.ToString(); return null; }
        }
        public ObservableCollection<Cls_Planlama> PopulateTopluIsemriAcList(Dictionary<string, string> kisitPairs)
		{
			try
			{

				variables.Query = "select * from vbvAcilacakIsemirleri where 1=1 ";
				variables.Counter = 0;

				if (!string.IsNullOrEmpty(kisitPairs["siparisNo"]))
				{
					variables.Query = variables.Query + "and siparisNo like '%' + @siparisNo + '%' ";
					variables.Counter++;
				}
				if (!string.IsNullOrEmpty(kisitPairs["stokKodu"]))
				{
					variables.Query = variables.Query + "and urunKodu like '%' + @stokKodu + '%' ";
					variables.Counter++;
				}


				if (!string.IsNullOrEmpty(kisitPairs["stokAdi"]))
				{
					variables.Query = variables.Query + "and urunAdi like '%' + @stokAdi + '%' ";
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

				temp_coll_planlama.Clear();
				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,variables.Fabrika))
				{
					if (reader != null && reader.HasRows)
					{
						while (reader.Read())
						{
							Cls_Planlama planItem = new Cls_Planlama
							{
								SiparisNumarasi = reader[0].ToString(),
								SiparisSira = Convert.ToInt32(reader[1]),
                                CariKodu = reader[2].ToString(),
                                CariAdi = reader[3].ToString(),
                                UrunKodu = reader[4].ToString(),
								UrunAdi = reader[5].ToString(),
								SiparisMiktar = Convert.ToInt32(reader[6]),
								KalanIsemriMiktar = Convert.ToInt32(reader[7]),
								GonderilecekIsemriMiktar = Convert.ToInt32(reader[7]),
								IsChecked = false,
							};
							temp_coll_planlama.Add(planItem);
						}
					}
				}

				PlanlamaCollection = temp_coll_planlama;
				return PlanlamaCollection;

			}
			catch { return null; }
		}
        public ObservableCollection<Cls_Planlama> PopulateTamamlanmamisSiparislerList(Dictionary<string, string> kisitPairs)
		{
			try
			{

				variables.Query = "select * from vbvTamamlanmamisSiparisler where 1=1 ";
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


				if (kisitPairs["kapaliSiparis"] == "Gosterme")
				{
					variables.Query = variables.Query + " and siparisDurum <> 'K'";
				}
				if (kisitPairs["acilmamisIsemri"] == "Gosterme")
				{
					variables.Query = variables.Query + " and referansIsemri is not null";
				}

				temp_coll_planlama.Clear();
				using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameters,variables.Fabrika))
				{
					if (reader != null && reader.HasRows)
					{
						while (reader.Read())
						{
							Cls_Planlama planItem = new Cls_Planlama
							{
								SiparisNumarasi = reader[0].ToString(),
								SiparisSira = Convert.ToInt32(reader[1]),
								UrunKodu = reader[2].ToString(),
								UrunAdi = reader[3].ToString(),
								SiparisTarih = reader[4].ToString(),
								TerminTarih = Convert.ToDateTime(reader[5]),
								SiparisMiktar = Convert.ToInt32(reader[6]),
								TeslimMiktar = Convert.ToInt32(reader[7]),
								AcikSevkMiktar = Convert.ToInt32(reader[8]),
								DepoMiktar = Convert.ToInt32(reader[9]),
								MinimumPaketUretimAdedi = Convert.ToInt32(reader[10]),
								CariAdi = reader[11].ToString(),
								CariKodu = reader[12].ToString(),
							};
							temp_coll_planlama.Add(planItem);
						}
					}
				}

				PlanlamaCollection = temp_coll_planlama;
				return PlanlamaCollection;

			}
			catch { return null; }
		}
		public ObservableCollection<Cls_Planlama> GetKayitliPlanAdlari(string simulasyonTip)
		{ 	
			try 
			{
				ObservableCollection<Cls_Planlama> kayitliPlanAdlariCollection = new();

				if(simulasyonTip == "Simülasyon")
					variables.Query = "Select distinct plan_adi from vbtSimulasyonModuler";
                if (simulasyonTip == "Simülasyon Sunta")
                    variables.Query = "Select distinct plan_adi from vbtSimulasyonSunta";
				if (simulasyonTip == "Ahsap Plan")
					variables.Query = "Select distinct planAdi from vatSimulasyon";

                temp_coll_planlama.Clear();
				using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil,variables.Fabrika))
				{
					if (reader == null)
						return null;

					while (reader.Read()) 
					{ 
						Cls_Planlama plan = new Cls_Planlama
						{
							PlanAdi = reader[0].ToString(), 
						};

						temp_coll_planlama.Add(plan);
					}
				}
				if (temp_coll_planlama.Any())
				{
					kayitliPlanAdlariCollection = temp_coll_planlama;
					return kayitliPlanAdlariCollection;
				}
				else
					return null;
				
			}
			catch 
			{
				return null;
			}
		}
		public int InsertPlanAdi(ObservableCollection<Cls_Planlama> planAdlari, string planAdi, bool isNew, string simulasyonTip)
		{
			try
			{

				if (isNew)
				{
					if(simulasyonTip=="Simülasyon")
						variables.Query = "Select distinct plan_adi from vbtSimulasyonModuler";
                    if (simulasyonTip == "Simülasyon Sunta")
                        variables.Query = "Select distinct plan_adi from vbtSimulasyonSunta";
                    if (simulasyonTip == "Ahsap Plan")
                        variables.Query = "Select distinct planAdi from vatSimulasyon";

                    temp_coll_planlama.Clear();
					using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil, variables.Fabrika))
					{ 
						while (reader.Read())
						{
							if (!reader.HasRows)
								return 2;

							Cls_Planlama plan = new Cls_Planlama
							{
								PlanAdi = reader[0].ToString()
							};
							temp_coll_planlama.Add(plan) ;
						}
						PlanlamaCollection = temp_coll_planlama;
					}
                    //yeni açılacak plan adı mevcut mu kontrol
                    var result = PlanlamaCollection.Any(x => x.PlanAdi == planAdi);
                    if (result)
                       return 3;
                    if(simulasyonTip == "Simülasyon")
						variables.Query = "select top 1 cast(sira_no as nvarchar(4)) from vbtSimulasyonModuler order by sira_no desc";
                    if (simulasyonTip == "Simülasyon Sunta")
                        variables.Query = "select top 1 cast(sira_no as nvarchar(4)) from vbtSimulasyonSunta order by sira_no desc";
                    if (simulasyonTip == "Ahsap Plan")
                        variables.Query = "select top 1 cast(PlanAdiSira as nvarchar(4)) from vatSimulasyon order by planAdiSira desc";
                    using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil,variables.Fabrika))
					{
						if (!reader.HasRows)
						{
							PlanAdiSira = 1;
						}

						while (reader.Read())
						{

							PlanAdiSira = Convert.ToInt32(reader[0]) + 1;
						}
						
					}
				}

				if(!isNew)
				{
                    if (simulasyonTip == "Simülasyon")
                        variables.Query = "select distinct sira_no from vbtSimulasyonModuler where plan_adi=@planAdi";
                    if (simulasyonTip == "Simülasyon Sunta")
                        variables.Query = "select distinct sira_no from vbtSimulasyonSunta where plan_adi=@planAdi";
                    if (simulasyonTip == "Ahsap Plan")
                        variables.Query = "select distinct PlanAdiSira from vatSimulasyon where planAdi=@planAdi";
                    SqlParameter[] parameter = new SqlParameter[1];

					parameter[0] = new SqlParameter("@planAdi",SqlDbType.NVarChar,500);
					parameter[0].Value = planAdi;

					using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameter,variables.Fabrika))
					{
						while (reader.Read())
						{
							if (!reader.HasRows)
								return 2;

							PlanAdiSira = Convert.ToInt32(reader[0]);
						}

					}
				}

                if (simulasyonTip == "Simülasyon")
				{ 
					foreach (Cls_Planlama plan in planAdlari)
					{
						SqlParameter[] parameters = new SqlParameter[7];

						parameters[0] = new SqlParameter("@planAdi", SqlDbType.NVarChar, 500);
						parameters[0].Value = planAdi;
						parameters[1] = new SqlParameter("@planSira", SqlDbType.Int);
						parameters[1].Value = PlanAdiSira;
						parameters[2] = new SqlParameter("@mamulKodu", SqlDbType.NVarChar, 50);
						parameters[2].Value = plan.UrunKodu;
						parameters[3] = new SqlParameter("@mamulAdi", SqlDbType.NVarChar, 500);
						parameters[3].Value = plan.UrunAdi;
						parameters[4] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
						parameters[4].Value = plan.SiparisNumarasi;
						parameters[5] = new SqlParameter("@siparisSira", SqlDbType.Int);
						parameters[5].Value = plan.SiparisSira;
						parameters[6] = new SqlParameter("@siparisMiktar", SqlDbType.Int);
						parameters[6].Value = plan.SiparisMiktar;

						variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertSimulasyon", variables.Yil, parameters);
						if (!variables.Result)
							return 4;
					}
                }

                if (simulasyonTip == "Simülasyon Sunta")
                {
                    foreach (Cls_Planlama plan in planAdlari)
                    {
                        SqlParameter[] parameters = new SqlParameter[5];

                        parameters[0] = new SqlParameter("@planAdi", SqlDbType.NVarChar, 500);
                        parameters[0].Value = planAdi;
                        parameters[1] = new SqlParameter("@planSira", SqlDbType.Int);
                        parameters[1].Value = PlanAdiSira;
                        parameters[2] = new SqlParameter("@mamulKodu", SqlDbType.NVarChar, 50);
                        parameters[2].Value = plan.UrunKodu;
                        parameters[3] = new SqlParameter("@mamulAdi", SqlDbType.NVarChar, 500);
                        parameters[3].Value = plan.UrunAdi;
                        parameters[4] = new SqlParameter("@siparisMiktar", SqlDbType.Int);
                        parameters[4].Value = plan.SiparisMiktar;

                        variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertSimulasyonSunta", variables.Yil, parameters);
                        if (!variables.Result)
                            return 4;
                    }
                }
                if (simulasyonTip == "Ahsap Plan")
                {
                    foreach (Cls_Planlama plan in planAdlari)
                    {
                        SqlParameter[] parameters = new SqlParameter[5];

                        parameters[0] = new SqlParameter("@planAdi", SqlDbType.NVarChar, 500);
                        parameters[0].Value = planAdi;
                        parameters[1] = new SqlParameter("@planSira", SqlDbType.Int);
                        parameters[1].Value = PlanAdiSira;
                        parameters[2] = new SqlParameter("@mamulKodu", SqlDbType.NVarChar, 50);
                        parameters[2].Value = plan.UrunKodu;
                        parameters[3] = new SqlParameter("@mamulAdi", SqlDbType.NVarChar, 500);
                        parameters[3].Value = plan.UrunAdi;
                        parameters[4] = new SqlParameter("@siparisMiktar", SqlDbType.Int);
                        parameters[4].Value = plan.SiparisMiktar;

                        variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertAhsapPlanSimulasyon", variables.Yil, parameters,variables.Fabrika);
                        if (!variables.Result)
                            return 4;
                    }
                }
                return 1;
			}
			catch 
			{
				return 5;
			}
		}
        public async Task<int> InsertTopluIsemri(ObservableCollection<Cls_Planlama> isemirleriCollection)
        {
            try
            {
				login.User = login.GetUserName();
                variables.Counter = 1;
                foreach (Cls_Planlama isemri in isemirleriCollection)
                {
					SqlParameter[] parameters = new SqlParameter[7];
					
					parameters[0] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
					parameters[0].Value = isemri.SiparisNumarasi;
					parameters[1] = new SqlParameter("@siparisSira", SqlDbType.Int);
					parameters[1].Value = isemri.SiparisSira;
					parameters[2] = new SqlParameter("@siparisMiktar", SqlDbType.Int);
					parameters[2].Value = isemri.SiparisMiktar;
					parameters[3] = new SqlParameter("@aciklama", SqlDbType.NVarChar, 800);
					parameters[3].Value = isemri.IsemriAciklama;
					parameters[4] = new SqlParameter("@user", SqlDbType.NVarChar, 12);
					parameters[4].Value = login.User.Substring(0,12).ToUpper();
					parameters[5] = new SqlParameter("@urunKodu", SqlDbType.NVarChar,35);
					parameters[5].Value = isemri.UrunKodu;
					parameters[6] = new SqlParameter("@teslimTarihi", SqlDbType.DateTime);
					parameters[6].Value = isemri.TeslimTarih;

					variables.Result = await data.ExecuteStoredProcedureWithParametersAsync("vbpInsertIsemri", variables.Yil, parameters,variables.Fabrika);
					if (!variables.Result)
					    return 2;

					variables.Counter++;
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }
        public bool TruncatePlanAdlari(string simulasyonTip)
		{
			try
			{
				if(simulasyonTip == "Simülasyon")
					variables.Query = "truncate table vbtSimulasyonModuler";
                if (simulasyonTip == "Simülasyon Sunta")
                    variables.Query = "truncate table vbtSimulasyonSunta";
                if (simulasyonTip == "Planlama Ahsap")
                    variables.Query = "truncate table vatSimulasyon";

                variables.Result = data.ExecuteCommand(variables.Query, variables.Yil,variables.Fabrika);

				return variables.Result;
			}
			catch 
			{
				return false;
			}
		}
        public ObservableCollection<Cls_Planlama> GetDistinctPlanAdi(string simulasyonTip)
		{
			try
			{
				if(simulasyonTip == "Simülasyon")
					variables.Query = "Select distinct sira_no, plan_adi from vbtSimulasyonModuler";
                if (simulasyonTip == "Simülasyon Sunta")
                    variables.Query = "Select distinct sira_no, plan_adi from vbtSimulasyonSunta";
                if (simulasyonTip == "Ahsap Plan")
                    variables.Query = "Select distinct planAdiSira, planAdi from vatSimulasyon";
                temp_coll_planlama.Clear();
				using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query,variables.Yil,variables.Fabrika))
				{
					if (!reader.HasRows)
						return null;

					while(reader.Read()) 
					{ 
						Cls_Planlama item = new Cls_Planlama()
						{
							PlanAdiSira = Convert.ToInt32(reader[0]),
							PlanAdi = reader[1].ToString(),
						};
						temp_coll_planlama.Add(item);
                    }
                }

				PlanlamaCollection = temp_coll_planlama;
				return PlanlamaCollection;
			}
			catch 
			{
				return null;
			}
        }
        public ObservableCollection<Cls_Planlama> GetDistinctPlanAdiForSimulation(string simulasyonTip)
        {
            try
            {

				if (simulasyonTip == "Simülasyon")
				{
					variables.Query = "Select distinct sira_no, plan_adi from vbtSimulasyonModuler";

					temp_coll_planlama.Clear();
					using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil))
					{
						if (!reader.HasRows)
							return null;

						while (reader.Read())
						{
							Cls_Planlama item = new Cls_Planlama()
							{
								PlanAdiSira = Convert.ToInt32(reader[0]),
								PlanAdi = reader[1].ToString(),
							};
							temp_coll_planlama.Add(item);
						}
					}

					PlanlamaCollection = temp_coll_planlama;

					foreach (var item in PlanlamaCollection)
					{
						variables.Query = $"IF OBJECT_ID('simulasyon{item.PlanAdi}TalepEksikleri') IS NOT NULL select 1";
						using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil))
						{
							if (reader.HasRows)
							{
								item.Renk = "Brown";
                                continue;
							}
						}
						variables.Query = $"IF OBJECT_ID('simulasyon{item.PlanAdi}SiparisEksikleri') IS NOT NULL select 1";
						using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil))
						{
							if (reader.HasRows)
							{
								item.Renk = "Orange";
                                PlanlamaCollection.Add(item);
                                continue;
							}
						}
						variables.Query = $"IF OBJECT_ID('simulasyon{item.PlanAdi}DepoEksikleri') IS NOT NULL select 1";
						using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil))
						{
							if (reader.HasRows)
							{
								item.Renk = "Yellow";
                                PlanlamaCollection.Add(item);
                                continue;
							}
						}
						item.Renk = "Green";
					}
					return PlanlamaCollection;
				}

				else if (simulasyonTip == "Simülasyon Sunta" ||
						simulasyonTip == "Ahsap Plan")
				{
					if(simulasyonTip == "Simulasyon Sunta")
						variables.Query = "select * from vbvSimulasyonSuntaDurum";
					if(simulasyonTip == "Ahsap Plan")
						variables.Query = "select * from vbvSimulasyonAhsapPlan";

					temp_coll_renklendir.Clear();
					decimal depoMiktar = 0, siparisMiktar = 0, talepMiktar = 0, kumulatifDurum = 0;
					using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil,variables.Fabrika))
					{
						if (!reader.HasRows)
							return null;

						while (reader.Read())
						{
							Cls_Planlama generalItem = new();

                            depoMiktar = Convert.ToDecimal(reader[2]);
                            siparisMiktar = Convert.ToDecimal(reader[3]);
                            talepMiktar = Convert.ToDecimal(reader[4]);


                            //plan adlarının başladığı sütundan itibaren döngüye girerek renklendir
                            for (int i = 6; i < reader.FieldCount; i++)
							{
                                Cls_Planlama item = new();
                                item.HamKodu = reader[0].ToString();
                                item.HamAdi = reader[1].ToString();
                                item.DepoMiktar = Convert.ToDecimal(reader[2]);
                                item.TedarikSiparisMiktar = Convert.ToDecimal(reader[3]);
                                item.TedarikTalepMiktar = Convert.ToDecimal(reader[4]);

                                item.PlanAdi = reader.GetName(i);

                                if (reader[i] == DBNull.Value || reader[i] == null)
									item.HamIhtiyacMiktar = 0; // Set a default value when the database value is DBNull or null
                                
                                else
									item.HamIhtiyacMiktar = Convert.ToDecimal(reader[i]);
                                

                                if (depoMiktar > 0)
								{
									depoMiktar = depoMiktar - item.HamIhtiyacMiktar;
									if (depoMiktar < 0)
									{
										siparisMiktar = siparisMiktar - Math.Abs(depoMiktar);
									}
								}
								if (siparisMiktar > 0 && depoMiktar < 0)
									siparisMiktar = siparisMiktar - item.HamIhtiyacMiktar;
								if (siparisMiktar < 0)
								{
									talepMiktar = talepMiktar - Math.Abs(siparisMiktar);
								}
								if (talepMiktar > 0 && depoMiktar < 0 && siparisMiktar < 0)
									talepMiktar = talepMiktar - item.HamIhtiyacMiktar;

								if (depoMiktar > 0)
								{
									item.Renk = "Green";
									temp_coll_renklendir.Add(item);
									continue;
								}
								if (siparisMiktar > 0)
								{
									item.Renk = "Yellow";
									temp_coll_renklendir.Add(item);
									continue;
								}
								if (talepMiktar > 0)
								{
									item.Renk = "Orange";
									temp_coll_renklendir.Add(item);
									continue;
								}
								if (item.HamIhtiyacMiktar > 0)
									item.Renk = "Brown";
								else
									item.Renk = "Green";

								temp_coll_renklendir.Add(item);
							}
						}
					}

					RenklendirCollection = temp_coll_renklendir;

					return RenklendirCollection;

				}
				else return null;
				
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<Cls_Planlama> GetPlanAdiDetayWithOnlyPlanAdi(Cls_Planlama planDetay, string simulasyonTip)
        {
            try
            {
                if (simulasyonTip == "Simülasyon")
                    variables.Query = "Select * from vbtSimulasyonModuler where plan_adi = @planAdi ";
                if (simulasyonTip == "Simülasyon Sunta")
                    variables.Query = "Select * from vbtSimulasyonSunta where plan_adi = @planAdi ";
                if (simulasyonTip == "Ahsap Plan")
                    variables.Query = "Select * from vatSimulasyon where planAdi = @planAdi ";

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@planAdi", SqlDbType.NVarChar, 800);
                parameters[0].Value = planDetay.PlanAdi;


                temp_coll_planlama.Clear();
                if (variables.Fabrika == "Vita")
                {
                    using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters, variables.Fabrika))
                    {
                        if (!reader.HasRows)
                            return null;

                        while (reader.Read())
                        {
                            Cls_Planlama item = new Cls_Planlama()
                            {
                                PlanAdiSira = Convert.ToInt32(reader["SIRA_NO"]),
                                PlanAdi = reader["PLAN_ADI"].ToString(),
                                SiparisNumarasi = reader["SIP_NO"].ToString(),
                                SiparisSira = Convert.ToInt32(reader["SIP_SIRA"]),
                                UrunKodu = reader["MAMUL_KODU"].ToString(),
                                UrunAdi = reader["MAMUL_ADI"].ToString(),
                                SiparisMiktar = Convert.ToInt32(reader["SIPMIKTAR"]),
                            };
                            temp_coll_planlama.Add(item);
                        }
                    }
                }
                if (variables.Fabrika == "Ahşap")
                {
                    using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters, variables.Fabrika))
                    {
                        if (!reader.HasRows)
                            return null;

                        while (reader.Read())
                        {
                            Cls_Planlama item = new Cls_Planlama()
                            {
                                PlanAdiSira = Convert.ToInt32(reader["PlanAdiSira"]),
                                PlanAdi = reader["PlanAdi"].ToString(),
                                SiparisNumarasi = reader["SiparisNumarasi"].ToString(),
                                SiparisSira = Convert.ToInt32(reader["SiparisSira"]),
                                UrunKodu = reader["UrunKodu"].ToString(),
                                UrunAdi = reader["UrunAdi"].ToString(),
                                SiparisMiktar = Convert.ToInt32(reader["SiparisMiktar"]),
                            };
                            temp_coll_planlama.Add(item);
                        }
                    }
                }
                PlanlamaCollection = temp_coll_planlama;
                return PlanlamaCollection;
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<Cls_Planlama> GetPlanAdiDetay(Cls_Planlama planDetay, string simulasyonTip)
        {
            try
            {
				if(simulasyonTip == "Simülasyon")
					variables.Query = "Select * from vbtSimulasyonModuler where plan_adi = @planAdi and sira_no = @planSira";
				if(simulasyonTip == "Simülasyon Sunta")
					variables.Query = "Select * from vbtSimulasyonSunta where plan_adi = @planAdi and sira_no = @planSira";
				if(simulasyonTip == "Ahsap Plan")
					variables.Query = "Select * from vatSimulasyon where planAdi = @planAdi and planAdiSira = @planSira";

                SqlParameter[] parameters = new SqlParameter[2];
				parameters[0] = new SqlParameter("@planAdi", SqlDbType.NVarChar, 800);
				parameters[0].Value = planDetay.PlanAdi;
                parameters[1] = new SqlParameter("@planSira", SqlDbType.Int);
				parameters[1].Value = planDetay.PlanAdiSira;


                temp_coll_planlama.Clear();
				if(variables.Fabrika == "Vita")
				{ 
					using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameters,variables.Fabrika))
					{
						if (!reader.HasRows)
							return null;

						while(reader.Read()) 
						{ 
							Cls_Planlama item = new Cls_Planlama()
							{
								PlanAdiSira = Convert.ToInt32(reader["SIRA_NO"]),
								PlanAdi = reader["PLAN_ADI"].ToString(),
								SiparisNumarasi = reader["SIP_NO"].ToString(),
								SiparisSira = Convert.ToInt32(reader["SIP_SIRA"]),
								UrunKodu = reader["MAMUL_KODU"].ToString(),
								UrunAdi = reader["MAMUL_ADI"].ToString(),
								SiparisMiktar = Convert.ToInt32(reader["SIPMIKTAR"]),
							};
							temp_coll_planlama.Add(item);
						}
					}
                }
				if(variables.Fabrika == "Ahşap")
				{ 
					using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil,parameters,variables.Fabrika))
					{
						if (!reader.HasRows)
							return null;

						while(reader.Read()) 
						{ 
							Cls_Planlama item = new Cls_Planlama()
							{
								PlanAdiSira = Convert.ToInt32(reader["PlanAdiSira"]),
								PlanAdi = reader["PlanAdi"].ToString(),
								SiparisNumarasi = reader["SiparisNumarasi"].ToString(),
								SiparisSira = Convert.ToInt32(reader["SiparisSira"]),
								UrunKodu = reader["UrunKodu"].ToString(),
								UrunAdi = reader["UrunAdi"].ToString(),
								SiparisMiktar = Convert.ToInt32(reader["SiparisMiktar"]),
							};
							temp_coll_planlama.Add(item);
						}
					}
                }
                PlanlamaCollection = temp_coll_planlama;
                return PlanlamaCollection;
            }
            catch
            {
                return null;
            }
        }
		public async Task<int> Simulasyon(string simulasyonTipi)
			{
				try
				{
					if(simulasyonTipi=="Simülasyon")
						variables.Result = await data.ExecuteStoredProcedureAsync("vbpSimulasyonIhtiyacHesaplaModuler", variables.Yil);
					
					if(simulasyonTipi=="Simülasyon Sunta")
						variables.Result = await data.ExecuteStoredProcedureAsync("vbpSimulasyonSunta", variables.Yil);
					
					if(simulasyonTipi=="Ahsap Plan")
						variables.Result = await data.ExecuteStoredProcedureAsync("vbpSimulasyonAhsapPlan", variables.Yil,variables.Fabrika);

						if (!variables.Result)
							return 2;

					if(simulasyonTipi=="Simülasyon")
					{ 

						variables.Query = "select distinct PLAN_ADI from vbtSimulasyonModuler";

						temp_coll_planlama.Clear();
						using(SqlDataReader reader = await data.Select_Command_Data_ReaderAsync(variables.Query,variables.Yil)) 
						{
							if(!reader.HasRows)
								return 3;

							while(reader.Read()) 
							{
								Cls_Planlama plan = new Cls_Planlama
								{
									PlanAdi = reader[0].ToString(),
								};
								temp_coll_planlama.Add(plan);
							}
						}

						foreach (Cls_Planlama item in temp_coll_planlama)
						{
							SqlParameter[] parameter = new SqlParameter[1];
							parameter[0] = new SqlParameter("@planName", SqlDbType.NVarChar, 255);
							parameter[0].Value = item.PlanAdi;
							variables.Result = await data.ExecuteStoredProcedureWithParametersAsync("vbpCreateSimulationTables", variables.Yil, parameter);

							if (!variables.Result)
								return 4;
						}
					}
                return 1;
				}
				catch (Exception)
				{
					return -1;
				}
			}
		public ObservableCollection<Cls_Planlama> GetPlanRenkDurum(ObservableCollection<Cls_Planlama> renkDurumBelirle)
		{
			try
			{
				var distinctPlanAdi = renkDurumBelirle.Select(p => p.PlanAdi).Distinct().ToList();

				ObservableCollection<Cls_Planlama> planRenk = new();

				foreach(String item in distinctPlanAdi)
				{
					Cls_Planlama planRenkDurum = new();
					planRenkDurum.PlanAdi = item;

					var distinctColor = renkDurumBelirle.Where(p => p.PlanAdi == item).Distinct().ToList();

					if (distinctColor.Where(r => r.Renk == "Brown").Any())
					{ planRenkDurum.Renk = "Brown"; planRenk.Add(planRenkDurum); continue; }
                    if (distinctColor.Where(r => r.Renk == "Orange").Any())
                    { planRenkDurum.Renk = "Orange"; planRenk.Add(planRenkDurum); continue; }
                    if (distinctColor.Where(r => r.Renk == "Yellow").Any())
                    { planRenkDurum.Renk = "Yellow"; planRenk.Add(planRenkDurum); continue; }

					 planRenkDurum.Renk = "Green"; planRenk.Add(planRenkDurum); 
                };

                return planRenk;
			}
			catch (Exception)
			{
				return null;
			}
		}
        public DataTable GetDataForExcelFromSimulatedTable(string simulasyonTip)
        {
			if(simulasyonTip == "Simulasyon Sunta")
				variables.Query = "select * from vbvSimulasyonSuntaDurum";
			if(simulasyonTip == "Ahsap Plan")
				variables.Query = "select * from vbvSimulasyonAhsapPlan";

            var dataTable = new DataTable();

            dataTable.Columns.Add("Ham Kodu");
            dataTable.Columns.Add("Ham Adı");
            dataTable.Columns.Add("Depo Adet");
            dataTable.Columns.Add("Sipariş Miktar");
            dataTable.Columns.Add("Talep Miktar");

            using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil,variables.Fabrika))
			{
				if (reader != null && reader.HasRows)
				{
					string columnName = string.Empty;
					while (reader.Read())
					{
                        var dataRow = dataTable.NewRow();

                        dataRow["Ham Kodu"] = reader[0].ToString();
                        dataRow["Ham Adı"] = reader[1].ToString();
                        dataRow["Depo Adet"] = Convert.ToDecimal(reader[2]);
                        dataRow["Sipariş Miktar"] = Convert.ToDecimal(reader[3]);
                        dataRow["Talep Miktar"] = Convert.ToDecimal(reader[4]);

						for (int i = 6; i < reader.FieldCount; i++)
						{
							columnName = reader.GetName(i);

							if (dataTable.Rows.Count == 0)
								dataTable.Columns.Add(columnName);

                            if (reader[i] == DBNull.Value || reader[i] == null)
								dataRow[columnName] = 0;

							else
								dataRow[columnName] = Convert.ToDecimal(reader[i]);
						}

                        dataTable.Rows.Add(dataRow);
                    }
                
				}
			}
            return dataTable;
        }
		public bool DeleteIsemri(ObservableCollection<Cls_Planlama> isemriCollection, string fabrika)
		{
			try
			{
				foreach(Cls_Planlama isemri in isemriCollection) 
				{ 
				
					SqlParameter[] parameters = new SqlParameter[3];
					parameters[0] = new SqlParameter("@siparisNo", SqlDbType.NVarChar, 15);
					parameters[1] = new SqlParameter("@siparisSira", SqlDbType.Int);
					parameters[2] = new SqlParameter("@urunKodu", SqlDbType.NVarChar,35);
					parameters[0].Value = isemri.SiparisNumarasi;
					parameters[1].Value = isemri.SiparisSira;
					parameters[2].Value = isemri.UrunKodu;
					
					variables.Result = data.ExecuteStoredProcedureWithParameters("vbpDeleteIsemri",variables.Yil,parameters,fabrika);

					if (!variables.Result)
						return false;
						
				}
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
