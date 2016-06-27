using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.IO;

namespace AutoMate
{
    public class Commons
    {
        public static string[] AllowedFormats = { "jpg","jpeg","png","gif","mp4","gifv","webm"};
        public static int DownloadFile(string source, string destination, string expectedFormat)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(source).Result;
            if (response.IsSuccessStatusCode)
            {
                var filetype = response.Content.Headers.ContentType.MediaType;
                var imageArray = response.Content.ReadAsByteArrayAsync();
                Console.WriteLine("FIleType:" + filetype);
            }
            else return 123;
            return 0;
        }
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public static string[] GetImageLinks(string URL)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {

                var response = client.GetAsync(URL).Result;
               
                string htmlString = response.Content.ReadAsStringAsync().Result;
                
                //int mastercount = 0;
                Regex regPattern = new Regex(@"http://i.imgur.com/(.*?).", RegexOptions.Singleline);
                MatchCollection matchImageLinks = regPattern.Matches(htmlString);
                Console.WriteLine("lol");
                foreach (Match img_match in matchImageLinks)
                {
                    string imgurl = img_match.Groups[1].Value.ToString();
                    Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                    RegexOptions.IgnoreCase);
                    MatchCollection ms = regx.Matches(imgurl);
                    foreach (Match m in ms)
                        Console.WriteLine("Images:" + m.Value);
                }

                return null;
            }
        }
    }
}
