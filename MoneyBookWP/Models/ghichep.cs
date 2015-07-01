using SQLite;
using System;

namespace MoneyBookWP.Models
{
    [Table("ghichep")]
    public class ghichep
    {
        private int id;
        private string loai;
        private string sotien;
        private string mucdichchi;
        private string ngay;
        private string gio;
        private string ghichu;
        private string tutaikhoan;
        
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Loai
        {
            get { return loai; }
            set { loai = value; }
        }

        public string Ghichu
        {
            get
            {
                return ghichu;
            }

            set
            {
                ghichu = value;
            }
        }

        public string Sotien
        {
            get
            {
                return sotien;
            }

            set
            {
                sotien = value;
            }
        }

        public string Mucdichchi
        {
            get
            {
                return mucdichchi;
            }

            set
            {
                mucdichchi = value;
            }
        }

        public string Ngay
        {
            get
            {
                return ngay;
            }

            set
            {
                ngay = value;
            }
        }

        public string Gio
        {
            get
            {
                return gio;
            }

            set
            {
                gio = value;
            }
        }

        public string Tutaikhoan
        {
            get
            {
                return tutaikhoan;
            }

            set
            {
                tutaikhoan = value;
            }
        }
    }
}
