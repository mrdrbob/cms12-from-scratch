using Blend.Cms12.Business.ContentIndex;
using Blend.Cms12.Models.Blocks;
using Blend.Cms12.Models.Blocks.ViewModels;
using Blend.Cms12.Models.Pages;
using Blend.ContentIndex;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blend.Cms12.Controllers.Blocks
{
    public class RelatedPagesBlockController : BlockComponent<RelatedPagesBlock>
    {
        private readonly OptimizelyContentIndexService indexService;
        private readonly IPageRouteHelper pageRouteHelper;
        private readonly IContentLoader contentLoader;

        public RelatedPagesBlockController(OptimizelyContentIndexService indexService, IPageRouteHelper pageRouteHelper, IContentLoader contentLoader)
        {
            this.indexService = indexService;
            this.pageRouteHelper = pageRouteHelper;
            this.contentLoader = contentLoader;
        }

        protected override IViewComponentResult InvokeComponent(RelatedPagesBlock currentContent)
        {
            var contentQuery = new ContentQuery();

            // All pages in this site should be `AbstractContentPage`. So we'll only search for pages that descend from this type.
            contentQuery.WhereMatch("Type", nameof(AbstractContentPage));


            // If we can find the start page, then only search for pages that are descendants of that page, and only in that page's language,
            // which for simplicity, we'll assume is the desired language for search.
            var currentStartPage = SiteDefinition.Current?.StartPage;
            if (!ContentReference.IsNullOrEmpty(currentStartPage) && contentLoader.TryGet(currentStartPage, out PageData startPage))
            {
                contentQuery.WhereMatch("Ancestors", startPage.ContentLink.ID.ToString());
                contentQuery.Language = startPage.Language.Name;
            }
            
            // MatchAny on the categories an editor might have selected.
            if (currentContent.MatchAnyCategory is not null && currentContent.MatchAnyCategory.Any())
            {
                contentQuery.WhereMatchAny("Category", currentContent.MatchAnyCategory.Select(x => x.ToString()).ToArray());
            }

            // MatchAll on the categories an editor might have selected.
            if (currentContent.MatchAllCategories is not null && currentContent.MatchAllCategories.Any())
            {
                contentQuery.WhereMatchAll("Category", currentContent.MatchAllCategories.Select(x => x.ToString()).ToArray());
            }

            // Finally execute the query.
            var matchingPages = indexService.Query<AbstractContentPage>(contentQuery);

            // Build the viewModel and render.
            var viewModel = new RelatedPagesBlockViewModel(currentContent, matchingPages);
            return View("~/Views/Blocks/RelatedPagesBlock.cshtml", viewModel);
        }
    }
}
