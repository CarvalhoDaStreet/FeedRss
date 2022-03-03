using Android;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using AndroidX.Core.Content;
using FeedRss.Mobile.Models;
using Microsoft.Toolkit.Parsers.Rss;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Essentials;
using Environment = System.Environment;

namespace FeedRss.Mobile
{
    public partial class MainPage : ContentPage
    {
        public string url;
        public string palavrasChave;
        string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");
       
        public ObservableCollection<Feed> RSSFeed { get; }


        public MainPage()
        {
            InitializeComponent();

            
            cbChooseEdit.Items.Add("Titulo");
            cbChooseEdit.Items.Add("Link");
            cbChooseEdit.Items.Add("Palavra Chave");
            cbChooseEdit.SelectedIndex = 0;
            cbFeedTitleEdit.SelectedIndex = 0;

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
            string pal;

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


        private void btAdicionar_Clicked(object sender, EventArgs e)
        {
            lvGrid.IsVisible = false;
            AddFeedGrid.IsVisible = true;
            btAdicionar.IsVisible = false;
            btBack.IsVisible = true;
            cbFeedTitle.IsVisible = false;

        }

        private void btNotis_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotisPage());
        }

        private void btAddFeedConfirm_Clicked(object sender, EventArgs e)
        {
            lvGrid.IsVisible = true;
            AddFeedGrid.IsVisible = false;
            btAdicionar.IsVisible = true;
            btBack.IsVisible = false;
            cbFeedTitle.IsVisible = true;

           

                StringBuilder csv = new StringBuilder();
                string newLine = string.Format("{0};{1};{2}", tbAddFeedName.Text, tbAddFeedLink.Text, tbAddFeedPC.Text);
                csv.AppendLine(newLine);

                File.AppendAllText(file, csv.ToString());

            Navigation.PushAsync(new MainPage());

        }

        private void btBack_Clicked(object sender, EventArgs e)
        {
            lvGrid.IsVisible = true;
            EditFeedGrid.IsVisible = false;
            AddFeedGrid.IsVisible = false;
            btAdicionar.IsVisible = true;
            btBack.IsVisible = false;
            cbFeedTitle.IsVisible = true;

        }

        private void btEditFeedConfirm_Clicked(object sender, EventArgs e)
        {

          
            string title = tbEditFeedName.Text;
            string link = tbEditFeedLink.Text;
            string pc = tbEditFeedPC.Text;
            string selected = cbFeedTitleEdit.SelectedIndex.ToString();


            switch (cbChooseEdit.SelectedIndex)
            {
                case 0:
                    tbAddFeedLink.IsEnabled = false;
                    tbAddFeedPC.IsEnabled = false;
                    if (title != "")
                    {

                       string text = File.ReadAllText(file);
                       using (StreamReader s = new StreamReader(file))
                        {
                            String line;
                            while ((line = s.ReadLine()) != null)
                            {
                                if (cbFeedTitleEdit.SelectedIndex.ToString() == line.Split(';')[0])
                                {
                                    string parts = line.Split(';')[0];
                                    text = text.Replace(parts, title);
                                    
                                }
                            }
                        }

                        File.WriteAllText(file, text);
                    }
                    break;

                case 1:
                    tbAddFeedName.IsEnabled = false;
                    tbAddFeedPC.IsEnabled = false;
                    if (link != "")
                    {
                        string text = File.ReadAllText(file);

                        using (var s= new System.IO.StreamReader(file))
                        {
                            String line;
                            while ((line = s.ReadLine()) != null)
                            {
                                if (cbFeedTitleEdit.SelectedIndex.ToString() == line.Split(';')[0])
                                {
                                    string parts = line.Split(';')[1];
                                    text = text.Replace(parts, link);
                                }
                            }
                        }

                        File.WriteAllText(file, text);
                    }
                    break;
                case 2:
                    tbAddFeedLink.IsEnabled = false;
                    tbAddFeedName.IsEnabled = false;
                    if (pc != "")
                    {
                        string text = File.ReadAllText(file);

                        using (StreamReader s = new StreamReader(file))
                        {
                            String line;
                            while ((line = s.ReadLine()) != null)
                            {
                                if (cbFeedTitleEdit.SelectedIndex.ToString() == line.Split(';')[0])
                                {
                                    string parts = line.Split(';')[2];
                                    text = text.Replace(parts, pc);
                                }
                            }
                        }

                        File.WriteAllText(file, text);

                    }
                    break;
                default:
                    break;
            }

        }

        private void btEditar_Clicked(object sender, EventArgs e)
        {
            lvGrid.IsVisible = false;
            EditFeedGrid.IsVisible = true;
            AddFeedGrid.IsVisible = false;
            btAdicionar.IsVisible = false;
            btBack.IsVisible = true;
            cbFeedTitle.IsVisible = false;

            string Line = string.Empty;
            //var tmp = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            //System.IO.Stream s = tmp.GetManifestResourceStream("FeedRss.Mobile.dados.txt");

            StreamReader sr = new StreamReader(file);
            while ((Line = sr.ReadLine()) != null)
            {
                cbFeedTitleEdit.Items.Add(Line.Split(';')[0]);

            }
            view();

        }

        private void btDeleteFeed_Clicked(object sender, EventArgs e)
        {
            string Line = string.Empty;

            string selected = cbFeedTitleEdit.SelectedIndex.ToString();
           
            //var tmp = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            //System.IO.Stream fileName = tmp.GetManifestResourceStream("FeedRss.Mobile.dados.txt");

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //var filename = Path.Combine(path, "dados.txt");

            string strSearchText = selected;
            string strOldText;
            string n = "";
            StreamReader sr = new StreamReader(file);
            while ((strOldText = sr.ReadLine()) != null)
            {
                if (!strOldText.Contains(strSearchText))
                {
                    n += strOldText + Environment.NewLine;
                }
            }
            sr.Close();
            File.WriteAllText(file, n);

        }

        private void btListar_Clicked(object sender, EventArgs e)
        {

        }
    }

}
