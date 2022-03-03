using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FeedRss.Models
{

    public class User
    {
        public Dictionary<string, FeedData>Lista {get; private set;}
        public event MetodosSemParametros DadosAtualizados;
        public event MetodosSemParametros EscritaTerminada;

        public User()
        {
            Lista = new Dictionary<string, FeedData>();
          
    }

        public void EscreverXML(string filename)
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", Encoding.UTF8.ToString(),"yes"),
                new XComment("Feeds Listados"),
                new XElement("Feeds",
                new XElement("Titulo"),
                new XElement("Link"),
                new XElement("Palavras")));

            foreach(FeedData fd in Lista.Values)
            {
                XElement newFeed = new XElement("Feed", new XAttribute("Titulo", fd.Titulo), new XAttribute("Link", fd.Link), new XAttribute("Palavras", fd.Palavras));
                doc.Element("Feeds").Add(newFeed);
            }

            //Guarda dados do XML em Ficheiro
            doc.Save(filename);

            //if (EscritaTerminada != null)
              //  EscritaTerminada();

        }

    }   
}
