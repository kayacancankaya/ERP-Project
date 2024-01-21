using Layer_2_Common.Type;
using Layer_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Layer_Business
{
    public class Cls_Arge : INotifyPropertyChanged
    {
        public string StokAdi { get; set; }
        public string StokKodu { get; set; }
        public string YariMamulKodu { get; set; }
        public string YariMamulAdi { get; set; }
        public string HamKodu { get; set; }
        public string HamAdi { get; set; }
        public int En { get; set; } = 0;
        public int Boy { get; set; } = 0;
        public int Yukseklik { get; set; } = 0;

        public decimal ReceteTuketimMiktar { get; set; } = decimal.Zero;

        public decimal M2 { get; set; } = decimal.Zero;


        public ObservableCollection<Cls_Arge> ArgeCollection = new();
        private ObservableCollection<Cls_Arge> tempCollArge = new();
        Variables variables = new();
        DataLayer data = new();
        public ObservableCollection<Cls_Arge> PopulateReceteIcinKoliDonusturListControl(int en, int boy,int yukseklik)
        { 
            try 
	        {	        
		       
                variables.Query = "select sabit.stok_kodu,sabit.STOK_ADI,sabitHam.STOK_KODU as hamKodu,sabitHam.STOK_ADI as hamAdi from TBLSTSABIT (nolock) sabit " +
                                    "left join TBLSTOKURM urm (nolock) on sabit.STOK_KODU = urm.MAMUL_KODU " +
                                    "left join tblstsabit (nolock) sabitHam on sabitHam.STOK_KODU = urm.HAM_KODU " +
                                    "where sabitHam.KOD_1='AMBALAJ' AND sabitHam.KOD_2='KARTON' and sabitHam.kod_3<>'ABOX' AND sabitHam.KOD_4<>'TRIPLEX' " +
                                    "and sabitHam.STOK_ADI not like '%TABAKA%' and sabitHam.STOK_ADI not like '%KILITLI KUTU YAPISIK%' and sabitHam.STOK_ADI not like '%ONDULA%' and sabit.STOK_KODU like 'Y%' " +
                                    "and coalesce(try_cast(SUBSTRING(sabit.STOK_ADI, CHARINDEX('*', sabit.STOK_ADI) + 1, 4) as int), 0) = @en " +
                                    "and coalesce(try_cast(SUBSTRING(sabit.STOK_ADI, CHARINDEX('*', sabit.STOK_ADI) - 4, 4) as int), 0) = @boy " +
                                    "and coalesce(try_cast(SUBSTRING(sabit.STOK_ADI, CHARINDEX('*', sabit.STOK_ADI) + 6, 4) as int), 0) = @yukseklik ";

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@en", SqlDbType.Int);
                parameters[0].Value = en;
                parameters[1] = new SqlParameter("@boy", SqlDbType.Int);
                parameters[1].Value = boy;
                parameters[2] = new SqlParameter("@yukseklik", SqlDbType.Int);
                parameters[2].Value = yukseklik;

                if(tempCollArge != null )
                    tempCollArge.Clear();

                using (SqlDataReader reader = data.Select_Command_Data_Reader_With_Parameters(variables.Query, variables.Yil, parameters))
                {
                    if (reader == null)
                        return null;

                    if(!reader.HasRows && reader != null) 
                    {
                        Cls_Arge item = new Cls_Arge
                        {
                            HamKodu = "Boş Sonuç",
                        };
                        tempCollArge.Add(item);
                    }
                    while (reader.Read()) 
                    {
                        Cls_Arge item = new Cls_Arge
                        {
                            YariMamulKodu = reader["stok_kodu"].ToString(),
                            YariMamulAdi = reader["stok_adi"].ToString(),
                            HamKodu = reader["hamKodu"].ToString(),
                            HamAdi = reader["hamAdi"].ToString(),
                        };
                        tempCollArge.Add(item);
                    }
                }
                ArgeCollection = tempCollArge;
                return ArgeCollection;
            }
	        catch 
	        {
                return null;
		    }
        }

        public string GetKoliYariMamulKodu()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@prefix", SqlDbType.NVarChar, 3);
                parameters[0].Value = "YM8";
                parameters[1] = new SqlParameter("@tableName", SqlDbType.NVarChar, 128);
                parameters[1].Value = "TBLSTSABIT";
                parameters[2] = new SqlParameter("@columnName", SqlDbType.NVarChar, 128);
                parameters[2].Value = "STOK_KODU";
                YariMamulKodu = data.Get_One_String_Result_Stored_Proc_With_Parameters("vbpGetFisno", variables.Yil, parameters);

                if (YariMamulKodu.Length >= 4)
                {
                    YariMamulKodu = YariMamulKodu.Remove(3,1);
                }

                if (YariMamulKodu == string.Empty)
                    YariMamulKodu = "YM8000000001";

                

                return YariMamulKodu;
            }
            catch 
            {
                return string.Empty;
            }
        }

        public string GetHamAdiFromHamKodu(string hamKodu)
        {
            try
            {
                variables.Query = "SELECT STOK_ADI FROM TBLSTSABIT (NOLOCK) WHERE " +
                    "STOK_KODU =@hamKodu";
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@hamKodu",SqlDbType.NVarChar, 35);
                parameter[0].Value = hamKodu;

                HamAdi = data.Get_One_String_Result_Command_With_Parameters(variables.Query, variables.Yil, parameter);

                if (HamAdi == "STRING HATA")
                    return "HAMADI HATA";

                return HamAdi;

            }
            catch 
            {
                return "HAMADI HATA";
            }
        }

        public string GetStokAdi(string stokKodu,string fabrika)
        {
            try
            {
                variables.Query = "select stok_adi from tblstsabit (nolock) where stok_kodu=@stokKodu";

                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                parameter[0].Value = stokKodu;

                StokAdi = data.Get_One_String_Result_Command_With_Parameters(variables.Query, variables.Yil, parameter, fabrika);

                return StokAdi;
                
            }
            catch 
            {
                return string.Empty;
            }
        }

        public bool IfStokKoduExists(string stokKodu, string fabrika) 
        {
            try
            {
                variables.Query = "Select count(*) from tblstsabit (nolock) where stok_kodu = @stokKodu";
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                parameter[0].Value = stokKodu;
                variables.ResultInt = data.Get_One_Int_Result_Command_With_Parameters(variables.Query, variables.Yil, parameter, fabrika);

                if (variables.ResultInt != 1)
                    return false;
                
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool InsertStokKartiveReceteKoli(ObservableCollection<Cls_Arge> receteVeStokKartiCollection)
        {
            try
            {
                YariMamulKodu = receteVeStokKartiCollection.Select(y=>y.YariMamulKodu).FirstOrDefault();
                YariMamulAdi = receteVeStokKartiCollection.Select(y=>y.YariMamulAdi).FirstOrDefault();
                HamKodu = receteVeStokKartiCollection.Select(h=>h.HamKodu).FirstOrDefault();
                En = receteVeStokKartiCollection.Select(e => e.En).FirstOrDefault();
                Boy = receteVeStokKartiCollection.Select(b => b.Boy).FirstOrDefault();
                Yukseklik = receteVeStokKartiCollection.Select(g => g.Yukseklik).FirstOrDefault();
                M2 = receteVeStokKartiCollection.Select(m => m.M2).FirstOrDefault();

                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@stokKodu",SqlDbType.NVarChar,35);
                parameters[0].Value = YariMamulKodu;
                parameters[1] = new SqlParameter("@stokAdi", SqlDbType.NVarChar, 100);
                parameters[1].Value = YariMamulAdi;
                parameters[2] = new SqlParameter("@hamKodu", SqlDbType.NVarChar, 35);
                parameters[2].Value = HamKodu;
                parameters[3] = new SqlParameter("@en", SqlDbType.Int);
                parameters[3].Value = En;
                parameters[4] = new SqlParameter("@boy", SqlDbType.Int);
                parameters[4].Value = Boy;
                parameters[5] = new SqlParameter("@genislik", SqlDbType.Int);
                parameters[5].Value = Yukseklik;
                parameters[6] = new SqlParameter("@miktar", SqlDbType.Float);
                parameters[6].Value = Convert.ToDouble(M2);
                
                variables.Result = data.ExecuteStoredProcedureWithParameters("vbpInsertStokKartiveUrunRecetesiKoli",variables.Yil,parameters);

                if (!variables.Result)
                    return false;

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool UpdateStokAdi(string stokKodu,string stokAdi, string fabrika)
        {
            try
            {
                variables.Query = "update tblstsabit set stok_adi = @stokAdi where stok_kodu = @stokKodu";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@stokAdi", SqlDbType.NVarChar, 100);
                parameters[0].Value = stokAdi;
                parameters[1] = new SqlParameter("@stokKodu", SqlDbType.NVarChar, 35);
                parameters[1].Value = stokKodu;
                variables.Result = data.ExecuteCommandWithParameters(variables.Query,variables.Yil,parameters,fabrika);
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
