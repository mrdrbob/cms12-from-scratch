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
        private readonly OptimizelyContentIndexService indexService;

        public ReindexSiteScheduledJob(
            IContentLoader loader, 
            ISiteDefinitionRepository siteRepository, 
            ILanguageBranchRepository languageRepository,
            OptimizelyContentIndexService indexService)
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

            indexService.DeleteAll();
            var context = new PageTreeUpdateContext();

            foreach (var site in sites)
            {
                if (!context.ShouldContinue)
                    break;

                foreach (var lang in languages)
                {
                    if (!context.ShouldContinue)
                        break;

                    IndexSiteAndLanguage(site, lang, context);
                }
            }

            return $"Processed {_pages} pages.";
        }

        private void IndexSiteAndLanguage(SiteDefinition site, LanguageBranch language, PageTreeUpdateContext context)
        {
            // var startPage = loader.Get<PageData>(site.StartPage);
            if (ContentReference.IsNullOrEmpty(site.StartPage))
                return;

            if (!loader.TryGet(site.StartPage, language.Culture, out PageData startPage))
                return;

            indexService.ReindexTree(startPage, context);
        }
    }
}
