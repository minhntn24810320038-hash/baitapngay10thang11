using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyTongHop.DAL
{
    public class TaskDAL
    {
        public DataTable GetAllDuAn()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DuAn", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetTaskByDuAn(int maDuAn, string trangThai)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM CongViec WHERE MaDuAn = @MaDuAn";
                if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả")
                    query += " AND TrangThai = @TrangThai";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDuAn", maDuAn);
                if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả")
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool CapNhatTrangThaiTask(int maTask, string trangThaiMoi)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE CongViec SET TrangThai = @TrangThai WHERE MaTask = @MaTask";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThaiMoi);
                cmd.Parameters.AddWithValue("@MaTask", maTask);
                if (conn.State != ConnectionState.Open) conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Yêu cầu đề bài: Dùng SqlDataReader để duyệt công việc trễ hạn (Overdue)
        public DataTable GetTaskOverdue()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaTask", typeof(int));
            dt.Columns.Add("TenTask", typeof(string));
            dt.Columns.Add("HanNop", typeof(DateTime));
            dt.Columns.Add("TrangThai", typeof(string));

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT MaTask, TenTask, HanNop, TrangThai FROM CongViec WHERE HanNop < GETDATE() AND TrangThai != N'Done'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State != ConnectionState.Open) conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["MaTask"],
                            reader["TenTask"],
                            reader["HanNop"],
                            reader["TrangThai"]
                        );
                    }
                }
            }
            return dt;
        }

        // Yêu cầu đề bài: Dùng ExecuteScalar tính tỷ lệ % hoàn thành
        public double TinhTyLeHoanThanh(int maDuAn)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                string sqlTong = "SELECT COUNT(*) FROM CongViec WHERE MaDuAn = @MaDuAn";
                SqlCommand cmdTong = new SqlCommand(sqlTong, conn);
                cmdTong.Parameters.AddWithValue("@MaDuAn", maDuAn);
                int tongTask = Convert.ToInt32(cmdTong.ExecuteScalar());

                if (tongTask == 0) return 0;

                string sqlDone = "SELECT COUNT(*) FROM CongViec WHERE MaDuAn = @MaDuAn AND TrangThai = N'Done'";
                SqlCommand cmdDone = new SqlCommand(sqlDone, conn);
                cmdDone.Parameters.AddWithValue("@MaDuAn", maDuAn);
                int doneTask = Convert.ToInt32(cmdDone.ExecuteScalar());

                return (double)doneTask / tongTask * 100;
            }
        }
    }
}