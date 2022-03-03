using FeedRss.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FeedRss
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User ModelUser { get; private set; }
        public Feed Model_Feed { get; private set; }
        public App()
        {
            //Instanciação das classes Model

            ModelUser = new User();
            Model_Feed = new Feed();
        }

    }
}
