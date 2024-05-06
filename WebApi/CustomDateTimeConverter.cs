using System.Text.Json.Serialization;
using System.Text.Json;

class CustomDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateTime.TryParse(reader.GetString(), out var date))
            {
                return date;
            }
            else
            {
                throw new JsonException("無法將 JSON 值轉換為日期時間。請確保日期時間格式正確。");
            }
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}