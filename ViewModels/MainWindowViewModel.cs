using RSSFeeder.Models;
using RSSFeeder.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Xml;

namespace RSSFeeder.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        /// <summary>Коллекция лент для отображения вкладками в TabControl</summary>
        public ObservableCollection<Feed> FeedTabs { get; }
        
        
        public string DefaultUrl { get; set; }
        public MainWindowViewModel()
        {
            DefaultUrl = Properties.Settings.Default.DefaultUrl;

            FeedTabs = new ObservableCollection<Feed>();
            FeedTabs.Add(GetFeedByUrl(DefaultUrl));
            FeedTabs.Add(GetFeedByUrl(@"https://www.fontanka.ru/fontanka.rss"));
            FeedTabs.Add(GetFeedByUrl(@"https://news.yandex.ru/index.rss"));
            

            #region Commands

            OpenSettingsWindowCommand = new ActionCommand(OnOpenSettingsWindowCommandExecuted, CanOpenSettingsWindowCommandExecute);
            
            #endregion
        }

        #region Commands
        #region OpenSettingsWindowCommand
        public ICommand OpenSettingsWindowCommand { get; }
        private void OnOpenSettingsWindowCommandExecuted (object p)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
        private bool CanOpenSettingsWindowCommandExecute(object p) => true;
        #endregion


        #endregion

        

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
