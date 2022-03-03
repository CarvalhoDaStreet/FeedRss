using System;
using System.Collections.Generic;
using System.Text;

namespace FeedRss.Models
{
    class UpdateFeed
    {
        public Feed feed;
        public User user;
        public string title { get; set; }
        public string description { get; set; }
        public string newsPageLink { get; set; }
        public string category { get; set; }

        private void update(){
            
        }
    }
}
