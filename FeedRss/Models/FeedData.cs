using System;
using System.Collections.Generic;
using System.Text;

namespace FeedRss.Models
{
    public class FeedData
    {
        public string Titulo { get; private set; }
        public string Link { get; private set; }
        public string[] Palavras { get; private set; }
       

        public FeedData(string titulo, string link, string[] palavras)
        {
            //Inicializaçao das propriedades
            this.Titulo = titulo;
            this.Link = link;
            this.Palavras = palavras;
            
        }


    }
}
