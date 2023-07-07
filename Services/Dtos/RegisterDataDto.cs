namespace Services.Dtos
{
    public class RegisterDataDto : LoginEntryDto
    {
        public string Name { get; set; }
        public List<int> RoleIdList { get; set; }
    }
}