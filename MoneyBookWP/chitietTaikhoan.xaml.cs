using MoneyBookWP.Common;
using MoneyBookWP.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MoneyBookWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class chitietTaikhoan : Page
    {
        int id;
        public List<ghichep> ghicheps { get; set; }
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public chitietTaikhoan()
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
            var taikhoanItem = (taikhoan)e.Parameter;
            id = taikhoanItem.Id;
            var qGhiChep = conn.Table<ghichep>().Where(t => t.Tutaikhoan.Contains(taikhoanItem.Tentaikhoan));
            ghicheps = await qGhiChep.ToListAsync();
            ghichepListItems.ItemsSource = ghicheps;
            double sotienbandau = 0;

            var Sotienquery = await conn.QueryAsync<taikhoan>("SELECT sotienbandau from taikhoan where Id = " + id);
            foreach (var g in Sotienquery.ToList())
            {
                sotienbandau = double.Parse(g.Sotienbandau);
            }

            tonggiatritaikhoan.Text = "Số dư: " + sotienbandau.ToString("#,##0")+ " đ";
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ghichepListItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            var taikhoanItem = (ghichep)e.ClickedItem;
            this.Frame.Navigate(typeof(editGhichep), taikhoanItem);
        }

        private void themGhiChepIcon_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(addGhiChep));
        }

        private void editTaikhoanicons_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(editTaiKhoan), id);
        }
    }
}
