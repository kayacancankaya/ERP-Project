using System.Data.SqlClient;
using System.Data;
using System.Windows.Input;
using System;
using Layer_2_Common.Type;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Layer_Data
{
    public class DataLayer
    {
        private string ConnectionString { get; set; } = "";
        SqlConnection connection = null;
        public SqlDataReader Select_Command_Data_Reader_With_Parameters(string query, int yil, SqlParameter[] parameters, string fabrika)
        {
            if (fabrika == "Ahşap")
                ConnectionString = GetConnectionStringAhsap(yil);
            
            else
                ConnectionString = GetConnectionString(yil);

            try
            {

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                
                SqlDataReader sdr = command.ExecuteReader(); 
                return sdr;
            }
            
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                connection.Close();
                return null;
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }
        public SqlDataReader Select_Command_Data_Reader_With_Parameters(string query, int yil, SqlParameter[] parameters)
        {

            ConnectionString = GetConnectionString(yil);
            try
            {

                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                
                SqlDataReader sdr = command.ExecuteReader(); 
                return sdr;
            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                Mouse.OverrideCursor = null;
                return null;
            }
        }

        public SqlDataReader Select_Command_Data_Reader(string query, int yil, string fabrika)
        {

            if (fabrika == "Ahşap")
               ConnectionString = GetConnectionStringAhsap(yil);
            
            else
                ConnectionString = GetConnectionString(yil);
            try
            {

                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader sdr = command.ExecuteReader(); // Ensure that the connection is closed when the reader is closed
                return sdr;
            }

            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
                return null;
            }

        }
        public SqlDataReader Select_Command_Data_Reader(string query, int yil)
        {

            //Create a SqlConnection to the database.
            ConnectionString = GetConnectionString(yil);
            try
            {

                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader sdr = command.ExecuteReader(); // Ensure that the connection is closed when the reader is closed
                return sdr;
            }

            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
                return null;
            }

        }
        public async Task<SqlDataReader> Select_Command_Data_ReaderAsync(string query, int yil)
        {

            ConnectionString = GetConnectionString(yil);
            try
            {

                SqlConnection connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader sdr = await command.ExecuteReaderAsync(); // Ensure that the connection is closed when the reader is closed
                return sdr;
            }

            catch
            {
                return null;
            }

        }
        public DataTable Select_Command(string query, int yil, string fabrika)
        { 
             if (fabrika == "Ahşap")
                ConnectionString = GetConnectionStringAhsap(yil);
           
            else
                ConnectionString = GetConnectionString(yil);
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);

                        }
                        return dataTable;
                    }

                }
            }

            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
                return null;
            }

        }
        public DataTable Select_Command(string query, int yil)
        {

            //Create a SqlConnection to the database.
            ConnectionString = GetConnectionString(yil);
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);

                        }
                        return dataTable;
                    }

                }
            }

            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
                return null;
            }

        }
        public DataTable Select_CommandLtd(string query, int yil)
        {
            string yilCorrection = yil.ToString();
            char secondLastChar = yilCorrection[yilCorrection.Length - 2];
            char lastChar = yilCorrection[yilCorrection.Length - 1];
            string yilShortedstr = new string(new[] { secondLastChar, lastChar });
            int yilShorted = Convert.ToInt32(yilShortedstr);
            //Create a SqlConnection to the database.
            ConnectionString = GetConnectionStringLTD(yilShorted);
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);

                        }
                        return dataTable;
                    }

                }
            }

            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
                return null;
            }

        }
        public bool ExecuteCommand(string query, int yil)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                connection = new SqlConnection(ConnectionString);
                
                connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.ExecuteNonQuery();

                    }
                connection.Close ();

                return true;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public bool ExecuteCommand(string query, int yil, string fabrika)
        {
            
            try
            {
                if (fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);

                else
                    ConnectionString = GetConnectionString(yil);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.ExecuteNonQuery();

                    }
                }
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public bool ExecuteCommandWithParameters(string query, int yil, SqlParameter[] parameters)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                connection = new SqlConnection(ConnectionString);
                
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        command.ExecuteNonQuery();

                    }
                connection.Close();

                return true;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public bool ExecuteCommandWithParameters(string query, int yil, SqlParameter[] parameters, string fabrika)
        {
            try
            {
                if (fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);
                else
                    ConnectionString = GetConnectionString(yil);

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.ExecuteNonQuery();
                }

                connection.Close();

                return true;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public bool ExecuteStoredProcedure(string procedure, int yil)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.ExecuteNonQuery();

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                return false;
            }
        }
        public async Task <bool> ExecuteStoredProcedureAsync(string procedure, int yil)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        await command.ExecuteNonQueryAsync();

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                return false;
            }
        }
        public async Task <bool> ExecuteStoredProcedureAsync(string procedure, int yil, string fabrika)
        {
            try
            {
                if (fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);

                else
                    ConnectionString = GetConnectionString(yil);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        await command.ExecuteNonQueryAsync();

                    }
                }

                return true;
            }
            catch
            {
                Mouse.OverrideCursor = null;
                return false;
            }
        }

        public bool ExecuteStoredProcedureWithParameters(string procedure, int yil, Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters, string fabrika)
        {
            try
            {
                if (fabrika == "Ahşap")
                   ConnectionString = GetConnectionStringAhsap(yil);
               
                else
                    ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters from the dictionary
                        foreach (var param in parameters)
                        {
                            if (param.Value.Type == SqlDbType.Decimal)
                            {
                                // For SqlDbType.Decimal, set Precision and Scale
                                SqlParameter decimalParam = new SqlParameter(param.Key, param.Value.Type)
                                {
                                    Precision = (byte)param.Value.Precision,
                                    Scale = (byte)param.Value.Scale,
                                    Value = param.Value.Value,
                                };
                                command.Parameters.Add(decimalParam);
                            }
                            else
                            {
                                // For other types, use Size
                                command.Parameters.Add(new SqlParameter(param.Key, param.Value.Type, param.Value.Precision)).Value = param.Value.Value;
                            }
                        }

                        command.ExecuteNonQuery();

                    }
                }

                return true;
            }
            catch 
            {
                Mouse.OverrideCursor = null;
                return false;
            }
        }
        public bool ExecuteStoredProcedureWithParameters(string procedure, int yil, Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters from the dictionary
                        foreach (var param in parameters)
                        {
                            if (param.Value.Type == SqlDbType.Decimal)
                            {
                                // For SqlDbType.Decimal, set Precision and Scale
                                SqlParameter decimalParam = new SqlParameter(param.Key, param.Value.Type)
                                {
                                    Precision = (byte)param.Value.Precision,
                                    Scale = (byte)param.Value.Scale,
                                    Value = param.Value.Value,
                                };
                                command.Parameters.Add(decimalParam);
                            }
                            else
                            {
                                // For other types, use Size
                                command.Parameters.Add(new SqlParameter(param.Key, param.Value.Type, param.Value.Precision)).Value = param.Value.Value;
                            }
                        }

                        command.ExecuteNonQuery();

                    }
                }

                return true;
            }
            catch 
            {
                Mouse.OverrideCursor = null;
                return false;
            }
        }

        public bool ExecuteStoredProcedureWithParameters(string procedure, int yil, SqlParameter[] parameters,string fabrika)
        {

            SqlConnection connection = null;
            try
            {

                if (fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);
                
                else
                    ConnectionString = GetConnectionString(yil);

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if(parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        command.ExecuteNonQuery();

                    }

                connection.Close();

                return true;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public bool ExecuteStoredProcedureWithParameters(string procedure, int yil, SqlParameter[] parameters)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if(parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        command.ExecuteNonQuery();

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                Mouse.OverrideCursor = null;
                return false;
            }
        }
        public async Task<bool> ExecuteStoredProcedureWithParametersAsync(string procedure, int yil, SqlParameter[] parameters)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            await connection.OpenAsync();
                        }
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public async Task<bool> ExecuteStoredProcedureWithParametersAsync(string procedure, int yil, SqlParameter[] parameters, string fabrika)
        {
            SqlConnection connection = null;
            try
            {
                if (fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);

                else
                    ConnectionString = GetConnectionString(yil);

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            await connection.OpenAsync();
                        }
                        await command.ExecuteNonQueryAsync();
                    }
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public async Task<bool> ExecuteStoredProcedureWithParametersAsync(string procedure, int yil, Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters)
        {
            try
            {
                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        foreach (var param in parameters)
                        {
                            if (param.Value.Type == SqlDbType.Decimal)
                            {
                                // For SqlDbType.Decimal, set Precision and Scale
                                SqlParameter decimalParam = new SqlParameter(param.Key, param.Value.Type)
                                {
                                    Precision = (byte)param.Value.Precision,
                                    Scale = (byte)param.Value.Scale,
                                    Value = param.Value.Value,
                                };
                                command.Parameters.Add(decimalParam);
                            }
                            else
                            {
                                // For other types, use Size
                                command.Parameters.Add(new SqlParameter(param.Key, param.Value.Type, param.Value.Precision)).Value = param.Value.Value;
                            }
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            await connection.OpenAsync();
                        }
                        await command.ExecuteNonQueryAsync();

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Insert_Stored_Proc_Param(string proc, string param1, string param1Val, string param2, string param2Val, string param3, string param3Val, string param4, string param4Val, string param5, string param5Val, string param6, string param6Val, int yil, string tableName, int adet)
        {

            try
            {

                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(proc, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Define and set the parameters
                        command.Parameters.Add(new SqlParameter(param1, SqlDbType.NVarChar, 35)).Value = param1Val;
                        command.Parameters.Add(new SqlParameter(param2, SqlDbType.NVarChar, 35)).Value = param2Val;
                        command.Parameters.Add(new SqlParameter(param3, SqlDbType.NVarChar, 100)).Value = param3Val;
                        command.Parameters.Add(new SqlParameter(param4, SqlDbType.NVarChar, 50)).Value = param4Val;
                        command.Parameters.Add(new SqlParameter(param5, SqlDbType.NVarChar, 100)).Value = param5Val;
                        command.Parameters.Add(new SqlParameter(param6, SqlDbType.NVarChar, 12)).Value = param6Val;

                        command.ExecuteNonQuery();

                        connection.Close();
                        CRUDmessages.InsertSuccessMessage(tableName, adet);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                Mouse.OverrideCursor = null;
                return false;
            }

        }

        public bool Insert_Stored_Proc_Param_6(string proc, string param1, string param1Val, string param2, string param2Val, string param3, string param3Val, string param4, string param4Val, string param5, string param5Val, string param6, string param6Val, int yil, string tableName, int adet)
        {

            try
            {

                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(proc, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Define and set the parameters
                        command.Parameters.Add(new SqlParameter(param1, SqlDbType.NVarChar, 35)).Value = param1Val;
                        command.Parameters.Add(new SqlParameter(param2, SqlDbType.NVarChar, 35)).Value = param2Val;
                        command.Parameters.Add(new SqlParameter(param3, SqlDbType.NVarChar, 100)).Value = param3Val;
                        command.Parameters.Add(new SqlParameter(param4, SqlDbType.NVarChar, 50)).Value = param4Val;
                        command.Parameters.Add(new SqlParameter(param5, SqlDbType.NVarChar, 100)).Value = param5Val;
                        command.Parameters.Add(new SqlParameter(param6, SqlDbType.NVarChar, 12)).Value = param6Val;

                        command.ExecuteNonQuery();
                        
                        connection.Close();
                        CRUDmessages.InsertSuccessMessage(tableName, adet);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                Mouse.OverrideCursor = null;
                return false;
            }

        }

        public int Get_One_Int_Result_Command(string query, int yil, string fabrika)
        {
            try
            {
                if(fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);
                else
                    ConnectionString = GetConnectionString(yil);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            catch { return -1; }

        }
        public int Get_One_Int_Result_Command_With_Parameters(string query, int yil,SqlParameter[] parameters, string fabrika)
        {
            try
            {
                if(fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);
                else
                    ConnectionString = GetConnectionString(yil);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddRange(parameters);

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            catch { return -1; }

        }
        public string Get_One_String_Result_Command(string procedure, int yil)
        {
            try
            {
                    ConnectionString = GetConnectionString(yil);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.Text;

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch { return "STRING HATA"; }

        }
        public string Get_One_String_Result_Command_With_Parameters(string procedure, int yil, SqlParameter[] parameters,string fabrika)
        {
            try
            {
                if(fabrika == "Ahşap")
                    ConnectionString = GetConnectionStringAhsap(yil);
                else
                    ConnectionString = GetConnectionString(yil);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddRange(parameters);

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch { return "STRING HATA"; }

        }
        public string Get_One_String_Result_Command_With_Parameters(string procedure, int yil, SqlParameter[] parameters)
        {
            try
            {

                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddRange(parameters);
                        
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch { return "STRING HATA"; }

        }
        
        public string Get_One_String_Result_Stored_Proc_With_Parameters(string procedure, int yil, SqlParameter[] parameters)
        {
            try
            {

                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddRange(parameters);
                        
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch { return "STRING HATA"; }

        }
        public int Get_One_Int_Result_Stored_Proc_With_Parameters(string procedure, int yil, Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters, string fabrika)
        {
            try
            {
                if (fabrika == "Ahşap")
                   ConnectionString = GetConnectionStringAhsap(yil);
                else
                    ConnectionString = GetConnectionString(yil);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        foreach (var param in parameters)
                        {
                            if (param.Value.Type == SqlDbType.Decimal)
                            {
                                // For SqlDbType.Decimal, set Precision and Scale
                                SqlParameter decimalParam = new SqlParameter(param.Key, param.Value.Type)
                                {
                                    Precision = (byte)param.Value.Precision,
                                    Scale = (byte)param.Value.Scale,
                                    Value = param.Value.Value,
                                };
                                command.Parameters.Add(decimalParam);
                            }
                            else
                            {
                                // For other types, use Size
                                command.Parameters.Add(new SqlParameter(param.Key, param.Value.Type, param.Value.Precision)).Value = param.Value.Value;
                            }
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return Convert.ToInt32(result); 
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            catch { return -1; }

        }
        public int Get_One_Int_Result_Stored_Proc_With_Parameters(string procedure, int yil, Dictionary<string, (SqlDbType Type, int Precision, int Scale, object Value)> parameters)
        {
            try
            {

                ConnectionString = GetConnectionString(yil);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        foreach (var param in parameters)
                        {
                            if (param.Value.Type == SqlDbType.Decimal)
                            {
                                // For SqlDbType.Decimal, set Precision and Scale
                                SqlParameter decimalParam = new SqlParameter(param.Key, param.Value.Type)
                                {
                                    Precision = (byte)param.Value.Precision,
                                    Scale = (byte)param.Value.Scale,
                                    Value = param.Value.Value,
                                };
                                command.Parameters.Add(decimalParam);
                            }
                            else
                            {
                                // For other types, use Size
                                command.Parameters.Add(new SqlParameter(param.Key, param.Value.Type, param.Value.Precision)).Value = param.Value.Value;
                            }
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        object result = command.ExecuteScalar();

                        connection.Close();

                        if (result != null)
                        {
                            return Convert.ToInt32(result); 
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            catch { return -1; }

        }

        public string Get_One_String_Result_Stored_Proc(string procedure, int yil)
		{
			try
			{

				ConnectionString = GetConnectionString(yil);
				using (SqlConnection connection = new SqlConnection(ConnectionString))
				{
					using (SqlCommand command = new SqlCommand(procedure, connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						
						if (connection.State == ConnectionState.Closed)
						{
							connection.Open();
						}

						object result = command.ExecuteScalar();

						connection.Close();

						if (result != null)
						{
							return result.ToString();
						}
						else
						{
							return string.Empty;
						}
					}
				}
			}
			catch { return "STRING HATA"; }

		}

        public DataTable Stored_Proc_With_Parameters_Returns_Table(string procedureName, int yil,SqlParameter[] parameters)
        {
            
                try
                {
                    string connectionString = GetConnectionString(yil); 

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(procedureName, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            if(parameters != null)
                                command.Parameters.AddRange(parameters);

                           DataTable dataTable = new DataTable();

                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                Mouse.OverrideCursor = null;
                return null;
            }
            
        }
        public DataTable Stored_Proc_Parameterless_Returns_Table(string procedureName, int yil)
        {
            
                try
                {
                    string connectionString = GetConnectionString(yil); 

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(procedureName, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            DataTable dataTable = new DataTable();

                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                Mouse.OverrideCursor = null;
                return null;
            }
            
        }
        public DataTable Select_Stored_Proc_LTD(string proc,string param1,string param2,int param1Val,string param2Val, int yil)
        {
            
                try
                {
                    string yilCorrection = yil.ToString();
                    char secondLastChar = yilCorrection[yilCorrection.Length - 2];
                    char lastChar = yilCorrection[yilCorrection.Length - 1];
                    string yilShortedstr = new string(new[] { secondLastChar, lastChar });
                    int yilShorted = Convert.ToInt32(yilShortedstr);
                    string connectionString = GetConnectionStringLTD(yilShorted); 
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(proc, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Define and set the parameters
                            command.Parameters.Add(new SqlParameter(param1, SqlDbType.Int)).Value = param1Val; 
                            command.Parameters.Add(new SqlParameter(param2, SqlDbType.NVarChar, -1)).Value = param2Val; 

                            // Create a DataTable to hold the results
                            DataTable dataTable = new DataTable();

                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                Mouse.OverrideCursor = null;
                return null;
            }
            
        }
        public void Insert_Statement(string query, int yil, string tableName, int adet)
        {
            try
            { 

                using (SqlConnection connection = new SqlConnection(GetConnectionString(yil)))
                {
                    connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();

                    connection.Close();
                    CRUDmessages.InsertSuccessMessage(tableName, adet);
                }

            }
                catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null; 
            }
        }
        public void Update_Statement(string query, int yil, string tableName, int adet)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(GetConnectionString(yil)))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                    CRUDmessages.UpdateSuccessMessage(tableName, adet);
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
            }
        }
        public void Update_Statement(string query, int yil)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(GetConnectionString(yil)))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
            }
        }
        public void Delete_Statement(string query, int yil, string tableName, int adet)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(GetConnectionString(yil)))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                    CRUDmessages.DeleteSuccessMessage(tableName, adet);
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the task
                System.Windows.MessageBox.Show(ex.Message);
                // loading durumunda ise kaldır
                Mouse.OverrideCursor = null;
            }
        }

        private void HandleSqlException(SqlException ex)
        {
            foreach (SqlError error in ex.Errors)
            {
                if (error.Number == -2)  // SQL Server timeout error
                {
                    System.Windows.MessageBox.Show("Sorgu Zaman Aşımına Uğradı.");

                    return;
                }
            }
        }

        static private string GetConnectionStringLTD(int yil)
        {
            // To avoid storing the connection string in your code,
            // you can retrieve it from a configuration file.
            return "Data Source=192.168.1.11;Initial Catalog=VITALTD" + yil + ";Persist Security Info=True;User ID=sa;Password=sapass;TrustServerCertificate=true;";
        }
        static private string GetConnectionStringAhsap(int yil)
        {
            yil = yil % 100;
            return "Data Source=192.168.1.11;Initial Catalog=VITAHSAPLTD" + yil + ";Persist Security Info=True;User ID=sa;Password=sapass;TrustServerCertificate=true;";
        }
        static private string GetConnectionString(int yil)
        {
            // To avoid storing the connection string in your code,
            // you can retrieve it from a configuration file.
            return "Data Source=192.168.1.11;Initial Catalog=VITA" + yil + ";Persist Security Info=True;User ID=sa;Password=sapass;TrustServerCertificate=true;";
        }
    }
}
