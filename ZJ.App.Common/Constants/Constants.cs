using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJ.App.Common.Constants
{
    public class Constants
    {
        /// <summary>
        ///  对应 ArticleCategory id, 对应文章类别 CategoryId
        /// </summary>
        public static  Dictionary<string, int> ChannelToCategory = new Dictionary<string, int>
        {
            { "jt",1 }, //集团新闻
            { "gs",2 }, //公司新闻
            { "yw",3}, //业务动态(外企服务公司-6;关爱通公司-7,培训部-8,工会联合会-9)
            { "yw_wq",6}, //外企服务公司-6;关爱通公司-7,培训部-8,工会联合会-9)
            { "yw_gat",7},
            { "yw_px",8},
            { "yw_gh",9},
            { "jiaojuguoqi",4}, //聚焦国企
            { "jiaojuguoqi_guozhiyaowen",10},//国资要闻
            { "jiaojuguoqi_gaigeqianyan",11},//改革前沿
            { "yw_rcgw",12},//改革前沿//TODO 增加文章类别
            { "yw_kc",13},//科创公司//TODO 增加文章类别
            { "yw_dw",14},//党委
            { "yw_flsw",15},//法律事务部
            { "yw_pxzx",16}  //培训中心
        };
    }
}
