using System.Text.Json.Serialization;

namespace RabbitCommon;

public class RabbitMessage
{
    [JsonPropertyName("country")] 
    public required string Country { get; init; }

    [JsonPropertyName("environment")] 
    public required string Environment { get; init; }

    [JsonPropertyName("data_type")] 
    public required string DataType { get; init; }

    [JsonPropertyName("import_date")] 
    public required string ImportDate { get; set; }

    public override string ToString()
    {
        return $"{Country} {Environment} {DataType} {ImportDate}";
    }
}