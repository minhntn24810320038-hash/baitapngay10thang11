using System;
using System.Windows.Forms;
using QuanLyTongHop.Forms;

namespace QuanLyTongHop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain());
        }
    }
}