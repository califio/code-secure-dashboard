using CodeSecure.Application.Module.Report.Model;

namespace CodeSecure.Application.Module.Report;

public interface IReportManager
{
    byte[] ExportPdf(ReportModel model);
    byte[] ExportExcel(ReportModel model);
    byte[] ExportJson(ReportModel model);
}