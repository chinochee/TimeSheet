namespace Data.Entities
{
    public class Scope
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Rate { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}