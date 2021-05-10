using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ItemFix
{
    /// <summary>
    /// A Page, has at most 12 Post represented via <see cref="Post.Post(ItemID)"/>
    /// </summary>
    public class Page
    {
        private bool genned = false;
        private ItemID[] p;

        private HtmlNode Main { get; set; }
        private HtmlNode PostNode { get;  set; }

        /// <summary>
        /// Link to the direct page
        /// </summary>
        public string Link { get; private set; }
        /// <summary>
        /// The link with no offset, i.e. 'offset='
        /// </summary>
        public string LinkNoOffset { get; private set; }
        /// <summary>
        /// Collected ID's from this Page
        /// </summary>
        public ItemID[] PostIDs
        {
            get
            {
                if (!genned)
                {
                    genned = true;
                    p = GetIDs().ToArray();
                    return p;
                }
                else
                    return p;
            }
        }


        internal Page(string pg, string noOffset)
        {
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(pg);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Main = doc.DocumentNode;
            
            PostNode = Main.SelectSingleNode("/html/body/section/div/div/div[1]");

            LinkNoOffset = noOffset;
            Link = pg;
        }

        /// <summary>
        /// Move to another page
        /// </summary>
        /// <param name="pg"></param>
        /// <returns></returns>
        public Page MoveTo(int pg)
        {
            return new Page($"{LinkNoOffset}{pg * 12}", LinkNoOffset);
        }

        private IEnumerable<ItemID> GetIDs()
        {
            for (int i = 2; i < 14; i++)
            {
                HtmlNode node = PostNode.SelectSingleNode($"div[{i}]/div[1]/a[1]");

                if (node is null)
                    break;

                string id = node.GetAttributeValue("href", "").GetID();

                yield return new ItemID(id, ItemID.Type.Post);
            }
        }
    }
}
