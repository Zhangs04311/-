using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BookManagementSystem.Utils
{
    public static class DBUtil
    {

        private static SqlConnection con;
        public static string ConnectionString {get; set;}

        public static SqlConnection GetConnection()
        {
            //appsetting.jsonからうまくConnectionStringが取得できなかった場合
            if (string.IsNullOrEmpty(ConnectionString))
            {
                //エラーとする
                throw new Exception("DB設定されていません");
            }
            con = new SqlConnection()
            { 
                ConnectionString = ConnectionString
            };
            con.Open();

            return con;
        }

        public static void CloseConnection(SqlConnection sqlCon)
        {
            if (sqlCon != null)
            {
                sqlCon.Close();
            }
        }
    }
}