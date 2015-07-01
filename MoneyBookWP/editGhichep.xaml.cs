using MoneyBookWP.Common;
using MoneyBookWP.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MoneyBookWP
{
    public sealed partial class editGhichep : Page
    {
        int id;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public editGhichep()
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
            var query = await conn.QueryAsync<taikhoan>("SELECT tentaikhoan from taikhoan");
            List<string> n = new List<string>();
            foreach (var g in query.ToList())
            {
                n.Add(g.Tentaikhoan);
            }
            tutaikhoan.ItemsSource = n;
            var ghichepItem = (ghichep)e.Parameter;
            id = ghichepItem.Id;
            loai.SelectedItem = loai.Items.Cast<ComboBoxItem>().Where(cmbi => (string)cmbi.Tag == ghichepItem.Loai).First();
            sotien.Text = ghichepItem.Sotien;
            mucdichchi.Text = ghichepItem.Mucdichchi;
            ngay.Date = DateTime.Parse(ghichepItem.Ngay);
            gio.Time = TimeSpan.Parse(ghichepItem.Gio);
            ghichu.Text = ghichepItem.Ghichu;
            tutaikhoan.SelectedValue = ghichepItem.Tutaikhoan;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private async Task UpdateGhichepnAsync(int oldid, string newloai, string newsotien, string newmucdichchi, string newngay, string newgio, string newghichu, string newtutaikhoan)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");


            // Retrieve taisan
            var p = await conn.Table<ghichep>().Where(x => x.Id == oldid).FirstOrDefaultAsync();

            if (p != null)
            {
                // Modify
                p.Ghichu = newghichu;
                p.Loai = newloai;
                p.Sotien = newsotien;
                p.Mucdichchi = newmucdichchi;
                p.Ngay = newngay;
                p.Gio = newgio;
                p.Tutaikhoan = newtutaikhoan;
                // Update recordn
                await conn.UpdateAsync(p);
            }
        }
        private async void editGhichepIcon_Click(object sender, RoutedEventArgs e)
        {
            if (sotien.Text == "")
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chưa nhập số tiền.");
                await messageDialog.ShowAsync();
                return;
            }
            if (mucdichchi.Text == "")
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chưa nhập mục đích chi.");
                await messageDialog.ShowAsync();
                return;
            }
            if (loai.SelectedIndex == -1)
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chọn loại ghi chép.");
                await messageDialog.ShowAsync();
                return;
            }
            if (tutaikhoan.SelectedIndex == -1)
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chọn từ tài khoản nào.");
                await messageDialog.ShowAsync();
                return;
            }
            await UpdateGhichepnAsync(id, ((ComboBoxItem)loai.SelectedItem).Content.ToString(), sotien.Text, mucdichchi.Text, ngay.Date.ToString(), gio.Time.ToString(), ghichu.Text, tutaikhoan.SelectedValue.ToString());

            Frame.Navigate(typeof(MainPage));

        }
        private async Task DeleteGhichepAsync(int oldid)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");

            // Retrieve taisan
            var pv = await conn.Table<ghichep>().Where(x => x.Id == oldid).FirstOrDefaultAsync();
            if (pv != null)
            {
                // Delete record
                await conn.DeleteAsync(pv);
            }
        }
        private async void deleteGhichepIcon_Click(object sender, RoutedEventArgs e)
        {
            await DeleteGhichepAsync(id);
            Frame.Navigate(typeof(MainPage));
        }
    }
}
