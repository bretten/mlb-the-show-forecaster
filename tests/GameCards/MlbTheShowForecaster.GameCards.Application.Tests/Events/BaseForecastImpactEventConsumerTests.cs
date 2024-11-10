using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events;

public class BaseForecastImpactEventConsumerTests
{
    protected SeasonYear Year = SeasonYear.Create(2024);
    protected MlbId MlbId = MlbId.Create(1);
    protected CardExternalId CardExternalId = CardExternalId.Create(new Guid("00000000-0000-0000-0000-000000000001"));

    protected static Mock<ICommandSender> MockCommandSender()
    {
        return new Mock<ICommandSender>();
    }

    protected static Mock<ITrendReporter> MockTrendReporter()
    {
        return new Mock<ITrendReporter>();
    }

    protected static ForecastImpactDuration StubImpactDuration(int d = 3)
    {
        return new ForecastImpactDuration(d, d, d, d, d, d, d, d, d, d);
    }

    protected static ForecastImpact[] ItIs(ForecastImpact i)
    {
        return Match.Create<ForecastImpact[]>(x => x[0] == i);
    }
}