using System.ComponentModel;

namespace Layer_2_Common.Type
{ 

    public class Variables : INotifyPropertyChanged
    {
		private string _query = string.Empty;

		public string Query
		{
			get { return _query; }
			set { _query = value;
                OnPropertyChanged(nameof(Query));
            }
		}
		
		private string _query2 = string.Empty;

		public string Query2
		{
			get { return _query2; }
			set { _query2 = value;
                OnPropertyChanged(nameof(Query2));
            }
		}

		private string _fabrika = "Vita";

		public string Fabrika
		{
			get { return _fabrika; }
			set { 
				_fabrika = value; 
				OnPropertyChanged(nameof(_fabrika));
				}
		}



		private int _yil = 2024;

		public int	Yil
		{
			get { return _yil; }
			set { _yil = value;
                OnPropertyChanged(nameof(Yil));
                }
		}


		private decimal _kdv = 10;

		public decimal Kdv
		{
			get { return _kdv; }
			set {
                _kdv = value;
                OnPropertyChanged(nameof(Kdv));
                }
		}
		private int _current_year = 2024;

		public int	CurrentYear
		{
			get { return _current_year; }
			set {
                _current_year = value;
                OnPropertyChanged(nameof(CurrentYear));
                }
		}

        public static string LoadingSymbolofCursor { get; set; } = "\\\\192.168.1.11\\Netsis\\Images\\aero_busy.ani";

		public static string ImagePath = "\\\\192.168.1.11\\Netsis\\Images\\vb.png";

		public string FilePath = string.Empty;
        public string SheetName = string.Empty;

        private bool _isChecked = false;
        public bool IsChecked
		{
			get { return _isChecked; }
			set { _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                
            }
		}
        private bool _isTrue = false;
        public bool IsTrue
		{
			get { return _isTrue; }
			set {
                _isTrue = value;
                OnPropertyChanged(nameof(IsTrue));
                
            }
		}

		private bool _result = false;

		public bool Result
		{
			get { return _result; }
			set { 
				_result = value; 
				OnPropertyChanged(nameof(Result));
				}
		}
		private bool _continue = true;

		public bool Continue
		{
			get { return _continue; }
			set {
                _continue = value; 
				OnPropertyChanged(nameof(Continue));
				}
		}
		private int _resultInt = 0;

		public int ResultInt
		{
			get { return _resultInt; }
			set {
                _resultInt = value; 
				OnPropertyChanged(nameof(ResultInt));
				}
		}
		private int _resultInt16 = 0;

		public int ResultInt16
		{
			get { return _resultInt16; }
			set {
                _resultInt16 = value; 
				OnPropertyChanged(nameof(ResultInt));
				}
		}


		private int _counter = 0;

		public int Counter
		{
			get { return _counter; }
			set { _counter = value; }
		}

		private string _errorMessage = string.Empty;

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { _errorMessage = value;
				OnPropertyChanged(nameof(ErrorMessage));
			}
		}

		private string _successMessage = string.Empty;

		public string SuccessMessage
		{
			get { return _successMessage; }
			set {
                _successMessage = value;
				OnPropertyChanged(nameof(SuccessMessage));
			}
		}

        private string _warningMessage = string.Empty;

        public string WarningMessage
        {
            get { return _warningMessage; }
            set
            {
                _warningMessage = value;
                OnPropertyChanged(nameof(WarningMessage));
            }
        }

		private decimal _qumulativeSum;

		public decimal QumulativeSum
		{
			get { return _qumulativeSum; }
			set { _qumulativeSum = value; 
				OnPropertyChanged(nameof(QumulativeSum));
				}
		}

		private string _tableName = string.Empty;

		public string TableName
		{
			get { return _tableName = string.Empty; }
			set { _tableName = value; 
				  OnPropertyChanged(nameof(TableName));
				}
		}


        private string _buttonName = string.Empty;
        public string ButtonName
        {
            get { return _buttonName; }
            set
            {
                _buttonName = value;
                OnPropertyChanged(nameof(ButtonName));
            }
        }


        private string _updateMessage = string.Empty;
        public string UpdateMessage
        {
            get { return _updateMessage; }
            set
            {
                _updateMessage = value;
                OnPropertyChanged(nameof(UpdateMessage));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyChanged)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
        }
        
    }
}
