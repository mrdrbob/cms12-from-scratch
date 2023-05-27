using Blend.Cms12.Models.Blocks;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Blend.Cms12.Models.Pages
{
    [ContentType(
        DisplayName = "General Page",
        GUID = "d9853ee5-3b18-4b3a-b016-09b1b8605145",
        GroupName = "General Content")]
    public class GeneralPage : AbstractContentPage
    {
        [Display(
            Name = "Headline",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string? Headline { get; set; }

        [Display(
            Name = "Headline Slug",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string? HeadlineSlug { get; set; }


        [Display(
            Name = "Body",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString? Body { get; set; }

        [Display(
            Name = "Bottom Content Area",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        [AllowedTypes(AllowedTypes = new[] { typeof(SectionMediaBlock), typeof(LinkGridBlock), typeof(RelatedPagesBlock) })]
        public virtual ContentArea? BottomContentArea { get; set; }
    }
}
