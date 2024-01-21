using Layer_2_Common.Type;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer_Data;

namespace Layer_Business
{
    public class Cls_Uretim : INotifyPropertyChanged
    {
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string MamulKodu { get; set; }
        public string MamulAdi { get; set; }
        public string Kod1 { get; set; }
        public string Kod2 { get; set; }
        public string Kod3 { get; set; }
        public decimal IskeletSure { get; set; }
        public decimal CilaSure { get; set; }
        public decimal MontajSure { get; set; }
        public decimal PaketSure { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { 
                    _isChecked = value; 
                    OnPropertyChanged(nameof(IsChecked));
                }
        }


        private ObservableCollection<Cls_Uretim> temp_coll_uretim = new();
        public ObservableCollection<Cls_Uretim> UretimCollection = new();
        public List<string> Kod1Collection = new();
        public List<string> Kod2Collection = new();
        public List<string> Kod3Collection = new();
        Variables variables = new();
        LoginLogic login = new();
        DataLayer data = new();
        
        public Cls_Uretim()
        {
            variables.Fabrika = login.GetFabrika();

        }
        public ObservableCollection<Cls_Uretim> PopulateSureEkle(Dictionary<string, string> kisitPairs)
        {
            try
            {

                variables.Query = "select isnull(UrunKodu,'') as UrunKodu, isnull(UrunAdi,'') as UrunAdi, isnull(Kod1,'') as Kod1," +
                    " isnull(Kod2,'') as Kod2, isnull(Kod3,'') as Kod3, isnull(IskeletSure,0) as IskeletSure, isnull(CilaSure,0) as CilaSure, " +
                    " isnull(MontajSure,0) as MontajSure, isnull(PaketSure,0) as PaketSure from vatMamulSureHesap where 1=1 ";
                variables.Counter = 0;

                if (!string.IsNullOrEmpty(kisitPairs["urunKodu"]))
                {
                    variables.Query = variables.Query + "and UrunKodu like '%' + @urunKodu + '%' ";
                    variables.Counter++;
                }

                if (!string.IsNullOrEmpty(kisitPairs["urunAdi"]))
                {
                    variables.Query = variables.Query + "and UrunAdi like '%' + @urunAdi + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["kod1"]))
                {
                    variables.Query = variables.Query + "and Kod1 like '%' + @kod1 + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["kod2"]))
                {
                    variables.Query = variables.Query + "and Kod2 like '%' + @kod2 + '%' ";
                    variables.Counter++;
                }
                if (!string.IsNullOrEmpty(kisitPairs["kod3"]))
                {
                    variables.Query = variables.Query + "and Kod3 like '%' + @kod3 + '%' ";
                    variables.Counter++;
                }

                if(variables.Counter == 0)
                {
                    temp_coll_uretim.Clear();
                    using (SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query, variables.Yil, variables.Fabrika))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Cls_Uretim urun = new Cls_Uretim
                                {
                                    UrunKodu = reader["UrunKodu"].ToString(),
                                    UrunAdi = reader["UrunAdi"].ToString(),
                                    Kod1 = reader["Kod1"].ToString(),
                                    Kod2 = reader["Kod2"].ToString(),
                                    Kod3 = reader["Kod3"].ToString(),
                                    IskeletSure = Convert.ToDecimal(reader["IskeletSure"]),
                                    CilaSure = Convert.ToDecimal(reader["CilaSure"]),
                                    MontajSure = Convert.ToDecimal(reader["MontajSure"]),
                                    PaketSure = Convert.ToDecimal(reader["PaketSure"]),
                                };

                                temp_coll_uretim.Add(urun);
                            }
                        }
                    }
                }

                else
                { 

                    SqlParameter[] parameters = new SqlParameter[variables.Counter];
                    variables.Counter = 0;

                    if (!string.IsNullOrEmpty(kisitPairs["urunKodu"]))
                    {
                        parameters[variables.Counter] = new SqlParameter("@urunKodu", SqlDbType.NVarChar, 35);
                        parameters[variables.Counter].Value = kisitPairs["urunKodu"];
                        variables.Counter++;
                    }
                    if (!string.IsNullOrEmpty(kisitPairs["urunAdi"]))
                    {
                        parameters[variables.Counter] = new SqlParameter("@urunAdi", SqlDbType.NVarChar, 500);
                        parameters[variables.Counter].Value = kisitPairs["urunAdi"];
                        variables.Counter++;
                    }
                    if (!string.IsNullOrEmpty(kisitPairs["kod1"]))
                    {
                        parameters[variables.Counter] = new SqlParameter("@kod1", SqlDbType.NVarChar, 500);
                        parameters[variables.Counter].Value = kisitPairs["kod1"];
                        variables.Counter++;
                    }
                    if (!string.IsNullOrEmpty(kisitPairs["kod2"]))
                    {
                        parameters[variables.Counter] = new SqlParameter("@kod2", SqlDbType.NVarChar, 500);
                        parameters[variables.Counter].Value = kisitPairs["kod2"];
                        variables.Counter++;
                    }
                    if (!string.IsNullOrEmpty(kisitPairs["kod3"]))
                    {
                        parameters[variables.Counter] = new SqlParameter("@kod3", SqlDbType.NVarChar, 500);
                        parameters[variables.Counter].Value = kisitPairs["kod3"];
                        variables.Counter++;
                    }

                    temp_coll_uretim.Clear();
                    using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters,variables.Fabrika))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Cls_Uretim urun = new Cls_Uretim
                                {
                                    UrunKodu = reader["UrunKodu"].ToString(),
                                    UrunAdi = reader["UrunAdi"].ToString(),
                                    IskeletSure = Convert.ToDecimal(reader["IskeletSure"]),
                                    CilaSure = Convert.ToDecimal(reader["CilaSure"]),
                                    MontajSure = Convert.ToDecimal(reader["MontajSure"]),
                                    PaketSure = Convert.ToDecimal(reader["PaketSure"]),
                                };

                                temp_coll_uretim.Add(urun);
                            }
                        }
                    }
                }
                UretimCollection = temp_coll_uretim;
                return UretimCollection;

            }
            catch { return null; }
        }

        public List<string> GetKod1()
        {
            try
            {
                variables.Query = "Select distinct kod_1 from tblstsabit (nolock) where stok_kodu like 'm%'  order by kod_1 asc";
                using(SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query,variables.Yil,variables.Fabrika)) 
                {

                    if (reader == null || reader.HasRows == false)
                        return null;
                    
                    Kod1Collection.Clear();
                    while (reader.Read()) 
                    {
                        Kod1Collection.Add(reader[0].ToString());
                    }
        
                }

                return Kod1Collection;
            }
            catch 
            {
                return null;
            }
        }
        public List<string> GetKod2()
        {
            try
            {
                variables.Query = "Select distinct kod_2 from tblstsabit (nolock) where stok_kodu like 'm%'  order by kod_2 asc";
                using(SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query,variables.Yil,variables.Fabrika)) 
                {

                    if (reader == null || reader.HasRows == false)
                        return null;
                    
                    Kod2Collection.Clear();
                    while (reader.Read()) 
                    {
                        Kod2Collection.Add(reader[0].ToString());
                    }
        
                }

                return Kod2Collection;
            }
            catch 
            {
                return null;
            }
        }
        public List<string> GetKod3()
        {
            try
            {
                variables.Query = "Select distinct kod_3 from tblstsabit (nolock) where stok_kodu like 'm%' order by kod_3 asc";
                using(SqlDataReader reader = data.Select_Command_Data_Reader(variables.Query,variables.Yil,variables.Fabrika)) 
                {

                    if (reader == null || reader.HasRows == false)
                        return null;
                    
                    Kod3Collection.Clear();
                    while (reader.Read()) 
                    {
                        Kod3Collection.Add(reader[0].ToString());
                    }
        
                }

                return Kod3Collection;
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
