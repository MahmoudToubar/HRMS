using Core.Entities;

namespace Core.Interfaces
{
    public interface IExcelExportService
    {
        byte[] ExportEmployeesToExcel(IReadOnlyList<Employee> employees);
    }
}
