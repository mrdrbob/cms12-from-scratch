using Blend.Cms12.Models.Pages;
using Blend.ContentIndex;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Blend.Cms12.Business.ContentIndex
{
    [InitializableModule]
    public class ContentIndexInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var events = ServiceLocator.Current.GetInstance<IContentEvents>();

            events.PublishedContent += Events_PublishedContent;
            events.MovedContent += Events_MovedContent;
            events.DeletedContent += Events_DeletedContent;
        }

        private void Events_DeletedContent(object? sender, EPiServer.DeleteContentEventArgs e)
        {
            var indexService = ServiceLocator.Current.GetInstance<OptimizelyContentIndexService>();
            if (!(e.Content is PageData page))
                return;

            if (!(page is IHaveContent indexable))
                return;

            indexService.Delete(e.ContentLink);
        }

        private void Events_MovedContent(object? sender, EPiServer.ContentEventArgs e)
        {
            var indexService = ServiceLocator.Current.GetInstance<OptimizelyContentIndexService>();
            if (!(e.Content is PageData page))
                return;

            if (!(page is IHaveContent indexable))
                return;

            bool isDeleted = e.TargetLink.CompareToIgnoreWorkID(ContentReference.WasteBasket);
            if (isDeleted)
            {
                indexService.Delete(e.ContentLink);
            }
            else
            {
                indexService.ReindexTree(page, new PageTreeUpdateContext());
            }
        }

        private void Events_PublishedContent(object? sender, EPiServer.ContentEventArgs e)
        {
            var indexService = ServiceLocator.Current.GetInstance<IndexService>();
            if (!(e.Content is PageData page))
                return;

            if (!(page is IHaveContent indexable))
                return;

            var indexBuilder = new IndexBuilder(page.ContentLink.ID.ToString(), page.Language.Name);
            indexable.BuildIndex(indexBuilder);
            indexService.Update(indexBuilder);
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = ServiceLocator.Current.GetInstance<IContentEvents>();

            events.PublishedContent -= Events_PublishedContent;

        }
    }
}
