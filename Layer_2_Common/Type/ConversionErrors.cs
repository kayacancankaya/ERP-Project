namespace Layer_2_Common.Type
{
    public class ConversionErrors
    {
        public string ConversionError(string message)
        {
            string errorMessage = message.ToUpper() + " Dönüştürmeye Çalışırken Hata Oluştu.";
            return errorMessage;
        }
    }
}
