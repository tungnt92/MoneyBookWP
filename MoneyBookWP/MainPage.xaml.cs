using MoneyBookWP.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Linq;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MoneyBookWP
{
    public sealed partial class MainPage : Page
    {
        public List<taikhoan> taikhoans { get; set; }
        public List<ghichep> ghicheps { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool dbExist = await CheckDbAsync("moneybook.db");
            if (!dbExist)
            {
                await CreateDatabaseAsync();
                //await CreateTaiKhoanItems();
            }
            // Get Items
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            var query = conn.Table<taikhoan>();
            taikhoans = await query.ToListAsync();

            var queryk = await conn.QueryAsync<ghichep>("SELECT * from ghichep order by ngay");
            List<ghichep> n = new List<ghichep>();
            foreach (ghichep g in queryk.ToList())
            {
                n.Add(g);
            }

            //var qGhiChep = conn.Table<ghichep>();
            //ghicheps = await qGhiChep.ToListAsync();

            // Show Tiems
            taikhoanList.ItemsSource = taikhoans;

            var result = from item in n
                         group item by DateTime.Parse(item.Ngay).ToString("dd-MM-yyyy")
                             into grp
                         orderby grp.Key
                         select grp;
            cvsGhiChepList.Source = result;

            ghichepListpv.DataContext = n;

            double sotienbandau = 0;
            var Sotienquery = await conn.QueryAsync<taikhoan>("SELECT sotienbandau from taikhoan");
            foreach (var g in Sotienquery.ToList())
            {
                sotienbandau += double.Parse(g.Sotienbandau);
            }
            tongsodutaikhoan.Text = "Tổng: " + sotienbandau.ToString("#,##0") +" đ";
        }

        private async Task CreateTaiKhoanItems()
        {
            var taikhoanlist = new List<taikhoan>
            {
                new taikhoan()
                {
                    Tentaikhoan = "Tiền mặt",
                    Sotienbandau = "-10000000",
                    Loaitiente = "USD",
                    Loaitaikhoan = "Ví",
                    Ghichu = "Không có"
                }
            };
            var ghichepList = new List<ghichep>
            {
                new ghichep()
                {
                    Ghichu = "Khong co",
                    Mucdichchi="khong biet",
                    Loai = "Thu tien",
                    Sotien = "1202"
                }
            };
            // Add rows to the taisan Table
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            await conn.InsertAllAsync(taikhoanlist);
            await conn.InsertAllAsync(ghichepList);
        }

        #region SQLite utils
        private async Task<bool> CheckDbAsync(string dbName)
        {
            bool dbExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
            }
            catch (Exception)
            {
                dbExist = false;
            }

            return dbExist;
        }

        private async Task CreateDatabaseAsync()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            await conn.CreateTableAsync<taikhoan>();
            await conn.CreateTableAsync<ghichep>();
        }
        private async Task SearchtaisanByNameAsync(string name)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");

            var query = conn.Table<taikhoan>().Where(x => x.Loaitaikhoan.Contains("Pablo"));
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                // ...
            }

            var alltaisans = await conn.QueryAsync<taikhoan>("SELECT * FROM taisan");
            foreach (var taisan in alltaisans)
            {
                // ...
            }

            var citytaisans = await conn.QueryAsync<taikhoan>(
                "SELECT name FROM taisan WHERE brand = ?", new object[] { "Sony" });
            foreach (var taisan in citytaisans)
            {
                // ...
            }
        }

        private async Task UpdatetaisanNameAsync(string oldName, string newName)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");

            // Retrieve taisan
            var taisan = await conn.Table<taikhoan>().Where(x => x.Loaitiente == oldName).FirstOrDefaultAsync();
            if (taisan != null)
            {
                // Modify taisan
                taisan.Tentaikhoan = newName;

                // Update record
                await conn.UpdateAsync(taisan);
            }
        }

        private async Task DeletetaisanAsync(string name)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");

            // Retrieve taisan
            var taisan = await conn.Table<taikhoan>().Where(x => x.Tentaikhoan == name).FirstOrDefaultAsync();
            if (taisan != null)
            {
                // Delete record
                await conn.DeleteAsync(taisan);
            }
        }

        private async Task DropTableAsync(string name)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            await conn.DropTableAsync<taikhoan>();
        }

        #endregion SQLite utils

        private void TaisanList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void themtaisanIcon_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(addTaiKhoan));
        }

        private void taikhoanList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var taikhoanItem = (taikhoan)e.ClickedItem;
            this.Frame.Navigate(typeof(chitietTaikhoan), taikhoanItem);
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(pivot.SelectedIndex == 0)
            {
                themtaisanIcon.Visibility = Visibility.Visible;
                themghichuIcon.Visibility = Visibility.Collapsed;
                tongsodutaikhoan.Visibility = Visibility.Visible;
            }
            if (pivot.SelectedIndex == 1)
            {
                tongsodutaikhoan.Visibility = Visibility.Collapsed;
                themtaisanIcon.Visibility = Visibility.Collapsed;
                themghichuIcon.Visibility = Visibility.Visible;
                
            }
            if (pivot.SelectedIndex == 2)
            {
                themtaisanIcon.Visibility = Visibility.Collapsed;
                themghichuIcon.Visibility = Visibility.Collapsed;
                
            }
        }

        private void themghichuIcon_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(addGhiChep));
        }

        private void ghichepList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var ghichepItem = (ghichep)e.ClickedItem;
            this.Frame.Navigate(typeof(editGhichep), ghichepItem);
        }

        private void introApp_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(gioithieu));
        }
    }
}
