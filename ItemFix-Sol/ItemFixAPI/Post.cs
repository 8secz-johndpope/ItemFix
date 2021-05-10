using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ItemFix
{
    /// <summary>
    /// A Post on ItemFix
    /// </summary>
    public class Post
    {
        private HtmlNode Main { get;  set; }
        private HtmlNode DownloadNode { get;  set; }
        private HtmlNode AuthorNode { get;  set; }
        private HtmlNode Video_Node { get;  set; }

        /// <summary>
        /// The <see cref="User"/> who uploaded this post
        /// </summary>
        public User Author => new User(Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[1]/div/div[2]/span[1]/a[1]")?.GetAttributeValue("href", ""));

        /// <summary>
        /// The name of the uploader
        /// </summary>
        public string AuthorName => AuthorNode.InnerText;
        /// <summary>
        /// The link to the author's page
        /// </summary>
        public string AuthorLink => AuthorNode.GetAttributeValue("href", "");
        /// <summary>
        /// a png to the author's pfp
        /// </summary>
        public string AuthorPfpLink => Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[1]/div/div[1]/span[1]/img")?.GetAttributeValue("src", "");

        /// <summary>
        /// The description
        /// </summary>
        public string Description => Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[1]/div/p")?.InnerText;
        /// <summary>
        /// The amount of views this post has
        /// </summary>
        public string Views => Video_Node.SelectSingleNode("span[2]")?.InnerText.Replace(" ", "");
        /// <summary>
        /// The score of th is post
        /// </summary>
        public string Score => Video_Node.SelectSingleNode("span[3]")?.InnerText.Replace(" ", "");
        /// <summary>
        /// The amount of comments 
        /// </summary>
        public string Comments => Video_Node.SelectSingleNode("span[4]")?.InnerText.Replace(" ", "");
        /// <summary>
        /// How many <see cref="User"/> bookmarked this?
        /// </summary>
        public string Bookmarked => Video_Node.SelectSingleNode("span[5]")?.InnerText.Replace(" ", "");
        /// <summary>
        /// How many <see cref="User"/> shared this?
        /// </summary>
        public string Shared => Video_Node.SelectSingleNode("span[6]")?.InnerText.Replace(" ", "");
        /// <summary>
        /// How many <see cref="User"/> downloaded this?
        /// </summary>
        public string Downloads => Video_Node.SelectSingleNode("span[7]")?.InnerText.Replace(" ", "");
        /// <summary>
        /// A link to the download this post
        /// </summary>
        public string DownloadLink => "https://www.itemfix.com/" + DownloadNode.GetAttributeValue("href", "");

        /// <summary>
        /// The title 
        /// </summary>
        public string Title => Main.SelectSingleNode("/html/body/section/div/div/div[1]/h1/text()")?.InnerText.Replace("&nbsp;", "").Trim();
        /// <summary>
        /// How long does this video? last
        /// </summary>
        public TimeSpan Duration => TimeSpan.Parse(Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[6]/div[2]")?.InnerText.Trim());

        /// <summary>
        /// an ID to refer back to this post
        /// </summary>
        public ItemID PostID => new ItemID(Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[3]/div[2]")?.InnerText.Replace(" Copy", ""), ItemID.Type.Post);

        internal Post(string pstURL)
        {
            SetUp(pstURL);
            //Author set here
        }

        private void SetUp(string pstURL)
        {
            string html = string.Empty;
            using (WebClient web = new WebClient())
            {
                html = web.DownloadString(pstURL);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Main = doc.DocumentNode;
            DownloadNode = Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[2]/ul/li[2]/a");
            AuthorNode = Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[1]/div/div[1]/span[1]/a");
            Video_Node = Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[1]/div/div[1]");
        }

        /// <summary>
        /// Get a post via <see cref="ItemID"/>
        /// </summary>
        /// <param name="id"></param>
        public Post(ItemID id)
        {
            switch (id.IDType)
            {
                case ItemID.Type.Post:
                    break;
                case ItemID.Type.User:
                    return;
            }
            string pstURL = $"https://www.itemfix.com/v?t={id.ID}";
            SetUp(pstURL);
        }
    }
}
