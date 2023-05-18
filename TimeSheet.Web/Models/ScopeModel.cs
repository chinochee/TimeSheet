using Services.Dtos;

namespace TimeSheet.Web.Models
{
    public class ScopeModel
    {
        public ScopeModel(CacheDateTimeDto? cacheDateTime, ScopeEntryBTCDto[] scopes)
        {
            CacheDateTime = cacheDateTime;
            Scopes = scopes;
        }

        public CacheDateTimeDto? CacheDateTime { get; set; }
        public ScopeEntryBTCDto[] Scopes { get; set; }
    }
}