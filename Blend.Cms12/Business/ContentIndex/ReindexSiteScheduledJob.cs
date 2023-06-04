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
            _context = new PageTreeUpdateContext
            {
                OnStatusChanged = this.OnStatusChanged
            };
        }

        private readonly PageTreeUpdateContext _context;

        public override void Stop()
        {
            _context.ShouldContinue = false;
        }

        public override string Execute()
        {
            _context.ShouldContinue = true;
            _context.Pages = 0;

            var sites = siteRepository.List();
            var languages = languageRepository.ListEnabled();

            indexService.DeleteAll();

            foreach (var site in sites)
            {
                if (!_context.ShouldContinue)
                    break;

                foreach (var lang in languages)
                {
                    if (!_context.ShouldContinue)
                        break;

                    IndexSiteAndLanguage(site, lang);
                }
            }

            return $"Processed {_context.Pages} pages.";
        }

        private void IndexSiteAndLanguage(SiteDefinition site, LanguageBranch language)
        {
            // var startPage = loader.Get<PageData>(site.StartPage);
            if (ContentReference.IsNullOrEmpty(site.StartPage))
                return;

            if (!loader.TryGet(site.StartPage, language.Culture, out PageData startPage))
                return;

            indexService.ReindexTree(startPage, _context);
        }
    }
}
