using Blend.Cms12.Models.Pages;
using Blend.Cms12.Models.Pages.ViewModels;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Blend.Cms12.Controllers.Pages
{
    public abstract class BasePageController<T> : PageController<T> where T : AbstractContentPage
    {
        protected IActionResult PageView(T model)
        {
            var viewModel = new PageViewModel<T>(model);
            return PageView(viewModel);
        }

        protected IActionResult PageView(PageViewModel<T> viewModel)
        {
            viewModel.PageTitle ??= string.IsNullOrEmpty(viewModel.Page.PageTitle) ? viewModel.Page.Name : viewModel.Page.PageTitle;

            return View($"~/Views/Pages/{typeof(T).Name}.cshtml", viewModel);
        }
    }
}
