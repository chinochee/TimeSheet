namespace Services
{
    public interface ICurrencyImportService
    {
        void PostCurrenciesFromStreamXlsx(Stream stream);
    }
}