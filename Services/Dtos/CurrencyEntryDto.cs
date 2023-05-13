namespace Services.Dtos
{
    public class CurrencyEntryDto
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string? FullName { get; set; }
        public double DollarExchangeRate { get; set; }
    }
}