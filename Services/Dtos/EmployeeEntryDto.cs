namespace Services.Dtos
{
    public class EmployeeEntryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double TotalPriceUSD { get; set; }
    }

    public class EmployeeEntryBTCDto : EmployeeEntryDto
    {
        public EmployeeEntryBTCDto(EmployeeEntryDto other, double btcRate)
        {
            Id = other.Id;
            Name = other.Name;
            TotalPriceUSD = Math.Round(other.TotalPriceUSD, 2);
            TotalPriceInBTC = Math.Round(other.TotalPriceUSD / btcRate, 2);
        }

        public double? TotalPriceInBTC { get; set; }
    }
}