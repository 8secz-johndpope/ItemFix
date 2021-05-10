using System;

namespace ItemFix
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = Search.GetUser("ItemFixer");

            Post post = new Post(user.PostIDs[0]);

            Console.WriteLine(post.Title);
            Console.WriteLine(post.Score);
            Console.WriteLine(post.Shared);
            Console.WriteLine(post.Description);
            Console.WriteLine(post.Duration);
            Console.WriteLine(post.Views);
            Console.WriteLine(post.AuthorName);
            Console.WriteLine(post.AuthorPfpLink);
            //to do 
            //offset is wrong for page? check to see how much the offset should : Search
            //check for errors
        }
    }
}
