using RSSFeeder.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace RSSFeeder.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ObservableCollection<Feed> Feeds { get; }
        public ObservableCollection<Item> Items { get; }
        
        public MainWindowViewModel()
        {
            Feeds = new ObservableCollection<Feed>();
            Feeds.Add(GetFeedByUrl("https://habr.com/rss/interesting/"));
            Items = new ObservableCollection<Item>(Feeds[0].Items);
            Title = Feeds[0].Title;
        }

        
        private string _Title = "RSSFeeder";
        
        public string Title
        {
            get => _Title;
            set
            {
                if (Equals(_Title, value)) return;
                _Title = value;
                OnPropertyChanged();
            }
        }

        private Feed GetFeedByUrl (string url)
        {
            Feed newFeed = new Feed();
            newFeed.Items = new List<Item>();
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
                                    newFeed.Title = channelNode.InnerText;
                                    break;
                                }
                            case "link":
                                {
                                    newFeed.Link = channelNode.InnerText;
                                    break;
                                }
                            case "description":
                                {
                                    newFeed.Description = channelNode.InnerText;
                                    break;
                                }
                            case "item":
                                {
                                    Item item = new Item();
                                    foreach (XmlNode xmlTag in channelNode.ChildNodes)
                                    {
                                        switch (xmlTag.Name)
                                        {
                                            case "title":
                                                {
                                                    item.Title = xmlTag.InnerText;
                                                    break;
                                                }
                                            case "link":
                                                {
                                                    item.Link = xmlTag.InnerText;
                                                    break;
                                                }
                                            case "description":
                                                {
                                                    item.Description = Regex.Replace(xmlTag.InnerText, "<.*?>", String.Empty);
                                                    break;
                                                }
                                            case "pubDate":
                                                {
                                                    item.PublishDate = xmlTag.InnerText;
                                                    break;
                                                }
                                        }
                                    }
                                    (newFeed.Items as List<Item>).Add(item);
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    throw new Exception("Ошибка в XML.Описание канала не найдено!");
                }
                return newFeed;
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.NameResolutionFailure) throw new Exception("Невозможно соединиться с указаным источником.\r\n" + url);
                else throw exception;
            }
            catch (System.IO.FileNotFoundException)
            {
                throw new Exception("Файл" + url + "не найден!");
            }
            finally
            {
                xmlTextReader.Close();
            }
        }
    }
}
