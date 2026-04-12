namespace HTP.App.Core.Settings;

public class SeederSettings
{
    public const string SectionName = "Seeder";

    public required string AdminLogin { get; set; }
    public required string AdminPassword { get; set; }
}
