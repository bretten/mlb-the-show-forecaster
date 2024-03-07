using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFramework;

public sealed class PlayerStatsBySeasonEntityTypeConfiguration : IEntityTypeConfiguration<PlayerStatsBySeason>
{
    public void Configure(EntityTypeBuilder<PlayerStatsBySeason> builder)
    {
        throw new NotImplementedException();
    }
}