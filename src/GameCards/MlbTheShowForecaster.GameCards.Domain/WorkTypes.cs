﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain;

/// <summary>
/// Defines work for the Cards sub-domain
/// </summary>
public interface ICardWork : IUnitOfWorkType;

/// <summary>
/// Defines work for the Forecast sub-domain
/// </summary>
public interface IForecastWork : IUnitOfWorkType;

/// <summary>
/// Defines work for the Marketplace sub-domain
/// </summary>
public interface IMarketplaceWork : IUnitOfWorkType;