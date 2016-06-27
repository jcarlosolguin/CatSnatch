using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons.DL;
using System.Data.SqlClient;

namespace AutoMate
{
    class ImageDesc
    {
        private bool _isNew = true;
        private int _id; public int ID { get { return _id; } set { _id = value; } }
        private string _title; public string Title { get { return _title; } set { _title = value.Replace("'", "''"); } }
        private string _description; public string Description { get { return _description; } set { _description = value.Replace("'", "''"); } }
        private string _fullURL; public string FUllURL { get { return _fullURL; } set { _fullURL = value; } }
        private string _site; public string Site { get { return _site; } set { _site = value; } }
        private string _imgURL; public string IMGURL { get { return _imgURL; } set { _imgURL = value; } }
        private string _contextURL; public string ContextURL { get { return _contextURL; } set { _contextURL = value; } }
        private string _localIMGURL; public string LocalIMGURL { get { return _localIMGURL; } set { _localIMGURL = value; } }
       
        private string _fileType; public string FileType { get { return _fileType; } set { _fileType = value; } }
        public int ParentPostID { get; set; }
        public  ImageDesc()
        {
            _isNew = true;
            _id = -1;
            
            Title = "NOTITLE";
            Description = "NODESC";
        }
        public  ImageDesc(int ID):base()
        {
            _isNew = false;
            DBManager dbm = new DBManager("local");
            string _query = "select * from CS_OriginalPost where _ID="+ID;
            SqlDataReader sqr = dbm.ExecuteQuery(_query);
            if(sqr.Read())
            {
                this.ID=ID;
                this.ContextURL=sqr["ContextURL"].ToString();
                this.Description=sqr["Description"].ToString();
                this.FileType=sqr["FileType"].ToString();
                this.FUllURL=sqr["FullURL"].ToString();
                this.IMGURL=sqr["IMGURL"].ToString();
                this.LocalIMGURL=sqr["LocalIMGURL"].ToString();
                this.Site=sqr["Site"].ToString();
                this.Title=sqr["Title"].ToString();
            }
            sqr.Close();
            dbm.Kill();

        }
        public void Save()
        {
            if (_isNew)
            {
                DBManager dbm = new DBManager("local");
                string _query = @"insert into CS_ImageDesc (Title,Description,FullURL, Site, IMGURL, ContextURL,LocalIMGURL,FileType,ParentPostID)
                values('" + Title + "','" + Description + "','" + FUllURL + "','" + Site + "','" + IMGURL + "','" + ContextURL + "','" + LocalIMGURL + "','" + FileType + "'," + ParentPostID + "); select SCOPE_IDENTITY() as ScopeIdentity;";
                SqlDataReader sqr=dbm.ExecuteQuery(_query);
                if (sqr.Read())
                {
                    ID = int.Parse(sqr[0].ToString());
                    _isNew = false;
                }
                sqr.Close();
                dbm.Kill();
            }
            else
            {
                DBManager dbm = new DBManager("local");
                string _query = @"update CS_ImageDesc set Title='" + Title + "',Description='" + Description + "',FullURL='" + FUllURL + "', Site='" + Site + "', IMGURL='" + IMGURL + "', ContextURL='" + ContextURL + "',LocalIMGURL='" + LocalIMGURL + "',FileType='" + FileType + "',ParentPostID=" + ParentPostID + " where _ID=" + ID;

                dbm.ExecuteNonQuery(_query);
                dbm.Kill();
            }
        }
    }
}
