namespace Services
{
    public interface ICurrencyImportService
    {
        Task Import(Stream stream);
    }
}