using CodeSecure.Manager.Report.Model;

namespace CodeSecure.Manager.Report;

public interface IReportManager
{
    byte[] ExportPdf(ReportModel model);
    byte[] ExportExcel(ReportModel model);
    byte[] ExportJson(ReportModel model);
}