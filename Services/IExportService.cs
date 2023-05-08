namespace Services
{
    public interface ITableSheetExportService
    {
        Task<MemoryStream> GetCurrencyStreamXlsx();
    }
}