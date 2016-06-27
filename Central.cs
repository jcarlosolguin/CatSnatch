using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditSharp;

namespace AutoMate
{
    public class Central
    {
        public static System.Web.HttpServerUtility Server { get; set; }
        public static Reddit reddit;
        public static void StartMacro(int macro)
        {
            if(macro==1)RedditConsumer.Macro1();
            if (macro == 0) RedditConsumer.Macro0();
            if (macro == 23) RedditConsumer.Macro23();
        }
        public static void StartSubreddit(string sub,string order,int count)
        {
            if (order.ToLower().Equals("top") || order.ToLower().Equals("hot") || order.ToLower().Equals("new"))
            {
                try
                {
                    RedditConsumer.AddTask(new RedditTask(sub, order, count));
                    RedditConsumer.Consume();
                }
                catch (Exception ex) { }
            }
        }
    }
}
