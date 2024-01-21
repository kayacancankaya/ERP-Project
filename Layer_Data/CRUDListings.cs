using Layer_Data;
using System.Data;

namespace Layer_Data
{
    public class SelectStatement
    {
        static public DataTable GetDataTable(string query, int yil)
        {
            DataLayer dl = new DataLayer();

            DataTable dataTable = dl.Select_Command(query, yil);

            return dataTable;
        }
    }
}
