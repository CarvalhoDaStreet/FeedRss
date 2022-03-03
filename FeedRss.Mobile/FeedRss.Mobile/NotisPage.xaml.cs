using FeedRss.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedRss.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotisPage : ContentPage
    {

        public string url;
        public string palavrasChave;
        string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");


        public ObservableCollection<Feed> RSSFeed { get; }

        public NotisPage()
        {
            InitializeComponent();

            carregarInfo();
            BindingContext = new ViewModel(url, palavrasChave);
            cbFeedTitle.SelectedIndexChanged += this.myPickerSelectedIndexChanged;

            

        }

        void ListView_OnItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var selectedItem = (Feed)e.Item;
            Device.OpenUri(new Uri(selectedItem.Link));

            
        }

        public void carregarInfo()
        {

            string Line = string.Empty;

            StreamReader sr = new StreamReader(file);
            while ((Line = sr.ReadLine()) != null)
            {
                cbFeedTitle.Items.Add(Line.Split(';')[0]);
              
            }
            sr.Close();
            view();

        }

        private void view()
        {
            string Line = string.Empty;
            cbFeedTitle.SelectedIndex = 1;
            string selected = cbFeedTitle.SelectedItem.ToString();

            StreamReader sr = new StreamReader(file);
            while ((Line = sr.ReadLine()) != null)
            {

                if (selected == Line.Split(';')[0])
                {
                    url = Line.Split(';')[1];
                    palavrasChave = Line.Split(';')[2];
                }
            }
            sr.Close();

        }


        public void myPickerSelectedIndexChanged(object sender, EventArgs e)
        {

            var currentSelectedIndex = cbFeedTitle.SelectedIndex;
            BindingContext = new ViewModel(url, palavrasChave);
            view();

        }

      

        private void btBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

       

        private void RemoveNoti_Clicked_1(object sender, EventArgs e)
        {
            var button = sender as Button;
            var feed = button?.BindingContext as Feed;
            var vm = BindingContext as ViewModel;
            vm?.RemoveCommand.Execute(feed);
        }
    }
}