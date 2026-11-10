using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyTongHop.Forms
{
    public class FormMain : Form
    {
        private Panel panelMenu;
        private Panel panelDesktop;
        private Button btnTask;
        private Button btnGarage;
        private Button btnKaraoke;
        private Label lblTitle;
        private Form activeForm = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panelMenu = new Panel();
            this.panelDesktop = new Panel();
            this.btnTask = new Button();
            this.btnGarage = new Button();
            this.btnKaraoke = new Button();
            this.lblTitle = new Label();

            this.Text = "HỆ THỐNG QUẢN LÝ TỔNG HỢP - BÀI TẬP 6 (3-TIER)";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.panelMenu.Dock = DockStyle.Left;
            this.panelMenu.Width = 260;
            this.panelMenu.BackColor = Color.FromArgb(41, 53, 65);

            this.lblTitle.Text = "MENU CHỨC NĂNG";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            this.lblTitle.Dock = DockStyle.Top;
            this.lblTitle.Height = 60;
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            SetupButton(btnTask, "1. Quản lý Dự án & Task", 70, (s, e) => OpenChildForm(new FormTask()));
            SetupButton(btnGarage, "2. Quản lý Garage Ô tô", 135, (s, e) => OpenChildForm(new FormGarage()));
            SetupButton(btnKaraoke, "3. Đặt phòng Karaoke", 200, (s, e) => OpenChildForm(new FormKaraoke()));

            this.panelDesktop.Dock = DockStyle.Fill;
            this.panelDesktop.BackColor = Color.FromArgb(245, 247, 250);

            this.panelMenu.Controls.Add(btnKaraoke);
            this.panelMenu.Controls.Add(btnGarage);
            this.panelMenu.Controls.Add(btnTask);
            this.panelMenu.Controls.Add(lblTitle);

            this.Controls.Add(this.panelDesktop);
            this.Controls.Add(this.panelMenu);
        }

        private void SetupButton(Button btn, string text, int topPosition, EventHandler clickEvent)
        {
            btn.Text = text;
            btn.Top = topPosition;
            btn.Left = 10;
            btn.Width = 240;
            btn.Height = 50;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(51, 63, 75);
            btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(12, 0, 0, 0);
            btn.Click += clickEvent;
        }

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null) activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktop.Controls.Add(childForm);
            this.panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
    }
}