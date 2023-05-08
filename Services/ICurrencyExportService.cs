namespace Services
{
    public interface ICurrencyExportService
    {
        Task<MemoryStream> GetMemoryStreamXlsx();
    }
}