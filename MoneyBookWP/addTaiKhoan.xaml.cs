using MoneyBookWP.Common;
using MoneyBookWP.Models;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using SQLite;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MoneyBookWP
{
    public sealed partial class addTaiKhoan : Page
    {
        public List<taikhoan> taikhoans { get; set; }
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public addTaiKhoan()
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }


        #endregion

        private async void addtaikhoan_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (tentaikhoan.Text == "")
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chưa nhập tên tài Khoản.");
                await messageDialog.ShowAsync();
                return;
            }
            if (sotienbandau.Text == "")
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chưa nhập số tiền ban đầu.");
                await messageDialog.ShowAsync();
                return;
            }
            if (loaitaikhoan.SelectedIndex == -1)
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chọn loại tài khoản.");
                await messageDialog.ShowAsync();
                return;
            }
            if (loaitiente.SelectedIndex == -1)
            {
                MessageDialog messageDialog = new MessageDialog("Bạn chọn loại tiền tệ.");
                await messageDialog.ShowAsync();
                return;
            }
            taikhoan newTaiKhoan = new taikhoan()
            {
                Tentaikhoan = tentaikhoan.Text,
                Sotienbandau = sotienbandau.Text,
                Loaitaikhoan = ((ComboBoxItem)loaitaikhoan.SelectedItem).Content.ToString(),
                Loaitiente = ((ComboBoxItem)loaitiente.SelectedItem).Content.ToString(),
                Ghichu = ghichu.Text
            };
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("moneybook.db");
            await conn.InsertAsync(newTaiKhoan);

            Frame.Navigate(typeof(MainPage));
        }
    }
}
