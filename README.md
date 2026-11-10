# HỆ THỐNG QUẢN LÝ TỔNG HỢP - WINFORMS 3-TIER

## MỤC 1: THÔNG TIN SINH VIÊN

- **Họ và tên sinh viên**: Nguyễn Tiến Nhật Minh
- **Mã sinh viên**: 24810320038
- **Lớp / Khóa**: Lập trình Windows Forms / C#

---

## MỤC 2: CẤU TRÚC THƯ MỤC SOLUTION (3-TIER ARCHITECTURE)

baitapngay10thang11/
├── Database/
│ └── ScriptDB.sql # Script khởi tạo CSDL SQL Server và chèn dữ liệu mẫu
├── src/ # Mã nguồn dự án (ProjectName)
│ ├── DTO/ # Chứa các lớp Data Transfer Object
│ ├── DAL/ # Chứa các lớp Data Access Layer (Truy vấn ADO.NET)
│ ├── BUS/ # Chứa các lớp Business Logic Layer (Xử lý nghiệp vụ)
│ ├── Forms/ # GUI - Chứa Form giao diện WinForms (Presentation Layer)
│ └── Program.cs # Điểm khởi chạy ứng dụng
├── images/ # Thư mục lưu trữ hình ảnh báo cáo
│ ├── 01_main_screen.png
│ ├── 02_crud_screen.png
│ ├── 03_transaction_screen.png
│ ├── 04_report_screen.png
│ └── 05_error_handling.png
├── README.md # Báo cáo chi tiết bài tập
└── QuanLyTongHop.sln # Solution File

---

## MỤC 3: HÌNH ẢNH KẾT QUẢ CÁC MÀN HÌNH CHỨC NĂNG

1. **Màn hình Chính / Tổng quan hệ thống**:
   ![Màn hình chính](images/01_main_screen.png)

2. **Màn hình Quản lý Danh mục (CRUD - Thêm/Sửa/Xóa)**:
   ![Màn hình CRUD](images/02_crud_screen.png)

3. **Màn hình Thực thi Nghiệp vụ chính (Lập Hóa đơn / Bán hàng / Chấm công / Đặt phòng)**:
   ![Màn hình Nghiệp vụ](images/03_transaction_screen.png)

4. **Màn hình Báo cáo - Thống kê**:
   ![Màn hình Thống kê](images/04_report_screen.png)

5. **Minh họa An toàn Dữ liệu (Phòng chống SQL Injection / Báo lỗi Transaction Rollback)**:
   ![Minh họa Xử lý lỗi](images/05_error_handling.png)
