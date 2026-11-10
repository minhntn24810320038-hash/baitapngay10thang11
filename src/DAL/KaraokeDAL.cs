using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyTongHop.DAL
{
    public class KaraokeDAL
    {
        public DataTable GetDanhSachPhong()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM PhongHat", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetDanhSachDichVu()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DichVu", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool MoPhong(int maPhong)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlHD = "INSERT INTO HoaDonKaraoke (MaPhong, GioVao, TrangThai) VALUES (@MaPhong, GETDATE(), 0)";
                        SqlCommand cmdHD = new SqlCommand(sqlHD, conn, trans);
                        cmdHD.Parameters.AddWithValue("@MaPhong", maPhong);
                        cmdHD.ExecuteNonQuery();

                        string sqlPhong = "UPDATE PhongHat SET TrangThai = N'Đang hát' WHERE MaPhong = @MaPhong";
                        SqlCommand cmdPhong = new SqlCommand(sqlPhong, conn, trans);
                        cmdPhong.Parameters.AddWithValue("@MaPhong", maPhong);
                        cmdPhong.ExecuteNonQuery();

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool ThanhToanKaraoke(int maPhong, out decimal tongThanhToan)
        {
            tongThanhToan = 0;
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlGetHD = "SELECT TOP 1 MaHD, GioVao FROM HoaDonKaraoke WHERE MaPhong = @MaPhong AND TrangThai = 0 ORDER BY MaHD DESC";
                        SqlCommand cmdGet = new SqlCommand(sqlGetHD, conn, trans);
                        cmdGet.Parameters.AddWithValue("@MaPhong", maPhong);
                        
                        int maHD = 0;
                        DateTime gioVao = DateTime.Now;
                        using (SqlDataReader r = cmdGet.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                maHD = Convert.ToInt32(r["MaHD"]);
                                gioVao = Convert.ToDateTime(r["GioVao"]);
                            }
                        }

                        if (maHD == 0) { trans.Rollback(); return false; }

                        // Lấy giá giờ
                        string sqlGia = "SELECT GiaTheoGio FROM PhongHat WHERE MaPhong = @MaPhong";
                        SqlCommand cmdGia = new SqlCommand(sqlGia, conn, trans);
                        cmdGia.Parameters.AddWithValue("@MaPhong", maPhong);
                        decimal giaGio = Convert.ToDecimal(cmdGia.ExecuteScalar());

                        double soGio = (DateTime.Now - gioVao).TotalHours;
                        if (soGio < 0.1) soGio = 0.1;
                        decimal tienPhong = (decimal)soGio * giaGio;

                        // Tính tiền dịch vụ
                        string sqlDV = "SELECT ISNULL(SUM(SoLuong * DonGia), 0) FROM ChiTietDichVu WHERE MaHD = @MaHD";
                        SqlCommand cmdDV = new SqlCommand(sqlDV, conn, trans);
                        cmdDV.Parameters.AddWithValue("@MaHD", maHD);
                        decimal tienDV = Convert.ToDecimal(cmdDV.ExecuteScalar());

                        tongThanhToan = tienPhong + tienDV;

                        // Cập nhật Hóa đơn
                        string sqlUpdateHD = @"UPDATE HoaDonKaraoke 
                                               SET GioRa = GETDATE(), TongTienPhong = @TP, TongTienDichVu = @TDV, TrangThai = 1 
                                               WHERE MaHD = @MaHD";
                        SqlCommand cmdUpHD = new SqlCommand(sqlUpdateHD, conn, trans);
                        cmdUpHD.Parameters.AddWithValue("@TP", tienPhong);
                        cmdUpHD.Parameters.AddWithValue("@TDV", tienDV);
                        cmdUpHD.Parameters.AddWithValue("@MaHD", maHD);
                        cmdUpHD.ExecuteNonQuery();

                        // Trả trạng thái phòng
                        string sqlFree = "UPDATE PhongHat SET TrangThai = N'Trống' WHERE MaPhong = @MaPhong";
                        SqlCommand cmdFree = new SqlCommand(sqlFree, conn, trans);
                        cmdFree.Parameters.AddWithValue("@MaPhong", maPhong);
                        cmdFree.ExecuteNonQuery();

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}