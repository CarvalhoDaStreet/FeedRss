using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Syndication; 
using System.Drawing;
using System.Diagnostics;
using System.Web;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using SyndicationItem = System.ServiceModel.Syndication.SyndicationItem;
using System.Xml.Linq;
using FeedRss.Models;
using Path = System.Windows.Shapes.Path;

namespace FeedRss
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string url;
        public string desti;
        public string palavrasChave;
        public DateTime date;
        public int count;

        public MainWindow()
        {

            InitializeComponent();
            carregarInfo();

            cbEdit.Items.Add("Titulo");
            cbEdit.Items.Add("Link");
            cbEdit.Items.Add("Palavra-Chave");

            cbFeedTitle.SelectionChanged += new SelectionChangedEventHandler(comboBox1SelectionChanged);
        }


        private void carregarInfo()
        {
            string Line = string.Empty;
            string fileName = @"C:\Users\carlo\Desktop\FeedRssv1\FeedRss\FeedRss\Data\dados.txt";

            StreamReader sr = new StreamReader(fileName);
            while ((Line = sr.ReadLine()) != null)
            {
                cbFeedTitle.Items.Add(Line.Split(';')[0]);
                url = Line.Split(";")[1];
            }
            view();
        }


        private void view()
        {

            string Line = string.Empty;

            string fileName = @"C:\Users\carlo\Desktop\FeedRssv1\FeedRss\FeedRss\Data\dados.txt";
            string selected = cbFeedTitle.SelectedValue.ToString();

            StreamReader sr = new StreamReader(fileName);
            while ((Line = sr.ReadLine()) != null)
            {
                try
                {
                    url = Line.Split(";")[1];
                    palavrasChave = Line.Split(";")[2];
                    string[] words = palavrasChave.Split(' ');
                    if (selected == Line.Split(';')[0])
                    {

                        XmlReader xmlReader = XmlReader.Create(url);
                        SyndicationFeed syndicationFeed = SyndicationFeed.Load(xmlReader);


                        foreach (SyndicationItem item in syndicationFeed.Items)
                        {

                            string desc = item.Title.Text;
                            foreach (string x in words)
                            {
                                if (item.Title.Text.Contains(x))
                                {
                                    lstFeedItems.Items.Add(item.Title.Text);
                                    lstFeedItems.Items.Add(item.Links[0].Uri);
                                    date = item.PublishDate.DateTime;
                                    lstFeedItems.Items.Add(date);
                                    lstFeedItems.Items.Add("_____________________________________________________________________________________________________________________________");
                                    count++;
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                }


            }
            tbnotis.Text = count.ToString();

        }


        private void LinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void lstFeedItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            string noticia = lstFeedItems.SelectedItem.ToString();
            NewsShow.Navigate(noticia);


            NewsShow.Visibility = System.Windows.Visibility.Visible;
            NewsShow.SetValue(Grid.ColumnSpanProperty, 2);
            lstFeedItems.Visibility = System.Windows.Visibility.Hidden;
            btClose.Visibility = System.Windows.Visibility.Visible;
            count--;
            tbnotis.Text = count.ToString();
        }




        private void btAddFeed_Click(object sender, RoutedEventArgs e)
        {
            feedCard.Visibility = System.Windows.Visibility.Hidden;
            AddFeedCard.Visibility = System.Windows.Visibility.Visible;
        }

        private void btEditFeed_Click(object sender, RoutedEventArgs e)
        {
            feedCard.Visibility = System.Windows.Visibility.Hidden;
            AddFeedCard.Visibility = System.Windows.Visibility.Hidden;
            EditFeedCard.Visibility = System.Windows.Visibility.Visible;

        }



        private void btAddFeedConfirmar_Click(object sender, RoutedEventArgs e) {
            feedCard.Visibility = System.Windows.Visibility.Visible;
            AddFeedCard.Visibility = System.Windows.Visibility.Hidden;
            cbFeedTitle.Items.Add(tbAddFeedTitle.Text);



            StringBuilder csv = new StringBuilder();
            string newLine = string.Format("\n{0};{1};{2}", tbAddFeedTitle.Text, tbAddFeedLink.Text, tbAddFeedPC.Text);
            csv.AppendLine(newLine);

            string fileName = @"C:\Users\carlo\Desktop\FeedRssv1\FeedRss\FeedRss\Data\dados.txt";
            File.AppendAllText(fileName, csv.ToString());

        }

        private void btCloseAddFeed_MouseDown(object sender, MouseButtonEventArgs e)
        {
            feedCard.Visibility = System.Windows.Visibility.Visible;
            AddFeedCard.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btCloseEditFeed_MouseDown(object sender, MouseButtonEventArgs e)
        {
            feedCard.Visibility = System.Windows.Visibility.Visible;
            EditFeedCard.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btEditFeedConfirm_Click(object sender, RoutedEventArgs e)
        {


            string fileName = @"C:\Users\carlo\Desktop\FeedRssv1\FeedRss\FeedRss\Data\dados.txt";

            string title = tbEditFeedTitle.Text;
            string link = tbEditFeedLink.Text;
            string pc = tbEditFeedPC.Text;
            string selected = cbFeedTitle.SelectedValue.ToString();


            switch (cbEdit.SelectedIndex)
            {
                case 0:
                    if (title != "")
                    {

                        string text = File.ReadAllText(fileName);
                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (cbFeedTitle.Text == line.Split(';')[0])
                                {
                                    string parts = line.Split(';')[0];
                                    text = text.Replace(parts, title);
                                }
                            }
                        }

                        File.WriteAllText(fileName, text);
                    }
                    break;

                case 1:
                    if (link != "")
                    {
                        string text = File.ReadAllText(fileName);

                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (cbFeedTitle.Text == line.Split(';')[0])
                                {
                                    string parts = line.Split(';')[1];
                                    text = text.Replace(parts, link);
                                }
                            }
                        }

                        File.WriteAllText(fileName, text);
                    }
                    break;
                case 2:
                    if (pc != "")
                    {
                        string text = File.ReadAllText(fileName);

                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (cbFeedTitle.Text == line.Split(';')[0])
                                {
                                    string parts = line.Split(';')[2];
                                    text = text.Replace(parts, pc);
                                }
                            }
                        }

                        File.WriteAllText(fileName, text);

                    }
                    break;
                default:
                    break;
            }

            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

        private void btRemoveFeed_Click(object sender, RoutedEventArgs e)
        {
            string Line = string.Empty;
            string fileName = @"C:\Users\carlo\Desktop\FeedRssv1\FeedRss\FeedRss\Data\dados.txt";

            string strSearchText = cbFeedTitle.Text ;
            string strOldText;
            string n = "";
            StreamReader sr = File.OpenText(fileName);
            while ((strOldText = sr.ReadLine()) != null)
            {
                if (!strOldText.Contains(strSearchText))
                {
                    n += strOldText + Environment.NewLine;
                }
            }
            sr.Close();
            File.WriteAllText(fileName, n);

            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();

        }


        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }


        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }

        private void NewsShow_Navigated(object sender, NavigationEventArgs e)
        {
            SetSilent(NewsShow, true);
        }

        private void btClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewsShow.Visibility = System.Windows.Visibility.Hidden;
            lstFeedItems.Visibility = System.Windows.Visibility.Visible;
            btClose.Visibility = System.Windows.Visibility.Hidden;
        }

        void comboBox1SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelectedIndex = cbFeedTitle.SelectedIndex;
            count = 0;
            lstFeedItems.Items.Clear();
            view();

        }

        private void btListPC_Click(object sender, RoutedEventArgs e)
        {

            string Line = string.Empty;
            string fileName = @"C:\Users\carlo\Desktop\FeedRssv1\FeedRss\FeedRss\Data\dados.txt";
            StreamReader sr = new StreamReader(fileName);
            string pal;
            while ((Line = sr.ReadLine()) != null)
            {
                    if (cbFeedTitle.Text == Line.Split(";")[0])
                    {
                        pal = Line.Split(";")[2];
                        MessageBox.Show(string.Format("{0}", pal));
                }
            }

        }
            private void btNotis_MouseDown(object sender, MouseButtonEventArgs e)
            {

            }
        }
}

    
