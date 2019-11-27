using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles { get; }

        void SaveArticle(Article article);

        void DeleteArticle(Article article);

        string GetShortName(string name, int maxID);

        Article GetArticleByShortName(string shortName);

        void RefreshAllArticlesShortNames();

        // Task<IEnumerable<Article>> GetArticleListAsync();


    }
}
