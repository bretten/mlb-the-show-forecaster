using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters.Exceptions;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;

public sealed class ItemJsonConverter : JsonConverter<ItemDto>
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(ItemDto);

    public override ItemDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Determine the item type
        var readerCopy = reader; // Reader is a value type, so make a copy by assigning it to a new var
        // Use the reader copy to determine the item type (only need to traverse two JSON properties to get to type)
        var itemType = DetermineItemType(ref readerCopy);


        return itemType switch
        {
            ItemType.MlbCard => JsonSerializer.Deserialize<MlbCardDto>(ref reader),
            ItemType.Stadium => JsonSerializer.Deserialize<StadiumDto>(ref reader),
            ItemType.Equipment => JsonSerializer.Deserialize<EquipmentDto>(ref reader),
            ItemType.Sponsorship => JsonSerializer.Deserialize<SponsorshipDto>(ref reader),
            ItemType.Unlockable => JsonSerializer.Deserialize<UnlockableDto>(ref reader),
            _ => throw new UnknownItemTypeException(
                $"Determined item type was not in the expected range of {typeof(ItemType)}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ItemDto value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    private ItemType DetermineItemType(ref Utf8JsonReader reader)
    {
        ReadUntilProperty(ref reader, "type");
        reader.Read(); // Read the type value
        var type = reader.GetString();

        return type switch
        {
            Constants.ItemTypes.MlbCard => ItemType.MlbCard,
            Constants.ItemTypes.Stadium => ItemType.Stadium,
            Constants.ItemTypes.Equipment => ItemType.Equipment,
            Constants.ItemTypes.Sponsorship => ItemType.Sponsorship,
            Constants.ItemTypes.Unlockable => ItemType.Unlockable,
            _ => throw new UnknownItemTypeException($"Could not deserialize item type of {type}")
        };
    }

    /// <summary>
    /// Advances the reader until the specified JSON property name is encountered
    /// </summary>
    /// <param name="reader">The reader to advance</param>
    /// <param name="propertyName">The property name to look for</param>
    private void ReadUntilProperty(ref Utf8JsonReader reader, string propertyName)
    {
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (reader.ValueTextEquals(Encoding.UTF8.GetBytes(propertyName)))
                    {
                        return;
                    }

                    break;
                default:
                    reader.TrySkip();
                    break;
            }
        }
    }
}