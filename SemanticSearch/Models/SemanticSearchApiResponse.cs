namespace SemanticSearch.Models
{
    public class SemanticSearchApiResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("founded_functions")]
        public List<FoundFunction> FoundFunctions { get; set; } = new List<FoundFunction>();

        [System.Text.Json.Serialization.JsonPropertyName("answer")]
        public string? Answer { get; set; } = String.Empty;
    }

    public class FoundFunction
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("params")]
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}
