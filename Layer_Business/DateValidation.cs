using System;
using System.Globalization;

namespace Layer_Business
{
    public class DateValidation
    {
        public bool CheckDate(string date)
        { 
        // Define the expected format
            string format = "dd.MM.yyyy";

        // Try to parse the date string
            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string ConverttoDateString(string date) 
        {
            string originalDateString = date; // Original date string in dd.mm.yyyy format

            // Define the expected format for the original string
            string originalFormat = "dd.MM.yyyy";

            if (DateTime.TryParseExact(originalDateString, originalFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Format the DateTime object to the desired format (yyyy-MM-dd)
                string formattedDate = parsedDate.ToString("yyyy-MM-dd");
                return formattedDate;
            }
            else
            {
                return "Hata";
            }
        }
    }
}
