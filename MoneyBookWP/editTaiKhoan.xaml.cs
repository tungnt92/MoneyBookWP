using MoneyBookWP.Common;
using MoneyBookWP.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MoneyBookWP
{
    public sealed partial class editTaiKhoan : Page
    {
        int id;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public editTaiKhoan()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            var taikhoanID = (int)e.Parameter;
            var query = await conn.QueryAsync<taikhoan>("SELECT * from taikhoan WHERE Id = " + taikhoanID);
            foreach (var g in query.ToList())
            {

                id = g.Id;
                tentaikhoan.Text = g.Tentaikhoan;
                loaitaikhoan.SelectedItem = loaitaikhoan.Items.Cast<ComboBoxItem>().Where(cmbi => (string)cmbi.Tag == g.Loaitaikhoan).First();
                loaitiente.SelectedItem = loaitiente.Items.Cast<ComboBoxItem>().Where(cmbi => (string)cmbi.Tag == g.Loaitiente).First();
                sotienbandau.Text = g.Sotienbandau;
                ghichu.Text = g.Ghichu;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void edittaikhoan_Click(object sender, RoutedEventArgs e)
        {
            await UpdateTaiKhoanAsync(id, tentaikhoan.Text, ((ComboBoxItem)loaitiente.SelectedItem).Content.ToString(), ((ComboBoxItem)loaitaikhoan.SelectedItem).Content.ToString(), sotienbandau.Text, ghichu.Text);  

            Frame.Navigate(typeof(MainPage));
        }
        private async Task UpdateTaiKhoanAsync(int oldid, string newtentaikhoan, string newloaitaikhoan, string newloaitiente, string newsotienbandau, string newghichu)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");


            // Retrieve taisan
            var pTaiKhoan = await conn.Table<taikhoan>().Where(x => x.Id == oldid).FirstOrDefaultAsync();

            if (pTaiKhoan != null)
            {
                // Modify
                pTaiKhoan.Ghichu = newghichu;
                pTaiKhoan.Loaitaikhoan = newloaitaikhoan;
                pTaiKhoan.Loaitiente = newloaitiente;
                pTaiKhoan.Sotienbandau = newsotienbandau;
                pTaiKhoan.Tentaikhoan = newtentaikhoan;
                // Update recordn
                await conn.UpdateAsync(pTaiKhoan);
            }
        }
        private async Task DeleteTaiSanAsync(int oldid)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");

            // Retrieve taisan
            var pvtaisan = await conn.Table<taikhoan>().Where(x => x.Id == oldid).FirstOrDefaultAsync();
            if (pvtaisan != null)
            {
                // Delete record
                await conn.DeleteAsync(pvtaisan);
            }
        }

        private async void deletetaikhoan_Click(object sender, RoutedEventArgs e)
        {
            await DeleteTaiSanAsync(id);
            Frame.Navigate(typeof(MainPage));
        }
    }
}
