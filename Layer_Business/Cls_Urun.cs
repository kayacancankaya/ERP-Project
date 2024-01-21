using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using Layer_Data;
using Layer_2_Common.Type;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace Layer_Business
{
    public class Cls_Urun : INotifyPropertyChanged
    {
        private string _urunTipi;

        public string UrunTipi
        {
            get { return _urunTipi; }
            set
            {
                _urunTipi = value;
                OnPropertyChanged(nameof(UrunTipi));
            }
        }

        private string _model;

        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }
        private string _satisSekil;

        public string SatisSekil
        {
            get { return _satisSekil; }
            set
            {
                _satisSekil = value;
                OnPropertyChanged(nameof(SatisSekil));
            }
        }

        private string _urunKodu;

        public string UrunKodu
        {
            get { return _urunKodu; }
            set { _urunKodu = value;
                OnPropertyChanged(nameof(UrunKodu));
            }
        }

        private string _sablonKod;

        public string SablonKod
        {
            get { return _sablonKod; }
            set {
                _sablonKod = value;
                OnPropertyChanged(nameof(SablonKod));
            }
        }

        private string _urunAdi;

        public string UrunAdi
        {
            get { return _urunAdi; }
            set {
                _urunAdi = value;
                OnPropertyChanged(nameof(UrunAdi));
            }
        }
        private int _urunMiktar;

        public int UrunMiktar
        {
            get { return _urunMiktar; }
            set {
                _urunMiktar = value;
                OnPropertyChanged(nameof(UrunMiktar));
            }
        }

        private string _varyantVarMi;

        public string VaryantVarMi
        {
            get { return _varyantVarMi; }
            set {
                _varyantVarMi = value;
                OnPropertyChanged(nameof(VaryantVarMi));
            }
        }

        public string OzellikTipi { get; set; }

        private string _urunGrubuKodu;

        public string UrunGrubuKodu
        {
            get { return _urunGrubuKodu; }
            set { _urunGrubuKodu = value;
                OnPropertyChanged(nameof(_urunGrubuKodu));
            }
        }

        public int UrunGrubuSira { get; set; }

        private string _modelKodu;

        public string ModelKodu
        {
            get { return _modelKodu; }
            set {
                _modelKodu = value;
                OnPropertyChanged(nameof(ModelKodu));
            }
        }

        public string ModelIsim { get; set; }
        public string UrunGrubuIsim { get; set; }
        public int UrunGrubuTur { get; set; }
        public int TakimKodu { get; set; }
        public int UniteKod { get; set; }
        public string Kilit { get; set; }
        public string ModelKilit { get; set; }
        public string SatisSekilKilit { get; set; }

        public string Sayfa { get; set; }
        public int Kdv { get; set; }
        public int Muhdetay { get; set; }
        public string MenuGrup { get; set; }              
        public int TeslimGunu { get; set; }
        public string Kod1 { get; set; }
        public string Kod2 { get; set; }
        public string Kod3 { get; set; }
        public string Kod4 { get; set; }
        public string Kod5 { get; set; }
        public string ModelKod1 { get; set; }
        public string ModelKod2 { get; set; }
        public string ModelKod3 { get; set; }
        public string ModelKod4 { get; set; }
        public string ModelKod5 { get; set; }
        public string SatisSekilKod1 { get; set; }
        public string SatisSekilKod2 { get; set; }
        public string SatisSekilKod3 { get; set; }
        public string SatisSekilKod4 { get; set; }
        public string SatisSekilKod5 { get; set; }

        public int ModelSira { get; set; }

        private string _satisSekilKodu;

        public string SatisSekilKodu
        {
            get { return _satisSekilKodu; }
            set {
                _satisSekilKodu = value;
                OnPropertyChanged(nameof(SatisSekilKodu));
            }
        }

        public string SatisSekilIsim { get; set; }
        public int SatisSekilSira { get; set; }

        private string _varyantIsmi;

        public string VaryantIsmi
        {
            get { return _varyantIsmi; }
            set {
                _varyantIsmi = value;
                OnPropertyChanged(nameof(VaryantIsmi));
            }
        }

        private string _varyantKodu;

        public string VaryantKodu
        {
            get { return _varyantKodu; }
            set {
                _varyantKodu = value;
                OnPropertyChanged(nameof(VaryantKodu));
            }
        }
        private string _varyantNo;

        public string VaryantNo
        {
            get { return _varyantNo; }
            set {
                _varyantNo = value;
                OnPropertyChanged(nameof(VaryantNo));
            }
        }

        private string _varyantTipi;

        public string VaryantTipi
        {
            get { return _varyantTipi; }
            set {
                _varyantTipi = value;
                OnPropertyChanged(nameof(VaryantTipi));
            }
        }
        private string _varyantSira;

        public string VaryantSira
        {
            get { return _varyantSira; }
            set {
                _varyantSira = value;
                OnPropertyChanged(nameof(VaryantSira));
            }
        }

        private string _ozKisit;

        public string OzKisit
        {
            get { return _ozKisit; }
            set {
                _ozKisit = value;
                OnPropertyChanged(nameof(OzKisit));
            }
        }

        private string _ozKisitIsim;

        public string OzKisitIsim
        {
            get { return _ozKisitIsim; }
            set {
                _ozKisitIsim = value;
                OnPropertyChanged(nameof(OzKisitIsim));
            }
        }

        private string _ingilizceIsimAnahtar;

        public string IngilizceIsimAnahtar
        {
            get { return _ingilizceIsimAnahtar; }
            set {
                _ingilizceIsimAnahtar = value;
                OnPropertyChanged(nameof(IngilizceIsimAnahtar));
            }
        }
        private string _ingilizceIsim;

        public string IngilizceIsim
        {
            get { return _ingilizceIsim; }
            set {
                _ingilizceIsim = value;
                OnPropertyChanged(nameof(IngilizceIsim));
            }
        }

        private string _isimAnahtar;

        public string IsimAnahtar
        {
            get { return _isimAnahtar; }
            set {
                _isimAnahtar = value;
                OnPropertyChanged(nameof(IsimAnahtar));
            }
        }

        private string _query;

        public string Query
        {
            get { return _query; }
            set { _query = value; }
        }

        private ObservableCollection<Cls_Urun> _urunCollection;
        public ObservableCollection<Cls_Urun> UrunCollection
        {
            get { return _urunCollection; }
            set { _urunCollection = value; }
        }
        ObservableCollection<Cls_Urun> coll_urun = new ObservableCollection<Cls_Urun>();
        DataLayer dataLayer = new DataLayer();
        DataTable dataTable = new DataTable();
        SqlDataReader reader;
        DataRow firstRow;
        Object columnValueByIndex;
        Variables variables = new();


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Cls_Urun> PopulateUrunListele(Dictionary<string, string> constraints)
        {
            try
            {
                variables.Query = "SELECT * FROM vbvSiparisGirisArama where 1=1 ";

                variables.Counter = 0;


                if (!string.IsNullOrWhiteSpace(constraints["sablonKod"]))
                {
                    if (constraints["sablonKod"].Length > 11) 
                    { 
                        constraints["sablonKod"] = constraints["sablonKod"].Substring(0, 11);
					}
					variables.Query = variables.Query + "and stok_kodu like '%' + @sablonKod + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["urunAdi"]))
                {
                    variables.Query = variables.Query + "and stok_adi like '%' + @urunAdi + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["urunTipi"]))
                {
                    variables.Query = variables.Query + "and urun_isim like '%' + @urunTipi + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["model"]))
                {
                    variables.Query = variables.Query + "and model_isim like '%' + @model + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrWhiteSpace(constraints["satisSekil"]))
                {
                    variables.Query = variables.Query + "and ssekil_isim like '%' + @satisSekil + '%' ";
                    variables.Counter++;
                }

                SqlParameter[] parameters = new SqlParameter[variables.Counter];

                variables.Counter = 0;

                if (!string.IsNullOrWhiteSpace(constraints["sablonKod"]))
                {
                    parameters[variables.Counter] = new("@sablonKod", SqlDbType.NVarChar, 11);
                    parameters[variables.Counter].Value = constraints["sablonKod"];

					variables.Counter++;
				}

				if (!string.IsNullOrWhiteSpace(constraints["urunAdi"]))
				{
					parameters[variables.Counter] = new("@urunAdi", SqlDbType.NVarChar, 500);
					parameters[variables.Counter].Value = constraints["urunAdi"];

					variables.Counter++;
				}

				if (!string.IsNullOrWhiteSpace(constraints["urunTipi"]))
				{
					parameters[variables.Counter] = new("@urunTipi", SqlDbType.NVarChar, 500);
					parameters[variables.Counter].Value = constraints["urunTipi"];

					variables.Counter++;
				}

				if (!string.IsNullOrWhiteSpace(constraints["model"]))
				{
					parameters[variables.Counter] = new("@model", SqlDbType.NVarChar, 500);
					parameters[variables.Counter].Value = constraints["model"];

					variables.Counter++;
				}

				if (!string.IsNullOrWhiteSpace(constraints["satisSekil"]))
				{
					parameters[variables.Counter] = new("@satisSekil", SqlDbType.NVarChar, 500);
					parameters[variables.Counter].Value = constraints["satisSekil"];

					variables.Counter++;
				}

				reader = dataLayer.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters);

                if(!reader.HasRows) { return null; }                

                coll_urun.Clear();

                while (reader.Read())
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Urun cls_urun = new Cls_Urun
                    {
                        UrunKodu = reader["stok_kodu"].ToString(),
                        UrunAdi = reader["stok_adi"].ToString(),
                        UrunTipi = reader["urun_isim"].ToString(),
                        Model = reader["model_isim"].ToString(),
                        SatisSekil = reader["ssekil_isim"].ToString(),
                        UrunMiktar = 0,
                        VaryantVarMi = reader["VaryantVarMi"].ToString(),
                        UrunGrubuKodu = reader["urun_kodu"].ToString(),
                        ModelKodu = reader["model_kodu"].ToString(),
                        SatisSekilKodu = reader["ssekil_kodu"].ToString()
                    };
                    coll_urun.Add(cls_urun);
                }

                UrunCollection = coll_urun;
                reader.Close();
                return UrunCollection;
            }
            catch { return null; }
            
        }

        public ObservableCollection<Cls_Urun> PopulatePopUpVaryantSecimWindow()
        {
            try
            {
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                coll_urun.Clear();

                foreach (DataRow row in dataTable.Rows)
                {

                    Cls_Urun cls_urun = new Cls_Urun
                    {
                        VaryantIsmi = row["ozisim"].ToString(),
                        VaryantKodu = row["ozkod"].ToString()
                    };
                    coll_urun.Add(cls_urun);
                }

                UrunCollection = coll_urun;
                OnPropertyChanged(nameof(VaryantIsmi));

                return UrunCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<Cls_Urun> PopulateOzellikListe(string ozellikKisit)
        {
            try
            {
                if (ozellikKisit == "Ürün Grup")
                { 
                    variables.Query = "Select * from stburungrup";

                    using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.Yil))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            coll_urun.Clear();
                            while (reader.Read())
                            {
                                Cls_Urun urun = new Cls_Urun
                                {
                                    UrunGrubuKodu = reader["kod"].ToString(),
                                    UrunGrubuIsim = reader["isim"].ToString(),
                                    UrunGrubuTur = Convert.ToInt32(reader["tur"]),
                                    TakimKodu = Convert.ToInt32(reader["takimkod"]),
                                    UniteKod = Convert.ToInt32(reader["unitekod"]),
                                    UrunGrubuSira = Convert.ToInt32(reader["sira"]),
                                    Kilit = reader["kilit"].ToString(),
                                    Sayfa = reader["sayfa"].ToString(),
                                    Kdv = Convert.ToInt32(reader["kdvo"]),
                                    Muhdetay = Convert.ToInt32(reader["muhdetay"]),
                                    MenuGrup = reader["menugrup"].ToString(),
                                    TeslimGunu = Convert.ToInt32(reader["teslimgunu"]),
                                    Kod1 = reader["kod1"].ToString(),
                                    Kod2 = reader["kod2"].ToString(),
                                    Kod3 = reader["kod3"].ToString(),
                                    Kod4 = reader["kod4"].ToString(),
                                    Kod5 = reader["kod5"].ToString(),
                                };

                                coll_urun.Add(urun);
                            }
                        }
                    }
                    UrunCollection = coll_urun;
                }
                if (ozellikKisit == "Model")
                {
                    variables.Query = "Select * from stbmodel";

                    using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.Yil))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            coll_urun.Clear();
                            while (reader.Read())
                            {
                                Cls_Urun urun = new Cls_Urun
                                {
                                    ModelKodu = reader["kod"].ToString(),
                                    ModelIsim = reader["isim"].ToString(),
                                    ModelSira = Convert.ToInt32(reader["sira"]),
                                    ModelKilit = reader["kilit"].ToString(),
                                    ModelKod1 = reader["kod1"].ToString(),
                                    ModelKod2 = reader["kod2"].ToString(),
                                    ModelKod3 = reader["kod3"].ToString(),
                                    ModelKod4 = reader["kod4"].ToString(),
                                    ModelKod5 = reader["kod5"].ToString(),
                                };
                                coll_urun.Add(urun);
                            }
                        }
                    }
                    UrunCollection = coll_urun;
                }
                if (ozellikKisit == "Satış Şekil")
                {
                    variables.Query = "Select * from stbsatissekil";

                    using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.Yil))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            coll_urun.Clear();
                            while (reader.Read())
                            {
                                Cls_Urun urun = new Cls_Urun
                                {
                                    SatisSekilKodu = reader["kod"].ToString(),
                                    SatisSekilIsim = reader["isim"].ToString(),
                                    SatisSekilSira = Convert.ToInt32(reader["sira"]),
                                    SatisSekilKilit = reader["kilit"].ToString(),
                                    SatisSekilKod1 = reader["kod1"].ToString(),
                                    SatisSekilKod2 = reader["kod2"].ToString(),
                                    SatisSekilKod3 = reader["kod3"].ToString(),
                                    SatisSekilKod4 = reader["kod4"].ToString(),
                                    SatisSekilKod5 = reader["kod5"].ToString(),
                                };
                                coll_urun.Add(urun);
                            }
                        }
                    }
                    UrunCollection = coll_urun;
                }

                return UrunCollection;
            }
            catch 
            {
                return null;
            }
        }

        public string GetKod(string ozellikTipi)
        {
            try
            {
                if(ozellikTipi == "Ürün Grup")
                    variables.Query = "select top 1 kod from stburungrup order by kod desc";
                if(ozellikTipi == "Model")
                    variables.Query = "select top 1 kod from stbmodel order by kod desc";
                if (ozellikTipi == "Satış Şekil")
                    return string.Empty;

                UrunGrubuKodu = dataLayer.Get_One_String_Result_Command(variables.Query, variables.Yil);

                UrunGrubuKodu = (Convert.ToInt32(UrunGrubuKodu) + 1).ToString(); 

                return UrunGrubuKodu;
            }
            catch 
            {
                return "STRING HATA";
            }
        }

        public bool InsertOzellik(Cls_Urun urunOzellik, string ozellikTipi) 
        {
            try
            {
                if (ozellikTipi == "Ürün Grup")
                {
                    variables.Query = "insert into stburungrup (kod,isim,tur,takimkod,unitekod,sira,kilit,sayfa,kdvo,muhdetay,menugrup,teslimgunu,kod1,kod2,kod3,kod4,kod5) values " +
                                  "(@kod,@isim,@tur,@takimkod,@unitekod,@sira,@kilit,@sayfa,@kdvo,@muhdetay,@menugrup,@teslimgunu,@kod1,@kod2,@kod3,@kod4,@kod5)";

                    SqlParameter[] parameters = new SqlParameter[17];

                    parameters[0] = new SqlParameter("@kod", SqlDbType.NVarChar, 3);
                    parameters[1] = new SqlParameter("@isim", SqlDbType.NVarChar, 50);
                    parameters[2] = new SqlParameter("@tur", SqlDbType.Int);
                    parameters[3] = new SqlParameter("@takimkod", SqlDbType.NVarChar, 3);
                    parameters[4] = new SqlParameter("@unitekod", SqlDbType.NVarChar, 2);
                    parameters[5] = new SqlParameter("@sira", SqlDbType.Int);
                    parameters[6] = new SqlParameter("@kilit", SqlDbType.NVarChar, 1);
                    parameters[7] = new SqlParameter("@sayfa", SqlDbType.NVarChar, 50);
                    parameters[8] = new SqlParameter("@kdvo", SqlDbType.Float);
                    parameters[9] = new SqlParameter("@muhdetay", SqlDbType.Int);
                    parameters[10] = new SqlParameter("@menugrup", SqlDbType.NVarChar, 50);
                    parameters[11] = new SqlParameter("@teslimgunu", SqlDbType.Int);
                    parameters[12] = new SqlParameter("@kod1", SqlDbType.NVarChar, 50);
                    parameters[13] = new SqlParameter("@kod2", SqlDbType.NVarChar, 50);
                    parameters[14] = new SqlParameter("@kod3", SqlDbType.NVarChar, 50);
                    parameters[15] = new SqlParameter("@kod4", SqlDbType.NVarChar, 50);
                    parameters[16] = new SqlParameter("@kod5", SqlDbType.NVarChar, 50);

                    parameters[0].Value = urunOzellik.UrunGrubuKodu;
                    parameters[1].Value = urunOzellik.UrunGrubuIsim;
                    parameters[2].Value = urunOzellik.UrunGrubuTur;
                    parameters[3].Value = urunOzellik.TakimKodu;
                    parameters[4].Value = urunOzellik.UniteKod;
                    parameters[5].Value = urunOzellik.UrunGrubuSira;
                    parameters[6].Value = urunOzellik.Kilit;
                    parameters[7].Value = urunOzellik.Sayfa;
                    parameters[8].Value = urunOzellik.Kdv;
                    parameters[9].Value = urunOzellik.Muhdetay;
                    parameters[10].Value = urunOzellik.MenuGrup;
                    parameters[11].Value = urunOzellik.TeslimGunu;
                    parameters[12].Value = urunOzellik.Kod1;
                    parameters[13].Value = urunOzellik.Kod2;
                    parameters[14].Value = urunOzellik.Kod3;
                    parameters[15].Value = urunOzellik.Kod4;
                    parameters[16].Value = urunOzellik.Kod5;

                    variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);  
                }
                if (ozellikTipi == "Model")
                {
                    variables.Query = "insert into stbmodel (kod,isim,sira,kilit,kod1,kod2,kod3,kod4,kod5) values " +
                                  "(@kod,@isim,@sira,@kilit,@kod1,@kod2,@kod3,@kod4,@kod5)";

                    SqlParameter[] parameters = new SqlParameter[9];

                    parameters[0] = new SqlParameter("@kod", SqlDbType.NVarChar, 3);
                    parameters[1] = new SqlParameter("@isim", SqlDbType.NVarChar, 50);
                    parameters[2] = new SqlParameter("@sira", SqlDbType.Int);
                    parameters[3] = new SqlParameter("@kilit", SqlDbType.NVarChar, 1);
                    parameters[4] = new SqlParameter("@kod1", SqlDbType.NVarChar, 50);
                    parameters[5] = new SqlParameter("@kod2", SqlDbType.NVarChar, 50);
                    parameters[6] = new SqlParameter("@kod3", SqlDbType.NVarChar, 50);
                    parameters[7] = new SqlParameter("@kod4", SqlDbType.NVarChar, 50);
                    parameters[8] = new SqlParameter("@kod5", SqlDbType.NVarChar, 50);

                    parameters[0].Value = urunOzellik.ModelKodu;
                    parameters[1].Value = urunOzellik.ModelIsim;
                    parameters[2].Value = urunOzellik.ModelSira;
                    parameters[3].Value = urunOzellik.ModelKilit;
                    parameters[4].Value = urunOzellik.ModelKod1;
                    parameters[5].Value = urunOzellik.ModelKod2;
                    parameters[6].Value = urunOzellik.ModelKod3;
                    parameters[7].Value = urunOzellik.ModelKod4;
                    parameters[8].Value = urunOzellik.ModelKod5;

                    variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);  
                }
                if (ozellikTipi == "Satış Şekil")
                {
                    variables.Query = "insert into stbsatissekil (kod,isim,sira,kilit,kod1,kod2,kod3,kod4,kod5) values " +
                                  "(@kod,@isim,@sira,@kilit,@kod1,@kod2,@kod3,@kod4,@kod5)";


                    SqlParameter[] parameters = new SqlParameter[9];

                    parameters[0] = new SqlParameter("@kod", SqlDbType.NVarChar, 3);
                    parameters[1] = new SqlParameter("@isim", SqlDbType.NVarChar, 50);
                    parameters[2] = new SqlParameter("@sira", SqlDbType.Int);
                    parameters[3] = new SqlParameter("@kilit", SqlDbType.NVarChar, 1);
                    parameters[4] = new SqlParameter("@kod1", SqlDbType.NVarChar, 50);
                    parameters[5] = new SqlParameter("@kod2", SqlDbType.NVarChar, 50);
                    parameters[6] = new SqlParameter("@kod3", SqlDbType.NVarChar, 50);
                    parameters[7] = new SqlParameter("@kod4", SqlDbType.NVarChar, 50);
                    parameters[8] = new SqlParameter("@kod5", SqlDbType.NVarChar, 50);

                    parameters[0].Value = urunOzellik.SatisSekilKodu;
                    parameters[1].Value = urunOzellik.SatisSekilIsim;
                    parameters[2].Value = urunOzellik.SatisSekilSira;
                    parameters[3].Value = urunOzellik.SatisSekilKilit;
                    parameters[4].Value = urunOzellik.SatisSekilKod1;
                    parameters[5].Value = urunOzellik.SatisSekilKod2;
                    parameters[6].Value = urunOzellik.SatisSekilKod3;
                    parameters[7].Value = urunOzellik.SatisSekilKod4;
                    parameters[8].Value = urunOzellik.SatisSekilKod5;

                    variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);  
                }

                return variables.Result;
            }
            catch
            {
                return false;
            }
        }
        public ObservableCollection<Cls_Urun> VaryantOzKisitVarIseListele()
        {
            try
            {
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                coll_urun.Clear();

                foreach (DataRow row in dataTable.Rows)
                {

                    Cls_Urun cls_urun = new Cls_Urun
                    {
                        VaryantTipi = row["isim"].ToString()
                    };
                    coll_urun.Add(cls_urun);
                }

                UrunCollection = coll_urun;

                return UrunCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public int GetMaxSira()
        {
            try
            {
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                firstRow = dataTable.Rows[0];
                columnValueByIndex = firstRow[0];
                int maxSira = Convert.ToInt32(columnValueByIndex);

                return maxSira;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
        public ObservableCollection<Cls_Urun> GetNameKeys()
        {
            try
            {
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                coll_urun.Clear();

                foreach (DataRow row in dataTable.Rows)
                {

                    Cls_Urun cls_urun = new Cls_Urun
                    {
                        IsimAnahtar = row["isimanahtar"].ToString(),
                        IngilizceIsimAnahtar = row["isimanahtaring"].ToString(),
                        SablonKod = row["kod5"].ToString(),
                    };
                    coll_urun.Add(cls_urun);
                }

                UrunCollection = coll_urun;

                return UrunCollection;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<Cls_Urun> VaryantListele()
        {
            try
            {
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                coll_urun.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    // Create an instance of the ViewModel and populate it from the DataRow
                    Cls_Urun cls_urun = new Cls_Urun
                    {
                        VaryantTipi = row["isim"].ToString()
                    };
                    coll_urun.Add(cls_urun);
                }

                UrunCollection = coll_urun;
                OnPropertyChanged(nameof(OzKisit));

                return UrunCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public bool UpdateStokAdiIfVaryantExists(string mamulKodu, string mamulAdi, string mamulAdiIngilizce)
        {
            try
            {
                Query = $"Select stok_kodu from tblstsabit where stok_kodu='{mamulKodu}'";
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                
                    
                    if (dataTable.Rows.Count == 0)
                {

                    return false;
                }

                else
                {

                    Query = $"Select stok_kodu from tblstsabitek where stok_kodu='{mamulKodu}'";
                    dataTable = dataLayer.Select_Command(Query, variables.Yil);
                    

                    if (dataTable.Rows.Count == 0)
                    {

                        Query = $"Update tblstsabit set stok_adi='{mamulAdi}' where stok_kodu='{mamulKodu}' ";
                        dataLayer.Update_Statement(Query, variables.Yil);
                        
                        variables.ErrorMessage = "Ürünün Stok Kartı Bulunmakta Ancak Ek Bilgileri \n Mevcut Olmadığından İngilizce İsim Kaydedilemedi.";
                        MessageBox.Show(variables.ErrorMessage);

                        return true;
                    }
                    else
                    {
                        Query = $"Update tblstsabit set stok_adi='{mamulAdi}' where stok_kodu='{mamulKodu}' ";
                        Query = Query + $"Update tblstsabitek set ingisim='{mamulAdiIngilizce}' where stok_kodu='{mamulKodu}' ";

                        dataLayer.Update_Statement(Query, variables.Yil);
                        return true;
                    }
                    
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        public bool UpdateUrunOzellik(Cls_Urun dataItem, string secilenOzellikTipi)
        {
            try
            {
                if (secilenOzellikTipi == "Ürün Grup")
                { 
                    variables.Query = "update stburungrup set isim=@isim,tur=@tur,takimkod=@takimkod,unitekod=@unitekod,sira=@sira,kilit=@kilit,sayfa=@sayfa,kdvo=@kdvo " +
                                       ",muhdetay=@muhdetay,menugrup=@menugrup,teslimgunu=@teslimgunu,kod1=@kod1,kod2=@kod2,kod3=@kod3,kod4=@kod4,kod5=@kod5 " +
                                       "where kod=@kod";

                    SqlParameter[] parameters = new SqlParameter[17];

                    parameters[0] = new SqlParameter("@kod", SqlDbType.NVarChar, 2);
                    parameters[1] = new SqlParameter("@isim", SqlDbType.NVarChar, 50);
                    parameters[2] = new SqlParameter("@tur", SqlDbType.Int);
                    parameters[3] = new SqlParameter("@takimkod", SqlDbType.NVarChar, 3);
                    parameters[4] = new SqlParameter("@unitekod", SqlDbType.NVarChar, 2);
                    parameters[5] = new SqlParameter("@sira",SqlDbType.Int);
                    parameters[6] = new SqlParameter("@kilit", SqlDbType.NVarChar, 1);
                    parameters[7] = new SqlParameter("@sayfa", SqlDbType.NVarChar, 50);
                    parameters[8] = new SqlParameter("@kdvo", SqlDbType.Float);
                    parameters[9] = new SqlParameter("@muhdetay", SqlDbType.Int);
                    parameters[10] = new SqlParameter("@menugrup", SqlDbType.NVarChar, 50);
                    parameters[11] = new SqlParameter("@teslimgunu", SqlDbType.Int);
                    parameters[12] = new SqlParameter("@kod1", SqlDbType.NVarChar, 50);
                    parameters[13] = new SqlParameter("@kod2", SqlDbType.NVarChar, 50);
                    parameters[14] = new SqlParameter("@kod3", SqlDbType.NVarChar, 50);
                    parameters[15] = new SqlParameter("@kod4", SqlDbType.NVarChar, 50);
                    parameters[16] = new SqlParameter("@kod5", SqlDbType.NVarChar, 50);

                    parameters[0].Value = dataItem.UrunGrubuKodu;
                    parameters[1].Value = dataItem.UrunGrubuIsim;
                    parameters[2].Value = dataItem.UrunGrubuTur;
                    parameters[3].Value = dataItem.TakimKodu;
                    parameters[4].Value = dataItem.UniteKod;
                    parameters[5].Value = dataItem.UrunGrubuSira;
                    parameters[6].Value = dataItem.Kilit;
                    parameters[7].Value = dataItem.Sayfa;
                    parameters[8].Value = dataItem.Kdv;
                    parameters[9].Value = dataItem.Muhdetay;
                    parameters[10].Value = dataItem.MenuGrup;
                    parameters[11].Value = dataItem.TeslimGunu;
                    parameters[12].Value = dataItem.Kod1;
                    parameters[13].Value = dataItem.Kod2;
                    parameters[14].Value = dataItem.Kod3;
                    parameters[15].Value = dataItem.Kod4;
                    parameters[16].Value = dataItem.Kod5;

                     variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);
                }

                if (secilenOzellikTipi == "Model")
                {
                    variables.Query = "update stbmodel set isim=@isim,sira=@sira,kilit=@kilit,kod1=@kod1,kod2=@kod2,kod3=@kod3,kod4=@kod4,kod5=@kod5 " +
                                        "where kod=@kod";

                    SqlParameter[] parameters = new SqlParameter[9];

                    parameters[0] = new SqlParameter("@kod", SqlDbType.NVarChar, 2);
                    parameters[1] = new SqlParameter("@isim", SqlDbType.NVarChar, 50);
                    parameters[2] = new SqlParameter("@sira", SqlDbType.Int);
                    parameters[3] = new SqlParameter("@kilit", SqlDbType.NVarChar, 1);
                    parameters[4] = new SqlParameter("@kod1", SqlDbType.NVarChar, 50);
                    parameters[5] = new SqlParameter("@kod2", SqlDbType.NVarChar, 50);
                    parameters[6] = new SqlParameter("@kod3", SqlDbType.NVarChar, 50);
                    parameters[7] = new SqlParameter("@kod4", SqlDbType.NVarChar, 50);
                    parameters[8] = new SqlParameter("@kod5", SqlDbType.NVarChar, 50);

                    parameters[0].Value = dataItem.ModelKodu;
                    parameters[1].Value = dataItem.ModelIsim;
                    parameters[2].Value = dataItem.ModelSira;
                    parameters[3].Value = dataItem.ModelKilit;
                    parameters[4].Value = dataItem.ModelKod1;
                    parameters[5].Value = dataItem.ModelKod2;
                    parameters[6].Value = dataItem.ModelKod3;
                    parameters[7].Value = dataItem.ModelKod4;
                    parameters[8].Value = dataItem.ModelKod5;

                    variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);
                }

                if (secilenOzellikTipi == "Satış Şekil")
                {
                    variables.Query = "update stbsatissekil set isim=@isim,sira=@sira,kilit=@kilit,kod1=@kod1,kod2=@kod2,kod3=@kod3,kod4=@kod4,kod5=@kod5 " +
                                        "where kod=@kod";


                    SqlParameter[] parameters = new SqlParameter[9];

                    parameters[0] = new SqlParameter("@kod", SqlDbType.NVarChar, 2);
                    parameters[1] = new SqlParameter("@isim", SqlDbType.NVarChar, 50);
                    parameters[2] = new SqlParameter("@sira", SqlDbType.Int);
                    parameters[3] = new SqlParameter("@kilit", SqlDbType.NVarChar, 1);
                    parameters[4] = new SqlParameter("@kod1", SqlDbType.NVarChar, 50);
                    parameters[5] = new SqlParameter("@kod2", SqlDbType.NVarChar, 50);
                    parameters[6] = new SqlParameter("@kod3", SqlDbType.NVarChar, 50);
                    parameters[7] = new SqlParameter("@kod4", SqlDbType.NVarChar, 50);
                    parameters[8] = new SqlParameter("@kod5", SqlDbType.NVarChar, 50);

                    parameters[0].Value = dataItem.SatisSekilKodu;
                    parameters[1].Value = dataItem.SatisSekilIsim;
                    parameters[2].Value = dataItem.SatisSekilSira;
                    parameters[3].Value = dataItem.SatisSekilKilit;
                    parameters[4].Value = dataItem.SatisSekilKod1;
                    parameters[5].Value = dataItem.SatisSekilKod2;
                    parameters[6].Value = dataItem.SatisSekilKod3;
                    parameters[7].Value = dataItem.SatisSekilKod4;
                    parameters[8].Value = dataItem.SatisSekilKod5;

                    variables.Result = dataLayer.ExecuteCommandWithParameters(variables.Query, variables.Yil, parameters);
                }

                    return variables.Result;
            }
            catch 
            {
                return false;
            }
        }
        public bool InsertVaryant(string sablonKod, string mamulKodu, string mamulAdi, string identifier, string mamulAdiIngilizce)
        {
            try
            {
                
                LoginLogic login = new();
                string userName = login.GetUserName();

                variables.IsTrue = dataLayer.Insert_Stored_Proc_Param_6("vbpVaryantStokKartiAc", "@sablonKod", sablonKod, "@varyantKod", mamulKodu, "@varyantIsim", mamulAdi, "@identifier", identifier, "@varyantIngIsim", mamulAdiIngilizce, "@kayitYapanKul", userName, variables.Yil,"Stok Kartı Tabloları",1);

                return variables.IsTrue;
            }

            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
        
        public string GetIngilizceAnahtarDegeri()
        {
            try
            {
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                firstRow = dataTable.Rows[0];
                columnValueByIndex = firstRow[0];
                string ingilizceAnahtarDegeri = columnValueByIndex.ToString();

                return ingilizceAnahtarDegeri;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public string GetVaryantKodu()
        {
            try
            {
                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                firstRow = dataTable.Rows[0];
                columnValueByIndex = firstRow[0];
                string ozellikKodu = columnValueByIndex.ToString();

                return ozellikKodu;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public int GetOzellikKodu()
        {
            try
            {

                dataTable = dataLayer.Select_Command(Query, variables.Yil);
                firstRow = dataTable.Rows[0];
                columnValueByIndex = firstRow[0];
                int ozellikKodu = Convert.ToInt32(columnValueByIndex);

                return ozellikKodu;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        protected void OnPropertyChanged(string propertyChanged)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
		}
	}
}
