using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.TestClasses;

/// <summary>
/// Asserts expectations
/// </summary>
public static class Asserter
{
    /// <summary>
    /// Assert expectations done here to keep <see cref="TrendReport"/> DTO lean and free of equality checks since
    /// it is not needed elsewhere
    /// </summary>
    public static void Equal(TrendReport expected, TrendReport actual)
    {
        Assert.Equal(expected.Year, actual.Year);
        Assert.Equal(expected.CardExternalId, actual.CardExternalId);
        Assert.Equal(expected.MlbId, actual.MlbId);
        Assert.Equal(expected.PrimaryPosition, actual.PrimaryPosition);
        Assert.Equal(expected.OverallRating, actual.OverallRating);
        Assert.Equal(expected.CardName, actual.CardName);
        Assert.Equal(expected.MetricsByDate, actual.MetricsByDate);
        Assert.Equal(expected.Impacts, actual.Impacts);
    }
}