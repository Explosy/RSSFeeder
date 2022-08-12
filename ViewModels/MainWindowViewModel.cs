using RSSFeeder.Models;
using RSSFeeder.Services;
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

        private DataService _DataService;
        private System.Collections.Specialized.StringCollection UserUrls;


        public uint UpdateTime { get; set; }
        public string DefaultUrl { get; set; }
        
        
        public MainWindowViewModel()
        {
            DefaultUrl = Properties.Settings.Default.DefaultUrl;
            UpdateTime = Properties.Settings.Default.UpdateTime;
            _DataService = new DataService();
            UserUrls = Properties.Settings.Default.UserUrls;

            FeedTabs = new ObservableCollection<Feed>();
            if (UserUrls.Count != 0)
            {
                foreach (var url in UserUrls)
                {
                    var newFeed = _DataService.GetFeedByUrl(url);
                    FeedTabs.Add(newFeed);
                }
            }
            else if (!Equals(DefaultUrl, ""))
            {
                var newFeed = _DataService.GetFeedByUrl(DefaultUrl);
                FeedTabs.Add(newFeed);
            }
            
            
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


        #region old version GetFeed
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
        #endregion
    }
}
