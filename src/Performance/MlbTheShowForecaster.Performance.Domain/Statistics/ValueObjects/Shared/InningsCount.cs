using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

/// <summary>
/// Innings count
///
/// <para>Used to represent innings pitched (IP) by a pitcher or innings played (INN) by a defensive player.
/// When a pitcher makes one out or a fielder plays on defense for one out, it counts as 1/3 of an inning. Two outs
/// counts as 2/3 of an inning. MLB shortens these to 0.1 IP and 0.2 IP respectively.</para>
/// </summary>
public sealed class InningsCount : ValueObject
{
    /// <summary>
    /// The number of fractional digits when the partial innings count is represented as a decimal
    /// </summary>
    private const int FractionalDigitCount = 3;

    /// <summary>
    /// The underlying innings pitched value
    /// </summary>
    public decimal Value => Round(FullInnings.Value + PartialInningsAsDecimal);

    /// <summary>
    /// The number of full innings
    /// </summary>
    public NaturalNumber FullInnings { get; }

    /// <summary>
    /// The number of additional outs. Possible values are 0, 1, or 2
    /// </summary>
    public NaturalNumber AdditionalOuts => NaturalNumber.Create(_partialInnings.AsAdditionalOuts);

    /// <summary>
    /// Partial innings count expressed as a decimal. Possible values are 0.0, 0.333 (1 out), or 0.667 (2 outs)
    /// </summary>
    public decimal PartialInningsAsDecimal => Round(_partialInnings.AsDecimal);

    /// <summary>
    /// Partial innings count expressed in MLB shorthand notation. Possible values are 0.0, 0.1 (1 out), or 0.2 (2 outs)
    /// </summary>
    public decimal PartialInningsAsShorthand => _partialInnings.AsShorthand;

    /// <summary>
    /// The amount of partial innings
    /// </summary>
    private readonly PartialInnings _partialInnings;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The underlying innings pitched value</param>
    private InningsCount(decimal value)
    {
        FullInnings = NaturalNumber.Create((int)Math.Truncate(value));
        // Partial innings is the original decimal value minus the full, whole innings
        _partialInnings = new PartialInnings(value - FullInnings.Value);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="fullInnings">The number of full innings</param>
    /// <param name="additionalOuts">The number of additional outs, where 3 outs is a full inning</param>
    private InningsCount(int fullInnings, int additionalOuts)
    {
        // There may be full innings in the additional outs, so divide by three
        var partialInnings = (decimal)additionalOuts / 3;
        var additionalFullInnings = (int)Math.Truncate(partialInnings);
        // Partial innings is the decimal value minus the additional full, whole innings
        _partialInnings = new PartialInnings(partialInnings - additionalFullInnings);

        // The total full innings
        FullInnings = NaturalNumber.Create(fullInnings + additionalFullInnings);
    }

    /// <summary>
    /// Creates <see cref="InningsCount"/>
    /// </summary>
    /// <param name="inningsCount">The number of innings</param>
    /// <returns><see cref="InningsCount"/></returns>
    public static InningsCount Create(decimal inningsCount)
    {
        return new InningsCount(inningsCount);
    }

    /// <summary>
    /// Creates <see cref="InningsCount"/>
    /// </summary>
    /// <param name="inningsCount">The number of innings</param>
    /// <returns><see cref="InningsCount"/></returns>
    public static InningsCount Create(string inningsCount)
    {
        if (!decimal.TryParse(inningsCount, out var innings))
        {
            throw new InvalidInningsCountDecimalException(
                $"Invalid partial innings count: {inningsCount}. It can only end in n.0, n.1 (1/3), or n.2 (2/3)");
        }

        return new InningsCount(innings);
    }

    /// <summary>
    /// Creates <see cref="InningsCount"/>
    /// </summary>
    /// <param name="fullInnings">The number of full innings</param>
    /// <param name="additionalOuts">The number of additional outs, where 3 outs is a full inning</param>
    /// <returns><see cref="InningsCount"/></returns>
    public static InningsCount Create(NaturalNumber fullInnings, NaturalNumber additionalOuts)
    {
        return new InningsCount(fullInnings.Value, additionalOuts.Value);
    }

    /// <summary>
    /// Standard rounding for innings count
    /// </summary>
    /// <param name="inningsCount">The number of innings</param>
    /// <returns>Rounded innings count</returns>
    private static decimal Round(decimal inningsCount)
    {
        return Math.Round(inningsCount, FractionalDigitCount, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Represents a partial inning
    /// <para>When a pitcher makes one out or a fielder plays on defense for one out, it counts as 1/3 of an inning.
    /// Two outs counts as 2/3 of an inning. MLB shortens these to 0.1 IP and 0.2 IP respectively.</para>
    /// </summary>
    private readonly record struct PartialInnings
    {
        private static readonly PartialInnings Zero = new(0);
        private static readonly PartialInnings OneThird = new(1);
        private static readonly PartialInnings TwoThirds = new(2);

        /// <summary>
        /// Maps partial innings values to their actual values
        /// </summary>
        private static readonly Dictionary<decimal, PartialInnings> ValidPartialInningsMap = new()
        {
            { 0.0m, Zero },
            // Decimal
            { Round((decimal)1 / 3), OneThird },
            { Round((decimal)2 / 3), TwoThirds },
            // Shorthand
            { 0.1m, OneThird },
            { 0.2m, TwoThirds }
        };

        /// <summary>
        /// The number of additional outs. Possible values are 0, 1, or 2
        /// </summary>
        public int AsAdditionalOuts { get; }

        /// <summary>
        /// Partial innings count expressed as a decimal. Possible values are 0.0, 0.333 (1 out), or 0.667 (2 outs)
        /// </summary>
        public decimal AsDecimal { get; }

        /// <summary>
        /// Partial innings count expressed in MLB shorthand notation. Possible values are 0.0, 0.1 (1 out), or 0.2 (2 outs)
        /// </summary>
        public decimal AsShorthand { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partialInnings">Partial innings in decimal or shorthand format</param>
        /// <exception cref="InvalidInningsCountDecimalException">Thrown if the partial innings is not valid</exception>
        internal PartialInnings(decimal partialInnings)
        {
            partialInnings = Round(partialInnings);
            // Partial innings may be in decimal (1 out = .333, 2 outs = .667) or shorthand (1 out = .1, 2 outs = 0.2)
            if (!ValidPartialInningsMap.TryGetValue(partialInnings, out var actualPartialInnings))
            {
                throw new InvalidInningsCountDecimalException(
                    $"Invalid partial innings count: {partialInnings}. It can only be 0.0, 0.1 (1/3), or 0.2 (2/3)");
            }

            AsAdditionalOuts = actualPartialInnings.AsAdditionalOuts;
            AsDecimal = actualPartialInnings.AsDecimal;
            AsShorthand = actualPartialInnings.AsShorthand;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="additionalOuts">Partial innings expressed as partial outs</param>
        private PartialInnings(int additionalOuts)
        {
            AsAdditionalOuts = additionalOuts;
            switch (additionalOuts)
            {
                case 1:
                    AsDecimal = (decimal)1 / 3;
                    AsShorthand = 0.1m;
                    break;
                case 2:
                    AsDecimal = (decimal)2 / 3;
                    AsShorthand = 0.2m;
                    break;
                default:
                    AsDecimal = 0;
                    AsShorthand = 0;
                    break;
            }
        }
    }
}