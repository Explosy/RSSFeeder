using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace RSSFeeder.Models
{
    public class RssFeed
    {
        public string Title;
        public string Description;
        public string Link;
        public List<RssItem> Items;

        public RssFeed(string url)
        {
            Items = new List<RssItem>();
            XmlTextReader xmlTextReader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlTextReader);
                xmlTextReader.Close();
                XmlNode channelXmlNode = xmlDoc.GetElementsByTagName("channel")[0];

                if (channelXmlNode != null)
                {
                    foreach (XmlNode channelNode in channelXmlNode.ChildNodes)
                    {
                        switch (channelNode.Name)
                        {
                            case "title":
                                {
                                    this.Title = channelNode.InnerText;
                                    break;
                                }
                            case "link":
                                {
                                    this.Link = channelNode.InnerText;
                                    break;
                                }
                            case "description":
                                {
                                    this.Description = channelNode.InnerText;
                                    break;
                                }
                            case "item":
                                {
                                    RssItem channelItem = new RssItem(channelNode);
                                    Items.Add(channelItem);
                                    break;
                                }
                        }
                    } 
                }
                else
                {
                    throw new Exception("Ошибка в XML.Описание канала не найдено!");
                }
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.NameResolutionFailure) throw new Exception("Невозможно соединиться с указаным источником.\r\n" + url);
                else throw exception;
            }
            catch (System.IO.FileNotFoundException)
            {
                throw new Exception("Файл" +url + "не найден!");
            }
            finally
            {
                xmlTextReader.Close();
            }
        }

    }
}
