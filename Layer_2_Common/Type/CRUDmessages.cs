using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Layer_2_Common.Type
{
    public class CRUDmessages
    {
        public static string message;
        public static void InsertSuccessMessage(string tableName, int adet)
        {
            message = tableName.ToUpper() + " Tablosuna Başarıyla Kaydedildi. " + adet + " Kayıt İşlendi.";
            MessageBox.Show(message, "Başarılı İşlem",MessageBoxButton.OK,MessageBoxImage.Information);
        }
        public static void InsertSuccessMessage(string tableName)
        {
            message = tableName.ToUpper() + " Tablosuna Başarıyla Kaydedildi.";
            MessageBox.Show(message, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void UpdateSuccessMessage(string tableName, int adet)
        {
            message = tableName.ToUpper() + " Tablosu Başarıyla Güncellendi. " + adet + " Kayıt Güncellendi.";
            MessageBox.Show(message, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information); 
        }
        public static void UpdateSuccessMessage(string tableName)
        {
            message = tableName.ToUpper() + " Tablosu Başarıyla Güncellendi.";
            MessageBox.Show(message, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        public static void DeleteSuccessMessage(string tableName, int adet)
        {
            message = tableName.ToUpper() + " Tablosundan " + adet + " Kayıt Başarıyla Silindi.";
            MessageBox.Show(message, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        public static void DeleteSuccessMessage(string tableName)
        {
            message = tableName.ToUpper() + " Tablosundan Silme İşlemi Başarıyla Gerçekleştirildi.";
            MessageBox.Show(message, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        public static void InsertFailureMessage(string tableName)
        {
            message = tableName.ToUpper() + " Tablosuna Kayıt Yapılırken Hata İle Karşılaşıldı.";
            MessageBox.Show(message, "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void InsertFailureMessage(string tableName, int satirNo)
        {
            message = tableName.ToUpper() + " Tablosuna Kayıt Yapılırken Hata İle Karşılaşıldı." + satirNo + ". Satır";
            MessageBox.Show(message, "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void UpdateFailureMessage(string tableName)
        {
            message = tableName.ToUpper() + " Tablosunda Güncelleme Yapılırken Hata İle Karşılaşıldı.";
            MessageBox.Show(message, "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void DeleteFailureMessage(string tableName)
        {
            message = tableName.ToUpper() + " Tablosunda Silme İşlemi Yapılırken Hata İle Karşılaşıldı.";
            MessageBox.Show(message, "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void GeneralSuccessMessage(string whatHappenedSuccessfully)
        {
            message = string.Format("{0} Başarılı.", whatHappenedSuccessfully);
            MessageBox.Show(message, "Başarılı İşlem", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void GeneralFailureMessageNoInput()
        {
            MessageBox.Show("Hiç Değer Girmediniz.", "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void GeneralFailureMessageMoreSelectionThanExpected()
        {
            MessageBox.Show("Olması Gerekenden Fazla Seçim Yaptınız.", "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void GeneralFailureMessage(string errorEncounteredDuringWhatHappens)
        {
            message = string.Format("{0} Hata İle Karşılaşıldı.", errorEncounteredDuringWhatHappens);
            MessageBox.Show(message, "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void QueryIsEmpty(string whatIsNotFound)
        {

            message = string.Format("Listelenecek {0} Bulunamadı.", whatIsNotFound.Trim().ToUpper());
            MessageBox.Show(message, "Uyarı", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void NoInput()
        {
            message = "Hiç Değer Girmediniz";
            MessageBox.Show(message, "Uyarı", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void GeneralFailureMessageCustomMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Başarısız İşlem", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static bool DeleteOnayMessage()
        {
            MessageBoxResult result = MessageBox.Show("Silme İşlemini Onaylıyor Musunuz?", "Silme İşlemi", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) return true;

            else return false; 
      
        }
        public static bool UpdateOnayMessage()
        {
            MessageBoxResult result = MessageBox.Show("Güncelleme İşlemini Onaylıyor Musunuz?", "Güncelleme İşlemi", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) return true;

            else return false;

        }
        public static bool InsertOnayMessage()
        {
            MessageBoxResult result = MessageBox.Show("Kayıt İşlemini Onaylıyor Musunuz?", "Kayıt İşlemi", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) return true;

            else return false;

        }
        public static bool DoYouWishToContinue(string warningMessage)
        {
            MessageBoxResult result = MessageBox.Show(warningMessage + "\nDevam Etmek İstiyor Musunuz?", "Devam?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) return true;

            else return false; 
      
        }

    }
}
