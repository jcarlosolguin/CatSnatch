using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using RedditSharp;

namespace AutoMate
{
    public class RedditTask
    {
        public string SubReddit = "NOSUB";
        public DateTime StartDate = DateTime.Now;
        public DateTime EndDate = DateTime.Now;
        public string OrderBy = "HOT";//by def
        public int MaxPosts = 1000;//Maximum number of posts to consume
        public RedditSharp.Things.FromTime TOPTIME = RedditSharp.Things.FromTime.All;
        public RedditTask(string sub, DateTime start, DateTime end, string order, int max, RedditSharp.Things.FromTime toptime) 
        {
            SubReddit = sub;
            StartDate = start;
            EndDate = end;
            OrderBy = order;
            MaxPosts = max;
            TOPTIME = toptime;
        }
        public RedditTask(string sub, DateTime start, DateTime end, string order, int max)
        {
            SubReddit = sub;
            StartDate = start;
            EndDate = end;
            OrderBy = order;
            MaxPosts = max;
        }
        public RedditTask(string sub, string order, int max)
        {
            SubReddit = sub;
            OrderBy = order;
            MaxPosts = max;
        }
    }
    public class RedditConsumer
    {
        private static bool _amIin = false;
        private static ArrayList _tasks= new ArrayList();
        public static void Macro1()
        {
            AddTask(new RedditTask("aww","HOT",100));
            AddTask(new RedditTask("AnimalsBeingBros", "HOT", 100));
            AddTask(new RedditTask("AnimalsBeingJerks", "HOT", 100));
            Consume();
        }
        public static void Macro0()
        {
            AddTask(new RedditTask("aww", "HOT", 1000));
            AddTask(new RedditTask("AnimalsBeingBros", "HOT", 1000));
            AddTask(new RedditTask("AnimalsBeingJerks", "HOT", 1000));
            Consume();
        }
        public static void Macro23()
        {
            AddTask(new RedditTask("aww", "TOP", 1000));
            AddTask(new RedditTask("AnimalsBeingBros", "TOP", 1000));
            AddTask(new RedditTask("AnimalsBeingJerks", "TOP", 1000));
            Consume();
        }
        public static void AddTask(RedditTask task)
        {
            _tasks.Add(task);
        }
        public static void Consume()
        {
            if(!_amIin)
                if(!Login())
                {
                    Console.WriteLine("Cannot login asshole! Fix it!");
                    return;
                }
            foreach(RedditTask task in _tasks)
            {

                RedditSharp.Things.Subreddit _sub=Central.reddit.GetSubreddit(task.SubReddit);
                if(task.OrderBy.ToUpper().Equals("HOT"))_consumePosts(_sub.Hot,task.MaxPosts);
                if (task.OrderBy.ToUpper().Equals("TOP")) _consumePosts(_sub.GetTop(task.TOPTIME),task.MaxPosts);
                if (task.OrderBy.ToUpper().Equals("NEW")) _consumePosts(_sub.New, task.MaxPosts);
                
            }
        }
        private static void _consumePosts(Listing<RedditSharp.Things.Post> posts, int max)
        {
            int i=0;
            int _errors = 0;
            foreach(RedditSharp.Things.Post _post in posts )
            {
                if (i > max||_errors>20) return;
                int _res=_savePost(_post);
                if ( _res== 0) i++;
                if (_res > 1) _errors++;
                
                Console.WriteLine("Consuming:"+_post.Url.ToString());
            }
        }
        private static bool IHasIt(RedditSharp.Things.Post post)
        {
            return OriginalPost.rddt_IHazIt(post.Id);
        }
        private static int _savePost(RedditSharp.Things.Post post)
        {
            if (IHasIt(post)) return 1;//fuck it, cause I am too cool and I procedure
            string __root=Tools.GetRoot();
            OriginalPost op = new OriginalPost();
            op.ContextURL = "http://www.reddit.com/"+post.Permalink.ToString();
            op.Date = post.Created;
            op.Description = post.SelfText;
            op.NLikes=post.Upvotes-post.Downvotes;
            op.OPName=post.AuthorName;
            op.OPUserLink="http://www.reddit.com/user/"+post.AuthorName;
            op.Site="www.reddit.com";
            op.Title=post.Title;
            op.FUllURL = post.Url.ToString();
            op.IDOnSource = post.Id;
            op.Save();
            IMGURDec _dec;  
            if (post.Url.ToString().IndexOf("imgur") >= 0)
            {
                try
                {
                    //is imgur
                    _dec = IMGURConsumer.GetImages(post.Url.ToString());
                }
                catch (Exception ex) { return 2; }
                if (_dec.IsAlbum)
                {
                    op.HasAlbum = true;
                    //ImageDesc[] _images= new ImageDesc[_dec.Titles.Length];
                    for (int i = 0; i < _dec.Titles.Length; i++)
                    {
                        ImageDesc _image = new ImageDesc();
                        _image = new ImageDesc();
                        _image.ContextURL = _dec.ContextURL;
                        _image.Description = _dec.Descs[i];
                        _image.FileType = _dec.FileTypes[i];
                        _image.FUllURL = _dec.FullURLs[i];
                        _image.IMGURL = _dec.FullURLs[i];
                        _image.LocalIMGURL = __root + @"reddit\" + post.SubredditName + @"\imgur\" + _dec.FileNames[i];
                        _image.ParentPostID = op.ID;
                        _image.Site = "www.imgur.com";
                        _image.Title = _dec.Titles[i];
                        _image.Save();
                        Tools.DownloadFile(_image.FUllURL, _image.LocalIMGURL);
                    }
                }
                else
                {
                    op.HasAlbum = false;
                    
                    op.Description = _dec.Descs[0];
                    op.FileType = _dec.FileTypes[0];
                    op.IMGURL = _dec.FullURLs[0];
                    op.LocalIMGURL = __root + @"reddit\" + post.SubredditName + @"\imgur\" + _dec.FileNames[0];
                    
                }

            }
            else
            {
                //non-IMGUR
                string[] _ext2 = Tools.GetFileExtension(post.Url.ToString());

                op.HasAlbum = false;
                op.Description = "NODESC";
                op.FileType = _ext2[1];
                op.IMGURL = post.Url.ToString();
                op.LocalIMGURL = __root + @"reddit\" + post.SubredditName + @"\else\" + _ext2[0]+_ext2[1];
                
            }
            
            if (op.IMGURL != null && op.LocalIMGURL != null)
                op.ResourceError=Tools.DownloadFile(op.IMGURL, op.LocalIMGURL);
            op.Save();
            MasterMind.Archive(op);
            return 0;
        }
        public static bool Login()
        {
            Central.reddit = null;
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];
            try
            {
                Central.reddit = new Reddit(username, password);
                _amIin = Central.reddit.User != null;
                return _amIin;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Incorrect login." + ex.Message);
                return false;
            }
        }
    }

}
