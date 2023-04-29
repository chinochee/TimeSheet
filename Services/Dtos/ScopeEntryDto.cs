using Data.Entities;

namespace Services.Dtos
{
    public class ScopeEntryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceUSD { get; set; }
        public Currency Currency { get; set; }
        public string? NameCurrency { get; set; }
    }
}