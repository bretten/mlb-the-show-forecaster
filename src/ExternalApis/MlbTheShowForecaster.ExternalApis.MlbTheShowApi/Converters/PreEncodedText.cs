using System.Text.Json;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;

/// <summary>
/// Performance wise, when writing JSON using <see cref="Utf8JsonWriter"/> it is better to pre-encode known property names:
/// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-utf8jsonwriter#write-with-utf-8-text
/// </summary>
public static class PreEncodedText
{
    public static class Items
    {
        public static readonly JsonEncodedText Uuid = JsonEncodedText.Encode("uuid");
        public static readonly JsonEncodedText Type = JsonEncodedText.Encode("type");
        public static readonly JsonEncodedText Image = JsonEncodedText.Encode("img");
        public static readonly JsonEncodedText Name = JsonEncodedText.Encode("name");
        public static readonly JsonEncodedText Rarity = JsonEncodedText.Encode("rarity");
        public static readonly JsonEncodedText IsSellable = JsonEncodedText.Encode("is_sellable");
        public static readonly JsonEncodedText TeamShortName = JsonEncodedText.Encode("team_short_name");
        public static readonly JsonEncodedText Brand = JsonEncodedText.Encode("brand");

        public static class MlbCards
        {
            public static readonly JsonEncodedText Series = JsonEncodedText.Encode("series");
            public static readonly JsonEncodedText Overall = JsonEncodedText.Encode("ovr");
        }

        public static class Stadiums
        {
            public static readonly JsonEncodedText Capacity = JsonEncodedText.Encode("capacity");
            public static readonly JsonEncodedText Surface = JsonEncodedText.Encode("surface");
            public static readonly JsonEncodedText Elevation = JsonEncodedText.Encode("elevation");
            public static readonly JsonEncodedText Built = JsonEncodedText.Encode("built");
        }

        public static class Equipment
        {
            public static readonly JsonEncodedText Slot = JsonEncodedText.Encode("slot");
        }

        public static class Sponsorships
        {
            public static readonly JsonEncodedText Bonus = JsonEncodedText.Encode("bonus");
        }

        public static class Unlockables
        {
            public static readonly JsonEncodedText CategoryId = JsonEncodedText.Encode("category_id");
            public static readonly JsonEncodedText SubCategoryId = JsonEncodedText.Encode("sub_category_id");
        }
    }
}