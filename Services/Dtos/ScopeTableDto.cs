namespace Services.Dtos
{
    public class ScopeTableDto
    {
        public ScopeEntryDto[] Entries { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
    }
}