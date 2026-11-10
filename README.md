# HỆ THỐNG QUẢN LÝ TỔNG HỢP - WINFORMS 3-TIER ARCHITECTURE

## MỤC 1: THÔNG TIN SINH VIÊN

- **Họ và tên sinh viên:** Nguyễn Tiến Nhật Minh
- **Mã sinh viên:** 24810320038
- **Tên Đề tài / Bài tập:** Bài tập 6 - Tích hợp 3 ứng dụng WinForms (Quản lý Dự án / Task, Quản lý Garage Ô tô và Đặt phòng Karaoke) trên cùng một mô hình kiến trúc 3 lớp (3-Tier Architecture: GUI - BUS - DAL - DTO).

---

## MỤC 2: HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN

1. **Yêu cầu hệ thống:**
   - Visual Studio / VS Code với .NET SDK.
   - SQL Server.

2. **Cách khởi chạy:**
   - Mở Terminal trong VS Code và chuyển vào thư mục `src`:
     ```powershell
     cd D:\baitapngay1011\src
     ```
   - Chạy lệnh khởi tạo và thực thi ứng dụng (CSDL được khởi tạo tự động):
     ```powershell
     dotnet run
     ```

---

## MỤC 3: HÌNH ẢNH KẾT QUẢ CÁC MÀN HÌNH CHỨC NĂNG

### 1. Màn hình chính / Tổng quan hệ thống

![Màn hình chính](images/01_main_screen.png)

### 2. Màn hình Quản lý Danh mục (CRUD)

![Màn hình CRUD](images/02_crud_screen.png)

### 3. Màn hình Nghiệp vụ chính (Đặt phòng Karaoke)

![Màn hình Nghiệp vụ](images/03_transaction_screen.png)

### 4. Màn hình Báo cáo - Thống kê (Tiến độ & Cảnh báo trễ hạn)

![Màn hình Báo cáo](images/04_report_screen.png)

### 5. Minh họa An toàn Dữ liệu & Xử lý Lỗi (Exception Handling)

![Xử lý lỗi](images/05_error_handling.png)
