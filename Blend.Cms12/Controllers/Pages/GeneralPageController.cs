using Blend.Cms12.Models.Blocks;
using Blend.Cms12.Models.Pages;
using EPiServer;
using EPiServer.Core;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blend.Cms12.Controllers.Pages
{
    public class GeneralPageController : BasePageController<GeneralPage>
    {
        private readonly IContentLoader contentLoader;

        public GeneralPageController(IContentLoader contentLoader)
        {
            this.contentLoader = contentLoader;
        }

        public IActionResult Index(GeneralPage currentContent)
        {
            return PageView(currentContent);
        }
    }
}
