namespace Services.Configuration
{
    public class TableSettings
    {
        public const string Settings = "TableSettings";
        public int PageSize { get; set; }
        public int TopScopes { get; set; }
        public int TopEmployees { get; set; }
    }
}