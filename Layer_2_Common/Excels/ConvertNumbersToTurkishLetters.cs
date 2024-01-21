using System;

namespace Layer_2_Common.Excels
{
    public class ConvertNumbersToTurkishLetters
    {
        static string[] ones = { "sıfır", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz" };
        static string[] tens = { "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };
        static string[] thousands = { "", "bin", "milyon", "milyar", "trilyon", "katrilyon" };

        public static string ConvertToTurkishText(decimal number)
        {
            string[] ones = { "sıfır", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz" };
            string[] tens = { "", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };
            string[] thousands = { "", "bin", "milyon", "milyar", "trilyon", "katrilyon" };

            string result = "";

            if (number == 0)
                return ones[0];

            string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            string[] parts = number.ToString().Split(decimalSeparator.ToCharArray());

            // Process the integer part
            string integerPart = parts[0];
            int integerLength = integerPart.Length;
            int groupCount = (integerLength + 2) / 3; // Calculate the number of groups (thousands, millions, etc.)

            for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
            {
                string group = integerPart.Substring(Math.Max(0, integerLength - (groupIndex + 1) * 3), Math.Min(3, integerLength - groupIndex * 3));
                int groupValue = int.Parse(group);

                if (groupValue > 0)
                {
                    if (groupValue < 10)
                    {
                        result = ones[groupValue] + " " + result;
                    }
                    else if (groupValue < 100)
                    {
                        int tensDigit = groupValue / 10;
                        int onesDigit = groupValue % 10;

                        if (onesDigit > 0)
                        {
                            result = tens[tensDigit] + " " + ones[onesDigit] + " " + result;
                        }
                        else
                        {
                            result = tens[tensDigit] + " " + result;
                        }
                    }
                    else
                    {
                        // Handle values greater than or equal to 100 (e.g., 100, 200, 110, etc.) here if needed.
                    }

                    if (groupIndex < thousands.Length)
                    {
                        result = thousands[groupIndex] + " " + result;
                    }
                }

            }

            // Process the fractional part if it exists
            if (parts.Length > 1)
            {
                string fractionalPart = parts[1];
                int fractionalLength = fractionalPart.Length;

                for (int i = 0; i < fractionalLength; i++)
                {
                    int digit = int.Parse(fractionalPart[i].ToString());
                    if (digit > 0)
                    {
                        result += ones[digit] + " ";
                    }
                }
            }

            return result.Trim();
        }

    }
}
