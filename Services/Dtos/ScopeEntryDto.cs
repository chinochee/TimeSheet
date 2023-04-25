namespace Services.Dtos
{
    public class ScopeEntryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string TotalPrice { get; set; }
        public double TotalPriceUSD { get; set; }
    }
}