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
            { "nocategory",0},
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
            { "yw_pxzx",16},  //培训中心
            { "yw_xmwb",17} //项目外包
        };


        //TODO 判断  categoryKey 是否存在
        public static string articleDetialImg_YW = "tx_35.png";
        /// <summary>
        /// 详细页上的图标
        /// </summary>
        public static Dictionary<string, string> ListImage = new Dictionary<string, string>
            {
                { "nocategory",""},
                { "jt","tx_25.png" }, //集团新闻
                { "gs","tx_33.png" }, //公司新闻
                { "yw","tx_35.png"}, //业务动态
                { "jiaojuguoqi","jujiaoguoqi.png"},//聚焦国企
                { "jiaojuguoqi_guozhiyaowen","guoziyaowen.png"},//国资要闻
                { "jiaojuguoqi_gaigeqianyan","gaigeqianyan.png"},//改革前沿
                { "yw_wq",articleDetialImg_YW},
                { "yw_gat",articleDetialImg_YW},
                { "yw_px",articleDetialImg_YW},
                { "yw_gh",articleDetialImg_YW},
                { "yw_rcgw",articleDetialImg_YW},//TODO 增加文章类别+Constants.cs
                { "yw_kc",articleDetialImg_YW},
                { "yw_dw",articleDetialImg_YW},
                { "yw_flsw",articleDetialImg_YW},
                { "yw_pxzx",articleDetialImg_YW},  //培训中心
                { "yw_xmwb",articleDetialImg_YW} //项目外包
            };
    }
}
