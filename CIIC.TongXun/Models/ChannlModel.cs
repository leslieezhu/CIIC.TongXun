using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZJ.App.Entity;

namespace CIIC.TongXun
{
    /// <summary>
    /// 频道模型
    /// </summary>
    public class ChannlModel
    {
        /// <summary>
        /// 该频道下的所有类别
        /// </summary>
        public List<ArticleCategoryEntity> categories { get; private set; }
        public List<ArticleEntity> articles { get; private set; }
        public ChannlModel(List<ArticleCategoryEntity> categories, List<ArticleEntity> articles)
        {
            this.categories = categories;
            this.articles = articles;
        }
    }
}