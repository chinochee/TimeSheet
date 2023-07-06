namespace Services.Extensions
{
    public static class ListExtensions
    {
        public static List<T> RemoveDuplicates<T>(List<T> list)
        {
            return new HashSet<T>(list).ToList();
        }
    }
}