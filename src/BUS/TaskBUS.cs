using System.Data;
using QuanLyTongHop.DAL;

namespace QuanLyTongHop.BUS
{
    public class TaskBUS
    {
        private TaskDAL dal = new TaskDAL();

        public DataTable GetAllDuAn() => dal.GetAllDuAn();
        public DataTable GetTaskByDuAn(int maDuAn, string trangThai) => dal.GetTaskByDuAn(maDuAn, trangThai);
        public bool CapNhatTrangThaiTask(int maTask, string trangThai) => dal.CapNhatTrangThaiTask(maTask, trangThai);
        public DataTable GetTaskOverdue() => dal.GetTaskOverdue();
        public double TinhTyLeHoanThanh(int maDuAn) => dal.TinhTyLeHoanThanh(maDuAn);
    }
}