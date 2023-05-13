namespace Data.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string? FullName { get; set; }
        public double DollarExchangeRate { get; set; }
    }
}