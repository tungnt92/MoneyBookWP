using SQLite;
using System;

namespace MoneyBookWP.Models
{
    [Table("taikhoan")]
    public class taikhoan
    {
        private int id;
        private string tentaikhoan;
        private string loaitaikhoan;
        private string loaitiente;
        private string sotienbandau;
        private string ghichu;

        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Tentaikhoan
        {
            get { return tentaikhoan; }
            set { tentaikhoan = value; }
        }
        public string Loaitaikhoan
        {
            get { return loaitaikhoan; }
            set { loaitaikhoan = value; }
        }
        public string Loaitiente
        {
            get { return loaitiente; }
            set { loaitiente = value; }
        }
        public string Sotienbandau
        {
            get { return sotienbandau; }
            set { sotienbandau = value; }
        }
        public string Ghichu
        {
            get { return ghichu; }
            set { ghichu = value; }
        }
    }
}
