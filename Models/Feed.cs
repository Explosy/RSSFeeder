using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFeeder.Models
{
    internal class Feed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IEnumerable<Item> Items { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
