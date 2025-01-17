namespace CodeSecure.Extension;

public class VersionInfo: IComparable<VersionInfo>
{
    private string VersionString { get; set; } = string.Empty;
    private int Major { get; set; }
    private int Minor { get; set; }
    private int Patch { get; set; }
    private string? PreRelease { get; set; }
    private string? BuildMetadata { get; set; }
    private bool IsPreRelease => !string.IsNullOrEmpty(PreRelease);
    
    public static VersionInfo Parse(string version)
    {
        version = version.Trim();
        var result = new VersionInfo
        {
            VersionString = version
        };
        var metadataSplit = version.Split('+');
        result.BuildMetadata = metadataSplit.Length > 1 ? metadataSplit[1] : null;

        var prereleaseSplit = metadataSplit[0].Split('-');
        result.PreRelease = prereleaseSplit.Length > 1 ? prereleaseSplit[1] : null;

        var mainParts = prereleaseSplit[0].TrimStart('v').Split('.');
        result.Major = mainParts.Length > 0 ? int.Parse(mainParts[0]) : 0;
        result.Minor = mainParts.Length > 1 ? int.Parse(mainParts[1]) : 0;
        result.Patch = mainParts.Length > 2 ? int.Parse(mainParts[2]) : 0;
        return result;
    }
    
    public static bool TryParse(string version, out VersionInfo? versionInfo)
    {
        versionInfo = null;
        try
        {
            versionInfo = Parse(version);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public int CompareTo(VersionInfo? other)
    {
        if (other != null)
        {
            if (Major != other.Major) return Major.CompareTo(other.Major);
            if (Minor != other.Minor) return Minor.CompareTo(other.Minor);
            if (Patch != other.Patch) return Patch.CompareTo(other.Patch);
            if (IsPreRelease && !other.IsPreRelease) return -1;
            if (!IsPreRelease && other.IsPreRelease) return 1;
            if (IsPreRelease && other.IsPreRelease)
            {
                return string.Compare(PreRelease, other.PreRelease, StringComparison.OrdinalIgnoreCase);
            }
        }
        return 0;
    }

    public override string ToString()
    {
        return VersionString;
    }
}