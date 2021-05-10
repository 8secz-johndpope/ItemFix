# ItemFix
An API for Item Fix, allows you collect information on User, Post, and Pages

## How To Use

Basics

```csharp

//A page is kinda like a container that has ItemID[] for Post.

//                         query    page    search type            search media type                   search item types              From 0seconds to 3mins
Page page = Search.GetPage("Dogs!", page:0, order:Order.Relevance, media: Media.Videos | Media.Images, type: Type.Items | Type.Fixes, from:0, to:180);

//we can get post from this page just like this

//Gets the first post on said page
Post post = new Post(page.PostIDs[0]);

//we can then get the user 
User user = post.Author;

//This is just simple surfing, we can even move pages on some Items.

//This moves to a different offset.
Page page2 = page.MoveTo(2);

//This moves to a page of the user, but is represented via id.
ItemID userID = user.MoveTo(3);

//we then can set said user to be on page 3 as shown above
user = new User(userID);
```
