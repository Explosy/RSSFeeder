using System.Xml;

namespace RSSFeeder.Models
{
    public class RssItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string PublishDate { get; set; }

        public RssItem(XmlNode ItemTag)
        {
            //Просмотр всех тегов XML
            foreach (XmlNode xmlTag in ItemTag.ChildNodes)
            {
                switch (xmlTag.Name)
                {
                    case "title":
                        {
                            this.Title = xmlTag.InnerText;
                            break;
                        }
                    case "link":
                        {
                            this.Link = xmlTag.InnerText;
                            break;
                        }
                    case "description":
                        {
                            this.Description = xmlTag.InnerText;
                            break;
                        }
                    case "pubDate":
                        {
                            this.PublishDate = xmlTag.InnerText;
                            break;
                        }
                }
            }
        }

    }
}
