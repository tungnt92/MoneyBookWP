using MoneyBookWP.Common;
using MoneyBookWP.Models;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using SQLite;
using System.Linq;
using System.Threading.Tasks;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MoneyBookWP
{
    public sealed partial class addGhiChep : Page
    {
        public List<ghichep> ghicheps { get; set; }
        string date;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public addGhiChep()
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
            this.navigationHelper.OnNavigatedTo(e);
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            var query = await conn.QueryAsync<taikhoan>("SELECT tentaikhoan from taikhoan");
            List<string> n = new List<string>();
            foreach (var g in query.ToList())
            {
                n.Add(g.Tentaikhoan);
            }
            tutaikhoan.ItemsSource = n;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void addGhichep_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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
            ghichep newTaiKhoan = new ghichep()
            {
                Loai = ((ComboBoxItem)loai.SelectedItem).Content.ToString(),
                Sotien = sotien.Text,
                Mucdichchi = mucdichchi.Text,
                Ngay = ngay.Date.ToString(),
                Gio = gio.Time.ToString(),
                Ghichu = ghichu.Text,
                Tutaikhoan = tutaikhoan.SelectedValue.ToString()
            };
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            await conn.InsertAsync(newTaiKhoan);

            int sotienbandau = 0;
            var Sotienquery = await conn.QueryAsync<taikhoan>("SELECT sotienbandau from taikhoan where tentaikhoan LIKE '" + tutaikhoan.SelectedValue.ToString() + "'");
            foreach (var g in Sotienquery.ToList())
            {
                sotienbandau = int.Parse(g.Sotienbandau);
            }

            if (((ComboBoxItem)loai.SelectedItem).Content.ToString() == "Thu Tiền")
            {
                sotienbandau += int.Parse(sotien.Text);
            }
            if (((ComboBoxItem)loai.SelectedItem).Content.ToString() == "Chi Tiền")
            {
                sotienbandau -= int.Parse(sotien.Text);
            }
            string tentaikhoan = tutaikhoan.SelectedValue.ToString();
            var taisan = await conn.Table<taikhoan>().Where(x => x.Tentaikhoan == tentaikhoan).FirstOrDefaultAsync();
            if (taisan != null)
            {
                // Modify taisan
                taisan.Sotienbandau = sotienbandau.ToString();

                // Update record
                await conn.UpdateAsync(taisan);
            }
            Frame.Navigate(typeof(MainPage));
        }
       
    }
}
