using AutoMapper;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Mapping.AutoMapper;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Mapping.AutoMapper.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Mapping.AutoMapper;

public class AutoMapperObjectMapperTests
{
    [Fact]
    public void Map_ObjectTypeA_ReturnsObjectTypeB()
    {
        // Arrange
        var configuration = new MapperConfiguration(x => { x.AddProfile<MappingProfile>(); });
        var autoMapper = configuration.CreateMapper();
        var abstractedMapper = new AutoMapperObjectMapper(autoMapper);

        var objectToMap = new ObjectTypeA(1, "A");

        // Act
        var actual = abstractedMapper.Map<ObjectTypeA, ObjectTypeB>(objectToMap);

        // Assert
        Assert.IsType<ObjectTypeB>(actual);
        Assert.Equal(1, actual.IntegerValue);
        Assert.Equal("A", actual.StringValue);
    }
}