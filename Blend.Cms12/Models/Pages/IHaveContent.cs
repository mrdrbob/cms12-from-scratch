using Blend.ContentIndex;

namespace Blend.Cms12.Models.Pages
{
    public interface IHaveContent
    {
        void BuildIndex(IndexBuilder indexBuilder);
    }
}
