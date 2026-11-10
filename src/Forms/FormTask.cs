using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTongHop.BUS;

namespace QuanLyTongHop.Forms
{
    public class FormTask : Form
    {
        private TaskBUS bus = new TaskBUS();
        private DataGridView dgvTask;
        private DataGridView dgvOverdue;
        private ComboBox cbDuAn;
        private ComboBox cbTrangThai;
        private Label lblPercent;

        public FormTask()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Công việc";
            this.Size = new Size(950, 650);

            Label lblDA = new Label { Text = "Dự án:", Location = new Point(20, 20), AutoSize = true };
            cbDuAn = new ComboBox { Location = new Point(80, 16), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cbDuAn.SelectedIndexChanged += (s, e) => FilterTask();

            Label lblTT = new Label { Text = "Trạng thái:", Location = new Point(300, 20), AutoSize = true };
            cbTrangThai = new ComboBox { Location = new Point(380, 16), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cbTrangThai.Items.AddRange(new object[] { "Tất cả", "To Do", "In Progress", "Review", "Done" });
            cbTrangThai.SelectedIndex = 0;
            cbTrangThai.SelectedIndexChanged += (s, e) => FilterTask();

            lblPercent = new Label { Text = "Tiến độ: 0%", Location = new Point(560, 20), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Blue };

            dgvTask = new DataGridView { Location = new Point(20, 60), Size = new Size(900, 280), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            
            Label lblOverdue = new Label { Text = "⚠️ DẠNG CẢNH BÁO TRỄ HẠN (SQLDATAREADER):", Location = new Point(20, 360), AutoSize = true, ForeColor = Color.Red, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            dgvOverdue = new DataGridView { Location = new Point(20, 390), Size = new Size(900, 180), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            this.Controls.AddRange(new Control[] { lblDA, cbDuAn, lblTT, cbTrangThai, lblPercent, dgvTask, lblOverdue, dgvOverdue });
        }

        private void LoadData()
        {
            cbDuAn.DataSource = bus.GetAllDuAn();
            cbDuAn.DisplayMember = "TenDuAn";
            cbDuAn.ValueMember = "MaDuAn";
            dgvOverdue.DataSource = bus.GetTaskOverdue();
        }

        private void FilterTask()
        {
            if (cbDuAn.SelectedValue != null && int.TryParse(cbDuAn.SelectedValue.ToString(), out int maDA))
            {
                dgvTask.DataSource = bus.GetTaskByDuAn(maDA, cbTrangThai.SelectedItem?.ToString());
                double percent = bus.TinhTyLeHoanThanh(maDA);
                lblPercent.Text = $"Tiến độ: {percent:F1}%";
            }
        }
    }
}