using ClosedXML.Excel;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportEmployeesToExcel(IReadOnlyList<Employee> employees)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            worksheet.Cell(1, 1).Value = "Full Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Mobile";
            worksheet.Cell(1, 4).Value = "Date Of Birth";
            worksheet.Cell(1, 5).Value = "Department";
            worksheet.Cell(1, 6).Value = "Job";

            worksheet.Range(1, 1, 1, 6).Style.Font.Bold = true;

            for (int i = 0; i < employees.Count; i++)
            {
                var row = i + 2;
                var emp = employees[i];

                worksheet.Cell(row, 1).Value = emp.FullName;
                worksheet.Cell(row, 2).Value = emp.Email;
                worksheet.Cell(row, 3).Value = emp.Mobile;
                worksheet.Cell(row, 4).Value = emp.DateOfBirth.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 5).Value = emp.Department.Name;
                worksheet.Cell(row, 6).Value = emp.Job.Name;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
