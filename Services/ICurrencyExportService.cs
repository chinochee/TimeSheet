using ClosedXML.Excel;

namespace Services
{
    public interface ICurrencyExportService
    {
        Task<XLWorkbook> Export();
    }
}