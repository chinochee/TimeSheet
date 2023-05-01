namespace Data.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TimeSheet> TimeSheetList { get; set; }
    }
}