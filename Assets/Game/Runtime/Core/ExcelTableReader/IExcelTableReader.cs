using OfficeOpenXml;

namespace Game.Runtime.Core.ExcelTableReader
{
    public interface IExcelTableReader
    {
        void Read(ExcelWorksheet sheet, ExcelTableContext context);
    }
}