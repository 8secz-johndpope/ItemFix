using System;
using System.Collections.Generic;
using System.Text;

namespace ItemFix
{
    /// <summary>
    /// Allows you to search for <see cref="Page"/>, <seealso cref="Post"/>, and <seealso cref="User"/>
    /// </summary>
    public static class Search
    {
        /// <summary>
        /// The media Type
        /// </summary>
        [Flags]
        public enum Media
        {
            /// <summary>
            /// Represents nothing
            /// </summary>
            None = 0,
            /// <summary>
            /// Only Images
            /// </summary>
            Images = 1,
            /// <summary>
            /// Only Audio
            /// </summary>
            Audio = 2,
            /// <summary>
            /// Only Videos
            /// </summary>
            Videos = 4,
        }

        /// <summary>
        /// </summary>
        [Flags]
        public enum Type
        {
            /// <summary>
            /// Represents nothing
            /// </summary>
            None = 0,
            /// <summary>
            /// Represents only Items
            /// </summary>
            Items = 1,
            /// <summary>
            /// Represents only Fixes
            /// </summary>
            Fixes = 2
        }

        /// <summary>
        /// 
        /// </summary>
        public enum Order
        {
            /// <summary>
            /// The relavance -> Date and Virality
            /// </summary>
            Relevance,
            /// <summary>
            /// Virality of the Fix
            /// </summary>
            Virality,
            /// <summary>
            /// How new is this fix
            /// </summary>
            Newest
        }

        /// <summary>
        /// Gets the first post on the <see cref="Page"/>
        /// </summary>
        /// <param name="query">The query to use on ItemFix</param>
        /// <param name="page">The page to get, * 12 to get page offset</param>
        /// <param name="order"></param>
        /// <param name="media">The Media Type</param>
        /// <param name="type"></param>
        /// <param name="from">Duration from 0-></param>
        /// <param name="to">Duration To 3></param>
        /// <returns>A Page full of Post</returns>
        public static Page GetPage(string query, int page = 0, Order order = Order.Relevance, Media media = Media.None, Type type = Type.None, int from = 0, int to = 180)
        {
            string pgLink = ConstructQuery(query, order, from, to, media, type);

            return new Page(pgLink += $"{page * 12}", pgLink);
        }
        #region Page
        private static string ConstructQuery(string searchItem, Order order, int from = 0, int to = 180, Media media = Media.None, Type type = Type.None)
        {
            if (from < 0)
                from = 0;

            if (to < 0)
                to = 180;

            if (string.IsNullOrWhiteSpace(searchItem))
            {
                string basic = $"https://www.itemfix.com/list?q=&order_by={order}&duration_from={from}&duration_to={to}";

                basic = GiveFlags(basic, type, media);
                basic += $"&offset=";

                return basic;
            }
            else
            {
                searchItem = AppendQuery(searchItem);

                string basic = $"https://www.itemfix.com/list?q={searchItem}&order_by={order}&duration_from={from}&duration_to={to}";

                basic = GiveFlags(basic, type, media);
                basic += $"&offset=";

                return basic;
            }
        }
        private static string GiveFlags(string existing, Type type, Media media)
        {
            string basic = existing;

            if (type.HasFlag(Type.Items))
                basic += "&type_array%5B%5D=item";

            if (type.HasFlag(Type.Fixes))
                basic += "&type_array%5B%5D=fix";


            if (media.HasFlag(Media.Videos))
                basic += "&media_type_array%5B%5D=video";

            if (media.HasFlag(Media.Images))
                basic += "&media_type_array%5B%5D=image";

            if (media.HasFlag(Media.Audio))
                basic += "&media_type_array%5B%5D=audio";


            return basic;
        }
        private static string AppendQuery(string a)
        {
            return a.Replace(" ", "+");
        }
        #endregion

        /// <summary>
        /// Get a <see cref="User"/> by <seealso cref="ItemID"/> | <seealso cref="User.User(ItemID)"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static User GetUser(ItemID id)
        {
            switch (id.IDType)
            {
                case ItemID.Type.Post:
                    return null;
                case ItemID.Type.User:
                    break;
                case ItemID.Type.UserToken:
                    break;
                case ItemID.Type.None:
                    return null;
            }

            return new User(id);
        }
        /// <summary>
        /// Get a <see cref="User"/> by their name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static User GetUser(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return new User($"https://www.itemfix.com/c/{name}");
        }

        /// <summary>
        /// Gets the first post on the <see cref="Page"/>
        /// </summary>
        /// <param name="search">The query to use on ItemFix</param>
        /// <param name="page">The page to get, * 12 to get page offset</param>
        /// <param name="order"></param>
        /// <param name="media">The Media Type</param>
        /// <param name="type"></param>
        /// <param name="from">Duration from 0-></param>
        /// <param name="to">Duration To 3></param>
        /// <returns>A Single Post</returns>
        public static Post GetPost(string search, int page = 0, Order order = Order.Relevance, Media media = Media.None, Type type = Type.None, int from = 0, int to = 180)
        {
            return new Post(GetPage(search, page, order, media, type, from, to).PostIDs[0]);
        }

        /// <summary>
        /// Gets a <see cref="Post"/> by <seealso cref="ItemID"/> | <seealso cref="Post.Post(ItemID)"/>
        /// </summary>
        /// <param name="id">The ID to use</param>
        /// <returns></returns>
        public static Post GetPost(ItemID id)
        {
            switch (id.IDType)
            {
                case ItemID.Type.Post:
                    break;
                case ItemID.Type.User:
                    return null;
                case ItemID.Type.UserToken:
                    return null;
                case ItemID.Type.None:
                    return null;
            }

            return new Post(id);
        }

    }
}
