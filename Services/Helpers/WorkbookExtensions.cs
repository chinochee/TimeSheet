using ClosedXML.Excel;

namespace Services.Helpers
{
    public static class WorkbookExtensions
    {
        public static MemoryStream AsMemoryStream(this XLWorkbook workbook)
        {
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);

                return stream;
            }
        }
    }
}