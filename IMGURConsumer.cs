using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgurDotNet;

namespace AutoMate
{
    public class IMGURDec
    {
        public bool IsAlbum = false;
        public string[] Titles = { "NOTITLES" };
        public string[] Descs = { "NODESCS" };
        public string[] FullURLs = { "NOURLS"};
        public string ContextURL = "NOCONTEXTURL";
        public string[] FileTypes = { "NOFILETYPE"};
        public string[] FileNames = { "NOFILEName" };
    }
    public class IMGURConsumer
    {
        public static IMGURDec GetImages(string contextURL)
        {
            IMGURDec _dec = new IMGURDec();
            _dec.ContextURL = contextURL;
            string[] _ext = Tools.GetFileExtension(contextURL);
            ImgurDotNet.Imgur _gurl = new Imgur();
            bool _isImage = false;
            try { ImgurImage _img = _gurl.GetImage(_ext[0]); _isImage = true; }
            catch (Exception ex) { }
            if(!_ext[1].Equals("=(")||_isImage)
            {
                if (_isImage)
                {
                    //is image
                    ImgurImage _img = _gurl.GetImage(_ext[0]);
                    _dec.FullURLs = new string[] { _img.OriginalUrl.ToString() };
                    _dec.IsAlbum = false;
                    _dec.Titles = new string[] { _img.Title };
                    _dec.Descs = new string[] { _img.Name };
                    if (!_ext[1].Equals("=("))
                    {
                        _dec.FileTypes = new string[] { _ext[1] };
                        _dec.FileNames = new string[] { _ext[0] + "." + _ext[1] };

                        if (!_ext[1].Equals("gif") && !_ext[1].Equals("gifv"))
                        {
                            string[] _mime = _img.Type.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            if (!_mime[_mime.Length - 1].Equals(_dec.FileTypes))
                            {
                                _dec.FileTypes = new string[] { _mime[_mime.Length - 1] };
                            }
                        }
                    }
                    else
                    {
                        string[] _ext0 = Tools.GetFileExtension(_img.OriginalUrl.ToString());
                        _dec.FileTypes = new string[] { _ext0[1] };
                        _dec.FileNames = new string[] { _ext0[0] + "." + _ext0[1] };
                    }
                    
                }
                else
                {
                    _dec.FullURLs = new string[] { contextURL};
                    _dec.IsAlbum = false;
                    _dec.Titles = new string[] { "NOTITLE"};
                    _dec.FileTypes = new string[] { _ext[1] };
                    _dec.FileNames = new string[] { _ext[0] + "." + _ext[1] };
                    _dec.Descs = new string[] { "NODESC" };
                }
            }
            else
            {
                ImgurAlbum _alb = _gurl.GetAlbum(_ext[0]);
                string[] _fullURLS= new string[_alb.Images.Count];
                string[] _titles = new string[_alb.Images.Count];
                string[] _descs = new string[_alb.Images.Count];
                string[] _fileTypes = new string[_alb.Images.Count];
                string[] _fileNames = new string[_alb.Images.Count];
                int _count = 0;
                foreach(ImgurImage _img in _alb.Images)
                {
                    string[] _ext2 = Tools.GetFileExtension(_img.OriginalUrl.ToString());
                    _fullURLS[_count] = _img.OriginalUrl.ToString();
                    _titles[_count] = _img.Title;
                    _descs[_count] = _img.Caption;
                    _fileTypes[_count] = _ext2[1];
                    _fileNames[_count] = _ext2[0] + "." + _ext2[1];
                    if (!_ext[1].Equals("gif") && !_ext2[1].Equals("gifv"))
                    {
                        string[] _mime = _img.Type.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        if (!_mime[_mime.Length - 1].Equals(_fileTypes[_count]))
                        {
                            _fileTypes[_count] = _mime[_mime.Length - 1];
                        }
                    }
                    _count++;

                }
                _dec.IsAlbum = true;
                _dec.FullURLs = _fullURLS;
                _dec.Titles = _titles;
                _dec.Descs = _descs;
                _dec.FileTypes = _fileTypes;
                _dec.FileNames = _fileNames;
            }
            return _dec;
        }
    }
}
