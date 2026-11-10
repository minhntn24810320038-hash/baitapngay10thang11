using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTongHop.BUS;

namespace QuanLyTongHop.Forms
{
    public class FormKaraoke : Form
    {
        private KaraokeBUS bus = new KaraokeBUS();
        private FlowLayoutPanel panelPhong;
        private DataGridView dgvDichVu;
        private int maPhongDangChon = 0;
        private Label lblSelected;

        public FormKaraoke()
        {
            InitializeComponent();
            LoadPhongGrid();
            dgvDichVu.DataSource = bus.GetDanhSachDichVu();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Karaoke";
            this.Size = new Size(950, 650);

            panelPhong = new FlowLayoutPanel { Location = new Point(20, 20), Size = new Size(450, 520), AutoScroll = true };

            lblSelected = new Label { Text = "Đang chọn: Chưa chọn phòng", Location = new Point(490, 20), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            Button btnMo = new Button { Text = "Mở phòng", Location = new Point(490, 50), Width = 120, Height = 40, BackColor = Color.Blue, ForeColor = Color.White };
            btnMo.Click += (s, e) => {
                if (maPhongDangChon > 0 && bus.MoPhong(maPhongDangChon)) LoadPhongGrid();
            };

            Button btnPay = new Button { Text = "Thanh toán (Transaction)", Location = new Point(620, 50), Width = 200, Height = 40, BackColor = Color.Red, ForeColor = Color.White };
            btnPay.Click += (s, e) => {
                if (maPhongDangChon > 0 && bus.ThanhToanKaraoke(maPhongDangChon, out decimal tongTien))
                {
                    MessageBox.Show($"Thanh toán thành công! Tổng tiền: {tongTien:N0} VNĐ");
                    LoadPhongGrid();
                }
            };

            dgvDichVu = new DataGridView { Location = new Point(490, 110), Size = new Size(430, 430), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            this.Controls.AddRange(new Control[] { panelPhong, lblSelected, btnMo, btnPay, dgvDichVu });
        }

        private void LoadPhongGrid()
        {
            panelPhong.Controls.Clear();
            DataTable dt = bus.GetDanhSachPhong();

            foreach (DataRow r in dt.Rows)
            {
                Button btn = new Button
                {
                    Width = 120,
                    Height = 90,
                    Text = $"{r["TenPhong"]}\n({r["TrangThai"]})",
                    Tag = r["MaPhong"],
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                string tt = r["TrangThai"].ToString();
                btn.BackColor = tt == "Trống" ? Color.LightGreen : Color.Salmon;

                btn.Click += (s, e) => {
                    maPhongDangChon = Convert.ToInt32(((Button)s).Tag);
                    lblSelected.Text = "Đang chọn: " + ((Button)s).Text.Replace("\n", " ");
                };

                panelPhong.Controls.Add(btn);
            }
        }
    }
}