using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efarmer
{
    class FeedData
    {
        private List<FeedItem> _items = new List<FeedItem>();
        public List<FeedItem> items
        {
            get
            {
                return this._items;
            }
        }
    }
    public class FeedItem
    {

        public string Title { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime PubDate { get; set; }

        public Uri Link { get; set; }

    }
}
