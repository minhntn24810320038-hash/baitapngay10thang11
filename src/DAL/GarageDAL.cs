using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyTongHop.DAL
{
    public class GarageDAL
    {
        // Tra cứu bằng SqlParameter
        public DataTable TraCuuXe(string bienSo)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT x.BienSo, x.HieuXe, x.NamSanXuat, k.TenKH, k.DienThoai 
                               FROM Xe x JOIN KhachHangGarage k ON x.MaKH = k.MaKH 
                               WHERE x.BienSo = @BienSo";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@BienSo", bienSo ?? "");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetPhuTung()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM PhuTung", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Lập phiếu sửa chữa kèm kiểm tra tồn kho SqlTransaction
        public bool LapPhieuSuaChua(string bienSo, string noiDung, DataTable dsPhuTung, out string message)
        {
            message = "";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Tạo phiếu sửa chữa
                        string sqlPhieu = @"INSERT INTO PhieuSuaChua (BienSo, NoiDungSua, TongTien, TrangThai) 
                                           OUTPUT INSERTED.MaPhieu 
                                           VALUES (@BienSo, @NoiDung, 0, N'Đang sửa')";
                        SqlCommand cmdPhieu = new SqlCommand(sqlPhieu, conn, trans);
                        cmdPhieu.Parameters.AddWithValue("@BienSo", bienSo);
                        cmdPhieu.Parameters.AddWithValue("@NoiDung", noiDung);
                        int maPhieu = (int)cmdPhieu.ExecuteScalar();

                        decimal tongTien = 0;

                        // 2. Thêm chi tiết sửa chữa & Kiểm tra kho
                        foreach (DataRow r in dsPhuTung.Rows)
                        {
                            int maPT = Convert.ToInt32(r["MaPhuTung"]);
                            int soLuong = Convert.ToInt32(r["SoLuong"]);
                            decimal tienCong = Convert.ToDecimal(r["TienCong"]);
                            decimal donGia = Convert.ToDecimal(r["DonGia"]);

                            // Kiểm tra tồn kho
                            string sqlCheckKho = "SELECT SoLuongTon FROM PhuTung WHERE MaPhuTung = @MaPT";
                            SqlCommand cmdCheck = new SqlCommand(sqlCheckKho, conn, trans);
                            cmdCheck.Parameters.AddWithValue("@MaPT", maPT);
                            int tonKho = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (tonKho < soLuong)
                            {
                                trans.Rollback();
                                message = $"Phụ tùng ID {maPT} không đủ tồn kho (Còn: {tonKho}, Cần: {soLuong})!";
                                return false;
                            }

                            // Trừ tồn kho
                            string sqlTruKho = "UPDATE PhuTung SET SoLuongTon = SoLuongTon - @SL WHERE MaPhuTung = @MaPT";
                            SqlCommand cmdTru = new SqlCommand(sqlTruKho, conn, trans);
                            cmdTru.Parameters.AddWithValue("@SL", soLuong);
                            cmdTru.Parameters.AddWithValue("@MaPT", maPT);
                            cmdTru.ExecuteNonQuery();

                            // Lưu ChiTietSuaChua
                            string sqlCT = "INSERT INTO ChiTietSuaChua VALUES (@MaPhieu, @MaPT, @SL, @TienCong)";
                            SqlCommand cmdCT = new SqlCommand(sqlCT, conn, trans);
                            cmdCT.Parameters.AddWithValue("@MaPhieu", maPhieu);
                            cmdCT.Parameters.AddWithValue("@MaPT", maPT);
                            cmdCT.Parameters.AddWithValue("@SL", soLuong);
                            cmdCT.Parameters.AddWithValue("@TienCong", tienCong);
                            cmdCT.ExecuteNonQuery();

                            tongTien += (soLuong * donGia) + tienCong;
                        }

                        // Cập nhật tổng tiền
                        string sqlUpdateTien = "UPDATE PhieuSuaChua SET TongTien = @TongTien WHERE MaPhieu = @MaPhieu";
                        SqlCommand cmdTien = new SqlCommand(sqlUpdateTien, conn, trans);
                        cmdTien.Parameters.AddWithValue("@TongTien", tongTien);
                        cmdTien.Parameters.AddWithValue("@MaPhieu", maPhieu);
                        cmdTien.ExecuteNonQuery();

                        trans.Commit();
                        message = "Lập phiếu sửa chữa thành công!";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        message = "Lỗi hệ thống: " + ex.Message;
                        return false;
                    }
                }
            }
        }
    }
}