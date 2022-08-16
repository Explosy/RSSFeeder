using System.Collections.Generic;

namespace RSSFeeder.Models
{
    internal class Feed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool Visible { get; set; } = true;
        public IEnumerable<Item> Items { get; set; }
    }
}
