using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons.DL;
using System.Data.SqlClient;

namespace AutoMate
{
    class AutoMeta
    {
        private bool _isNew = true;
        private int _id = -1; public int ID { get { return _id; } set { _id = value; } }
        private int _postID = -1; public int PostID { get { return _postID; } set { _postID = value; } }
        private string _contentType = "NOCONTENT"; public string ContentType { get { return _contentType; } set { _contentType = value; } }
        private string _contentFromTitle = "NOCONTENT"; public string ContentFromTitle { get { return _contentFromTitle;} set { _contentFromTitle = value; } }
        private int _contentConfidence = 0; public int ContentConfidence { get { return _contentConfidence; } set { _contentConfidence = value; } }
        private string _topCommentOnSource = "NOCOMMENT"; public string TopCommentOnSource { set { _topCommentOnSource = value; } get { return _topCommentOnSource; } }
        private string _contentSubtype = "NOSUBTYPE"; public string ContentSubtype { get { return _contentSubtype; } set { _contentSubtype = value; } }
        private int _OCConfidence = 0; public int OCConfidence { get { return _OCConfidence; } set { _OCConfidence = value; } }
        public AutoMeta()
        {
            _isNew = true;
        }
        public AutoMeta(int id)
        {
            _isNew = false;
        }
        public void Save()
        {
            if (_isNew)
            {
                DBManager dbm = new DBManager("local");
                string _query = @"insert into CS_AutoMeta (PostID, ContentType,ContentConfidence,ContentFromTitle,TopCommentOnSource,ContentSubtype,OCConfidence)
            values (" + PostID + ",'" + ContentType + "'," + ContentConfidence + ",'" + ContentFromTitle + "','" + TopCommentOnSource + "','" + ContentSubtype + "'," + OCConfidence + ")";
                dbm.ExecuteQuery(_query);
                dbm.Kill();
            }
        }
    }
}
