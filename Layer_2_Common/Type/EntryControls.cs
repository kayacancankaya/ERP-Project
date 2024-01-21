namespace Layer_2_Common.Type
{
    public class EntryControls
    {
        public static bool IsValidDecimal(string input)
        {

            if (decimal.TryParse(input, out decimal result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
