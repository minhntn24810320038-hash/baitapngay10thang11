using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace QuanLyTongHop.DAL
{
    public static class DatabaseHelper
    {
        private static string activeConnectionString = "";

        public static SqlConnection GetConnection()
        {
            if (!string.IsNullOrEmpty(activeConnectionString))
            {
                SqlConnection conn = new SqlConnection(activeConnectionString);
                conn.Open();
                return conn;
            }

            string machineName = Environment.MachineName;

            // Danh sách các tên Server SQL phổ biến trên máy tính
            string[] serverInstances = new string[]
            {
                @".\SQLEXPRESS",
                @".",
                $@"{machineName}\SQLEXPRESS",
                machineName,
                @"(localdb)\MSSQLLOCALDB",
                @"localhost"
            };

            string masterConnStr = "";
            foreach (string server in serverInstances)
            {
                try
                {
                    string testConnStr = $@"Data Source={server};Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;Connection Timeout=2";
                    using (SqlConnection conn = new SqlConnection(testConnStr))
                    {
                        conn.Open();
                        masterConnStr = testConnStr;
                        activeConnectionString = $@"Data Source={server};Initial Catalog=QuanLyTongHopDB;Integrated Security=True;TrustServerCertificate=True";
                        break;
                    }
                }
                catch { }
            }

            if (string.IsNullOrEmpty(masterConnStr))
            {
                throw new Exception("Không tìm thấy SQL Server nào đang hoạt động trên máy! Hãy kiểm tra lại dịch vụ SQL Server trong Services.msc.");
            }

            // Tự động kiểm tra và tạo CSDL QuanLyTongHopDB nếu chưa có
            AutoCreateDatabase(masterConnStr);

            SqlConnection finalConn = new SqlConnection(activeConnectionString);
            finalConn.Open();
            return finalConn;
        }

        private static void AutoCreateDatabase(string masterConnStr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(masterConnStr))
                {
                    conn.Open();
                    string checkDbSql = "SELECT COUNT(*) FROM sys.databases WHERE name = 'QuanLyTongHopDB'";
                    SqlCommand cmdCheck = new SqlCommand(checkDbSql, conn);
                    int dbCount = (int)cmdCheck.ExecuteScalar();

                    if (dbCount == 0)
                    {
                        // Tìm file ScriptDB.sql ở thư mục gốc hoặc thư mục Database
                        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Database", "ScriptDB.sql");
                        if (!File.Exists(scriptPath))
                        {
                            scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Database", "ScriptDB.sql");
                        }

                        if (File.Exists(scriptPath))
                        {
                            string scriptContent = File.ReadAllText(scriptPath);
                            string[] commands = Regex.Split(scriptContent, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                            foreach (string cmdText in commands)
                            {
                                if (!string.IsNullOrWhiteSpace(cmdText))
                                {
                                    using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}