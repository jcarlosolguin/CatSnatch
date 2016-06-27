using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons.DL;
using System.Data.SqlClient;
using System.Collections;

namespace AutoMate
{
    public class PostManager
    {
        public static OriginalPostGroup GetListToRank_ONLYIMUGR(string orderby, int count)
        {
            //for now order by date always!!
            OriginalPostGroup _res = new OriginalPostGroup();
            ArrayList _list = new ArrayList();
            
            DBManager dbm = new DBManager("local");
            string _query = "select top "+count+" _ID from CS_OriginalPost where _ID not in (select PostID from CS_ManualMeta) and FullURL like '%imgur.com%' order by _date asc";
            SqlDataReader _sqr = dbm.ExecuteQuery(_query);
            int i = 0;
            while(_sqr.Read())
            {
                OriginalPost op = new OriginalPost(int.Parse(_sqr[0].ToString()));
                op.LocalIMGURL ="re_srcs"+ op.LocalIMGURL.Split(new string[]{"re_srcs"},StringSplitOptions.RemoveEmptyEntries)[1];
                 op.LocalIMGURL = op.LocalIMGURL.Replace(".gif",".mp4");
                _list.Add(op );
                i++;
            }
            _res.Kitties =(OriginalPost[]) _list.ToArray(typeof(OriginalPost));
            return _res;
        }

        public static KittyViewGroup GetKitties(string orderby, int count, string category)
        {
            KittyViewGroup _res = new KittyViewGroup();
            ArrayList _l = new ArrayList();
            //TESTIN'!!!
            DBManager dbm = new DBManager("local");
            string _query = "select top " + count + " * from CS_OriginalPost order by _ID asc";
            SqlDataReader sqr = dbm.ExecuteQuery(_query);
            while (sqr.Read())
            {
                if (!sqr["LocalIMGURL"].ToString().Equals(""))
                {
                    string[] _imgParts = sqr["LocalIMGURL"].ToString().Split(new string[] { "re_srcs" }, StringSplitOptions.RemoveEmptyEntries);
                    string __imgURL = "re_srcs" + _imgParts[1];
                    KittyView k = new KittyView();
                    k.Abstract = sqr["Description"].ToString();
                    k.Description = sqr["Description"].ToString();
                    k.Category = "TETSIN'";
                    k.ID = int.Parse(sqr["_ID"].ToString());
                    k.LocalIMGURL = __imgURL;
                    k.IMGURL=sqr["IMGURL"].ToString();
                    k.Title = sqr["Title"].ToString();
                    k.FileType=sqr["FileType"].ToString();
                    _l.Add(k);
                }
            }
            //TESTIN'!!!
            _res.Kitties = (KittyView[])_l.ToArray(typeof(KittyView));
            return _res;
        }
    }
}
