using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTongHop.BUS;

namespace QuanLyTongHop.Forms
{
    public class FormGarage : Form
    {
        private GarageBUS bus = new GarageBUS();
        private TextBox txtBienSo;
        private TextBox txtNoiDung;
        private DataGridView dgvXe;
        private DataGridView dgvPhuTung;
        private Button btnLapPhieu;

        public FormGarage()
        {
            InitializeComponent();
            LoadPhuTung();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Garage Ô tô";
            this.Size = new Size(950, 650);

            Label lblBS = new Label { Text = "Biển số xe:", Location = new Point(20, 20), AutoSize = true };
            txtBienSo = new TextBox { Location = new Point(100, 16), Width = 150 };
            Button btnTim = new Button { Text = "Tra cứu (SqlParameter)", Location = new Point(260, 15), Width = 160 };
            btnTim.Click += (s, e) => dgvXe.DataSource = bus.TraCuuXe(txtBienSo.Text.Trim());

            dgvXe = new DataGridView { Location = new Point(20, 50), Size = new Size(900, 120), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Label lblND = new Label { Text = "Nội dung sửa:", Location = new Point(20, 190), AutoSize = true };
            txtNoiDung = new TextBox { Location = new Point(120, 186), Width = 400 };

            dgvPhuTung = new DataGridView { Location = new Point(20, 230), Size = new Size(900, 260), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            btnLapPhieu = new Button { Text = "Lập Phiếu & Trừ Kho (SqlTransaction)", Location = new Point(20, 510), Size = new Size(300, 45), BackColor = Color.DarkGreen, ForeColor = Color.White };
            btnLapPhieu.Click += BtnLapPhieu_Click;

            this.Controls.AddRange(new Control[] { lblBS, txtBienSo, btnTim, dgvXe, lblND, txtNoiDung, dgvPhuTung, btnLapPhieu });
        }

        private void LoadPhuTung()
        {
            dgvPhuTung.DataSource = bus.GetPhuTung();
        }

        private void BtnLapPhieu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBienSo.Text))
            {
                MessageBox.Show("Vui lòng nhập biển số!");
                return;
            }

            DataTable dtSelected = bus.GetPhuTung();
            dtSelected.Columns.Add("SoLuong", typeof(int));
            dtSelected.Columns.Add("TienCong", typeof(decimal));

            // Lấy dòng đang chọn trên lưới
            if (dgvPhuTung.CurrentRow != null)
            {
                DataRow r = dtSelected.NewRow();
                r["MaPhuTung"] = dgvPhuTung.CurrentRow.Cells["MaPhuTung"].Value;
                r["DonGia"] = dgvPhuTung.CurrentRow.Cells["DonGia"].Value;
                r["SoLuong"] = 1;
                r["TienCong"] = 50000;
                dtSelected.Rows.Add(r);
            }

            if (bus.LapPhieuSuaChua(txtBienSo.Text.Trim(), txtNoiDung.Text, dtSelected, out string msg))
            {
                MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPhuTung();
            }
            else
            {
                MessageBox.Show(msg, "Cảnh báo Rollback", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}