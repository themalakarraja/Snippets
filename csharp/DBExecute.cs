using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Dal
{
    public class DBExecute
    {
        private readonly string _connectionString;

        public DBExecute(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public DBExecute()
        //{
        //    ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["INVENTORY_DB"];
        //    if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
        //        throw new Exception("Fatal error: missing connecting string in web.config file");
        //    _connectionString = mySetting.ConnectionString;
        //}

        public int execute(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connectionObj = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connectionObj))
                {
                    connectionObj.Open();
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in parameters)
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    int affectedRows = cmd.ExecuteNonQuery();
                    connectionObj.Close();
                    return affectedRows;
                }
            }
        }

        public int executetWithProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            using (SqlConnection connectionObj = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connectionObj))
                {
                    connectionObj.Open();
                    foreach (KeyValuePair<string, object> parameter in parameters)
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int affectedRows = cmd.ExecuteNonQuery();
                    connectionObj.Close();
                    return affectedRows;
                }
            }
        }

        public string executeScalar(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connectionObj = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connectionObj))
                {
                    connectionObj.Open();
                    if(parameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in parameters)
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    string result = cmd.ExecuteScalar() + "";
                    connectionObj.Close();
                    return result;
                }
            }
        }

        public string executeScalarWithProcedure(string procedureName, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connectionObj = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connectionObj))
                {
                    connectionObj.Open();
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in parameters)
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    string result = cmd.ExecuteScalar() + "";
                    connectionObj.Close();
                    return result;
                }
            }
        }

        public DataSet executeSelect(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connectionObj = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connectionObj))
                {
                    connectionObj.Open();
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in parameters)
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet resultDs = new DataSet();
                    da.Fill(resultDs);
                    connectionObj.Close();
                    return resultDs;
                }
            }
        }

        public DataSet executeSelectWithProcedure(string procedureName, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connectionObj = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connectionObj))
                {
                    connectionObj.Open();
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in parameters)
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet resultDs = new DataSet();
                    da.Fill(resultDs);
                    connectionObj.Close();
                    return resultDs;
                }
            }
        }

        public void BulkInsert(DataTable dt)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "tbl_shippingtrack";

            objbulk.ColumnMappings.Add("RID", "RID");
            objbulk.ColumnMappings.Add("TrackingNo", "TrackingNo");
            objbulk.ColumnMappings.Add("isRead", "isRead");
            objbulk.ColumnMappings.Add("isMailSent", "isMailSent");
            objbulk.ColumnMappings.Add("IsActive", "IsActive");
            objbulk.ColumnMappings.Add("ShipVia", "ShipVia");
            objbulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            objbulk.ColumnMappings.Add("ModifiedDate", "ModifiedDate");

            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
        }
    }
}
