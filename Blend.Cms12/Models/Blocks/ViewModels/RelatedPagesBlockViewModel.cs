using Blend.Cms12.Models.Pages;
using System.Collections.Generic;

namespace Blend.Cms12.Models.Blocks.ViewModels
{
    public class RelatedPagesBlockViewModel
    {
        public RelatedPagesBlockViewModel(RelatedPagesBlock block, IEnumerable<AbstractContentPage> relatedPages)
        {
            Block = block;
            RelatedPages = relatedPages;
        }

        public RelatedPagesBlock Block { get; }
        public IEnumerable<AbstractContentPage> RelatedPages { get; }
    }
}
