using Blend.Cms12.Models.Pages;
using Blend.ContentIndex;
using EPiServer;
using EPiServer.Core;
using System;
using System.Collections.Generic;

namespace Blend.Cms12.Business.ContentIndex
{
    public class OptimizelyContentIndexService
    {
        private readonly IndexService indexService;
        private readonly IContentLoader loader;

        public OptimizelyContentIndexService(IndexService indexService, IContentLoader loader)
        {
            this.indexService = indexService;
            this.loader = loader;
        }
        
        public void ReindexTree(PageData startPage, PageTreeUpdateContext context)
        {
            Stack<PageData> stack = new Stack<PageData>();
            stack.Push(startPage);

            while (stack.Count > 0)
            {
                if (!context.ShouldContinue)
                    break;

                var page = stack.Pop();

                ProcessPage(page);
                context.Pages += 1;

                context.OnStatusChanged?.Invoke($"Processed {context.Pages} pages.");

                var childPages = loader.GetChildren<PageData>(page.ContentLink, page.Language);
                foreach (var child in childPages)
                {
                    stack.Push(child);
                }
            }
        }

        private void ProcessPage(PageData page)
        {
            if (!(page is IHaveContent indexable))
                return;

            var indexBuilder = new IndexBuilder(page.ContentLink.ID.ToString(), page.Language.Name);
            indexable.BuildIndex(indexBuilder);
            indexService.Update(indexBuilder);
        }

        public void Delete(ContentReference contentLink)
        {
            var query = new ContentQuery()
                .WhereMatch("Ancestors", contentLink.ID.ToString());
            indexService.Delete(query);
        }

        public void DeleteAll()
        {
            indexService.Delete(new ContentQuery());
        }
    }

    public class PageTreeUpdateContext
    {
        public int Pages { get; set; }
        public bool ShouldContinue { get; set; } = true;
        public Action<string>? OnStatusChanged { get; set; }
    }
}
