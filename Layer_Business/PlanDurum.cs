using Layer_2_Common.Type;
using Layer_Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Layer_Business
{

    public class PlanDurum : INotifyPropertyChanged
    {
        public string SiparisTarih { get; set; }
        public string SiparisNo { get; set; }
        public string SiparisSira { get; set; }
        public string SiparisDurum { get; set; }
        public string SatisCariKod { get; set; }
        public string SatisCariIsim { get; set; }
        public string CariKod { get; set; }
        public string CariIsim { get; set; }
        public string Destinasyon { get; set; }
        public string PO { get; set; }
        public string PlanNo { get; set; }
        public string TakipNo { get; set; }
        public string ReferansIsemri { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string Durum { get; set; }
        public string IhtiyacDuyulanTarih { get; set; }
        public string GercekTarih { get; set; }
        public string PlanlananTarih { get; set; }
        public string Aciklama { get; set; }
        public string PlanlamaAciklama { get; set; }
        public string UrunMiktar { get; set; }
        public string PaketAdet { get; set; }
        public string ToplamPaket { get; set; }
        public string KalanPaket { get; set; }
        public string MamulStok { get; set; }
        public string SevkMiktar { get; set; }
        public string PaketStok { get; set; }
        public string DosemeIeMik { get; set; }
        public string DosemeUrsMik { get; set; }
        public string KonfeksiyonIeMik { get; set; }
        public string KonfeksiyonUrsMik { get; set; }
        public string KumasIeMik { get; set; }
        public string KumasUrsMik { get; set; }
        public string PlakaIeMik { get; set; }
        public string PlakaUrsMik { get; set; }
        public string IskeletIeMik { get; set; }
        public string IskeletUrsMik { get; set; }
        public string KBandiIeMik { get; set; }
        public string KBandiUrsMik { get; set; }
        public string SiparisSatirAciklama { get; set; }
        public string SiparisGenelAciklama { get; set; }

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
        
        private int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private int _selectedRecord =100 ;
        public int SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                OnPropertyChanged(nameof(SelectedRecord));
            }
        }

        private int _numberOfRows;
        public int NumberOfRows
        {
            get { return _numberOfRows; }
            set
            {
                _numberOfRows = value;
                OnPropertyChanged(nameof(NumberOfRows));
            }
        }

        private int _numberOfPages = 20;

        public int NumberOfPages
        {
            get { return _numberOfPages; }
            set
            {
                _numberOfPages = value;
                OnPropertyChanged(nameof(NumberOfPages));
                OnPropertyChanged(nameof(NumberOfRows));

            }
        }


        private bool _isFirstEnabled = false;

        public bool IsFirstEnabled
        {
            get { return _isFirstEnabled; }
            set
            {
                _isFirstEnabled = value;
                OnPropertyChanged(nameof(IsFirstEnabled));
            }
        }


        private bool _isPreviousEnabled = false;

        public bool IsPreviousEnabled
        {
            get { return _isPreviousEnabled; }
            set
            {
                _isPreviousEnabled = value;
                OnPropertyChanged(nameof(IsPreviousEnabled));

            }
        }


        private bool _isNextEnabled = true;

        public bool IsNextEnabled
        {
            get { return _isNextEnabled; }
            set
            {
                _isNextEnabled = value;
                OnPropertyChanged(nameof(IsNextEnabled));

            }
        }


        private bool _isLastEnabled = true;

        public bool IsLastEnabled
        {
            get { return _isLastEnabled; }
            set
            {
                _isLastEnabled = value;

                OnPropertyChanged(nameof(IsLastEnabled));
            }
        }

        
        private string _query = "";

        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;

                OnPropertyChanged(nameof(Query));
            }
        }


        private string _queryCount = "";

        public string QueryCount
        {
            get { return _queryCount; }
            set
            {
                _queryCount = value;

                OnPropertyChanged(nameof(QueryCount));
            }
        }


        private string _searchQuery = "";

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;

                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private ObservableCollection<PlanDurum> _planDurumCollection;

        public ObservableCollection<PlanDurum> PlanDurumCollection
        {
            get { return _planDurumCollection; }
            set
            {
                if (_planDurumCollection != value)
                {
                    _planDurumCollection = value;
                    OnPropertyChanged(nameof(PlanDurumCollection)); // Notify property change if you are implementing INotifyPropertyChanged
                }
            }
        }

        ObservableCollection<PlanDurum> planDurums = new ObservableCollection<PlanDurum>();  

        DataLayer dataLayer = new DataLayer();

        Variables variables = new();

        LoginLogic login = new();

        SqlDataReader reader;

        //public ICommand CariGuncelleCommand { get; private set; }
        //public ICommand POGuncelleCommand { get; private set; }

        public PlanDurum()
        {
            // Initialize the commands
            //CariGuncelleCommand = new RelayCommand(CariGuncelle);
            //POGuncelleCommand = new RelayCommand(POGuncelle);
            variables.Fabrika = login.GetFabrika();
        }

        public void DataGridContextMenuUpdate(string updateQuery,string tableName,int counter)
        {
            
            if (updateQuery != string.Empty)
            {
                dataLayer.Update_Statement(updateQuery, variables.Yil, tableName, counter);
            }
            else
            {
                MessageBox.Show("Sorgu Değer Döndürmedi.");
            }

        }


        public ObservableCollection<PlanDurum> DisplayedRowSelectionChanged()
        {
            Query = $"select top {SelectedRecord} * from vbvPlanDurum where 1=1 {SearchQuery} order by SiparisTarih desc";
            PlanDurumCollection = PopulateMainWindow(Query);

            QueryCount = "select count(*) from vbvPlanDurum where 1=1 " + SearchQuery;
            UpdatePageNumber(QueryCount);
            OnPropertyChanged(nameof(PlanDurumCollection));
            return PlanDurumCollection;
                
        }
        public int UpdatePageNumber(string QueryCount)
        {
            try
            {

            reader = dataLayer.Select_Command_Data_Reader(QueryCount, variables.Yil,variables.Fabrika);
			
            if(!reader.Read())
                    return -1;
                 
			NumberOfRows = Convert.ToInt32(reader[0]);
			reader.Close();
            if (NumberOfRows < SelectedRecord)
            {
                NumberOfPages = 1;
            }
            else
            { 
            NumberOfPages = Convert.ToInt32(Math.Floor((double)NumberOfRows / SelectedRecord));
            }

            OnPropertyChanged(nameof(NumberOfRows));
            OnPropertyChanged(nameof(NumberOfPages));

            return NumberOfPages;
			}
			catch
			{
				return -1;
			}
		}

        public ObservableCollection<PlanDurum> PopulateMainWindow(string Query)
        {
            try
            { 
            if (PlanDurumCollection != null)
            { 
            PlanDurumCollection.Clear();
            }
            
            if (planDurums != null)
            {
                planDurums.Clear();
            }
            
            reader = dataLayer.Select_Command_Data_Reader(Query,variables.Yil,variables.Fabrika);

            if (reader == null)
                return null;

                if (variables.Fabrika == "Ahşap")
                {
                    while (reader.Read())
                    {
                        // Create an instance of the ViewModel and populate it from the DataRow
                        PlanDurum planDurum = new PlanDurum
                        {
                            SiparisTarih = reader["SiparisTarih"].ToString(),
                            SiparisNo = reader["SiparisNo"].ToString(),
                            SiparisSira = reader["SiparisSira"].ToString(),
                            SiparisDurum = reader["SiparisDurum"].ToString(),
                            CariKod = reader["CARI_KOD"].ToString(),
                            CariIsim = reader["CARI_ISIM"].ToString(),
                            SiparisSatirAciklama = reader["SiparisSatirAciklama"].ToString(),
                            SiparisGenelAciklama = reader["SiparisGenelAciklama"].ToString(),
                            ReferansIsemri = reader["ReferansIsemri"].ToString(),
                            UrunKodu = reader["STOK_KODU"].ToString(),
                            UrunAdi = reader["STOK_ADI"].ToString(),
                            Durum = reader["DURUM"].ToString(),
                            IhtiyacDuyulanTarih = reader["TerminTarih"].ToString(),
                            PlanlananTarih = reader["TeslimTarih"].ToString(),
                            PlanlamaAciklama = reader["Aciklama"].ToString(),
                            UrunMiktar = reader["SiparisMiktar"].ToString(),
                            MamulStok = reader["MamulStok"].ToString(),
                            SevkMiktar = reader["SevkMiktar"].ToString(),
                        };
                        planDurums.Add(planDurum);
                        
                    }
                }
                else if (variables.Fabrika == "Vita")
                {
                    while (reader.Read())
                    {
                        // Create an instance of the ViewModel and populate it from the DataRow
                        PlanDurum planDurum = new PlanDurum
                        {
                            SiparisTarih = reader["SiparisTarih"].ToString(),
                            SiparisNo = reader["SiparisNo"].ToString(),
                            SiparisSira = reader["SiparisSira"].ToString(),
                            SiparisDurum = reader["SiparisDurum"].ToString(),
                            SatisCariKod = reader["SATIS_CARI_KOD"].ToString(),
                            SatisCariIsim = reader["SATIS_CARI_ISIM"].ToString(),
                            CariKod = reader["CARI_KOD"].ToString(),
                            CariIsim = reader["CARI_ISIM"].ToString(),
                            Destinasyon = reader["Destinasyon"].ToString(),
                            PO = reader["PO"].ToString(),
                            PlanNo = reader["PlanNo"].ToString(),
                            TakipNo = reader["TakipNo"].ToString(),
                            ReferansIsemri = reader["ReferansIsemri"].ToString(),
                            UrunKodu = reader["STOK_KODU"].ToString(),
                            UrunAdi = reader["STOK_ADI"].ToString(),
                            Durum = reader["DURUM"].ToString(),
                            IhtiyacDuyulanTarih = reader["TerminTarih"].ToString(),
                            PlanlananTarih = reader["TeslimTarih"].ToString(),
                            PlanlamaAciklama = reader["Aciklama"].ToString(),
                            UrunMiktar = reader["SiparisMiktar"].ToString(),
                            PaketAdet = reader["paketAdet"].ToString(),
                            ToplamPaket = reader["toplamPaket"].ToString(),
                            KalanPaket = reader["KalanPaket"].ToString(),
                            MamulStok = reader["MamulStok"].ToString(),
                            SevkMiktar = reader["SevkMiktar"].ToString(),
                            PaketStok = reader["PaketStok"].ToString(),
                            DosemeIeMik = reader["DosemeIeMik"].ToString(),
                            DosemeUrsMik = reader["DosemeUrsMik"].ToString(),
                            KonfeksiyonIeMik = reader["KonfeksiyonIeMik"].ToString(),
                            KonfeksiyonUrsMik = reader["KonfeksiyonUrsMik"].ToString(),
                            KumasIeMik = reader["KumasIeMik"].ToString(),
                            KumasUrsMik = reader["KumasUrsMik"].ToString(),
                            PlakaIeMik = reader["PlakaIeMik"].ToString(),
                            PlakaUrsMik = reader["PlakaUrsMik"].ToString(),
                            IskeletIeMik = reader["IskeletIeMik"].ToString(),
                            IskeletUrsMik = reader["IskeletUrsMik"].ToString(),
                            KBandiIeMik = reader["KBandiIeMik"].ToString(),
                            KBandiUrsMik = reader["KBandiUrsMik"].ToString()
                        };
                        planDurums.Add(planDurum);
                    }
                }
                else
                    return null;

                reader.Close();
                PlanDurumCollection = planDurums;
                OnPropertyChanged(nameof(PlanDurumCollection));
                return PlanDurumCollection;
			}
			catch
			{
				return null;
			}
		}

        private void UpdateEnableState()
        {
            IsFirstEnabled = CurrentPage > 1;
            IsPreviousEnabled = CurrentPage > 1;
            IsNextEnabled = CurrentPage < NumberOfPages;
            IsLastEnabled = CurrentPage < NumberOfPages;

            OnPropertyChanged(nameof(IsFirstEnabled));
            OnPropertyChanged(nameof(IsPreviousEnabled));
            OnPropertyChanged(nameof(IsNextEnabled));
            OnPropertyChanged(nameof(IsLastEnabled));
            OnPropertyChanged(nameof(CurrentPage));

        }

        public ObservableCollection<PlanDurum> PagingButtonsClicked(string buttonName) 
        {
            switch (buttonName)
            {
                case "btn_pag_previous":
                    CurrentPage --;
                    break;

                case "btn_pag_first":
                    CurrentPage = 1;
                    break;

                case "btn_pag_next":
                    CurrentPage ++;
                    break;

                case "btn_pag_last":
                    CurrentPage = NumberOfPages;
                    break;

                default:
                    break;
            }

            int startListingFrom = CurrentPage * SelectedRecord;
            
            Query = $"select * from vbvPlanDurum where 1=1 {SearchQuery} order by SiparisTarih desc offset {startListingFrom} rows fetch next {SelectedRecord} rows only";

            PlanDurumCollection = PopulateMainWindow(Query);

            UpdateEnableState();

            return PlanDurumCollection;
        
        }

        public ObservableCollection<PlanDurum> SearchBoxChanged(string searchString1, string searchString2, string searchString3, string searchString4)
        {


            SearchQuery = string.Empty;

            if (variables.Fabrika == "Vita")
            { 
                 if (!string.IsNullOrEmpty(searchString1))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString1}%' or SiparisSira like '%{searchString1}%' or SiparisTarih like '%{searchString1}%' or CARI_ISIM like '%{searchString1}%' " +
                                                $" or SATIS_CARI_KOD like '%{searchString1}%' or SATIS_CARI_ISIM like '%{searchString1}%' or CARI_KOD like '%{searchString1}%' or Destinasyon like '%{searchString1}%' " +
                                                $" or PO like '%{searchString1}%' or PlanNo like '%{searchString1}%' or TakipNo like '%{searchString1}%' or ReferansIsemri like '%{searchString1}%' " +
                                                $" or STOK_KODU like '%{searchString1}%' or STOK_ADI like '%{searchString1}%' or TerminTarih like '%{searchString1}%' or TeslimTarih like '%{searchString1}%' " +
                                                $" or Aciklama like '%{searchString1}%') "; 

			    }
                if (!string.IsNullOrEmpty(searchString2))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString2}%' or SiparisSira like '%{searchString2}%' or SiparisTarih like '%{searchString2}%' or CARI_ISIM like '%{searchString2}%' " +
											    $" or SATIS_CARI_KOD like '%{searchString2}%' or SATIS_CARI_ISIM like '%{searchString2}%' or CARI_KOD like '%{searchString2}%' or Destinasyon like '%{searchString2}%' " +
											    $" or PO like '%{searchString2}%' or PlanNo like '%{searchString2}%' or TakipNo like '%{searchString2}%' or ReferansIsemri like '%{searchString2}%' " +
											    $" or STOK_KODU like '%{searchString2}%' or STOK_ADI like '%{searchString2}%' or TerminTarih like '%{searchString2}%' or TeslimTarih like '%{searchString2}%' " +
											    $" or Aciklama like '%{searchString2}%') ";
			    }
                if (!string.IsNullOrEmpty(searchString3))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString3}%' or SiparisSira like '%{searchString3}%' or SiparisTarih like '%{searchString3}%' or CARI_ISIM like '%{searchString3}%' " +
											    $" or SATIS_CARI_KOD like '%{searchString3}%' or SATIS_CARI_ISIM like '%{searchString3}%' or CARI_KOD like '%{searchString3}%' or Destinasyon like '%{searchString3}%' " +
											    $" or PO like '%{searchString3}%' or PlanNo like '%{searchString3}%' or TakipNo like '%{searchString3}%' or ReferansIsemri like '%{searchString3}%' " +
											    $" or STOK_KODU like '%{searchString3}%' or STOK_ADI like '%{searchString3}%' or TerminTarih like '%{searchString3}%' or TeslimTarih like '%{searchString3}%' " +
											    $" or Aciklama like '%{searchString3}%') "; 
                }
                if (!string.IsNullOrEmpty(searchString4))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString4}%' or SiparisSira like '%{searchString4}%' or SiparisTarih like '%{searchString4}%' or CARI_ISIM like '%{searchString4}%' " +
											    $" or SATIS_CARI_KOD like '%{searchString4}%' or SATIS_CARI_ISIM like '%{searchString4}%' or CARI_KOD like '%{searchString4}%' or Destinasyon like '%{searchString4}%' " +
											    $" or PO like '%{searchString4}%' or PlanNo like '%{searchString4}%' or TakipNo like '%{searchString4}%' or ReferansIsemri like '%{searchString4}%' " +
											    $" or STOK_KODU like '%{searchString4}%' or STOK_ADI like '%{searchString4}%' or TerminTarih like '%{searchString4}%' or TeslimTarih like '%{searchString4}%' " +
											    $" or Aciklama like '%{searchString4}%') ";
			    }
            }
            if (variables.Fabrika == "Ahşap")
            {
                if (!string.IsNullOrEmpty(searchString1))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString1}%' or SiparisSira like '%{searchString1}%' or SiparisTarih like '%{searchString1}%' or CARI_ISIM like '%{searchString1}%' " +
                                                $" or CARI_KOD like '%{searchString1}%' or SiparisGenelAciklama like '%{searchString1}%' or SiparisSatirAciklama like '%{searchString1}%' " +
                                                $" or ReferansIsemri like '%{searchString1}%' " +
                                                $" or STOK_KODU like '%{searchString1}%' or STOK_ADI like '%{searchString1}%' or TerminTarih like '%{searchString1}%' or TeslimTarih like '%{searchString1}%' " +
                                                $" or Aciklama like '%{searchString1}%') ";

                }
                if (!string.IsNullOrEmpty(searchString2))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString2}%' or SiparisSira like '%{searchString2}%' or SiparisTarih like '%{searchString2}%' or CARI_ISIM like '%{searchString2}%' " +
                                                $" or CARI_KOD like '%{searchString2}%' or SiparisGenelAciklama like '%{searchString2}%' or SiparisSatirAciklama like '%{searchString2}%' " +
                                                $" or ReferansIsemri like '%{searchString2}%' " +
                                                $" or STOK_KODU like '%{searchString2}%' or STOK_ADI like '%{searchString2}%' or TerminTarih like '%{searchString2}%' or TeslimTarih like '%{searchString2}%' " +
                                                $" or Aciklama like '%{searchString2}%') ";
                }
                if (!string.IsNullOrEmpty(searchString3))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString3}%' or SiparisSira like '%{searchString3}%' or SiparisTarih like '%{searchString3}%' or CARI_ISIM like '%{searchString3}%' " +
                                                $" or CARI_KOD like '%{searchString3}%' or SiparisGenelAciklama like '% {searchString3} %' or SiparisSatirAciklama like '%{searchString3}%' " +
                                                $" or ReferansIsemri like '%{searchString3}%' " +
                                                $" or STOK_KODU like '%{searchString3}%' or STOK_ADI like '%{searchString3}%' or TerminTarih like '%{searchString3}%' or TeslimTarih like '%{searchString3}%' " +
                                                $" or Aciklama like '%{searchString3}%') ";
                }
                if (!string.IsNullOrEmpty(searchString4))
                {
                    SearchQuery = SearchQuery + $" and (SiparisNo like '%{searchString4}%' or SiparisSira like '%{searchString4}%' or SiparisTarih like '%{searchString4}%' or CARI_ISIM like '%{searchString4}%' " +
                                                $" or CARI_KOD like '%{searchString4}%' or SiparisGenelAciklama like '% {searchString4} %' or SiparisSatirAciklama like '%{searchString4}%' " +
                                                $" or ReferansIsemri like '%{searchString4}%' " +
                                                $" or STOK_KODU like '%{searchString4}%' or STOK_ADI like '%{searchString4}%' or TerminTarih like '%{searchString4}%' or TeslimTarih like '%{searchString4}%' " +
                                                $" or Aciklama like '%{searchString4}%') ";
                }
            }

            CurrentPage = 1;

            int startListingFrom = 0;

            string paginationQuery = $"order by CONVERT(DATE, SiparisTarih, 104) DESC offset {startListingFrom} rows fetch next {SelectedRecord} rows only";
            Query = $"select * from vbvPlanDurum where 1=1 {SearchQuery} {paginationQuery}";
            QueryCount = $"select count(*) from vbvPlanDurum where 1=1 {SearchQuery}";

            planDurums = PopulateMainWindow(Query);
            PlanDurumCollection = planDurums;
            UpdatePageNumber(QueryCount);
            UpdateEnableState();
            OnPropertyChanged(nameof(PlanDurumCollection));
            OnPropertyChanged(nameof(SearchQuery));
            OnPropertyChanged(nameof(NumberOfPages));
            OnPropertyChanged(nameof(CurrentPage));

            return PlanDurumCollection;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string getStr)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(getStr));
        }
   
    }
}
