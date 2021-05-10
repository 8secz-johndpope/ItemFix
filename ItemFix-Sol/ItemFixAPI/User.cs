using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ItemFix
{
    /// <summary>
    /// Represents an existing user on ItemFix.com
    /// </summary>
    public class User
    {
        private bool collected = false;
        private ItemID[] _id;

        private HtmlNode Main { get; set; }
        private HtmlNode PostNode { get; set; }

        /// <summary>
        /// The amount of Subscribers this <see cref="User"/> has. Formatted
        /// </summary>
        public string Subscribers => Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[2]/div[3]/span[2]")?.InnerText.Trim();

        /// <summary>
        /// The amount of Views that this <see cref="User"/> has
        /// </summary>
        public string Views => Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[2]/div[3]/span[1]/strong")?.InnerText.Trim();

        /// <summary>
        /// The name of the <see cref="User"/>
        /// </summary>
        public string Name => Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[2]/div[1]/h1")?.InnerText.Trim();
        /// <summary>
        /// The direct link to the pfp file
        /// </summary>
        public string PfpLink => Main.SelectSingleNode("/html/body/section/div/div/div[1]/div[1]/img")?.GetAttributeValue("src", "");

        internal User(string usrURL)
        {
            Setup(usrURL, false);
        }

        /// <summary>
        /// Get a <see cref="User"/> via <seealso cref="ItemID"/>
        /// </summary>
        /// <param name="userID"></param>
        public User(ItemID userID)
        {
            switch (userID.IDType)
            {
                case ItemID.Type.Post:
                    return;
                case ItemID.Type.User:
                    {
                        Setup($"https://www.itemfix.com/c/{userID.ID}", false);
                        return;
                    }
                case ItemID.Type.UserToken:
                    {
                        Setup($"https://www.itemfix.com/{userID.ID}", true);

                        return;
                    }
                case ItemID.Type.None:
                    return;
            }

        }

        /// <summary>
        /// An array of ID's that are for <see cref="Post"/>. Limit is 12 or less
        /// </summary>
        public ItemID[] PostIDs
        {
            get
            {
                if (!collected)
                {
                    collected = true;
                    ItemID[] p = CollectPostIDs().ToArray();
                    _id = p;
                    return p;
                }
                else
                    return _id;
            }
        }

        /// <summary>
        /// The User Token, helps search for specific offets...
        /// </summary>
        public string UserToken { get; private set; }

        private void Setup(string usrURL, bool isUserToken)
        {
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(usrURL);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Main = doc.DocumentNode;

            if (!isUserToken)
                PostNode = Main.SelectSingleNode("/html/body/section/div/div/div[2]");
            else
            {
                PostNode = Main.SelectSingleNode("/html/body/section/div/div/div[1]");
            }

            string s = string.Empty;
            Main.Descendants().FirstOrDefault(x => (s = x.GetAttributeValue("href", "")).Contains("list?user_token="));

            int z = s.GetUserTokenOffset();

            if(z == -1)
            {
                UserToken = null;
                return;
            }

            UserToken = s.Remove(s.Length - z.GetNumCount());
        }

        /// <summary>
        /// Gives a specific page of the user via an ID
        /// </summary>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ItemID MoveTo(int pg)
        {
            if (pg < 0 || UserToken == null)
                return ItemID.None;

            int offset = pg * 12;

            string link = $"{UserToken}{offset}";

            return new ItemID(link, ItemID.Type.UserToken);
        }

        private IEnumerable<ItemID> CollectPostIDs()
        {
            for (int i = 1; i < 13; i++)
            {
                HtmlNode upload = PostNode.SelectSingleNode($"div[{i}]");

                if (upload is null)
                    break;

                string id = upload.SelectSingleNode("div[1]/a[1]").GetAttributeValue("href", "").GetID();

                yield return new ItemID(id, ItemID.Type.Post);
            }
        }
    }
}
