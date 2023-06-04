using Blend.ContentIndex;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System.ComponentModel.DataAnnotations;

namespace Blend.Cms12.Models.Pages
{
    public abstract class AbstractContentPage : PageData, IHaveContent
    {
        [Display(
            Name = "Page Title",
            GroupName = "SEO",
            Order = 10)]
        public virtual string? PageTitle { get; set; }

        [Display(
            Name = "Meta Description",
            GroupName = "SEO",
            Order = 20)]
        public virtual string? MetaDescription { get; set; }

        public virtual void BuildIndex(IndexBuilder indexBuilder)
        {
            var type = this.GetOriginalType();

            while (type is not null && type != typeof(PageData))
            {
                indexBuilder.Add("Type", type.Name);
                type = type.BaseType;
            }

            foreach(var cat in this.Category)
            {
                indexBuilder.Add("Category", cat.ToString());
            }

            AbstractContentPage? page = this;
            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            while (page is not null)
            {
                indexBuilder.Add("Ancestors", page.ContentLink.ID.ToString());

                if (ContentReference.IsNullOrEmpty(page.ParentLink))
                    break;

                if (!loader.TryGet(page.ParentLink, page.Language, out IContent parentContent))
                    break;

                if (!(parentContent is AbstractContentPage abstractContent))
                    break;

                page = abstractContent;
            }
        }
    }
}
