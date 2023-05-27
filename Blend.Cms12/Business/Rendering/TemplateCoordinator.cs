using Blend.Cms12.Models.Blocks;
using Blend.Cms12.Models.Pages;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;

namespace Blend.Cms12.Business.Rendering
{
    [ServiceConfiguration]
    public class TemplateCoordinator : IViewTemplateModelRegistrator
    {
        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
            RegisterBlock<LinkGridBlock>(viewTemplateModelRegistrator);
            RegisterBlock<SectionMediaBlock>(viewTemplateModelRegistrator);

            RegisterPartial<AbstractContentPage>(viewTemplateModelRegistrator, "ArticlePartial");
            RegisterPartial<AbstractContentPage>(viewTemplateModelRegistrator, "LinkGrid");
        }

        private void RegisterPartial<T>(TemplateModelCollection viewTemplateModelRegistrator, string tagName) where T : AbstractContentPage
        {
            viewTemplateModelRegistrator.Add(typeof(T), new EPiServer.DataAbstraction.TemplateModel
            {
                Name = $"{typeof(T).Name}-{tagName}",
                AvailableWithoutTag = false,
                Tags = new[] { tagName },
                Inherit = true,
                TemplateTypeCategory = EPiServer.Framework.Web.TemplateTypeCategories.MvcPartialView,
                Path = $"~/Views/{typeof(T).Name}/{tagName}.cshtml"
            });

        }

        private void RegisterBlock<T>(TemplateModelCollection viewTemplateModelRegistrator) where T : BlockData
        {
            viewTemplateModelRegistrator.Add(typeof(T), new EPiServer.DataAbstraction.TemplateModel
            {
                Name = "SectionMediaBlock-Default",
                AvailableWithoutTag = true,
                TemplateTypeCategory = EPiServer.Framework.Web.TemplateTypeCategories.MvcPartialView,
                Path = $"~/Views/Blocks/{typeof(T).Name}.cshtml"
            });

        }
    }
}
