using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFArticleRepository : IArticleRepository
    {
        private RegNumDBContext context;

        public EFArticleRepository(RegNumDBContext context)
        {
            this.context = context;
        }

        public IQueryable<Article> Articles
        {
            get { return context.Articles; }
        }

        public void SaveArticle(Article article)
        {
            article.UpdateDate = DateTime.Now;

            if (article.ArticleID == 0)
            {
                article.ShortLink = GetShortName(article.Header, context.Articles.Max(x => x.ArticleID) + 1);
                context.Articles.Add(article);
            }
            else
            {
                article.ShortLink = GetShortName(article.Header, context.Articles.Max(x => x.ArticleID) + 1);
                context.Entry(article).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteArticle(Article article)
        {
            context.Articles.Remove(article);
            context.SaveChanges();
        }


        public string GetShortName(string name, int maxID)
        {

            //EFDbContext ef = new EFDbContext();
            {
                string s = Constants.TransliterateText(name);
                if (context.Articles.Any(x => x.ShortLink == s && x.ArticleID != maxID))
                {
                    return s + maxID.ToString();
                }
                return s;
            }

        }

        public void RefreshAllArticlesShortNames()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var article in context.Articles)
                {
                    article.ShortLink = GetShortName(article.Header, article.ArticleID);
                    context.Entry(article).State = EntityState.Modified;
                }
                context.SaveChanges();
            }

        }

        //public async Task<IEnumerable<Article>> GetArticleListAsync()
        //{
        //    return await context.Articles.ToListAsync().ConfigureAwait(false);
        //}


        public Article GetArticleByShortName(string shortName)
        {
            throw new NotImplementedException();
        }


    }
}
