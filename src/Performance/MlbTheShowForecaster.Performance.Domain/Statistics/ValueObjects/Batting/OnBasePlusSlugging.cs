using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

/// <summary>
/// On-base plus slugging (OPS)
/// = OBP + SLG
///
/// <para>The sum of a player's on-base percentage and slugging percentage. It measures the ability to hit and
/// their hitting power</para>
/// </summary>
public sealed class OnBasePlusSlugging : CalculatedStat
{
    /// <summary>
    /// On-base percentage
    /// </summary>
    public OnBasePercentage OnBasePercentage { get; }

    /// <summary>
    /// Slugging
    /// </summary>
    public Slugging Slugging { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="onBasePercentage">On-base percentage</param>
    /// <param name="slugging">Slugging</param>
    private OnBasePlusSlugging(OnBasePercentage onBasePercentage, Slugging slugging)
    {
        OnBasePercentage = onBasePercentage;
        Slugging = slugging;
    }

    /// <summary>
    /// Calculates on-base plus slugging
    /// </summary>
    /// <returns>On-base plus slugging</returns>
    protected override decimal Calculate()
    {
        return OnBasePercentage.Value + Slugging.Value;
    }

    /// <summary>
    /// Creates <see cref="OnBasePlusSlugging"/>
    /// </summary>
    /// <param name="onBasePercentage">On-base percentage</param>
    /// <param name="slugging">Slugging</param>
    /// <returns><see cref="OnBasePlusSlugging"/></returns>
    public static OnBasePlusSlugging Create(OnBasePercentage onBasePercentage, Slugging slugging)
    {
        return new OnBasePlusSlugging(onBasePercentage, slugging);
    }
}