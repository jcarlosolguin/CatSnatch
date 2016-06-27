using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons.DL;
using System.Data.SqlClient;


namespace AutoMate
{
    public class OriginalPostGroup
    {
        public OriginalPost[] Kitties { get; set; }
    }
    public class OriginalPost
    {
        private bool _isNew = true;
        private int _id; public int ID { get { return _id; } set { _id = value; } }
        private string _title; public string Title { get { return _title; } set { if (value != null)_title = value.Replace("'", "''"); } }
        private string _description; public string Description { get { return _description; } set { if(value!=null)_description = value.Replace("'", "''"); } }
        private string _fullURL; public string FUllURL { get { return _fullURL; } set { _fullURL = value; } }
        private string _site; public string Site { get { return _site; } set { _site = value; } }
        private string _imgURL; public string IMGURL { get { return _imgURL; } set { _imgURL = value; } }
        private string _contextURL; public string ContextURL { get { return _contextURL; } set { _contextURL = value; } }
        private string _localIMGURL; public string LocalIMGURL { get { return _localIMGURL; } set { _localIMGURL = value; } }
        private int _nLikes; public int NLikes { get { return _nLikes; } set { _nLikes = value; } }
        private string _opName; public string OPName { get { return _opName; } set { _opName = value; } }
        private string _opUserLink; public string OPUserLink { get { return _opUserLink; } set { _opUserLink = value; } }
        private DateTime _date; public DateTime Date { get { return _date; } set { _date = value; } }
        private string _fileType; public string FileType { get { return _fileType; } set { _fileType = value; } }
        public int ParentPostID { get; set; }
        public bool HasAlbum { get; set; }
        public string IDOnSource { get; set; }
        public string ResourceError { get; set; }
        public  OriginalPost()
        {
            _isNew = true;
            _id = -1;
            _nLikes = -1;
            Title = "NOTITLE";
            Description = "NODESC";
            IDOnSource = "NOID";
        }
        public  OriginalPost(int ID):base()
        {
            _isNew = false;
            DBManager dbm = new DBManager("local");
            string _query = "select * from CS_OriginalPost where _ID="+ID;
            SqlDataReader sqr = dbm.ExecuteQuery(_query);
            if(sqr.Read())
            {
                this.ID=ID;
                this.ContextURL=sqr["ContextURL"].ToString();
                this.Date = DateTime.Parse(sqr["_Date"].ToString());
                this.Description=sqr["Description"].ToString();
                this.FileType=sqr["FileType"].ToString();
                this.FUllURL=sqr["FullURL"].ToString();
                this.IMGURL=sqr["IMGURL"].ToString();
                this.LocalIMGURL=sqr["LocalIMGURL"].ToString();
                this.NLikes = int.Parse(sqr["NLikes"].ToString());
                this.OPName=sqr["OPName"].ToString();
                this.OPUserLink=sqr["OPUserLink"].ToString();
                this.Site=sqr["Site"].ToString();
                this.Title=sqr["Title"].ToString();
                this.IDOnSource = sqr["IDOnSource"].ToString();
                this.ResourceError=sqr["ResourceError"].ToString();
            }
            sqr.Close();
            dbm.Kill();

        }
        public static bool rddt_IHazIt(string id)//method for reddit stuff
        {
            DBManager dbm = new DBManager("local");
            string _query = "select _ID from CS_OriginalPost where IDOnSource='"+id+"'";
            SqlDataReader sqr = dbm.ExecuteQuery(_query);
            if (sqr.Read())
            {
                sqr.Close(); dbm.Kill(); return true;
            }
            sqr.Close(); dbm.Kill();
            return false;
        }
        public void Save()
        {
            if (_isNew)
            {
                DBManager dbm = new DBManager("local");
                string _query = @"insert into CS_OriginalPost (Title,Description,FullURL, Site, IMGURL, ContextURL,LocalIMGURL,NLikes,OPName,OPUserLink,_Date,HasAlbum,FileType,ParentPostID,IDOnSource,ResourceError)
                values('" + Title + "','" + Description + "','" + FUllURL + "','" + Site + "','" + IMGURL + "','" + ContextURL + "','" + LocalIMGURL + "'," + NLikes + ",'" + OPName + "','" + OPUserLink + "','" + Date + "'," + (HasAlbum?1:0) + ",'" + FileType + "'," + ParentPostID + ",'"+IDOnSource+"','"+ResourceError+"'); select SCOPE_IDENTITY() as ScopeIdentity;";
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
                string _query = @"update CS_OriginalPost set Title='" + Title + "',Description='" + Description + "',NLikes=" + NLikes + ",OPName='" + OPName + "',OPUserLink='" + OPUserLink + "',_Date='" + Date.ToShortDateString() + " " + Date.ToShortTimeString() + "',HasAlbum=" + (HasAlbum ? 1 : 0) + ",FullURL='" + FUllURL + "', Site='" + Site + "', IMGURL='" + IMGURL + "', ContextURL='" + ContextURL + "',LocalIMGURL='" + LocalIMGURL + "',FileType='" + FileType + "',ParentPostID=" + ParentPostID + ", IDOnSource='"+IDOnSource+"',ResourceError='"+ResourceError+"' where _ID=" + ID;
                
                dbm.ExecuteNonQuery(_query);
                dbm.Kill();
            }
        }
        public object this[string propertyName] 
        {
            get{
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(OriginalPost);                   
                System.Reflection.PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set{
                Type myType = typeof(OriginalPost);                   
                System.Reflection.PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }
    }
}
