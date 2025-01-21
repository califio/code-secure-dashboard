using CodeSecure.Database.Metadata;

namespace CodeSecure.Api.Project.Model;

public record ProjectSetting
{
    public SastSetting? Sast { get; set; }
    public ScaSetting? Sca { get; set; }
}