namespace Services.Dtos
{
    public class ScopeEntryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceUSD { get; set; }
        public string? NameCurrency { get; set; }
    }

    public class ScopeEntryBTCDto : ScopeEntryDto
    {
        public ScopeEntryBTCDto(ScopeEntryDto other, double btcRate)
        {
            Id = other.Id;
            Name = other.Name;
            TotalPrice = other.TotalPrice;
            TotalPriceUSD = Math.Round(other.TotalPriceUSD, 2);
            TotalPriceInBTC = Math.Round(other.TotalPriceUSD / btcRate, 2);
        }

        public double? TotalPriceInBTC { get; set; }
    }
}