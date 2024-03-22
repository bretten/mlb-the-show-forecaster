using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters.Exceptions;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;

/// <summary>
/// <see cref="JsonConverter"/> used to serialize/deserialize <see cref="ItemDto"/>
/// </summary>
public sealed class ItemJsonConverter : JsonConverter<ItemDto>
{
    /// <summary>
    /// Makes sure the specified type can be converted by this converter
    /// </summary>
    /// <param name="typeToConvert">The type to check</param>
    /// <returns>True if it can be converted, otherwise false</returns>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(ItemDto);

    /// <summary>
    /// Uses the reader to parse the current JSON scope as a <see cref="ItemDto"/>
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <param name="typeToConvert">The type being converted</param>
    /// <param name="options">The options for the serializer</param>
    /// <returns>The parsed object</returns>
    /// <exception cref="UnknownItemTypeException">Thrown when the <see cref="ItemType"/> is not valid</exception>
    public override ItemDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Determine the item type
        var readerCopy = reader; // Reader is a value type, so make a copy by assigning it to a new var
        // Use the reader copy to determine the item type (only need to traverse two JSON properties to get to type)
        var itemType = DetermineItemType(ref readerCopy);

        // With the type now known, parse using the default deserialization behavior
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

    /// <summary>
    /// Writes an <see cref="ItemDto"/>
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    /// <param name="options">The options for the serializer </param>
    public override void Write(Utf8JsonWriter writer, ItemDto value, JsonSerializerOptions options)
    {
        // Start the whole JSON object
        writer.WriteStartObject();

        // Write the item
        switch (value.Type)
        {
            case Constants.ItemTypes.MlbCard:
                WriteMlbCard(writer, (value as MlbCardDto)!);
                break;
            case Constants.ItemTypes.Stadium:
                WriteStadium(writer, (value as StadiumDto)!);
                break;
            case Constants.ItemTypes.Equipment:
                WriteEquipment(writer, (value as EquipmentDto)!);
                break;
            case Constants.ItemTypes.Sponsorship:
                WriteSponsorship(writer, (value as SponsorshipDto)!);
                break;
            case Constants.ItemTypes.Unlockable:
                WriteUnlockable(writer, (value as UnlockableDto)!);
                break;
            default:
                throw new UnknownItemTypeException($"Cannot write because ItemType value {value.Type} is not known");
        }

        // End the whole JSON object
        writer.WriteEndObject();
    }

    /// <summary>
    /// Determines the <see cref="ItemType"/> of the <see cref="ItemDto"/>
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <returns>The <see cref="ItemType"/></returns>
    /// <exception cref="UnknownItemTypeException">Thrown when the JSON had an unknown item type string value</exception>
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

    /// <summary>
    /// Writes an <see cref="ItemDto"/>
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    private void WriteItem(Utf8JsonWriter writer, ItemDto value)
    {
        writer.WriteString(PreEncodedText.Items.Uuid, value.Uuid);
        writer.WriteString(PreEncodedText.Items.Type, value.Type);
        writer.WriteString(PreEncodedText.Items.Image, value.ImageUrl);
        writer.WriteString(PreEncodedText.Items.Name, value.Name);
        writer.WriteString(PreEncodedText.Items.Rarity, value.Rarity);
        writer.WriteBoolean(PreEncodedText.Items.IsSellable, value.IsSellable);
    }

    /// <summary>
    /// Writes a <see cref="MlbCardDto"/>
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    private void WriteMlbCard(Utf8JsonWriter writer, MlbCardDto value)
    {
        WriteItem(writer, value);
        writer.WriteString(PreEncodedText.Items.MlbCards.Series, value.Series);
        writer.WriteNumber(PreEncodedText.Items.MlbCards.Overall, value.Overall);
    }

    /// <summary>
    /// Writes a <see cref="StadiumDto"/>
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    private void WriteStadium(Utf8JsonWriter writer, StadiumDto value)
    {
        WriteItem(writer, value);
        writer.WriteString(PreEncodedText.Items.Stadiums.Capacity, value.Capacity);
        writer.WriteString(PreEncodedText.Items.Stadiums.Surface, value.Surface);
        writer.WriteString(PreEncodedText.Items.Stadiums.Elevation, value.Elevation);
        writer.WriteNumber(PreEncodedText.Items.Stadiums.Built, value.Built);
    }

    /// <summary>
    /// Writes a <see cref="EquipmentDto"/>
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    private void WriteEquipment(Utf8JsonWriter writer, EquipmentDto value)
    {
        WriteItem(writer, value);
        writer.WriteString(PreEncodedText.Items.Equipment.Slot, value.Slot);
    }

    /// <summary>
    /// Writes a <see cref="SponsorshipDto"/>
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    private void WriteSponsorship(Utf8JsonWriter writer, SponsorshipDto value)
    {
        WriteItem(writer, value);
        writer.WriteString(PreEncodedText.Items.Sponsorships.Bonus, value.Bonus);
    }

    /// <summary>
    /// Writes a <see cref="UnlockableDto"/>
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    private void WriteUnlockable(Utf8JsonWriter writer, UnlockableDto value)
    {
        WriteItem(writer, value);
        writer.WriteNumber(PreEncodedText.Items.Unlockables.CategoryId, value.CategoryId);
        writer.WriteNumber(PreEncodedText.Items.Unlockables.SubCategoryId, value.SubCategoryId);
    }
}