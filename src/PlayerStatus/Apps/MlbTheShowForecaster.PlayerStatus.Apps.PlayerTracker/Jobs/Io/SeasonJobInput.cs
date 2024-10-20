using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Jobs.Io;

/// <summary>
/// Input for a job that deals with a single <see cref="SeasonYear"/>
/// </summary>
/// <param name="Year">The season</param>
public record SeasonJobInput(SeasonYear Year) : IJobInput;