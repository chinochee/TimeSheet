namespace Services.Configuration
{
    public class Permissions
    {
        public const string Key = "Permissions";
        public Dictionary<int, List<string>> RolesPermissions { get; set; }
    }
}