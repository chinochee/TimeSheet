using Bogus;
using Data.Entities;

namespace Data
{
    public static class GeneratorTimeSheet
    {
        public static TimeSheet CreateTimeSheet()
        {
            var faker = new Faker();

            var timeSheet = new TimeSheet
            {
                Id = faker.Random.Int(1, 100000),
                Name = faker.Random.String2(13, "qwertyuiopasdfghjklzxcvbnm"),
                Scope = faker.Random.String2(13, "qwertyuiopasdfghjklzxcvbnm"),
                WorkHours = faker.Random.Int(1, 100000),
                DateOfWorks = RandomDay(),
                Comment = faker.Random.String2(13, "qwertyuiopasdfghjklzxcvbnm"),
                DateLastEdit = DateTime.Now
            };
            
            return timeSheet; 
        }

        public static List<TimeSheet> CreateTimeSheetList(int current = 10)
        {
            var timeSheetList = new List<TimeSheet>();

            for(int i = 0; i < current; i++)
            {
                timeSheetList.Add(CreateTimeSheet());
            }

            return timeSheetList;
        }

        private static DateTime RandomDay()
        {
            var dateTime = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - dateTime).Days;
            return dateTime.AddDays(new Random().Next(range));
        }
    }
}
