using Blend.Cms12.Models.Blocks;
using Blend.Cms12.Models.Blocks.ViewModels;
using Blend.Cms12.Models.Pages;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blend.Cms12.Controllers.Blocks
{
    public class RelatedPagesBlockController : BlockComponent<RelatedPagesBlock>
    {
        protected override IViewComponentResult InvokeComponent(RelatedPagesBlock currentContent)
        {
            // TODO
            var viewModel = new RelatedPagesBlockViewModel(currentContent, Enumerable.Empty<AbstractContentPage>());

            return View("~/Views/Blocks/RelatedPagesBlock.cshtml", viewModel);
        }
    }
}
