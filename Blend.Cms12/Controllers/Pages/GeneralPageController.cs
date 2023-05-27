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

            if (currentContent.BottomContentArea is not null)
            {
                var blockReferences = currentContent.BottomContentArea.FilteredItems;
                foreach (var reference in blockReferences)
                {
                    var block = contentLoader.Get<BlockData>(reference.ContentLink);

                    if (block is SectionMediaBlock sectionMedia)
                    {
                        Console.WriteLine($"SectionMediaBlock - {((IContent)sectionMedia).ContentLink}");
                    } else if (block is LinkGridBlock linkGridBlock)
                    {
                        Console.WriteLine($"LinkGridBlock");
                    }
                }
            }

            return PageView(currentContent);
        }
    }
}
