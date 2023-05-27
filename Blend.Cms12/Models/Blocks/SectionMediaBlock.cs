using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace Blend.Cms12.Models.Blocks
{
    [ContentType(
        DisplayName = "Section Media Block",
        GUID = "7a34252a-1670-460d-96bf-a136cacd87ee")]
    public class SectionMediaBlock : BlockData
    {
        [Display(
            Name = "Headline",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string? Headline { get; set; }

        [Display(
            Name = "Body",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString? Body { get; set; }

        [Display(
            Name = "Image",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference? Image { get; set; }
    }
}
