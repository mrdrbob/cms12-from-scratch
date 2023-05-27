using Blend.Cms12.Models.Pages;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Blend.Cms12.Models.Blocks
{
    [ContentType(
        DisplayName = "Link Grid Block",
        GUID = "38bdc4f8-457c-44b4-be61-2d98f60eb264")]
    public class LinkGridBlock : BlockData
    {
        [Display(
            Name = "Links",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [AllowedTypes(AllowedTypes = new[] { typeof (AbstractContentPage) })]
        public virtual ContentArea? Links { get; set; }
    }
}
