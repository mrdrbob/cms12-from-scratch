using EPiServer.Core;
using EPiServer.DataAnnotations;
using System;

namespace Blend.Cms12.Models.Blocks
{
    [ContentType(
    DisplayName = "Related Pages Block",
        GUID = "1d1fe18f-19d3-425b-bf32-af113cf5d0e1")]
    public class RelatedPagesBlock : BlockData
    {
        public virtual CategoryList? MatchAllCategories { get; set; }

        public virtual CategoryList? MatchAnyCategory { get; set; }
    }
}
