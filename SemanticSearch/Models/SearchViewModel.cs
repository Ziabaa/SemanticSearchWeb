namespace SemanticSearch.Models
{
    public class SearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public string AiResponse { get; set; } = string.Empty;
        public bool IsSearched { get; set; } = false;
        public List<ExecutedFunction> ExecutedFunctions { get; set; } = new List<ExecutedFunction>();
    }

    public class ExecutedFunction
    {
        public string Name { get; set; }
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}
