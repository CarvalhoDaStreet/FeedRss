using Microsoft.Toolkit.Parsers.Rss;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FeedRss.Mobile.Models
{
    public class ViewModel : INotifyPropertyChanged
    {
      #region Property

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion*/

       

       private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ObservableCollection<Feed> RSSFeed { get; }

        public Command<Feed> RemoveCommand
        {
            get
            {
                return new Command<Feed>((feed) =>
                {
                    RSSFeed.Remove(feed);
                });
            }
        }

        public ViewModel(string url,string pc)
        {
            RSSFeed = new ObservableCollection<Feed>();
            CarregaRSS(url,pc);
        }

       
        private async Task CarregaRSS(string url,string pc)
        {
            IsBusy = true;

            string feed = null;

            using (var client = new HttpClient())
            {

                feed = await client.GetStringAsync(url);

            }

            if (feed != null)
            {
                RSSFeed.Clear();

                var parser = new RssParser();
                var rss = parser.Parse(feed).OrderByDescending(e => e.PublishDate).ToList(); ;
                string[] words = pc.Split(' ');

                foreach (var rssSchema in rss)
                {
                    foreach (string x in words)
                    {
                        if (rssSchema.Title.Contains(x))
                        {
                            {
                                RSSFeed.Add(new Feed
                                {
                                    Title = rssSchema.Title,
                                    PubDate = rssSchema.PublishDate,
                                    Link = rssSchema.FeedUrl,
                                    Guid = rssSchema.InternalID,
                                    Author = rssSchema.Author,
                                    Thumbnail = string.IsNullOrWhiteSpace(rssSchema.ImageUrl) ? $"" : rssSchema.ImageUrl,
                                    Description = rssSchema.Summary
                                });
                            }
                        }
                    }
                }
                IsBusy = false;

            }
        }
}
}
