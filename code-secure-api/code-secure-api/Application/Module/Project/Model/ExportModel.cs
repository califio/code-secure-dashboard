namespace CodeSecure.Application.Module.Project.Model;

public class ExportModel
{
    public required string FileName { get; set; }
    public required string MineType { get; set; }
    public required byte[] Data { get; set; }
}