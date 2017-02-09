using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class MSSqlHelper
    {
        private static string connStr = ConfigHelper.strConnection;

        #region 执行sql查询操作,返回DataTable
        public static DataTable ExecuteDateTable(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }
                adapter.Fill(dt);
            }
            return dt;
        }
        #endregion

        #region 执行sql查询操作,返回DataTable
        public static DataSet ExecuteDataSet(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    if (parameters != null)
                    {
                        adapter.SelectCommand.Parameters.AddRange(parameters);
                    }
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);  //DataTable dt = dataSet.Tables[0]; 
                    return dataSet;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 执行sql非查询操作(参数可无)
        public static int ExecuteNoQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region 执行sql，返回第一行的第一列
        public static object Executescalar(string sql, params  SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region 执行sql，返回DataReader
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            try
            {
                conn.Open();
                //关闭SqlDataReader 会自动关闭Sqlconnection  
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return sdr;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

    }
}
