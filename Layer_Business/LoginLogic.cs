using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Layer_2_Common.Type;
using Layer_Data;

namespace Layer_Business
{ 
    public class LoginLogic : INotifyPropertyChanged
    {


        private string _user;


        public string User
        {
            get { return _user; }
            set
            {
                _user = value;

                OnPropertyChanged(nameof(User));
            }
        }
        private int _userID;

        public int UserID
        {
            get { return _userID; }
            set
            {
                _userID = value;

                OnPropertyChanged(nameof(UserID));
            }
        }
        private int _portalID;

        public int PortalID
        {
            get { return _portalID; }
            set
            {
                _portalID = value;

                OnPropertyChanged(nameof(PortalID));
            }
        }


        private string _password;

        public string Password
        {
            get { return _password; }
            set {
                _password = value;

                OnPropertyChanged(nameof(Password));
            }
        }
        private string _address;

        public string Address { get; set; }

        private bool _isCheckedAutoLogin = true;
        string hashedPass = string.Empty;

        private string _departman;

        public string Departman
        {
            get { return _departman; }
            set { _departman = value;
                OnPropertyChanged(nameof(Departman));
            }
        }


        public bool IsCheckedAutoLogin
        {
            get { return _isCheckedAutoLogin; }
            set {
                _isCheckedAutoLogin = value;
                OnPropertyChanged(nameof(IsCheckedAutoLogin));
            }
        }

        Variables variables = new ();
        DataLayer dataLayer = new ();
        int numberOfRows = 0;
        SqlDataReader reader;
        private string hashedAddr = string.Empty;
        public bool CheckIfUserExists(string username)
        {
            try
            {

            variables.Query = $"select count(*) from vbtUserInfo where kullaniciAdi='{username}'";

            reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);
			while(reader.Read())
            {
				numberOfRows = Convert.ToInt32(reader[0]);
			}
			if (!reader.HasRows)
                return false;

            reader.Close();

			bool result = numberOfRows > 0 ? true : false;
            return result;

			}
			catch (Exception)
			{
                return false;
			}
		}
        private bool CheckIfPasswordExists(string hashedPassword)
        {
            try
            {

            variables.Query = $"select count(*) from vbtUserInfo where sifre='{hashedPassword}'";

            reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

            while (reader.Read())
            { 
            numberOfRows = Convert.ToInt32(reader[0]);
		    }
            
			if (!reader.HasRows)
				return false;
            
            reader.Close();

			bool result = numberOfRows > 0 ? true : false;
			return result;

			}
			catch (Exception)
			{
                return false;
			}

		}
		public bool CheckIfPcAddressExists()
        {

            Address = GetMotherboardSerialNumber();

            hashedAddr = HashPassword(Address);

            variables.Query = $"select count(*) from vbtUserInfo where adres='{hashedAddr}'";

            reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

            while(reader.Read())
            { 
			numberOfRows = Convert.ToInt32(reader[0]);
			}

			if (!reader.HasRows)
				return false;

			reader.Close(); 
            
            bool result = numberOfRows > 0 ? true : false;
            return result;

        }
        public void UpdatePCAutoLoginStatustoZero()
        {
            try
            { 

            hashedAddr = HashPassword(GetMotherboardSerialNumber());

            variables.Query = $"update vbtUserInfo set OtoLogin='0' where adres='{hashedAddr}'";

            dataLayer.Update_Statement(variables.Query, variables.CurrentYear);
            }
            catch (Exception ex) {MessageBox.Show(ex.Message); }

        }

        public bool SaveNewUser(string user, string password, string department, string email)
        {
            Address = GetMotherboardSerialNumber();

            hashedAddr = HashPassword(Address);

            hashedPass = HashPassword(password);
            try
            {
                variables.Query = string.Format("insert into vbtUserInfo (KullaniciAdi,sifre,Departman,Email,Adres,OtoLogin) values('{0}','{1}','{2}','{3}','{4}',1)", user, hashedPass, department, email,hashedAddr);
                dataLayer.Insert_Statement(variables.Query, variables.CurrentYear, "Kullanıcı", 1);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }

            bool result = CheckIfUserExists(user); 
            return result;
        }

        public bool SavePaswordChanged(string user, string password)
        {
            Address = GetMotherboardSerialNumber();

            hashedAddr = HashPassword(Address);


            hashedPass = HashPassword(password);
            try
            {
                variables.Query = string.Format("update vbtUserInfo set sifre='{0}' where Adres='{1}' and KullaniciAdi='{2}'", hashedPass, hashedAddr, user);
                dataLayer.Update_Statement(variables.Query, variables.CurrentYear, "Kullanıcı", 1);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }

            bool result = CheckIfPasswordExists(hashedPass);
            return result;
        }
        public int CheckLoginAttemp(string username,string passwordentry)
        {
            try
            { 
            hashedPass = HashPassword(passwordentry);

            variables.Query = $"select count(*) from vbtUserInfo where sifre='{hashedPass}' and kullaniciAdi='{username}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

                while (reader.Read()) { 
				numberOfRows = Convert.ToInt32(reader[0]);
				}

				if (!reader.HasRows)
					return 3;

				reader.Close();

				return numberOfRows;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return 3; }
        }

        public int IfStatusAutoLogin()
        {
            try
            { 
            int IsAutoLogin = 0;

            Address = GetMotherboardSerialNumber();

            hashedAddr = HashPassword(Address);

            variables.Query = $"select top 1 OtoLogin from vbtUserInfo where adres='{hashedAddr}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

                while(reader.Read()) 
                { 
				IsAutoLogin = Convert.ToInt32(reader[0]);
				}

				if (!reader.HasRows)
					return 3;

				reader.Close();

            return IsAutoLogin;
            }

            catch (Exception ex) { MessageBox.Show(ex.Message); return 3; }
        }

       public string CallGetMotherboardSerialNumber()
        { return GetMotherboardSerialNumber(); }

        public string CallHashPassword(string address)
        { return HashPassword(address); }
        

        public string GetUserName()
        {
            hashedAddr = HashPassword(GetMotherboardSerialNumber());
            try
            {
                variables.Query = $"select top 1 KullaniciAdi from vbtUserInfo where Adres='{hashedAddr}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

				while (reader.Read())
				{
					User = reader[0].ToString();
				}
				if (!reader.HasRows)
					return null;

				reader.Close();

				return User;
				
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
           
        }

        public string GetUserName(string HashedAddress) 
        {
            try
            { 
            variables.Query = $"select top 1 KullaniciAdi from vbtUserInfo where Adres='{HashedAddress}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

				while (reader.Read())
				{
					User = reader[0].ToString();
				}
				if (!reader.HasRows)
					return null;

				reader.Close(); 

				return User;

			}
			catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
            
        }

		public int GetPortalID()
		{

			PortalID = ConvertUserIDtoPortalID(GetUserId());
			return PortalID;
		}
		public int GetUserIDFromPortalID(int portalID)
		{

			UserID = ConverPortalIDtoUserID(portalID);
			return UserID;
		}
		public string GetAddress()
		{
			Address = HashPassword(GetMotherboardSerialNumber());
			return Address;
		}

        public string GetFabrika()
        {
            try
            { 
                string fabrika = string.Empty;

                variables.Query = "Select top 1 departman from vbtUserInfo";
                using (SqlDataReader reader = dataLayer.Select_Command_Data_Reader(variables.Query,variables.Yil))
                {
                    if (reader == null)
                        return string.Empty;

                    while (reader.Read())
                    {
                        if (!reader.HasRows)
                            return string.Empty;
                        Departman = reader[0].ToString();
                    }
                }
                if (Departman.Contains("Ahsap"))
                    fabrika = "Ahşap";
                else
                    fabrika = "Vita";

                return fabrika;
            }
            catch 
            {
                return string.Empty;
            }
        }
		public string GetDepartment()
		{
			hashedAddr = HashPassword(GetMotherboardSerialNumber());
			try
			{
				variables.Query = $"select top 1 Departman from vbtUserInfo where Adres='{hashedAddr}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);


				while (reader.Read())
				{
					Departman = reader[0].ToString();
				}
				if (!reader.HasRows)
					return null;
				reader.Close();


				return Departman;

			}
			catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
		}

		public int GetUserId()
        {
            hashedAddr = HashPassword(GetMotherboardSerialNumber());
            try
            {
                variables.Query = $"select top 1 UserId from vbtUserInfo where Adres='{hashedAddr}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

				while (reader.Read())
				{
					UserID = Convert.ToInt32(reader[0]);
				}
				if (!reader.HasRows)
					return 3;

				reader.Close();

				return UserID;
		    }
            catch (Exception ex) { MessageBox.Show(ex.Message); return 0; }
        }
        public string GetUserNameFromUserID(int userID)
        {
            hashedAddr = HashPassword(GetMotherboardSerialNumber());
            try
            {
                variables.Query = $"select top 1 KullaniciAdi from vbtUserInfo where UserID='{userID}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

				while (reader.Read())
				{
					User = reader[0].ToString();
				}
				if (!reader.HasRows)
					return "KULLANICI BULUNAMADI.";

				reader.Close();

				return User;
		    }
            catch (Exception ex) { MessageBox.Show(ex.Message); return "HATA"; }
        }

        public int ConvertUserIDtoPortalID(int userID)
        {
            try
            {

                switch(userID)
                { 
                    case (1):
                    PortalID = 71;
                    break;
                    
                    case (2):
                    PortalID = 3;
                    break;
                    
                    case (3):
                    PortalID = 94;
                    break;
                    
                    case (4):
                    PortalID = 96;
                    break;
                    
                    case (5):
                    PortalID = 82;
                    break;

                    case (6):
                    PortalID = 81;
                    break;

                    case (7):
                    PortalID = 100;
                    break;

                    case (8):
                    PortalID = 9;
                    break;

                    case (11):
                        PortalID = 81;
                        break;

                    case (12):
                        PortalID = 71;
                        break;
                    case (13):

                        PortalID = 47;
                        break;
                }

                return PortalID;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); return 0; }
            
        }
        public int ConverPortalIDtoUserID(int portalID)
        {
            try
            {

                switch(portalID)
                { 
                    case (71):
						UserID = 1;
                    break;
                    
                    case (3):
						UserID = 2;
                    break;
                    
                    case (94):
                    UserID = 3;
                    break;
                    
                    case (96):
                    UserID = 4;
                    break;
                    
                    case (82):
                    UserID = 5;
                    break;

                    case (81):
						UserID = 6;
                    break;

                    case (100):
						UserID = 7;
                    break;

                    case (9):
						UserID = 8;
                    break;

                    case (47):
                        UserID = 13;
                        break;
                }

                return UserID;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); return 0; }
            
        }


        public bool AutoLoginAttemp()
        {
            try
            {
                Address = GetMotherboardSerialNumber();
                hashedAddr = HashPassword(Address);

                variables.Query = $"select count(*) from vbtUserInfo where adres='{hashedAddr}'";

				reader = dataLayer.Select_Command_Data_Reader(variables.Query, variables.CurrentYear);

				while (reader.Read())
				{
					numberOfRows = Convert.ToInt32(reader[0]);
				}
				if (!reader.HasRows)
					return false;
				
				reader.Close();

				return true;
		    }

            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        public void UpdateAutoLoginStatus(string user, string password) 
        {
            try
            {

                hashedPass = HashPassword(password);
                variables.Query = string.Empty;

                if (!IsCheckedAutoLogin)
                {
                    variables.Query = $"update vbtUserInfo set OtoLogin=0 where KullaniciAdi='{user}' and sifre='{hashedPass}'";
                }
                else
                {
                    variables.Query = $"update vbtUserInfo set OtoLogin=1 where KullaniciAdi='{user}' and sifre='{hashedPass}'";
                }

                dataLayer.Update_Statement(variables.Query, variables.CurrentYear);
            }

            catch (Exception ex) { MessageBox.Show(ex.Message);}
        }

        private static string GetMotherboardSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject obj in collection)
                {
                    if (obj["SerialNumber"] != null)
                    {
                        return obj["SerialNumber"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            //// Fallback to a random unique identifier if unable to retrieve the motherboard serial number.
            //return Guid.NewGuid().ToString();
            return null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert the byte to a hexadecimal string.
                }

                return builder.ToString();
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;
       
        protected void OnPropertyChanged(string propertyChanged)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
        }


    }
}
