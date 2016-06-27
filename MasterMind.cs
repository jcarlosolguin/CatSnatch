using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons.DL;
using System.Net;
using System.Configuration;
using System.Web;

namespace AutoMate
{
    class Tools
    {
        public static string GetRoot()
        {
            string _root = "";
            string isweb=ConfigurationManager.AppSettings["isweb"];
            if(isweb.Equals("true"))
            {
                _root=Central.Server.MapPath("~")+ConfigurationManager.AppSettings["webroot"];
            }
            else
                _root = ConfigurationManager.AppSettings["root"];
            return _root;
        }
        public static string DownloadFile(string url,string destination)
        {

            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(destination);
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                using (var client = new WebClient())
                {
                    //if ((url.IndexOf(".gif") < 0)) 
                    client.DownloadFile(url, destination);
                }
                if (url.IndexOf("imgur.com") >= 0)
                {
                    if ((url.IndexOf(".gifv") >= 0))
                    {
                        url = url.Replace(".gifv", ".mp4"); destination = destination.Replace(".gifv", ".mp4");
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(url, destination);
                        }
                    }
                    if ((url.IndexOf(".gif") >= 0))
                    {
                        url = url.Replace(".gif", ".mp4"); destination = destination.Replace(".gif", ".mp4");
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(url, destination);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                return "UNDOWNLABLE:"+ex.Message;
            }
            
            return "SUCCESS";
        }
        public static string[] GetFileExtension(string url)
        {
            string[] _res = new string[2];
            string _ext = "";
            foreach (string s in Commons.AllowedFormats)
            {
                if (url.IndexOf("." + s) >= 0) { _ext = s; break; }
            }
            if (!_ext.Equals(""))
            {
                string[] _parts = url.Split(new char[] { '.' });
                _res[1] = _ext;
                string[] _parts2 = _parts[_parts.Length - 2].Split(new char[] { '/' });
                _res[0] = _parts2[_parts2.Length - 1];
                return _res;
            }
            else
            {
                _res[0] = "=(";
                _res[1] = "=(";
                string[] _parts2 = url.Split(new char[] { '/' },StringSplitOptions.RemoveEmptyEntries);
                _res[0] = _parts2[_parts2.Length - 1];
               // Console.WriteLine("Getting album id:"+_res[0]);
                return _res;
            }
        }
        public static string[] GetFileExtensionIMGUR(string url)
        {
            string[] _res = new string[2];
            string _ext = "";
            foreach (string s in Commons.AllowedFormats)
            {
                if (url.IndexOf("." + s) >= 0) { _ext = s; break; }
            }
            if (!_ext.Equals(""))
            {
                string[] _parts = url.Split(new char[] { '.' });
                _res[1] = _ext;
                string[] _parts2 = _parts[_parts.Length - 2].Split(new char[] { '/' });
                _res[0] = _parts2[_parts2.Length - 1];
                return _res;
            }
            else
            {
                _res[0] = "=(";
                _res[1] = "=(";
                string[] _parts2 = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                _res[0] = _parts2[_parts2.Length - 1];
                if (_res[0].ToLower().Equals("new")) _res[0] = _parts2[_parts2.Length - 2];
                // Console.WriteLine("Getting album id:"+_res[0]);
                return _res;
            }
        }
        public static string ContainsString(string where, string what)
        {
            where = where.ToLower();
            if (where.IndexOf(what) >= 0) return what;
            return "=(";
        }
        public static string ContainsString(string where, string[] what)
        {
            where = where.ToLower();
            foreach (string s in what)
                if (where.IndexOf(s) >= 0) return s;
            return "=(";
        }
    }
    class MasterMind
    {
        public static string[] dogWords={"dog","dogg","pup", "husk","chow","shepperd","retriever","labrador" ,"bulldog","yorkshire","poodle","rottweiler","doberman","schnauzer","terrier","chihuahua"};
        public static string[] catWords={"cat","kitt", "pus","meow"};
        public static string[] animalWords={"paw"};
        public static string[] OCWords={"my","meet"};
        public static void Archive(OriginalPost post)
        {
            if(post.Site.Equals("www.reddit.com"))_archiveRedditPost(post);
        }
        private static void _archiveRedditPost(OriginalPost post)
        {
            AutoMeta _meta= new AutoMeta();
            _meta.PostID=post.ID;
            string[] _content=_checkContent(post);if(_content!=null){_meta.ContentFromTitle=_content[0];_meta.ContentSubtype=_content[1];_meta.ContentConfidence=5;}
            _meta.OCConfidence+=_checkOCLevel(post);
            _meta.Save();
        }
        private static int _checkOCLevel(OriginalPost post)
        {
            string _ocWORDS=Tools.ContainsString(post.Title,OCWords);
            if(!_ocWORDS.Equals("=("))return 5;
            return 0;
        }
        private static string[] _checkContent(OriginalPost post)
        {
            string[] _res={"",""};
            bool _hasCat=false;bool _hasDog=false;bool _hasAnimal=false;
            string _cat="";string _dog="";
            _cat=Tools.ContainsString(post.Title,catWords);
            _dog=Tools.ContainsString(post.Title,dogWords);
            _hasCat=!_cat.Equals("=(");
            _hasDog=!_dog.Equals("=(");
            if(_hasCat&&_hasDog){_res[0]="CAT_DOG";_res[1]=_cat+"_"+_dog;return _res;}
            if(_hasCat){_res[0]="CAT";_res[1]=_cat;return _res;}
            if(_hasDog){_res[0]="DOG";_res[1]=_dog;return _res;}
            string _other=Tools.ContainsString(post.Title,animalWords);
            if(!_other.Equals("=(")){_res[0]="ANIMAL";_res[1]=_other; return _res;}
            return null;
        }
    }
}
