namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Tests.TestFiles;

public static class TestFilesConstants
{
    public static class Objects
    {
        public static readonly string BattingStatsJson =
            $"TestFiles{Path.DirectorySeparatorChar}Responses{Path.DirectorySeparatorChar}BattingStats.json";

        public static readonly string PitchingStatsJson =
            $"TestFiles{Path.DirectorySeparatorChar}Responses{Path.DirectorySeparatorChar}PitchingStats.json";

        public static readonly string FieldingStatsJson =
            $"TestFiles{Path.DirectorySeparatorChar}Responses{Path.DirectorySeparatorChar}FieldingStats.json";

        public static readonly string UnknownStatsJson =
            $"TestFiles{Path.DirectorySeparatorChar}Responses{Path.DirectorySeparatorChar}UnknownStats.json";
    }

    public static class ExpectedJson
    {
        public static readonly string BattingStats =
            $"TestFiles{Path.DirectorySeparatorChar}ExpectedJson{Path.DirectorySeparatorChar}BattingStats.json";

        public static readonly string PitchingStats =
            $"TestFiles{Path.DirectorySeparatorChar}ExpectedJson{Path.DirectorySeparatorChar}PitchingStats.json";

        public static readonly string FieldingStats =
            $"TestFiles{Path.DirectorySeparatorChar}ExpectedJson{Path.DirectorySeparatorChar}FieldingStats.json";
    }
}