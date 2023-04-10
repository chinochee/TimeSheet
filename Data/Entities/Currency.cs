using Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [EntityTypeConfiguration(typeof(CurrencyConfiguration))]
    public class Currency
    {
        public int Id { get; set; }
        public string? ShortName { get; set; }
        public string? FullName { get; set; }
        public double DollarExchangeRate { get; set; }
    }
}
