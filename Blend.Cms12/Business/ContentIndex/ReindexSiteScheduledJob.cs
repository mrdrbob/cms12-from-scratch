using Blend.Cms12.Models.Pages;
using Blend.ContentIndex;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using System.Collections.Generic;

namespace Blend.Cms12.Business.ContentIndex
{
    [ScheduledPlugIn(
        DisplayName = "Reindex Site Job",
        Description = "Reindex all content into the Content Index",
        GUID = "c69df786-9747-4031-85b8-78f30df7fce8",
        DefaultEnabled = true
    )]
    public class ReindexSiteScheduledJob : ScheduledJobBase
    {
        private readonly IContentLoader loader;
        private readonly ISiteDefinitionRepository siteRepository;
        private readonly ILanguageBranchRepository languageRepository;
        private readonly IndexService indexService;

        public ReindexSiteScheduledJob(
            IContentLoader loader, 
            ISiteDefinitionRepository siteRepository, 
            ILanguageBranchRepository languageRepository,
            IndexService indexService)
        {
            this.loader = loader;
            this.siteRepository = siteRepository;
            this.languageRepository = languageRepository;
            this.indexService = indexService;

            this.IsStoppable = true;
        }

        private bool _shouldContinue;
        private int _pages;

        public override void Stop()
        {
            _shouldContinue = false;
        }

        public override string Execute()
        {
            _shouldContinue = true;
            _pages = 0;

            var sites = siteRepository.List();
            var languages = languageRepository.ListEnabled();

            indexService.Delete(new ContentQuery());

            foreach (var site in sites)
            {
                if (!_shouldContinue)
                    break;

                foreach (var lang in languages)
                {
                    if (!_shouldContinue)
                        break;

                    IndexSiteAndLanguage(site, lang);
                }
            }

            return $"Processed {_pages} pages.";
        }

        private void IndexSiteAndLanguage(SiteDefinition site, LanguageBranch language)
        {
            Stack<PageData> stack = new Stack<PageData>();

            // var startPage = loader.Get<PageData>(site.StartPage);
            if (ContentReference.IsNullOrEmpty(site.StartPage))
                return;

            if (!loader.TryGet(site.StartPage, language.Culture, out PageData startPage))
                return;

            stack.Push(startPage);

            while (stack.Count > 0)
            {
                if (!_shouldContinue)
                    break;

                var page = stack.Pop();

                ProcessPage(page);
                _pages += 1;

                OnStatusChanged($"Processed {_pages} pages.");

                var childPages = loader.GetChildren<PageData>(page.ContentLink, language.Culture);
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
    }
}
