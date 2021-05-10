using System;

namespace ItemFix
{
    class Program
    {
        static void Main(string[] args)
        {
            Post post = Search.GetPost("oh no my car");
            Console.WriteLine(post.URL);
            Console.WriteLine(post.AuthorPfpLink);
            Console.WriteLine(post.Views);
            //to do 
            //offset is wrong for page? check to see how much the offset should : Search
            //check for errors
        }
    }
}
