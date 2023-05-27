using System.Collections.Generic;
using System.Linq;

namespace Blend.Cms12.Models.Pages.ViewModels
{
    public class HomepageViewModel : PageViewModel<Homepage>
    {
        public HomepageViewModel(Homepage page) : base(page)
        {
        }

        public IEnumerable<AbstractContentPage> RelatedContent { get; internal set; } = Enumerable.Empty<AbstractContentPage>();
    }
}
