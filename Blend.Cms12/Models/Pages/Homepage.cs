using EPiServer.DataAnnotations;

namespace Blend.Cms12.Models.Pages
{
    [ContentType(
        DisplayName = "Homepage",
        Description = "Homepage",
        GUID = "fff1c9a2-ad98-446c-978a-21bd49f61c26",
        GroupName = "Specialized Content")]
    public class Homepage : AbstractContentPage
    {
    }
}
