namespace Data.Entities
{
    public class Scope
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Rate { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public ICollection<TimeSheet> TimeSheetList { get; set; }
    }
}