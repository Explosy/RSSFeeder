using RSSFeeder.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace RSSFeeder.Services
{
    internal class DataService
    {
        public Feed GetFeedByUrl(string url)
        {
            try
            {
                XmlDocument xmlDocument = GetXmlDocument(url);
                XmlNode channelXmlNode = xmlDocument.GetElementsByTagName("channel")[0];
                if (channelXmlNode == null) throw new Exception("Ошибка в XML.Описание канала не найдено!");

                Feed newFeed = new Feed();
                newFeed.Items = new List<Item>();
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
                return newFeed;
            }
            catch (Exception exception)
            {
                return new Feed() { Title = "Error", Items = new List<Item>() { new Item() { Description = exception.Message } } };
            }
            


        }

        private XmlDocument GetXmlDocument(string url)
        {
            XmlTextReader xmlTextReader = new XmlTextReader(url);
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(xmlTextReader);
                xmlTextReader.Close();
                return xmlDocument;
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.NameResolutionFailure) throw new Exception("Невозможно соединиться с указаным источником.\r\n" + url);
                else throw exception;
            }
            catch (System.IO.FileNotFoundException)
            {
                throw new Exception("Файл " + url + " не найден!");
            }
            finally
            {
                xmlTextReader.Close();
            }
        }
    }
}
