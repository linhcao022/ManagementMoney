using ManagementMoney.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ManagementMoney
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public ObservableCollection<Money> moneys { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            DBConnect.InitializeDatabase();

            moneys = new ObservableCollection<Money>();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            DBConnect.AddData(collect.Text,day.Text, spend.Text);
            DBConnect.LoadRecordAsync(moneys);
            Console.WriteLine(moneys.Count);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var id = btn.Tag;

            for (var i = 0; i < moneys.Count; i++)
            {
                Money money = moneys[i];
                if (money.id == Int16.Parse(id.ToString()))
                {
                    moneys.Remove(money);
                    break;
                }
            }

        }

        private Money editMoney;

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var id = btn.Tag;

            for (var i = 0; i < moneys.Count; i++)
            {
                Money money = moneys[i];
                if (money.id == Int16.Parse(id.ToString()))
                {
                    editMoney = money;
                    day.Text = editMoney.day;
                    collect.Text = editMoney.moneyCollected;
                    spend.Text = editMoney.spend;
                    break;
                }
            }
            edit.ShowAsync();
        }

        private void edit_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void edit_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            editCollect.Text = editMoney.moneyCollected;
            editSpend.Text = editMoney.spend;
            editDay.Text = editMoney.day;
        }
    }

}

