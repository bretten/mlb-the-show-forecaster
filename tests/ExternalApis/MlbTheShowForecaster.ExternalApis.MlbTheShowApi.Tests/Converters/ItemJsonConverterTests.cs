using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters.Exceptions;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Tests.TestFiles;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Tests.Converters;

public class ItemJsonConverterTests
{
    [Fact]
    public void Read_MlbCardJson_ReturnsParsedMlbCard()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsMlbCard);

        // Act
        var actual = JsonSerializer.Deserialize<ItemDto>(json);

        // Assert
        Assert.IsType<MlbCardDto>(actual);
        var actualItem = actual as MlbCardDto;
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actualItem!.Uuid);
        Assert.Equal("mlb_card", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCR2MvRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--a6aeae283163aa6cb6bf114e18ab406978aabf4f/2d1fb7af6a8075ea3e67b28d066e2556.webp",
            actualItem.ImageUrl);
        Assert.Equal("Shohei Ohtani", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal("Live", actualItem.Series);
        Assert.Equal("LAD", actualItem.TeamShortName);
        Assert.Equal("SP", actualItem.DisplayPosition);
        Assert.Equal(94, actualItem.Overall);
        Assert.Equal(86, actualItem.Stamina);
        Assert.Equal(106, actualItem.PitchingClutch);
        Assert.Equal(101, actualItem.HitsPerBf);
        Assert.Equal(89, actualItem.KPerBf);
        Assert.Equal(51, actualItem.BbPerBf);
        Assert.Equal(85, actualItem.HrPerBf);
        Assert.Equal(96, actualItem.PitchVelocity);
        Assert.Equal(69, actualItem.PitchControl);
        Assert.Equal(93, actualItem.PitchMovement);
        Assert.Equal(68, actualItem.ContactLeft);
        Assert.Equal(83, actualItem.ContactRight);
        Assert.Equal(93, actualItem.PowerLeft);
        Assert.Equal(102, actualItem.PowerRight);
        Assert.Equal(49, actualItem.PlateVision);
        Assert.Equal(93, actualItem.PlateDiscipline);
        Assert.Equal(88, actualItem.BattingClutch);
        Assert.Equal(35, actualItem.BuntingAbility);
        Assert.Equal(25, actualItem.DragBuntingAbility);
        Assert.Equal(0, actualItem.HittingDurability);
        Assert.Equal(96, actualItem.FieldingDurability);
        Assert.Equal(62, actualItem.FieldingAbility);
        Assert.Equal(94, actualItem.ArmStrength);
        Assert.Equal(66, actualItem.ArmAccuracy);
        Assert.Equal(55, actualItem.ReactionTime);
        Assert.Equal(0, actualItem.Blocking);
        Assert.Equal(86, actualItem.Speed);
        Assert.Equal(62, actualItem.BaseRunningAbility);
        Assert.Equal(72, actualItem.BaseRunningAggression);
    }

    [Fact]
    public void Read_StadiumJson_ReturnsParsedStadium()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsStadium);

        // Act
        var actual = JsonSerializer.Deserialize<ItemDto>(json);

        // Assert
        Assert.IsType<StadiumDto>(actual);
        var actualItem = actual as StadiumDto;
        Assert.Equal("7520fa31d14f45add6d61e52df5a03ff", actualItem!.Uuid);
        Assert.Equal("stadium", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRUtsRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--1cde10216309186128b5554d70496996a8598e9f/4d92741ae4ca5675c82b335638d512fa.webp",
            actualItem.ImageUrl);
        Assert.Equal("Crosley Field", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal("FA", actualItem.TeamShortName);
        Assert.Equal("29,603", actualItem.Capacity);
        Assert.Equal("Grass", actualItem.Surface);
        Assert.Equal("550ft", actualItem.Elevation);
        Assert.Equal(1912, actualItem.Built);
    }

    [Fact]
    public void Read_EquipmentJson_ReturnsParsedEquipment()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsEquipment);

        // Act
        var actual = JsonSerializer.Deserialize<ItemDto>(json);

        // Assert
        Assert.IsType<EquipmentDto>(actual);
        var actualItem = actual as EquipmentDto;
        Assert.Equal("5ef8cf77201dc48f2a2f22cd14ec648c", actualItem!.Uuid);
        Assert.Equal("equipment", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTnRMRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--6a2788233bdefd14e277d29cdd07909d979ab18d/c402564d08b21bb9d94a7513b79c98f8.webp",
            actualItem.ImageUrl);
        Assert.Equal("1989-2010 Ken Griffey Jr. C271 Ash", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal("Louisville Slugger&reg;", actualItem.Brand);
        Assert.Equal("Bat", actualItem.Slot);
        Assert.Equal(
            "The legendary Louisville Slugger Ken Griffey Jr. C271 Ash\"As long as I have fun playing, the stats will take care of themselves.\" - Ken Griffey Jr.",
            actualItem.Description);
    }

    [Fact]
    public void Read_SponsorshipJson_ReturnsParsedSponsorship()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsSponsorship);

        // Act
        var actual = JsonSerializer.Deserialize<ItemDto>(json);

        // Assert
        Assert.IsType<SponsorshipDto>(actual);
        var actualItem = actual as SponsorshipDto;
        Assert.Equal("0fe297769a5113a8c7b5942ebbef4d96", actualItem!.Uuid);
        Assert.Equal("sponsorship", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCTDdKRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--dc09e114b344777ad20f5e6ec1900e3a116325e0/default-sponsorship.webp",
            actualItem.ImageUrl);
        Assert.Equal("Diamond Sponsorship", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal("Oakley&reg;", actualItem.Brand);
        Assert.Equal("$ 2,500 Per Hit", actualItem.Bonus);
    }

    [Fact]
    public void Read_UnlockableJson_ReturnsParsedUnlockable()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsUnlockable);

        // Act
        var actual = JsonSerializer.Deserialize<ItemDto>(json);

        // Assert
        Assert.IsType<UnlockableDto>(actual);
        var actualItem = actual as UnlockableDto;
        Assert.Equal("fe6a68822b44d9bcaa8c858f62f06f34", actualItem!.Uuid);
        Assert.Equal("unlockable", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCT2VaRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--5ec915bc35e04955ad47fb6f72b6cd68c5dad3eb/c50deac26950085ecf67d4409d506c30.webp",
            actualItem.ImageUrl);
        Assert.Equal("1980's Retro Banner", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal(10002, actualItem.CategoryId);
        Assert.Equal(10006, actualItem.SubCategoryId);
    }

    [Fact]
    public void Read_UnknownItemType_ThrowsException()
    {
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsUnknown);
        var action = () => JsonSerializer.Deserialize<ItemDto>(json);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnknownItemTypeException>(actual);
    }

    [Fact]
    public void Write_MlbCard_SerializesMlbCard()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsMlbCard);
        var dto = JsonSerializer.Deserialize<ItemDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.ItemsMlbCard);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Write_Stadium_SerializesStadium()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsStadium);
        var dto = JsonSerializer.Deserialize<ItemDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.ItemsStadium);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Write_Equipment_SerializesEquipment()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsEquipment);
        var dto = JsonSerializer.Deserialize<ItemDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.ItemsEquipment);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Write_Sponsorship_SerializesSponsorship()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsSponsorship);
        var dto = JsonSerializer.Deserialize<ItemDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.ItemsSponsorship);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Write_Unlockable_SerializesUnlockable()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.ItemsUnlockable);
        var dto = JsonSerializer.Deserialize<ItemDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.ItemsUnlockable);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Write_UnknownItemType_ThrowsException()
    {
        // Arrange
        var dto = new SponsorshipDto("id1", "unknownType", "imgUrl", "Unknown Name", "Diamond", true, "brand", "bonus");
        var action = () => JsonSerializer.Serialize(dto as ItemDto);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnknownItemTypeException>(actual);
    }
}