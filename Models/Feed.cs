using System.Collections.Generic;

namespace RSSFeeder.Models
{
    internal class Feed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
