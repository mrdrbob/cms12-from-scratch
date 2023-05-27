using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Blend.Cms12.Models.Media
{
    [ContentType(
        DisplayName = "Image",
        GUID = "3b3ba5ae-1346-4696-b938-7cef25ab524f")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,png")]
    public class ImageFile : ImageData
    {
        [Display(
            Name = "Alternate Text")]
        public virtual string? AltText { get; set; }
    }
}
